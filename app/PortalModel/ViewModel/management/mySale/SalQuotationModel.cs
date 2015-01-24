using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同销售报价单接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/new", HttpMethods.Post, Notes = "根据采购询价单生成报价单")]
    public class SalQuotationRequest : IReturn<SalQuotationResponse>
    {

        [DataMember(Order = 1)]
        [ApiMember(Description = "报价ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Order = 1)]
        [ApiMember(Description = "报价单编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SCode { get; set; }

        [ApiMember(Description = "产品明细",
         ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 17)]
        public List<SalItem> ItemList { get; set; }

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

        [ApiMember(Description = "附件",
                     ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 16)]
        public List<PicResources> PicResources { get; set; }
  
        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 24)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Eid { set; get; }
        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 25)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 26)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
        [DataMember(Order = 27)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [DataMember(Order = 28)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }

        #region 报价信息

        [ApiMember(Description = "报价说明",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 18)]
        public string QuoteExplain { get; set; }

        [ApiMember(Description = "报价有效期",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 21)]
        public DateTime SFinalDateTime { get; set; }
         
        [ApiMember(Description = "合计信息",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 22)]
        public Decimal TotalAmt { get; set; }
        #endregion

        #region  联系信息

        [ApiMember(Description = "报价企业",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 24)]
        public string SCompanyName { get; set; }

        [ApiMember(Description = "联系人",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 25)]
        public string SLinkman { get; set; }

        [ApiMember(Description = "手机",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 26)]
        public string SMobile { get; set; }

        [ApiMember(Description = "固定电话",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 28)]
        public string SFixedLine { get; set; }

        [ApiMember(Description = "传真",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 29)]
        public string SFax { get; set; }

        [ApiMember(Description = "联系地址",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 29)]
        public string SAddress { get; set; }

        #endregion

        [ApiMember(Description = " 发货地址id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long SendAddressID { get; set; }

        [ApiMember(Description = " 询价单id",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long InquiryID { get; set; }

        [ApiMember(Description = "报价日期",
             ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 2)]
        public DateTime CreateTime { get; set; }

        [DataMember(Order = 31)]
        [ApiMember(Description = "询价主题",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Subject { get; set; }


        [DataMember(Order = 32)]
        [ApiMember(Description = "询价编码",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string InquiryCode { get; set; }
    }

    
     [DataContract]
    public class SalQuotationResponse
    {
        public SalQuotationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 1)]
        public string Data { get; set; }
    }

    [DataContract]
    public class SalItem : IReturn<SalQuotationResponse>
    {
        [ApiMember(Description = "产品ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "产品代码",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string ItemName { get; set; }

        [ApiMember(Description = "数量",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int Quantity { get; set; }

        [ApiMember(Description = "单位ID",
                ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public long UnitID { get; set; }

        [ApiMember(Description = "单位代码",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string UnitCode { get; set; }

        [ApiMember(Description = "单位名称",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string UnitName { get; set; }

        [ApiMember(Description = "属性",
                ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<Property> PropertyList { get; set; }

        [ApiMember(Description = "描述",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string MM { get; set; }

        [ApiMember(Description = "交货日期",
                ParameterType = "json", DataType = "Date", IsRequired = true)]
        [DataMember(Order = 10)]
        public DateTime DeliveryDate { get; set; }

        [ApiMember(Description = "单价",
                ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 13)]
        public decimal Price { get; set; }

        [ApiMember(Description = "备注",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 14)]
        public string Remark { get; set; }

        [ApiMember(Description = "小计",
                ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 15)]
        public decimal Amount { get; set; }

         [ApiMember(Description = "附件",
                ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 16)]
        public List<PicResources> PicResources { get; set; }

         [ApiMember(Description = "平台标准产品编码",
             ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 17)]
         public string StandardItemCode { get; set; }

         [ApiMember(Description = "平台标准产品名称",
             ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 18)]
         public string StandardItemName { get; set; }

         [ApiMember(Description = "属性值",
          ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 19)]
         public string PropertyName { get; set; }

         [ApiMember(Description = "企业产品分类编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 20)]
         public string CategoryCode { get; set; }

         [ApiMember(Description = "企业产品分类名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 21)]
         public string CategoryName { get; set; }

         [ApiMember(Description = "报价产品描述",
               ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 14)]
         public string SRemark { get; set; }

         [ApiMember(Description = "报价备注",
             ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 14)]
         public string SMM { get; set; }
    }

   }
