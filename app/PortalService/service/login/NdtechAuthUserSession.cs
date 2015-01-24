using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalService.Auth
{
     [DataContract]
   public class NdtechAuthUserSession:INdtechAuthSession
    {
         [DataMember(Order = 1)]
       public string UserID
       {
           get;
           set;
       }
         [DataMember(Order = 02)]
         public string UserName { get; set; }

         [DataMember(Order = 03)]
         public string UserCode { get; set; }


         [DataMember(Order = 04)]
         public string SessionID { get; set; }


         [DataMember(Order = 05)]
         public DateTime CreatedAt { get; set; }


         [DataMember(Order = 06)]
         public DateTime LastModified { get; set; }

         [DataMember(Order = 07)]

         public bool IsAuthenticated { get; set; }

         [DataMember(Order = 08)]
         public List<string> Roles { get; set; }

         [DataMember(Order = 09)]
         public List<string> Permissions { get; set; }

         [DataMember(Order = 10)]
         public long Role { get; set; }
        public bool HasRole(string role)
        {
            return this.Roles != null && this.Roles.Contains(role);
        }

        public bool HasPermission(string permission)
        {
            return this.Permissions != null && this.Permissions.Contains(permission);
        }

        public bool IsAuthorized(string provider)
        {
            return true ;
        }

        public void OnRegistered(ServiceStack.ServiceInterface.IServiceBase registrationService)
        {
           
        }

        public void OnAuthenticated(ServiceStack.ServiceInterface.IServiceBase authService, INdtechAuthSession session, Dictionary<string, string> authInfo)
        {
       
        }

        public void OnLogout(ServiceStack.ServiceInterface.IServiceBase authService)
        {
         
        }

        public void OnCreated(ServiceStack.ServiceHost.IHttpRequest httpReq)
        {
           
        }


        public int AccountID { get; set; }

        public string CorpNum { get; set; }
    }
}
