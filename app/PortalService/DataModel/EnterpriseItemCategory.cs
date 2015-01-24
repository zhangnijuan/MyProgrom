using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 产品分类档案(企业产品)
    /// add by yangshuo 2014/12/15
    /// </summary>
    [Alias("udoc_enterprise_category")]
    public class EnterpriseItemCategory
    {
        /// <summary>
        /// 产品分类id
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
        /// 上级分类id
        /// </summary>
        [Alias("pid")]
        public long ParentID { get; set; }

        /// <summary>
        /// 产品分类代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(64)]
        public string CategoryName { get; set; }
    }
}
