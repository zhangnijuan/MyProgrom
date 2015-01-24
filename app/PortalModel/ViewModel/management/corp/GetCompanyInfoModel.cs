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
    [Api("恩维协同获取公司信息接口")]
    [Route("/companyInfo/{AccountId}", HttpMethods.Get, Notes = "根据账套获取企业信息")]
    [Route("/{AppKey}/{Secretkey}/companyInfo/{AccountId}", HttpMethods.Get, Notes = "根据账套获取企业信息")]
    [DataContract]
    public class GetCompanyInfoRequest : IReturn<GetCompanyInfoResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
        [ApiMember(Description = "账套Id",
                  ParameterType = "path", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountId { get; set; }
    }
    [DataContract]
    public class GetCompanyInfoResponse
    {
        public GetCompanyInfoResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public ReturnCompanyInfo Data { get; set; }
        [DataMember(Order = 2)]
        public bool Success { get; set; }
        [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回到前端的model
    /// </summary>
    [DataContract]
    public class ReturnCompanyInfo
    {
        

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember(Order = 1)]
        public string CompName { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        [DataMember(Order = 2)]
        public string TelPhone { get; set; }

        /// <summary>
        /// 公司传真
        /// </summary>
        [DataMember(Order = 3)]
        public string Fax { get; set; }

        /// <summary>
        /// 公司网站
        /// </summary>
        [DataMember(Order = 4)]
        public string WebUrl { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(Order = 5)]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember(Order = 6)]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 公司规模id
        /// </summary>
        [DataMember(Order = 7)]
        public int ScaleId { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        [DataMember(Order = 8)]
        public string CompanyScale { get; set; }

        /// <summary>
        /// 公司面积
        /// </summary>
        [DataMember(Order = 9)]
        public string CompanyArea { get; set; }
     

        /// <summary>
        /// 图片资源对象
        /// </summary>
        [DataMember(Order = 10)]
        public ReturnPicResources PicResources { get; set; }
        /// <summary>
        /// 主营产品
        /// </summary>
        [DataMember(Order = 11)]
        public List<ReturnMainProduct> MainProducts { get; set; }
        [DataMember(Order = 12)]
        public string CompAddress { get; set; }
         [DataMember(Order = 13)]
        public string CompNature { get; set; }
         [DataMember(Order = 14)]
         public long ID { get; set; }


    }
    /// <summary>
    /// 公司主营产品返回对象
    /// </summary>
    [DataContract]
    public class ReturnMainProduct
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [DataMember(Order = 1)]
        public long Id { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }
        /// <summary>
        /// 产品类别编码
        /// </summary>
        [DataMember(Order = 3)]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 产品类别名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        [DataMember(Order = 5)]
        public long ParentID { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember(Order = 6)]
        public long CompID { get; set; }
    }
}
