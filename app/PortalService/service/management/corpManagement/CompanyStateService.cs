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
using ServiceStack.ServiceInterface.Auth;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    public class CompanyStateSercice : Service, IGet<CompanyStateRequest>
    {
        public object Get(CompanyStateRequest request)
        {
            CompanyStateResponse response = new CompanyStateResponse();
            var CompanyState = this.Db.FirstOrDefault<NdtechCompany>(c => c.AccountID == request.AccountID);
            if (CompanyState != null && !string.IsNullOrEmpty(CompanyState.CompName) && !string.IsNullOrEmpty(CompanyState.TelPhone))
            {
                response.EditSuccess = true;
            }

            var QualificationState = this.Db.FirstOrDefault<CompanyCertification>(c => c.Accountid == request.AccountID);
            if (QualificationState != null)
            {
                response.QualificationSuccess  = true;
            }

            return response;
        }

    }
}
