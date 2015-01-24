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
    /// 获取商家比价的交易快照Service
    /// add by yangshuo 2015/01/04
    /// </summary>
    public class GetOldEnterprisesPriceService : Service, IPost<GetOldEnterprisesPriceRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetOldEnterprisesPriceService));

        public object Post(GetOldEnterprisesPriceRequest request)
        {
            GetEnterprisesPriceResponse response = new GetEnterprisesPriceResponse();

            if (string.IsNullOrEmpty(request.OrderTime))
            {
                response.ResponseStatus.ErrorCode = "Err_OrderTimeIsNull";
                response.ResponseStatus.Message = "No OrderTime Parameter";
                return response;
            }
            if (string.IsNullOrEmpty(request.StandardItemCode))
            {
                response.ResponseStatus.ErrorCode = "Err_StandardItemCodeIsNull";
                response.ResponseStatus.Message = "No StandardItemCode Parameter";
                return response;
            }
            if (request.AccountID <= 0)
            {
                response.ResponseStatus.ErrorCode = "Err_AccountIDIsNull";
                response.ResponseStatus.Message = "No AccountID Parameter";
                return response;
            }
            if (request.PageIndex <= 0 || request.PageSize <= 0)
            {
                response.ResponseStatus.ErrorCode = "Err_PageParameterIsNull";
                response.ResponseStatus.Message = "No PageIndex or PageSize Parameter";
                return response;
            }

            return PostMethod(request);
        }

        private GetEnterprisesPriceResponse PostMethod(GetOldEnterprisesPriceRequest request)
        {
            GetEnterprisesPriceResponse response = new GetEnterprisesPriceResponse();

            //1.根据帐套分组查询<=订单确认时间最近一笔的产品价格
            string orderTime = Convert.ToDateTime(request.OrderTime).ToString("yyyy-MM-dd 23:59:59");
            string sql = string.Format(@"select * from (select a.mid as id, a.a, a.prd as salprc, b.n as compname, d.address, d.city, d.standard_n, g.corpnum,
                                                        row_number() over (partition by a.a order by a.startdate desc) as rank 
                                                        from udoc_enterprise_price a
                                                        inner join udoc_enterprise_item d on a.mid = d.id
                                                        left join udoc_comp b on a.a = b.a
                                                        left join gl_acntsystems g on g.id = b.a
                                                        where a.a != {0} and a.startdate <= '{1}' and d.biztype = 2 and d.state = 1 and d.standard_c = '{2}'
                                                        ) as t where rank = 1;",
                                        request.AccountID, orderTime, request.StandardItemCode);

            List<GetEnterprisesPrice> priceList = this.Db.Query<GetEnterprisesPrice>(sql);
            if (priceList != null && priceList.Count > 0)
            {
                //2.获取总行数
                response.RowsCount = priceList.Count;

                //3.分页
                priceList = priceList.OrderBy(x => x.CompName).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();

                //4.返回对象
                response.Data = priceList;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Mes_EnterprisesPrice";
                response.ResponseStatus.Message = "No Enterprises Price Log";
            }

            response.Success = true;
            return response;
        }
    }
}
