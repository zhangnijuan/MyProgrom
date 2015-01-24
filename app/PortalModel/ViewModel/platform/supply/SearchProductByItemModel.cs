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
    [Api("恩维协同产品搜索接口")]
    [Route("/web/product/search", HttpMethods.Post, Notes = "全网查询企业产品")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/compitem/search", HttpMethods.Post, Notes = "查询企业产品")]
    [DataContract]
    public class SearchEnterpriseItemRequest : IReturn<ProductByItemResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "企业ID",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        [ApiMember(Description = "第几页",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public Page page { get; set; }

        [ApiMember(Description = "字段条件",
  ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 5)]
        public List<SearchField> SearchCondition { get; set; }



        [ApiMember(Description = "属性条件",
                     ParameterType = "json", DataType = "list")]
        [DataMember(Order = 6)]
        public List<SearchAttribute> AttCondition { get; set; }
        [ApiMember(Description = "排序",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 7)]
        public List<Order> orders { get; set; }
        [ApiMember(Description = "供应产品收藏",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        public string SubscribeProduct { get; set; }

        [ApiMember(Description = "商家比价标识(ComparePrice=Y为比价)",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        public string ComparePrice { get; set; }

        [ApiMember(Description = "需要查询产品质量认证logo,参数值传Y",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 10)]
        public string SearchItemCertificationFlag { get; set; }

        [ApiMember(Description = "只查询已认证机构logo,参数值传Y",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 11)]
        public string SearchHasCertificationFlag { get; set; }
    }

    [DataContract]
    public class ProductByItemResponse
    {
        public ProductByItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<ProductList> Data { get; set; }
        [DataMember(Order = 3)]
        public long RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }

    }
    /// <summary>
    /// 返回给用户的数据
    /// </summary>
    [DataContract]
    public class ProductList : IReturn<ProductByItemResponse>
    {
        [ApiMember(Description = "id",
  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }
        [ApiMember(Description = "账套id",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }
        [ApiMember(Description = "平台产品代码",
  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string StandardItemCode { get; set; }
        [ApiMember(Description = "平台物品名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string StandardItemName { get; set; }
        [ApiMember(Description = "产品代码",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string ItemCode { get; set; }
        [ApiMember(Description = "物品名称",
  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string ItemName { get; set; }
        [ApiMember(Description = "发货地",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string Address { get; set; }
        [ApiMember(Description = "所在地区",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string City { get; set; }
        [ApiMember(Description = "价格",
          ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 9)]
        public decimal SalPrc { get; set; }
        [ApiMember(Description = "公司名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        [Alias("compname")]
        public string CompName { get; set; }

        [ApiMember(Description = "一级产品类别ID",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string Category1ID { get; set; }

        [ApiMember(Description = "一级产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string Category1Code { get; set; }

        [ApiMember(Description = "一级产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        public string Category1Name { get; set; }

        [ApiMember(Description = "二级产品类别父ID",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 14)]
        public string Category2ID { get; set; }

        [ApiMember(Description = "二级产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 15)]
        public string Category2Code { get; set; }

        [ApiMember(Description = "二级产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 16)]
        public string Category2Name { get; set; }

        [ApiMember(Description = "三级产品类别ID",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 17)]
        public string Category3ID { get; set; }

        [ApiMember(Description = "三级产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 18)]
        public string Category3Code { get; set; }

        [ApiMember(Description = "三级产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 19)]
        public string Category3Name { get; set; }


        [ApiMember(Description = "属性",
         ParameterType = "json", DataType = "Attributes", IsRequired = true)]
        [DataMember(Order = 20)]
        public List<ItemAttribute> PropertyList { get; set; }
        [ApiMember(Description = "UnitID",
ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 21)]
        public long UnitID { get; set; }
        [ApiMember(Description = "UnitCode",
ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 22)]
        public string UnitCode { get; set; }
        [ApiMember(Description = "UnitName",
ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 23)]
        public string UnitName { get; set; }

        [ApiMember(Description = "产品描述",
    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 24)]
        public string Remark { get; set; }

        [ApiMember(Description = "发布时间",
            ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 25)]
        public DateTime PublishDate { get; set; }

        [ApiMember(Description = "发布状态",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 26)]
        public int State { get; set; }


        /// <summary>
        /// 省
        /// </summary>
        [DataMember(Order = 27)]
        public string Province { get; set; }

        /// <summary>
        /// 企业云ID
        /// </summary>
        [DataMember(Order = 28)]
        public string CorpNum { get; set; }

        /// <summary>
        /// 产品质量认证集合
        /// 同一产品可以被多家认证机构认证
        /// </summary>
        [DataMember(Order = 29)]
        public List<ItemCertificationInfo> ItemCertificationInfo { get; set; }
        /// <summary>
        /// 是否已经收藏
        /// </summary>
        [DataMember(Order = 30)]
        public int Substate { get; set; }
    }
    [DataContract]
    public class ItemAttribute
    {
        /// <summary>
        /// 属性类
        /// </summary>
        [ApiMember(Description = "",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [ApiMember(Description = "",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string PropertyValue { get; set; }

        /// <summary>
        /// 属性单位
        /// add 2015/01/14
        /// </summary>
        [ApiMember(Description = "",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string UnitName { get; set; }
    }

    [DataContract]
    public class ItemCertificationInfo
    {
        /// <summary>
        /// 认证机构名称
        /// </summary>
        [DataMember(Order = 1)]
        [Alias("certificationname")]
        public string CertificationName { get; set; }

        /// <summary>
        /// 认证机构帐套
        /// </summary>
        [DataMember(Order = 2)]
        [Alias("ca")]
        public int CAccountID { get; set; }

        /// <summary>
        /// 认证结果
        /// 0不合格 1合格 -1待审核
        /// -99未申请认证
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("results")]
        public int Results { get; set; }

        /// <summary>
        /// 认证logo
        /// 灰色logoid or 彩色logoid
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("picid")]
        public long CertificationPicID { get; set; }

        /// <summary>
        /// 认证明细id
        /// 点击logo查受理认证详情
        /// 平台产品详情使用
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("did")]
        public long DID { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        [DataMember(Order = 6)]
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 单证状态0未处理  1已处理 2处理中
        /// </summary>
        [DataMember(Order = 7)]
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 产品质量认证logo资源
        /// </summary>
        [DataMember(Order = 8)]
        public ReturnPicResources CertificationResources { get; set; }
    }
}
