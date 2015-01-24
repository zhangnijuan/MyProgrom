using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据采购申请明细ID集合获取实体接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/plan/to/inquiry", HttpMethods.Post, Notes = "根据采购申请明细ID集合获取实体信息")]
    [DataContract]
    public class GetPlanDetailByIDRequest : IReturn<GetPlanDetailByIDResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "采购申请明细ID集合",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 1)]
        public List<IDList> Condition { get; set; }

        /// <summary>
        ///采购申请主表ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "采购申请主表ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long PlanID { set; get; }
    }

    [DataContract]
    public class GetPlanDetailByIDResponse
    {
        public GetPlanDetailByIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public List<PlanDetail> Data { get; set; }

        /// <summary>
        ///采购申请主表ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "采购申请主表ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long PlanID { set; get; }
    }

    [DataContract]
    public class IDList
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "标识ID",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ID { get; set; }

    }

    [DataContract]
    public class PlanDetail
    {
        [ApiMember(Description = "明细ID",
                     ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ID { get; set; }

        [ApiMember(Description = "数量",
                     ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 1)]
        public decimal Quantity { get; set; }

        [ApiMember(Description = "交货期",
                    ParameterType = "json", DataType = "Date", IsRequired = true)]
        [DataMember(Order = 2)]
        public DateTime DeliveryDate { get; set; }

        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string MM { get; set; }

        [ApiMember(Description = "产品ID",
                     ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ItemID { get; set; }

        [ApiMember(Description = "产品代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string ItemName { get; set; }

        [ApiMember(Description = "单位ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
        public long UnitID { get; set; }

        [ApiMember(Description = "单位代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string UnitCode { get; set; }

        [ApiMember(Description = "单位名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string UnitName { get; set; }

        [ApiMember(Description = "产品类别ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 10)]
        public long CategoryID { get; set; }

        [ApiMember(Description = "产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string CategoryCode { get; set; }

        [ApiMember(Description = "产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string CategoryName { get; set; }

        [ApiMember(Description = "产品类别父ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 13)]
        public long CategoryMID { get; set; }

        [ApiMember(Description = "平台标准产品编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "平台标准产品名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "产品描述",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string Remark { get; set; }

        /// <summary>
        /// 产品属性名称
        /// </summary>
        [DataMember(Order = 13)]
        public string PropertyName { get; set; }

        [ApiMember(Description = "采购申请明细ID",
                        ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long PlanDetailID { get; set; }
    }
}
