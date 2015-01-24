using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 订单主表
    /// </summary>
    [Alias("udoc_statelog")]
    class StateLog
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 来源单证主表ID
        /// </summary>
        [Alias("sid")]
        public long SRCID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        [Alias("eid")]
        //[stringlengthattribute(32)]
        public long EID { get; set; }

        /// <summary>
        /// 操作人编码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string ECODE { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(32)]
        public string ENAME { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 操作后单证状态
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 备注1
        /// </summary>
        [Alias("firstmm")]
        [StringLengthAttribute(1024)]
        public string FirstMM { get; set; }

        /// <summary>
        /// 备注2
        /// </summary>
        [Alias("secondmm")]
        [StringLengthAttribute(1024)]
        public string SecondMM { get; set; }
    }
}
