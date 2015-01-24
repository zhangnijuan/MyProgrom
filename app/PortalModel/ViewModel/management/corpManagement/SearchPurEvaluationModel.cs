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
    [Api("恩维协同平台搜索评价列表接口")]
    [Route("/{AppKey}/{Secretkey}/evaluation/search", HttpMethods.Post, Notes = "自定义查询")]
    [DataContract]
    public class SearchPurEvaluationRequest : IReturn<SearchPurEvaluationResponse>
    {
        [ApiMember(Description = "第几页",
                 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }
        [ApiMember(Description = "查询条件",
  ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 3)]
        public List<SearchPurEvaluationField> SearchCondition { get; set; }
        [ApiMember(Description = "排序规则",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        public string Order { get; set; }

        [ApiMember(Description = "对方企业标识",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int OtherAccountID { get; set; }

        [ApiMember(Description = "评价企业标识",
     ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }

        [ApiMember(Description = "供应商云ID",
       ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CorpNum { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 7)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "评价类型 1 登录人对对方 2 对方对登录人",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int EvaluationType { get; set; }

        [ApiMember(Description = "身份类型 1 采购商 2 供应商",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int IdentityType { get; set; }
    }
    [DataContract]
    public class SearchPurEvaluationResponse
    {
        public SearchPurEvaluationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<PurEvaluationInfo> Data { get; set; }
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回评价信息
    /// </summary>
    [DataContract]
    public class PurEvaluationInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 评价单号
        /// </summary>
        [Alias("c")]
        [DataMember(Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        [Alias("createtime")]
        [DataMember(Order = 2)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 评价人
        /// </summary>
        [Alias("ename")]
        [DataMember(Order = 4)]
        public string Ename { get; set; }

        /// <summary>
        ///  订单号
        /// </summary>
        [Alias("ordercode")]
        [DataMember(Order = 6)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 评价分数
        /// </summary>
        [Alias("level")]
        [DataMember(Order = 4)]
        public int Level { get; set; }
    }

    /// <summary>
    /// 查询字段
    /// </summary>
    [DataContract]
    public class SearchPurEvaluationField
    {
        [DataMember(Order = 1)]
        public SearchPurEvaluationEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
    /// <summary>
    /// 查询条件枚举
    /// </summary>
    [DataContract]
    public enum SearchPurEvaluationEnum
    {
        /// <summary>
        /// 评价开始日期
        /// </summary>
        BeginDateTime,
        /// <summary>
        /// 评价结束日期
        /// </summary>
        EndDateTime,
        /// <summary>
        /// 订单号或评价单号
        /// </summary>
        OrderCodeOrEvaluationCode

    }
}
