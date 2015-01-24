using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
   public interface INdtechAuthSession
    {
       string UserID { get; set; }
        string UserName { get; set; }
        string UserCode { get; set; }
        string SessionID { get; set; }
       DateTime CreatedAt { get; set; }
       DateTime LastModified { get; set; }

        int AccountID { get; set; }

        string CorpNum { get; set; }

       bool IsAuthenticated { get; set; }

       long Role { get; set; }
       bool HasRole(string role);
       bool HasPermission(string permission);
       bool IsAuthorized(string provider);

       void OnRegistered(IServiceBase registrationService);
       void OnAuthenticated(IServiceBase authService, INdtechAuthSession session, Dictionary<string, string> authInfo);
       void OnLogout(IServiceBase authService);
       void OnCreated(IHttpRequest httpReq);
    }
}
