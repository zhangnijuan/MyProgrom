using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 评价题目表
    /// add by liuzhiqiang 2015/1/5
    /// </summary>
    [Alias("udoc_pro_options")]
    public class UdocProOptions
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 外键ID
        /// </summary>
        [Alias("projectid")]
        public long ProjectID { get; set; }

        /// <summary>
        ///选项名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(64)]
        public string Name { get; set; }

        /// <summary>
        /// 0 入门（一般） 1 初级（满意） 2 中级（非常满意） 3 高级（崇拜）
        /// </summary>
        [Alias("level")]
        public int Level { get; set; }

    }
}
