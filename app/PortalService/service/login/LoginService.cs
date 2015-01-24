using Ndtech.PortalModel.ViewModel;
using ServiceStack.Common.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalService.Extensions;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.FluentValidation.Internal;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.Common;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    public class LoginValidator : AbstractValidator<NdtechAuthRequest>
    {
        public INdtechUserAuthRepository UserAuthRepo { get; set; }
  
        public override ValidationResult Validate(NdtechAuthRequest instance)
        {
            ValidationResult result = new ValidationResult();
            //验证用户名是否为空
            if (string.IsNullOrEmpty(instance.UserName))
            {
                result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameIsnull, "Err_UserNameIsnull"));
                return result;
            }
            else
            {
                //验证用户名是否存在
                    NdtechStaffInfo userAuth = UserAuthRepo.GetUserAuthByUserName(instance.UserName);
             
                    if (userAuth == null)
                    {
                        result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameNotExisting, "Err_UserNameNotExisting"));
                        return result;
                    }
                    //验证用户名是否启用
                    else if (userAuth.State==0)
                    {
                        result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameState, "Err_UserNameState"));
                        return result;
                    }//验证密码是否为空
                    else if (string.IsNullOrEmpty(instance.Password))
                    {
                        result.Errors.Add(new ValidationFailure("Password", Const.Err_PasswordIsnull, "Err_PasswordIsnull"));
                        return result;
                    }
                    else
                    {
                        //验证密码是否存在
                        if (!UserAuthRepo.TryAuthenticate(instance.UserName, instance.Password.Trim(), out userAuth))
                        {
                            result.Errors.Add(new ValidationFailure("Password", Const.Err_PasswordIsError, "Err_PasswordIsError"));
                            return result;
                        }
                    }
            }
           
            return base.Validate(instance);
        }
    }
    public class LoginService : Service, IPost<NdtechAuthRequest>
    {

        static LoginService()
        {
            CurrentSessionFactory = () => new NdtechAuthUserSession();
        }
        public const string CredentialsProvider = "credentials";
        public const string LogoutAction = "logout";
        public static string DefaultOAuthProvider { get; private set; }
        public static string DefaultOAuthRealm { get; private set; }
        public IValidator<LoginValidator>  UserLoginValidator { get; set; }
        public static Func<INdtechAuthSession> CurrentSessionFactory { get; set; }
        public static INdtechAuthProvider[] AuthProviders { get; private set; }
        public INdtechUserAuthRepository UserAuthRepo { get; set; }
        public IValidator<NdtechAuthRequest> LoginValidator { get; set; }
        public static INdtechAuthProvider GetAuthProvider(string provider)
        {
            if (AuthProviders == null || AuthProviders.Length == 0) return null;
            if (provider == LogoutAction) return AuthProviders[0];

            foreach (var authConfig in AuthProviders)
            {
                if (string.Compare(authConfig.Provider, provider,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                    return authConfig;
            }

            return null;
        }
        public static void Init(Func<INdtechAuthSession> sessionFactory, params INdtechAuthProvider[] authProviders)
        {
            EndpointHost.AssertTestConfig();

            if (authProviders.Length == 0)
                throw new ArgumentNullException("authProviders");

            DefaultOAuthProvider = authProviders[0].Provider;
            DefaultOAuthRealm = authProviders[0].AuthRealm;

            AuthProviders = authProviders;
            if (sessionFactory != null)
                CurrentSessionFactory = sessionFactory;
        }
        public object Post(NdtechAuthRequest request)
        {
            //第一步:校验前端的数据合法性
            ValidationResult result = LoginValidator.Validate(request);
            NdtechAuthResponse response = new NdtechAuthResponse();
            if (!result.IsValid)
            {

                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
            }
            else
            {
                //第二步:校验身份保存session
                var provider = AuthProviders[0].Provider;
                var oAuthConfig = GetAuthProvider(provider);
                //创建session
                var session = this.GetNdtechSession();
                NdtechStaffInfo nsi;
                try
                {
                    //校验身份保存session
                    nsi = (NdtechStaffInfo)Authenticate(request, provider, session, oAuthConfig); 
                }
                catch (HttpError ex)
                {
                    NdtechAuthResponse res = new NdtechAuthResponse();
                    res.ResponseStatus.ErrorCode = "404";
                    res.ResponseStatus.Message = "身份校验失败";
                    return res;

                }
                //是否需要保存会话时间 暂时去掉 baiyude
                //if (request.RememberMe.HasValue)
                //{
                //    var opt = request.RememberMe.GetValueOrDefault(false)
                //        ? SessionOptions.Permanent
                //        : SessionOptions.Temporary;

                //    base.RequestContext.Get<IHttpResponse>()
                //        .AddSessionOptions(base.RequestContext.Get<IHttpRequest>(), opt);
                //}
                //第三步:填充登陆成功后返回信息
                NdtechStaffCompany sc = null;
                using (var trans =  this.Db.BeginTransaction())
                {
                    try
                    {
                        sc = this.Db.QuerySingle<NdtechStaffCompany>(string.Format("select a.n,a.a ,a.c,a.staffrole_enum,a.id as UserID, a.tel,a.email,a.loginname as loginname,b.n as compname,c.corpnum,a.staffrole,b.id as CompID from udoc_staff a inner join udoc_comp b on a.a=b.a inner join gl_acntsystems c on a.a=c.id  where a.loginname= '{0}'", request.UserName));
                        //DataModel转成ViewModel
                        response.Data = ToUserInfo(sc);
                        response.Data.CompInfo = ToCompInfo(sc);

                        var picResources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == sc.UserID);
                        //获取图片资源
                        ReturnPicResources picres = new ReturnPicResources();
                        picres = ToAuth(picResources);
                        if (picres != null)
                        {
                            picres.FileUrl = "/fileuploads/" + sc.AccountID;
                        }
                        response.Data.PicResources = picres;
                       // 获取AppData
                        AppData ad = this.Db.QuerySingle<AppData>(string.Format("select * from gl_appdata where a= '{0}' and provide= '{1}' and uid = '{2}'", nsi.AccountID, request.Provide, sc.UserID));

                        if (ad != null)
                        {

                            ad.AppKey = Guid.NewGuid().ToString("N");
                            ad.Secretkey = Guid.NewGuid().ToString("N");
                            this.Db.Update<AppData>(ad);
                        }
                        else
                        {
                            ad = new AppData()
                            {
                                A = sc.AccountID,
                                AppKey = Guid.NewGuid().ToString("N"),
                                Secretkey = Guid.NewGuid().ToString("N"),
                                Provide = request.Provide,
                                // UserID = sc.UserID
                            };
                            this.Db.Insert<AppData>(ad);
                        }
                        //DataModel转成ViewModel
                        response.Data.AppKey = ad.AppKey;
                        response.Data.Secretkey = ad.Secretkey;
                        //保存登陆日志
                        NdtechLoginLog loginLog = new NdtechLoginLog
                        {
                            ID = RecordIDService.GetRecordID(1),
                            AccountID = nsi.AccountID,
                            LoginID = nsi.ID,
                            SysCode = nsi.SysCode,
                            UserName = nsi.UserName,
                            ClientIP = Request.RemoteIp,
                            Client = request.Client,
                            CreateTime = DateTime.Now
                        };
                        this.Db.Insert(loginLog);
                        response.Success = true;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
             
            }
            return response;

        }
        private object Authenticate(NdtechAuthRequest request, string provider, INdtechAuthSession session, INdtechAuthProvider oAuthConfig)
        {
            NdtechStaffInfo response = null;
            //if (!oAuthConfig.IsAuthorized(session, request))
            //{
                
                response = oAuthConfig.Authenticate(this, session, request) as NdtechStaffInfo;

                //if (null != response)
                //{
                //    session.UserName = response.UserName;
                //    session.UserID = response.ID.ToString();
                //    session.UserCode = response.SysCode;
                //    session.AccountID = response.AccountID;
                //    session.Role = response.RoleID;
                //    this.SaveSession(session,new TimeSpan(0,10,0));
                //}

            //}
            return response;
        }

        private UserInfo ToUserInfo(NdtechStaffCompany userInfo)
        {

            return userInfo.TranslateTo<UserInfo>();
        }

        private CompInfo ToCompInfo(NdtechStaffCompany compInfo)
        {

            return compInfo.TranslateTo<CompInfo>();
        }

        private ReturnPicResources ToAuth(Resources picResources)
        {
            return picResources.TranslateTo<ReturnPicResources>();
        }
    }
}
