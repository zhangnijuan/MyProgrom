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
    [Api("恩维协同获取企业档案接口")]
    [Route("/web/company/{CorpNum}", HttpMethods.Get, Notes = "获取企业档案")]
    [Route("/web/companyaccountid/{AccoutID}", HttpMethods.Get, Notes = "获取企业档案")]
    [DataContract]
    public class GetCompanyRecordByIdRequest : IReturn<GetCompanyRecordByIdResponse>
    {
      //  [DataMember(Order = 1)]
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        [ApiMember(Description = "企业云Id", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string CorpNum { get; set; }
        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int AccoutID { get; set; }
        //[ApiMember(Description = "公司Id",
        //        ParameterType = "path", DataType = "long", IsRequired = true)]
        //[DataMember(Order = 4)]
        //public long Id { get; set; }
    }
    [DataContract]
    public class GetCompanyRecordByIdResponse
    {
        public GetCompanyRecordByIdResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public CompanyRecord Data { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }
    /// <summary>
    /// 返回大实体数据
    /// </summary>
    [DataContract]
    public class CompanyRecord
    {
        [DataMember(Order = 1)]
        public CompanyIntroInfo CompanyIntro { get; set; }
        [DataMember(Order = 2)]
        public CompCertification CompanyCertification { get; set; }
        [DataMember(Order = 3)]
        public ReturnCompanyInfo CompanyInfo { get; set; }
    }
    
    /// <summary>
    /// 公司认证信息
    /// </summary>
    public class CompCertification
    {
        /// <summary>
        /// 公司性质
        /// </summary>
       [DataMember(Order = 1)]
        public string CompNature { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
       [DataMember(Order = 2)]
        public string CompAddress { get; set; }
        /// <summary>
        /// 注册资金
        /// </summary>
        [DataMember(Order = 3)]
        public string Capital { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        [DataMember(Order = 4)]
        public string LegalPerson { get; set; }
        /// <summary>
        /// 登记机关
        /// </summary>
        [DataMember(Order = 5)]
        public string RegistrationAuthority { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>
        [DataMember(Order = 6)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 年检时间
        /// </summary>
        [DataMember(Order = 7)]
        public string AnnualInspection { get; set; }
        /// <summary>
        /// 营业开始期限
        /// </summary>
        [DataMember(Order = 8)]

        public DateTime BusinessStart { get; set; }
        /// <summary>
        /// 营业结束期限
        /// </summary>
       [DataMember(Order = 9)]
        public DateTime BusinessEnd { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [DataMember(Order = 10)]
        public string BusinessScope { get; set; }
        /// <summary>
        /// 营业执照编号
        /// </summary>
        [DataMember(Order = 11)]
        public string License { get; set; }
        /// <summary>
        /// 营业执照照片资源
        /// </summary>
        [DataMember(Order = 12)]
        public ReturnPicResources LicenseResources { get; set; }
        /// <summary>
        /// 税务登记证编号
        /// </summary>
        [DataMember(Order = 13)]
        public string Tax { get; set; }
        /// <summary>
        /// 税务登记证照片资源
        /// </summary>
        [DataMember(Order = 14)]
        public ReturnPicResources TaxResources { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
       [DataMember(Order = 15)]
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 税务登记证照片资源
        /// </summary>
       [DataMember(Order = 16)]
       public ReturnPicResources OrganizationResources { get; set; }
        /// <summary>
        /// 开户许可证资源
        /// </summary>
       [DataMember(Order = 17)]
       public ReturnPicResources OpensResources { get; set; }
        /// <summary>
        /// 审核人Id
        /// </summary>
        [DataMember(Order = 18)]
        public long Reviewer { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
       [DataMember(Order = 19)]
        public string ReviewerName { get; set; }
        /// <summary>
        /// 审核人编码
        /// </summary>
       [DataMember(Order = 20)]
        public string ReviewerCode { get; set; }
        /// <summary>
        /// 审核单位
        /// </summary>
        [DataMember(Order = 21)]
        public string ReviewerComp { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [DataMember(Order = 22)]
        public DateTime ReviewerDate { get; set; }
    }

}
