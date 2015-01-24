using Ndtech.FileUpload;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// HostServer 的摘要说明
/// </summary>
public class HostServer : AppHostBase
{
        private static ILog log;
        public HostServer()
            : base("Ndtech Interface", typeof(FileUpLoadService).Assembly)
        {
            LogManager.LogFactory = new NLogFactory();
            log = LogManager.GetLogger(typeof(HostServer));

            EndpointHost.Config.WriteErrorsToResponse = true;
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Origin", "*");
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Headers", "Content-Type");

            //EndpointHost.Config.AssertContentType("application/json");
            EndpointHost.Config.DefaultContentType = "application/json";
            EndpointHost.Config.AllowJsonpRequests = true;
            EndpointHost.Config.DebugMode = false;
        }

        public override void Configure(Funq.Container container)
        {
            container.Register<IResourceManager>(new ConfigurationResourceManager());
            
        }
}