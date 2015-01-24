using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同生成购物车接口")]
    [Route("/web/shopping/new", HttpMethods.Post, Notes = "保存购物车信息")]
    [DataContract]
    public class CreatePurShoppingCartRequest : IReturn<CreatePurShoppingCartResponse>
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

        [ApiMember(Description = "供应企业物品ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long ItemID { get; set; }

        [ApiMember(Description = "购买数量",
               ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 5)]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Eid { set; get; }
        /// <summary>
        /// 建档人编码
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }

        /// <summary>
        /// 建档人名称
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        /// <summary>
        /// 供应商云ID
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "供应商云ID",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public long Supply { get; set; }
    }

    [DataContract]
    public class CreatePurShoppingCartResponse
    {
        public CreatePurShoppingCartResponse()
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
