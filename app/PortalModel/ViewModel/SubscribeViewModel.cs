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
    [Api("收藏企业产品采购信息接口")]
    [Route("/subscribe/new", HttpMethods.Post, Notes = "收藏企业产品采购信息")]
    [DataContract]
    public class SubscribeViewModelRequest : IReturn<SubscribeResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "来源数据主键ID",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string FromDataID { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "0 取消收藏 1 收藏",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int Substate { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "0 询价信息 1 产品 2 采购商 3 供应商",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int Subtype { get; set; }


        [DataMember(Order = 4)]
        [ApiMember(Description = "收藏人企业标示",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountID { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "收藏人ID",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Subsciber { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "收藏人编号",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SubscriberCode { get; set; }


        [DataMember(Order = 7)]
        [ApiMember(Description = "收藏人名称",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SubscriberName { get; set; }
    }

    [DataContract]
    public class SubscribeResponse
    {
        public SubscribeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
