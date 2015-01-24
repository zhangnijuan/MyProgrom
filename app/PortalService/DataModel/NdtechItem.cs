using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 平台标准产品档案
    /// add by yangshuo 2014/12/05
    /// </summary>
    [Alias("udoc_item")]
    public class NdtechItem
    {
        /// <summary>
        /// 产品id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountId { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(64)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(1024)]
        public string ItemName { get; set; }

        /// <summary>
        /// 一级产品分类id
        /// </summary>
        [Alias("category1")]
        public long Category1ID { get; set; }

        /// <summary>
        /// 一级产品分类代码
        /// </summary>
        [Alias("category1_c")]
        [StringLengthAttribute(32)]
        public string Category1Code { get; set; }

        /// <summary>
        /// 一级产品分类名称
        /// </summary>
        [Alias("category1_n")]
        [StringLengthAttribute(64)]
        public string Category1Name { get; set; }

        /// <summary>
        /// 二级产品分类id
        /// </summary>
        [Alias("category2")]
        public long Category2ID { get; set; }

        /// <summary>
        /// 二级产品分类代码
        /// </summary>
        [Alias("category2_c")]
        [StringLengthAttribute(32)]
        public string Category2Code { get; set; }

        /// <summary>
        /// 二级产品分类名称
        /// </summary>
        [Alias("category2_n")]
        [StringLengthAttribute(64)]
        public string Category2Name { get; set; }

        /// <summary>
        /// 三级产品分类id
        /// </summary>
        [Alias("category3")]
        public long Category3ID { get; set; }

        /// <summary>
        /// 三级产品分类代码
        /// </summary>
        [Alias("category3_c")]
        [StringLengthAttribute(32)]
        public string Category3Code { get; set; }

        /// <summary>
        /// 三级产品分类名称
        /// </summary>
        [Alias("category3_n")]
        [StringLengthAttribute(64)]
        public string Category3Name { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        [Alias("u")]
        public long UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [Alias("u_c")]
        [StringLengthAttribute(32)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Alias("u_n")]
        [StringLengthAttribute(32)]
        public string UnitName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        [Alias("createdate")]
        public DateTime Createdate { get; set; }
    }
}
