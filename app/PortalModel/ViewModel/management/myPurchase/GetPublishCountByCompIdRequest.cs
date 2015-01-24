using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同企业信息发布产品数量接口")]
    [Route("/Company/PublishCount", HttpMethods.Get, Notes = "企业信息发布产品数量")]
    [DataContract]
    public class GetPublishCountByCompIdRequest : IReturn<PublishCountResponce>
    {
        [ApiMember(Description = "企业代码",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string CompCode { get; set; }
    }
    [DataContract]
    public class PublishCountResponce
    {
        [ApiMember(Description = "企业代码",
        ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string CompCode { get; set; }
        [ApiMember(Description = "发布产品数量",
    ParameterType = "json", DataType = "int")]
        [DataMember(Order = 2)]
        public int Count { get; set; }
    }
}
