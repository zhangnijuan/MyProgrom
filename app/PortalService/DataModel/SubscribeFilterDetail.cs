using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
     [Alias("udoc_subscribe_filterdetail")]
    public class SubscribeFilterDetail
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        [AutoIncrement]
        public long ID { get; set; }
         /// <summary>
         /// 订阅主表Id
         /// </summary>
         [Alias("smid")]
        public long Mid { get; set; }
         /// <summary>
         /// 分类姓名
         /// </summary>
         [Alias("subsctheme")]
         [StringLengthAttribute(50)]
         public string CategoryName { get; set; }
    }
}
