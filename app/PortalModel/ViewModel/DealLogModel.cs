using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同交易日志接口")]
    [Route("/deallog", HttpMethods.Get, Notes = "交易日志")]
    [DataContract]
    public class DealLogRequest : IReturn<DealLogResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "采购方云ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        public string PurCode { get; set; }


        [DataMember(Order = 2)]
        [ApiMember(Description = "采购方名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PurName { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "询价编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SCode { get; set; }


        [DataMember(Order = 4)]
        [ApiMember(Description = "询价主题",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Subject { get; set; }


        [DataMember(Order = 5)]
        [ApiMember(Description = "供应商ID",
          ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Supply { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "供应商编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SupplyCode { get; set; }

        [DataMember(Order = 7)]
        [ApiMember(Description = "供应商名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SupplyName { get; set; }


        [DataMember(Order = 8)]
        [ApiMember(Description = "平台标准产品编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string StandardItemCode { get; set; }

        [DataMember(Order = 9)]
        [ApiMember(Description = "平台标准产品名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string StandardItemName { get; set; }

        [DataMember(Order = 10)]
        [ApiMember(Description = "采购方物品编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PurItemCode { get; set; }


        [DataMember(Order = 11)]
        [ApiMember(Description = "采购方物品名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PurItemName { get; set; }

        [DataMember(Order = 12)]
        [ApiMember(Description = "销售方物品编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SalItemCode { get; set; }

        [DataMember(Order = 13)]
        [ApiMember(Description = "销售方物品名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SalItemName { get; set; }

        [DataMember(Order = 14)]
        [ApiMember(Description = "产品属性",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PropertyName { get; set; }

        [DataMember(Order = 15)]
        [ApiMember(Description = "企业产品分类编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string CategoryCode { get; set; }

        [DataMember(Order = 16)]
        [ApiMember(Description = "企业产品分类名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string CategoryName { get; set; }

        [DataMember(Order = 17)]
        [ApiMember(Description = "采购单位",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string UName { get; set; }

        [DataMember(Order = 18)]
        [ApiMember(Description = "交易日期",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public DateTime TransactionDate { get; set; }

        [DataMember(Order = 19)]
        [ApiMember(Description = "采购数量",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public decimal PurQty { get; set; }

        [DataMember(Order = 20)]
        [ApiMember(Description = "报价单价",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public decimal SalPrc { get; set; }


        [DataMember(Order = 21)]
        [ApiMember(Description = "报价金额",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public decimal SalAmt { get; set; }

        [DataMember(Order = 22)]
        [ApiMember(Description = "成交日期",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public DateTime CompleteDate { get; set; }
    }
    public class DealLogResponse
    {
        public DealLogResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
