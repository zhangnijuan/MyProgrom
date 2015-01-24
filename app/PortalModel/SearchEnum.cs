using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel
{
    /// <summary>
    /// 查询字段（除属性）
    /// </summary> 
    [DataContract]
    public class SearchField
    {
         [DataMember(Order = 1)]
         public SearchEnum SeacheKey { get; set; }
         [DataMember(Order = 2)]
        public string Value { get; set; }

    }
    /// <summary>
    /// 查询字段枚举 产品分类，产品名称或代码
    /// </summary> 
     [DataContract]
     public enum SearchEnum
     {
         [Description("我的单证")]
         EID,
         [Description("我以及我的下级的单证")]
         EAndLowerID,
         [Description("商品类型代码")]
         CategoryCode,
         [Description("平台标准产品代码或名称")]
         ItemorCode,
         [Description("询价主题或代码")]
         InquiryCodeorSubject,
         [Description("询价企业或主题")]
         InquiryAccountSubject,
         [Description("报价搜索开始日期")]
         BeginDateTime,
         [Description("报价搜索结束日期")]
         EndDateTime,
         [Description("询价状态")]
         InquiryState,
         [Description("报价状态")]
         QuotationState,
         [Description("我的询价信息收藏")]
         SubscribeInquiry,
         [Description("我的产品信息收藏")]
         SubscribeProduct,
         [Description("我的采购商信息收藏")]
         Subscribepurchaser,
         [Description("我的供应商信息收藏")]
         Subscribesupplier,
         [Description("产品状态")]
         ItemState,
         [Description("地址")]
         Address,
         [Description("最低价")]
         MinPrice,
         [Description("最高价")]
         MaxPrice,
         [Description("业务类型(0不分类 1采购物品 2供应物品)")]
         BizType,
         [Description("收藏类型")]
         Subtype,
         [Description("平台产品代码")]
         StandardItemCode,
         [Description("平台产品名称")]
         StandardItemName,
         [Description("是否认证(-1全部 1已认证 0未认证)")]
         IsCertification,
         [Description("[平台标准产品代码或名称]或[我的产品代码或名称]")]
         ItemInfoOrCodeInfo,
         [Description("三级产品分类id")]
         CategoryThirdID,
         [Description("去除平台标准产品代码为空的产品(询价发布选择产品使用,导入采购产品时,还没有对应平台产品关系)")]
         ExceptStandardItemCodeIsNull
     }
     /// <summary>
     /// 操作符 （等于、模糊查询）
     /// </summary> 
     [DataContract]
    public enum operatorEnum
    {
        eq,
        lk,
        bt
    }
     /// <summary>
     /// 查询属性字段
     /// </summary> 
     [DataContract]
     public class SearchAttribute
     {
         [DataMember(Order = 1)]
         public string SearchKey { get; set; }
        [DataMember(Order = 2)]
         public operatorEnum operatortype { get; set; }
         [DataMember(Order = 3)]
         public List<string> Value { get; set; }

     }
     public enum SearchType
     {
          [DataMember(Order = 1)]
         enterprise,
          [DataMember(Order = 2)]
         platform
     }

    /// <summary>
    /// 销售或者采购状态
    /// </summary>
      [DataContract]
     public enum CounterPartyEnum
     {
         /// <summary>
         /// 采购状态
         /// </summary>
         [DataMember(Order = 1)]
         Purchase,
         /// <summary>
         /// 销售状态
         /// </summary>
         [DataMember(Order = 2)]
         Sell

     }

      /// <summary>
      /// 订单来源状态
      /// </summary>
      [DataContract]
      public enum SourceEnum
      {
          /// <summary>
          /// 优选状态
          /// </summary>
          [DataMember(Order = 1)]
          Select,
          /// <summary>
          /// 购物车状态
          /// </summary>
          [DataMember(Order = 2)]
          ShoppingCart

      }
}
