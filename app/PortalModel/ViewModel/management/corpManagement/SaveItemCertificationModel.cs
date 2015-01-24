using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同保存产品质量认证")]
    [Route("/web/certificationapp/new", HttpMethods.Post, Notes = "申请产品质量认证")]
    [Route("/web/certificationapp/modify", HttpMethods.Post, Notes = "受理产品质量认证")]
    [DataContract]
    public class SaveItemCertificationRequest : IReturn<ItemCertificationResponse>
    {
        [ApiMember(Description = "主表ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "申请方帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [ApiMember(Description = "受理机构帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 3)]
        public int CAccountID { get; set; }

        [ApiMember(Description = "申请编号",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        public string Code { get; set; }

        [ApiMember(Description = "申请方公司名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 5)]
        public string Whatsoever { get; set; }

        [ApiMember(Description = "申请方联系人",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 6)]
        public string Linkman { get; set; }

        [ApiMember(Description = "申请方手机",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 7)]
        public string Mobile { get; set; }

        [ApiMember(Description = "申请方固话",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        public string Phone { get; set; }

        [ApiMember(Description = "申请方传真",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        public string Tax { get; set; }

        [ApiMember(Description = "申请方邮箱",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 10)]
        public string Email { get; set; }

        [ApiMember(Description = "申请方邮编",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 11)]
        public string ZipCode { get; set; }

        [ApiMember(Description = "申请方详细地址",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 12)]
        public string Address { get; set; }

        [ApiMember(Description = "受理机构名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 13)]
        public string CertificationName { get; set; }

        [ApiMember(Description = "申请日期",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 14)]
        public DateTime CreateDate { get; set; }

        [ApiMember(Description = "处理日期",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 15)]
        public DateTime AcceptDate { get; set; }

        [ApiMember(Description = "单证状态0未处理  1已处理 2处理中",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 16)]
        public int State { get; set; }

        [ApiMember(Description = "申请人ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 17)]
        public long Eid { get; set; }

        [ApiMember(Description = "申请人编码",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 18)]
        public string EidCode { get; set; }

        [ApiMember(Description = "申请方联系人",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 19)]
        public string EidName { get; set; }

        [ApiMember(Description = "企业产品认证申请明细资料",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 20)]
        public List<ItemCertificationDetail> DetailData { get; set; }

        [ApiMember(Description = "若受理产品质量认证功能,参数值Y",
             ParameterType = "json", DataType = "bool")]
        [DataMember(Order = 17)]
        public string AcceptCertificationFlag { get; set; }
    }

    /// <summary>
    /// 企业产品认证申请明细ViewModel
    /// </summary>
    [DataContract]
    public class ItemCertificationDetail
    {
        [ApiMember(Description = "明细ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 2)]
        [Alias("a")]
        public int AccountID { get; set; }

        [ApiMember(Description = "主档ID",
              ParameterType = "json", DataType = "long")]
        [DataMember(Order = 3)]
        public long Mid { get; set; }

        [ApiMember(Description = "平台产品代码",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        [Alias("standard_c")]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "平台产品名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 5)]
        [Alias("standard_n")]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "产品id",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 6)]
        [Alias("i")]
        public long ItemID { get; set; }

        [ApiMember(Description = "产品代码",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 7)]
        [Alias("i_c")]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        [Alias("i_n")]
        public string ItemName { get; set; }

        [ApiMember(Description = "产品分类名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        public string CategoryName { get; set; }

        [ApiMember(Description = "产品属性",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 10)]
        public string PropertyName { get; set; }

        [ApiMember(Description = "认证时间",
             ParameterType = "json", DataType = "DateTime")]
        [DataMember(Order = 11)]
        public DateTime AcceptDate { get; set; }

        [ApiMember(Description = "产品描述",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 12)]
        public string Remark { get; set; }

        [ApiMember(Description = "备注",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 13)]
        public string MM { get; set; }

        [ApiMember(Description = "认证说明",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 14)]
        public string Instructions { get; set; }

        [ApiMember(Description = "认证结果 0不合格 1合格",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 15)]
        public int Results { get; set; }

        //[ApiMember(Description = "质检报告资源ID",
        //     ParameterType = "json", DataType = "long")]
        //[DataMember(Order = 16)]
        //public long ReportResources { get; set; }

        [ApiMember(Description = "认证报告",
            ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 16)]
        public List<ReturnPicResources> PicResources { get; set; }
    }

    [DataContract]
    public class ItemCertificationResponse
    {
        public ItemCertificationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "质量认证列表集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<ItemCertification> Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
