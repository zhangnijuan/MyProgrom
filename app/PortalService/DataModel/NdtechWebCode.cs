using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    [Alias("udoc_webcode")]
    public class NdtechWebCode
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Alias("username")]
        [StringLengthAttribute(128)]
        public string UserName { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        [Alias("webcode")]
        [StringLengthAttribute(32)]
        public string WebCode { get; set; }
    }
}
