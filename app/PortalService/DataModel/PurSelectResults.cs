using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 采购优选结果
    /// add by yangshuo 2014/12/19
    /// </summary>
    [Alias("pur_select_results")]
    public class PurSelectResults
    {
        /// <summary>
        /// 主键id
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
        /// 优选单主档ID
        /// </summary>
        [Alias("mid")]
        public long MID { get; set; }

        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [Alias("standarditemcode")]
        [StringLengthAttribute(32)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [Alias("standarditemname")]
        [StringLengthAttribute(1024)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 企业产品分类编码
        /// </summary>
        [Alias("category_c")]
        [StringLengthAttribute(32)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 企业产品分类名称
        /// </summary>
        [Alias("category_n")]
        [StringLengthAttribute(32)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("i_c")]
        [StringLengthAttribute(32)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("i_n")]
        [StringLengthAttribute(1024)]
        public string ItemName { get; set; }

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
        /// 询价数量
        /// </summary>
        [Alias("qty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 优选数量
        /// </summary>
        [Alias("sqty")]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 优选单价
        /// </summary>
        [Alias("prc")]
        public decimal Prc { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Alias("amt")]
        public decimal Amt { get; set; }

        /// <summary>
        /// 产品属性值
        /// </summary>
        [Alias("propertyname")]
        [StringLengthAttribute(1024)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [Alias("deliverydate")]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 询价单明细ID
        /// </summary>
        [Alias("inquirydid")]
        public long Inquirydid { get; set; }

        /// <summary>
        /// 报价单明细ID
        /// </summary>
        [Alias("quotationdid")]
        public long Quotationdid { get; set; }

        /// <summary>
        /// 报价企业
        /// </summary>
        [Alias("compname")]
        [StringLengthAttribute(64)]
        public string CompName { get; set; }

        /// <summary>
        /// 报价企业云ID
        /// </summary>
        [Alias("compid")]
        [StringLengthAttribute(32)]
        public string CompID { get; set; }

        /// <summary>
        /// 优选供应商产品总金额
        /// </summary>
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 报价方产品描述
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }
    }
}
