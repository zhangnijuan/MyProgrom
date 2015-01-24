using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Ndtech.PortalModel
{
    /// <summary>
    /// 资源对象
    /// </summary>
    [DataContract]
    public class ReturnPicResources
    {
        [ApiMember(Description = "主键Id",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        [DataMember(Order = 3)]
        public string DocumentID { get; set; }
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
              ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string FileLength { get; set; }

        [DataMember(Order = 8)]
        public string FileUrl { get; set; }
    }
}
