using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品认证申请表
    /// add by yangshuo 2015/01/05
    /// </summary>
    [Alias("udoc_certification_application")]
    public class EnterpriseItemsCertification
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 申请方帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 受理机构帐套
        /// </summary>
        [Alias("ca")]
        public int CAccountID { get; set; }

        /// <summary>
        /// 申请编号
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(64)]
        public string Code { get; set; }

        /// <summary>
        /// 申请方公司名称
        /// </summary>
        [Alias("whatsoever")]
        [StringLengthAttribute(64)]
        public string Whatsoever { get; set; }

        /// <summary>
        /// 申请方联系人
        /// </summary>
        [Alias("linkman")]
        [StringLengthAttribute(32)]
        public string Linkman { get; set; }

        /// <summary>
        /// 申请方手机
        /// </summary>
        [Alias("mobile")]
        [StringLengthAttribute(32)]
        public string Mobile { get; set; }

        /// <summary>
        /// 申请方固话
        /// </summary>
        [Alias("phone")]
        [StringLengthAttribute(32)]
        public string Phone { get; set; }

        /// <summary>
        /// 申请方传真
        /// </summary>
        [Alias("tax")]
        [StringLengthAttribute(32)]
        public string Tax { get; set; }

        /// <summary>
        /// 申请方邮箱
        /// </summary>
        [Alias("email")]
        [StringLengthAttribute(32)]
        public string Email { get; set; }

        /// <summary>
        /// 申请方邮编
        /// </summary>
        [Alias("zipcode")]
        [StringLengthAttribute(32)]
        public string ZipCode { get; set; }

        /// <summary>
        /// 申请方详细地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(1024)]
        public string Address { get; set; }

        /// <summary>
        /// 受理机构名称
        /// </summary>
        [Alias("certificationname")]
        [StringLengthAttribute(32)]
        public string CertificationName { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 处理日期
        /// </summary>
        [Alias("acceptdate")]
        public DateTime AcceptDate { get; set; }

        /// <summary>
        /// 单证状态
        /// 0未处理  1已处理 2处理中
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 申请人ID
        /// </summary>
        [Alias("eid")]
        public long Eid { get; set; }

        /// <summary>
        /// 申请人编码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EidCode { get; set; }

        /// <summary>
        /// 申请人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(32)]
        public string EidName { get; set; }
    }
}
