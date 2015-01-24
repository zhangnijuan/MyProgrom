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
    [Api("收藏企业产品列表采购信息接口")]
    [Route("/subscribe/list/new", HttpMethods.Post, Notes = "收藏企业产品列表采购信息")]
    [DataContract]
    public class SubscribeViewListRequest : IReturn<SubscribeViewListResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "收藏列表集合",
      ParameterType = "json", DataType = "List", IsRequired = true)]
        public List<SubscribeViewModelRequest> SubscriList { get; set; }
    }
    [DataContract]
    public class SubscribeViewListResponse
    {
        public SubscribeViewListResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
