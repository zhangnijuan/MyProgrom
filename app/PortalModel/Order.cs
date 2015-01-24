using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel
{
    [DataContract]
    public class Order
    {
        [DataMember(Order = 1)]
        public OrderKey orderKey { get; set; }
        [DataMember(Order = 2)]
        public SortKey sortKey { get; set; }
    }
    [DataContract]
    public enum OrderKey
    {
        prc,
        createtime,
        FinalDateTime,
        Address,
        /// <summary>
        /// 收藏时间
        /// </summary>
        Subdatetime,
        //产品创建时间
        createdate,
        /// <summary>
        /// 收藏人气
        /// </summary>
        Favorites,
        /// <summary>
        /// 询价日期
        /// </summary>
        publisherdt,
        /// <summary>
        /// 采购申请时间
        /// </summary>
        Plan_create
    }
    [DataContract]
    public enum SortKey
    {
        Ascending,
        Descending
    }
}
