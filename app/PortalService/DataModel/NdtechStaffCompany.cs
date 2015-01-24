using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    class NdtechStaffCompany
    {
     
        public long UserID { get; set; }

        /// <summary>
        /// 员工登陆账号
        /// </summary>
        [Alias("loginname")]
        [StringLengthAttribute(30)]
        public string LoginName { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(30)]
        public string UserCode { get; set; }

         [Alias("staffrole")]
        public int RoleID { get; set; }

         /// <summary>
         /// 用户权限名称
         /// </summary>
         [Alias("staffrole_enum")]
         [StringLengthAttribute(32)]
         public string Role_Enum { get; set; }

         /// <summary>
         /// 联系电话
         /// </summary>
         [Alias("tel")]
         [StringLengthAttribute(50)]
         public string TelNum { get; set; }

         /// <summary>
         /// 电子邮件地址
         /// </summary>
         [Alias("email")]
         [StringLengthAttribute(50)]
         public string Email { get; set; }

         [Alias("compname")]
         [StringLengthAttribute(100)]
         public string CompName { get; set; }

         /// <summary>
         /// 云ID
         /// </summary>
         [StringLengthAttribute(20)]
         [Alias("corpnum")]
         public string CorpNum { get; set; }

        [Alias("CompID")]
         public long CompID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        [Alias("n")]
        public string UserName { get; set; }
    }
}
