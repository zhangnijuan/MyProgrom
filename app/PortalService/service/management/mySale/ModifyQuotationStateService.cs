using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;

namespace Ndtech.PortalService.Auth
{
    public class ModifyQuotationStateService : Service, IPost<ModifyQuotationStateRequest>
    {
        public object Post(ModifyQuotationStateRequest request)
        {
            ModifyInquiryStateResponse response = new ModifyInquiryStateResponse();
            string State = "";
            SalQuotation oldInfo = null;
            if (!string.IsNullOrEmpty(request.AccountId.ToString()))
            {
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchIgnoreStateEnum.ID && !string.IsNullOrEmpty(obj.Value))
                        {
                            long ID = long.Parse(obj.Value);
                            oldInfo = this.Db.FirstOrDefault<SalQuotation>(x => x.AccountID == request.AccountId && x.ID == ID);
                            
                        }
                        if (obj.SeacheKey == SearchIgnoreStateEnum.State && !string.IsNullOrEmpty(obj.Value))
                        {
                            State = obj.Value;
                        }
                    }
                    if (oldInfo != null)
                    {
                        if (!string.IsNullOrEmpty(State))
                        {
                            using (var trans = this.Db.BeginTransaction())
                            {
                                this.Db.Update<SalQuotation>(string.Format("State = {2} where a = {0} and id = {1}", request.AccountId, oldInfo.ID, State));

                                trans.Commit();
                            }
                            response.Success = true;
                        }
                        else
                        {
                            response.ResponseStatus.ErrorCode = "Err_NoState";
                            response.ResponseStatus.Message = "No State";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.ResponseStatus.ErrorCode = "Err_NoDateOrNoID";
                        response.ResponseStatus.Message = "No data";
                        response.Success = false;
                    }
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoSearchCondition";
                    response.ResponseStatus.Message = "No SearchCondition";
                    response.Success = false;
                }

            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoAccountId";
                response.ResponseStatus.Message = "No data";
                response.Success = false;
            }
            return response;

        }
    }
}
