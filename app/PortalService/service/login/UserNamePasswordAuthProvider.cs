using Ndtech.PortalService.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
   public class UserNamePasswordAuthProvider:NdtechAuthProvider
    {
     public  INdtechUserAuthRepository repository { set; get; }
       public UserNamePasswordAuthProvider()
       {
           this.Provider = "credentials";
       }
        public override bool IsAuthorized(INdtechAuthSession session, PortalModel.ViewModel.NdtechAuthRequest request = null)
        {
            return session.UserName == request.UserName;
        }

        public override object Authenticate(ServiceStack.ServiceInterface.IServiceBase authService, INdtechAuthSession session, PortalModel.ViewModel.NdtechAuthRequest request)
        {
            if (null == repository)
            {
                repository = authService.TryResolve<INdtechUserAuthRepository>();
            }
            NdtechStaffInfo staffinfo = null;
            //去数据库校验登陆人身份信息
            if (repository != null)
            {
                repository.TryAuthenticate(request.UserName, request.Password, out staffinfo);
            }
            return staffinfo;
        }
    }
}
