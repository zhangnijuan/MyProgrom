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

namespace Ndtech.PortalService.service.management.Subscription
{
    public class SearchSubscriptionService : Service, IPost<SearchSubscriptionRequest>
    {

        public object Post(SearchSubscriptionRequest request)
        {
            SearchSubscriptionResponse response = new SearchSubscriptionResponse();
            StringBuilder sbSql = new StringBuilder("select distinct a.id,a.c,a.subject,u.n,a.createtime,a.finaldatetime,a.quotations,a.anonymouscode,u.c,u.a from pur_inquiry a join pur_inquirydetail b on a.id=b.mid   join udoc_comp u on a.a=u.a where  a.id not in (select mid from sal_quotationcomp where a = '" + request.AccountID + "' )");
            //按主题查询
            #region 按主题查询
            if (request.SearchCondition != null && request.EstimteCondition == Estimate.Theme)
            {
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchSubscriptionEnum.CategoryName && item.Value != null && item.Value.Count > 0)
                    {
                        sbSql.AppendFormat(" and (");
                        //多个产品分类拼接
                        foreach (var v in item.Value)
                        {
                            sbSql.AppendFormat(" b.categorythirdname='{0}' or", v);
                        }
                        sbSql.Remove(sbSql.Length - 2, 2);
                        sbSql.AppendFormat(")");
                    }
                    if (item.SeacheKey == SearchSubscriptionEnum.DeliveryAddress && item.Value != null && item.Value.Count > 0)
                    {
                        sbSql.AppendFormat(" and a.address like '%{0}%'", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSubscriptionEnum.ItemCode && item.Value != null && item.Value.Count > 0)
                    {
                        sbSql.AppendFormat(" and ( b.standarditemcode like '%{0}%' or  b.standarditemname like '%{0}%')", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSubscriptionEnum.MaxQty && item.Value.Count > 0 && Convert.ToInt32(item.Value[0]) != 0)
                    {
                        sbSql.AppendFormat(" and  b.qty<={0}", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSubscriptionEnum.MinQty && item.Value.Count > 0)
                    {
                        sbSql.AppendFormat(" and b.qty>={0}", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSubscriptionEnum.FromName && item.Value != null && item.Value.Count > 0)
                    {
                        if (string.IsNullOrEmpty(item.Value[0]))
                        {
                            sbSql.AppendFormat(" and u.n like '%{0}%'", item.Value[0]);
                        }
                        else
                        {
                            sbSql.AppendFormat(" and u.n like '%{0}%' and a.anonymouscode!=1", item.Value[0]);
                        }
                       
                    }
                    
                }
                sbSql.AppendFormat(" and a.finaldatetime>='{0}'", DateTime.Now);
                //更改订阅主表最后一次查询的时间
                this.Db.Update<SubscribeFilter>(set: "LastTime={0}".Params(DateTime.Now), where: "ID={0}".Params(request.ID));
                if (request.OrderBy != null)
                {

                    foreach (var order in request.OrderBy)
                    {

                        if (order.sortKey == SortKey.Ascending)
                        {

                            sbSql.AppendFormat(" order by {0} asc", order.orderKey);
                        }
                        else
                        {
                            sbSql.AppendFormat(" order by {0} desc", order.orderKey);
                        }
                    }
                }

                response.RowsCount = this.Db.Query<ReturnSearchInfo>(sbSql.ToString()).Count;
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
                response.Data = this.Db.Query<ReturnSearchInfo>(sbSql.ToString());
            }
            #endregion
            //按分类查询
            #region 按分类查询
            if (request.SearchCondition != null && request.EstimteCondition == Estimate.Category)
            {
                List<ReturnSearchInfo> list = new List<ReturnSearchInfo>();
                var listId = this.Db.Where<SubscribeFilter>(s => s.Subscriber == request.ID).Select(s => s.ID);
                var subDetail = this.Db.Where<SubscribeFilterDetail>(s => s.CategoryName == request.CategoryName && listId.Contains(s.Mid));
                foreach (var item in subDetail)
                {
                    var sub = this.Db.FirstOrDefault<SubscribeFilter>(s => s.ID == item.Mid);
                    //更改订阅主表最后一次查询的时间
                    this.Db.Update<SubscribeFilter>(set: "LastTime={0}".Params(DateTime.Now), where: "ID={0}".Params(sub.ID));
                    sbSql = new StringBuilder("select distinct a.id,a.c,a.subject,u.n,a.createtime,a.finaldatetime,a.quotations from pur_inquiry a join pur_inquirydetail b on a.id=b.mid   join udoc_comp u on a.a=u.a where a.id not in (select mid from sal_quotationcomp where a = '" + request.AccountID + "' ) ");
                    foreach (var s in request.SearchCondition)
                    {
                        if (s.SeacheKey == SearchSubscriptionEnum.ItemCode && s.Value != null && s.Value.Count > 0)
                        {
                            sbSql.AppendFormat("  and ( b.standarditemcode like '%{0}%' or b.standarditemname like '%{0}%')", s.Value[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(sub.FromName))
                    {
                        if (string.IsNullOrEmpty(sub.FromName))
                        {
                            sbSql.AppendFormat(" and u.n like '%{0}%'", sub.FromName);
                        
                        }
                        else
                        {
                            sbSql.AppendFormat(" and u.n like '%{0}%' and a.anonymouscode!=1", sub.FromName);
                        }
                       
                    }
                    if (!string.IsNullOrEmpty(sub.DeliveryAddress))
                    {
                        sbSql.AppendFormat(" and a.address like '%{0}%'", sub.DeliveryAddress);
                    }
                    if (sub.MaxQty != 0)
                    {
                        sbSql.AppendFormat(" and b.qty>={0} and b.qty<={1}", sub.MinQty, sub.MaxQty);
                    }
                    sbSql.AppendFormat(" and b.categorythirdname='{0}' and a.finaldatetime>='{1}'", item.CategoryName, DateTime.Now);
                    list.AddRange(this.Db.Query<ReturnSearchInfo>(sbSql.ToString()));
                }
                //去掉重复的数据
                if (list.Count > 0)
                {
                    HashSet<long> ids = list.Select(s => s.ID).ToHashSet();
                    request.PageIndex = request.PageIndex == 0 ? 1 : request.PageIndex;
                    if (request.OrderBy != null)
                    {

                        foreach (var order in request.OrderBy)
                        {
                            if (order.orderKey == OrderKey.FinalDateTime)
                            {
                                list = list.Where(s => ids.Contains(s.ID)).OrderByDescending(s => s.FinalDateTime).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
                            }
                            if (order.orderKey == OrderKey.createtime)
                            {
                                if (order.sortKey == SortKey.Ascending)
                                {
                                    list = list.Where(s => ids.Contains(s.ID)).OrderBy(s => s.CreateTime).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
                                }
                                else
                                {
                                    list = list.Where(s => ids.Contains(s.ID)).OrderByDescending(s => s.CreateTime).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
                                }
                            }
                        }
                    }

                    response.RowsCount = ids.Count;
                    response.Data = list;
                }

            }
            #endregion

            response.Success = true;


            return response;
        }
    }
}
