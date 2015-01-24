using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 公司认证信息
    /// </summary>
    [Alias("udoc_comp_qualification")]
   public  class CompanyCertification
    {
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int Accountid { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        [Alias("mid")]
    
        public long CompId { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>
        [Alias("nature")]
        [StringLengthAttribute(100)]
        public string CompNature { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(200)]
        public string CompAddress { get; set; }
        /// <summary>
        /// 注册资金
        /// </summary>
        [Alias("capital")]
        [StringLengthAttribute(50)]
        public string Capital { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        [Alias("legalPerson")]
        [StringLengthAttribute(50)]
        public string LegalPerson { get; set; }
        /// <summary>
        /// 登记机关
        /// </summary>
        [Alias("authority")]
        [StringLengthAttribute(100)]
        public string RegistrationAuthority { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>
        [Alias("createtime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 年检时间
        /// </summary>
        [Alias("annualinspection")]
        [StringLengthAttribute(50)]
        public string AnnualInspection { get; set; }
        /// <summary>
        /// 营业开始期限
        /// </summary>
        [Alias("businessstart")]
      
        public DateTime BusinessStart { get; set; }
        /// <summary>
        /// 营业结束期限
        /// </summary>
        [Alias("businesssend")]

        public DateTime BusinessEnd { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [Alias("scope")]
        [StringLengthAttribute(500)]
        public string BusinessScope { get; set; }
        /// <summary>
        /// 营业执照编号
        /// </summary>
        [Alias("license")]
        [StringLengthAttribute(50)]
        public string License { get; set; }
        /// <summary>
        /// 营业执照照片Id
        /// </summary>
        [Alias("licenserid")]
        public long LicenseResources { get; set; }
        /// <summary>
        /// 税务登记证编号
        /// </summary>
        [Alias("tax")]
        [StringLengthAttribute(50)]
        public string Tax { get; set; }
        /// <summary>
        /// 税务登记证照片Id
        /// </summary>
        [Alias("taxrid")]
        public long TaxResources { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        [Alias("organization")]
        [StringLengthAttribute(50)]
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 税务登记证照片Id
        /// </summary>
        [Alias("organizationrid")]
        public long OrganizationResources { get; set; }
        /// <summary>
        /// 开户许可证ID
        /// </summary>
        [Alias("opensrid")]
        public long OpensResources { get; set; }
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Alias("reviewerid")]
        public long Reviewer { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        [Alias("reviewer_name")]
        [StringLengthAttribute(50)]
        public string ReviewerName { get; set; }
        /// <summary>
        /// 审核人编码
        /// </summary>
        [Alias("reviewer_code")]
        [StringLengthAttribute(50)]
        public string ReviewerCode { get; set; }
        /// <summary>
        /// 审核单位
        /// </summary>
        [Alias("reviewer_comp")]
        [StringLengthAttribute(500)]
        public string ReviewerComp { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [Alias("reviewerdate")]
        public DateTime ReviewerDate { get; set; }
     

    }
}
