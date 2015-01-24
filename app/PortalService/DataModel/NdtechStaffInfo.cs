using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    [Alias("udoc_staff")]
    public class NdtechStaffInfo
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

        [Alias("salt")]
        [StringLengthAttribute(128)]
        public string Salt { get; set; }
        
        /// <summary>
        /// 员工编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string SysCode{ get; set;}
        
        /// <summary>
        /// 员工名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(50)]
        public string SysName { get; set; }
        
        /// <summary>
        /// 员工登陆账号
        /// </summary>
        [Alias("loginname")]
        [StringLengthAttribute(30)]
        public string UserName { get; set; }

        /// <summary>
        /// 员工登陆密码
        /// </summary>
        [Alias("loginpwd")]
        [StringLengthAttribute(64)]
        public string PassWord { get; set; }

        /// <summary>
        /// 公司职位
        /// </summary>
        [Alias("comptitle")]
        [StringLengthAttribute(32)]
        public string Office { get; set; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
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
        /// <summary>
        /// 是否启用状态ID
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 是否启用状态名称
        /// </summary>
        [Alias("state_enum")]
        [StringLengthAttribute(32)]
        public string State_Enum { get; set; }

        /// <summary>
        /// 图片名
        /// </summary>
        [Alias("staffpic")]
        [StringLengthAttribute(50)]
        public string Pic_Name { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [Alias("staffpic_url")]
        [StringLengthAttribute(256)]
        public string Pic_Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("remark")]
        [StringLengthAttribute(256)]
        public string Remark { set; get; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [Alias("eid")]
        public long Eid { get; set; }

        /// <summary>
        /// 建档人编码
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EidCode { get; set; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(50)]
        public string EidName { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateTime { get; set; }
    }
}
