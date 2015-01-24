using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 购物车主表
    /// add by liuzhiqiang 2014/12/29
    /// </summary>
    [Alias("pur_shopping_cart")]
    class PurShoppingCart
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
        /// 制单人
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 制单人code
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人Name
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(50)]
        public string EIDName { get; set; }

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
        /// 单位名称
        /// </summary>
        [Alias("u_n")]
        [StringLengthAttribute(32)]
        public string UnitName { get; set; }

        /// <summary>
        /// 购买数量
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
        /// 供应企业名称
        /// </summary>
        [Alias("compname")]
        [StringLengthAttribute(64)]
        public string CompName { get; set; }

        /// <summary>
        /// 供应企业云ID
        /// </summary>
        [Alias("compid")]
        [StringLengthAttribute(32)]
        public string CompID { get; set; }

        /// <summary>
        /// 总报价金额
        /// </summary>
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态 1启用 0停用
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }
        
    }
}
