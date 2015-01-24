using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同查询认证机构列表")]
    [Route("/web/certification/search", HttpMethods.Post, Notes = "认证机构列表")]
    [DataContract]
    public class GetCertificationRequest : IReturn<GetCertificationResponse>
    {
        [ApiMember(Description = "认证机构名称(支持模糊搜索)",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 1)]
        public string CertificationName { get; set; }

        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageSize { get; set; }
    }

    [DataContract]
    public class GetCertificationResponse
    {
        public GetCertificationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public List<Certification> Data { get; set; }

        [DataMember(Order = 3)]
        public long RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class Certification
    {
        /// <summary>
        /// id
        /// </summary>
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Alias("n")]
        [DataMember(Order = 3)]
        public string CertificationName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("linkman")]
        [DataMember(Order = 4)]
        public string Linkman { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Alias("phone")]
        [DataMember(Order = 5)]
        public string Phone { get; set; }

        /// <summary>
        /// 送检地址
        /// </summary>
        [Alias("address")]
        [DataMember(Order = 6)]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Alias("zipcode")]
        [DataMember(Order = 7)]
        public string ZipCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Alias("mm")]
        [DataMember(Order = 8)]
        public string MM { get; set; }
    }
}
