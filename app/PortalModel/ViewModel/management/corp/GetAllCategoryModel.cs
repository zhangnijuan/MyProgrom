using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取所有平台标准产品分类接口")]
    [Route("/web/category/search", HttpMethods.Get, Notes = "平台标准产品分类")]
    [DataContract]
    public class GetAllCategoryRequest : IReturn<GetCategoryResponse>
    {
    }

    [DataContract]
    public class GetCategoryResponse
    {
        public GetCategoryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "平台产品分类集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<CategoryFirst> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 产品分类一二三层级资料
    /// </summary>
    [DataContract]
    public class CategoryFirst
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
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 3)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        [DataMember(Order = 5)]
        public string ParentID { get; set; }

        /// <summary>
        /// 二三层级资料
        /// </summary>
        [DataMember(Order = 6)]
        public List<CategorySecond> CategorySecond { get; set; }
    }

    /// <summary>
    /// 产品分类二三层级资料
    /// </summary>
    [DataContract]
    public class CategorySecond
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
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 3)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        [DataMember(Order = 5)]
        public string ParentID { get; set; }

        /// <summary>
        /// 三级分类资料
        /// </summary>
        [DataMember(Order = 6)]
        public List<CategoryThird> CategoryThird { get; set; }
    }

    /// <summary>
    /// 产品分类三层级资料
    /// </summary>
    [DataContract]
    public class CategoryThird
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
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 3)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        [DataMember(Order = 5)]
        public string ParentID { get; set; }
    }
}
