using System;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 获取认证申请单号Service
    /// add by yangshuo 2015/01/07
    /// </summary>
    public class GetItemCertificationCodeService : Service, IPost<GetItemCertificationCodeRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemCertificationCodeService));

        public object Post(GetItemCertificationCodeRequest request)
        {
            GetItemCertificationCodeResponse response = new GetItemCertificationCodeResponse();
            if (request.AccountID > 0)
            {
                return PostMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_ParameterIsNull";
                response.ResponseStatus.Message = "No AccountID  Parameter";
                return response;
            }
        }

        private GetItemCertificationCodeResponse PostMethod(GetItemCertificationCodeRequest request)
        {
            //获取认证申请单号
            GetItemCertificationCodeResponse response = new GetItemCertificationCodeResponse();
            string code = RecordSnumService.GetSnum(this, request.AccountID, SnumType.ItemCertification);
            response.Data = code;
            response.Success = true;
            return response;
        }
    }
}
