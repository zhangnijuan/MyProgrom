using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// (平台与企业共用)产品属性类表
    /// add by yangshuo 2014/12/11
    /// </summary>
    [Alias("udoc_attribute")]
    public class NdtechAttribute
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
        /// 产品属性类
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string AttributeClass { get; set; }

        /// <summary>
        /// 产品分类id(最底层分类id)
        /// </summary>
        [Alias("categoryid")]
        public long CategoryID { get; set; }

        /// <summary>
        /// 是否可范围搜索
        /// 默认0
        /// 0否 1是
        /// </summary>
        [Alias("isscope")]
        public int IsScope { get; set; }

        /// <summary>
        /// 范围搜索区域值
        /// </summary>
        [Alias("scopeinfo")]
        [StringLengthAttribute(1024)]
        public string ScopeInfo { get; set; }
    }
}
