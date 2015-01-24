using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 上传资源信息
    /// </summary>
    [Alias("udoc_resources")]
   public  class Resources
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public  long Id{get;set;}
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }
        /// <summary>
        /// 档案外键
        /// </summary>
        [Alias("mid")]
        public long DocumentID { get; set; }
        /// <summary>
        /// 文件原始名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(100)]
        public string OriginalName { get; set; }
        /// <summary>
        /// 平台修改后的文件名称
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(100)]
        public string NewName { get; set; }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        [Alias("s")]
        [StringLengthAttribute(50)]
        public string Suffix { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [Alias("fileLength")]
        public string  FileLength { get; set; }

    }
}
