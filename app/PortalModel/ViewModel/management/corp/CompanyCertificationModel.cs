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
    [Api("恩维协同公司认证接口")]
    [Route("/{AppKey}/{Secretkey}/company/certification/{AccountID}", HttpMethods.Get, Notes = "公司认证请求Url")]
    [Route("/{AppKey}/{Secretkey}/company/certification/new", HttpMethods.Post, Notes = "公司认证请求Url")]
    [DataContract]
    public class CompanyCertificationRequest : IReturn<CompanyCertificationResponse>
    {
        [DataMember(Order=1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order=2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
       
        [ApiMember(Description = "公司Id", ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long CompId { get; set; }
      
        [ApiMember(Description = "公司性质", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CompNature { get; set; }
       
        [ApiMember(Description = "公司地址", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string CompAddress { get; set; }
   
        [ApiMember(Description = "注册资金", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string Capital { get; set; }
      
        [ApiMember(Description = "法人代表", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string LegalPerson { get; set; }
        
        [ApiMember(Description = "登记机关", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string RegistrationAuthority { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>
        [ApiMember(Description = "成立时间", ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 10)]
        public DateTime CreateTime { get; set; }
        
        [ApiMember(Description = "年检时间", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string AnnualInspection { get; set; }

        [ApiMember(Description = "营业开始期限", ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 12)]
        public DateTime BusinessStart { get; set; }
        [ApiMember(Description = "营业结束期限", ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 13)]
        public DateTime BusinessEnd { get; set; }
        [ApiMember(Description = "经营范围", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 14)]
        public string BusinessScope { get; set; }
       
        [ApiMember(Description = "营业执照编号", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 15)]
        public string License { get; set; }
        [DataMember(Order = 16)]
        [ApiMember(Description = "营业执照图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public PicResources LicenseResources { get; set; }

        /// <summary>
        /// 税务登记证编号
        /// </summary>
        [ApiMember(Description = "税务登记证编号", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 17)]
        public string Tax { get; set; }

        [DataMember(Order = 18)]
        [ApiMember(Description = "税务登记证图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public PicResources TaxResources { get; set; }
        
        [ApiMember(Description = "组织机构代码", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 19)]
        public string OrganizationCode { get; set; }

        [DataMember(Order = 20)]
        [ApiMember(Description = "组织机构代码证图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public PicResources OrganizationResources { get; set; }
        [DataMember(Order = 21)]
        [ApiMember(Description = "开户许可证图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public PicResources OpenResources { get; set; }
    }
    [DataContract]
    public class CompanyCertificationResponse
    {
        public CompanyCertificationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public CompCertification Data { get; set; }
       
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
