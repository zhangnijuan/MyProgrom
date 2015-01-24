using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同修改企业地址薄接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/address/modify", HttpMethods.Post, Notes = "修改企业地址薄")]
    [DataContract]
    public class ModifyERPAddressRequest : IReturn<EnterpriseAddressResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
               ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "公司ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long CompID { get; set; }

        [ApiMember(Description = "公司名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string CompName { get; set; }

        [ApiMember(Description = "所在地区(省份/直辖市)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string Province { get; set; }

        [ApiMember(Description = "市",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string City { get; set; }

        [ApiMember(Description = "区",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string District { get; set; }

        [ApiMember(Description = "详细地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string Address { get; set; }

        [ApiMember(Description = "邮政编码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string ZipCode { get; set; }

        [ApiMember(Description = "设为默认地址(0否,1是)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 11)]
        public int IsDef { get; set; }

        [ApiMember(Description = "ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 12)]
        public long ID { get; set; }

        [ApiMember(Description = "总地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 13)]
        public string FullAddress { get; set; }

        [ApiMember(Description = "登录人ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 14)]
        public long UserID { get; set; }

        [ApiMember(Description = "登录人code",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 15)]
        public string UserCode { get; set; }
    }
}
