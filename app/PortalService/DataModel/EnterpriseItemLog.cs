using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品修改日志表
    /// add by yangshuo 2014/12/31
    /// </summary>
    [Alias("udoc_enterprise_itemlog")]
    public class EnterpriseItemLog
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
        /// 产品id
        /// </summary>
        [Alias("mid")]
        public long ItemID { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 修改人代码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(32)]
        public string EIDName { get; set; }

        /// <summary>
        /// 修改前json对象字符串
        /// </summary>
        [Alias("obj")]
        public string Obj { get; set; }

        /// <summary>
        /// 新增日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }
    }
}
