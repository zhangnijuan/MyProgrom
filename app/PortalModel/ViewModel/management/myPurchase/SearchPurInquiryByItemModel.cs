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
    [Api("恩维协同询价搜索接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/inquiry/search", HttpMethods.Post, Notes = "搜索询价信息")]
    [Route("/web/inquiry/search", HttpMethods.Post, Notes = "全网查询询价信息")]
    [Route("/web/inquiry/search/{AccountId}", HttpMethods.Post, Notes = "企业查询询价信息")]
    [DataContract]
    public class SearchPurInquiryByItemRequest : IReturn<PurInquiryByItemResponse>
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
        public List<SearchField> SearchCondition { get; set; }

        [ApiMember(Description = "属性条件",                                                                                                                                                                                                                                                                                     
                     ParameterType = "json", DataType = "list")]
        [DataMember(Order = 6)]
        public List<SearchAttribute> AttCondition { get; set; }
        [ApiMember(Description = "排序",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 7)]
        public List<Order> orders { get; set; }
        [ApiMember(Description = "采购信息收藏",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        public string SubscribeInquiry { get; set; }

        [ApiMember(Description = "报价方云ID",
     ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        public string QuotationCompNumber { get; set; }

        [ApiMember(Description = "是否报价",
ParameterType = "json", DataType = "bool")]
        [DataMember(Order = 10)]
        public bool AlreadyQutation { get; set; }

        [ApiMember(Description = "是否是查询所有（true：表示查询所有）",
                 ParameterType = "json", DataType = "bool", IsRequired = true)]
        [DataMember(Order = 11)]
        public bool IsSearchAll { get; set; }
        [ApiMember(Description = "是否是从企业门户查询",
                ParameterType = "json", DataType = "bool", IsRequired = true)]
        [DataMember(Order = 12)]
        public bool IsPortal { get; set; }

    }
    [DataContract]
    public class AttributeCondition
    {
        [ApiMember(Description = "成员名称",
                 ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string MemberName { get; set; }
        [ApiMember(Description = "成员值",
         ParameterType = "json", DataType = "string")]
        [DataMember(Order = 2)]
        public string MemberValue { get; set; }
        [ApiMember(Description = "比较类型",
        ParameterType = "json", DataType = "string")]
        [DataMember(Order = 3)]
        public string CompareType { get; set; }
    }
    [DataContract]
    public class PurInquiryByItemResponse
    {
        public PurInquiryByItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public List<PurInquiryList> Data { get; set; }
        [DataMember(Order = 2)]
        public long RowsCount { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 4)]
        public bool Success { get; set; }
    }
    /// <summary>
    /// 返回给用户的数据
    /// </summary>
    [DataContract]
    public class PurInquiryList : IReturn<PurInquiryByItemResponse>
    {

        [ApiMember(Description = "标示id",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }
        [ApiMember(Description = "询价主题",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        [Alias("subject")]
        public string Subject { get; set; }
        [ApiMember(Description = "询价发布日期",
          ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 3)]
        [Alias("createtime")]
        public DateTime CreateTime { get; set; }
        [ApiMember(Description = "报价截止日期",
                  ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 4)]
        [Alias("finaldateTime")]
        public DateTime FinalDateTime { get; set; }
        [ApiMember(Description = "公司名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        [Alias("compname")]
        public string CompName { get; set; }
        [ApiMember(Description = "收货地代码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        [Alias("address")]
        public string AddressCode { get; set; }
        [ApiMember(Description = "建档人名称",
  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        [Alias("eid_usrname")]
        public string EidName { get; set; }
        [ApiMember(Description = "报价数",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 8)]
        [Alias("quotations")]
        public int quotations { get; set; }

        [ApiMember(Description = "询价代码",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        [Alias("c")]
        public string InquiryCode { get; set; }

        [ApiMember(Description = "状态",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 10)]
        [Alias("state")]
        public int State { get; set; }
        [ApiMember(Description = "发布状态",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 11)]
        [Alias("anonymouscode")]
        public int AnonymousCode { get; set; }
        [ApiMember(Description = "账套",
  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 12)]
        [Alias("a")]
        public int AccountID { get; set; }

        [ApiMember(Description = "云Id",
 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        [Alias("corpnum")]
        public string CorpNum { get; set; }
        /// <summary>
        /// 是否已经收藏
        /// </summary>
        [DataMember(Order = 14)]
        public int Substate { get; set; }

    }
     [DataContract]
    public class SearchPurItem
    { 
    
        [ApiMember(Description = "产品类别ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long CategoryID { get; set; }

        [ApiMember(Description = "产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string CategoryCode { get; set; }

        [ApiMember(Description = "产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string CategoryName { get; set; }

        [ApiMember(Description = "产品类别父ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 13)]
        public long CategoryMID { get; set; }
    }
}
