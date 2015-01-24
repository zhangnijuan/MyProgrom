using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与数据库有关
using ServiceStack.Common;
using ServiceStack.WebHost.Endpoints;
using Ndtech.PortalModel;
using Ndtech.PortalService.service.management.myPurchase;

namespace Ndtech.PortalService.Auth
{
    public class GetPurInquryStateCountService: Service, IPost<GetPurInquryStateCountRequest>
    {
        public object Post(GetPurInquryStateCountRequest request)
        {
            GetPurInquryStateCountResponse response = new GetPurInquryStateCountResponse();
            List<StateInfo> StateInfo = new List<StateInfo>();

            if (!string.IsNullOrEmpty(request.AccountId.ToString()))
            {
                StringBuilder AllEidCondition = new StringBuilder();//按照制单人以及下级条件过滤
                StringBuilder EidCondition = new StringBuilder();//按制单人过滤
                bool isSearchAll = false;
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.EAndLowerID && !string.IsNullOrEmpty(obj.Value))
                        {
                            isSearchAll = true;
                            string name = this.Db.QuerySingle<string>(string.Format("select c from udoc_staff where id = {0} ", obj.Value));
                            string str = obj.Value;
                            if (name != "S001")//非管理员  查询自己的和下级的采购订单
                            {
                                GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                                List<long> empList = getEmpList.GetListBuyId(long.Parse(obj.Value));
                                empList.Add(long.Parse(obj.Value));
                                str = string.Join(",", empList.Select(x => x.ToString()).ToArray());
                                AllEidCondition.AppendFormat(" and p.eid in ({0})", str);
                                
  
                            }
                            else
                            {
                                  EidCondition.AppendFormat(" and p.eid <>-2", obj.Value);

                            }
                        }
                        if (obj.SeacheKey == SearchEnum.EID && !string.IsNullOrEmpty(obj.Value))
                        {
                            EidCondition.Append(" and p.eid=" + obj.Value);
                        }
                    }
                }
                if (AllEidCondition.Length == 0)
                    AllEidCondition.Append(EidCondition.ToString());
                if (isSearchAll)
                {
                    EidCondition = new StringBuilder();
                    EidCondition.Append(AllEidCondition);
                }
                StringBuilder SerchStateSql = new StringBuilder("select State,StateName,StateCount from (SELECT '-1' as State,'全部' as StateName,count(*) as StateCount,1 as Orderinfo FROM pur_inquiry p where p.a=" + request.AccountId + AllEidCondition);

                SerchStateSql.Append(" union  select '1' as State,'询价中' as StateName,count(*) as StateCount,2 as Orderinfo from pur_inquiry p where finaldatetime>now() and state=1 and p.a=" + request.AccountId + EidCondition);
                SerchStateSql.Append(" union  select '4' as State,'已截止' as StateName,count(*) as StateCount,3 as Orderinfo from pur_inquiry p where finaldatetime<now() and state=4 and p.a=" + request.AccountId + EidCondition);
                SerchStateSql.Append(" union  select '2' as State,'已优选' as StateName,count(*) as StateCount,4 as Orderinfo from pur_inquiry p where  state=2 and p.a=" + request.AccountId + EidCondition);
                SerchStateSql.Append(" union  select '5' as State,'已下单' as StateName,count(*) as StateCount,5 as Orderinfo from pur_inquiry p where  state=5 and p.a=" + request.AccountId + EidCondition);
                SerchStateSql.Append(" union  select '3' as State,'已关闭' as StateName,count(*) as StateCount,6 as Orderinfo from pur_inquiry p where  state=3 and p.a=" + request.AccountId + EidCondition);
                SerchStateSql.Append(") as TotalInfo order by Orderinfo");
                var list = this.Db.Query<StateInfo>(string.Format(SerchStateSql.ToString()));
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
