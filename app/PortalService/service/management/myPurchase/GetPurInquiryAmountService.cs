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

namespace Ndtech.PortalService.Auth
{
    public class GetPurInquiryAmountService : Service, IGet<GetPurInquiryAmountRequest>
    {

        public object Get(GetPurInquiryAmountRequest request)
        {

            var list = this.Db.Where<PurInquiry>(n => n.Eid == request.Id && n.State == request.State);
            
            GetPurInquiryAmountResponse response = new GetPurInquiryAmountResponse();
            response.RowsCount = list.Count;
            response.Success = true;
            return response;
        }
    }
}
