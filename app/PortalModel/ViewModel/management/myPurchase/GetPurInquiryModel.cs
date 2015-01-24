using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同发布询价接口")]
    [Route("/{AppKey}/{Secretkey}/purchase/list/{InquiryCode}", HttpMethods.Get, Notes = "保存询价信息")]
    [DataContract]
    public class GetPurInquiryRequest : IReturn<GetPurInquiryResponse>
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
        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        [ApiMember(Description = "询价代码",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string InquiryCode { get; set; }
    }

    [DataContract]
    public class GetPurInquiryResponse
    {
        public GetPurInquiryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public PurInquiryInfo Data { get; set; }
    }

    [DataContract]
    public class PurInquiryInfo
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


        [ApiMember(Description = "收货地代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string AddressCode { get; set; }


        [ApiMember(Description = "备注",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string MM { get; set; }

        [ApiMember(Description = "附件",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string Attachment { get; set; }


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

        [ApiMember(Description = "全网发布 0=否;1=是",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 15)]
        public int AllPublic { get; set; }

        [ApiMember(Description = "邀请发布 0=否;1=是",
               ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 16)]
        public int InvitePublic { get; set; }

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
        public int Accountid { get; set; }

        [ApiMember(Description = "供应商明细",
        ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 22)]
        public List<Supplier> SupplierList { get; set; }

        [ApiMember(Description = "附件Url",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 23)]
        public string AttachmentUrl { get; set; }

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
    }

    
}
