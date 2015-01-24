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
    [Alias("pur_evaluation")]
    public class PurEvaluation
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
        /// 编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string Code { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Alias("mid")]
        public long OrderID { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        
        /// <summary>
        /// 评价人名称
        /// </summary>
        [Alias("ename")]
        [StringLengthAttribute(64)]
        public string Ename { get; set; }


        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createtime")]
        public DateTime CreateTime { get; set; }

    }
}
