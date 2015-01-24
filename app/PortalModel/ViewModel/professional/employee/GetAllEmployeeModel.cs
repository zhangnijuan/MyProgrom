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
    [Api("恩维协同获取所有员工列表接口")]
    [Route("/{AppKey}/{Secretkey}/employee/list/{AccountID}/{PageIndex}/{PageSize}", HttpMethods.Get, Notes = "获取所有员工列表请求url")]
    [Route("/{AppKey}/{Secretkey}/employee/list", HttpMethods.Post, Notes = "自定义条件查询")]
    [DataContract]
    public class GetAllEmployeeRequest : IReturn<GetAllEmployeeResponse>
    {

        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [ApiMember(Description = "权限Id",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RoleID { get; set; }

        [ApiMember(Description = "是否启用状态",
                       ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long State { get; set; }
        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }

        [ApiMember(Description = "员工编码", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string SysCode { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        [ApiMember(Description = "员工姓名", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string SysName { get; set; }

        /// <summary>
        /// 员工登陆账号
        /// </summary>
        [ApiMember(Description = "员工登陆名", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string UserName { get; set; }

        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
    }
    [DataContract]
    public class GetAllEmployeeResponse
    {
        public GetAllEmployeeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public List<ReturnList> Data { get; set; }
        [DataMember(Order = 2)]
        public long RowsCount { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
      

    }

    /// <summary>
    /// 返回给用户的数据
    /// </summary>
    [DataContract]
    public class ReturnList : IReturn<GetAllEmployeeResponse>
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 账套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }



        /// <summary>
        /// 员工编码
        /// </summary>

        [DataMember(Order = 3)]
        public string SysCode { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>

        [DataMember(Order = 4)]
        public string SysName { get; set; }

        /// <summary>
        /// 员工登陆账号
        /// </summary>

        [DataMember(Order = 5)]
        public string UserName { get; set; }



        /// <summary>
        /// 角色权限ID
        /// </summary>

        [DataMember(Order = 6)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>

        [DataMember(Order = 7)]
        public string Role_Enum { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>

        [DataMember(Order = 8)]
        public string TelNum { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>

        [DataMember(Order = 9)]
        public string Email { get; set; }
        /// <summary>
        /// 是否启用状态ID
        /// </summary>

        [DataMember(Order = 10)]
        public int State { get; set; }

        /// <summary>
        /// 是否启用状态名称
        /// </summary>

        [DataMember(Order = 11)]
        public string State_Enum { get; set; }

      

        /// <summary>
        /// 公司职位
        /// </summary>

        [DataMember(Order = 12)]
        public string Office { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 13)]
        public string Remark { set; get; }

        [DataMember(Order = 14)]
        public ReturnPicResources PicResources { get; set; }
        [DataMember(Order = 15)]
        public List<ReturnSupLowEmployee> Superiors { get; set; }
        [DataMember(Order = 16)]
        public List<ReturnSupLowEmployee> Lowers { get; set; }

    }
    /// <summary>
    /// 上下级员工对象
    /// </summary>
     [DataContract]
    public class ReturnSupLowEmployee
    {
        /// <summary>
        /// 上级员工id
        /// </summary>
        [DataMember(Order = 1)]
        public long EId { get; set; }
        /// <summary>
        /// 上级员工编号
        /// </summary>
       [DataMember(Order = 2)]
        public string EIdCode { get; set; }
        /// <summary>
        /// 上级员工姓名
        /// </summary>
        [DataMember(Order = 3)]
        public string EIdName { get; set; }
    }
   
   
}
