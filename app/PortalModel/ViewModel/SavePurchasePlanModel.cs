using ServiceStack.Common.Web;
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
    [Route("/{AppKey}/{Secretkey}/{AccountID}/save/plan", HttpMethods.Post)]
    [Api("保存采购计划")]
    public class SavePurchasePlanRequest : IReturn<SavePurchasePlanResponse>
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
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public int AccountID { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "申请主题",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PlanSubject { get; set; }
        [DataMember(Order =5)]
        [ApiMember(Description = "备注",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string MM { get; set; }
        [DataMember(Order = 6)]
        [ApiMember(Description = "明细",
        ParameterType = "json", DataType = "List", IsRequired = true)]
        public List<PurchasePlanDetail> PurchsePlanList { get; set; }
        [DataMember(Order = 7)]
        [ApiMember(Description = "创建人id",
        ParameterType = "json", DataType = "long", IsRequired = true)]
        public long EID { get; set; }
        [DataMember(Order = 8)]
        [ApiMember(Description = "创建人编码",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ECode { get; set; }
        [DataMember(Order = 9)]
        [ApiMember(Description = "创建人姓名",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EName { get; set; }
        [DataMember(Order = 10)]
        [ApiMember(Description = "主单主键ID",
        ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ID { get; set; }
      
    }
    [DataContract]
    public class SavePurchasePlanResponse
    {
        public SavePurchasePlanResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        public List<PurchasePlanDetail> Data { get; set; }
    }
}
