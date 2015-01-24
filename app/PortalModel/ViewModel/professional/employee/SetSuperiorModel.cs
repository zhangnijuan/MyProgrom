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

    [Api("恩维协同设置员工上级接口")]
    [Route("/{AppKey}/{Secretkey}/employee/getsuperior/{AccountId}/{StaffId}/{EmpolyeeEnum}/{PageIndex}/{PageSize}", HttpMethods.Get, Notes = "获取上下级员工")]
    [Route("/{AppKey}/{Secretkey}/employee/setsuperior", HttpMethods.Post, Notes = "提交员工设置")]
    [DataContract]
    public  class SetSuperiorRequest:IReturn<SetSuperiorResopnse>
    {
        
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }
        
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "员工Id",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public long StaffId { get; set; }
 
        [ApiMember(Description = "账套id",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountId { get; set; }
        [ApiMember(Description = "当前第几页",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
         ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        public int PageSize { get; set; }

        [ApiMember(Description = "选中的所有的上级员工",
       ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 7)]
        public List<Superior> Superiors { get; set; }
        [ApiMember(Description = "是设置上级（Superior）或者下级（Lower）",
      ParameterType = "json", DataType = "SearchEmpolyeeEnum", IsRequired = true)]
        [DataMember(Order = 8)]
        public SearchEmpolyeeEnum EmpolyeeEnum { get; set; }
    }
    [DataContract]
    public enum SearchEmpolyeeEnum
	{
        /// <summary>
        /// 下级
        /// </summary>
	        Lower,
        /// <summary>
        /// 上级
        /// </summary>
        Superior
 
	}
    [DataContract]
    public class SetSuperiorResopnse
    {
        public SetSuperiorResopnse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public Data Data { get; set; }
        [DataMember(Order = 2)]
        public bool Success { get; set; }
         [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    /// <summary>
    /// 返回上级员工的详细信息
    /// </summary>
     [DataContract]
    public class Superior
    {
         [DataMember(Order = 1)]
        public long ID { get; set; }
         [DataMember(Order = 2)]
        public string SysCode { get; set; }
         [DataMember(Order = 3)]
        public string SysName { get; set; }

        
    }
     public class Data
     {
         [DataMember(Order = 1)]
       public  List<Superior> SuperiorList { get; set; }
          [DataMember(Order = 2)]
        public Page Page { get; set; }
     }
    /// <summary>
    /// 接收的员工对象
    /// </summary>
    [DataContract]
    public class ReceiveSuperior
    {
        [ApiMember(Description = "上级员工Id",
              ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long EId { get; set; }

        [ApiMember(Description = "上级员工编号",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string EIdCode { get; set; }

        [ApiMember(Description = "上级员工姓名",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string EIdName { get; set; }
    }
}
