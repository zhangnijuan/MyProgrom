using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 付款收款记录
    /// add by Zoumeiyue 2014/12/25
    /// </summary>
    [Alias("arap_receiving")]
    public class ArapReceiving
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }


        /// <summary>
        /// 采购方企业标识
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 供应方企业标识
        /// </summary>
        [Alias("sa")]
        public int SAccountID { get; set; }

        /// <summary>
        /// 订单主表ID
        /// </summary>
        [Alias("orderid")]
        public long OrderID { get; set; }


        /// <summary>
        /// 付款日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        [Alias("collection")]
        public decimal Collection { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [Alias("payment")]
        public decimal Payment { get; set; }

        /// <summary>
        /// 状态 0未付款 1已付款
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

    }
}
