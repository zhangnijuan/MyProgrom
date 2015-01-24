using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同发布询价接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/inquiry/new", HttpMethods.Post, Notes = "保存询价信息")]
    [DataContract]
    public class PurInquiryRequest : IReturn<PurInquiryResponse>
    {
        [ApiMember(Description = "询价主题",
                     ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string Subject { get; set; }

        [ApiMember(Description = "报价截止日期",
                  ParameterType = "json", DataType = "DateTime", IsRequired = true)]
        [DataMember(Order = 2)]
        public DateTime FinalDateTime { get; set; }

        [ApiMember(Description = "付款方式",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string PayType { get; set; }

        [ApiMember(Description = "发票类型",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string InvoiceType { get; set; }

        [ApiMember(Description = "运费承担代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string FreightTypeCode { get; set; }

        [ApiMember(Description = "收货地址id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 6)]
        public long AddressID { get; set; }

   
        [ApiMember(Description = "备注",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string MM { get; set; }

        [ApiMember(Description = "附件",
                  ParameterType = "json", DataType = "List<PicResources>", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<PicResources> PicResources { get; set; }


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
        public string FixedLine  { get; set; }

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

        [ApiMember(Description = "收货地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 29)]
        public string Address { get; set; }

        /// <summary>
        /// 建档人公司名称
        /// </summary>
        [DataMember(Order = 26)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCompName { set; get; }

        /// <summary>
        ///采购申请主表ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "采购申请主表ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long PlanID { set; get; }
    }

    [DataContract]
    public class Supplier
    {
        [ApiMember(Description = "公司ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long CompId { get; set; }

        [ApiMember(Description = "公司代码",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 2)]
        public string CompCode { get; set; }

        [ApiMember(Description = "帐套",
                   ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "公司名称",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public string CompName { get; set; }

        [ApiMember(Description = "联系人",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 25)]
        public string SLinkman { get; set; }

        [ApiMember(Description = "手机",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 26)]

        public string SMobile { get; set; }

        [ApiMember(Description = "邮箱",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 27)]
        public string SEmailInfo { get; set; }

        [ApiMember(Description = "固定电话",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 28)]
        public string SFixedLine { get; set; }

        [ApiMember(Description = "传真",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 29)]
        public string SFax { get; set; }
    }

    [DataContract]
    public class PurItem
    {
        [ApiMember(Description = "明细ID",
                     ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ID { get; set; }

        [ApiMember(Description = "数量",
                     ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int Qty { get; set; }

        [ApiMember(Description = "交货期",
                    ParameterType = "json", DataType = "Date", IsRequired = true)]
        [DataMember(Order = 2)]
        public DateTime DeliveryDate { get; set; }

        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string MM { get; set; }

        [ApiMember(Description = "产品ID",
                     ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ItemID { get; set; }

        [ApiMember(Description = "产品代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string ItemName { get; set; }

        [ApiMember(Description = "单位ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
        public long UnitID { get; set; }

        [ApiMember(Description = "单位代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string UnitCode { get; set; } 

        [ApiMember(Description = "单位名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string UnitName { get; set; }

        [ApiMember(Description = "产品类别ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 10)]
        public long CategoryThirdID { get; set; }

        [ApiMember(Description = "产品类别代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string CategoryThirdCode { get; set; }

        [ApiMember(Description = "产品类别名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string CategoryThirdName { get; set; }

        [ApiMember(Description = "产品类别父ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 13)]
        public long CategoryMID { get; set; }

        [ApiMember(Description = "产品属性",
        ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 14)]
        public List<Property> PropertyList { get; set; }

        [ApiMember(Description = "平台标准产品编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "平台标准产品名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "附件",
          ParameterType = "json", DataType = "List<PicResources>", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<PicResources> PicResources { get; set; }

        [ApiMember(Description = "产品描述",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string Remark { get; set; }

        /// <summary>
        /// 产品属性名称
        /// </summary>
        [DataMember(Order = 13)]
        public string PropertyName { get; set; }

        [ApiMember(Description = "采购申请明细ID",
                        ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long PlanDetailID { get; set; }
    }


    [DataContract]
    public class Property
    {
        [ApiMember(Description = "产品类别ID",
                   ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long CategoryID { get; set; }

        [ApiMember(Description = "产品属性ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 2)]
        public long PropertyID { get; set; }

         [ApiMember(Description = "产品属性代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 3)]
        public string PropertyCode { get; set; }

         [ApiMember(Description = "产品属性名称",
           ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 4)]
        public string PropertyName { get; set; }

         [ApiMember(Description = "产品属性值",
            ParameterType = "json", DataType = "string", IsRequired = true)]
         [DataMember(Order = 5)]
        public string PropertyValue { get; set; }
    }
    [DataContract]
    public class PurInquiryResponse
    {
        public PurInquiryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        } 

       
        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
