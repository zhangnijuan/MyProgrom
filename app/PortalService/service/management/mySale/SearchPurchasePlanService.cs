using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface.Validation;
using Ndtech.PortalService.Extensions;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;
using System.Collections;
using Ndtech.PortalModel;
using Ndtech.PortalService.service.management.myPurchase;
using ServiceStack.WebHost.Endpoints;
namespace Ndtech.PortalService.service.management.mySale
{
    public class SearchPurchasePlanService : Service, IPost<SearchPurchasePlanRequest>
    {
        public object Post(SearchPurchasePlanRequest request)
        {
            SearchPurchasePlanResponse response = new SearchPurchasePlanResponse();
            StringBuilder sql = new StringBuilder("select plan_id,plan_a,plan_c,plan_subject,plan_create,plan_eid_n,plan_state_enum,plan_mm from pur_plan");
            sql.AppendFormat(" where plan_a={0} and plan_state!=2", request.AccountID);

            //计算状态的数量sql语句
            StringBuilder sqlCount=new StringBuilder("select plan_id,plan_a,plan_c,plan_subject,plan_create,plan_eid_n,plan_state_enum,plan_mm from pur_plan");
            sqlCount.AppendFormat(" where plan_a={0} and plan_state!=2", request.AccountID);
            if (request.IsSearchAll)
            {//查询所有
                long eid = Convert.ToInt64(request.EID);
                string name = this.Db.Where<NdtechStaffInfo>(s => s.ID == eid).Select(x => x.SysCode).FirstNonDefault();
                if (name != "S001")//非管理员  查询自己的和下级
                {
                    GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                    List<long> listID = getEmpList.GetListBuyId(eid);
                    listID.Add(eid);
                    string str = string.Join(",", listID.Select(x => x.ToString()).ToArray());
                    sql.AppendFormat(" and plan_eid in ({0})", str);
                    sqlCount.AppendFormat(" and plan_eid in ({0})", str);
                }
            }
            else
            {//查询我的
                sql.AppendFormat(" and plan_eid={0}", request.EID);
                sqlCount.AppendFormat(" and plan_eid={0}", request.EID);
            }
            //拼接搜索条件
            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchPlanEnum.PlanSubject)
                    {
                        sql.AppendFormat(" and plan_subject like '%{0}%'", item.Value);
                       
                    }
                    if (item.SeacheKey == SearchPlanEnum.State)
                    {
                        sql.AppendFormat(" and plan_state={0}", item.Value);
                        
                    }
                    if (item.SeacheKey == SearchPlanEnum.BeginTime)
                    {
                        sql.AppendFormat(" and plan_create>='{0}'", item.Value);
                        
                    }
                    if (item.SeacheKey == SearchPlanEnum.EndTime)
                    {
                        sql.AppendFormat(" and plan_create<='{0}'", item.Value);
                     
                    }
                }

            }
            DataCount counts = new DataCount();
            counts.AllCount = this.Db.Query<PurchasePlan>(sqlCount.ToString()).Count;
            sqlCount.Append(" and plan_state=0");
            counts.UnCount = this.Db.Query<PurchasePlan>(sqlCount.ToString()).Count;
            counts.DoCount = counts.AllCount - counts.UnCount;
            response.DataCounts = counts;
            if (request.Orders != null && request.Orders.Count > 0)
            {
                foreach (var item in request.Orders)
                {
                    if (item.sortKey== SortKey.Ascending)
                    {
                        sql.AppendFormat(" order by {0} asc", item.orderKey);
                    }
                    else
                    {
                        sql.AppendFormat(" order by {0} desc", item.orderKey);
                    }
                }
            }

            response.RowsCount = this.Db.Query < PurchasePlan>(sql.ToString()).Count;
            request.PageIndex=request.PageIndex < 1 ? 1 : request.PageIndex;
            sql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
            response.Data = this.Db.Query<PurchasePlan>(sql.ToString());
            response.Success = true;
            return response;
        }
    }
}
