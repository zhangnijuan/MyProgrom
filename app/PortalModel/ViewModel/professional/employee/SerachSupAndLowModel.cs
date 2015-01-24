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
    [Api("恩维协同模糊搜索员工上下级接口")]
   
    [Route("/{AppKey}/{Secretkey}/employee/searchsuplow", HttpMethods.Post, Notes = "模糊搜索上下级")]
    [DataContract]
    public class SerachSupAndLowRequest : IReturn<SerachSupAndLowResopnse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "账套id",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountId { get; set; }

        [ApiMember(Description = "员工Id",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long StaffId { get; set; }
        [ApiMember(Description = "员工编码", ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 5)]
        public string SysCode { get; set; }

       
        [ApiMember(Description = "员工姓名", ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 6)]
        public string SysName { get; set; }
        
    }
     [DataContract]
    public class SerachSupAndLowResopnse
    {
        public  SerachSupAndLowResopnse()
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
