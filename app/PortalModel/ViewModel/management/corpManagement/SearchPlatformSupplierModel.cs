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
    [Api("恩维协同平台搜索供应商列表接口")]
    [Route("/web/enterprise/search", HttpMethods.Post, Notes = "自定义查询")]
    [DataContract]
    public  class SearchPlatformSupplierRequest:IReturn<SearchPlatformSupplierResponse>
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
        public List<SearchSupplierField> SearchCondition { get; set; }
        [ApiMember(Description = "排序规则",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        public string Order { get; set; }

        [ApiMember(Description = "企业类型 Supplier 供应商 Purchaser 采购商",
          ParameterType = "json", DataType = "EnterpriseEnum")]
        [DataMember(Order = 5)]
        public EnterpriseEnum EnterpriseType { get; set; }

        [ApiMember(Description = "登录者的账套",
                 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        public int AccountID { get; set; }
    }
    [DataContract]
    public class SearchPlatformSupplierResponse
    {
        public SearchPlatformSupplierResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<SupplierInfo> Data { get; set; }
        [DataMember(Order = 3)]
        public int  RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    /// <summary>
    /// 返回供应商信息
    /// </summary>
    [DataContract]
    public class SupplierInfo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// 云ID
        /// </summary>
        [Alias("corpnum")]
        [DataMember(Order = 2)]
        public string CorpNum { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Alias("n")]
        [DataMember(Order = 4)]
        public string CompName { get; set; }
        /// <summary>
        /// 主营产品分类
        /// </summary>
        [DataMember(Order = 5)]
        public List<MainProducts> MainProduct { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        [Alias("scale")]
        [DataMember(Order = 6)]
        public string CompanyScale { get; set; }
        /// <summary>
        /// 发布产品数量
        /// </summary>
        [Alias("releases")]
        [DataMember(Order = 7)]
        public int Releases { get; set; }

        /// <summary>
        /// 发布询价数量
        /// </summary>
        [Alias("inquirynumber")]
        [DataMember(Order = 7)]
        public int InquiryNumber { get; set; }

        /// <summary>
        ///订阅时间
        /// </summary>
        [Alias("subdatetime")]
        [DataMember(Order = 8)]
        public DateTime SubDateTime { get; set; }
        /// <summary>
        /// 是否已经收藏
        /// </summary>
        [DataMember(Order = 9)]
        public int SubState { get; set; }
    }
    [DataContract]
    public class MainProducts
    {

        /// <summary>
        /// 产品类别名称
        /// </summary>
        [Alias("n")]
        [DataMember(Order = 1)]
        public string CategoryName { get; set; }
        
    }
    /// <summary>
    /// 查询字段
    /// </summary>
    [DataContract]
    public class SearchSupplierField
    {
        [DataMember(Order = 1)]
        public SearchSupplierEnum SeacheKey { get; set; }
         [DataMember(Order = 2)]
        public List<string> Value { get; set; }
    }
    /// <summary>
    /// 查询条件枚举
    /// </summary>
     [DataContract]
    public enum SearchSupplierEnum
    {
         /// <summary>
        /// 公司名称，主营产品或者云ID
         /// </summary>
         CompNameOrProductOrCorpnum,
         /// <summary>
         /// 公司规模
         /// </summary>
        CompanyScale,
         /// <summary>
         /// 公司性质
         /// </summary>
        CompNature,
         /// <summary>
         /// 主营产品
         /// </summary>
         MainProduct
      
    }

     /// <summary>
     /// 查询条件枚举
     /// </summary>
     [DataContract]
     public enum EnterpriseEnum
     {
         /// <summary>
         /// 供应商
         /// </summary>
         Supplier,
         /// <summary>
         /// 采购商
         /// </summary>
         Purchaser
        
     }
}
