using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 评价表
    /// add by liuzhiqiang 2015/1/5
    /// </summary>
    [Alias("pur_evaluationdetail")]
    public class PurEvaluationDetail
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [Alias("mid")]
        public long MID { get; set; }

        /// <summary>
        /// 评价题目ID
        /// </summary>
        [Alias("pid")]
        public long ProjectID { get; set; }

        /// <summary>
        /// 评价选项ID
        /// </summary>
        [Alias("optionid")]
        public long ProjectOptionID { get; set; }


        /// <summary>
        /// 选项说明
        /// </summary>
        [Alias("optionmm")]
        [StringLengthAttribute(1024)]
        public string OptionMM { get; set; }

    } 
}
