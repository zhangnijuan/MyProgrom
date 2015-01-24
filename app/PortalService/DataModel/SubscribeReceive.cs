using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{

    [Alias("udoc_subscribe_receive")]
   public class SubscribeReceive
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
          /// 0 询价信息 1 产品信息
          /// </summary>
          [Alias("subtype")]
          public int Subtype { get; set; }


          /// <summary>
          /// 来源信息ID
          /// </summary>
          [Alias("from_id")]
          public long FromID { get; set; }


          /// <summary>
          ///0未订阅 1 已订阅
          /// </summary>
          [Alias("substate")]
          public int Substate { get; set; }

          /// <summary>
          ///订阅人企业标识
          /// </summary>
          [Alias("to_a")]
          public int ToAccountID { get; set; }


          /// <summary>
          ///订阅人编码
          /// </summary>
          [Alias("subscribercode")]
          public string SubscriberCode { get; set; }
          /// <summary>
          ///订阅时间
          /// </summary>
          [Alias("subdatetime")]
          public DateTime SubDateTime { get; set; }

    }
}
