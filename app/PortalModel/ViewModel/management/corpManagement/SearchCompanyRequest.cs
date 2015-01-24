using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同企业信息搜索接口")]
    [Route("/search/Company", HttpMethods.Post, Notes = "搜索企业信息")]
    [DataContract]
    public class SearchCompanyRequest : IReturn<SearchCompanyResponce>
    {
        [ApiMember(Description = "第几页",
         ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [ApiMember(Description = "企业名称|产品分类",
                    ParameterType = "json", DataType = "string")]
        [DataMember(Order = 3)]
        public string Item { get; set; }
        [ApiMember(Description = "公司规模",
 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int ScaleId { get; set; }
        [ApiMember(Description = "公司性质",
 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int CompType { get; set; }
    }
     [DataContract]
    public class SearchCompanyResponce
    {
        [ApiMember(Description = "公司标示",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int AccountId { get; set; }
        [ApiMember(Description = "公司名称",
ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string CompName { get; set; }
        [ApiMember(Description = "公司规模",
ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int ScaleId { get; set; }
        [ApiMember(Description = "产品分类",
ParameterType = "json", DataType = "string[]", IsRequired = true)]
        [DataMember(Order = 4)]
        public string[] CategoryNames { get; set; }
    }
}
