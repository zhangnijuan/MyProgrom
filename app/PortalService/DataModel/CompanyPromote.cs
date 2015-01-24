using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业推广产品信息表
    /// </summary>
    [Alias("udoc_comp_promote_product")]
    public  class CompanyPromote
    {
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }
        /// <summary>
        /// 企业产品Id
        /// </summary>
        [Alias("i")]
        public long ItemID { get; set; }
        /// <summary>
        /// 企业产品编码
        /// </summary>
        [Alias("i_c")]
        [StringLengthAttribute(100)]
        public string ItemCode { get; set; }
        /// <summary>
        /// 企业产品名称
        /// </summary>
        [Alias("i_n")]
        [StringLengthAttribute(100)]

        public string ItemName { get; set; }
        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [Alias("standarditemcode")]
        [StringLengthAttribute(100)]
        public string StandardItemCode { get; set; }
        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [Alias("standarditemname")]
        [StringLengthAttribute(100)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 物品图片ID
        /// </summary>
        [Alias("Itemrid")]
        public long ItemResources{get;set;}
        /// <summary>
        /// 排序顺序
        /// </summary>
        [Alias("oid")]
        public int OrderNumber { get; set; }
    }
}
