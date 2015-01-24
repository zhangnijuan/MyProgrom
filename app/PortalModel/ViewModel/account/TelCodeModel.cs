using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取手机验证码接口")]
    [Route("/telCode/json/syncreply/TelCodeRequest", HttpMethods.Post, Notes = "获取验证码请求url")]
    [Route("/telRegist", HttpMethods.Get, Notes = "激活短信服务器")]
    [DataContract]
    public class TelCodeRequest : IReturn<TelCodeResponse>
    {
        [ApiMember(Description = "手机号码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string TelNum { get; set; }

        [ApiMember(Description = "登录名",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string UserName { get; set; }

        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        [DataMember(Order = 3)]
        public AuthProvide Provide { get; set; }

        [ApiMember(Description = "客户端类型 Web = 浏览器 IOS= IOS客户端 Android = Android客户端",
          ParameterType = "json", DataType = "ClientType", IsRequired = true)]
        [DataMember(Order = 4)]
        public ClientType Client { get; set; }

        [ApiMember(Description = "标识",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string Flag { get; set; }
    }

    public class TelCodeResponse
    {
        public TelCodeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "手机验证码", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string TelCode { get; set; }

        [ApiMember(Description = "手机号", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string TelNumber { get; set; }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public bool Success { get; set; }




    }
}
