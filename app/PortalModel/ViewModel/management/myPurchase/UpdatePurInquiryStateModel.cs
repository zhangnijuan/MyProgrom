using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同更新询价状态接口")]
    [Route("/{AppKey}/{Secretkey}/purchase/updatestate", HttpMethods.Post, Notes = "保存询价信息")]
    [DataContract]
    public class UpdatePurInquiryStateRequest : IReturn<UpdatePurInquiryStateResponse>
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
        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        /// <summary>
        /// 状态ID
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "状态ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int State { set; get; }

    
        [DataMember(Order = 5)]
        [ApiMember(Description = "状态编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string State_Enum { set; get; }

        [ApiMember(Description = "询价代码",
                    ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string InquiryCode { get; set; }

    }

    [DataContract]
    public class UpdatePurInquiryStateResponse
    {
        public UpdatePurInquiryStateResponse()
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
