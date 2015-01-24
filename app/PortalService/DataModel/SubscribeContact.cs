using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
       [Alias("udoc_subscribe_contact")]
   public class SubscribeContact
    {
           /// <summary>
           /// 主键ID
           /// </summary>
           [PrimaryKey]
           [Alias("id")]
           [AutoIncrement]
           public long ID { get; set; }

           /// <summary>
           /// 被订阅企业标识
           /// </summary>
           [Alias("from_a")]
           public int FromAccountID { get; set; }

           /// <summary>
           /// 订阅人ID
           /// </summary>
           [Alias("subscriber")]
           public long Subscriber { get; set; }


           /// <summary>
           /// 订阅人姓名
           /// </summary>
            [Alias("subscriber_n")]
            [StringLengthAttribute(32)]
           public string SubscriberName { get; set; }

           /// <summary>
           /// 订阅人编号
           /// </summary>
            [Alias("subscriber_c")]
            [StringLengthAttribute(32)]
            public string SubscriberCode { get; set; }



            /// <summary>
            /// 订阅人企业标识
            /// </summary>
            [Alias("to_a")]
            public int ToAccountID { get; set; }





    }
}
