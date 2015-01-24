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
    [Api("恩维协同单证操作日志接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/purorder/{SRCID}", HttpMethods.Get, Notes = "获取单证操作日志")]
    [DataContract]
    public class GetStateLogRequest : IReturn<GetStateLogResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "来源单证主表ID", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string SRCID { get; set; }

    }
    [DataContract]
    public class GetStateLogResponse
    {
        public GetStateLogResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public OrderStateLog Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 1)]
        public bool Success { get; set; }
    }

    [DataContract]
    public class OrderStateLog
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "主键",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        [Alias("id")]
        public long ID { get; set; }

        [DataMember(Order = 1)]
        [ApiMember(Description = "来源单证主表ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        [Alias("sid")]
        public long SRCID { get; set; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 24)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        [Alias("eid")]
        public long Eid { set; get; }

        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 25)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [Alias("eid_syscode")]
        public string EidCode { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 26)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [Alias("eid_usrname")]
        public string EidName { set; get; }

        [ApiMember(Description = "创建时间",
             ParameterType = "json", DataType = "string")]
        [DataMember(Order = 14)]
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        [ApiMember(Description = "操作后单证状态",
         ParameterType = "json", DataType = "int")]
        [DataMember(Order = 16)]
        [Alias("state")]
        public int State { get; set; }

        [ApiMember(Description = "取消理由备注",
          ParameterType = "json", DataType = "string")]
        [DataMember(Order = 23)]
        [Alias("firstmm")]
        public string FirstMM { get; set; }

        [ApiMember(Description = "备注说明",
          ParameterType = "json", DataType = "string")]
        [DataMember(Order = 23)]
        [Alias("secondmm")]
        public string SecondMM { get; set; }
    }
}
