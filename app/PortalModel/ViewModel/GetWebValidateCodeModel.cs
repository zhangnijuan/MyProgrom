using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("页面随机生成数字接口")]
    [Route("/getcode",HttpMethods.Get, Notes = "页面随机生成数字接口url")]
    [DataContract]
    public class GetWebValidateCodeRequest : IReturn<GetWebValidateCodeResponse>
    {
        [ApiMember(Description = "会员登录名",
             ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }
    }
    public class GetWebValidateCodeResponse
    {
        public GetWebValidateCodeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        [ApiMember(Description = "页面验证码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public Stream WebValidateCode { get; set; }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }
}
