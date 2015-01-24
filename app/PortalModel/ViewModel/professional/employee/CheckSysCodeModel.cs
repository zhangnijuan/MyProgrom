using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel.professional.employee
{
    [Api("检验员工编码是否已经存在")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/employee/checkcode/{syscode}", HttpMethods.Get, Notes = "检验员工编码是否已经存在")]
    [DataContract]
    public class CheckSysCodeRequest : IReturn<CheckSysCodeResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "企业ID",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        [ApiMember(Description = "编码",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string SysCode { get; set; }
    }
    public class CheckSysCodeResponse
    {
        public CheckSysCodeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public ReturnList Data { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }
}
