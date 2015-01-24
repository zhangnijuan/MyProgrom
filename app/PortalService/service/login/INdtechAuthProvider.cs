using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
   public interface INdtechAuthProvider
    {
        string AuthRealm { get; set; }
        string Provider { get; set; }
        string CallbackUrl { get; set; }

        /// <summary>
        /// Remove the Users Session
        /// </summary>
        /// <param name="service"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        object Logout(IServiceBase service, NdtechAuthRequest request);

        /// <summary>
        /// The entry point for all AuthProvider providers. Runs inside the AuthService so exceptions are treated normally.
        /// Overridable so you can provide your own Auth implementation.
        /// </summary>
        object Authenticate(IServiceBase authService, INdtechAuthSession session, NdtechAuthRequest request);

        bool IsAuthorized(INdtechAuthSession session, NdtechAuthRequest request = null);
    }
}
