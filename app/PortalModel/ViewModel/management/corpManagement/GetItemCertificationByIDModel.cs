using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品质量认证详情(申请人和受理人共用)")]
    [Route("/web/certificationapp/search/{ID}", HttpMethods.Get, Notes = "一笔产品质量认证详情")]
    [DataContract]
    public class GetItemCertificationByIDRequest : IReturn<GetItemCertificationByIDResponse>
    {
        [ApiMember(Description = "ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
    }

    [DataContract]
    public class GetItemCertificationByIDResponse
    {
        public GetItemCertificationByIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ItemCertification Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
