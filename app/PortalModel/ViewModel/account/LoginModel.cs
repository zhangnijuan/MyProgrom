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
    [Api("恩维协同身份验证接口")]
    [Route("/login",HttpMethods.Post, Notes = "用户登陆请求url")]
    [DataContract]
    public class NdtechAuthRequest : IReturn<NdtechAuthResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "登陆名",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }
        [DataMember(Order = 2)]
        [ApiMember(Description = "密码",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Password { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "是否保存cookie",
          ParameterType = "json", DataType = "string", IsRequired = true)]
        public bool? RememberMe { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "校验方式 User = 普通用户校验;App = 第三方接口校验",
          ParameterType = "json", DataType = "AuthProvide", IsRequired = true)]
        public AuthProvide Provide { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "客户端类型 Web = 浏览器 IOS= IOS客户端 Android = Android客户端",
          ParameterType = "json", DataType = "ClientType", IsRequired = true)]
        public ClientType Client { get; set; }

    }

    /// <summary>
    /// 返回公司的数据
    /// </summary>
    [DataContract]
    public class UserInfo : IReturn<NdtechAuthResponse>
    {
       
        /// <summary>
        ///用户ID
        /// </summary>

        [DataMember(Order = 1)]
        public string UserID { get; set; }

        /// <summary>
        ///登录名
        /// </summary>
        [DataMember(Order = 2)]
        public string LoginName { get; set; }

        [DataMember(Order = 3)]
        public CompInfo CompInfo { get; set; }

        /// <summary>
        ///角色ID
        /// </summary>

        [DataMember(Order = 4)]
        public int RoleID { get; set; }

        [DataMember(Order = 5)]
        public string AppKey { get; set; }

        [DataMember(Order = 6)]
        public string Secretkey { get; set; }

        /// <summary>
        /// 员工编码
        /// </summary>
        [DataMember(Order = 6)]
        public string UserCode { get; set; }

        /// <summary>
        /// 用户权限名称
        /// </summary>
       [DataMember(Order = 6)]
        public string Role_Enum { get; set; }

       /// <summary>
       ///用户名
       /// </summary>
       [DataMember(Order = 2)]
       public string UserName { get; set; }


       /// <summary>
       ///邮箱
       /// </summary>
       [DataMember(Order = 2)]
       public string Email { get; set; }


       [DataMember(Order = 6)]
       public ReturnPicResources PicResources { get; set; }
    }


    /// <summary>
    /// 返回公司的数据
    /// </summary>
    [DataContract]
    public class CompInfo : IReturn<NdtechAuthResponse>
    {
        /// <summary>
        /// 公司ID
        /// </summary>

        [DataMember(Order = 1)]
        public string CompID { get; set; }
     
        /// <summary>
        /// 公司名称
        /// </summary>

        [DataMember(Order = 2)]
        public string CompName { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        /// <summary>
        /// 云ID
        /// </summary>
        [DataMember(Order = 4)]
        public string CorpNum { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember(Order = 5)]
        public string TelNum { get; set; }

       
    }
    [DataContract]
    public class NdtechAuthResponse
    {
        public NdtechAuthResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
     
        [DataMember(Order=4)] public ResponseStatus ResponseStatus { get; set; }

        [DataMember(Order = 9)]
        public UserInfo Data { get; set; }

        [DataMember(Order = 10)]
        public bool Success { get; set; }

      
    }
}
