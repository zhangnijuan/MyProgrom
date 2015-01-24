
using Ndtech.FileUpload;
using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.Auth;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.Feature;
using Ndtech.PortalService.Filter;
using Ndtech.PortalService.SMS;
using Ndtech.PortalService.SystemService;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.DesignPatterns.Serialization;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.Logging.NLogger;
using ServiceStack.Api.Swagger;
using ServiceStack.Plugins.ProtoBuf;
using ServiceStack.Common.Web;


namespace Ndtech.PortalServer
{
    public class ProtalHostServer : AppHostBase
    {
        private static ILog log;
        public ProtalHostServer()
            : base("Ndtech Interface", typeof(RegisteredService).Assembly, typeof(FileUpLoadService).Assembly)
        {
            LogManager.LogFactory = new NLogFactory();
            log = LogManager.GetLogger(typeof(ProtalHostServer));

            EndpointHost.Config.WriteErrorsToResponse = true;
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Origin", "*");
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            EndpointHost.Config.GlobalResponseHeaders.Add("Access-Control-Allow-Headers", "Content-Type");

            //EndpointHost.Config.AssertContentType("application/json");
            EndpointHost.Config.DefaultContentType = "application/json";
            EndpointHost.Config.AllowJsonpRequests = true;
            EndpointHost.Config.DebugMode = false;
            //EndpointHost.Config.OnlySendSessionCookiesSecurely = true;
          
            //EndpointHost.Config.RestrictAllCookiesToDomain = "api";
            log.Info("info start...");
            
            log.Error("error start...");
        }
        public override void Configure(Funq.Container container)
        {

            container.Register<IResourceManager>(new ConfigurationResourceManager());
            var appSettings = container.Resolve<IResourceManager>();
            //EndpointHost.Config.WebHostUrl = appSettings.GetString("WebHostUrl");
            //if (!string.IsNullOrEmpty(appSettings.GetString("smsurl")))
            //{
            //    container.Register<ISendMessage>(c =>
            //                 new SendMessageImpl(appSettings.GetString("smsurl")));
            //}
            PostgreSQLDialectProvider provider = PostgreSQLDialectProvider.Instance;
            provider.UseUnicode = true;
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(appSettings.GetString("ConnectionString"),true, provider));

            //container.Register<ICacheClient>(new MemoryCacheClient());


            
    //        this.ContentTypeFilters.Register(ContentType.ProtoBuf,
    //(reqCtx, res, stream) => ProtoBuf.Serializer.NonGeneric.Serialize(stream, res),
    //ProtoBuf.Serializer.NonGeneric.Deserialize);
            #region 添加插件



            //Plugins.Add(new RequestLogsFeature()
            //{
            //    EnableErrorTracking = true,
            //    EnableResponseTracking = true,
            //    EnableSessionTracking = true,
            //    EnableRequestBodyTracking = true,
            //    RequiredRoles = new string[0]
            //});
            //Plugins.Add(new SwaggerFeature());
            //Plugins.Add(new SessionFeature());
            //Plugins.Add(new DataTableManagerFeature());
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new RcordIDFeature());
            Plugins.Add(new RepositoryFeature());
            container.RegisterValidators(typeof(LoginValidator).Assembly);
            Plugins.Add(new NdtechAuthFeature(
               () => new NdtechAuthUserSession(), new[] { new UserNamePasswordAuthProvider() }
           ));

            //Plugins.Add(new ProtoBufFormat());
            #endregion

            //container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("10.211.55.2:6379"));
            //container.Register<ICacheClient>(c => c.Resolve<IRedisClientsManager>().GetCacheClient());



            #region 过滤器
            
            
            this.RequestFilters.Add((req, res, dto) =>
            {
                #region appkey过滤
                
              
                //PropertyInfo appKeyInfo = dto.GetType().GetProperty("AppKey");
                //if (null != appKeyInfo)
                //{
                //    object key = appKeyInfo.GetGetMethod().Invoke(dto, null);
                //    string appKey = key == null ? string.Empty : key.ToString();
                //    PropertyInfo secretKeyInfo = dto.GetType().GetProperty("Secretkey");
                //    if (null != secretKeyInfo && !string.IsNullOrEmpty(appKey))
                //    {
                //        string sercretKey = secretKeyInfo.GetGetMethod().Invoke(dto, null).ToString();
                //        PropertyInfo accountIDInfo = dto.GetType().GetProperty("AccountID");
                //        if (null != accountIDInfo)
                //        {
                //            int a = -1;
                //            int.TryParse(accountIDInfo.GetGetMethod().Invoke(dto, null).ToString(), out a);
                //            IDbConnectionFactory dbFactroy = this.Container.Resolve<IDbConnectionFactory>();
                //            using (var conn = dbFactroy.OpenDbConnection())
                //            {
                //                try
                //                {
                //                    List<AppData> appDataList = conn.Where<AppData>(x => x.AppKey == appKey && x.A == a);
                //                    if (null == appDataList || appDataList.Count == 0)
                //                    {
                //                        ResponseStatus ResponseStatus = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus() { ErrorCode = "appKeyInvalid", Message = "appKey无效,请重新登录" };
                //                        res.ContentType = "application/json";

                //                        res.WriteToResponse(req, new { Success = false, ResponseStatus = ResponseStatus });
                //                        res.Close();
                //                    }
                //                }
                //                finally
                //                {
                //                    conn.Close();
                //                }
                //            }

                //        }
                //    }
                //}
                #endregion

                #region sql注入过滤
                foreach (PropertyInfo info in dto.GetType().GetProperties())
                {
                    MethodInfo method = info.GetGetMethod();
                    if (method != null)
                    {
                        if (info.PropertyType == Type.GetType("System.String") || info.PropertyType == Type.GetType("System.Int64") || info.PropertyType == Type.GetType("System.Int32"))
                        {
                            object obj = method.Invoke(dto,null);
                            if (obj != null)
                            {
                                string value = obj.ToString();
                                if (!string.IsNullOrEmpty(value) && !ProcessSqlStr(value))
                                {
                                    ResponseStatus ResponseStatus = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus() { ErrorCode = "valueInvalid", Message = string.Format("{0}是非法关键字", value) };
                                    res.ContentType = "application/json";

                                    res.WriteToResponse(req, new { Success = false, ResponseStatus = ResponseStatus });
                                    res.Close();
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion


            });
            #endregion

            #region 过滤器
            this.PreRequestFilters.Insert(0, (httpReq, httpRes) =>
            {
                httpReq.UseBufferedStream = true;
                httpReq.ResponseContentType = "application/json";
               
            });
            #endregion

            #region 异常信息
            this.ExceptionHandler = (req, res, operationName, ex) =>
            {
                log.Error(req.AbsoluteUri);
                log.Error(ex);
            };
            this.ServiceExceptionHandler = (httpReq, request, exception) =>
            {
                log.Error(httpReq.AbsoluteUri);
                log.Error(exception);

                return exception;
            };
            #endregion
        }
        public override void Release(object instance)
        {
            if (instance is Service)
            {
                Service service = instance as Service;
                if (service.Db != null)
                {
                    service.Db.Close();
                    service.Db.Dispose();
                }
            }
            base.Release(instance);
        }
        private bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string SqlStr =
                    "select*|and'|or'|insertinto|deletefrom|altertable|update|createtable|createview|dropview|createindex|dropindex|createprocedure|dropprocedure|createtrigger|droptrigger|createschema|dropschema|createdomain|alterdomain|dropdomain|);|select@|declare@|print@|char(|select|'";
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0 || ss.IndexOf(Str)>=0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        //public override ServiceStack.ServiceHost.IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        //{
        //    return new ServiceRunerFilter<TRequest>(this, actionContext);
        //}



    }
}
