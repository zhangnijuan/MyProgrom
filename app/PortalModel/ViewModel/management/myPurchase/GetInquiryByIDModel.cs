using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Ndtech.PortalModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据ID查询企业询价信息接口")]
    [Route("/inquiry/{ID}", HttpMethods.Post, Notes = "根据ID查询企业询价信息(我的询价、公开询价搜索、优选时显示询价资料调用)")]
    [DataContract]
    public class GetInquiryByIDRequest : IReturn<GetInquiryByIDResponse>
    {
      //  [DataMember(Order = 1)] 全网获取询价信息 无需传递key bai
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "账套Id(优选时显示询价资料需要传此参数)",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "企业产品ID",
      ParameterType = "path", DataType = "long", IsRequired = true)]
        public long ID { get; set; }

    }
    [DataContract]
    public class GetInquiryByIDResponse
    {
        public GetInquiryByIDResponse()
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
        public InquiryInfoList Data { get; set; }
    }

    [DataContract]
    public class InquiryInfoList : IReturn<GetInquiryByIDResponse>
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 询价代码
        /// </summary>
        [DataMember(Order = 3)]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [DataMember(Order = 4)]
        public string Subject { get; set; }

        /// <summary>
        /// 报价截止日期
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime FinalDateTime { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [DataMember(Order = 6)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [DataMember(Order = 7)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 运费承担代码
        /// </summary>
        [DataMember(Order = 8)]
        public string FreightTypeCode { get; set; }

        /// <summary>
        /// 收货地址ID
        /// </summary>
        [DataMember(Order = 9)]
        public string AddressID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 10)]
        public string MM { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [DataMember(Order = 11)]
        public string Attachment { get; set; }


        /// <summary>
        /// 附件Url
        /// </summary>
        [DataMember(Order = 12)]
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 13)]
        public string Linkman { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember(Order = 14)]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember(Order = 15)]
        public string EmailInfo { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [DataMember(Order = 16)]
        public string FixedLine { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [DataMember(Order = 17)]
        public string Fax { get; set; }

        /// <summary>
        /// 匿名代码
        /// </summary>
        [DataMember(Order = 18)]
        public int AnonymousCode { get; set; }

        /// <summary>
        /// 状态ID
        /// </summary>
        [DataMember(Order = 19)]
        public int State { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        [DataMember(Order = 20)]
        public string State_Enum { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        [DataMember(Order = 21)]
        public int InquiryType { get; set; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 23)]
        public string Eid { get; set; }

        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 24)]
        public string EidCode { get; set; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 25)]
        public string EidName { get; set; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
        [DataMember(Order = 26)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [DataMember(Order = 27)]
        public string Role_Enum { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [DataMember(Order = 28)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 询价明细信息
        /// </summary>
        [DataMember(Order = 29)]
        public List<InquiryDetList> PurItemList { get; set; }

        /// <summary>
        /// 报价企业信息
        /// </summary>
        [DataMember(Order = 30)]
        //public List<QuotationCompList> SupplierList { get; set; }
        public List<Supplier> SupplierList { get; set; }

        /// <summary>
        /// 主表附件
        /// </summary>
        [DataMember(Order = 31)]
        public List<Ndtech.PortalModel.ReturnPicResources> PicResources { get; set; }

        /// <summary>
        /// 收货地址名称
        /// </summary>
        [DataMember(Order = 32)]
        public string Address { get; set; }

        /// <summary>
        /// 报价企业数量
        /// </summary>
        [DataMember(Order = 33)]
        public string Quotations { get; set; }

        /// <summary>
        /// 优选单主档ID
        /// 优选画面使用 add by yangshuo
        /// </summary>
        [DataMember(Order = 34)]
        public string PurSelectID { get; set; }

        /// <summary>
        /// 优选说明->优选单主档
        /// 优选画面使用 add by yangshuo
        /// </summary>
        [DataMember(Order = 35)]
        public string PurSelectMM { get; set; }

        /// <summary>
        /// 询价公司名称
        /// 打开画面进行公开报价使用 add by yangshuo
        /// </summary>
        [DataMember(Order = 36)]
        public string Purchaser { get; set; }
    }
    /// <summary>
    /// 询价明细信息
    /// </summary>
    [DataContract]
    public class InquiryDetList
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        [DataMember(Order = 3)]
        public string MID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMember(Order = 4)]
        public string ItemID { get; set; }

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
        /// 单位id
        /// </summary>
        [DataMember(Order = 7)]
        public string UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 8)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 9)]
        public string UnitName { get; set; }

        /// <summary>
        /// 产品一级类别id
        /// </summary>
        [DataMember(Order = 10)]
        public string CategoryFirstID { get; set; }


        /// <summary>
        /// 产品一级类别代码
        /// </summary>
        [DataMember(Order = 11)]
        public string CategoryFirstCode { get; set; }

        /// <summary>
        /// 产品一级类别名称
        /// </summary>
        [DataMember(Order = 12)]
        public string CategoryFirstName { get; set; }

        /// <summary>
        /// 产品二级类别id
        /// </summary>
        [DataMember(Order = 13)]
        public string CategorySecondID { get; set; }


        /// <summary>
        /// 产品二级类别代码
        /// </summary>
        [DataMember(Order = 14)]
        public string CategorySecondCode { get; set; }

        /// <summary>
        /// 产品二级类别名称
        /// </summary>
        [DataMember(Order = 15)]
        public string CategorySecondName { get; set; }

        /// <summary>
        /// 产品三级类别id
        /// </summary>
        [DataMember(Order = 16)]
        public string CategoryThirdID { get; set; }


        /// <summary>
        /// 产品三级类别代码
        /// </summary>
        [DataMember(Order = 17)]
        public string CategoryThirdCode { get; set; }

        /// <summary>
        /// 产品三级类别名称
        /// </summary>
        [DataMember(Order = 18)]
        public string CategoryThirdName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [DataMember(Order = 19)]
        public string Attachment { get; set; }


        /// <summary>
        /// 附件
        /// </summary>
        [DataMember(Order = 20)]
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember(Order = 21)]
        public long Qty { get; set; }

        /// <summary>
        /// 产品属性id
        /// </summary>

        [DataMember(Order = 22)]
        public long PropertyID { get; set; }

        /// <summary>
        /// 产品属性代码
        /// </summary>
        [DataMember(Order = 23)]
        public string PropertyCode { get; set; }

        /// <summary>
        /// 产品属性名称
        /// </summary>
        [DataMember(Order = 24)]
        public string PropertyName { get; set; }


        /// <summary>
        /// 交货期
        /// </summary>
        [DataMember(Order = 25)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 26)]
        public string MM { get; set; }

        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [DataMember(Order = 27)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [DataMember(Order = 28)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 明细相关的资源
        /// </summary>
        [DataMember(Order = 29)]
        public List<Ndtech.PortalModel.ReturnPicResources> PicResources { get; set; }

        /// <summary>
        /// 优选结果
        /// </summary>
        [DataMember(Order = 30)]
        public string SelectResults { get; set; }

        /// <summary>
        /// 优选说明
        /// </summary>
        [DataMember(Order = 31)]
        public string SelectMM { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [DataMember(Order = 32)]
        public string Remark { get; set; }
    }
}
