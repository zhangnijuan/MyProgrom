using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取企业和平台价格趋势接口")]
    [Route("/web/compitem/price/{ID}", HttpMethods.Post, Notes = "企业和平台近六个月价格趋势资料")]
    [DataContract]
    public class GetItemPriceTrendRequest : IReturn<GetItemPriceTrendResponse>
    {
        [ApiMember(Description = "产品ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "平台标准产品代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "订单点击交易快照取价格趋势",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string OrderTime { get; set; }
    }

    [DataContract]
    public class GetItemPriceTrendResponse
    {
        public GetItemPriceTrendResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "价格趋势集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<ItemPriceTrend> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class ItemPriceTrend
    {
        [ApiMember(Description = "月份",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string Date { get; set; }

        [ApiMember(Description = "公司产品价格",
                  ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 2)]
        public decimal ERPPrice { get; set; }

        [ApiMember(Description = "平台产品价格",
                  ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 3)]
        public decimal PlatPrice { get; set; }
    }
}
