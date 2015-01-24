using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Ndtech.PortalModel.ViewModel
{
    [Api("创建订阅采购信息接口")]
    [Route("/{AppKey}/{Secretkey}/subscribe/new", HttpMethods.Post, Notes = "创建订阅采购信息接口")]
    [DataContract]
    public class CreateSubscriptionRequest : IReturn<CreateSubscriptionResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "订阅主题",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 3)]
        public string SubTheme { get; set; }

        [ApiMember(Description = "订阅供应商Id",
            ParameterType = "json", DataType = "int")]
        [DataMember(Order = 4)]
        public int ToAccountID { get; set; }
        [ApiMember(Description = "交货地址",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 5)]
        public string DeliveryAddress { get; set; }
        [ApiMember(Description = "产品分类名称",
          ParameterType = "json", DataType = "string")]
        [DataMember(Order = 6)]
        public List<string> CategoryName { get; set; }

        [ApiMember(Description = "报价最小数量",
           ParameterType = "json", DataType = "string")]
        [DataMember(Order = 7)]
        public decimal MinQty { get; set; }

        [ApiMember(Description = "报价最大数量",
          ParameterType = "json", DataType = "string")]
        [DataMember(Order = 8)]
        public decimal MaxQty { get; set; }
        [ApiMember(Description = "订阅者Id",
            ParameterType = "json", DataType = "string")]
        [DataMember(Order = 9)]
        public long Subscriber { get; set; }

        [ApiMember(Description = "订阅者编码",
          ParameterType = "json", DataType = "string")]
        [DataMember(Order = 10)]
        public string SubscriberCode { get; set; }
        [ApiMember(Description = "订阅者姓名",
         ParameterType = "json", DataType = "string")]
        [DataMember(Order = 11)]
        public string SubscriberName { get; set; }
        /// <summary>
        /// 被订阅企业标识
        /// </summary>
        [ApiMember(Description = "采购商",
         ParameterType = "json", DataType = "string")]
        [DataMember(Order = 12)]
        public string FromName { get; set; }
    }
    [DataContract]
    public class CreateSubscriptionResponse
    {
        public CreateSubscriptionResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
