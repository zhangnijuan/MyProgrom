using Ndtech.PortalModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与数据库有关
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;
using Ndtech.PortalModel;


namespace Ndtech.PortalService.Auth
{
    public class GetSalQuotationStateCntService : Service, IGet<GetSalQuotationStateCntRequest>
    {
        public object Get(GetSalQuotationStateCntRequest request)
        {
            GetSalQuotationStateCntResponse response = new GetSalQuotationStateCntResponse();
            List<StateInfo> StateInfo = new List<StateInfo>();
            StringBuilder saleid = new StringBuilder();

            if (!string.IsNullOrEmpty(request.AccountId.ToString()))
            {
                StringBuilder ADDEID = new StringBuilder();
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.EID && !string.IsNullOrEmpty(obj.Value))
                        {
                            ADDEID.Append(" and p.eid=" + obj.Value);
                            saleid.Append(" and (p.eid=" + obj.Value + " or p.eid=0)");
                        }
                    }
                }
                StringBuilder SerchStateSql = new StringBuilder(" select State,StateName,StateCount from (SELECT '-1' as State,'全部' as StateName,count(*) as StateCount,1 as Orderinfo FROM sal_quotation p where p.a=" + request.AccountId + ADDEID);

                SerchStateSql.Append(" union  select '0' as State,'待报价' as StateName,count(*) as StateCount,2 as Orderinfo from sal_quotation p join pur_inquiry b on p.inquiryid=b.id where p.state = 0 and b.state=1 and p.a=" + request.AccountId);
                SerchStateSql.Append(" union  select '5' as State,'已关闭' as StateName,count(*) as StateCount,3 as Orderinfo from sal_quotation p where state = 5 and p.a=" + request.AccountId + ADDEID);
                SerchStateSql.Append(" union  select '6' as State,'已忽略' as StateName,count(*) as StateCount,4 as Orderinfo from sal_quotation p where state = 6 and p.a=" + request.AccountId + ADDEID);
                SerchStateSql.Append(" ) as TotalInfo order by Orderinfo");

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
