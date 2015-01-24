using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品属性值档案
    /// add by yangshuo 2014/12/11
    /// </summary>
    [Alias("udoc_enterprise_attribute")]
    public class EnterpriseItemAttribute
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
        /// 属性类
        /// </summary>
        [Alias("attribute_class")]
        [StringLengthAttribute(32)]
        public string AttributeClass { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [Alias("attribute_value")]
        [StringLengthAttribute(64)]
        public string AttributeValue { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Alias("itemid")]
        public long ItemID { get; set; }

        /// <summary>
        /// 属性单位
        /// add 2015/01/14
        /// </summary>
        [Alias("u_n")]
        [StringLengthAttribute(8)]
        public string UnitName { get; set; }
    }
}
