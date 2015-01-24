using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 供应商档案表
    /// add by suyang 2014/12/17
    /// </summary>
    [Alias("udoc_supplier")]
    public class UdocSupplier
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
        /// 客户企业ID
        /// </summary>
        [Alias("comp")]
        public long Comp { get; set; }

        /// <summary>
        /// 客户企业编码
        /// </summary>
        [Alias("comp_c")]
        [StringLengthAttribute(32)]
        public string Cropnum { get; set; }

        /// <summary>
        /// 客户企业名称
        /// </summary>
        [Alias("comp_n")]
        [StringLengthAttribute(64)]
        public string CompName { get; set; }

        /// <summary>
        /// 档案状态
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 档案状态_枚举
        /// 0 黑名单 1 白名单 
        /// </summary>
        [Alias("state_enum")]
        [StringLengthAttribute(8)]
        public string StateEnum { get; set; }
    }
}

