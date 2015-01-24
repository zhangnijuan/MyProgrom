using Ndtech.PortalModel.ViewModel;
using Ndtech.SMSService.SMS;
using ServiceStack.Configuration;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.Logging;
namespace Ndtech.SMSService
{
    public class SendMessageService : Service, IPost<MessageViewModel>, IGet<MessageViewModel>
    {
        private readonly string sendUrl = "http://sdkhttp.eucp.b2m.cn/sdkproxy/sendsms.action";
        private readonly string registUrl = "http://sdkhttp.eucp.b2m.cn/sdkproxy/regist.action?cdkey=3SDK-EMY-0130-JIVRN&password=966365";
        ILog log = LogManager.GetLogger(typeof(SendMessageService));
        public object Post(MessageViewModel request)
        {
            var appSettings =  this.GetAppHost().GetContainer().Resolve<IResourceManager>();
            string url = appSettings.GetString("smsurl");
            if (!string.IsNullOrEmpty(url))
            {
                log.Debug(url);
                var client = new JsonServiceClient(url);
                client.HttpMethod = "POST";
                
                TelCodeRequest req = new TelCodeRequest();
                req.TelNum = request.TelNum;
                req.Provide = AuthProvide.User;
                req.Client = ClientType.Web;
                req.UserName = request.UserName;
                req.Flag = request.Flag;
                
               var response= client.Send<TelCodeResponse>(req);
               if (response.Success)
               {
                   string tel = request.TelNum;
                   string message = string.Format("cdkey=3SDK-EMY-0130-JFTQT&password=pickbranch&phone={0}&message={1}", response.TelNumber, string.Format("【品美派】{0}", response.TelCode));
                   try
                   {
                       log.Debug(message);
                       string result = HttpUtility.GetData(string.Format("{0}?{1}", sendUrl, message));
                       //string result = HttpUtility.SendPostHttpRequest(sendUrl, "POST", message);
                       log.Debug(result);
                   }
                   catch (Exception ex)
                   {
                       log.Error(ex);
                   }

                   return new MessageResponse() { Success = true };
               }
               else

               {
                   return new MessageResponse() { Success = false, ResponseStatus = response.ResponseStatus };
               }
            
            }
            else
            {
                return HttpUtility.GetData("http://www.baidu.com");
            }
        }

        public object Get(MessageViewModel request)
        {
            if (request.UserName == "ndtech")
            {
                return HttpUtility.GetData(registUrl);
            }
            else
            {
                return HttpUtility.GetData("http://www.baidu.com");
            }
        }
    }
}
