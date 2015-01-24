using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取订单状态数量信息接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/{counterparty}/purorder/state/group", HttpMethods.Post, Notes = "获取订单状态数量信息")]
    [DataContract]
    public class GetPurOrderStateCntRequest:IReturn<GetPurOrderStateCntResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "账套Id",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "判断是采购方还是供应方",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public CounterPartyEnum counterparty { get; set; }

        [ApiMember(Description = "字段条件",
      ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 5)]
        public List<SearchField> SearchCondition { get; set; }
    }
    [DataContract]
    public class GetPurOrderStateCntResponse
    {
        public GetPurOrderStateCntResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public List<StateInfo> Data { get; set; }

    }
}
