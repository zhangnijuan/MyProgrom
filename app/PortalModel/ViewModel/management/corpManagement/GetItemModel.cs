using System;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取平台标准产品档案接口")]
    [Route("/{AppKey}/{Secretkey}/item/search", HttpMethods.Post, Notes = "自定义条件查询平台标准产品(含分页)or根据ID获取平台标准产品")]
    [DataContract]
    public class GetItemRequest : IReturn<GetItemResponse>
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

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [ApiMember(Description = "产品名称或代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string ItemInfo { get; set; }

        [ApiMember(Description = "最底层产品分类ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string CategoryID { get; set; }

        [ApiMember(Description = "最底层产品分类代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string CategoryCode { get; set; }

        [ApiMember(Description = "最底层产品分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string CategoryName { get; set; }

        [ApiMember(Description = "产品ID(根据ID获取标准产品Post方法)",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 9)]
        public long ID { get; set; }
    }
    [DataContract]
    public class GetItemResponse
    {
        public GetItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "产品资料集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<ItemView> Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回的产品资料
    /// </summary>
    [DataContract]
    public class ItemView : IReturn<GetItemResponse>
    {
        /// <summary>
        /// 物品id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [DataMember(Order = 3)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Order = 4)]
        public string ItemName { get; set; }

        /// <summary>
        /// 一级产品分类id
        /// </summary>
        [DataMember(Order = 5)]
        public string Category1ID { get; set; }

        /// <summary>
        /// 一级产品分类代码
        /// </summary>
        [DataMember(Order = 6)]
        public string Category1Code { get; set; }

        /// <summary>
        /// 一级产品分类名称
        /// </summary>
        [DataMember(Order = 7)]
        public string Category1Name { get; set; }

        /// <summary>
        /// 二级产品分类id
        /// </summary>
        [DataMember(Order = 8)]
        public string Category2ID { get; set; }

        /// <summary>
        /// 二级产品分类代码
        /// </summary>
        [DataMember(Order = 9)]
        public string Category2Code { get; set; }

        /// <summary>
        /// 二级产品分类名称
        /// </summary>
        [DataMember(Order = 10)]
        public string Category2Name { get; set; }

        /// <summary>
        /// 三级产品分类id
        /// </summary>
        [DataMember(Order = 11)]
        public string Category3ID { get; set; }

        /// <summary>
        /// 三级产品分类代码
        /// </summary>
        [DataMember(Order = 12)]
        public string Category3Code { get; set; }

        /// <summary>
        /// 三级产品分类名称
        /// </summary>
        [DataMember(Order = 13)]
        public string Category3Name { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        [DataMember(Order = 14)]
        public string UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 15)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 16)]
        public string UnitName { get; set; }

        /// <summary>
        /// 标准售价
        /// </summary>
        [DataMember(Order = 17)]
        public string SalPrc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 18)]
        public string Remark { get; set; }

        /// <summary>
        /// 平台标准属性值集合
        /// </summary>
        [DataMember(Order = 19)]
        public IList<ItemAttributeView> ItemAttributeViewList { get; set; }
    }

    /// <summary>
    /// 平台标准属性值集合
    /// </summary>
    [DataContract]
    public class ItemAttributeView : IReturn<ItemView>
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 属性类
        /// </summary>
        [DataMember(Order = 3)]
        public string AttributeClass { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [DataMember(Order = 4)]
        public string AttributeValue { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        [DataMember(Order = 5)]
        public string ItemID { get; set; }

        /// <summary>
        /// 属性单位
        /// </summary>
        [DataMember(Order = 6)]
        public string UnitName { get; set; }
    }
}
