using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同发布报价接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/draft/new", HttpMethods.Post, Notes = "保存报价信息")]
    [DataContract]
    public class SaveSalQuotationDraftRequest : IReturn<SaveSalQuotationDraftResponse>
    {
        [ApiMember(Description = "询价编码",
                     ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string InquiryCode { get; set; }

        [ApiMember(Description = "询价主题",
                     ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Subject { get; set; }

        [ApiMember(Description = "报价截止日期",
                  ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 3)]
        public DateTime FinalDateTime { get; set; }

        [ApiMember(Description = "付款方式",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string PayType { get; set; }

        [ApiMember(Description = "发票类型",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string InvoiceType { get; set; }

        [ApiMember(Description = "运费承担代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string FreightTypeCode { get; set; }


        [ApiMember(Description = "收货地址id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
        public long AddressID { get; set; }


        [ApiMember(Description = "备注",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string MM { get; set; }

   
        [ApiMember(Description = "联系人",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string Linkman { get; set; }

        [ApiMember(Description = "手机",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string Mobile { get; set; }

        [ApiMember(Description = "邮箱",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string EmailInfo { get; set; }

        [ApiMember(Description = "固定电话",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string FixedLine { get; set; }

        [ApiMember(Description = "传真",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        public string Fax { get; set; }

        [ApiMember(Description = "匿名代码 0=否;1=是",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 14)]
        public int AnonymousCode { get; set; }

        [ApiMember(Description = "0全网发布1 邀请发布 2 全选",
             ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 15)]
        public int InquiryType { get; set; }

        [ApiMember(Description = "产品明细",
         ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 17)]
        public List<PurItem> PurItemList { get; set; }

        [DataMember(Order = 18)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 19)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 20)]
        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        [ApiMember(Description = "帐套",
              ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 21)]
        public int AccountID { get; set; }

        [ApiMember(Description = "供应商明细",
        ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 22)]
        public List<Supplier> SupplierList { get; set; }


        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 23)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Eid { set; get; }
        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 24)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 25)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
        [DataMember(Order = 26)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [DataMember(Order = 27)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }

        [ApiMember(Description = "附件",
         ParameterType = "json", DataType = "List<PicResources>", IsRequired = true)]
        [DataMember(Order = 28)]
        public List<PicResources> PicResources { get; set; }

        [ApiMember(Description = " 询价单id",
                     ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 29)]
        public long InquiryID { get; set; }

        [ApiMember(Description = "收货地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 30)]
        public string Address { get; set; }

        /// <summary>
        /// 采购方公司名称
        /// </summary>

        [ApiMember(Description = "采购方公司名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 31)]
        public string Purchaser { get; set; }

        [DataMember(Order = 1)]
        [ApiMember(Description = "报价ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public long ID { get; set; }
    }

    public class SaveSalQuotationDraftResponse
    {
        public SaveSalQuotationDraftResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }


        [ApiMember(Description = "全网询价单返回实体", DataType = "SaveSalQuotationDraftRequest", IsRequired = true)]
        [DataMember(Order = 3)]
        public SaveSalQuotationDraftRequest SaveSalQuotationDraftReturn { get; set; }
    }
 
}
