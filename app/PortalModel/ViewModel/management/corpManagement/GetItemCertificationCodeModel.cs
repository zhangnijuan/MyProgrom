using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品质量认证申请单号")]
    [Route("/web/certificationapp/code/new", HttpMethods.Post, Notes = "认证申请单号")]
    [DataContract]
    public class GetItemCertificationCodeRequest : IReturn<GetItemCertificationCodeResponse>
    {
        [ApiMember(Description = "帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 1)]
        public int AccountID { get; set; }
    }

    [DataContract]
    public class GetItemCertificationCodeResponse
    {
        public GetItemCertificationCodeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
