using System;
using System.IO;
using System.Linq;
using ServiceStack.Common;
using ServiceStack.OrmLite;
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
    /// 根据询价ID获取优选汇总Service
    /// add by yangshuo 2014/12/25
    /// </summary>
    public class GetPurSelectService : Service, IPost<GetPurSelectRequest>
    {
        public object Post(GetPurSelectRequest request)
        {
            GetPurSelectResponse response = new GetPurSelectResponse();
            if (request.InquiryID >0)
            {
                return PostMethod(request);    
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_IDIsNull";
                response.ResponseStatus.Message = Const.Err_IDIsNull;
            }

            return response;
        }

        private GetPurSelectResponse PostMethod(GetPurSelectRequest request)
        {
            GetPurSelectResponse response = new GetPurSelectResponse();

            var purSelect = this.Db.QuerySingle<PurSelectMain>(string.Format("select * from pur_select where a = {0} and inquiryid = {1};",
                request.AccountID, request.InquiryID));
            if (purSelect != null)
            {
                var purSelectDetail = this.Db.Query<PurSelectDetail>(string.Format("select * from pur_select_results where a = {0} and mid = {1};",
                    request.AccountID, purSelect.ID));
                response.Data = purSelect;
                response.Data.DetailData = purSelectDetail;
                response.Success = true;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                response.ResponseStatus.Message = Const.Err_NoInfoByID;
            }

            return response;
        }
    }
}
