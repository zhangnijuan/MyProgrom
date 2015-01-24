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
    #region 导入平台标准物品
    [DataContract]
    [Route("/{AppKey}/{Secretkey}/Import/UdocItem", HttpMethods.Post)]
    [Api("导入平台标准产品档案api")]
    public class ImportItemRequest
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
        ParameterType = "json", DataType = "string", IsRequired = true)]
        public int AccountID { get; set; }


    }
    [DataContract]
    public class ImportItemResponse : IHasResponseStatus
    {
        public ImportItemResponse()
        {
            this.ResponseStatus = new ResponseStatus();
            thePath = "";
        }
        [DataMember(Order = 1)]
        public bool Sucess { get; set; }

        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 3)]
        public string thePath { get; set; }
    }
    #endregion
}
