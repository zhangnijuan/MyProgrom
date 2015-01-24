using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业物品价格表
    /// </summary>
    [Alias("udoc_enterprise_price")]
    public class EnterpriseItemPrice
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
        /// 物品ID
        /// </summary>
        [Alias("mid")]
        public long ItemID { get; set; }

        /// <summary>
        /// 标准价格
        /// </summary>
        [Alias("prd")]
        public Decimal Price { get; set; }

        /// <summary>
        /// 定价时间
        /// </summary>
        [Alias("startdate")]
        public DateTime PriceStartDate { get; set; }


    }
}
