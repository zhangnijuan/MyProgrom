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
    [Api("恩维协同找回密码接口")]
    [Route("/find",HttpMethods.Post, Notes = "找回密码请求url")]
    [DataContract]
    public class RetrievePwdRequest : IReturn<RetrievePwdResponse>
    {
        [ApiMember(Description = "会员登录名",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [ApiMember(Description = "会员密码",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string PassWord { get; set; }

        [ApiMember(Description = "手机验证码",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string TelCode { get; set; }
    }
    public class RetrievePwdResponse
    {
        public RetrievePwdResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public bool Success { get; set; }
    }
}
