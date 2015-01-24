using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DataAnnotations;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据ID查询优选汇总接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/optimization/search", HttpMethods.Post, Notes = "根据ID查询优选汇总集合")]
    [DataContract]
    public class GetPurSelectRequest : IReturn<GetPurSelectResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "询价ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long InquiryID { get; set; }
    }

    [DataContract]
    public class GetPurSelectResponse
    {
        public GetPurSelectResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "优选汇总集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public PurSelectMain Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class PurSelectMain
    {
        /// <summary>
        /// 优选id
        /// </summary>
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
        /// 优选编号
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("snum")]
        public string Code { get; set; }

        /// <summary>
        /// 询价主题
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 询价单主档ID
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("inquiryid")]
        public long InquiryID { get; set; }

        /// <summary>
        /// 询价单编号
        /// </summary>
        [DataMember(Order = 6)]
        [Alias("inquirycode")]
        public string InquiryCode { get; set; }

        /// <summary>
        /// 单证状态
        /// </summary>
        [DataMember(Order = 7)]
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [DataMember(Order = 8)]
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 制单人编码
        /// </summary>
        [DataMember(Order = 9)]
        [Alias("eid_syscode")]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人名称
        /// </summary>
        [DataMember(Order = 10)]
        [Alias("eid_usrname")]
        public string EIDName { get; set; }

        /// <summary>
        /// 优选备注
        /// </summary>
        [DataMember(Order = 11)]
        public string MM { get; set; }

        /// <summary>
        /// 优选结果汇总
        /// </summary>
        [DataMember(Order = 12)]
        public List<PurSelectDetail> DetailData { get; set; }
    }
}
