using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同报价企业物品明细接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/optimization/item/search", HttpMethods.Post, Notes = "根据询价明细ID查询报价企业集合")]
    [DataContract]
    public class GetQuotationByInquiryDIDRequest : IReturn<GetQuotationByInquiryDIDResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "询价明细ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long Inquirydid { get; set; }

        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        public int PageSize { get; set; }
    }

    [DataContract]
    public class GetQuotationByInquiryDIDResponse
    {
        public GetQuotationByInquiryDIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "报价企业物品集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public QuotationOptimization Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class QuotationOptimization 
    {
        /// <summary>
        /// 平台标准产品编码(画面"产品代码"绑定此字段)
        /// </summary>
        [DataMember(Order = 1)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称(画面"产品代码"绑定此字段)
        /// </summary>
        [DataMember(Order = 2)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 企业产品ID
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 企业产品代码
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("i_c")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 企业产品名称
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("i_n")]
        public string ItemName { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        [DataMember(Order = 6)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 产品描述(询价方)
        /// </summary>
        [DataMember(Order = 7)]
        public string Remark { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember(Order = 8)]
        [Alias("qty")]
        public long Quantity { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 9)]
        [Alias("u_c")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 10)]
        [Alias("u_n")]
        public string UnitName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 11)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 备注(询价方)
        /// </summary>
        [DataMember(Order = 12)]
        public string MM { get; set; }

        /// <summary>
        /// 上次成交价
        /// </summary>
        [DataMember(Order = 13)]
        public decimal LastPrc { get; set; }

        /// <summary>
        /// 上次成交供应商
        /// </summary>
        [DataMember(Order = 14)]
        public string LastCompName { get; set; }

        /// <summary>
        /// 上次交易历史ID
        /// 点击交易历史查询使用
        /// </summary>
        [DataMember(Order = 15)]
        public long LastID { get; set; }

        /// <summary>
        /// 企业产品分类代码
        /// </summary>
        [DataMember(Order = 16)]
        [Alias("category_c")]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 企业产品分类名称
        /// </summary>
        [DataMember(Order = 17)]
        [Alias("category_n")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [DataMember(Order = 18)]
        [Alias("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 询价单主档ID
        /// </summary>
        [DataMember(Order = 19)]
        [Alias("inquiryid")]
        public long InquiryID { get; set; }

        /// <summary>
        /// 询价单编号
        /// </summary>
        [DataMember(Order = 20)]
        [Alias("inquirycode")]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 优选id
        /// </summary>
        [DataMember(Order = 21)]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 报价供应商明细资料
        /// </summary>
        [DataMember(Order = 22)]
        public List<QuotationOptimizationDetail> DetailData { get; set; }
    }

    /// <summary>
    /// 报价明细和优选明细共用实体
    /// </summary>
    [DataContract]
    public class QuotationOptimizationDetail
    {
        /// <summary>
        /// 优选明细id
        /// </summary>
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 询价方帐套
        /// </summary>
        [DataMember(Order = 2)]
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 优选单主档ID
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("mid")]
        public long MID { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("compname")]
        public string CompName { get; set; }

        /// <summary>
        ///  供应商云ID
        ///  点击"查看供应商资质"使用
        ///  注意:声明类型要与db中该表的该字段类型一致
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("corpnum")]
        public string CompID { get; set; }

        /// <summary>
        /// 产品描述(报价方)
        /// </summary>
        [DataMember(Order = 6)]
        [Alias("mm")]
        public string Remark { get; set; }

        /// <summary>
        /// 上次成交价
        /// </summary>
        [DataMember(Order = 7)]
        [Alias("lastprc")]
        public decimal LastPrc { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember(Order = 8)]
        [Alias("prc")]
        public decimal Prc { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        [DataMember(Order = 9)]
        [Alias("quoamt")]
        public decimal QuoAmt { get; set; }

        /// <summary>
        /// 报价方备注
        /// </summary>
        [DataMember(Order = 10)]
        [Alias("smm")]
        public string SMM { get; set; }

        /// <summary>
        ///  报价单主表ID
        ///  点击"查看报价单"使用
        /// </summary>
        [DataMember(Order = 11)]
        [Alias("quotationid")]
        public long QuotationID { get; set; }

        /// <summary>
        ///  采购产品ID
        /// </summary>
        [DataMember(Order = 12)]
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        ///  询价单明细ID
        ///  输入"优选数量"保存优选汇总使用
        /// </summary>
        [DataMember(Order = 13)]
        [Alias("inquirydid")]
        public long Inquirydid { get; set; }

        /// <summary>
        ///  报价单明细ID
        ///  输入"优选数量"保存优选汇总使用
        /// </summary>
        [DataMember(Order = 14)]
        [Alias("quotationdid")]
        public long Quotationdid { get; set; }

        /// <summary>
        /// 优选数量
        /// </summary>
        [DataMember(Order = 15)]
        [Alias("sqty")]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 单个物品优选金额
        /// </summary>
        [DataMember(Order = 16)]
        [Alias("amt")]
        public decimal Amt { get; set; }

        /// <summary>
        /// 优选供应商产品总金额
        /// </summary>
        [DataMember(Order = 17)]
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 平台标准产品编码(画面"产品代码"绑定此字段)
        /// </summary>
        [DataMember(Order = 18)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称(画面"产品代码"绑定此字段)
        /// </summary>
        [DataMember(Order = 19)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 企业产品代码
        /// </summary>
        [DataMember(Order = 20)]
        [Alias("i_c")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 企业产品名称
        /// </summary>
        [DataMember(Order = 21)]
        [Alias("i_n")]
        public string ItemName { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        [DataMember(Order = 22)]
        [Alias("propertyname")]
        public string PropertyName { get; set; }

        /// <summary>
        /// 询价数量
        /// </summary>
        [DataMember(Order = 23)]
        [Alias("qty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 24)]
        [Alias("u_c")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 25)]
        [Alias("u_n")]
        public string UnitName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 26)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 企业产品分类代码
        /// </summary>
        [DataMember(Order = 27)]
        [Alias("category_c")]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 企业产品分类名称
        /// </summary>
        [DataMember(Order = 28)]
        [Alias("category_n")]
        public string CategoryName { get; set; }

        // <summary>
        /// 报价产品ID
        /// 点击"查看产品详情"使用
        /// </summary>
        [DataMember(Order = 29)]
        [Alias("quoitemid")]
        public long QuoItemID { get; set; }

        // <summary>
        /// 报价方帐套
        /// 查询"与该供应商上次成交价"使用
        /// 后端查询使用,前端不需绑定此参数
        /// </summary>
        [DataMember(Order = 30)]
        [Alias("sa")]
        public int SAccountID { get; set; }
    }
}
