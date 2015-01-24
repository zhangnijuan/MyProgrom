using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同打开页面后点击评价接口")]
    [Route("/{AppKey}/{Secretkey}/evaluation/new", HttpMethods.Post, Notes = "生成评价信息")]
    [DataContract]
    public class CreatePurEvaluationRequest : IReturn<CreatePurEvaluationResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "订单ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long OrderID { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Ename { set; get; }

        /// <summary>
        /// 评价集合
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "评价集合",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public List<EvaluationList> EvaluationList { set; get; }

        /// <summary>
        ///备注
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "备注",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string MM { set; get; }
    }

    [DataContract]
    public class EvaluationList
    {
        [ApiMember(Description = "评价题目ID",
                ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ProjectID { get; set; }

        [ApiMember(Description = "评价选项ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ProjectOptionID { get; set; }

        /// <summary>
        /// 选项说明
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "选项说明",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string OptionMM { set; get; }
    }

   
    [DataContract]
    public class CreatePurEvaluationResponse
    {
        public CreatePurEvaluationResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
