using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("找回密码验证用户名接口")]
    [Route("/validate/{UserName}/{WebCode}", HttpMethods.Get, Notes = "找回密码验证用户名请求url")]
    [DataContract]
    public class ValidateUserMobileRequest : IReturn<ValidateUserMobileResponse>
    {
        [ApiMember(Description = "会员登录名",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [ApiMember(Description = "网页验证码",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string WebCode { get; set; }
    }
    public class ValidateUserMobileResponse
    {
        public ValidateUserMobileResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Sucess { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "验证返回值",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string RetrunValue { get; set; }
    }
}
