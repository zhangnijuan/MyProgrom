using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同客户档案接口")]
    [Route("/ucust", HttpMethods.Get, Notes = "客户档案url")]
    [DataContract]
    public class UdocCustomerRequest : IReturn<UdocCustomerResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "客户企业ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Comp { get; set; }


        [DataMember(Order = 2)]
        [ApiMember(Description = "客户企业编码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string CropNum { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "客户企业名称",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string CompName { get; set; }


        [DataMember(Order = 4)]
        [ApiMember(Description = "档案状态",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        public int State { get; set; }


        [DataMember(Order = 5)]
        [ApiMember(Description = "档案状态_枚举",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string State_Enum { get; set; }
    }
    public class UdocCustomerResponse
    {
        public UdocCustomerResponse()
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
