using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同企业信息进行中的询价数接口")]
    [Route("/Company/PurInquiryCount", HttpMethods.Get, Notes = "企业信息进行中的询价数量")]
    [DataContract]
    public class GetPurInquiryCountByCompIdRequest : IReturn<PurInquiryCountResponce>
    {
        [ApiMember(Description = "企业代码",
    ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string CompCode { get; set; }
    }
    [DataContract]
    public class PurInquiryCountResponce
    {
        [ApiMember(Description = "企业代码",
        ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string CompCode { get; set; }
        [ApiMember(Description = "进行中的询价数",
    ParameterType = "json", DataType = "int")]
        [DataMember(Order = 2)]
        public int Count { get; set; }
    }
}
