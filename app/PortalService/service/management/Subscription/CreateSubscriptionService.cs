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
   public  class CreateSubscriptionService:Service,IPost<CreateSubscriptionRequest>
    {
        public object Post(CreateSubscriptionRequest request)
        {
            CreateSubscriptionResponse response = new CreateSubscriptionResponse();
            using (var trans=this.Db.BeginTransaction())
            {
                SubscribeFilter subFilter = request.TranslateTo<SubscribeFilter>();
               // subFilter.ID = RecordIDService.GetRecordID(1);
                subFilter.Subtype = 0;
                subFilter.LastTime = DateTime.Now;
                
                long id= Db.InsertParam<SubscribeFilter>(subFilter,selectIdentity: true);
                if (request.CategoryName!=null)
                {
                    foreach (var item in request.CategoryName)
                    {
                        SubscribeFilterDetail subDetail = new SubscribeFilterDetail();
                        subDetail.Mid = id;
                        subDetail.CategoryName = item;
                        this.Db.Insert<SubscribeFilterDetail>(subDetail);
                    }
                }
                trans.Commit();
                response.Success = true;
            }
          
            return response; 
        }
    }
}
