using System;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同根据产品类别获取产品属性接口")]
    [Route("/{AppKey}/{Secretkey}/property/list/{CategoryID}", HttpMethods.Get, Notes = "根据产品类别获取属性信息")]
    public class GetItemAttributeRequest : IReturn<GetItemAttributeResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
            ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "产品类别id",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string CategoryID { get; set; }
    }

    public class GetItemAttributeResponse
    {
        public GetItemAttributeResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "产品属性集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<ItemProperty> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回产品属性资料
    /// </summary>
    public class ItemProperty : IReturn<GetItemAttributeResponse>
    {
        /// <summary>
        /// 产品属性id
        /// 需使用string类型
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 产品属性代码
        /// </summary>
        public string PropertyCode { get; set; }

        /// <summary>
        /// 产品属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 产品属性值
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        /// 产品类别id
        /// </summary>
        public string CategoryID { get; set; }
    }
}
