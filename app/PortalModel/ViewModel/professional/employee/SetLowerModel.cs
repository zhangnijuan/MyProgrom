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
    [Api("恩维协同设置员工下级接口")]
    [Route("/{AppKey}/{Secretkey}/employee/setlower", HttpMethods.Post, Notes = "提交员工设置")]
    [DataContract]
   public   class SetLowerRequest:IReturn<SetLowerResopnse>
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

        [ApiMember(Description = "选中的所有的下级员工",
       ParameterType = "json", DataType = "list", IsRequired = true)]
        [DataMember(Order = 4)]
        public List<Superior> Superiors { get; set; }
    }
    [DataContract]
    public class SetLowerResopnse
    {
        public  SetLowerResopnse()
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
}
