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
using Ndtech.PortalModel;
using Ndtech.PortalModel.ViewModel.professional.employee;

namespace Ndtech.PortalService.service.professional.employee
{
    public class CheckSysCodeService : Service, IGet<CheckSysCodeRequest>
    {

        public object Get(CheckSysCodeRequest request)
        {
            var staff = this.Db.FirstOrDefault<NdtechStaffInfo>(n => n.AccountID == request.AccountID && n.SysCode == request.SysCode);
            if (staff!=null)
            {
                return new CheckSysCodeResponse { Success = false };
            }
            else
            {
                return new CheckSysCodeResponse { Success = true };
            }
        }
    }
}
