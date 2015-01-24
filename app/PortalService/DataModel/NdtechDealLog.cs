using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 交易日志表
    /// add by suyang 2014/12/17
    /// </summary>
    [Alias("udoc_deallog")]
    public class NdtechDealLog
    {
        /// <summary>
        /// id
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
        /// 采购方云ID
        /// </summary>
        [Alias("pur_c")]
        [StringLengthAttribute(32)]
        public string PurCode { get; set; }

        /// <summary>
        /// 采购方名称
        /// </summary>
        [Alias("pur_n")]
        [StringLengthAttribute(32)]
        public string PurName { get; set; }

        /// <summary>
        /// 询价编码
        /// </summary>
        [Alias("scode")]
        [StringLengthAttribute(32)]
        public string SCode { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [Alias("subject")]
        [StringLengthAttribute(32)]
        public string Subject { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Alias("su")]
        public long Supply { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [Alias("su_c")]
        [StringLengthAttribute(32)]
        public string SupplyCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Alias("su_n")]
        [StringLengthAttribute(32)]
        public string SupplyName { get; set; }

        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [Alias("standardItem_c")]
        [StringLengthAttribute(32)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [Alias("standardItem_n")]
        [StringLengthAttribute(1024)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 采购方物品编码
        /// </summary>
        [Alias("puritem_c")]
        [StringLengthAttribute(32)]
        public string PurItemCode { get; set; }

        /// <summary>
        /// 采购方物品名称
        /// </summary>
        [Alias("puritem_n")]
        [StringLengthAttribute(1024)]
        public string PurItemName { get; set; }

        /// <summary>
        /// 销售方物品编码
        /// </summary>
        [Alias("salitem_c")]
        [StringLengthAttribute(32)]
        public string SalItemCode { get; set; }

        /// <summary>
        /// 销售方物品名称
        /// </summary>
        [Alias("salitem_n")]
        [StringLengthAttribute(1024)]
        public string SalItemName { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        [Alias("propertyname")]
        [StringLengthAttribute(1024)]
        public string PropertyName { get; set; }

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
        /// 采购单位
        /// </summary>
        [Alias("u_n")]
        [StringLengthAttribute(32)]
        public string UName { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [Alias("tdate")]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        [Alias("purqty")]
        public decimal PurQty { get; set; }

        /// <summary>
        /// 报价单价
        /// </summary>
        [Alias("salprc")]
        public decimal SalPrc { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        [Alias("salamt")]
        public decimal SalAmt { get; set; }

        /// <summary>
        /// 成交日期
        /// </summary>
        [Alias("completedate")]
        public DateTime CompleteDate { get; set; }
    }
}
