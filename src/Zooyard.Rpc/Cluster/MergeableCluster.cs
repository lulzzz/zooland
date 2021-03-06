﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zooyard.Core;
using Zooyard.Rpc.Merger;

namespace Zooyard.Rpc.Cluster
{
    public class MergeableCluster : AbstractCluster
    {
        public const string NAME = "mergeable";
        public IDictionary<Type, IMerger> DefaultMergers { get; set; }
        public IDictionary<string, IMerger> MySelfMergers { get; set; }
        public const string MERGER_KEY = "merger";
        private string getGroupDescFromServiceKey(string key)
        {
            int index = key.IndexOf("/");
            if (index > 0)
            {
                return $"group [ {key.Substring(0, index)} ]";
            }
            return key;
        }
        public override IClusterResult DoInvoke(IClientPool pool, ILoadBalance loadbalance, URL address, IList<URL> urls, IInvocation invocation)
        {
            var goodUrls = new List<URL>();
            var badUrls = new List<BadUrl>();
            var invokers = urls;

            string merger = address.GetMethodParameter(invocation.MethodInfo.Name, MERGER_KEY);
            if (string.IsNullOrEmpty(merger))
            {
                // If a method doesn't have a merger, only invoke one Group
                foreach (var invoker in invokers)
                {
                    try
                    {
                        var client = pool.GetClient(invoker);
                        try
                        {
                            var refer = client.Refer();
                            var invokeResult = refer.Invoke(invocation);
                            pool.Recovery(client);
                            goodUrls.Add(invoker);
                            return new ClusterResult(invokeResult, goodUrls, badUrls, null, false);
                        }
                        catch (Exception ex)
                        {
                            pool.Recovery(client);
                            throw ex;
                        }
                    }
                    catch (Exception e)
                    {
                        badUrls.Add(new BadUrl { Url = invoker, BadTime = DateTime.Now, CurrentException = e });
                        return new ClusterResult(new RpcResult(e), goodUrls, badUrls, e, true);
                    }
                }

                try
                {
                    var client2 = pool.GetClient(invokers[0]);
                    try
                    {
                        var refer2 = client2.Refer();
                        var result2 = refer2.Invoke(invocation);
                        pool.Recovery(client2);
                        goodUrls.Add(invokers[0]);
                        return new ClusterResult(result2, goodUrls, badUrls, null, false);
                    }
                    catch (Exception ex)
                    {
                        client2.Dispose();
                        throw ex;
                    }
                }
                catch (Exception e)
                {
                    badUrls.Add(new BadUrl { Url = invokers[0], BadTime = DateTime.Now, CurrentException = e });
                    return new ClusterResult(new RpcResult(e), goodUrls, badUrls, e, true);
                }
                
            }

            Type returnType;
            try
            {
                returnType = invocation.TargetType.GetMethod(invocation.MethodInfo.Name, invocation.ArgumentTypes).ReturnType;
            }
            catch (Exception)
            {
                returnType = null;
            }

            IDictionary<string, Task<IResult>> results = new Dictionary<string, Task<IResult>>();
            foreach (var invoker in invokers)
            {
                var task = Task.Run<IResult>(() => 
                {
                    try
                    {
                        var client = pool.GetClient(invoker);
                        try
                        {
                            var refer = client.Refer();
                            var invoceResult = refer.Invoke(invocation);
                            pool.Recovery(client);
                            goodUrls.Add(invoker);
                            return invoceResult;
                        }
                        catch (Exception ex)
                        {
                            pool.Recovery(client);
                            throw ex;
                        }
                    }
                    catch (Exception e)
                    {
                        badUrls.Add(new BadUrl { Url = invoker, BadTime = DateTime.Now, CurrentException = e });
                        return new RpcResult(e);
                    }
                });
                results.Add(invoker.ServiceKey, task);
            }

            object resultValue = null;

            var resultList = new List<IResult>(results.Count);

            int timeout = address.GetMethodParameter(invocation.MethodInfo.Name, TIMEOUT_KEY, DEFAULT_TIMEOUT);

            Task.WaitAll(results.Values.ToArray(), timeout);
            foreach (var entry in results)
            {
                try
                {
                    //IResult r = future.GetAwaiter().GetResult();// (timeout, TimeUnit.MILLISECONDS);
                    var r = entry.Value.Result;
                    if (r.HasException)
                    {
                        logger.Error($"Invoke {entry.Key} {getGroupDescFromServiceKey(entry.Key)}  failed: {r.Exception.Message}", r.Exception);
                    }
                    else
                    {
                        resultList.Add(r);
                    }
                }
                catch (Exception e)
                {
                    var ex = new RpcException($"Failed to invoke service {entry.Key}: {e.Message}", e);
                    return new ClusterResult(new RpcResult(ex), goodUrls, badUrls, ex, true);
                }
            }

            if (resultList.Count == 0)
            {
                return new ClusterResult(new RpcResult(), goodUrls, badUrls, null, false);
            }
            else if (resultList.Count == 1)
            {
                return new ClusterResult(resultList[0], goodUrls, badUrls, null, false);
            }

            if (returnType == typeof(void))
            {
                return new ClusterResult(new RpcResult(), goodUrls, badUrls, null, false);
            }

            if (merger.StartsWith("."))
            {
                merger = merger.Substring(1);
                MethodInfo method;
                try
                {
                    method = returnType.GetMethod(merger, new[] { returnType });
                }
                catch (Exception e)
                {
                    var ex = new RpcException($"Can not merge result because missing method [{merger}] in class [{returnType.Name}]{e.Message}", e);
                    return new ClusterResult(new RpcResult(ex), goodUrls, badUrls, ex, true);
                }

                if (method==null)
                {
                    var ex = new RpcException($"Can not merge result because missing method [ {merger} ] in class [ {returnType.Name} ]");
                    return new ClusterResult(new RpcResult(ex), goodUrls, badUrls, ex, true);
                }

                //if (!method.IsPublic)
                //{
                //method.setAccessible(true);
                //}
                resultValue = resultList[0].Value;
                resultList.RemoveAt(0);
                try
                {
                    if (method.ReturnType != typeof(void)
                            && method.ReturnType.IsAssignableFrom(resultValue.GetType()))
                    {
                        foreach (var r in resultList)
                        {
                            resultValue = method.Invoke(resultValue, new[] { r.Value });
                        }
                    }
                    else
                    {
                        foreach (var r in resultList)
                        {
                            method.Invoke(resultValue, new[] { r.Value });
                        }
                    }
                }
                catch (Exception e)
                {
                    var ex = new RpcException($"Can not merge result: {e.Message}", e);
                    return new ClusterResult(new RpcResult(ex), goodUrls, badUrls, ex, true);
                }
            }
            else
            {
                IMerger resultMerger;
                if (merger.ToLower() == "true" || merger.ToLower() == "default")
                {
                    //resultMerger = DefaultMergers[returnType];
                    resultMerger = MergerFactory.GetMerger(returnType, DefaultMergers);
                }
                else
                {
                    resultMerger = MySelfMergers[merger];
                    //resultMerger = ExtensionLoader.getExtensionLoader(Merger.class).getExtension(merger);
                }

                if (resultMerger != null)
                {
                    var rets = new List<object>(resultList.Count);
                    foreach (var r in resultList)
                    {
                        rets.Add(r.Value);
                    }
                    resultValue = resultMerger.GetType().GetMethod("Merge", new Type[] { returnType }).Invoke(resultMerger, new[] { rets });
                }
                else
                {
                    var ex = new RpcException("There is no merger to merge result.");
                    return new ClusterResult(new RpcResult(ex), goodUrls, badUrls, ex, true);
                }
            }
            return new ClusterResult(new RpcResult(resultValue), goodUrls, badUrls, null, false);
        }
    }
}
