using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业地址薄明细档案
    /// add by yangshuo 2015/01/19
    /// </summary>
    [Alias("udoc_enterprise_def_address")]
    public class EnterpriseDefAddress
    {
        /// <summary>
        /// 主键
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
        /// 地址簿主档id
        /// </summary>
        [Alias("mid")]
        public long MID { get; set; }

        /// <summary>
        /// 员工id
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }
    }
}
