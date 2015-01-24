using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同新增员工接口")]
    [Route("/{AppKey}/{Secretkey}/employee/new", HttpMethods.Post,Notes = "新增员工请求url")]
    [DataContract]
    public class NdtechAddStaffInfoRequest : IReturn<NdtechAddStaffResponse>
    {
        /// <summary>
        /// 员工编码
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "员工编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SysCode { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "员工名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SysName { get; set; }

        /// <summary>
        /// 员工登陆账号
        /// </summary>
        [DataMember(Order = 3)]
        [ApiMember(Description = "员工登陆账号",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 员工登陆密码
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "员工登陆密码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PassWord { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "公司职位",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Office { get; set; }

        /// <summary>
        /// 角色权限ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "联系电话",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string TelNum { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "电子邮件",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Email { get; set; }
        /// <summary>
        /// 是否启用状态ID
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "是否启用状态ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int State { get; set; }

        /// <summary>
        /// 是否启用状态名称
        /// </summary>
        [DataMember(Order = 11)]
        [ApiMember(Description = "是否启用状态名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string State_Enum { get; set; }

        // <summary>
        // 图片名
        // </summary>
        //[DataMember(Order = 12)]
        //[ApiMember(Description = "图片名",
        //    ParameterType = "json", DataType = "string", IsRequired = true)]
        //public string Pic_Name { get; set; }

        //[DataMember(Order = 13)]
        //[ApiMember(Description = "图片路径",
        //    ParameterType = "json", DataType = "string", IsRequired = true)]
        //public string Pic_Url { get; set; }
        [DataMember(Order = 12)]
        [ApiMember(Description = "图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public PicResources PicResources { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 13)]
        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Remark { set; get; }

        /// <summary>
        /// 帐套1
        /// </summary>
        [DataMember(Order = 14)]
        [ApiMember(Description = "帐套",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountID { set; get; }
        
        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 15)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Eid { set; get; }
        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 16)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }
        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 17)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
    }
    /// <summary>
    /// 返回对象
    /// </summary>
    [DataContract]
    public class NdtechAddStaffResponse
    {
        public NdtechAddStaffResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order =1)]
        public bool Success { get; set; }
    }
    /// <summary>
    /// 资源对象
    /// </summary>
    [DataContract]
    public class PicResources
    {
        [ApiMember(Description = "主键Id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [DataMember(Order = 3)]
        public string DocumentID { get; set; }
       
        [ApiMember(Description = "文件原始名称",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string OriginalName { get; set; }
       
        [ApiMember(Description = "平台修改后的文件名称",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string NewName { get; set; }
      
        [ApiMember(Description = "文件后缀名",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string Suffix { get; set; }
    
        [ApiMember(Description = "文件大小",
              ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
        public string  FileLength { get; set; }
    }

 
}
