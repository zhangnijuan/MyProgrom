using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.OrmLite;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class RegisteredFeature : IPlugin
    {
        public string AtRestPath { get; set; }

        public RegisteredFeature()
        {
            this.AtRestPath = "/register";
        }
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<RegisteredService>(AtRestPath);
          

        }
    }
}
