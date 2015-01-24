using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 注册会员_帐套档案
    /// add by yangshuo 2014/12/03
    /// </summary>
    [Alias("gl_acntsystems")]
    public class NdtechAcntSystem
    {
        /// <summary>
        /// 帐套
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public int ID { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        [Alias("companyid")]
        public long CompId { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string CompCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(100)]
        public string CompName { get; set; }

        /// <summary>
        /// 云ID
        /// </summary>
        [StringLengthAttribute(20)]
        [Alias("corpnum")]
        public string CorpNum { get; set; }

        [Alias("createdate")]
        public DateTime Createdate { get; set; }
    }
}
