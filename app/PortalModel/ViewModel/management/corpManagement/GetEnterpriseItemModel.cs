using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取产品档案接口")]
    [Route("/enterpriseItem/list", HttpMethods.Post, Notes = "自定义条件查询产品(含分页)")]
    [DataContract]
    public class GetEnterpriseItemRequest : IReturn<GetEnterpriseItemReponse>
    {
        [ApiMember(Description = "第几页",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [ApiMember(Description = "产品代码",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public string ItemName { get; set; }

        [ApiMember(Description = "产品分类代码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CategoryCode { get; set; }

        [ApiMember(Description = "产品分类名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string CategoryName { get; set; }
    }

    public class GetEnterpriseItemReponse
    {
        public GetEnterpriseItemReponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "企业产品资料集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<EnterpriseItemView> Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回企业产品资料
    /// </summary>
    [DataContract]
    public class EnterpriseItemView : IReturn<GetEnterpriseItemReponse>
    {
        /// <summary>
        /// 产品id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [DataMember(Order = 3)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Order = 4)]
        public string ItemName { get; set; }

        /// <summary>
        /// 产品分类id
        /// </summary>
        [DataMember(Order = 4)]
        public long CategoryID { get; set; }

        /// <summary>
        /// 产品分类代码
        /// </summary>
        [DataMember(Order = 5)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 产品分类名称
        /// </summary>
        [DataMember(Order = 6)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        [DataMember(Order = 7)]
        public string UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 8)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 9)]
        public string UnitName { get; set; }

        /// <summary>
        /// 标准售价
        /// </summary>
        [DataMember(Order = 10)]
        public string SalPrc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember(Order = 13)]
        public string Remark { get; set; }
    }
}
