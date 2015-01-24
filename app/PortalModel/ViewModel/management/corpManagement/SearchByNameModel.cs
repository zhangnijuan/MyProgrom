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
    [Api("恩维协同根据员工姓名查询接口")]
    [Route("/{AppKey}/{Secretkey}/employee/search", HttpMethods.Post, Notes = "自定义条件查询员工")]
    [Route("/{AppKey}/{Secretkey}/employee/search/{AccountID}/{PageIndex}/{PageSize}/{Name}", HttpMethods.Get, Notes = "根据姓名查询员工信息请求url")]
    [DataContract]
   public  class SearchByNameRequest:IReturn<GetAllEmployeeResponse>
    {

        [ApiMember(Description = "第几页",
              ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [ApiMember(Description = "员工姓名(支持模糊)",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string Name { get; set; }
        [ApiMember(Description = "账套Id",
                     ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }

        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
    }
   
}
