﻿using System;
using System.Collections.Generic;
using System.Linq;
using Zooyard.Core;

namespace Zooyard.Rpc.Cluster
{
    public class FailoverCluster : AbstractCluster
    {
        public const string NAME = "failover";
        public const string RETRIES_KEY = "retries";
        public const int DEFAULT_RETRIES = 2;
        
        public override IClusterResult DoInvoke(IClientPool pool, ILoadBalance loadbalance, URL address, IList<URL> urls, IInvocation invocation)
        {
            var goodUrls = new List<URL>();
            var badUrls = new List<BadUrl>();
            //IResult result = null;

            var copyinvokers = urls;
            checkInvokers(copyinvokers, invocation);

            //*getUrl();
            int len = address.GetMethodParameter(invocation.MethodInfo.Name, RETRIES_KEY, DEFAULT_RETRIES) + 1;
            if (len <= 0)
            {
                len = 1;
            }
            // retry loop.
            RpcException le = null; // last exception.
            var invoked = new List<URL>(copyinvokers.Count); // invoked invokers.
            ISet<string> providers = new HashSet<string>();//*len
            for (int i = 0; i < len; i++)
            {
                //重试时，进行重新选择，避免重试时invoker列表已发生变化.
                //注意：如果列表发生了变化，那么invoked判断会失效，因为invoker示例已经改变
                if (i > 0)
                {
                    //*checkWhetherDestroyed();
                    //*copyinvokers = list(invocation);
                    //重新检查一下
                    checkInvokers(copyinvokers, invocation);
                }
                var invoker = base.select(loadbalance, invocation, copyinvokers, invoked);
                invoked.Add(invoker);
                RpcContext.GetContext().SetInvokers(invoked);
                try
                {
                    var client = pool.GetClient(invoker);
                    try
                    {
                        var refer = client.Refer();
                        var result = refer.Invoke(invocation);
                        pool.Recovery(client);
                        if (le != null && logger.IsWarnEnabled)
                        {
                            logger.Warn("Although retry the method " + invocation.MethodInfo.Name
                                    + " in the service " + invocation.TargetType.FullName
                                    + " was successful by the provider " + invoker.Address
                                    + ", but there have been failed providers " + string.Join(",", providers)
                                    + " (" + providers.Count + "/" + copyinvokers.Count
                                    + ") from the registry " + address.Address
                                    //+ " on the consumer " + NetUtils.GetLocalHost()
                                    //+ " using the dubbo version " + Version.getVersion() + ". Last error is: "
                                    + le.Message, le);
                        }
                        goodUrls.Add(invoker);
                        return new ClusterResult(result,goodUrls,badUrls,le,false);
                    }
                    catch (Exception ex)
                    {
                        pool.Recovery(client);
                        throw ex;
                    }
                //}
                //catch (RpcException e)
                //{
                //    if (e.Biz)
                //    { // biz exception.
                //        throw e;
                //    }
                //    le = e;
                }
                catch (Exception e)
                {
                    le = new RpcException(e.Message, e);

                    var badUrl = badUrls.FirstOrDefault(w => w.Url == invoker);
                    if (badUrl != null)
                    {
                        badUrls.Remove(badUrl);
                    }
                    badUrls.Add(new BadUrl { Url = invoker, BadTime = DateTime.Now, CurrentException = le });
                    
                }
                finally
                {
                    providers.Add(invoker.Address);
                }
            }

             var re = new RpcException(le != null ? le.Code : 0, "Failed to invoke the method "
                   + invocation.MethodInfo.Name + " in the service " + invocation.TargetType.FullName
                   + ". Tried " + len + " times of the providers " + string.Join(",", providers)
                   + " (" + providers.Count + "/" + copyinvokers.Count
                   //+ ") from the registry " + directory.getUrl().getAddress()
                   //+ " on the consumer " + NetUtils.getLocalHost() + " using the dubbo version "
                   //+ Version.getVersion() + ". Last error is: "
                   + (le != null ? le.Message : ""), le != null && le.InnerException != null ? le.InnerException : le);

            return new ClusterResult(new RpcResult(re), goodUrls, badUrls, re, true);
           
        }
    }

    
}
