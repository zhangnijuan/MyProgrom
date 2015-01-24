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
    [Alias("udoc_project")]
    public class UdocProject
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(64)]
        public string Name { get; set; }

        /// <summary>
        /// 0 禁用 1 启用
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 题目类型 0 供应类 1 销售类
        /// </summary>
        [Alias("type")]
        public int Type { get; set; }
    }
}