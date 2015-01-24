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
    [Api("恩维协同企业简介维护接口")]
    [Route("/{AccountID}/company/intro", HttpMethods.Get, Notes = "获取企业门户简介信息")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/company/intro", HttpMethods.Get, Notes = "获取企业简介信息")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/company/intro/new", HttpMethods.Post, Notes = "维护企业信息简介")]
    [DataContract]
    public class CompanyIntroRequest:IReturn<CompanyIntroResponse>
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


        [ApiMember(Description = "公司Id", ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long CompId { get; set; }

        [ApiMember(Description = "公司简介描述", ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CompanyDetails { get; set; }

        [ApiMember(Description = "公司展示图片集合", ParameterType = "json", DataType = "List<ReturnPicResources>", IsRequired = true)]
        [DataMember(Order = 6)]
        public List<ReturnPicResources> ResourcesImages { get; set; }

    }
    [DataContract]
    public class CompanyIntroResponse
    {
        public CompanyIntroResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public CompanyIntroInfo Data { get; set; }
       
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    [DataContract]
    public class CompanyIntroInfo
    {
        [DataMember(Order = 2)]
        public string CompanyDetails { get; set; }

         [DataMember(Order = 1)]
       public  List<ReturnPicResources> PicResources { get; set; }
    }
}
