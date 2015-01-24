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
    [Api("恩维协同询价单根据账套获取企业物品对象接口")]
    [Route("/{AppKey}/{Secretkey}/purchase/GetPurInquiryEPItem", HttpMethods.Get, Notes = "根据账套获取企业物品对象")]
    [DataContract]
    public class GetPurInquiryEPItemRequest : IReturn<GetPurInquiryEPItemResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "账套Id", 
      ParameterType = "json", DataType = "int", IsRequired = true)]

        public int AccountID { get; set; }
    }
      [DataContract]
    public class GetPurInquiryEPItemResponse
    {
        //public GetPurInquiryEPItemResponse()
        //{
        //    this.ResponseStatus = new ResponseStatus();
        //}
         [DataMember(Order = 1)]
          public List<EPItemList> Data { get; set; }
          //public List<EnterpriseItem> Data { get; set; }

         //[DataMember(Order = 2)]
         //public ResponseStatus ResponseStatus { get; set; }
    }
    /// <summary>
      /// 返回企业物品对象
    /// </summary>
      [DataContract]

      public class EPItemList : IReturn<GetPurInquiryEPItemResponse>
      {
        //  /// <summary>
        //  /// 产品代码
        //  /// </summary>
        //[DataMember(Order = 1)] 
        //  public string ItemCode { get; set; }

        //  /// <summary>
        //  /// 产品名称
        //  /// </summary>
        //[DataMember(Order = 2)] 
        //  public string ItemName { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
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
        /// 产品分类id
        /// </summary>
        [DataMember(Order = 7)]
        public long CategoryID { get; set; }

        /// <summary>
        /// 产品分类上级id
        /// </summary>
        [DataMember(Order = 8)]
        public long CategoryParentID { get; set; }

        /// <summary>
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 9)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 10)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 物品状态
        /// </summary>
        [DataMember(Order = 11)]
        public int State { get; set; }

        /// <summary>
        /// 物品状态_枚举
        /// 0 草稿 1 已发布 2 已下架
        /// </summary>
        [DataMember(Order = 12)]
        public string StateEnum { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        [DataMember(Order = 13)]
        public long UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 14)]
        public long UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 15)]
        public string UnitName { get; set; }

        /// <summary>
        /// 图片资源外键
        /// </summary>
        [DataMember(Order = 16)]
        public long Resources { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [DataMember(Order = 17)]
        public string Remark { get; set; }

        /// <summary>
        /// 产品详情描述
        /// </summary>
        [DataMember(Order = 18)]
        public string Description { get; set; }

        /// <summary>
        /// 标准售价
        /// </summary>
        [DataMember(Order = 19)]
        public decimal SalPrc { get; set; }

        /// <summary>
        /// 价格说明
        /// </summary>
        [DataMember(Order = 20)]
        public string PriceDes { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        [DataMember(Order = 21)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 22)]
        public string Contacts { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember(Order = 23)]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 付款方式说明
        /// </summary>
        [DataMember(Order = 24)]
        public string Payment { get; set; }
      }
}
