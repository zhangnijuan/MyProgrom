using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 订单明细表
    /// add by liuzhiqiang 2014/12/24
    /// </summary>
    [Alias("pur_orderdetail")]
    class PurOrderDetail
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
        /// 供应方标识
        /// </summary>
        [Alias("sa")]
        public int SAccountID { get; set; }

        /// <summary>
        /// 主表外键
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
        /// 单价
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
        /// 购物车明细ID
        /// </summary>
        [Alias("shoppingcartdid")]
        public long ShoppingCartdid { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }
        /// <summary>
        /// 累计到货数量
        /// </summary>
        [Alias("arrivalqty")]
     
        public decimal Arrivalqty { get; set; }
        /// <summary>
        /// 累计收款金额
        /// </summary>
        [Alias("receivingamt")]
        public decimal ReceivingAmt { get; set; }

        /// <summary>
        /// 采购申请明细ID
        /// </summary>
        [Alias("plandid")]
        public long Plandid { get; set; }
    }
}
