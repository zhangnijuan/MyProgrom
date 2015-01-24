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
    [Api("恩维协同平台搜索客户列表接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/customer/search", HttpMethods.Post, Notes = "自定义查询")]
    [DataContract]
    public class SearchCustomerRequest : IReturn<SearchCustomerResponse>
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
        public List<SearchCustomerField> SearchCondition { get; set; }
        [ApiMember(Description = "排序规则",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        public string Order { get; set; }

        [ApiMember(Description = "帐套",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 7)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
    }
    [DataContract]
    public class SearchCustomerResponse
    {
        public SearchCustomerResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<CustomerInfo> Data { get; set; }
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回客户信息
    /// </summary>
    [DataContract]
    public class CustomerInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// CompID
        /// </summary>
        [Alias("comp")]
        [DataMember(Order = 1)]
        public long CompID { get; set; }

        /// <summary>
        /// 云ID
        /// </summary>
        [Alias("comp_c")]
        [DataMember(Order = 2)]
        public string CorpNum { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Alias("comp_n")]
        [DataMember(Order = 4)]
        public string CompName { get; set; }
        /// <summary>
        /// 主营产品分类
        /// </summary>
        [DataMember(Order = 5)]
        public List<MainProducts> MainProduct { get; set; }

        /// <summary>
        ///  地址
        /// </summary>
        [Alias("address")]
        [DataMember(Order = 6)]
        public string Address { get; set; }

        /// <summary>
        ///  状态
        /// </summary>
        [Alias("state")]
        [DataMember(Order = 6)]
        public int State { get; set; }

        /// <summary>
        /// 企业标识
        /// </summary>
        [Alias("a")]
        [DataMember(Order = 1)]
        public long AccountID { get; set; }
    }

    /// <summary>
    /// 查询字段
    /// </summary>
    [DataContract]
    public class SearchCustomerField
    {
        [DataMember(Order = 1)]
        public SearchCustomerEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
    /// <summary>
    /// 查询条件枚举
    /// </summary>
    [DataContract]
    public enum SearchCustomerEnum
    {
        /// <summary>
        /// 公司名称或者代码
        /// </summary>
        CompNameOrCode,
        /// <summary>
        /// 地址
        /// </summary>
        Address,
        /// <summary>
        /// 状态
        /// </summary>
        State
    }
}
