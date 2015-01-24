using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    public class SalQuotationViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 询价单ID
        /// </summary>
        [Alias("inquiryid")]
        public long InquiryID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 报价单编码
        /// </summary>
        [Alias("snum")]
        [StringLengthAttribute(32)]
        public string SCode { get; set; }

        /// <summary>
        /// 询价代码
        /// </summary>
        [Alias("inquirycode")]
        [StringLengthAttribute(32)]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [Alias("subject")]
        [StringLengthAttribute(64)]
        public string Subject { get; set; }

        /// <summary>
        /// 询价日期
        /// </summary>
        [Alias("publisherdt")]
        public DateTime InquiryDateTime { get; set; }

        /// <summary>
        /// 询价截止日期
        /// </summary>
        [Alias("finaldatetime")]
        public DateTime FinalDateTime { get; set; }

        #region 采购方信息

        /// <summary>
        /// 采购方公司名称
        /// add 2014/12/19
        /// </summary>
        [Alias("companyname")]
        [StringLengthAttribute(64)]
        public string Purchaser { get; set; }

        /// <summary>
        /// 采购方公司云ID
        /// add 2014/12/19
        /// </summary>
        [Alias("corpnum")]
        [StringLengthAttribute(64)]
        public string PurCorpnum { get; set; }
        /// <summary>
        /// 采购方联系人
        /// </summary>
        [Alias("linkman")]
        [StringLengthAttribute(32)]
        public string Linkman { get; set; }

        /// <summary>
        /// 采购方手机
        /// </summary>
        [Alias("mobile")]
        [StringLengthAttribute(32)]
        public string Mobile { get; set; }

        /// <summary>
        /// 采购方固定电话
        /// </summary>
        [Alias("fixedline")]
        [StringLengthAttribute(32)]
        public string FixedLine { get; set; }

        /// <summary>
        /// 采购方传真
        /// add 2014/12/19
        /// </summary>
        [Alias("fax")]
        [StringLengthAttribute(32)]
        public string Fax { get; set; }

        /// <summary>
        /// 采购方邮箱
        /// </summary>
        [Alias("emialinfo")]
        [StringLengthAttribute(32)]
        public string EmailInfo { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 付款方式
        /// </summary>
        [Alias("paytype")]
        [StringLengthAttribute(8)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [Alias("invoicetype")]
        [StringLengthAttribute(8)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 运费承担方
        /// </summary>
        [Alias("freighttypecode")]
        [StringLengthAttribute(8)]
        public string FreightTypeCode { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(1024)]
        public string Address { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        #endregion

        #region 报价信息

        /// <summary>
        /// 报价说明
        /// </summary>
        [Alias("quoteexplain")]
        [StringLengthAttribute(256)]
        public string QuoteExplain { get; set; }

        /// <summary>
        /// 发布状态  0 公开发布 1 邀请
        /// </summary>
        [Alias("inquirytype")]
        public int InquiryType { get; set; }

        /// <summary>
        /// 询价是否匿名发布
        /// </summary>
        [Alias("anonymouscode")]
        public int AnonymousCode { get; set; }

        /// <summary>
        /// 报价有效期
        /// </summary>
        [Alias("sfinaldatetime")]
        public DateTime SFinalDateTime { get; set; }

        /// <summary>
        /// 合计信息
        /// </summary>
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }

        #endregion

        #region 联系信息

        /// <summary>
        /// 报价企业
        /// </summary>
        [Alias("scompanyname")]
        [StringLengthAttribute(100)]
        public string SCompanyName { get; set; }



        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("slinkman")]
        [StringLengthAttribute(50)]
        public string SLinkman { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [Alias("sfixedline")]
        [StringLengthAttribute(32)]
        public string SFixedLine { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Alias("smobile")]
        [StringLengthAttribute(32)]
        public string SMobile { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Alias("sfax")]
        [StringLengthAttribute(32)]
        public string SFax { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Alias("saddress")]
        [StringLengthAttribute(256)]
        public string SAddress { get; set; }

        #endregion

        #region 单证信息

        /// <summary>
        /// 单证状态 0 报价草稿 1 已报价 2 已优选 3 已下单 4已发货 5 关闭
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        ///// <summary>
        ///// 单证状态 
        ///// </summary>
        //[Alias("state_enum")]
        //[StringLengthAttribute(32)]
        //public string State_Enum { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 制单人code
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人Name
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(50)]
        public string EIDName { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 员工角色
        /// </summary>
        [Alias("roleid")]
        public int RoleID { get; set; }

        /// <summary>
        /// 员工角色名称
        /// </summary>
        [Alias("staffrole_enum")]
        [StringLengthAttribute(32)]
        public string Role_Enum { get; set; }



        #endregion

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 地址ID
        /// </summary>
        [Alias("sendaddressid")]
        public long SendAddressID { get; set; }
        /// <summary>
        /// inquiryState
        /// </summary>
        [Alias("inquiryState")]
        public int inquiryState { get; set; }
        /// <summary>
        /// 询价单账套
        /// </summary>
        [Alias("pura")]
        public int PurAccountID { get; set; }
    }
}
