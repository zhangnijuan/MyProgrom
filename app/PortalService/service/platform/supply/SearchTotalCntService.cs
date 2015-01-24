using Ndtech.PortalModel;
using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalModel.ViewModel.platform.supply;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;

namespace Ndtech.PortalService.service.platform.supply
{
    public class SearchTotalCntService : Service, IPost<SearchTotalCntRequest>
    {
       // ILog log = LogManager.GetLogger(typeof(SearchTotalCntService));
        
        public object Post(SearchTotalCntRequest request)
        {
            SearchTotalCntResponse response = new SearchTotalCntResponse();
            
            //List<StateCntInfoList> StateCntInfoList = new List<PortalModel.ViewModel.platform.supply.StateCntInfoList>();
            StateCntInfoList StateCnt = new PortalModel.ViewModel.platform.supply.StateCntInfoList();
            //询价单

            List<StateInfo> PurInquiryState = new List<StateInfo>();

            StringBuilder PurEidCondition = new StringBuilder();
            StringBuilder SalEidCondition = new StringBuilder();
            StringBuilder SalOrderEidCondition = new StringBuilder();
            if (!string.IsNullOrEmpty(request.AccountID.ToString()))
            {
                
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.EID && !string.IsNullOrEmpty(obj.Value))
                        {
                            PurEidCondition.Append(" and p.eid=" + obj.Value);
                            SalEidCondition.Append(" and (p.eid=" + obj.Value+" or p.eid=0)");
                            SalOrderEidCondition.Append(" and p.linkmanid=" + obj.Value);
                        }
                    }
                }
             }
            StringBuilder SerchInqStateSql = new StringBuilder("select State,StateName,StateCount from (SELECT '-1' as State,'全部' as StateName,count(*) as StateCount,1 as Orderinfo FROM pur_inquiry p where p.a=" + request.AccountID + PurEidCondition);

                SerchInqStateSql.Append(" union  select '1' as State,'询价中' as StateName,count(*) as StateCount,2 as Orderinfo from pur_inquiry p where finaldatetime>now() and state=1 and p.a=" + request.AccountID + PurEidCondition);
                SerchInqStateSql.Append(" union  select '4' as State,'已截止' as StateName,count(*) as StateCount,3 as Orderinfo from pur_inquiry p where finaldatetime<now() and state=4 and p.a=" + request.AccountID + PurEidCondition);
                SerchInqStateSql.Append(" union  select '2' as State,'已优选' as StateName,count(*) as StateCount,4 as Orderinfo from pur_inquiry p where  state=2 and p.a=" + request.AccountID + PurEidCondition);
                SerchInqStateSql.Append(" union  select '5' as State,'已下单' as StateName,count(*) as StateCount,5 as Orderinfo from pur_inquiry p where  state=5 and p.a=" + request.AccountID + PurEidCondition);
                SerchInqStateSql.Append(" union  select '3' as State,'已关闭' as StateName,count(*) as StateCount,6 as Orderinfo from pur_inquiry p where  state=3 and p.a=" + request.AccountID + PurEidCondition);
                SerchInqStateSql.Append(") as TotalInfo order by Orderinfo");
                var Inqlist = this.Db.Query<StateInfo>(string.Format(SerchInqStateSql.ToString()));
                if (Inqlist != null && Inqlist.Count > 0)
                {
                    foreach (var item in Inqlist)
                    {
                        PurInquiryState.Add(item);
                    }
                    StateCnt.PurInquiryStateInfo = PurInquiryState;
                    response.PurInquirySuccess = true;
                }
                

            //报价单
            List<StateInfo> SalQuotation=new List<StateInfo> ();
            StringBuilder SerchSalStateSql = new StringBuilder(" select State,StateName,StateCount from (SELECT '-1' as State,'全部' as StateName,count(*) as StateCount,1 as Orderinfo FROM sal_quotation p where p.a=" + request.AccountID + PurEidCondition);

            SerchSalStateSql.Append(" union  select '0' as State,'待报价' as StateName,count(*) as StateCount,2 as Orderinfo from sal_quotation p join pur_inquiry b on p.inquiryid=b.id where p.state = 0  and b.state=1 and p.a=" + request.AccountID);
                SerchSalStateSql.Append(" union  select '5' as State,'已关闭' as StateName,count(*) as StateCount,3 as Orderinfo from sal_quotation p where state = 5 and p.a=" + request.AccountID + PurEidCondition);
                SerchSalStateSql.Append(" union  select '6' as State,'已忽略' as StateName,count(*) as StateCount,4 as Orderinfo from sal_quotation p where state = 6 and p.a=" + request.AccountID + PurEidCondition);
                SerchSalStateSql.Append(" ) as TotalInfo order by Orderinfo");

                var sallist = this.Db.Query<StateInfo>(string.Format(SerchSalStateSql.ToString()));
                if (sallist != null && sallist.Count > 0)
                {
                    foreach (var item in sallist)
                    {
                        SalQuotation.Add(item);
                    }
                    StateCnt.SalQutationStateInfo = SalQuotation;
                    response.SalQuotationSuccess = true;
                }                

       
            //订单
                List<StateInfo> PurOrserState = new List<StateInfo>();
                StringBuilder SearchSql = new StringBuilder();
                StringBuilder UpdateStr = new StringBuilder();
                StringBuilder PurOrderADDEID = new StringBuilder();
                if (request.SearchCondition != null && request.SearchCondition.Count > 0)
                {
                    foreach (var obj in request.SearchCondition)
                    {
                        if (obj.SeacheKey == SearchEnum.EID && !string.IsNullOrEmpty(obj.Value))
                        {
                            if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//供应方
                            {
                                PurOrderADDEID.Append(" and p.eid=" + obj.Value);
                            }
                            if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                            {
                                PurOrderADDEID.Append(" and (p.linkmanid=" + obj.Value + " or coalesce(p.linkmanid,0)=0 or coalesce(p.linkmanid,0)=-1)");
                            }
                        }
                    }
                }
                
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方
                {
                    SearchSql.Append(" and p.sa=" + request.AccountID);
                    UpdateStr.Append(" and p.state!=0");
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())
                {
                    SearchSql.Append(" and p.a=" + request.AccountID);//采购方
                    UpdateStr.Append("");
                }
                StringBuilder SerchStateSql = new StringBuilder(" select State,StateName,StateCount from (SELECT '-1' as State,'所有订单' as StateName,count(*) as StateCount,1 as Orderinfo FROM pur_order p where 1=1 " + SearchSql + UpdateStr + PurOrderADDEID);

                SerchStateSql.Append(" union  select '0' as State,'订单草稿' as StateName,count(*) as StateCount,2 as Orderinfo from pur_order p where p.state = 0 " + SearchSql  + PurOrderADDEID);
                SerchStateSql.Append(" union  select '1' as State,'待确认' as StateName,count(*) as StateCount,3 as Orderinfo from pur_order p where p.state = 1 " + SearchSql + PurOrderADDEID);
                SerchStateSql.Append(" union  select '2' as State,'交易中' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 2 " + SearchSql + PurOrderADDEID);
                SerchStateSql.Append(" union  select '3' as State,'已取消' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 3 " + SearchSql + PurOrderADDEID);
                SerchStateSql.Append(" union  select '4' as State,'已完成' as StateName,count(*) as StateCount,4 as Orderinfo from pur_order p where p.state = 4 " + SearchSql + PurOrderADDEID);
                SerchStateSql.Append(" ) as TotalInfo order by Orderinfo");


                var list = this.Db.Query<StateInfo>(string.Format(SerchStateSql.ToString()));
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        PurOrserState.Add(item);
                    }
                    StateCnt.PurOrderStateInfo = PurOrserState;
                    response.PurOrderSuccess = true;
                }

                //订单交易中待收货，待收款数量统计
                List<StateInfo> PurOrserDetState = new List<StateInfo>();
                //StringBuilder SearchDetSql = new StringBuilder();
                string outSql = string.Format("select c.detailid from sal_outnoticedetail c where c.state = 0 and"
                + " c.detailid in  ( select b.id from pur_order p join pur_orderdetail b on  p.id = b.mid  where 1=1 and p.state=2 {0})  ", PurEidCondition.ToString());
              var outStateCount = this.Db.Query<long>(outSql).Count;
              string arapSql = string.Format("select count(*) from arap_receiving where state = 0 and orderid in (select id from pur_order p where 1=1 and p.state=2 {0} )", SalOrderEidCondition.ToString());
              var arapStateCount = this.Db.QuerySingle<int>(arapSql);
                //SearchDetSql.Append(" union select '2' as State,'交易中待收款' as StateName,count(*) as StateCount,2 as Orderinfo from pur_order p where 1=1 and state = 2 and circulationstate=2 " + SearchSql + ADDEID);
                //SearchDetSql.Append(" ) as TotalInfo order by Orderinfo");
                //var detlist = this.Db.Query<StateInfo>(string.Format(SearchDetSql.ToString()));
                //if (detlist != null && detlist.Count > 0)
                //{
                //    foreach (var item in detlist)
                //    {
                //        PurOrserDetState.Add(item);
                //    }
                //    StateCnt.PurOrderDetStateInfo = PurOrserDetState;
                //    response.PurOrderDetSuccess = true;
                //}
              PurOrserDetState.Add(new StateInfo { State = 1, StateCount = outStateCount, StateName = "交易中待收货" }); //把同一个制单人的不同订单的未发货记录合计
              PurOrserDetState.Add(new StateInfo { State = 2, StateCount = arapStateCount, StateName = "交易中待收款" });
              StateCnt.PurOrderDetStateInfo = PurOrserDetState;
                response.Data = StateCnt;

            return response;
        }

    }
}
