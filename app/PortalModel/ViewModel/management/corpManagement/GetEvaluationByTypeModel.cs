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
    [Api("恩维协同根据题目类型查询评价题目及选项接口")]
    [Route("/{AppKey}/{Secretkey}/project/search", HttpMethods.Post, Notes = "根据题目类型查询评价题目及选项信息")]
    [DataContract]
    public class GetEvaluationByTypeRequest : IReturn<GetEvaluationByTypeResponse>
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
        [ApiMember(Description = "题目类型 0 供应类 1 销售类",
      ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Type { get; set; }
    }

    [DataContract]
    public class GetEvaluationByTypeResponse
    {
        public GetEvaluationByTypeResponse()
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
        public List<Evaluation> Data { get; set; }
    }

    [DataContract]
    public class Evaluation
    {
        [ApiMember(Description = "评价题目ID",
                ParameterType = "json", DataType = "long", IsRequired = true)]
        [Alias("id")]
        [DataMember(Order = 1)]
        public long ProjectID { get; set; }

        [ApiMember(Description = "评价题目名称",
                       ParameterType = "json", DataType = "long", IsRequired = true)]
        [Alias("n")]
        [DataMember(Order = 1)]
        public string ProjectName { get; set; }

        /// <summary>
        /// 选项说明
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "选项说明",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        [Alias("optionmm")]
        public string OptionMM { set; get; }

        [ApiMember(Description = "评价选项集合",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public List<ProjectOption> ProjectOptionList { get; set; }

        [ApiMember(Description = "评价选项",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        [Alias("optionid")]
        public long OptionID { get; set; }

    }

    [DataContract]
    public class ProjectOption
    {
        [ApiMember(Description = "评价选项ID",
                ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        [Alias("id")]
        public long OptionID { get; set; }

        [ApiMember(Description = "评价选项名称",
                       ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        [Alias("n")]
        public string OptionName { get; set; }

    }
 
    
}
