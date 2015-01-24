using System;
using System.IO;
using System.Text;
using System.Linq;
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
    /// 获取交易历史Service(优选画面)
    /// add by yangshuo 2015/01/04
    /// </summary>
    public class GetPurOrderHistoryService : Service, IPost<GetPurOrderHistoryRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetPurOrderHistoryService));

        public object Post(GetPurOrderHistoryRequest request)
        {
            GetPurOrderHistoryResponse response = new GetPurOrderHistoryResponse();

            if (!string.IsNullOrEmpty(request.StandardItemCode) && !string.IsNullOrEmpty(request.StandardItemName))
            {
                //search + 返回response
                return PostMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_ParameterIsNull";
                response.ResponseStatus.Message = "Don't have StandardItemCode or StandardItemName";
                return response;
            }
        }

        private GetPurOrderHistoryResponse PostMethod(GetPurOrderHistoryRequest request)
        {
            GetPurOrderHistoryResponse response = new GetPurOrderHistoryResponse();

            //查询订单档案,订单状态为2交易中或者4已完成
            string sql = string.Format(@"select d.id, d.a, m.orderdate, m.su_n, d.prc, d.sqty, d.u_n, m.saddress, d.sa, d.standarditemname
                                         from pur_order as m 
                                         inner join pur_orderdetail as d 
                                         on m.id = d.mid and d.standarditemcode = '{0}' and d.standarditemname = '{1}'
                                         where (state = 2 or state = 4) and d.a = {2} order by orderdate desc;",
                                                request.StandardItemCode, request.StandardItemName, request.AccountId
                                       );

            var data = this.Db.Query<PurOrderHistory>(sql);

            if (request.PageIndex > 0 && request.PageSize > 0)
            {
                //分页
                if (data != null && data.Count > 0)
                {
                    data = data.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                }
            }
            response.RowsCount = data.Count;
            response.Data = data;
            response.Success = true;
            return response;
        }
    }
}
