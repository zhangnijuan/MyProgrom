using ServiceStack.Common.Web;
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
    [Api("恩维协同平台标准分类搜索接口")]
    //[Route("/web/compitem/search", HttpMethods.Post, Notes = "全网查询企业产品")]
    [Route("/web/enterprise/category/second/search", HttpMethods.Post, Notes = "查询企业产品二级及三级分类")]
    [DataContract]
    public class SearchCategoryRequest : IReturn<SearchCategoryResponse>
    {
      //  [DataMember(Order = 1)]
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        [DataMember(Order =2)]
        [ApiMember(Description = "账套Id",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "字段条件",
      ParameterType = "json", DataType = "List", IsRequired = true)]

        public List<SearchCategoryField> SearchCondition { get; set; }

    }
    [DataContract]
    public class SearchCategoryResponse
    {
        public SearchCategoryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        //public CategoryInfoList Data { get; set; }
        public List<CategorySecondInfo> Data { get; set; }
 
    }
    /// <summary>
    /// 查询字段（除属性）
    /// </summary> 
    [DataContract]
    public class SearchCategoryField
    {
        [DataMember(Order = 1)]
        public SearchCategoryEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }

    }
    /// <summary>
    /// 查询字段枚举 平台标准产品类别表中，类别名称或类别编码
    /// </summary> 
    [DataContract]
    public enum SearchCategoryEnum
    {
        [Description("类别名称或类别编码")]
        CategoryCode,
        //[Description("商品名称或代码")]
        //ItemorCode
    }

    /// <summary>
    /// 二级分类
    /// </summary>
    [DataContract]
    public class CategorySecondInfo
    {
        /// <summary>
        /// 产品分类id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 上级分类id
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentID { get; set; }

        /// <summary>
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 5)]
        public string CategoryName { get; set; }
        
        /// <summary>
        /// 三级分类
        /// </summary>
        [DataMember(Order = 6)]
        public List<CategoryThirdInfo> DataThird { get; set; }
 
    }
    [DataContract]
    public class CategoryThirdInfo
    {
        /// <summary>
        /// 产品分类id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 上级分类id
        /// </summary>
        [DataMember(Order = 3)]
        public string ParentID { get; set; }

        /// <summary>
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 5)]
        public string CategoryName { get; set; }
    }
}
