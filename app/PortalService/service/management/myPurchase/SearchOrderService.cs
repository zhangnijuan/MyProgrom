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
using Ndtech.PortalModel;
using ServiceStack.WebHost.Endpoints;
using Ndtech.PortalService.service.management.myPurchase;

namespace Ndtech.PortalService.Auth
{
    public class SearchOrderService : Service, IPost<SearchOrderRequest>
    {
        public object Post(SearchOrderRequest request)
        {
            SearchOrderResponse response = new SearchOrderResponse();
            StringBuilder sql = new StringBuilder();
            //根据搜索条件  拼接SQL语句
            if (request.CounterParty == CounterPartyEnum.Purchase)
            {
                sql = new StringBuilder("select p.id,p.c,p.createdate,p.su_n,p.totalamt,p.eid_usrname,p.state,p.state_enum ,p.linkmanname,u.n,u.a,p.sa,g.corpnum,p.arrivalstate,p.receivingstate,p.orderdate from pur_order p join udoc_staff s on p.eid=s.id join udoc_comp u on s.a=u.a join gl_acntsystems g on g.id=p.sa");
                sql.AppendFormat(" where p.a={0}", request.AccountID);
            }
            else
            {
                sql.Append("select p.id,p.c,p.createdate,p.su_n,p.totalamt,p.eid_usrname,p.state,p.state_enum ,p.linkmanname,u.n,u.a,g.corpnum,p.arrivalstate,p.receivingstate,p.orderdate from pur_order p join udoc_staff s on p.eid=s.id join udoc_comp u on s.a=u.a join gl_acntsystems g on g.id=u.a");
                sql.AppendFormat(" where p.sa={0} and p.state<>0", request.AccountID);

            }
            if (request.SearchCondition != null)
            {
                Dictionary<SearchOrderEnum, string> dic = new Dictionary<SearchOrderEnum, string>();
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchOrderEnum.State)
                    {
                        sql.AppendFormat(" and p.state={0}", item.Value);
                    }
                    if (item.SeacheKey == SearchOrderEnum.SearchName)
                    {
                        if (request.CounterParty == CounterPartyEnum.Purchase)
                        {
                            sql.AppendFormat(" and (p.eid_usrname like '%{0}%' or p.su_n like '%{0}%' or p.c like '%{0}%' )", item.Value);
                        }
                        else
                        {
                            sql.AppendFormat(" and (p.linkmanname like '%{0}%' or u.n like '%{0}%' or p.c like '%{0}%')", item.Value);
                        }

                    }
                    if (item.SeacheKey == SearchOrderEnum.BeginDateTime)
                    {
                        sql.AppendFormat(" and p.createdate>='{0}'", item.Value);
                    }
                    if (item.SeacheKey == SearchOrderEnum.EndDateTime)
                    {
                        sql.AppendFormat(" and p.createdate<='{0}'", item.Value);
                    }
                    if (item.SeacheKey == SearchOrderEnum.OrderBy)
                    {
                        dic.Add(item.SeacheKey, item.Value);
                    }
                }
                if (!string.IsNullOrEmpty(request.EID))
                {
                    if (request.CounterParty == CounterPartyEnum.Purchase)
                    {
                        //采购
                        if (request.IsSearchAll)
                        {//查询所有订单
                            long eid = Convert.ToInt64(request.EID);
                            string name = this.Db.Where<NdtechStaffInfo>(s => s.ID == eid).Select(x => x.SysCode).FirstNonDefault();
                            if (name != "S001")//非管理员  查询自己的和下级的采购订单
                            {
                                GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                                List<long> list = getEmpList.GetListBuyId(eid);
                                list.Add(eid);
                                string str = string.Join(",", list.Select(x => x.ToString()).ToArray());
                                sql.AppendFormat(" and p.eid in ({0})", str);
                            }

                        }
                        else
                        {//查询我的订单
                            sql.AppendFormat(" and p.eid={0}", request.EID);
                        }

                    }
                    else
                    {
                        //销售
                        if (request.IsSearchAll)
                        {//查询所有
                            long eid = Convert.ToInt64(request.EID);
                            string name = this.Db.Where<NdtechStaffInfo>(s => s.ID == eid).Select(x => x.SysCode).FirstNonDefault();
                            if (name != "S001") //非管理员  查询自己的和下级的销售订单
                            {
                                GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                                List<long> list = getEmpList.GetListBuyId(eid);
                                list.Add(eid);
                                string str = string.Join(",", list.Select(x => x.ToString()).ToArray());
                                sql.AppendFormat(" and (p.linkmanid in ({0}) or coalesce(p.linkmanid,0)=0 or coalesce(p.linkmanid,0)=-1)", str);
                            }
                        }
                        else
                        {//查询我的
                            sql.AppendFormat(" and (p.linkmanid={0} or coalesce(p.linkmanid,0)=0 or coalesce(p.linkmanid,0)=-1)", request.EID);
                        }
                       
                    }
                }
                if (dic.Count > 0)
                {
                    sql.AppendFormat(" order by p.createdate {0}", dic[SearchOrderEnum.OrderBy]);
                }
            }
            response.RowsCount = this.Db.Query<ProductOrder>(sql.ToString()).Count;
            int page =  (request.PageIndex - 1) * request.PageSize;
            
            sql.AppendFormat(" limit {0} offset {1}", request.PageSize,page<0?1:page);
            response.Data = this.Db.Query<ProductOrder>(sql.ToString());
            response.Success = true;
            return response;
        }
    }
}
