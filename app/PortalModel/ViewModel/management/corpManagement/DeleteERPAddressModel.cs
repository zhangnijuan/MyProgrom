using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同删除企业地址簿接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/address/delete", HttpMethods.Post, Notes = "删除企业地址薄")]
    [DataContract]
    public class DeleteERPAddressRequest : IReturn<EnterpriseAddressResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public long ID { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }
    }
}
