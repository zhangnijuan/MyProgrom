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
    [Api("恩维协同搜索订单列表接口")]

    [Route("/{AppKey}/{Secretkey}/{AccountID}/{counterparty}/purorder/search", HttpMethods.Post, Notes = "自定义查询")]
    [DataContract]
    public class SearchOrderRequest : IReturn<SearchOrderResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "第几页",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }
        [ApiMember(Description = "当前用户ID",
             ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public string EID { get; set; }
        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }
        [ApiMember(Description = "订单类型（采购(Purchase)或者销售(Sell)）", ParameterType = "json", DataType = "CounterPartyEnum", IsRequired = true)]
        [DataMember(Order = 6)]
        public CounterPartyEnum CounterParty { get; set; }
        [ApiMember(Description = "查询条件(State:订单状态,BeginDateTime:开始时间,EndDateTime:截止时间,SearchName:搜索框的数据,OrderBy:排序规则（asc:升序，desc:降序）)",
 ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 7)]
        public List<SearchOrderFild> SearchCondition { get; set; }

        [ApiMember(Description = "是否是查询所有（true：表示查询所有）",
                 ParameterType = "json", DataType = "bool", IsRequired = true)]
        [DataMember(Order = 8)]
        public bool IsSearchAll { get; set; }
    }
    [DataContract]
    public class SearchOrderResponse
    {
        public SearchOrderResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<ProductOrder> Data { get; set; }
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    /// <summary>
    /// 返回数据
    /// </summary>
    public class ProductOrder
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Alias("c")]
        [DataMember(Order = 2)]
        public string OrderCode { get; set; }
        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createdate")]
        [DataMember(Order = 3)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Alias("su_n")]
        [DataMember(Order = 4)]
        public string SupplyName { get; set; }

        /// <summary>
        /// 合计信息
        /// </summary>
        [Alias("totalamt")]
        [DataMember(Order = 5)]
        public decimal TotalAmt { get; set; }
        /// <summary>
        /// 采购人姓名
        /// </summary>
        [Alias("eid_usrname")]
        [DataMember(Order = 6)]
        public string EIDName { get; set; }
        /// <summary>
        /// 单证状态 
        /// </summary>
        [Alias("state_enum")]
        [DataMember(Order = 7)]
        public string State_Enum { get; set; }
        /// <summary>
        /// 供方联系人名称
        /// </summary>
        [Alias("linkmanname")]
        [DataMember(Order = 8)]
        public string LinkManName { get; set; }
        /// <summary>
        /// 采购方公司名称
        /// </summary>
        [Alias("n")]
        [DataMember(Order = 9)]
        public string CompName { get; set; }
        /// <summary>
        /// 单证状态 0 报价草稿 1 已报价 2 已优选 3 已下单 4已发货 5 关闭
        /// </summary>
        [Alias("state")]
        [DataMember(Order = 10)]
        public int State { get; set; }
        /// <summary>
        /// 评价状态
        /// </summary>
        [Alias("evaluation")]
        [DataMember(Order = 11)]
        public int Evaluation { get; set; }
        /// <summary>
        /// 已到货状态
        /// </summary>
        [Alias("arrivalstate")]
        [DataMember(Order = 12)]
        public int ArrivalState { get; set; }

        /// <summary>
        /// 已收款状态
        /// </summary>
        [Alias("receivingstate")]
        [DataMember(Order = 13)]
        public int ReceivingState { get; set; }
        /// <summary>
        /// 云ID
        /// </summary>
        [DataMember(Order = 14)]
        [Alias("corpnum")]
        public string CorpNum { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        [DataMember(Order = 15)]
        public int AccountID { get; set; }
        [Alias("orderdate")]
        [DataMember(Order = 16)]
        public DateTime OrderTime { get; set; }
        [Alias("sa")]
        [DataMember(Order = 17)]
        public int SAccountID { get; set; }
  

    }
    /// <summary>
    /// 查询字段
    /// </summary>
    [DataContract]
    public class SearchOrderFild
    {

        [DataMember(Order = 1)]
        public SearchOrderEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
    /// <summary>
    /// 查询条件枚举
    /// </summary>
    [DataContract]
    public enum SearchOrderEnum
    {
        /// <summary>
        /// 查询状态
        /// </summary>
        [DataMember(Order = 1)]
        State,
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember(Order = 2)]
        BeginDateTime,
        /// <summary>
        /// 截止时间
        /// </summary>
        [DataMember(Order = 3)]
        EndDateTime,
        /// <summary>
        /// 搜索框数据采购员或者供应商名称
        /// </summary>
        [DataMember(Order = 4)]
        SearchName,
        /// <summary>
        /// 排序规则
        /// </summary>
        [DataMember(Order = 1)]
        OrderBy
    }
}
