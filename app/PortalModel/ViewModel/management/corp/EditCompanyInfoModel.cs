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
    [Api("恩维协同维护公司信息提交接口")]
    [Route("/{AppKey}/{Secretkey}/companyinfo/modify", HttpMethods.Post, Notes = "维护公司信息提交请求url")]
    [DataContract]
    public class EditCompanyInfoRequest : IReturn<EditCompanyInfoResponse>
    {

        [ApiMember(Description = "账套Id",
                  ParameterType = "path", DataType = "int", IsRequired = true)]
        [DataMember(Order = 1)]
        public int AccountID { get; set; }
        [ApiMember(Description = "公司名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string CompName { get; set; }

      
        [ApiMember(Description = "公司电话",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string TelPhone { get; set; }

      
        [ApiMember(Description = "公司传真",
               ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 4)]
        public string Fax { get; set; }

       
        [ApiMember(Description = "公司网站地址",
               ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 5)]
        public string WebUrl { get; set; }
       
        [ApiMember(Description = "联系人",
               ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 6)]
        public string Contacts { get; set; }
       
        [ApiMember(Description = " 联系人电话",
               ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 7)]
        public string ContactsTel { get; set; }

        [ApiMember(Description = "公司规模id",
              ParameterType = "json", DataType = "int", IsRequired = false)]
        [DataMember(Order = 8)]
        public int ScaleId { get; set; }
        
        [ApiMember(Description = "公司规模",
             ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string CompanyScale { get; set; }

        [ApiMember(Description = "公司面积",
             ParameterType = "json", DataType = "string", IsRequired = false)]
        [DataMember(Order = 10)]
        public string CompanyArea { get; set; }
        
        [ApiMember(Description = "主营产品",
           ParameterType = "json", DataType = "MainProduct", IsRequired = false)]
        [DataMember(Order = 11)]
        public List<MainProduct> MainProducts { get; set; }


        [DataMember(Order = 12)]
        [ApiMember(Description = "图片信息对象",
            ParameterType = "json", DataType = "PicResources", IsRequired = true)]
        public ReturnPicResources PicResources { get; set; }

        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
    }
    [DataContract]
    public class EditCompanyInfoResponse
    {
        public EditCompanyInfoResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
         [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    [DataContract]
    public class MainProduct
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [DataMember(Order = 1)]
        public long Id { get; set; }
        /// <summary>
        /// 产品类别编码
        /// </summary>
        [DataMember(Order = 2)]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 产品类别名称
        /// </summary>
        [DataMember(Order = 3)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        [DataMember(Order = 4)]
        public long ParentID { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember(Order = 5)]
        public long CompID { get; set; }
      
    }
}
