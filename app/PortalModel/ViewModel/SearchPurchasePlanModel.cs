using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{

    [DataContract]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/search/plan", HttpMethods.Post)]
    [Api("搜索采购计划")]
    public class SearchPurchasePlanRequest : IReturn<SearchPurchasePlanResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
        [DataMember(Order = 3)]
        [ApiMember(Description = "帐套",
        ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountID { get; set; }
        [ApiMember(Description = "第几页",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int PageSize { get; set; }
        [ApiMember(Description = "查询条件(PlanSubject:主题,BeginTime:开始时间,EndTime: 结束时间,State：（全部不传，0：未完成，1：已完成)",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 6)]
        public List<SearchPlanFiled> SearchCondition { get; set; }
        [ApiMember(Description = "是否查询所有",
                ParameterType = "json", DataType = "bool", IsRequired = true)]
        [DataMember(Order = 7)]
        public bool IsSearchAll { get; set; }
        [ApiMember(Description = "是否查询所有",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 8)]
        public long EID { get; set; }
        [ApiMember(Description = "排序时间Plan_create",
              ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<Order> Orders { get; set; }

    }
    [DataContract]
    public class SearchPurchasePlanResponse
    {
          public SearchPurchasePlanResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        public List<PurchasePlan> Data { get; set; }
        [DataMember(Order = 4)]
        public int RowsCount { get; set; }
         [DataMember(Order = 5)]
        public DataCount DataCounts { get; set; }
    }
    [DataContract]
    public class DataCount
    {
        [DataMember(Order = 1)]
        public int AllCount { get; set; }
        [DataMember(Order = 2)]
        public int UnCount { get; set; }
        [DataMember(Order = 3)]
        public int DoCount { get; set; }
    }
    [DataContract]
    public class SearchPlanFiled
    {
        [DataMember(Order = 1)]
        public SearchPlanEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
    /// <summary>
    /// 搜索条件
    /// </summary>
     [DataContract]
    public enum SearchPlanEnum
    {
         /// <summary>
         /// 主题
         /// </summary>
         PlanSubject,
         /// <summary>
         /// 开始时间
         /// </summary>
         BeginTime,
         /// <summary>
         /// 结束时间
         /// </summary>
         EndTime,
         /// <summary>
         /// 完成状态
         /// </summary>
         State

    }
    [DataContract]
    public class PurchasePlan
    {
        /// <summary>
        /// 主键
        /// </summary>
      
        [Alias("plan_id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// 账套
        /// </summary>
        [Alias("plan_a")]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        [Alias("plan_c")]
        [DataMember(Order = 3)]
        public string PlanCode { get; set; }
        /// <summary>
        /// 申请主题
        /// </summary>
        [Alias("plan_subject")]
        [DataMember(Order = 4)]
        public string PlanSubject { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        [Alias("plan_create")]
        [DataMember(Order = 5)]
        public DateTime CreateTime { get; set; }
      
       
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [Alias("plan_eid_n")]
        [DataMember(Order = 6)]
        public string EName { get; set; }
      
        /// <summary>
        /// 状态名称
        /// </summary>
        [Alias("plan_state_enum")]
        [DataMember(Order = 7)]
        public string StateEnum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Alias("plan_mm")]
        [DataMember(Order = 8)]
        public string MM { get; set; }
     
    }
}
