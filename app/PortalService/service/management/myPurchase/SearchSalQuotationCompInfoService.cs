using Ndtech.PortalModel.ViewModel;
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
    public class SearchSalQuotationCompInfoService : Service, IPost<SearchSalQuotationCompInfoRequest>
    {
        public object Post(SearchSalQuotationCompInfoRequest request)
        {
            SearchSalQuotationCompInfoResponse response = new SearchSalQuotationCompInfoResponse();
            List<QuotCompList> QuotCompList = new List<PortalModel.ViewModel.QuotCompList>();
            StringBuilder SearchConditionsql = new StringBuilder(" select * from sal_quotationcomp where 1=1 and a="+request.AccountID);
            StringBuilder CountConditionsql = new StringBuilder(" select count(*) from sal_quotationcomp where 1=1 and a=" + request.AccountID);
                            //搜索关键字
            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                foreach (var obj in request.SearchCondition)
                {
                    if (obj.SeacheKey == SearchQuotCompEnum.MID)
                    {
                        SearchConditionsql.Append(string.Format(" and mid='{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and mid='{0}'", obj.Value)); 
                    }
                }
            }
            #region 分页和排序
            if (request.orders != null && request.orders.Count > 0)
            {
                StringBuilder ordersql = new StringBuilder(" order by ");
                foreach (var order in request.orders)
                {
                    if (order.sortKey == QuotCompSortKey.Ascending)
                    {
                        ordersql.Append(order.orderKey + " asc ");
                    }
                    else
                    {
                        ordersql.Append(order.orderKey + " desc ");
                    }
                }
                SearchConditionsql.Append(ordersql.ToString().Substring(0, ordersql.ToString().Length));
            }
            else
            {
                SearchConditionsql.Append(" order by CreateDate desc");
            }
            if (request.page != null && request.page.PageIndex >= 0 && request.page.PageNumber > 0)
            {
                if (request.page.PageIndex <= 0)
                    request.page.PageIndex = 1;
                SearchConditionsql.Append(" limit " + request.page.PageNumber + " offset " + (request.page.PageIndex - 1) * request.page.PageNumber + ";");
            }
            else
            {
                SearchConditionsql.Append(" limit 5 offset 0;");
            }
            #endregion
            var list = this.Db.Query<QuotCompList>(string.Format(SearchConditionsql.ToString()));
            long count = this.Db.QuerySingle<long>(string.Format(CountConditionsql.ToString()));
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    QuotCompList.Add(item);
                }

                response.RowsCount = count;
                response.Data = QuotCompList;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }

            return response;
        }

    }
}
