using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同购物车展示接口")]
    [Route("/web/shopping/search", HttpMethods.Post, Notes = "购物车展示信息")]
    [DataContract]
    public class GetPurShoppingCartByEIDRequest : IReturn<GetPurShoppingCartByEIDResponse>
    {

        [ApiMember(Description = "帐套",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        /// <summary>
        /// 建档人ID
        /// </summary>
        [DataMember(Order = 6)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EID { set; get; }

    }

    [DataContract]
    public class GetPurShoppingCartByEIDResponse
    {
        public GetPurShoppingCartByEIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public List<ShoppingCartItemGrop> Data { get; set; }
    }
    [DataContract]
    public class ShoppingCartItemGrop 
    {
        [DataMember(Order = 1)]
        public string CompID { get; set; }
         [DataMember(Order = 2)]
        public string CompName { get; set; }

        [DataMember(Order = 3)]
         public List<ShoppingCartItem> ShoppingCartItem { get; set; }

        [DataMember(Order = 4)]
        public int CompAccountID { get; set; }
    }

    [DataContract]
    public class ShoppingCartItem 
    {
        [ApiMember(Description = "产品ID",
                 ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public string ID { get; set; }

        [ApiMember(Description = "帐套",
                 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [ApiMember(Description = "制单人",
                 ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public long EID { get; set; }

        [ApiMember(Description = "制单人code",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string EIDCode { get; set; }

        [ApiMember(Description = "制单人Name",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string EIDName { get; set; }

        [ApiMember(Description = "平台标准产品编码",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string StandardItemCode { get; set; }

        [ApiMember(Description = "平台标准产品名称",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string StandardItemName { get; set; }

        [ApiMember(Description = "企业产品分类编码",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string CategoryCode { get; set; }

        [ApiMember(Description = "企业产品分类名称",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string CategoryName { get; set; }

        [ApiMember(Description = "产品ID",
                 ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string ItemID { get; set; }

        [ApiMember(Description = "产品代码",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string ItemCode { get; set; }

        [ApiMember(Description = "产品名称",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string ItemName { get; set; }

        [ApiMember(Description = "单位名称",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        public string UnitName { get; set; }

        [ApiMember(Description = "数量",
                ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 14)]
        public decimal SelectQty { get; set; }

        [ApiMember(Description = "产品描述",
                ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 15)]
        public string Remark { get; set; }

        [ApiMember(Description = "单价",
                ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 16)]
        public decimal Prc { get; set; }

        [ApiMember(Description = "金额",
                ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 17)]
        public decimal Amt { get; set; }

        [ApiMember(Description = "属性值",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 18)]
        public string PropertyName { get; set; }
        
        [ApiMember(Description = "供应企业名称",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 19)]
        public string CompName { get; set; }

        [ApiMember(Description = "供应企业云ID",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 20)]
        public string CompID { get; set; }

        [ApiMember(Description = "总报价金额",
           ParameterType = "json", DataType = "decimal", IsRequired = true)]
        [DataMember(Order = 21)]
        public decimal TotalAmt { get; set; }

        [ApiMember(Description = "备注",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 22)]
        public string MM { get; set; }

        [ApiMember(Description = "交货日期",
           ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 23)]
        public string DeliveryDate { get; set; }

        [ApiMember(Description = "状态 1启用 0停用",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 24)]
        public int State { get; set; }

        [ApiMember(Description = "属性",
         ParameterType = "json", DataType = "Attributes", IsRequired = true)]
        [DataMember(Order = 25)]
        public List<ItemAttribute> PropertyList { get; set; }


    }
}
