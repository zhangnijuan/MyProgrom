using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同生成订单接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/{source}/purorder/new", HttpMethods.Post, Notes = "保存订单信息")]
    [Route("/web/shopping/to/order", HttpMethods.Post, Notes = "保存购物车信息")]
    [DataContract]
    public class CreatePurOrderRequest : IReturn<CreatePurOrderResponse>
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

        [ApiMember(Description = "询价主档ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long InquiryID { get; set; }

        [ApiMember(Description = "优选单ID",
               ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public long PurSelectID { get; set; }

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
        /// 角色权限ID
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }

        [DataMember(Order = 12)]
        [ApiMember(Description = "查询类型 优选状态 = Select，购物车状态 = ShoppingCart",
      ParameterType = "json", DataType = "SourceEnum", IsRequired = true)]
        public SourceEnum Source { get; set; }

        [DataMember(Order = 1)]
        [ApiMember(Description = "产品集合",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public List<SimpleShoppingCartItem> SimpleShoppingCartItem { get; set; }
    }

    [DataContract]
    public class SimpleShoppingCartItem 
    {
        [ApiMember(Description = "产品ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long ID { get; set; }

        [ApiMember(Description = "数量",
                ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int Quantity { get; set; }
    }

    [DataContract]
    public class CreatePurOrderResponse
    {
        public CreatePurOrderResponse()
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
