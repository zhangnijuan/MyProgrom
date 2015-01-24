using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同新增企业产品接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/compitem/new", HttpMethods.Post, Notes = "新增企业产品(创建采购产品:BizType=1,发布供应产品BizType=2)")]
    [DataContract]
    public class NewEnterpriseItemRequest : IReturn<EnterpriseItemResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "平台产品代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "平台产品名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "产品代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string ItemName { get; set; }

        [ApiMember(Description = "一级产品分类ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 8)]
        public long Category1ID { get; set; }

        [ApiMember(Description = "一级产品分类代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string Category1Code { get; set; }

        [ApiMember(Description = "一级产品分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string Category1Name { get; set; }

        [ApiMember(Description = "二级产品分类ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 11)]
        public string Category2ID { get; set; }

        [ApiMember(Description = "二级产品分类代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string Category2Code { get; set; }

        [ApiMember(Description = "二级产品分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        public string Category2Name { get; set; }

        [ApiMember(Description = "三级产品分类ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 14)]
        public long Category3ID { get; set; }

        [ApiMember(Description = "三级产品分类代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 15)]
        public string Category3Code { get; set; }

        [ApiMember(Description = "三级产品分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 16)]
        public string Category3Name { get; set; }

        [ApiMember(Description = "产品属性值集合",
                  ParameterType = "json", DataType = "IList", IsRequired = true)]
        [DataMember(Order = 17)]
        public IList<ItemAttributeView> ItemAttributeViewList { get; set; }

        [ApiMember(Description = "产品描述",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 18)]
        public string Remark { get; set; }

        [ApiMember(Description = "单位ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 19)]
        public long UnitID { get; set; }

        [ApiMember(Description = "单位代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 20)]
        public string UnitCode { get; set; }

        [ApiMember(Description = "单位名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 21)]
        public string UnitName { get; set; }

        [ApiMember(Description = "市场价(参考价格)",
                  ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 22)]
        public decimal SalPrc { get; set; }

        [ApiMember(Description = "价格说明",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 23)]
        public string PriceDes { get; set; }

        [ApiMember(Description = "产品图片集合",
            ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 24)]
        public List<PicResources> PicResourcesList { get; set; }

        [ApiMember(Description = "详细说明",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 25)]
        public string Description { get; set; }

        [ApiMember(Description = "发货地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 26)]
        public string Address { get; set; }

        [ApiMember(Description = "发货地址ID(发货地址下拉选单选择的ID)",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 27)]
        public long AddressID { get; set; }

        [ApiMember(Description = "联系人",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 28)]
        public string Contacts { get; set; }

        [ApiMember(Description = "联系人手机号",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 29)]
        public string ContactsTel { get; set; }

        [ApiMember(Description = "付款方式说明",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 30)]
        public string Payment { get; set; }

        [ApiMember(Description = "物品状态(0 草稿 1 已发布 2 已下架)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 31)]
        public int State { get; set; }

        [ApiMember(Description = "物品状态_枚举(0 草稿 1 已发布 2 已下架)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 32)]
        public string StateEnum { get; set; }

        [ApiMember(Description = "业务类型(0不分类 1采购物品 2 供应物品)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 33)]
        public int BizType { get; set; }

        [ApiMember(Description = "省",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 34)]
        public string Province { get; set; }

        [ApiMember(Description = "市",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 35)]
        public string City { get; set; }

        [ApiMember(Description = "区",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 36)]
        public string District { get; set; }

        [ApiMember(Description = "是否认证1已认证 0未认证",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 37)]
        public int IsCertification { get; set; }
    }

    /// <summary>
    /// 返回对象
    /// </summary>
    [DataContract]
    public class EnterpriseItemResponse
    {
        public EnterpriseItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        public List<ErrorIDs> Data { get; set; }
    }

    public class ErrorIDs
    {
        public string ErrorID { get; set; }
    }
}
