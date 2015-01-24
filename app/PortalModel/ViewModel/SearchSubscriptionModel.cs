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
    [Api("展示订阅详细信息接口")]
    [Route("/{AppKey}/{Secretkey}/subscribe/inquiry/search", HttpMethods.Post, Notes = "展示订阅详细信息接口")]
    [DataContract]
    public class SearchSubscriptionRequest : IReturn<SearchSubscriptionResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "第几页",
                 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }
        [ApiMember(Description = "当前订阅查询的主键Id",
                ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public long ID { get; set; }
        [ApiMember(Description = "当前公司账套",
               ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        public int AccountID { get; set; }

        [ApiMember(Description = "查询条件(FromName:订阅供应商,DeliveryAddress:地址,CategoryName: 产品名称,MinQty:最小询价数量,MaxQty:最大询价数量,ItemCode:搜索框的值)",
 ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 7)]
        public List<SearchSubscriptionField> SearchCondition { get; set; }
        [ApiMember(Description = "OrderBy:排序规则（asc:升序，desc:降序）)",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<Order> OrderBy { get; set; }
        [ApiMember(Description = "Theme:按主题查询，Category：按分类查询)",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public Estimate EstimteCondition { get; set; }
        [ApiMember(Description = "分类名称",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string CategoryName { get; set; }

    }
    [DataContract]
    public class SearchSubscriptionResponse
    {
        public SearchSubscriptionResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<ReturnSearchInfo> Data { get; set; }
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    /// <summary>
    /// 查询字段
    /// </summary>
    [DataContract]
    public class SearchSubscriptionField
    {
        [DataMember(Order = 1)]
        public SearchSubscriptionEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public List<string> Value { get; set; }
    }

    /// <summary>
    /// 查询条件枚举
    /// </summary>
    [DataContract]
    public enum SearchSubscriptionEnum
    {
        /// <summary>
        /// 订阅供应商
        /// </summary>
        FromName,
        /// <summary>
        /// 地址
        /// </summary>
        DeliveryAddress,
        /// <summary>
        /// 产品名称
        /// </summary>
        CategoryName,
        /// <summary>
        /// 最大询价数量
        /// </summary>
        MaxQty,
        /// <summary>
        /// 最小询价数量
        /// </summary>
        MinQty,
        /// <summary>
        /// 文本框的值
        /// </summary>
        ItemCode
      


    }
    [DataContract]
    public class ReturnSearchInfo
    {
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// 询价代码
        /// </summary>
        [Alias("c")]
        [DataMember(Order = 2)]
        public string InquiryCode { get; set; }
        /// <summary>
        /// 询价主题
        /// </summary>
        [Alias("subject")]
        [DataMember(Order = 3)]
        public string Subject { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Alias("n")]
        [DataMember(Order = 4)]
        public string CompName { get; set; }
        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createtime")]
        [DataMember(Order = 5)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 报价截止日期
        /// </summary>
        [Alias("finaldatetime")]
        [DataMember(Order = 6)]
        public DateTime FinalDateTime { get; set; }
        /// <summary>
        /// 报价企业数量
        /// </summary>
        [Alias("quotations")]
        [DataMember(Order = 7)]
        public int Quotations { get; set; }

        /// <summary>
        /// 匿名代码
        /// </summary>
        [Alias("anonymouscode")]
        [DataMember(Order = 8)]
        public int AnonymousCode { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        [DataMember(Order = 9)]
        public int AccountID { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        [Alias("c")]
        [DataMember(Order = 10)]
        public string CompCode { get; set; }

    }
    public enum Estimate
	{
        /// <summary>
        /// 主题
        /// </summary>
	         Theme,
        /// <summary>
        /// 分类
        /// </summary>
        Category

	}
}
