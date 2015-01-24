using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同修改员工信息接口")]
    [Route("/{AppKey}/{Secretkey}/employee/modify/{ID}", HttpMethods.Post, Notes = "根据员工ID修改员工信息")]
    [DataContract]
    public class UpdateEmployeeRequest : IReturn<UpdateEmployeeResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "员工ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "员工编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SysCode { get; set; }

       
        [DataMember(Order = 3)]
        [ApiMember(Description = "员工名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SysName { get; set; }

        
        [DataMember(Order = 4)]
        [ApiMember(Description = "员工登陆账号",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }

       
       

        [DataMember(Order = 5)]
        [ApiMember(Description = "公司职位",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Office { get; set; }

     
        [DataMember(Order = 6)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int RoleID { get; set; }

        
        [DataMember(Order = 7)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }


        
        [DataMember(Order = 8)]
        [ApiMember(Description = "联系电话",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string TelNum { get; set; }

        
        [DataMember(Order = 9)]
        [ApiMember(Description = "电子邮件",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Email { get; set; }
       
        [DataMember(Order = 10)]
        [ApiMember(Description = "是否启用状态ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int State { get; set; }

        
        [DataMember(Order = 11)]
        [ApiMember(Description = "是否启用状态名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string State_Enum { get; set; }

        [DataMember(Order = 12)]
        [ApiMember(Description = "图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public UpdatePicResources PicResources { get; set; }

       
        [DataMember(Order = 13)]
        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Remark { set; get; }

        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
    }
    public class UpdateEmployeeResponse
    {
       public UpdateEmployeeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
       public ResponseStatus ResponseStatus { get; set; }
          [DataMember(Order = 2)]
       public bool Success { get; set; }
    }
    /// <summary>
    /// 资源对象
    /// </summary>
    [DataContract]
    public class UpdatePicResources
    {

        [ApiMember(Description = "主键Id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [DataMember(Order = 3)]
        public long DocumentID { get; set; }
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
        public string FileLength { get; set; }
    }
}
