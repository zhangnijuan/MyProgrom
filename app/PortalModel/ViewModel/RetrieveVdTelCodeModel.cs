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
    [Api("验证手机验证码接口")]
    [Route("/vdtelcode/{UserName}/{TelCode}", HttpMethods.Get, Notes = "验证手机验证码接口url")]
    [DataContract]
    public class ValidateTelCodeRequest : IReturn<ValidateTelCodeRespones>
    {
        [ApiMember(Description = "会员姓名",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

        [ApiMember(Description = "手机验证码",
       ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string TelCode { get; set; }

    }
    public class ValidateTelCodeRespones
    {
        public ValidateTelCodeRespones()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        [ApiMember(Description = "用户手机",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Phone { get; set; }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }
}
