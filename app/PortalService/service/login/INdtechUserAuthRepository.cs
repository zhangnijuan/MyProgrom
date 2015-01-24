using Ndtech.PortalService.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
   public interface INdtechUserAuthRepository
    {
       NdtechStaffInfo CreateUserAuth(NdtechStaffInfo newUser, string password);
       NdtechStaffInfo UpdateUserAuth(NdtechStaffInfo existingUser, NdtechStaffInfo newUser, string password);
       NdtechStaffInfo GetUserAuthByUserName(string userNameOrEmail);
       bool TryAuthenticate(string userName, string password, out NdtechStaffInfo userAuth);
       bool TryAuthenticate(Dictionary<string, string> digestHeaders, string PrivateKey, int NonceTimeOut, string sequence, out NdtechStaffInfo userAuth);
        //void LoadUserAuth(IAuthSession session, IOAuthTokens tokens);
       void LoadUserAuth(INdtechAuthSession session);
        NdtechStaffInfo GetUserAuth(string userAuthId);
        void SaveUserAuth(INdtechAuthSession authSession);
        void SaveUserAuth(NdtechStaffInfo userAuth);
        //List<UserOAuthProvider> GetUserOAuthProviders(string userAuthId);
        NdtechStaffInfo GetUserAuth(INdtechAuthSession authSession);
        string CreateOrMergeAuthSession(INdtechAuthSession authSession);
    }
}
