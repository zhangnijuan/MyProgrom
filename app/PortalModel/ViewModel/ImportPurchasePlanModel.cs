using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Ndtech.PortalModel
{
    [DataContract]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/Import/plan", HttpMethods.Post)]
    [Api("导入采购计划")]
    public class ImportPurchasePlanRequest : IReturn<ImportPurchasePlanResponse>
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
    }
    [DataContract]
    public class ImportPurchasePlanResponse
    {
        public ImportPurchasePlanResponse()
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
    [DataContract]
    public class PurchasePlanDetail
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "明细主键",
       ParameterType = "json", DataType = "long", IsRequired = true)]
        [Alias("pland_id")]
        public long ID { get; set; }
        [DataMember(Order = 2)]
        [Alias("pland_i_c")]
        public string ItemCode { get; set; }
        [DataMember(Order = 3)]
        [Alias("pland_i_n")]
        public string ItemName { get; set; }
        [DataMember(Order = 4)]
        [Alias("pland_categorythirdid")]
        public long CategoryID { get; set; }
        [DataMember(Order = 5)]
        [Alias("pland_categorythirdname")]
        public string CategoryName { get; set; }
        [DataMember(Order = 6)]
        [Alias("pland_categorythirdcode")]
        public string CategoryCode { get; set; }
        [DataMember(Order = 7)]
        
        [Alias("pland_standarditemname")]
        public string StandardItemName { get; set; }
        [DataMember(Order = 8)]
        [Alias("pland_standarditemcode")]
        public string StandardItemCode { get; set; }
        [DataMember(Order = 9)]
        [Alias("pland_qty")]
        public decimal Quantity { get; set; }
        [DataMember(Order = 10)]
        [Alias("pland_requirdate")]
        public DateTime DeliveryDate { get; set; }
        [DataMember(Order = 11)]
        [Alias("pland_propertyname")]
        public string PropertyName { get; set; }
        [DataMember(Order = 12)]
        [Alias("pland_remark")]
        public string Remark { get; set; }
        [DataMember(Order = 13)]
        [Alias("pland_u_n")]
        public string UnitCode { get; set; }
        [DataMember(Order = 14)]
        [ApiMember(Description = "主表id",
  ParameterType = "json", DataType = "long", IsRequired = true)]
        [Alias("pland_mid")]
        public long MID { get; set; }
        [DataMember(Order = 15)]
        [Alias("pland_state_enum")]
        public string StateEnum { get; set; }
        [DataMember(Order = 16)]
        [ApiMember(Description = "备注",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        [Alias("pland_mm")]
        public string MM { get; set; }

        [DataMember(Order = 17)]
        [Alias("pland_a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMember(Order = 18)]
        [Alias("pland_i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 状态号 1-已询价 2-已下单  默认0
        /// </summary>
        [DataMember(Order = 19)]
        [Alias("pland_state")]
        public int state { get; set; }

        /// 是否启用  0不启用  1启用
        /// </summary>
        [DataMember(Order = 20)]
        [Alias("pland_enabled")]
        public int Enabled { get; set; }

    }
}
