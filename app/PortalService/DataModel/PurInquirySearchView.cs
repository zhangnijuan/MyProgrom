using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    class PurInquirySearchView
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 询价代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [Alias("subject")]
        [StringLengthAttribute(64)]
        public string Subject { get; set; }

        /// <summary>
        /// 报价截止日期
        /// </summary>
        [Alias("finaldatetime")]
        public DateTime FinalDateTime { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Alias("paytype")]
        [StringLengthAttribute(32)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [Alias("invoicetype")]
        [StringLengthAttribute(32)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 运费承担代码
        /// </summary>
        [Alias("freighttypecode")]
        [StringLengthAttribute(32)]
        public string FreightTypeCode { get; set; }

        /// <summary>
        /// 收货地代码
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(128)]
        public string AddressCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(128)]
        public string MM { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [Alias("attach")]
        [StringLengthAttribute(32)]
        public string Attachment { get; set; }


        /// <summary>
        /// 附件Url
        /// </summary>
        [Alias("attachUrl")]
        [StringLengthAttribute(256)]
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("linkman")]
        [StringLengthAttribute(32)]
        public string Linkman { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Alias("mobile")]
        [StringLengthAttribute(32)]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Alias("emailinfo")]
        [StringLengthAttribute(32)]
        public string EmailInfo { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [Alias("fixedline")]
        [StringLengthAttribute(32)]
        public string FixedLine { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Alias("fax")]
        [StringLengthAttribute(32)]
        public string Fax { get; set; }

        /// <summary>
        /// 匿名代码
        /// </summary>
        [Alias("anonymouscode")]
        public int AnonymousCode { get; set; }

        /// <summary>
        /// 状态ID
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        [Alias("state_enum")]
        [StringLengthAttribute(32)]
        public string State_Enum { get; set; }

        /// <summary>
        /// 全网发布
        /// </summary>
        [Alias("allpublic")]
        public int AllPublic { get; set; }

        /// <summary>
        /// 邀请发布
        /// </summary>
        [Alias("invitepublic")]
        public int InvitePublic { get; set; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [Alias("eid")]
        public long Eid { get; set; }

        /// <summary>
        /// 建档人编码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EidCode { get; set; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(64)]
        public string EidName { get; set; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
        [Alias("staffrole")]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [Alias("staffrole_enum")]
        [StringLengthAttribute(32)]
        public string Role_Enum { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createtime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("CompName")]
        public string CompName { get; set; }
        /// <summary>
        /// 报价数
        /// </summary>
        [Alias("quotations")]
        public int quotations { get; set; }
    }
}
