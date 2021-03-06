﻿using Akka.Actor;
using Akka.Configuration;
using Zooyard.Core;
using Zooyard.Rpc.Support;

namespace Zooyard.Rpc.AkkaImpl
{
    public class AkkaClientPool : AbstractClientPool
    {
        public const string TIMEOUT_KEY = "akka_timeout";
        public const int DEFAULT_TIMEOUT = 5000;

        public string TheActorConfig { get; set; } = @"akka {  
                actor {
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                }
                remote {
                    helios.tcp {
                        transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                        applied-adapters = []
                        transport-protocol = tcp
                        port = 0
                        hostname = localhost
                    }
                }
            }";

        protected override IClient CreateClient(URL url)
        {
            //获得transport参数,用于反射实例化
            var timeout = url.GetParameter(TIMEOUT_KEY, DEFAULT_TIMEOUT);
            
            var config = ConfigurationFactory.ParseString(TheActorConfig);

            var system = ActorSystem.Create($"client-{url.ServiceInterface.Replace(".", "-")}", config);
            
            return new AkkaClient(system, url, timeout);
        }
    }
}
