using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.Auth;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalService.Extensions;
using System.Net;
using ServiceStack.Common.Web;

namespace Ndtech.PortalService.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NdtechAuthenticateAttribute : RequestFilterAttribute
    {
        public string Provider { get; set; }
        public override void Execute(ServiceStack.ServiceHost.IHttpRequest req, ServiceStack.ServiceHost.IHttpResponse res, object requestDto)
        {
            if (LoginService.AuthProviders == null)
                throw new InvalidOperationException(
                    "The LoginService must be initialized by calling LoginService.Init to use an authenticate attribute");

            var matchingOAuthConfigs = LoginService.AuthProviders.Where(x =>
               string.IsNullOrEmpty( this.Provider)
                || x.Provider == this.Provider).ToList();

            if (matchingOAuthConfigs.Count == 0)
            {
          
                res.Write("没有相关的校验方式");
                res.Close();
                return;
            }

            if (matchingOAuthConfigs.Any(x => x.Provider == DigestAuthProvider.Name))
                AuthenticateIfDigestAuth(req, res);

            //if (matchingOAuthConfigs.Any(x => x.Provider == BasicAuthProvider.Name))
            //    AuthenticateIfBasicAuth(req, res);

            var session = req.GetNdtechSession();
            if (session == null || !matchingOAuthConfigs.Any(x => session.IsAuthorized(x.Provider)))
            {

                 var baseAuthProvider = matchingOAuthConfigs[0] as NdtechAuthProvider;
                if (baseAuthProvider != null)
                {
                    baseAuthProvider.OnFailedAuthentication(session, req, res);
                    return;
                }

            res.StatusCode = (int)HttpStatusCode.Unauthorized;
            res.AddHeader(HttpHeaders.WwwAuthenticate, string.Format("{0} realm=\"{1}\"",matchingOAuthConfigs[0].Provider, matchingOAuthConfigs[0].AuthRealm));

            res.Close();
            }
        }
        public static void AuthenticateIfDigestAuth(IHttpRequest req, IHttpResponse res)
        {
            //Need to run SessionFeature filter since its not executed before this attribute (Priority -100)			
            SessionFeature.AddSessionIdToRequestFilter(req, res, null); //Required to get req.GetSessionId()

            var digestAuth = req.GetDigestAuth();
            if (digestAuth != null)
            {
                var authService = req.TryResolve<LoginService>();
                authService.RequestContext = new HttpRequestContext(req, res, null);
                var response = authService.Post(new NdtechAuthRequest
                {
         
                    UserName = digestAuth["username"],
                    Password = digestAuth["password"],
                    RememberMe = true
                });
            }
        }
    }
}
