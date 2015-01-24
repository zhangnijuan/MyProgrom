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
    [Api("展示订阅接口")]
    [Route("/{AppKey}/{Secretkey}/subscribe/search", HttpMethods.Post, Notes = "展示订阅接口")]
    [DataContract]
    public class SubscriptionListRequest : IReturn<SubscriptionListResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "订阅者ID",
            ParameterType = "json", DataType = "long")]
        [DataMember(Order = 3)]
        public long Subscriber { get; set; }
        [ApiMember(Description = "订阅者ID",
            ParameterType = "json", DataType = "long")]
        [DataMember(Order = 4)]
        public int AccountID { get; set; }
    }
    [DataContract]
    public class SubscriptionListResponse 
    {
        public SubscriptionListResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public SubscriptionList Data { get; set; }
        [DataMember(Order = 2)]
        public bool Success { get; set; }
      
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
 

    }
    [DataContract]
    public class SubscriptionList 
    {
        [DataMember(Order = 1)]
        public List<ReturnSubscription> Subscription { get; set; }
        [DataMember(Order = 2)]
        public List<CategoryName> Category { get; set; }
    }
    /// <summary>
    /// 分类和查到的订单数量
    /// </summary>
    [DataContract]
    public class CategoryName
	{
        [DataMember(Order = 1)]
		public string Name{get;set;}
        [DataMember(Order = 2)]
        public  int Count{get;set;}
	}
    [DataContract]
    public class ReturnSubscription
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [DataMember(Order = 1)]

        public long ID { get; set; }

        /// <summary>
        /// 被订阅企业标识
        /// </summary>
        [DataMember(Order = 2)]
        public int ToAccountID { get; set; }

        /// <summary>
        /// 0 询价信息 1 产品信息
        /// </summary>

        [DataMember(Order = 3)]
        public int Subtype { get; set; }

        /// <summary>
        /// 订阅主题
        /// </summary>
        [DataMember(Order = 4)]
        public string SubTheme { get; set; }

        /// <summary>
        /// 订阅者ID
        /// </summary>
        [DataMember(Order = 5)]
        public long Subscriber { get; set; }

        /// <summary>
        /// 订阅者编码
        /// </summary>
        [DataMember(Order = 6)]
        public string SubscriberCode { get; set; }

        /// <summary>
        /// 订阅者名称
        /// </summary>
        [DataMember(Order = 7)]
        public string SubscriberName { get; set; }


        /// <summary>
        /// 交货地址
        /// </summary>
        [DataMember(Order = 8)]
        public string DeliveryAddress { get; set; }



        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 9)]
        public List<string> CategoryName { get; set; }

        /// <summary>
        /// 报价最小数量
        /// </summary>
        [DataMember(Order = 10)]
        public decimal MinQty { get; set; }


        /// <summary>
        /// 报价最大数量
        /// </summary>
        [DataMember(Order = 11)]
        public decimal MaxQty { get; set; }
        /// <summary>
        /// 最后一次查询时间
        /// </summary>
        [DataMember(Order = 12)]
        public DateTime LastTime { get; set; }
        /// <summary>
        /// 最新询价的数量
        /// </summary>
        [DataMember(Order = 13)]
        public int Count { get; set; }
        [DataMember(Order = 14)]
        public string FromName { get; set; }
    }
}
