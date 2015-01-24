using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同删除修改购物车接口")]
    [Route("/web/shopping/modify", HttpMethods.Post, Notes = "删除修改购物车信息")]
    [DataContract]
    public class DeletePurShoppingCartItemRequest : IReturn<DeletePurShoppingCartItemResponse>
    {
      //  [DataMember(Order = 1)]
      //  [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string AppKey { get; set; }

      //  [DataMember(Order = 2)]
      //  [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      //ParameterType = "path", DataType = "string", IsRequired = true)]
      //  public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
           ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "删除条件", 
ParameterType = "json", DataType = "List", IsRequired = true)]
        [DataMember(Order = 1)]
        public List<DeleteCartList> DeleteCondition { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "数量",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public string SelectQty { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "总金额",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Amt { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "建档人ID",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EID { set; get; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "供应企业云ID",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string CompID { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "标识ID",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ID { get; set; }

        [DataMember(Order = 7)]
        [ApiMember(Description = "供应企业物品ID",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ItemID { get; set; }

        

    }

    [DataContract]
    public class DeletePurShoppingCartItemResponse
    {
        public DeletePurShoppingCartItemResponse()
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
   
    [DataContract]
    public class DeleteCartList
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "标识ID",
         ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ID { get; set; }

        
        [DataMember(Order = 4)]
        [ApiMember(Description = "建档人ID",
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EID { set; get; }

        //[DataMember(Order = 4)]
        //[ApiMember(Description = "供应企业云ID",
        //ParameterType = "json", DataType = "string", IsRequired = true)]
        //public string CompID { get; set; }
        [ApiMember(Description = "帐套",
   ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }
 
    }
}
