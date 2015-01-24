using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据ID查询企业产品接口")]
    [Route("/{AccountId}/compitem/{ID}", HttpMethods.Get, Notes = "根据ID查询企业产品信息")]
    [DataContract]
    public class GetCompItemByIDRequest : IReturn<GetCompItemByIDResponse>
    {
        //全网获取物品无需登录，暂时去掉key bai
      //  [DataMember(Order = 1)]
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        [DataMember(Order = 1)]
        [ApiMember(Description = "账套Id",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "企业产品ID",
      ParameterType = "path", DataType = "long", IsRequired = true)]    
        public long ID { get; set; }
    }
    [DataContract]
    public class GetCompItemByIDResponse
    {
        public GetCompItemByIDResponse()
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
        public CompItemList Data { get; set; }
    }
    [DataContract]
    public class CompItemList     //: IReturn<GetCompItemByIDResponse>
    {
        [DataMember(Order = 1)]
        public long ID { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountId { get; set; }
        /// <summary>
        /// 平台产品代码
        /// </summary>
        [DataMember(Order = 3)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台产品名称
        /// </summary>
        [DataMember(Order = 4)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [DataMember(Order = 5)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Order = 6)]
        public string ItemName { get; set; }

        /// <summary>
        /// 一级产品分类id
        /// </summary>
         [DataMember(Order = 7)]
        public long Category1ID { get; set; }

        /// <summary>
        /// 一级产品分类代码
        /// </summary>
         [DataMember(Order = 8)]
        public string Category1Code { get; set; }

        /// <summary>
        /// 一级产品分类名称
        /// </summary>
         [DataMember(Order = 9)]
        public string Category1Name { get; set; }

        /// <summary>
        /// 二级产品分类id
        /// </summary>
         [DataMember(Order = 10)]
        public long Category2ID { get; set; }

        /// <summary>
        /// 二级产品分类代码
        /// </summary>
        [DataMember(Order = 11)]
        public string Category2Code { get; set; }

        /// <summary>
        /// 二级产品分类名称
        /// </summary>
         [DataMember(Order = 12)]
        public string Category2Name { get; set; }

        /// <summary>
        /// 三级产品分类id
        /// </summary>
         [DataMember(Order = 13)]
        public long Category3ID { get; set; }

        /// <summary>
        /// 三级产品分类代码
        /// </summary>
         [DataMember(Order = 14)]
        public string Category3Code { get; set; }

        /// <summary>
        /// 三级产品分类名称
        /// </summary>
         [DataMember(Order = 15)]
        public string Category3Name { get; set; }

         /// <summary>
         /// 产品描述
         /// </summary>
         [DataMember(Order = 16)]
         public string Remark { get; set; }

         /// <summary>
         /// 参考价格(市场价)
         /// </summary>
         [DataMember(Order = 17)]
         public decimal SalPrc { get; set; }

         /// <summary>
         /// 单位id
         /// </summary>
         [DataMember(Order = 18)]
         public long UnitID { get; set; }

         /// <summary>
         /// 单位代码
         /// </summary>
         [DataMember(Order = 19)]
         public string UnitCode { get; set; }

         /// <summary>
         /// 单位名称
         /// </summary>
         [DataMember(Order = 20)]
         public string UnitName { get; set; }

         /// <summary>
         /// 价格说明
         /// </summary>
         [DataMember(Order = 21)]
         public string PriceDes { get; set; }

         /// <summary>
         /// 图片资源外键
         /// </summary>
         [DataMember(Order = 22)]
         public long ItemResources { get; set; }

         /// <summary>
         /// 详细说明
         /// </summary>
         [DataMember(Order = 23)]
         public string Description { get; set; }

         /// <summary>
         /// 联系人
         /// </summary>
         [DataMember(Order = 24)]
         public string Contacts { get; set; }

         /// <summary>
         /// 联系人手机号
         /// </summary>
         [DataMember(Order = 25)]
         public string ContactsTel { get; set; }

         /// <summary>
         /// 付款方式说明
         /// </summary>
         [DataMember(Order = 26)]
         public string Payment { get; set; }

         /// <summary>
         /// 所在区
         /// </summary>
         [DataMember(Order = 27)]
         public string District { get; set; }

         /// <summary>
         /// 所在市
         /// </summary>
         [DataMember(Order = 28)]
         public string City { get; set; }

         /// <summary>
         /// 省
         /// </summary>
         [DataMember(Order = 29)]
         public string Province { get; set; }

         /// <summary>
         /// 地址
         /// </summary>
         [DataMember(Order = 30)]
         public string Address { get; set; }

         /// <summary>
         /// 业务分类
         /// 0 不分类
         /// 1 采购 
         /// 2 销售
         /// </summary>
         [DataMember(Order = 31)]
         public int BizType { get; set; }

         /// <summary>
         /// 物品状态
         /// </summary>
         [DataMember(Order = 32)]
         public int State { get; set; }

         /// <summary>
         /// 物品状态_枚举
         /// 0 草稿 1 已发布 2 已下架
         /// </summary>
         [DataMember(Order = 33)]
         public string StateEnum { get; set; }

         /// <summary>
         /// 建档日期
         /// </summary>
         [DataMember(Order = 34)]
         public DateTime CreateDate { get; set; }

         /// <summary>
         /// 修改日期
         /// </summary>
         [DataMember(Order = 35)]
         public DateTime ModifyDate { get; set; }


        /// <summary>
        /// 属性明细对象
        /// </summary>
        [DataMember(Order = 36)]
         public List<AttributeList> ItemAttributeViewList { get; set; }

        /// <summary>
        /// 价格明细表
        /// </summary>
        [DataMember(Order = 37)]
        public List<PriceList> DataPrice { get; set; }

        /// <summary>
        /// 产品质量认证集合
        /// 同一产品可以被多家认证机构认证
        /// </summary>
        [DataMember(Order = 38)]
        public List<ItemCertificationInfo> ItemCertificationInfo { get; set; }

        /// <summary>
        /// 图片资源信息
        /// </summary>
        [DataMember(Order = 39)]
        public List<Ndtech.PortalModel.ReturnPicResources> PicResourcesList { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember(Order = 40)]
        public string AddressID { get; set; }

        [ApiMember(Description = "是否认证1已认证 0未认证",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 41)]
        public int IsCertification { get; set; }

    }

    [DataContract]
    public class AttributeList   //: IReturn<GetCompItemByIDResponse>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }

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
        /// 产品ID
        /// </summary>
        [DataMember(Order = 5)]
        public long ItemID { get; set; }

        /// <summary>
        /// 属性单位
        /// add 2015/01/14
        /// </summary>
        [DataMember(Order = 6)]
        public string UnitName { get; set; }
    }

    [DataContract]
    public class PriceList
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        [DataMember(Order = 3)]
        public long ItemID { get; set; }

        /// <summary>
        /// 标准价格
        /// </summary>
        [DataMember(Order = 4)]
        public Decimal Price { get; set; }

        /// <summary>
        /// 定价时间
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime PriceStartDate { get; set; }
 
    }
    [DataContract]
    public class CertificationList
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 物品ID
        /// </summary>
        [DataMember(Order = 3)]
        public long ItemID { get; set; }

        /// <summary>
        /// 认证企业名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CertificationComp { get; set; }

        /// <summary>
        /// 企业认证资源ID
        /// </summary>
        [DataMember(Order = 5)]
        public long CertificationResources { get; set; }

        /// <summary>
        /// 资源明细
        /// </summary>
        [DataMember(Order = 6)]
        public List<Ndtech.PortalModel.ReturnPicResources> DataResources { get; set; }

    }

    //[DataContract]
    //public class AddressList
    //{
    //    /// <summary>
    //    /// 主键
    //    /// </summary>
    //    [DataMember(Order = 1)]
    //    public long ID { get; set; }

    //    /// <summary>
    //    /// 帐套
    //    /// </summary>
    //    [DataMember(Order = 2)]
    //    public int AccountID { get; set; }

    //    /// <summary>
    //    /// 企业ID
    //    /// </summary>
    //    [DataMember(Order = 3)]
    //    public long CompID { get; set; }

    //    /// <summary>
    //    /// 企业名称
    //    /// </summary>
    //    [DataMember(Order = 4)]
    //    public string CompName { get; set; }

    //    /// <summary>
    //    /// 省
    //    /// </summary>
    //    [DataMember(Order = 5)]
    //    public string Province { get; set; }

    //    /// <summary>
    //    /// 市
    //    /// </summary>
    //    [DataMember(Order = 6)]
    //    public string City { get; set; }

    //    /// <summary>
    //    /// 所在地区
    //    /// </summary>
    //    [DataMember(Order = 7)]
    //    public string District { get; set; }

    //    /// <summary>
    //    /// 详细地址
    //    /// </summary>
    //    [DataMember(Order = 8)]
    //    public string Address { get; set; }

    //    /// <summary>
    //    /// 邮编
    //    /// </summary>
    //    [DataMember(Order = 9)]
    //    public string Zipcode { get; set; }

    //    /// <summary>
    //    /// 0 否 1 是
    //    /// </summary>
    //    [DataMember(Order = 10)]
    //    public int Isdef { get; set; }
    //}

}
