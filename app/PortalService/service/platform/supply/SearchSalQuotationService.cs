using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using Ndtech.PortalService.service.management.myPurchase;
using ServiceStack.WebHost.Endpoints;
namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 查询企业报价信息
    /// </summary>
    public class SearchSalQuotationService : Service, IPost<SearchSalQuotationRequest>
    {
        public object Post(SearchSalQuotationRequest request)
        {
            SearchSalQuotationResponse response = new SearchSalQuotationResponse();
            StringBuilder SearchConditionsql = new StringBuilder("select a.*,b.n as companyname,g.corpnum  as corpnum , c.State as inquiryState,c.a as pura from sal_quotation a left join pur_inquiry c on a.inquiryid=c.id  LEFT JOIN udoc_comp b ON c.a = b.a  left join gl_acntsystems g on g.id = b.a where 1=1");
            StringBuilder CountConditionsql = new StringBuilder("select count(a.id) from sal_quotation a  left join pur_inquiry c on a.inquiryid=c.id  LEFT JOIN udoc_comp b ON c.a = b.a  left join gl_acntsystems g on g.id = b.a where 1=1");
            SearchConditionsql.Append(" and a.a="+request.AccountID);
            CountConditionsql.Append(" and a.a=" + request.AccountID);
            int State = 0;
            try
            {
                //搜索关键字
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                  
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.InquiryAccountSubject && !string.IsNullOrEmpty(obj.Value))
                        {
                            SearchConditionsql.Append(string.Format(" and ( b.n like '%{0}%' or a.Subject like '%{0}%')", obj.Value));
                            CountConditionsql.Append(string.Format(" and ( b.n like '%{0}%' or a.Subject like '%{0}%')", obj.Value));
                        }
                        else if (obj.SeacheKey == SearchEnum.BeginDateTime && !string.IsNullOrEmpty(obj.Value) && request.type == "pendingQuotation")
                        {
                            SearchConditionsql.Append(string.Format(" and a.publisherdt >= '{0}'", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.publisherdt >= '{0}'", obj.Value));
                        }
                        else if (obj.SeacheKey == SearchEnum.EndDateTime && !string.IsNullOrEmpty(obj.Value) && request.type == "pendingQuotation")
                        {
                            SearchConditionsql.Append(string.Format(" and a.publisherdt <= '{0}'", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.publisherdt <= '{0}'", obj.Value));
                        }
                        else if (obj.SeacheKey == SearchEnum.BeginDateTime && !string.IsNullOrEmpty(obj.Value) && request.type != "pendingQuotation")
                        {
                            SearchConditionsql.Append(string.Format(" and a.createdate >= '{0}'", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.createdate >= '{0}'", obj.Value));
                        }
                        else if (obj.SeacheKey == SearchEnum.EndDateTime && !string.IsNullOrEmpty(obj.Value) && request.type != "pendingQuotation")
                        {
                            SearchConditionsql.Append(string.Format(" and a.createdate <= '{0}'", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.createdate <= '{0}'", obj.Value));
                        }
                        else if (obj.SeacheKey == SearchEnum.InquiryState && !string.IsNullOrEmpty(obj.Value))
                        {
                            SearchConditionsql.Append(string.Format(" and c.State = {0}", obj.Value));
                            CountConditionsql.Append(string.Format(" and c.State = {0}", obj.Value));
                            int.TryParse(obj.Value, out State);
                        }
                        else if (obj.SeacheKey == SearchEnum.QuotationState && !string.IsNullOrEmpty(obj.Value) && obj.Value=="0")
                        {
                            SearchConditionsql.Append(string.Format(" and a.State = {0} and c.state=1 ", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.State = {0} and c.state=1 ", obj.Value));
                            int.TryParse(obj.Value, out State);
                        }
                        else if (obj.SeacheKey == SearchEnum.QuotationState && !string.IsNullOrEmpty(obj.Value) && obj.Value != "0")
                        {
                            SearchConditionsql.Append(string.Format(" and a.State = {0}", obj.Value));
                            CountConditionsql.Append(string.Format(" and a.State = {0}", obj.Value));
                            int.TryParse(obj.Value, out State);
                        }
                    }

                }
                if (State != 0 && State!=6) //非待报价数据需要添加制单人过滤
                {
                    if (request.IsSearchAll)
                    {
                        //所有报价
                        long eid = Convert.ToInt64(request.Eid);
                        string name = this.Db.Where<NdtechStaffInfo>(s => s.ID == eid).Select(x => x.SysCode).FirstNonDefault();
                        if (name != "S001")//非管理员  查询自己的和下级的采购订单
                        {
                            GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                            List<long> listID = getEmpList.GetListBuyId(eid);
                            listID.Add(eid);
                            string str = string.Join(",", listID.Select(x => x.ToString()).ToArray());
                            SearchConditionsql.AppendFormat(" and a.eid in ({0})", str);
                            CountConditionsql.Append(string.Format(" and a.eid in ( {0})", str));
                        }
                    }
                    else
                    {//我的报价
                        SearchConditionsql.Append(string.Format(" and a.eid = {0}", request.Eid));
                        CountConditionsql.Append(string.Format(" and a.eid = {0}", request.Eid));
                    }
                }
                if (request.orders != null && request.orders.Count > 0)
                {
                    StringBuilder ordersql = new StringBuilder(" order by ");
                    foreach (var order in request.orders)
                    {
                        if (order.sortKey == SortKey.Ascending)
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
                if (request.page != null && request.page.PageIndex >= 0 && request.page.PageNumber > 0)
                {
                    if (request.page.PageIndex <= 0)
                        request.page.PageIndex = 1;
                    SearchConditionsql.Append("limit " + request.page.PageNumber + " offset " + (request.page.PageIndex - 1) * request.page.PageNumber + ";");
                }
                else
                {
                    SearchConditionsql.Append("limit 10 offset 0;");
                }
                var list = this.Db.Query<SalQuotationViewModel>(string.Format(SearchConditionsql.ToString()));
                long count = this.Db.QuerySingle<long>(string.Format(CountConditionsql.ToString()));
                List<SalQuotationview> listModel = new List<SalQuotationview>();
                foreach (var item in list)
                {
                    SalQuotationview aa = item.TranslateTo<SalQuotationview>();
                    listModel.Add(aa);
                }

                response.RowsCount = count;
                response.Data = listModel;
                response.Success = true;

            }
            catch (Exception ex)
            {

                response.Success = false;
            }
            return response;
        }
    }
}
