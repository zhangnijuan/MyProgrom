using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同查询报价企业列表信息接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/inquiry/basic/search", HttpMethods.Post, Notes = "显示报价企业列表信息")]
    [DataContract]
    public class SearchSalQuotationCompInfoRequest : IReturn<SearchSalQuotationCompInfoResponse>
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
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public Page page { get; set; }

        [ApiMember(Description = "字段条件",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 5)]
        public List<SearchQuotCompField> SearchCondition { get; set; }

        //[ApiMember(Description = "属性条件",
        //     ParameterType = "json", DataType = "list")]
        //[DataMember(Order = 6)]
        //public List<SearchAttribute> AttCondition { get; set; }
        [ApiMember(Description = "排序",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 6)]
        public List<QuotCompOrder> orders { get; set; }

    }
    /// <summary>
    /// 排序条件
    /// </summary>
    [DataContract]
    public class QuotCompOrder
    {
        [DataMember(Order = 1)]
        public QuotCompOrderKey orderKey { get; set; }
        [DataMember(Order = 2)]
        public QuotCompSortKey sortKey { get; set; }
    }
    [DataContract]
    public enum QuotCompOrderKey
    {
        CreateDate,//报价时间
        Amount//报价总价
    }
    [DataContract]
    public enum QuotCompSortKey
    {
        Ascending,
        Descending
    }

    [DataContract]
    public class SearchQuotCompField
    {
        [DataMember(Order = 1)]
        public SearchQuotCompEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
    [DataContract]
    public enum SearchQuotCompEnum
    {
         [Description("询价单主表ID")]
         MID
    }


    [DataContract]
    public class SearchSalQuotationCompInfoResponse
    {
        public SearchSalQuotationCompInfoResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public List<QuotCompList> Data { get; set; }
        [DataMember(Order = 2)]
        public long RowsCount { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 4)]
        public bool Success { get; set; }

    }
    [DataContract]
    public class QuotCompList
    {
        [ApiMember(Description = "标示id",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }


        [ApiMember(Description = "帐套",
        ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [ApiMember(Description = "公司名称",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string CompName { get; set; }

        [ApiMember(Description = "云ID",
       ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string CorpNum { get; set; }

        [ApiMember(Description = "报价总价",
       ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        [Alias("Amt")]
        public decimal Amount { get; set; }

        [ApiMember(Description = "报价单主表ID",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        [Alias("sid")]
        public long OfferID { get; set; }

        [ApiMember(Description = "报价单编码",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        [Alias("snum")]
        public string OfferSnum { get; set; }

        [ApiMember(Description = "报价截止日期",
        ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 8)]
        public DateTime SFinalDateTime { get; set; }

        [ApiMember(Description = "报价时间",
        ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 9)]
        public DateTime CreateDate { get; set; }

        [ApiMember(Description = "询价单主表",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public long MID { get; set; }
    }
}
