using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class NdtechAuthFeature : IPlugin
    {
        private readonly Func<INdtechAuthSession> sessionFactory;
        private readonly INdtechAuthProvider[] authProviders;
        public Dictionary<Type, string[]> ServiceRoutes { get; set; }

        public List<IPlugin> RegisterPlugins { get; set; }
        public NdtechAuthFeature(Func<INdtechAuthSession> sessionFactory, INdtechAuthProvider[] authProviders, string htmlRedirect = "~/login")
        {
            this.sessionFactory = sessionFactory;
            this.authProviders = authProviders;

            ServiceRoutes = new Dictionary<Type, string[]> {
                { typeof(LoginService), new[]{"/login", "/login/{provider}"} },
            };
 
            RegisterPlugins = new List<IPlugin> {
                new SessionFeature()                          
            };
        }
        public void Register(IAppHost appHost)
        {
            LoginService.Init(sessionFactory, authProviders);

            var unitTest = appHost == null;
            if (unitTest) return;

            foreach (var registerService in ServiceRoutes)
            {
                appHost.RegisterService(registerService.Key, registerService.Value);
            }
           
            RegisterPlugins.ForEach(x => appHost.LoadPlugin(x));
        }
    }
}
