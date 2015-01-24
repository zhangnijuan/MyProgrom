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
    [Api("恩维协同获取需要修改员工详细信息接口")]
    [Route("/{AppKey}/{Secretkey}/employee/list/{Id}", HttpMethods.Get, Notes = "根据ID获取员工信息")]
    [DataContract]
    public class GetEmployeeByIdRequest: IReturn<GetEmployeeByIdResponse>
    {

        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]

        public string Secretkey { get; set; }
        [ApiMember(Description = "员工Id",
                 ParameterType = "path", DataType = "long", IsRequired = true)]
        [DataMember(Order = 3)]
        public long Id { get; set; }
        
    }
    [DataContract]
    public class GetEmployeeByIdResponse
    {
        public GetEmployeeByIdResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public ReturnList Data { get; set; }
        [DataMember(Order = 2)]
        public ResponseStatus ResponseStatus { get; set; }
        [DataMember(Order = 3)]
        public bool Success { get; set; }
    }

    [DataContract]
    public class ReturnPicResources1
    {

        [ApiMember(Description = "主键Id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [DataMember(Order = 3)]
        public long DocumentID { get; set; }
        [ApiMember(Description = "文件原始名称",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string OriginalName { get; set; }

        [ApiMember(Description = "平台修改后的文件名称",
               ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 5)]
        public string NewName { get; set; }

        [ApiMember(Description = "文件后缀名",
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string Suffix { get; set; }

        [ApiMember(Description = "文件大小",
              ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 7)]
        public string FileLength { get; set; }
        [DataMember(Order = 8)]
        public string FileUrl { get; set; }
    }
}
