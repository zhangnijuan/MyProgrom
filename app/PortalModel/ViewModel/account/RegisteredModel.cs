using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同注册会员接口")]
    [Route("/register",HttpMethods.Post, Notes = "注册会员请求url")]
    [DataContract]
    public class RegisteredRequest : IReturn<RegisteredResponse>
    {
        [ApiMember(Description = "会员登录名",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [ApiMember(Description = "密码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Password { get; set; }

        [ApiMember(Description = "确认密码",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string ConfirmPassword { get; set; }

        [ApiMember(Description = "公司名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string CompName { get; set; }

        [ApiMember(Description = "邮箱",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string EmailInfo { get; set; }

        [ApiMember(Description = "手机号码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string TelNum { get; set; }

        [ApiMember(Description = "手机验证码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string TelCode { get; set; }

        [DataMember(Order = 8)]
        [ApiMember(Description = "注册类型 User = 普通用户;App = 第三方接口",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        [DataMember(Order = 9)]
        [ApiMember(Description = "客户端类型 Web = 浏览器 IOS= IOS客户端 Android = Android客户端",
          ParameterType = "json", DataType = "ClientType", IsRequired = true)]
        public ClientType Client { get; set; }
    }
    public class RegisteredResponse
    {
        public RegisteredResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public UserInfo Data { get; set; }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
