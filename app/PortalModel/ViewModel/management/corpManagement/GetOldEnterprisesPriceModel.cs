using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取商家比价的交易快照")]
    [Route("/web/compitem/enterprises/price/old", HttpMethods.Post, Notes = "商家比价的交易快照")]
    [DataContract]
    public class GetOldEnterprisesPriceRequest : IReturn<GetEnterprisesPriceResponse>
    {
        [ApiMember(Description = "搜索日期",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string OrderTime { get; set; }

        [ApiMember(Description = "平台标准产品代码",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 2)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "产品所属公司的帐套",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "每页显示的笔数",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [ApiMember(Description = "第几页",
             ParameterType = "json", DataType = "int")]
        [DataMember(Order = 5)]
        public int PageIndex { get; set; }
    }

    [DataContract]
    public class GetEnterprisesPriceResponse
    {
        public GetEnterprisesPriceResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public List<GetEnterprisesPrice> Data { get; set; }

        [DataMember(Order = 3)]
        public long RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class GetEnterprisesPrice
    {
        [ApiMember(Description = "商家产品id",
                            ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "商家帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        [Alias("a")]
        public int AccountID { get; set; }

        [ApiMember(Description = "供应商名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        [Alias("compname")]
        public string CompName { get; set; }

        [ApiMember(Description = "产品名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        [Alias("standard_n")]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "产品单价",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        [Alias("salprc")]
        public decimal SalPrc { get; set; }

        [ApiMember(Description = "发货地",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        [Alias("address")]
        public string Address { get; set; }

        [ApiMember(Description = "商家云ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        [Alias("corpnum")]
        public string CorpNum { get; set; }
    }
}
