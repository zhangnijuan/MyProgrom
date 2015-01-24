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
    [Api("验证用户名和网页验证码接口")]
    [Route("/vdwebcode/{UserName}/{WebCode}", HttpMethods.Get, Notes = "验证用户名和网页验证码接口url")]
    [DataContract]
    public class ValidateWebCodeRequest : IReturn<ValidateWebCodeResponse>
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
    public class ValidateWebCodeResponse
    {
        public ValidateWebCodeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "用户手机",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Phone { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "用户验证码返回状态",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public bool CodeStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
