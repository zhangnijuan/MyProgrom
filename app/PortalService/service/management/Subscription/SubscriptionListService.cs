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
namespace Ndtech.PortalService.service.management.Subscription
{
    public class SubscriptionListService : Service, IPost<SubscriptionListRequest>
    {
        public object Post(SubscriptionListRequest request)
        {
            SubscriptionListResponse response = new SubscriptionListResponse();
            var sf = this.Db.Where<SubscribeFilter>(s => s.Subscriber == request.Subscriber);
            SubscriptionList list = new SubscriptionList();

            if (sf.Count > 0)
            {
                List<ReturnSubscription> rs = new List<ReturnSubscription>();
                List<CategoryName> cn = new List<CategoryName>();
                Dictionary<string, int> dic = new Dictionary<string, int>();//用来存分类名称和该名称下的订单数量
                foreach (var item in sf)
                {
                    var sfd = this.Db.Where<SubscribeFilterDetail>(s => s.Mid == item.ID).Select(q => q.CategoryName).ToList();
                    var sub = item.TranslateTo<ReturnSubscription>();
                    StringBuilder sql = new StringBuilder("select distinct p.id from  pur_inquiry p join pur_inquirydetail q on p.id=q.mid  left join udoc_comp u on p.a=u.a  where p.id not in (select mid from sal_quotationcomp where a = '" + request.AccountID + "' )");
                    StringBuilder sqlCount = new StringBuilder("select distinct p.id from  pur_inquiry p join pur_inquirydetail q on p.id=q.mid  left join udoc_comp u on p.a=u.a  where p.id not in (select mid from sal_quotationcomp where a = '" + request.AccountID + "' )");
                    //拼sql语句 查询数量 
                    if (!string.IsNullOrEmpty(item.FromName))
                    {
                        sql.AppendFormat(" and u.n like '%{0}%'", item.FromName);
                        sqlCount.AppendFormat(" and u.n like '%{0}%'", item.FromName);
                    }
                    if (!string.IsNullOrEmpty(item.DeliveryAddress))
                    {
                        sql.AppendFormat(" and p.address like '%{0}%'", item.DeliveryAddress);
                        sqlCount.AppendFormat(" and p.address like '%{0}%'", item.DeliveryAddress);
                    }
                    //if (!string.IsNullOrEmpty(item.CategoryName))
                    //{
                    //    sql.AppendFormat(" and q.categorythirdname='{0}'", item.CategoryName);
                    //}
                    if (sfd.Count() > 0)
                    {
                        sql.AppendFormat("and (");
                        foreach (var s in sfd)
                        {
                            sql.AppendFormat(" q.categorythirdname='{0}' or", s);
                        }
                        sql.Remove(sql.Length - 2, 2);
                        sql.AppendFormat(")");
                    }

                    if (item.MaxQty != 0)
                    {
                        sql.AppendFormat(" and q.qty>={0} and q.qty<={1}", item.MinQty, item.MaxQty);
                        sqlCount.AppendFormat(" and q.qty>={0} and q.qty<={1}", item.MinQty, item.MaxQty);
                    }
                    sql.AppendFormat(" and p.createtime>='{0}' and p.finaldatetime>='{1}'", item.LastTime,DateTime.Now);
                    sqlCount.AppendFormat(" and p.createtime>='{0}' and p.finaldatetime>='{1}'", item.LastTime, DateTime.Now);
                    sub.Count = this.Db.Query<PurInquiry>(sql.ToString()).Count;
                    sub.CategoryName = sfd;
                    rs.Add(sub);
                    //把分类名称和订单数量存入键值对集合中
                    foreach (var s in sfd)
                    {
                        sqlCount.AppendFormat(" and q.categorythirdname='{0}'",s);
                        int count = this.Db.Query<PurInquiry>(sqlCount.ToString()).Count;
                        sqlCount.Remove(sqlCount.ToString().LastIndexOf("and"), sqlCount.Length - sqlCount.ToString().LastIndexOf("and"));
                        if (!dic.ContainsKey(s))
                        {
                            dic.Add(s, count);
                        }
                        else
                        {
                            dic[s] += count;
                        }
                    }

                    
                }
                foreach (var item in dic)
                {
                    cn.Add(new CategoryName() { Name = item.Key, Count = item.Value });
                }
                response.Data = new SubscriptionList() { Subscription = rs, Category = cn };
            }
            else
            {
                response.Data = null;
            }
            response.Success = true;
            return response;
        }
    }
}
