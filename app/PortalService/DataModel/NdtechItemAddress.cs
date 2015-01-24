using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业物品发货地址表
    /// add by yangshuo 2014/12/11
    /// </summary>
    [Alias("udoc_address")]
    public class NdtechItemAddress
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
        /// 物品发货地址
        /// </summary>
        [Alias("deliveryaddress")]
        [StringLengthAttribute(1024)]
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Alias("itemid")]
        public long ItemID { get; set; }
    }
}