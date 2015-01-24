using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品认证申请明细表
    /// add by yangshuo 2015/01/05
    /// </summary>
    [Alias("udoc_certification_appdetail")]
    public class EnterpriseItemsCertificationDetail
    {
        /// <summary>
        /// id
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
        /// 主档id
        /// </summary>
        [Alias("mid")]
        public long Mid { get; set; }

        /// <summary>
        /// 平台产品代码
        /// </summary>
        [Alias("standard_c")]
        [StringLengthAttribute(64)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台产品名称
        /// </summary>
        [Alias("standard_n")]
        [StringLengthAttribute(1024)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("i_c")]
        [StringLengthAttribute(64)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("i_n")]
        [StringLengthAttribute(1024)]
        public string ItemName { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [Alias("categoryname")]
        [StringLengthAttribute(64)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        [Alias("propertyname")]
        [StringLengthAttribute(1024)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 认证时间
        /// </summary>
        [Alias("acceptdate")]
        public DateTime AcceptDate { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        /// <summary>
        /// 认证说明
        /// </summary>
        [Alias("instructions")]
        [StringLengthAttribute(1024)]
        public string Instructions { get; set; }

        /// <summary>
        /// 认证结果
        /// 0不合格 1合格
        /// </summary>
        [Alias("results")]
        public int Results { get; set; }

        /// <summary>
        /// 质检报告资源ID
        /// </summary>
        [Alias("resources")]
        public long ReportResources { get; set; }
    }
}
