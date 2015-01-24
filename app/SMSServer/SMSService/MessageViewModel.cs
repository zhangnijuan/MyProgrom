using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.SMSService
{
    [Api("恩维协同获取手机验证码接口")]
    [Route("/telCode", HttpMethods.Post, Notes = "获取验证码请求url")]
    [Route("/telRegist", HttpMethods.Get, Notes = "激活短信服务器")]
    [DataContract]
    public class MessageViewModel : IReturn<MessageResponse>
    {
        [ApiMember(Description = "手机号码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string TelNum { get; set; }

        [ApiMember(Description = "登录名",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string UserName { get; set; }
        [ApiMember(Description = "标识",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string Flag { get; set; }
    }


      [DataContract]
  public class MessageResponse
  {
      public MessageResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
      //[ApiMember(Description = "手机验证码", DataType = "string", IsRequired = true)]
      //[DataMember(Order = 3)]
      //public string TelCode { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order =1)]
        public bool Success { get; set; }
  }
}
