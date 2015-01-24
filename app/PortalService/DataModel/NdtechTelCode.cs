using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 手机验证码档案
    /// add by yangshuo 2014/12/03
    /// </summary>
    [Alias("gl_telcode")]
    public class NdtechTelCode
    {
        [PrimaryKey]
        [Alias("id")]
        [AutoIncrement]
        public long ID { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Alias("tel")]
        [StringLengthAttribute(32)]
        public string TelNum { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Alias("validatecode")]
        [StringLengthAttribute(100)]
        public string TelCode { get; set; }

        [Alias("createdate")]
        public DateTime Createdate { get; set; }
    }
}
