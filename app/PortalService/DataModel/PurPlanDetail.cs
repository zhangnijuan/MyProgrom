using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;
using ServiceStack.ServiceHost;

namespace Ndtech.PortalService.DataModel
{
     [Alias("pur_plandetail")]
   public  class PurPlanDetail
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Alias("pland_id")]
        public long ID { get; set; }
        /// <summary>
        /// 账套
        /// </summary>
        [Alias("pland_a")]
        public int AccountID { get; set; }
        /// <summary>
        ///主单外键
        /// </summary>
        [Alias("pland_mid")]
        public long MID { get; set; }


        /// <summary>
        /// 产品ID
        /// </summary>
        [Alias("pland_i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("pland_i_c")]
        [StringLengthAttribute(32)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("pland_i_n")]
        [StringLengthAttribute(1024)]
        public string ItemName { get; set; }
        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [Alias("pland_standarditemcode")]
        [StringLengthAttribute(32)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [Alias("pland_standarditemname")]
        [StringLengthAttribute(1024)]
        public string StandardItemName { get; set; }
        /// <summary>
        /// 产品三级类别id
        /// </summary>
        [Alias("pland_categorythirdid")]
        public long CategoryID { get; set; }
        /// <summary>
        /// 产品三级类别代码
        /// </summary>
        [Alias("pland_categorythirdcode")]
        [StringLengthAttribute(32)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品三级类别名称
        /// </summary>
        [Alias("pland_categorythirdname")]
        [StringLengthAttribute(100)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 产品属性名称
        /// </summary>
        [Alias("pland_propertyname")]
        [StringLengthAttribute(1024)]
        public string PropertyName { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        [Alias("pland_remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }
        /// <summary>
        /// 单位代码
        /// </summary>
        [Alias("pland_u_n")]
         [StringLengthAttribute(8)]
        public string UnitCode { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        [Alias("pland_requirdate")]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Alias("pland_mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }
        /// <summary>
        /// 状态号 1-已询价 2-已下单  默认0
        /// </summary>
        [Alias("pland_state")]
        public int state { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        [Alias("pland_state_enum")]
        [StringLengthAttribute(8)]
        public string StateEnum { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Alias("pland_qty")]
        public decimal Quantity { get; set; }
         /// <summary>
         /// 是否启用  0不启用  1启用
         /// </summary>
         [Alias("pland_enabled")]
        public int Enabled { get; set; }

         /// <summary>
         /// 产品类别父id
         /// </summary>
         [Alias("pland_categorythirpid")]
         public long CategoryMID { get; set; }
    }
}
