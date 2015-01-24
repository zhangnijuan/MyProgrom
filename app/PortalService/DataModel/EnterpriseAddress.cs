using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业地址薄表
    /// </summary>
    [Alias("udoc_enterprise_address")]
    public class EnterpriseAddress
    {
        /// <summary>
        /// 主键
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
        /// 企业ID
        /// </summary>
        [Alias("compid")]
        public long CompID { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [Alias("compname")]
        [StringLengthAttribute(32)]
        public string CompName { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Alias("province")]
        [StringLengthAttribute(32)]
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Alias("city")]
        [StringLengthAttribute(32)]
        public string City { get; set; }

        /// <summary>
        /// 所在地区
        /// </summary>
        [Alias("district")]
        [StringLengthAttribute(32)]
        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(1024)]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Alias("zipcode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// 0 否 1 是
        /// </summary>
        [Alias("isdef")]
        public int IsDef { get; set; }

        /// <summary>
        /// 总地址
        /// </summary>
        [Alias("fulladdress")]
        [StringLengthAttribute(1120)]
        public string FullAddress { get; set; }
    }
}
