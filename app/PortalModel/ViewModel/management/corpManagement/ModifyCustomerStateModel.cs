using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同客户加入黑名单接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/customer/modify", HttpMethods.Post, Notes = "客户加入黑名单")]
    [DataContract]
    public class ModifyCustomerStateRequest : IReturn<ModifyCustomerStateResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
               ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ID { get; set; }

        [ApiMember(Description = "状态",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string State { get; set; }
    }

    [DataContract]
    public class ModifyCustomerStateResponse
    {
        public ModifyCustomerStateResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
