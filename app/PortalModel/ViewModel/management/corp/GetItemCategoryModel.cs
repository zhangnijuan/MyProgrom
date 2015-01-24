using System;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品分类接口")]
    [Route("/{AppKey}/{Secretkey}/category/search/{ParentID}", HttpMethods.Get, Notes = "根据上级分类ID获取产品分类信息")]
    [Route("/{AppKey}/{Secretkey}/category/search/{LikeSearch}/{CategoryName}", HttpMethods.Get, Notes = "根据分类名称模糊搜索产品分类")]
    [DataContract]
    public class GetItemCategoryRequest : IReturn<GetItemCategoryResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                 ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "上级分类ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string ParentID { get; set; }

        [ApiMember(Description = "根据名称模糊搜索分类资料(传Y即可)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string LikeSearch { get; set; }

        [ApiMember(Description = "分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CategoryName { get; set; }
    }

    [DataContract]
    public class GetItemCategoryResponse
    {
        public GetItemCategoryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "产品分类集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<Category> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回产品分类层级关系集合
    /// </summary>
    [DataContract]
    public class Category : IReturn<GetItemCategoryResponse>
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
        /// 顶级产品分类资料
        /// </summary>
        [DataMember(Order = 6)]
        public ItemCategory CategoryFirst { get; set; }

        /// <summary>
        /// 第二层产品分类资料
        /// </summary>
        [DataMember(Order = 7)]
        public ItemCategory CategorySecond { get; set; }

        /// <summary>
        /// 最底层产品分类资料
        /// </summary>
        [DataMember(Order = 8)]
        public ItemCategory CategoryThird { get; set; }
    }

    /// <summary>
    /// 返回的产品分类资料
    /// </summary>
    [DataContract]
    public class ItemCategory : IReturn<Category>
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
