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
    [Route("/{AppKey}/{Secretkey}/{AccountID}/search/plan/{ID}", HttpMethods.Get, Notes = "获取采购申请详情")]
    [Api("查看或者编辑采购申请详情")]
    public class EidtPurchasePlanByIdRequest : IReturn<EidtPurchasePlanByIdResponse>
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
        [DataMember(Order = 4)]
        [ApiMember(Description = "主单主键id",
        ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ID { get; set; }
    }
    [DataContract]
    public class EidtPurchasePlanByIdResponse
    {
        public EidtPurchasePlanByIdResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 3)]
        public ReturnData Data { get; set; }
    }
    [DataContract]
    public class ReturnData
    {
        [DataMember(Order = 1)]
        public PurchasePlan PurPlan { get; set; }
        [DataMember(Order = 2)]
        public List<PurchasePlanDetail> PurPlanDetailList { get; set; }
    }
}
