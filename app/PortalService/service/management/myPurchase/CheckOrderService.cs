using Ndtech.PortalModel.ViewModel;
using ServiceStack.Common;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalService.Extensions;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.FluentValidation.Internal;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.service.management.myPurchase
{
   public  class CheckOrderService:Service,IPost<CheckOrderRequest>
    {
        public object Post(CheckOrderRequest request)
        {
          SalQuotation sq=  this.Db.FirstOrDefault<SalQuotation>(s => s.ID == request.ID && s.AccountID == request.AccountID);
            if (sq!=null)
            {
                return new CheckOrderResponse() { Success = true };
            }
            else
            {
                return new CheckOrderResponse() { Success = false };
            }

            throw new NotImplementedException();
        }
    }
}
