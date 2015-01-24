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
    [Api("恩维协同管理员重置密码接口")]
    [Route("/{AppKey}/{Secretkey}/employee/restPwd", HttpMethods.Post, Notes = "管理员重置密码请求url")]
    [DataContract]
    public class RestPossWordRequest:IReturn<RestPossWordResponse>
    {


        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
        [DataMember(Order = 3)]
        [ApiMember(Description = "员工ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "重置的密码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string PassWord { get; set; }
    }
     [DataContract]
    public class RestPossWordResponse
    {
        public RestPossWordResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        
         [DataMember(Order = 1)]
         public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 2)]
         public Data Date { get; set; }
         [DataMember(Order = 3)]
         public bool IsSucess { get; set; }
    }
}
