using System;
using System.IO;
using System.Linq;
using ServiceStack.Text;
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
    /// 获取产品交易快照Service
    /// add by yangshuo 2015/01/04
    /// </summary>
    public class GetOldCompItemByIDService : Service, IPost<GetOldCompItemByIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetOldCompItemByIDService));

        public object Post(GetOldCompItemByIDRequest request)
        {
            GetCompItemByIDResponse response = new GetCompItemByIDResponse();

            if (!string.IsNullOrEmpty(request.StandardItemCode))
            {
                if (request.SAccountID >0)
                {
                    if (!string.IsNullOrEmpty(request.SearchDate))
                    {
                        //search + 返回response
                        return PostMethod(request);
                    }
                    else
                    {
                        response.ResponseStatus.ErrorCode = "Err_SearchDateIsNull";
                        response.ResponseStatus.Message = "No SearchDate Parameter";
                    }
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_SAccountIDIsNull";
                    response.ResponseStatus.Message = "No SAccountID Parameter";
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_StandardItemCodeIsNull";
                response.ResponseStatus.Message = "No StandardItemCode Parameter";
            }

            return response;
        }

        private GetCompItemByIDResponse PostMethod(GetOldCompItemByIDRequest request)
        {
            GetCompItemByIDResponse response = new GetCompItemByIDResponse();

            //根据平台标准代码供应商帐套查询供应商产品id
            long ID = -1;
            EnterpriseItem salItem = this.Db.FirstOrDefault<EnterpriseItem>(x => x.StandardItemCode == request.StandardItemCode
                && x.AccountID == request.SAccountID && x.BizType == 2);
            if (salItem != null)
            {
                //根据产品id获取企业产品交易快照
                ID = salItem.ID;
                string searchDate = Convert.ToDateTime(request.SearchDate).ToString("yyyy-MM-dd 23:59:59");
                List<EnterpriseItemLog> itemOld = this.Db.Where<EnterpriseItemLog>(x => x.ItemID == ID
                    && x.CreateDate <= Convert.ToDateTime(searchDate));
                if (itemOld != null && itemOld.Count > 0)
                {
                    EnterpriseItemLog log = itemOld.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    CompItemList data = log.Obj.FromJson<CompItemList>();
                    response.Data = data;
                }
                else
                {
                    //快照无资料时,企业产品档案作为快照
                    GetCompItemByIDRequest itemRequest = new GetCompItemByIDRequest();
                    itemRequest.ID = ID;
                    GetCompItemByIDService itemService = this.TryResolve<GetCompItemByIDService>();
                    response = itemService.Get(itemRequest).TranslateTo<GetCompItemByIDResponse>();
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoItem";
                response.ResponseStatus.Message = "No EnterpriseItem in Supplier Table";
            }

            response.Success = true;

            return response;
        }
    }
}
