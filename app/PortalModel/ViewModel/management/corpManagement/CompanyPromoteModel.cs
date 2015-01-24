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
    [Api("恩维协同企业推广维护接口")]
    [Route("/{AccountID}/company/Promote", HttpMethods.Get, Notes = "企业推广产品信息")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/company/Promote", HttpMethods.Get, Notes = "获取推广产品信息")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/company/Promote/new", HttpMethods.Post, Notes = "维护企业推广产品信息")]
    [DataContract]
     public class CompanyPromoteRequest:IReturn<CompanyPromoteResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }
      
        [ApiMember(Description = "账套Id", ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        [DataMember(Order = 4)]
        [ApiMember(Description = "产品对象集合",
       ParameterType = "json", DataType = "List", IsRequired = true)]
        public List<CompanyPromoteInfo> CompanyPromote { get; set; }
        [DataMember(Order = 5)]
        [ApiMember(Description = "图片资源对象集合",
        ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]  
        public List<ReturnPicResources> PicResources { get; set; }
      //  [DataMember(Order = 5)]
      //  [ApiMember(Description = "图片资源和产品对象集合",
      //ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]       
      //  ReturnPicResources FristResoures { get; set; }
      //  [DataMember(Order = 6)]
      //  [ApiMember(Description = "图片资源和产品对象集合",
      //ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]
      //  ReturnPicResources SecondResoures { get; set; }
      //  [DataMember(Order = 7)]
      //  [ApiMember(Description = "图片资源和产品对象集合",
      //ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]
      //  ReturnPicResources ThirdResoures { get; set; }
      //  [DataMember(Order = 8)]
      //  [ApiMember(Description = "图片资源和产品对象集合",
      //ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]
      //  ReturnPicResources FourthResoures { get; set; }
      //  [DataMember(Order = 9)]
      //  [ApiMember(Description = "图片资源和产品对象集合",
      //ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]
      //  ReturnPicResources FifthResoures { get; set; }
        
    }
    [DataContract]
    public class CompanyPromoteInfo:IReturn<CompanyPromoteResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "产品推广Id",
      ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ID { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "企业产品Id",
      ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ItemID { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "企业产品编码",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ItemCode { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "企业产品名称",
      ParameterType = "json", DataType = "string", IsRequired = true)]

        public string ItemName { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "平台标准产品编码",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string StandardItemCode { get; set; }

        [DataMember(Order = 6)]
        [ApiMember(Description = "平台标准产品名称",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string StandardItemName { get; set; }
        [ApiMember(Description = "排序序号",
           ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
         public int OrderNumber { get; set; }

        [ApiMember(Description = "图片资源",
           ParameterType = "json", DataType = "ReturnPicResources", IsRequired = true)]
        [DataMember(Order = 8)]
        public ReturnPicResources PicResources { get; set; }
    }
    [DataContract]
    public class CompanyPromoteResponse
    {
        public CompanyPromoteResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public List<CompanyPromoteInfo> Data { get; set; }
       
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
