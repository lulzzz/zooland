﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Zooyard.Core;

namespace Zooyard.Rpc.Support
{
    public abstract class AbstractClientPool : IClientPool
    {
        #region 内部成员
        /// <summary>
        /// 连接缓存池
        /// </summary>
        //protected ConcurrentQueue<IClient> clientsPool;
        protected ConcurrentDictionary<string, ConcurrentQueue<IClient>> ClientsPool;
        /// <summary>
        /// 同步连接
        /// </summary>
        protected AutoResetEvent resetEvent;

        /// <summary>
        /// 空闲连接数
        /// </summary>
        protected volatile ConcurrentDictionary<string, int> idleCount=new ConcurrentDictionary<string, int> ();

        //protected volatile int idleCount = 0;

        /// <summary>
        /// 活动连接数
        /// </summary>
        protected volatile ConcurrentDictionary<string, int> activeCount = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// 同步连接锁
        /// </summary>		
        protected object locker = new object();

        /// <summary>
        /// 释放标志
        /// </summary>
        protected bool disposed;
        /// <summary>
        /// 随机数生成器
        /// </summary>
        protected Random rand = new Random();
        #endregion


        #region 属性
        public URL Address { get; set; }

        /// <summary>
        /// 启动版本，连接池重启时版本递增
        /// </summary>
        //internal int Version { set; get; }

        /// <summary>
        /// 连接池最大活动连接数
        /// </summary>
        public int MaxActive { protected set; get; }
        /// <summary>
        /// 连接池最小空闲连接数
        /// </summary>
        public int MinIdle { protected set; get; }
        /// <summary>
        /// 连接池最大空闲连接数
        /// </summary>
        public int MaxIdle { protected set; get; }
        /// <summary>
        /// 通信超时时间，单位毫秒
        /// </summary>
        public int ClientTimeout { protected set; get; }

        /// <summary>
        /// 空闲连接数
        /// </summary>
        public IDictionary<string, int> IdleCount
        {
            get
            {
                return idleCount;
            }
        }

        /// <summary>
        /// 活动连接数
        /// </summary>
        public IDictionary<string, int> ActiveCount
        {
            get
            {
                return activeCount;
            }
        }
        #endregion

        #region 构造方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="urls">服务器地址及端口</param>
        protected AbstractClientPool()
        {
            //参数校验及赋值
            //if (urls == null)
            //{
            //    throw new ArgumentNullException("urls");
            //}
            //this.Version = 0;

            //初始化
            CreateResetEvent();
            CreatePool();
        }


        #endregion

        #region 公有操作方法

        /// <summary>
        /// 从连接池取出一个连接
        /// </summary>
        /// <returns>连接</returns>
        public virtual IClient GetClient(URL url)
        {
            if (Monitor.TryEnter(locker, TimeSpan.FromMilliseconds(ClientTimeout)))
            {
                try
                {
                    IClient client = null;
                    Exception innerErr = null;
                    var validClient = false;
                    //连接池无空闲连接	
                    
                    if (idleCount.ContainsKey(url.ToString()) && idleCount[url.ToString()] > 0 && !validClient)
                    {
                        client = DequeueClient(url.ToString());
                        validClient = ValidateClient(client, out innerErr);
                        if (!validClient)
                        {
                            DestoryClient(client);
                        }
                    }

                    //连接池无空闲连接	
                    if (!validClient)
                    {
                        //连接池已已创建连接数达上限				
                        if (idleCount.ContainsKey(url.ToString()) && activeCount[url.ToString()] > MaxActive)
                        {
                            if (!resetEvent.WaitOne(ClientTimeout))
                            {
                                throw new TimeoutException("the pool is busy,no available connections.");
                            }
                        }
                        else
                        {
                            client = InitializeClient(url, out innerErr);
                            if (client == null)
                            {
                                throw new InvalidOperationException("connection access failed. please confirm call service status.", innerErr);
                            }
                        }
                    }

                    //空闲连接数小于最小空闲数，添加一个连接到连接池（已创建数不能超标）			
                    //if ((!idleCount.ContainsKey(url.ToString()) || idleCount[url.ToString()] < MinIdle) 
                    //    && (!activeCount.ContainsKey(url.ToString()) || activeCount[url.ToString()] < MaxActive))
                    //{
                    //    var candiate = InitializeClient(url, out innerErr);
                    //    if (candiate != null)
                    //    {
                    //        EnqueueClient(url.ToString(), candiate);
                    //    }
                    //}

                    return client;
                }
                finally
                {
                    Monitor.Exit(locker);
                }
            }
            else
            {
                throw new TimeoutException($"gets the connection wait more than {ClientTimeout} milliseconds.");
            }
        }

        /// <summary>
        /// 归还一个连接至连接池
        /// </summary>
        /// <param name="client">连接</param>
        public virtual void Recovery(IClient client)
        {
            lock (locker)
            {
                //空闲连接数达到上限或者连接版本过期，不再返回线程池,直接销毁			
                if ((idleCount.ContainsKey(client.Url.ToString())
                    && idleCount[client.Url.ToString()] >= MaxIdle))//|| this.Version != client.Version
                {
                    DestoryClient(client);
                    Console.WriteLine($"recovery to destory idle overflow:{client.Version}:{client.Url.ToString()}");
                }
                else
                {
                    //更新最近触发时间
                    client.ActiveTime = DateTime.Now;
                    //连接回归连接池
                    EnqueueClient(client.Url.ToString(), client);
                    //发通知信号，连接池有连接变动
                    resetEvent.Set();
                    Console.WriteLine($"recovery to update:{client.Version}:{client.Url.ToString()}");
                }
            }
        }

        /// <summary>
        /// 重置连接池
        /// </summary>
        public virtual void ResetPool()
        {
            CreatePool();
        }

        /// <summary>
        /// 释放连接池
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region 私有方法

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    lock (locker)
                    {
                        //Version++;
                        //foreach (var item in this.Urls)
                        //{
                        //    while (idleCount.ContainsKey(item) && idleCount[item] > 0)
                        //    {
                        //        DequeueClient(item).Dispose();
                        //    }
                        //}
                        foreach (var item in ClientsPool.Keys)
                        {
                            while (idleCount[item] > 0)
                            {
                                var client = DequeueClient(item);
                                DestoryClient(client);
                            }
                        }
                    }
                }
                disposed = true;
            }
        }


        /// <summary>
        /// 创建线程同步对象
        /// </summary>
        protected virtual void CreateResetEvent()
        {
            lock (locker)
            {
                if (resetEvent == null)
                {
                    resetEvent = new AutoResetEvent(false);
                }
            }
        }

        /// <summary>
        /// 创建连接池
        /// </summary>

        protected virtual void CreatePool()
        {
            lock (locker)
            {
                //版本增加
                //Version++;

                //读取配置
                MaxActive = 100;
                MinIdle = 2;
                MaxIdle = 10;
                ClientTimeout = 5000;

                if (ClientsPool == null)
                {
                    ClientsPool = new ConcurrentDictionary<string,ConcurrentQueue<IClient>>();
                }
                //else
                //{
                    //foreach (var url in this.Urls)
                    //{
                    //    DestoryClient(DequeueClient(url));
                    //}

                    //while (idleCount > 0)
                    //{
                    //    DestoryClient(DequeueClient());
                    //}

                   // ClientsPool = new ConcurrentDictionary<string, ConcurrentQueue<IClient>>();
                //}

                //结点列表初始化
                //this.Urls = urls;
            }
        }


        /// <summary>
        /// 连接进入连接池
        /// </summary>
        /// <param name="client">连接</param>
        protected void EnqueueClient(string url,IClient client)
        {
            if (!ClientsPool.ContainsKey(url))
            {
                ClientsPool.TryAdd(url, new ConcurrentQueue<IClient>());
            }
            if (!idleCount.ContainsKey(url))
            {
                idleCount.TryAdd(url, 0);
            }
            ClientsPool[url].Enqueue(client);
            //clientsPool.Enqueue(client);
            idleCount[url]++;
        }

        /// <summary>
        /// 连接取出连接池
        /// </summary>
        /// <returns>连接</returns>
        protected IClient DequeueClient(string url)
        {
            IClient client;
            if (ClientsPool[url].TryDequeue(out client))
            {
                idleCount[url]--;
            }
            return client;
        }

        /// <summary>
        /// 创建一个连接，虚函数，应由特定连接池继承
        /// </summary>
        /// <returns>连接</returns>
        protected abstract IClient CreateClient(URL url);

        /// <summary>
        /// 初始化连接，隐藏创建细节
        /// </summary>
        /// <returns>连接</returns>
        protected IClient InitializeClient(URL url,out Exception err)
        {
            err = null;
            //if (Urls==null||Urls.Count == 0|| !Urls.Contains(url))
            //{
            //    err = new NullReferenceException("there is no avalible serivce node to be access!");
            //    return null;
            //}
            try
            {
                var client = CreateClient(url);
                if (ValidateClient(client, out err))
                {
                    if (!activeCount.ContainsKey(url.ToString()))
                    {
                        activeCount.TryAdd(url.ToString(), 0);
                    }
                    activeCount[url.ToString()]++;
                    client.Reset();
                    return client;
                }
            }
            catch (Exception e)
            {
                err = e;
            }

            //var hostIndexs = new int[Urls.Count];
            //int j = 0;
            //for (var i = 0; i < Urls.Count; i++)
            //{
            //    hostIndexs[i] = i;
            //}
            //for (var i = 0; i < Urls.Count; i++)
            //{
            //    try
            //    {
            //        j = rand.Next(Urls.Count);
            //        var client = CreateClient(Urls[hostIndexs[j]]);
            //        if (ValidateClient(client, out err))
            //        {
            //            activeCount++;
            //            client.Reset();
            //            return client;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        hostIndexs[j] = hostIndexs[(j + 1) % Urls.Count];
            //        err = e;
            //        continue;
            //    }
            //}
            return null;
        }

        /// <summary>
        /// 校验连接，确保连接开启
        /// </summary>
        /// <param name="client">连接</param>

        protected bool ValidateClient(IClient client, out Exception err)
        {
            try
            {
                client.Open();
                err = null;
                return true;
            }
            catch (Exception e)
            {
                err = e;
                return false;
            }
        }

        /// <summary>
        /// 销毁连接
        /// </summary>
        /// <param name="client">连接</param>
        protected void DestoryClient(IClient client)
        {
            if (client != null)
            {
                client.Close();
                client.Dispose();
                activeCount[client.Url.ToString()]--;
            }
        }
        /// <summary>
        /// 超时清除
        /// </summary>
        /// <param name="overTime"></param>
        public void TimeOver(DateTime overTime)
        {
            foreach (var item in ClientsPool.Keys)
            {
                var list = new List<IClient>();
                while (idleCount[item]>0)
                {
                    // Console.WriteLine($"client idleCount:{idleCount[item]}");
                    var client = DequeueClient(item);
                    if (client==null)
                    {
                        continue;
                    }
                    if (client.ActiveTime<=DateTime.MinValue || client.ActiveTime < overTime)
                    {
                        Console.WriteLine($"client time over:{client.Version}:{client.Url.ToString()}");
                        DestoryClient(client);
                    }
                    else
                    {
                        list.Add(client);
                    }
                }
                foreach (var client in list)
                {
                    EnqueueClient(item, client);
                }
            }

            PrintConsole();
        }

        #endregion

        private void PrintConsole()
        {
            //Console.WriteLine($"client pool information:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff")}");
            //Console.WriteLine("-----------------------------------------");
            //Console.WriteLine("client pools");
            //foreach (var pool in ClientsPool)
            //{
            //    Console.WriteLine($"{pool.Value?.Count()}:{pool.Key}");
            //}
            //Console.WriteLine("idleCount");
            //foreach (var item in idleCount)
            //{
            //    Console.WriteLine($"{item.Value}:{item.Key}");
            //}
            //Console.WriteLine("activeCount");
            //foreach (var item in activeCount)
            //{
            //    Console.WriteLine($"{item.Value}:{item.Key}");
            //}
            //Console.WriteLine("-----------------------------------------");
        }
    }
}
