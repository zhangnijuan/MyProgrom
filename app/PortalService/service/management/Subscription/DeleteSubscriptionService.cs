using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface.Validation;
using Ndtech.PortalService.Extensions;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;
using System.Collections;

namespace Ndtech.PortalService.service.management.Subscription
{
   public  class DeleteSubscriptionService:Service,IGet<DeleteSubscriptionRequest>
    {
        public object Get(DeleteSubscriptionRequest request)
        {
            using (var trans=this.Db.BeginTransaction())
            {

                if (this.Db.Delete<SubscribeFilter>(s => s.Where(q => q.ID == request.ID)) > 0)
                {
                    this.Db.Delete<SubscribeFilterDetail>(s => s.Where(q => q.Mid == request.ID));
                    trans.Commit();
                    return new DeleteSubscriptionResponse() { Success = true };
                }
                else
                {
                    return new DeleteSubscriptionResponse() { Success = false };
                }
            }

            
          
        }
    }
}
