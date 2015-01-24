using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 收发货记录表
    /// add by Zoumeiyue 2014/12/25
    /// </summary>
    [Alias("sal_outnoticedetail")]
    public class SalOutNoticeDetail
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
        /// 订单明细ID
        /// </summary>
        [Alias("detailid")]
        public long DetailID { get; set; }


        /// <summary>
        /// 收货日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 发货备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        [Alias("deliveryqty")]
        public decimal DeliveryQty { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        [Alias("arrivalqty")]
        public decimal ArrivalQty { get; set; }

        /// <summary>
        /// 状态 0未收货 1已收货
        /// </summary>
        [Alias("state")]
        public int State { get; set; }


    }
}
