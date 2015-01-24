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

namespace Ndtech.PortalService.Auth
{
   public  class CheckCompNameService:Service,IPost<CheckCompNameRequest>
    {
        public object Post(CheckCompNameRequest request)
        {
            UserNameCheckResponse response = new UserNameCheckResponse();
            var data = this.Db.FirstOrDefault<NdtechCompany>(n => n.CompName == request.CompName);
            if (data!=null)
            {
                response.Success = false;
                response.ResponseStatus.Message = Const.Err_CompNameIsExist;
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
    }
}
