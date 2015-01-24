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
    /// 认证机构详情Service
    /// add by yangshuo 2015/01/07
    /// </summary>
    public class GetCertificationByIDService : Service, IGet<GetCertificationByIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetCertificationByIDService));

        public object Get(GetCertificationByIDRequest request)
        {
            GetCertificationByIDResponse response = new GetCertificationByIDResponse();

            if (request.CAccountID > 0)
            {
                //第二步:search单笔 + response
                return GetMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_AccountIDIsNull";
                response.ResponseStatus.Message = "No CAccountID Parameter";
                return response;
            }
        }

        private GetCertificationByIDResponse GetMethod(GetCertificationByIDRequest request)
        {
            GetCertificationByIDResponse response = new GetCertificationByIDResponse();

            //查询认证机构详情
            var main = this.Db.QuerySingle<Certification>(string.Format("select id, a, n, linkman, phone, address, zipcode, mm from udoc_certification where a = {0}",
                    request.CAccountID));
            if (main != null)
            {
                response.Data = main;
                response.Success = true;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoInfoByCAccountID";
                response.ResponseStatus.Message = "NoData By CAccountID";
                return response;
            }
            
            return response;
        }
    }
}
