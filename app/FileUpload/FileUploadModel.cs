using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.FileUpload
{
    #region 上传图片
    [DataContract]
    [Route("/{AppKey}/{Secretkey}/fileuploads/{AccountId}", HttpMethods.Post)]
    [Api("文件上传下载api")]
    public   class FileUploadRequest
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
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AccountID { get; set; }

        [DataMember]
        [ApiMember(Description = "文件相对路径",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string RelativePath { get; set; }


        [DataMember]
        [ApiMember(Description = "自定义文件ID",
    ParameterType = "path", DataType = "string", IsRequired = true)]
        public string NewName { get; set; }

        [DataMember]
        [ApiMember(Description = "文件后缀名，用户获取文件",
    ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Suffix { get; set; }

        [DataMember]
        [ApiMember(Description = "缩略图",
    ParameterType = "path", DataType = "Thumbnail", IsRequired = true)]
        public Thumbnail Thumbnail { get; set; }
        [DataMember]
        [ApiMember(Description = "不生成缩略图（",
    ParameterType = "path", DataType = "Thumbnail", IsRequired = true)]
        public bool NoThumbnail { get; set; }
    }
    [DataContract]

    public class FileUploadResponse : IHasResponseStatus
    {
        public FileUploadResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
        [DataMember(Order = 1)]
        public bool Success { get; set; }
        [DataMember(Order = 2)]
        public ReturnData Data { get; set; }
         [DataMember(Order = 3)]
        public ResponseStatus ResponseStatus { get; set; }
    }
    public class ReturnData
    {
        public ReturnResources PicResources { get; set; }
    }
    [DataContract]
    public class ReturnResources
    {
          [DataMember]
        public string OriginalName { get; set; }

        [DataMember]
        public string FileLength { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public string Contents { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string NewName { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public string Suffix { get; set; }
    }


    #endregion
    
    public enum Mode
    {
        Cut,
        HW,
        W,
        H
    }

    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
    }
}
