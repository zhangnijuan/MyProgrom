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

    [Api("恩维协同验证公司名称是否已经存在接口")]
    [Route("/{AppKey}/{Secretkey}/companyInfo/checkName", HttpMethods.Post, Notes = "验证公司名称是否已经存在")]
    [DataContract]
    public  class CheckCompNameRequest:IReturn<UserNameCheckResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
       ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
       ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
        [DataMember(Order = 3)]
        [ApiMember(Description = "公司名称",
       ParameterType = "josn", DataType = "string", IsRequired = true)]
        public string CompName { get;set;}
    }
    public class CheckCompNameResponse
    {
        public CheckCompNameResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public ReturnCompanyInfo Data { get; set; }
        [DataMember(Order = 2)]
        public bool Success { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
