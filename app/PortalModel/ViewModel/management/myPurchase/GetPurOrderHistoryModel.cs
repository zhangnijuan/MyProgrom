using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品订单交易历史")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/purorder/history", HttpMethods.Post, Notes = "显示交易历史")]
    [DataContract]
    public class GetPurOrderHistoryRequest : IReturn<GetPurOrderHistoryResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [ApiMember(Description = "询价方帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountId { get; set; }

        [ApiMember(Description = "询价方平台产品代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "询价方平台产品名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string StandardItemName { get; set; }
    }

    [DataContract]
    public class GetPurOrderHistoryResponse
    {
        public GetPurOrderHistoryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "交易历史集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<PurOrderHistory> Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    [DataContract]
    public class PurOrderHistory
    {
        /// <summary>
        /// 采购订单明细id
        /// </summary>
        [DataMember(Order = 1)]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 采购方帐套
        /// </summary>
        [DataMember(Order = 2)]
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [DataMember(Order = 3)]
        [Alias("orderdate")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [DataMember(Order = 4)]
        [Alias("su_n")]
        public string SupplyName { get; set; }

        /// <summary>
        /// 产品单价
        /// </summary>
        [DataMember(Order = 5)]
        [Alias("prc")]
        public decimal Prc { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        [Alias("sqty")]
        [DataMember(Order = 6)]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Alias("u_n")]
        [DataMember(Order = 7)]
        public string UnitName { get; set; }

        /// <summary>
        /// 发货地
        /// </summary>
        [Alias("saddress")]
        [DataMember(Order = 8)]
        public string SAddress { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [Alias("standarditemname")]
        [DataMember(Order = 9)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 报价方帐套
        /// 后端使用
        /// </summary>
        [Alias("sa")]
        [DataMember(Order = 10)]
        public int SAccountID { get; set; }
    }
}
