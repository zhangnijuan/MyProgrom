using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 报价企业(or受邀企业)
    /// add by yangshuo 2014/12/10
    /// </summary>
    [Alias("sal_quotationcomp")]
    public class SalQuotationComp
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
        /// 公司名称
        /// </summary>
        [Alias("compname")]
        [StringLengthAttribute(64)]
        public string CompName { get; set; }

        /// <summary>
        /// 云ID
        /// </summary>
        [Alias("corpnum")]
        [StringLengthAttribute(20)]
        public string CorpNum { get; set; }

        /// <summary>
        /// 报价总价
        /// </summary>
        [Alias("amt")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 报价单主表ID
        /// </summary>
        [Alias("sid")]
        public long OfferID { get; set; }

        /// <summary>
        /// 报价单编码
        /// </summary>
        [Alias("snum")]
        public string OfferSnum { get; set; }

        /// <summary>
        /// 报价截止日期
        /// </summary>
        [Alias("sfinaldatetime")]
        public DateTime SFinalDateTime { get; set; }

        /// <summary>
        /// 报价时间
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 询价单主表
        /// </summary>
        [Alias("mid")]
        public long MID { get; set; }
    }
}
