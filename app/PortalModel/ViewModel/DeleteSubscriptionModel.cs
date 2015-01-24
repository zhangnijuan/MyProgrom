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
    [Api("删除订阅采购信息接口")]
    [Route("/{AppKey}/{Secretkey}/subscribe/delete/{ID}", HttpMethods.Get, Notes = "删除订阅采购信息接口")]
    [DataContract]
    public class DeleteSubscriptionRequest : IReturn<DeleteSubscriptionResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
  ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "需要删除的项的主键",
            ParameterType = "json", DataType = "long")]
        [DataMember(Order = 3)]
        public long ID { get; set; }
    }
    public class DeleteSubscriptionResponse
    {
        public DeleteSubscriptionResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
