using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品质量认证列表(申请人和受理人共用)")]
    [Route("/web/certificationapp/search", HttpMethods.Post, Notes = "认证信息列表")]
    [DataContract]
    public class GetItemCertificationRequest : IReturn<ItemCertificationResponse>
    {
        [ApiMember(Description = "帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 1)]
        public int AccountID { get; set; }

        [ApiMember(Description = "认证机构(支持模糊查询)-申请人打开列表参数",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 2)]
        public string CertificationName { get; set; }

        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [ApiMember(Description = "申请状态(-1全部  1已处理 0未处理 2处理中)-申请人打开列表参数.[若受理人打开待处理查询state!= 1]",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int State { get; set; }

        [ApiMember(Description = "单号或企业名称(支持模糊查询)-认证机构管理列表参数",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string CodeOrWhatsoever { get; set; }

        [ApiMember(Description = "受理人帐套(认证机构查询受理认证列表)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public int CAccountID { get; set; }

        [ApiMember(Description = "排序",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 8)]
        public List<Order> orders { get; set; }
    }

    /// <summary>
    /// 企业产品认证申请主档ViewModel
    /// </summary>
    [DataContract]
    public class ItemCertification
    {
        [ApiMember(Description = "主表ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }

        [ApiMember(Description = "申请方帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 2)]
        [Alias("a")]
        public int AccountID { get; set; }

        [ApiMember(Description = "受理机构帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 3)]
        [Alias("ca")]
        public int CAccountID { get; set; }

        [ApiMember(Description = "申请编号",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        [Alias("c")]
        public string Code { get; set; }

        [ApiMember(Description = "申请方公司名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 5)]
        [Alias("whatsoever")]
        public string Whatsoever { get; set; }

        [ApiMember(Description = "申请方联系人",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 6)]
        [Alias("linkman")]
        public string Linkman { get; set; }

        [ApiMember(Description = "申请方手机",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 7)]
        [Alias("mobile")]
        public string Mobile { get; set; }

        [ApiMember(Description = "申请方固话",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        [Alias("phone")]
        public string Phone { get; set; }

        [ApiMember(Description = "申请方传真",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        [Alias("tax")]
        public string Tax { get; set; }

        [ApiMember(Description = "申请方邮箱",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 10)]
        [Alias("email")]
        public string Email { get; set; }

        [ApiMember(Description = "申请方邮编",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 11)]
        [Alias("zipcode")]
        public string ZipCode { get; set; }

        [ApiMember(Description = "申请方详细地址",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 12)]
        [Alias("address")]
        public string Address { get; set; }

        [ApiMember(Description = "受理机构名称",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 13)]
        [Alias("certificationname")]
        public string CertificationName { get; set; }

        [ApiMember(Description = "申请日期",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 14)]
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        [ApiMember(Description = "处理日期",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 15)]
        [Alias("acceptdate")]
        public DateTime AcceptDate { get; set; }

        [ApiMember(Description = "单证状态0未处理  1已处理 2处理中",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 16)]
        [Alias("state")]
        public int State { get; set; }

        [ApiMember(Description = "企业产品认证申请明细资料",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 17)]
        public List<ItemCertificationDetail> DetailData { get; set; }

        [ApiMember(Description = "认证产品数量(查询列表绑定)",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 18)]
        [Alias("acceptcount")]
        public int AcceptCount { get; set; }

        #region 认证机构

        [ApiMember(Description = "联系人",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 19)]
        [Alias("clinkman")]
        public string CLinkMan { get; set; }

        [ApiMember(Description = "联系电话",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 20)]
        [Alias("cphone")]
        public string CPhone { get; set; }

        [ApiMember(Description = "邮编",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 21)]
        [Alias("czipcode")]
        public string CZipCode { get; set; }

        [ApiMember(Description = "送检地址",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 22)]
        [Alias("caddress")]
        public string CAddress { get; set; }

        [ApiMember(Description = "备注说明",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 23)]
        [Alias("cmm")]
        public string CMM { get; set; }

        #endregion
    }
}
