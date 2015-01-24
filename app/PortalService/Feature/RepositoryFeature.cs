using Ndtech.PortalService.Auth;
using Ndtech.PortalService.service.management.myPurchase;
using Ndtech.PortalService.Subscribe;
using ServiceStack.OrmLite;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Feature
{
    public class RepositoryFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.GetContainer().Register<INdtechUserAuthRepository>(c => new NdtechOrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
            appHost.GetContainer().Register<SubscribeAbstract>("Inquiry", c => new SubscribeInquiry(c.Resolve<IDbConnectionFactory>()));
            appHost.GetContainer().Register<SubscribeAbstract>("Product", c => new SubscribeProduct(c.Resolve<IDbConnectionFactory>()));
            appHost.GetContainer().Register<SubscribeAbstract>("Company", c => new SubscribeCompany(c.Resolve<IDbConnectionFactory>()));
            appHost.GetContainer().Register<GetLowerEmployeeListID>("LowerEmployeeList", c => new GetLowerEmployeeListID(c.Resolve<IDbConnectionFactory>()));

        }
    }
}
