using Ndtech.PortalModel.ViewModel;
using ServiceStack.Common.Web;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public abstract class NdtechAuthProvider : INdtechAuthProvider
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(NdtechAuthProvider));
                public TimeSpan? SessionExpiry { get; set; }
        public string AuthRealm { get; set; }
        public string Provider { get; set; }
        public string CallbackUrl { get; set; }
        public string RedirectUrl { get; set; }

        protected NdtechAuthProvider() { }

        protected NdtechAuthProvider(IResourceManager appSettings, string authRealm, string oAuthProvider)
        {
            // Enhancement per https://github.com/ServiceStack/ServiceStack/issues/741
            this.AuthRealm = appSettings != null ? appSettings.Get("OAuthRealm", authRealm) : authRealm;

            this.Provider = oAuthProvider;
            if (appSettings != null)
            {
                this.CallbackUrl =string.Format(appSettings.GetString("oauth.{0}.CallbackUrl"),oAuthProvider);
                this.RedirectUrl =string.Format( appSettings.GetString("oauth.{0}.RedirectUrl"),oAuthProvider);
            }
            this.SessionExpiry = SessionFeature.DefaultSessionExpiry;
        }



        /// <summary>
        /// Remove the Users Session
        /// </summary>
        /// <param name="service"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual object Logout(IServiceBase service, NdtechAuthRequest request)
        {
            var session = service.GetSession();
            var referrerUrl =this.CallbackUrl;

            session.OnLogout(service);

            service.RemoveSession();

            if (service.RequestContext.ResponseContentType == ContentType.Html && !String.IsNullOrEmpty(referrerUrl))
                return service.Redirect(referrerUrl.AddHashParam("s", "-1"));

            return new NdtechAuthResponse();
        }

        /// <summary>
        /// Saves the Auth Tokens for this request. Called in OnAuthenticated(). 
        /// Overrideable, the default behaviour is to call IUserAuthRepository.CreateOrMergeAuthSession().
        /// </summary>
        protected virtual void SaveUserAuth(IServiceBase authService, INdtechAuthSession session, INdtechUserAuthRepository authRepo)
        {
            if (authRepo == null) return;

             session.UserID = authRepo.CreateOrMergeAuthSession(session);
  

            authRepo.LoadUserAuth(session);

            //foreach (var oAuthToken in session.ProviderOAuthAccess)
            //{
            //    var authProvider = AuthService.GetAuthProvider(oAuthToken.Provider);
            //    if (authProvider == null) continue;
            //    var userAuthProvider = authProvider as OAuthProvider;
            //    if (userAuthProvider != null)
            //    {
            //        userAuthProvider.LoadUserOAuthProvider(session, oAuthToken);
            //    }
            //}

            authRepo.SaveUserAuth(session);

            var httpRes = authService.RequestContext.Get<IHttpResponse>();
            if (httpRes != null)
            {
                httpRes.Cookies.AddPermanentCookie(HttpHeaders.XUserAuthId, session.UserID);
            }
            OnSaveUserAuth(authService, session);
        }

        public virtual void OnSaveUserAuth(IServiceBase authService, INdtechAuthSession session) { }

        public virtual void OnAuthenticated(IServiceBase authService, INdtechAuthSession session, Dictionary<string, string> authInfo)
        {
            var userSession = session as NdtechAuthUserSession;
            if (userSession != null)
            {
                LoadUserAuthInfo(userSession,authInfo);
            }

            var authRepo = authService.TryResolve<INdtechUserAuthRepository>();
            if (authRepo != null)
            {
                //SaveUserAuth(authService, userSession, authRepo, tokens);
                
                authRepo.LoadUserAuth(session);

                //foreach (var oAuthToken in session.ProviderOAuthAccess)
                //{
                //    var authProvider = AuthService.GetAuthProvider(oAuthToken.Provider);
                //    if (authProvider == null) continue;
                //    var userAuthProvider = authProvider as OAuthProvider;
                //    if (userAuthProvider != null)
                //    {
                //        userAuthProvider.LoadUserOAuthProvider(session, oAuthToken);
                //    }
                //}
        
                var httpRes = authService.RequestContext.Get<IHttpResponse>();
                if (httpRes != null)
                {
                    httpRes.Cookies.AddPermanentCookie(HttpHeaders.XUserAuthId, session.UserID);
                }
                
            }

           SaveSession( authService.RequestContext.Get<IHttpRequest>(),session, SessionExpiry);
            session.OnAuthenticated(authService, session, authInfo);
        }
        public const string RequestItemsSessionKey = "__session";
        public  void SaveSession(IHttpRequest httpReq, INdtechAuthSession session, TimeSpan? expiresIn = null)
        {
            if (httpReq == null) return;

            using (var cache = httpReq.GetCacheClient())
            {
                var sessionKey = SessionFeature.GetSessionKey(httpReq.GetSessionId());
                cache.CacheSet(sessionKey, session, expiresIn ?? SessionFeature.DefaultSessionExpiry);
            }

            httpReq.Items[RequestItemsSessionKey] = session;
        }

        protected virtual void LoadUserAuthInfo(NdtechAuthUserSession userSession,Dictionary<string, string> authInfo) { }



        public abstract bool IsAuthorized(INdtechAuthSession session,  NdtechAuthRequest request = null);

        public abstract object Authenticate(IServiceBase authService, INdtechAuthSession session, NdtechAuthRequest request);

        public virtual void OnFailedAuthentication(INdtechAuthSession session, IHttpRequest httpReq, IHttpResponse httpRes)
        {
            httpRes.StatusCode = (int)HttpStatusCode.Unauthorized;
            httpRes.AddHeader(HttpHeaders.WwwAuthenticate, string.Format("{0} realm=\"{1}\"",this.Provider, this.AuthRealm));
            httpRes.Close();
 
        }
    }
}
