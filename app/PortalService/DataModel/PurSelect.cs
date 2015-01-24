using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 采购优选单
    /// add by yangshuo 2014/12/19
    /// </summary>
    [Alias("pur_select")]
    public class PurSelect
    {
        /// <summary>
        /// 主键id
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
        /// 优选编号
        /// </summary>
        [Alias("snum")]
        [StringLengthAttribute(64)]
        public string Code { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [Alias("subject")]
        [StringLengthAttribute(64)]
        public string Subject { get; set; }

        /// <summary>
        /// 询价单id
        /// </summary>
        [Alias("inquiryid")]
        public long InquiryID { get; set; }

        /// <summary>
        /// 询价单编号
        /// </summary>
        [Alias("inquiry_c")]
        [StringLengthAttribute(64)]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 状态
        /// 0 优选 
        /// 1 汇总结果
        /// 2 生成订单
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 优选说明
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 制单人编码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(32)]
        public string EIDName { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateTime { get; set; }
    }
}
