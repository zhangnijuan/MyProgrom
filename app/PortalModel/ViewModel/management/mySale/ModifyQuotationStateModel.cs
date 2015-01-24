using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同更改报价单已忽略状态接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/quotation/modify", HttpMethods.Post, Notes = "显示待报价列表状态为已忽略")]
    [DataContract]
    public class ModifyQuotationStateRequest : IReturn<ModifyQuotationStateResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "账套Id",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [ApiMember(Description = "字段条件",
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 4)]
        public List<SearchIgnoreStateField> SearchCondition { get; set; }
    }
    [DataContract]
    public class SearchIgnoreStateField
    {
        [DataMember(Order = 1)]
        public SearchIgnoreStateEnum SeacheKey { get; set; }
        [DataMember(Order = 2)]
        public string Value { get; set; }

    }
    /// <summary>
    /// 查询字段枚举 产品分类，产品名称或代码
    /// </summary> 
    [DataContract]
    public enum SearchIgnoreStateEnum
    {
        [Description("询价单ID")]
        ID,
        [Description("状态")]
        State
    }
    [DataContract]
    public class ModifyQuotationStateResponse
    {
        public ModifyQuotationStateResponse()
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
