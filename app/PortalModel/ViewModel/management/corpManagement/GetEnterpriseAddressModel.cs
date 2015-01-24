using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同获取企业地址簿接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/address/search", HttpMethods.Post, Notes = "获取企业地址薄")]
    [DataContract]
    public class GetEnterpriseAddressRequest : IReturn<GetERPAddressResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
                ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "第几页(不需要分页不用传参)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int PageIndex { get; set; }

        [ApiMember(Description = "每页显示的笔数(不需要分页不用传参)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 4)]
        public int PageSize { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int AccountID { get; set; }

        [ApiMember(Description = "登录人ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 6)]
        public long UserID { get; set; }

        [ApiMember(Description = "登录人code",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string UserCode { get; set; }

        [ApiMember(Description = "默认地址现在在第一位标识(Y)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string IsDefFirst { get; set; }
    }

        /// <summary>
    /// 返回对象
    /// </summary>
    [DataContract]
    public class GetERPAddressResponse
    {
        public GetERPAddressResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [ApiMember(Description = "企业地址簿资料集合", DataType = "List", IsRequired = true)]
        [DataMember(Order = 2)]
        public List<ERPAddressView> Data { get; set; }

        [ApiMember(Description = "总笔数", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int RowsCount { get; set; }

        [DataMember(Order = 4)]
        public ResponseStatus ResponseStatus { get; set; }
    }

    /// <summary>
    /// 返回的企业地址簿资料
    /// </summary>
    [DataContract]
    public class ERPAddressView : IReturn<GetERPAddressResponse>
    {
        [ApiMember(Description = "地址簿ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string ID { get; set; }

        [ApiMember(Description = "帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [ApiMember(Description = "公司ID",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 3)]
        public string CompID { get; set; }

        [ApiMember(Description = "公司名称",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string CompName { get; set; }

        [ApiMember(Description = "所在地区(省份/直辖市)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string Province { get; set; }

        [ApiMember(Description = "所在地区(城市)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string City { get; set; }

        [ApiMember(Description = "所在地区(地区)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string District { get; set; }

        [ApiMember(Description = "详细地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string Address { get; set; }

        [ApiMember(Description = "邮政编码",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 9)]
        public string ZipCode { get; set; }

        [ApiMember(Description = "设为默认地址(0否,1是)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 10)]
        public int IsDef { get; set; }

        [ApiMember(Description = "总地址",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string FullAddress { get; set; }
    }
}
