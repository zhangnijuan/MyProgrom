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
    [Api("恩维协同查询企业报价信息接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/offer/search", HttpMethods.Post, Notes = "查询企业报价信息")]
    [DataContract]
    public class SearchSalQuotationRequest : IReturn<SearchSalQuotationResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "企业ID",
  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        [ApiMember(Description = "第几页",
      ParameterType = "json", DataType = "Page", IsRequired = true)]
        [DataMember(Order = 4)]
        public Page page { get; set; }

        [ApiMember(Description = "字段条件",
  ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 5)]
        public List<SearchField> SearchCondition { get; set; }
        [ApiMember(Description = "排序",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 7)]
        public List<Order> orders { get; set; }

        [ApiMember(Description = "单证类型",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string type { get; set; }


        [ApiMember(Description = "是否是查询所有（true：表示查询所有）",
                 ParameterType = "json", DataType = "bool", IsRequired = true)]
        [DataMember(Order = 8)]
        public bool IsSearchAll { get; set; }

        [ApiMember(Description = "登陆人ID",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string Eid { get; set; }
    }
    [DataContract]
    public class SearchSalQuotationResponse
    {
        public SearchSalQuotationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public List<SalQuotationview> Data { get; set; }
        [DataMember(Order = 2)]
        public long RowsCount { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 4)]
        public bool Success { get; set; }
    }
    public class SalQuotationview : IReturn<SearchSalQuotationResponse>
    {
        [ApiMember(Description = "报价单id",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "询价单账套",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string PurAccountID { get; set; }

        [ApiMember(Description = "询价主题",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Subject { get; set; }
        [ApiMember(Description = "报价日期",
          ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 3)]
        public DateTime CreateTime { get; set; }
        [ApiMember(Description = "报价截止日期",
                  ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 4)]
        public DateTime FinalDateTime { get; set; }
        [ApiMember(Description = "采购公司名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string Purchaser { get; set; }


        [ApiMember(Description = "采购公司云ID",
  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string PurCorpnum { get; set; }



        [ApiMember(Description = "公司名称",
  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string SCompanyName { get; set; }
  
        [ApiMember(Description = "询价状态",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 8)]
        public int inquiryState { get; set; }

        [ApiMember(Description = "报价状态",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 9)]
        public int State { get; set; }

       [ApiMember(Description = "询价单id",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public long InquiryID { get; set; }
       /// <summary>
       /// 制单人Name 
       /// </summary>
       [DataMember(Order = 11)]
       public string EIDName { get; set; }
       /// <summary>
       /// 询价日期
       /// </summary>
       [DataMember(Order = 12)]
       public DateTime InquiryDateTime { get; set; }

       /// <summary>
       /// 匿名类型 
       /// </summary>
       [DataMember(Order = 11)]
       public int AnonymousCode { get; set; }


        
    }
}
