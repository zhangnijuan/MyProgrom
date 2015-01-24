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
    #region 导入企业特有物品
    [DataContract]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/Import/UdocEPItem/{BizType}", HttpMethods.Post)]
    [Api("导入企业采购产品or供应产品api")]
    public class ImportEPItemRequest : IReturn<ImportEPItemResponse>
    {
        [DataMember]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
        ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember]
        [ApiMember(Description = "帐套",
        ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountID { get; set; }

        [DataMember]
        [ApiMember(Description = "业务类型1采购产品 2供应产品",
        ParameterType = "json", DataType = "int", IsRequired = true)]
        public int BizType { get; set; }
    }
    [DataContract]
    public class ImportEPItemResponse : IHasResponseStatus
    {
        public ImportEPItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    #endregion
}
