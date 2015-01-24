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
    [Api("找回密码根据用户名返回手机号接口")]
    [Route("/mobile/{UserName}", HttpMethods.Get, Notes = "找回密码根据用户名返回手机号请求url")]
    [DataContract]
    public class ValidateUserGetMebileRequest : IReturn<ValidateUserGetMebileRespones>
    {
        [ApiMember(Description = "会员登录名",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string UserName { get; set; }

    }
    public class ValidateUserGetMebileRespones
    {
        public ValidateUserGetMebileRespones()
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
        public bool Sucess { get; set; }
    }
}
