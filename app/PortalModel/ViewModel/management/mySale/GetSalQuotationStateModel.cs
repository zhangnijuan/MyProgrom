using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同是否已报价接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/state", HttpMethods.Post, Notes = "是否已报价信息")]
    [DataContract]
    public class GetSalQuotationStateRequest : IReturn<GetSalQuotationStateResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "询价编码",
                     ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string InquiryCode { get; set; }

        [ApiMember(Description = "帐套",
              ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 21)]
        public int AccountId { get; set; }

    }
    [DataContract]
    public class GetSalQuotationStateResponse
    {
        public GetSalQuotationStateResponse()
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
