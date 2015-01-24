using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同注册时校验会员登录名唯一接口")]
    [Route("/check/userName/{UserName}/{Provide}/{Client}", HttpMethods.Get, Notes = "校验登录名唯一请求url")]
    [DataContract]
    public class UserNameCheckRequest : IReturn<UserNameCheckResponse>
    {
        [ApiMember(Description = "用户名",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "客户端类型 Web = 浏览器 IOS= IOS客户端 Android = Android客户端",
          ParameterType = "json", DataType = "ClientType", IsRequired = true)]
        public ClientType Client { get; set; }

    }

    public class UserNameCheckResponse
    {
        public UserNameCheckResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public string AppKey { get; set; }

        [DataMember(Order = 4)]
        public string SecretKey { get; set; }

        [DataMember(Order = 5)]
        public bool IsExist { get; set; }
    }
}
