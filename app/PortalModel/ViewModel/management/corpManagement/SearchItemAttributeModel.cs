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
    [Api("恩维协同根据平台三级分类代码获取属性值列表接口")]
    [Route("/web/{SearchType}/product/search/condition", HttpMethods.Post, Notes = "根据三级分类代码获取属性值列表")]
    [DataContract]
    public class SearchItemAttributeRequest : IReturn<SearchItemAttributeResponse>
    {
      //  [DataMember(Order = 1)]
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        //  [DataMember(Order = 3)]
        //  [ApiMember(Description = "账套Id",
        //ParameterType = "json", DataType = "int", IsRequired = true)]
        //  public int AccountId { get; set; }
      //  [DataMember(Order = 3)]
      //  [ApiMember(Description = "分类名称",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string CategoryName { get; set; }


        [DataMember(Order = 1)]
        [ApiMember(Description = "字段条件",
      ParameterType = "json", DataType = "List", IsRequired = true)]

        public List<SearchAttributeValue> SearchCondition { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "查询类型 企业属性 = enterprise，平台属性 = platform",
      ParameterType = "json", DataType = "SearchType", IsRequired = true)]
        public SearchType SearchType { get; set; }
    }
    [DataContract]
    public class SearchItemAttributeResponse
    {
        public SearchItemAttributeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public List<CategoryAttributeClass> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }

    }
    /// <summary>
    /// 平台标准产品分类属性类
    /// </summary>
    [DataContract]
    public class CategoryAttributeClass
    {
        [DataMember(Order = 0)]
        public string ClassName { get; set; }

        [DataMember(Order = 1)]
        public int IsScope { get; set; }

        [DataMember(Order = 2)]
        public List<CategoryAttributeValue> ClassValueList { get; set; }
    }
    /// <summary>
    /// 可搜索范围值
    /// </summary>
    [DataContract]
    public class CategoryAttributeValue
    {
        /// <summary>
        /// 最小值(范围区间取值)
        /// </summary>
        [DataMember(Order = 0)]
        public int Start { get; set; }

        /// <summary>
        /// 最大值(范围区间取值)
        /// </summary>
        [DataMember(Order = 1)]
        public int End { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [DataMember(Order = 3)]
        public string Unit { get; set; }

        /// <summary>
        /// 固定值(非范围区间取值)
        /// </summary>
        [DataMember(Order = 4)]
        public string ClassValue { get; set; }
    }


    /// <summary>
    /// 查询字段（除属性）
    /// </summary> 
    [DataContract]
    public class SearchAttributeValue
    {
        [DataMember(Order = 1)]
        public SearchAttributeEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }

    }
    /// <summary>
    /// 查询字段枚举 平台标准产品类别表中，类别名称或类别编码
    /// </summary> 
    [DataContract]
    public enum SearchAttributeEnum
    {
        [Description("分类代码")]
        CategoryCode,

    }
}
