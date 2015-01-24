using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同点击产品认证logo获取认证详情")]
    [Route("/web/certificationapp/search/detail/{ID}", HttpMethods.Get, Notes = "根据受理认证明细ID查询")]
    [DataContract]
    public class GetItemCertificationByDIDRequest : IReturn<GetItemCertificationByDIDResponse>
    {
        [ApiMember(Description = "受理明细ID",
             ParameterType = "json", DataType = "long")]
        [DataMember(Order = 1)]
        public long ID { get; set; }
    }

    [DataContract]
    public class GetItemCertificationByDIDResponse
    {
        public GetItemCertificationByDIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ItemCertificationDetailInfo Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class ItemCertificationDetailInfo
    {
        [ApiMember(Description = "认证机构",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        [Alias("certificationname")]
        public string CertificationName { get; set; }

        [ApiMember(Description = "认证通过日期",
             ParameterType = "json", DataType = "DateTime")]
        [DataMember(Order = 2)]
        [Alias("acceptdate")]
        public DateTime AcceptDate { get; set; }

        [ApiMember(Description = "认证报告集合",
             ParameterType = "json", DataType = "List")]
        [DataMember(Order = 3)]
        public List<ReturnPicResources> PicResources { get; set; }

        [ApiMember(Description = "认证说明",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 4)]
        [Alias("instructions")]
        public string Instructions { get; set; }
    }
}
