using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel.management.corpManagement
{
    [Api("我的收藏采购或者供应商列表接口")]
    [Route("/{AppKey}/{Secretkey}/enterprise/search/{counterparty}", HttpMethods.Post, Notes = "自定义查询")]
    [DataContract]
    public class SearchEnshrineCompanyRequest : IReturn<SearchEnshrineCompanyResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "第几页",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }
        [ApiMember(Description = "当前用户主键Id",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public long ID { get; set; }
        [ApiMember(Description = "当前用户公司账套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 6)]
        public int AccountID { get; set; }
        [ApiMember(Description = "查询条件(Address:地址，ItemorCode：文本框的值)",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 7)]
        public List<SearchField> SearchCondition { get; set; }

        [ApiMember(Description = "排序规则（OrederKey(Subdatetime:收藏时间,Favorites:人气）（sortKey(升序：Ascending,降序：Descending）",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 8)]
        public List<Order> OrderBy { get; set; }

        [ApiMember(Description = "类型（采购商(Purchaser)或者供应商(Supplier)）", ParameterType = "json", DataType = "CounterPartyEnum", IsRequired = true)]
        [DataMember(Order = 9)]
        public EnterpriseEnum CounterParty { get; set; }
    }
    [DataContract]
    public class SearchEnshrineCompanyResponse
    {
        public SearchEnshrineCompanyResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<SupplierInfo> Data { get; set; }
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
