using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel.platform.supply
{
    [Api("恩维协同查询所有状态接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/search/state/total", HttpMethods.Post, Notes = "查询所有状态")]
    [DataContract]
    public class SearchTotalCntRequest: IReturn<SearchTotalCntResponse>
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
        public string AccountID { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "查询条件",
        ParameterType = "json", DataType = "List", IsRequired = true)]        
        public List<SearchField> SearchCondition { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "判断是采购方还是供应方",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public CounterPartyEnum counterparty { get; set; }

    }
    [DataContract]
    public class SearchTotalCntResponse
    {
        public SearchTotalCntResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool PurInquirySuccess { get; set; }

        [DataMember(Order = 2)]
        public bool SalQuotationSuccess { get; set; }

        [DataMember(Order = 3)]
        public bool PurOrderSuccess { get; set; }

        [DataMember(Order = 4)]
        public bool PurOrderDetSuccess { get; set; }

        [DataMember(Order = 5)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 6)]
        public StateCntInfoList Data { get; set; }


 
    }
    [DataContract]
    public class StateCntInfoList
    {
        /// <summary>
        /// 询价单
        /// </summary>
        [DataMember(Order = 1)]
        public List<StateInfo> PurInquiryStateInfo { get; set; }

        /// <summary>
        /// 报价单
        /// </summary>
        [DataMember(Order = 2)]
        public List<StateInfo> SalQutationStateInfo { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        [DataMember(Order = 3)]
        public List<StateInfo> PurOrderStateInfo { get; set; }

        /// <summary>
        /// 订单交易中待收货，待收款数量统计
        /// </summary>
        [DataMember(Order = 3)]
        public List<StateInfo> PurOrderDetStateInfo { get; set; }
    }

}
