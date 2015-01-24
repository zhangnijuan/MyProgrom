using ServiceStack.Common.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同选择供应商列表接口")]
    [Route("/web/company/search/list/{AccountID}/{SupplierEnum}/{PageIndex}/{PageSize}", HttpMethods.Get, Notes = "获取选择供应商列表请求url")]
    [Route("/web/company/search", HttpMethods.Post, Notes = "根据供应商名称模糊查询")]
    [DataContract]
    public class SerachSupplierRequest:IReturn<SerachSupplierResponse>
    {
        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }
        [ApiMember(Description = "每页显示的条数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }
        [ApiMember(Description = "搜索公司名称",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string CompanyName { get; set; }
        [ApiMember(Description = "企业ID",
         ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }
        [ApiMember(Description = "企业ID",
         ParameterType = "json", DataType = "SerachSupplierEnum", IsRequired = true)]
        [DataMember(Order = 5)]
        public SerachSupplierEnum SupplierEnum { get; set; }
    }
    [DataContract]
    public class SerachSupplierResponse
    {
        public SerachSupplierResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<CompanyInfoReturn> Data { get; set; }
        [DataMember(Order = 3)]
        public int  RowsCount { get; set; }
        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    [DataContract]
    public enum SerachSupplierEnum
    {
        /// <summary>
        /// 全网
        /// </summary>
        All,
        /// <summary>
        /// 已交易
        /// </summary>
        Deal,
        /// <summary>
        /// 已收藏
        /// </summary>
        Favorites


    }



       [DataContract]
    public class CompanyInfoReturn
    {
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
       [DataMember(Order = 2)]
       [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("c")]
        public string CompCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("n")]
        public string CompName { get; set; }
        /// <summary>
        /// 公司规模id
        /// </summary>
       [DataMember(Order = 5)]
       [Alias("sid")]
        public int ScaleId { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        [DataMember(Order = 6)]
        [Alias("scale")]
        public string CompanyScale { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>
        [DataMember(Order = 7)]
        [Alias("nature")]
        public string CompNature { get; set; }

    }
}
