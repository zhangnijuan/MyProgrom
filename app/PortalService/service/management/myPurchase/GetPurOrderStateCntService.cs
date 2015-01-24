using Ndtech.PortalModel;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using ServiceStack.WebHost.Endpoints;
using Ndtech.PortalService.service.management.myPurchase;


namespace Ndtech.PortalService.Auth
{
    public class GetPurOrderStateCntServiceService : Service, IPost<GetPurOrderStateCntRequest>
    {
        public object Post(GetPurOrderStateCntRequest request)
        {
            GetPurOrderStateCntResponse response = new GetPurOrderStateCntResponse();
            List<StateInfo> StateInfo = new List<StateInfo>();
            if (!string.IsNullOrEmpty(request.AccountId.ToString()))
            {
                #region
                StringBuilder AllEidCondition = new StringBuilder();//按照制单人条件过滤
                StringBuilder EidCondition = new StringBuilder();//按照制单人条件过滤
                StringBuilder AcountCondition = new StringBuilder(); //按照账套过滤
                StringBuilder StateCondition = new StringBuilder();//按照状态过滤
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.EAndLowerID && !string.IsNullOrEmpty(obj.Value))
                        {
                            string name = this.Db.QuerySingle<string>(string.Format("select c from udoc_staff where id = {0} ", obj.Value));
                            string str = obj.Value;
                            if (name != "S001")//非管理员  查询自己的和下级的采购订单
                            {
                                GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                                List<long> empList = getEmpList.GetListBuyId(long.Parse(obj.Value));
                                empList.Add(long.Parse(obj.Value));
                                str = string.Join(",", empList.Select(x => x.ToString()).ToArray());
                                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方
                                {
                                    AllEidCondition.AppendFormat(" and p.eid in ({0})", str);
                                }
                                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                                {
                                    AllEidCondition.AppendFormat(" and (p.linkmanid in ({0}) or coalesce(p.linkmanid,0)=0 or coalesce(p.linkmanid,0)=-1)", str);
                                }
                            }
                            else
                            {
                                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方
                                {
                                    EidCondition.AppendFormat(" and p.eid <>-2", obj.Value);
                                }
                                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                                {
                                    EidCondition.AppendFormat(" and p.linkmanid <>-2", obj.Value);
                                }
                            }
                        }
                        if (obj.SeacheKey == SearchEnum.EID && !string.IsNullOrEmpty(obj.Value))
                        {
                            if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方
                            {
                                EidCondition.AppendFormat(" and p.eid in ({0})", obj.Value);
                            }
                            if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                            {
                                EidCondition.AppendFormat(" and (p.linkmanid in ({0}) or coalesce(p.linkmanid,0)=0 or coalesce(p.linkmanid,0)=-1)", obj.Value);
                            }
                        }
                    }
                }
                if (EidCondition.Length == 0 &&AllEidCondition.Length==0)
                {
                    response.Success = false;
                    response.ResponseStatus.ErrorCode = "Err_SearchCondition";
                    response.ResponseStatus.Message = Const.Err_SearchCondition;
                    return response;
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方
                {
                    AcountCondition.Append(" and p.sa=" + request.AccountId);
                    StateCondition.Append(" and p.state<>0");
                    //AddressStr.Append("saddressid={0},saddress='{1}'");
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())
                {
                    AcountCondition.Append(" and p.a=" + request.AccountId);//采购方
                    StateCondition.Append("  and p.state<>-1");
                    //AddressStr.Append("addressid={0},address='{1}'");
                }
                if (AllEidCondition.Length == 0) //如果选择我的订单，则所有订单就为我的所有订单
                    AllEidCondition.Append(EidCondition.ToString());
                if (AllEidCondition.Length > 0) //如果选择全部订单，则所有状态的订单都为我的以及我的下级的订单
                {
                    EidCondition = new StringBuilder();
                    EidCondition.Append(AllEidCondition);
                }
                StringBuilder SearchStateSql = new StringBuilder(" select State,StateName,StateCount from (SELECT '-1' as State,'所有订单' as StateName,count(*) as StateCount,1 as Orderinfo FROM pur_order p where 1=1 " + AcountCondition + StateCondition + AllEidCondition);

                SearchStateSql.Append(" union  select '0' as State,'订单草稿' as StateName,count(*) as StateCount,2 as Orderinfo from pur_order p where p.state = 0 " + AcountCondition + EidCondition);
                SearchStateSql.Append(" union  select '1' as State,'待确认' as StateName,count(*) as StateCount,3 as Orderinfo from pur_order p where p.state = 1 " + AcountCondition + EidCondition);
                SearchStateSql.Append(" union  select '2' as State,'交易中' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 2 " + AcountCondition + EidCondition);
                SearchStateSql.Append(" union  select '3' as State,'已取消' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 3 " + AcountCondition + EidCondition);
                SearchStateSql.Append(" union  select '4' as State,'已完成' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 4 " + AcountCondition + EidCondition);
                SearchStateSql.Append(" ) as TotalInfo order by Orderinfo");

                
                var list = this.Db.Query<StateInfo>(string.Format(SearchStateSql.ToString()));
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        StateInfo.Add(item);
                    }

                    response.Data = StateInfo;
                    response.Success = true;
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoCount";
                    response.ResponseStatus.Message = "No data";
                    response.Success = false;
                }
                #endregion
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
