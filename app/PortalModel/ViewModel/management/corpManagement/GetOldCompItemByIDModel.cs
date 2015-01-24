using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取企业产品交易快照")]
    [Route("/web/compitem/old", HttpMethods.Post, Notes = "获取企业产品交易快照")]
    [DataContract]
    public class GetOldCompItemByIDRequest : IReturn<GetCompItemByIDResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "平台标准代码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string StandardItemCode { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "供应商帐套",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int SAccountID { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "搜索日期",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SearchDate { get; set; }
    }
}
