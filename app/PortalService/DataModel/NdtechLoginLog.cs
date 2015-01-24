using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;
using Ndtech.PortalModel.ViewModel;

namespace Ndtech.PortalService.DataModel
{
     [Alias("udoc_loginlog")]
    class NdtechLoginLog
    {
        /// <summary>
        /// 标示ID
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
        /// 员工登陆ID
        /// </summary>
        [Alias("loginid")]
        public long LoginID { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string SysCode { get; set; }

        /// <summary>
        /// 员工登陆账号
        /// </summary>
        [Alias("loginname")]
        [StringLengthAttribute(32)]
        public string UserName { get; set; }

        /// <summary>
        /// 登陆日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        [Alias("clientip")]
        [StringLengthAttribute(32)]
        public string ClientIP { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        [Alias("Client")]
        public ClientType Client { get; set; }
    }
}
