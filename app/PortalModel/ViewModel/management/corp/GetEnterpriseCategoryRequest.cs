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
    [Api("恩维协同获取企业产品分类接口(平台与企业共用分类)")]
    [Route("/{AppKey}/{Secretkey}/category/search", HttpMethods.Get, Notes = "根据上级分类ID,分类名称模糊搜索产品分类")]
    [DataContract]
    public class GetEnterpriseCategoryRequest : IReturn<GetEnterpriseCategoryResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
         ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }
        [ApiMember(Description = "企业ID",
          ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }
        [ApiMember(Description = "上级类别ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string ParentID { get; set; }

        [ApiMember(Description = "根据名称模糊搜索分类资料(传Y即可)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string LikeSearch { get; set; }

        [ApiMember(Description = "类别名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string CategoryName { get; set; }
    }

    [DataContract]
    public class GetEnterpriseCategoryResponse
    {
        public GetEnterpriseCategoryResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "产品类别集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<Category> Data { get; set; }

        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
}
