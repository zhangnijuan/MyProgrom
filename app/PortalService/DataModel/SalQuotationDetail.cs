using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 报价单明细表
    /// add by yangshuo 2014/12/10
    /// </summary>
    [Alias("sal_quotationdetail")]
    public class SalQuotationDetail
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
        /// 主表ID
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
        /// 单位ID
        /// </summary>
        [Alias("u")]
        [StringLengthAttribute(32)]
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
        /// 数量
        /// </summary>
        [Alias("qty")]
        public decimal Quantity { get; set; }

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
        /// 产品属性名称
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
        /// 询价备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 询价产品描述
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 报价备注
        /// </summary>
        [Alias("smm")]
        [StringLengthAttribute(1024)]
        public string SMM { get; set; }

        /// <summary>
        /// 报价产品描述
        /// </summary>
        [Alias("sremark")]
        [StringLengthAttribute(1024)]
        public string SRemark { get; set; }
    
    }
}
