using Ndtech.PortalModel.ViewModel;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    [Alias("pur_plan")]
    public class PurPlan
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Alias("plan_id")]
        public long ID { get; set; }
        /// <summary>
        /// 账套
        /// </summary>
         [Alias("plan_a")]
        public int AccountID { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
         [Alias("plan_c")]
         [StringLengthAttribute(64)]
        public string PlanCode { get; set; }
         /// <summary>
         /// 申请主题
         /// </summary>
         [Alias("plan_subject")]
         [StringLengthAttribute(64)]
        public string PlanSubject { get; set; }
         /// <summary>
         /// 申请时间
         /// </summary>
         [Alias("plan_create")]  
        public DateTime CreateTime { get; set; }
         /// <summary>
         /// 创建人
         /// </summary>
         [Alias("plan_eid")]
        public long EID { get; set; }
         /// <summary>
         /// 创建人编码
         /// </summary>
         [Alias("plan_eid_c")]
         [StringLengthAttribute(32)]
        public string ECode { get; set; }
         /// <summary>
         /// 创建人姓名
         /// </summary>
         [Alias("plan_eid_n")]
         [StringLengthAttribute(32)]
        public string EName { get; set; }
         /// <summary>
         /// 状态号 0 未完成  1  已完成  2 草稿
         /// </summary>
         [Alias("plan_state")]
        public int state { get; set; }
         /// <summary>
         /// 状态名称
         /// </summary>
         [Alias("plan_state_enum")]
         [StringLengthAttribute(8)]
        public string StateEnum { get; set; }
         /// <summary>
         /// 备注
         /// </summary>
         [Alias("plan_mm")]
         [StringLengthAttribute(1024)]
         public string MM { get; set; }
       

    }
}
