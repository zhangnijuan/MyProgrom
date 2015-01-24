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
    [Api("恩维协同根据ID查询企业报价信息接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/{ID}", HttpMethods.Get, Notes = "根据ID查询企业报价信息")]
    [DataContract]
    public class GetQuotationByIDRequest : IReturn<GetQuotationByIDResponse>
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
        public int AccountId { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "企业产品ID",
      ParameterType = "path", DataType = "long", IsRequired = true)]
        public long ID { get; set; }
    }
    [DataContract]
    public class GetQuotationByIDResponse
    {
        public GetQuotationByIDResponse()
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
        public QuotationInfoList Data { get; set; }
    }

    /// <summary>
    /// 报价信息
    /// </summary>
    [DataContract]
    public class QuotationInfoList
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

        ///// <summary>
        ///// 报价单名称
        ///// </summary>
        //[DataMember(Order = 3)]
        //public string SName { get; set; }

        /// <summary>
        /// 报价单编码
        /// </summary>
        [DataMember(Order = 3)]
        public string SCode { get; set; }

        /// <summary>
        /// 询价代码
        /// </summary>
        [DataMember(Order = 4)]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [DataMember(Order = 5)]
        public string Subject { get; set; }

        /// <summary>
        /// 询价日期
        /// </summary>
        [DataMember(Order = 6)]
        public DateTime InquiryDateTime { get; set; }

        /// <summary>
        /// 询价截止日期
        /// </summary>
        [DataMember(Order = 7)]
        public DateTime FinalDateTime { get; set; }

        #region 采购方信息

        /// <summary>
        /// 采购方公司名称
        /// </summary>
        [DataMember(Order = 8)]
        public string Purchaser { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 9)]
        public string Linkman { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember(Order = 10)]
        public string Mobile { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [DataMember(Order = 11)]
        public string FixedLine { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [DataMember(Order = 12)]
        public string Fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember(Order = 13)]
        public string EmailInfo { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 付款方式
        /// </summary>
        [DataMember(Order = 14)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// 0无
        /// 1普通发票
        /// 2增值发票
        /// </summary>
        [DataMember(Order = 15)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 运费承担方
        /// </summary>
        [DataMember(Order = 16)]
        public string FreightTypeCode { get; set; }

        /// <summary>
        /// 收货地
        /// </summary>
        [DataMember(Order = 17)]
        public string Address { get; set; }

        /// <summary>
        /// 报价方备注
        /// </summary>
        [DataMember(Order = 18)]
        public string MM { get; set; }

        #endregion

        #region 报价信息

        /// <summary>
        /// 报价说明
        /// </summary>
        [DataMember(Order = 19)]
        public string QuoteExplain { get; set; }

        /// <summary>
        /// 发布状态  0 公开发布 1 邀请
        /// </summary>
        [DataMember(Order = 20)]
        public int InquiryType { get; set; }

        /// <summary>
        /// 询价是否匿名发布
        /// </summary>
        [DataMember(Order = 21)]
        public int AnonymousCode { get; set; }

        /// <summary>
        /// 报价有效期
        /// </summary>
        [DataMember(Order = 22)]
        public DateTime SFinalDateTime { get; set; }

        /// <summary>
        /// 合计信息
        /// </summary>
        [DataMember(Order = 23)]
        public decimal TotalAmt { get; set; }

        #endregion

        #region 联系信息

        /// <summary>
        /// 报价企业
        /// </summary>
        [DataMember(Order = 24)]
        public string SCompanyName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 25)]
        public string SContact { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [DataMember(Order = 26)]
        public string SFixedLine { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember(Order = 27)]
        public string SMobile { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [DataMember(Order = 28)]
        public string SFax { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [DataMember(Order = 29)]
        public string SAddress { get; set; }

        #endregion

        #region 单证信息

        /// <summary>
        /// 单证状态 0 报价草稿 1 已报价 2 已优选 3 已下单 4已发货 5 关闭
        /// </summary>
        [DataMember(Order = 30)]
        public int State { get; set; }

        ///// <summary>
        ///// 单证状态枚举
        ///// </summary>
        //[DataMember(Order = 31)]
        //public string State_Enum { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [DataMember(Order = 31)]
        public string EID { get; set; }

        /// <summary>
        /// 制单人code
        /// </summary>
        [DataMember(Order = 32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人Name
        /// </summary>
        [DataMember(Order = 33)]
        public string EIDName { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [DataMember(Order = 34)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 员工角色
        /// </summary>
        [DataMember(Order = 35)]
        public int RoleID { get; set; }

        /// <summary>
        /// 员工角色名称
        /// </summary>
        [DataMember(Order = 36)]
        public string Role_Enum { get; set; }

        #endregion

        /// <summary>
        /// 询价方备注
        /// </summary>
        [DataMember(Order = 37)]
        public string PMM { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 38)]
        public string SLinkman { get; set; }
        
        [DataMember(Order = 39)]
        public List<QuotationDetList> ItemList { get; set; }

        /// <summary>
        /// 报价主表附件
        /// </summary>
        [DataMember(Order = 40)]
        public List<Ndtech.PortalModel.ReturnPicResources> PicResources { get; set; }

        /// <summary>
        /// 询价单ID
        /// </summary>
        [DataMember(Order = 41)]
        public string InquiryID { get; set; }

        /// <summary>
        /// 已报价企业个数
        /// </summary>
        [DataMember(Order = 42)]
        public int Quotations { get; set; }

        /// <summary>
        /// 询价主表附件add by yangshuo
        /// </summary>
        [DataMember(Order = 43)]
        public List<Ndtech.PortalModel.ReturnPicResources> PurPicResources { get; set; }
    }
    [DataContract]
    public class QuotationDetList
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
        /// 数量
        /// </summary>
        [DataMember(Order = 10)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 产品属性id
        /// </summary>
        [DataMember(Order = 11)]
        public string PropertyID { get; set; }

        /// <summary>
        /// 产品属性代码
        /// </summary>
        [DataMember(Order = 12)]
        public string PropertyCode { get; set; }

        /// <summary>
        /// 产品属性名称
        /// </summary>
        [DataMember(Order = 13)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 14)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 询价方备注
        /// </summary>
        [DataMember(Order = 15)]
        public string MM { get; set; }


        /// <summary>
        /// 单价
        /// </summary>
        [DataMember(Order = 16)]
        public decimal Price { get; set; }

        /// <summary>
        /// 询价方描述
        /// </summary>
        [DataMember(Order = 17)]
        public string Remark { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        [DataMember(Order = 18)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [DataMember(Order = 19)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [DataMember(Order = 20)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 询价单明细ID
        /// </summary>
        [DataMember(Order = 21)]
        public string Inquirydid { get; set; }

        /// <summary>
        /// 报价单明细相关资源
        /// </summary>
         [DataMember(Order = 22)]
        public List<Ndtech.PortalModel.ReturnPicResources> PicResources { get; set; }

         /// <summary>
         /// 报价方备注
         /// </summary>
         [DataMember(Order = 22)]
         public string SMM { get; set; }

         /// <summary>
         /// 报价产品描述
         /// </summary>
         [DataMember(Order = 23)]
         public string SRemark { get; set; }

         /// <summary>
         /// 是否已发布该产品
         /// </summary>
         [DataMember(Order = 24)]
         public bool ItemIsPublish { get; set; }
    }
}
