using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品认证机构表
    /// add by yangshuo 2015/01/05
    /// </summary>
    [Alias("udoc_certification")]
    public class NdtechItemCertification
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(32)]
        public string CertificationName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("linkman")]
        [StringLengthAttribute(32)]
        public string Linkman { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Alias("phone")]
        [StringLengthAttribute(32)]
        public string Phone { get; set; }

        /// <summary>
        /// 送检地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(64)]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Alias("zipcode")]
        [StringLengthAttribute(32)]
        public string ZipCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 认证机构彩色logoID
        /// </summary>
        [Alias("colorpicid")]
        public long ColorPicID { get; set; }

        /// <summary>
        /// 认证机构灰色logoID
        /// </summary>
        [Alias("graypicid")]
        public long GrayPicID { get; set; }
    }
}
