using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据认证机构帐套获取认证机构详情")]
    [Route("/web/certification/search/{AccountID}", HttpMethods.Get, Notes = "认证机构详情")]
    [DataContract]
    public class GetCertificationByIDRequest
    {
        [ApiMember(Description = "认证机构帐套",
            ParameterType = "json", DataType = "int")]
        [DataMember(Order = 1)]
        public int CAccountID { get; set; }
    }

    [DataContract]
    public class GetCertificationByIDResponse
    {
        public GetCertificationByIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        /// <summary>
        /// 认证机构资料
        /// </summary>
        [DataMember(Order = 2)]
        public Certification Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
