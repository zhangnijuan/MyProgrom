using System;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 平台标准产品价格表
    /// </summary>
    [Alias("udoc_item_price")]
    public class NdtechItemPrice
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
        /// 产品ID
        /// </summary>
        [Alias("mid")]
        public long ItemID { get; set; }

        /// <summary>
        /// 标准价格
        /// </summary>
        [Alias("prd")]
        public Decimal Price { get; set; }

        /// <summary>
        /// 定价起始时间
        /// </summary>
        [Alias("startdate")]
        public DateTime PriceStartDate { get; set; }

        /// <summary>
        /// 定价结束时间
        /// </summary>
        [Alias("enddate")]
        public DateTime PriceEndDate { get; set; }
    }
}
