using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同采购申请生成订单接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/plan/to/order", HttpMethods.Post, Notes = "采购申请生成订单信息")]
    [DataContract]
    public class CreatePurOrderByPurPlanRequest : IReturn<CreatePurOrderByPurPlanResponse>
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

        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Eid { set; get; }
        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        /// <summary>
        /// 供应商集合
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "供应商集合",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public List<SupplierList> SupplierList { set; get; }

        /// <summary>
        ///采购申请主表ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "采购申请主表ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long PlanID { set; get; }
    }

   
    [DataContract]
    public class CreatePurOrderByPurPlanResponse
    {
        public CreatePurOrderByPurPlanResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 2)]
        public PurEvaluationID Data { get; set; }


        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }

      /// <summary>
    /// 返回评价信息
    /// </summary>
    [DataContract]
    public class PurEvaluationID : IReturn<CreatePurOrderByPurPlanResponse>
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }
    }
}
