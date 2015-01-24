using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同保存采购优选结果")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/optimization/new", HttpMethods.Post, Notes = "保存采购优选结果")]
    [DataContract]
    public class NewPurSelectRequest : IReturn<SavePurSelectResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "主键ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public long ID { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }

        [ApiMember(Description = "询价主档ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public long InquiryID { get; set; }

        [ApiMember(Description = "询价单编号",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string InquiryCode { get; set; }

        [ApiMember(Description = "询价主题",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string Subject { get; set; }

        [ApiMember(Description = "制单人",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 10)]
        public long EID { get; set; }

        [ApiMember(Description = "制单人编码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string EIDCode { get; set; }

        [ApiMember(Description = "制单人名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string EIDName { get; set; }

        [ApiMember(Description = "优选结果明细",
                  ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 13)]
        public List<PurSelectDetail> DetailData { get; set; }

        [ApiMember(Description = "询价单明细ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 14)]
        public long Inquirydid { get; set; }
    }

    /// <summary>
    /// 优选结果前台view
    /// =>PurSelectResults(dataModel)
    /// </summary>
    [DataContract]
    public class PurSelectDetail
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
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
        /// 平台标准产品编码
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("standarditemcode")]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("standarditemname")]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 企业产品分类编码
        /// </summary>
        [DataMember(Order = 6)]
        [Alias("category_c")]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 企业产品分类名称
        /// </summary>
        [DataMember(Order = 7)]
        [Alias("category_n")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 产品ID
        /// 优选取供应商产品ID
        /// modify 2015/01/06
        /// </summary>
        [DataMember(Order = 8)]
        [Alias("i")]
        public long QuoItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [DataMember(Order = 9)]
        [Alias("i_c")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Order = 10)]
        [Alias("i_n")]
        public string ItemName { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 11)]
        [Alias("u_c")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 12)]
        [Alias("u_n")]
        public string UnitName { get; set; }

        /// <summary>
        /// 询价数量
        /// </summary>
        [DataMember(Order = 13)]
        [Alias("qty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 优选数量
        /// </summary>
        [DataMember(Order = 14)]
        [Alias("sqty")]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember(Order = 15)]
        [Alias("prc")]
        public decimal Prc { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember(Order = 16)]
        [Alias("amt")]
        public decimal Amt { get; set; }

        /// <summary>
        /// 产品属性值
        /// </summary>
        [DataMember(Order = 17)]
        [Alias("propertyname")]
        public string PropertyName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 18)]
        [Alias("deliverydate")]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 询价单明细ID
        /// </summary>
        [DataMember(Order = 19)]
        [Alias("inquirydid")]
        public long Inquirydid { get; set; }

        /// <summary>
        /// 报价单明细ID
        /// </summary>
        [DataMember(Order = 20)]
        [Alias("quotationdid")]
        public long Quotationdid { get; set; }

        /// <summary>
        /// 报价企业
        /// </summary>
        [DataMember(Order = 21)]
        [Alias("compname")]
        public string CompName { get; set; }

        /// <summary>
        /// 报价企业云ID
        /// </summary>
        [DataMember(Order = 22)]
        [Alias("compid")]
        public string CompID { get; set; }

        /// <summary>
        /// 优选供应商产品总金额
        /// </summary>
        [DataMember(Order = 23)]
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 产品描述(报价方)
        /// </summary>
        [DataMember(Order = 24)]
        [Alias("mm")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 新增、修改Response结果
    /// </summary>
    public class SavePurSelectResponse
    {
        public SavePurSelectResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
