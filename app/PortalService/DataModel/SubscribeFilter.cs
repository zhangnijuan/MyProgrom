using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{

     [Alias("udoc_subscribe_filter")]
   public class SubscribeFilter
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        [AutoIncrement]

         public long ID { get; set; }

         /// <summary>
        /// 订阅企业标识
         /// </summary>
           [Alias("to_a")]
        public int ToAccountID { get; set; }
        /// <summary>
           /// 被订阅企业标识
        /// </summary>
         [Alias("from_a")]
         [StringLengthAttribute(50)]
           public string  FromName { get; set; }

         /// <summary>
         /// 0 询价信息 1 产品信息
         /// </summary>

          [Alias("subtype")]
           public int Subtype { get; set; }

          /// <summary>
          /// 订阅主题
          /// </summary>
          [Alias("subsctheme")]
          [StringLengthAttribute(50)]
          public string SubTheme { get; set; }

         /// <summary>
          /// 订阅者ID
         /// </summary>
          [Alias("subscriber")]
          public long Subscriber { get; set; }

         /// <summary>
          /// 订阅者编码
         /// </summary>
           [Alias("subscriber_c")]
           [StringLengthAttribute(32)]
          public string SubscriberCode { get; set; }

         /// <summary>
           /// 订阅者名称
         /// </summary>E:\Progrom\program\app\PortalService\Warning\
           [Alias("subscriber_n")]
           [StringLengthAttribute(32)]
           public string SubscriberName { get; set; }


         /// <summary>
           /// 交货地址
         /// </summary>
           [Alias("deliveryAddress")]
           [StringLengthAttribute(64)]
         public string DeliveryAddress { get; set; }



           /// <summary>
           /// 产品分类名称
           /// </summary>
           [Alias("category_n")]
           [StringLengthAttribute(64)]
           public string CategoryName { get; set; }

           /// <summary>
           /// 报价最小数量
           /// </summary>
           [Alias("minqty")]
           public decimal MinQty { get; set; }


           /// <summary>
           /// 报价最大数量
           /// </summary>
           [Alias("maxqty")]
           public decimal MaxQty { get; set; }
          /// <summary>
          /// 最后一次查询时间
          /// </summary>
           [Alias("lasttime")]
           public DateTime LastTime { get; set; }
    }
}
