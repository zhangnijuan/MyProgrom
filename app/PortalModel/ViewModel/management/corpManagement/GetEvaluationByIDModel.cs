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
    [Api("恩维协同根据ID查询评价题目及选项接口")]
    [Route("/{AppKey}/{Secretkey}/project/search/{ID}", HttpMethods.Get, Notes = "根据ID查询评价题目及选项信息")]
    [DataContract]
    public class GetEvaluationByIDRequest : IReturn<GetEvaluationByIDResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "评价单ID",
      ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ID { get; set; }
    }

    [DataContract]
    public class GetEvaluationByIDResponse
    {
        public GetEvaluationByIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public EvaluationInfo Data { get; set; }
    }

    [DataContract]
    public class EvaluationInfo
    {

        [DataMember(Order = 8)]
        public List<Evaluation> EvaluationDetail { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [Alias("mm")]
        public string MM { set; get; }
    }

   
}
