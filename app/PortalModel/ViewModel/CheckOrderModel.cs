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
    [Api("恩维协同获取公司信息接口")]

    [Route("/{AppKey}/{Secretkey}/checkorder", HttpMethods.Post, Notes = "根据账套获取企业信息")]
    [DataContract]
    public class CheckOrderRequest : IReturn<CheckOrderResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
        [ApiMember(Description = "报价单id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public long ID { get; set; }
        [ApiMember(Description = "报价公司账套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }
    }
    public class CheckOrderResponse
    {
           public  CheckOrderResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
