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
using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using Ndtech.PortalModel;
using System.Reflection;
using ServiceStack.ServiceInterface.ServiceModel;
using Ndtech.PortalService.service.management.myPurchase;
using ServiceStack.WebHost.Endpoints;
namespace Ndtech.PortalService.Auth
{

    public class SearchPurValidator : AbstractValidator<SearchPurInquiryByItemRequest>
    {
        //防止sql注入验证
        public override ValidationResult Validate(SearchPurInquiryByItemRequest instance)
        {
            ValidationResult result = new ValidationResult();
            SQLInjectionFilter.CheckOutList<SearchField>(instance.SearchCondition, result);
            SQLInjectionFilter.CheckOutList<SearchAttribute>(instance.AttCondition, result);
            SQLInjectionFilter.CheckOutList<Order>(instance.orders, result);
            return result;
            //ValidationResult result = new ValidationResult();
            //if (instance.SearchCondition != null && instance.SearchCondition.Count > 0)
            //{

            //    foreach (var item in instance.SearchCondition)
            //    {
            //        foreach (var info in item.GetType().GetProperties())
            //        {
            //            MethodInfo method = info.GetGetMethod();
            //            if (method != null)
            //            {

            //                if (info.PropertyType == Type.GetType("System.String") || info.PropertyType == Type.GetType("System.Int64") || info.PropertyType == Type.GetType("System.Int32") || info.PropertyType == Type.GetType("System.Boolean") || info.PropertyType == Type.GetType("System.Enum"))
            //                {
            //                    object obj = method.Invoke(item, null);
            //                    if (obj != null)
            //                    {
            //                        string value = obj.ToString();
            //                        if (!string.IsNullOrEmpty(value) && !ProcessSqlStr(value))
            //                        {
            //                            result.Errors.Add(new ValidationFailure(value, string.Format("{0}是非法关键字", value), "Error"));
            //                            return result;
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //    }
            //}

            //return base.Validate(instance);
        }

    }


    /// <summary>
    /// 询价搜索 根据产品代码|名称模糊搜索
    /// </summary>
    public class SearchPurInquiryByItemService : Service, IPost<SearchPurInquiryByItemRequest>
    {
        public object Post(SearchPurInquiryByItemRequest request)
        {
            SearchPurValidator asvalidator = new SearchPurValidator();
            ValidationResult result = asvalidator.Validate(request);
            PurInquiryByItemResponse response = new PurInquiryByItemResponse();
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }




            //第一种场景：未登陆查询平台询价信息
            StringBuilder SearchConditionsql = new StringBuilder("select distinct a.id,a.a,a.c,a.subject,a.FinalDateTime,a.createtime,a.Address,a.state,"
            + "c.n as CompName,quotations,eid_usrname,a.anonymouscode,g.corpnum from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c  on a.a=c.a join gl_acntsystems g on g.id=c.a where 1=1 and a.inquirytype!=1 and a.finaldatetime>=" + "'" + DateTime.Now + "'");
            StringBuilder CountConditionsql = new StringBuilder("select count(a.id) from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a where 1=1 a.finaldatetime>=" + "'" + DateTime.Now + "'");
            //第二种场景：登陆后管理后台查询本企业的询价信息
            #region 第二种场景：登陆后管理后台查询本企业的询价信息
            if (request.AccountID > 0 && string.IsNullOrEmpty(request.QuotationCompNumber))
            {
                SearchConditionsql = new StringBuilder("select distinct a.id,a.a,a.c,a.subject,a.FinalDateTime,a.createtime,a.Address,a.state,"
            + "c.n as CompName,quotations,eid_usrname,a.anonymouscode,g.corpnum  from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a join gl_acntsystems g on g.id=c.a");
                CountConditionsql = new StringBuilder("select count(distinct a.id) from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a");
                if (request.IsPortal)
                {
                    SearchConditionsql.Append(" where a.a=" + request.AccountID + " and a.anonymouscode=0");
                }
                else
                {
                    SearchConditionsql.Append(" where a.a=" + request.AccountID);
                }
               
                CountConditionsql.Append(" where a.a=" + request.AccountID);
                if (!string.IsNullOrEmpty(request.SubscribeInquiry))
                {
                    SearchConditionsql = new StringBuilder("select distinct a.id,a.a,a.c,a.subject,a.FinalDateTime,a.createtime,a.Address,a.state,"
            + "c.n as CompName,quotations,eid_usrname,Subdatetime,corpnum  from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid  join udoc_comp c on a.a=c.a join gl_acntsystems g on g.id=c.a");
                    CountConditionsql = new StringBuilder("select count(distinct a.id) from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a");
                    SearchConditionsql.Append(" join udoc_subscribe_receive d on a.id=d.from_id join udoc_Subscribe_contact e on d.to_a=e.to_a");
                    CountConditionsql.Append(" join udoc_subscribe_receive d on a.id=d.from_id join udoc_Subscribe_contact e on d.to_a=e.to_a");
                    SearchConditionsql.Append(" where d.to_a=" + request.AccountID);
                    CountConditionsql.Append(" where d.to_a=" + request.AccountID);
                }

                if (!string.IsNullOrEmpty(request.SubscribeInquiry))
                {
                    SearchConditionsql.Append(" and d.substate=1");
                    CountConditionsql.Append(" and d.substate=1");
                }

            }
            #endregion
            //第三种场景： 登陆后查询平台 已报价询价信息
            #region 登陆后查询平台 已报价询价信息
            if (!string.IsNullOrEmpty(request.QuotationCompNumber) && request.AlreadyQutation)
            {//未报价
                SearchConditionsql = new StringBuilder("select distinct a.id,a.a,a.c,a.subject,a.FinalDateTime,a.createtime,a.Address,a.state,"
+ "c.n as CompName,quotations,eid_usrname,a.anonymouscode,g.corpnum from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a  join gl_acntsystems g on g.id=c.a");
                CountConditionsql = new StringBuilder("select count(distinct a.id) from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a join sal_quotationcomp d on a.id = d.mid");
                SearchConditionsql.Append(" where a.id not in (select mid from sal_quotationcomp where CorpNum = '" + request.QuotationCompNumber + "' )  and a.inquirytype!=1 and b.a!='" + request.AccountID + "' ");
                CountConditionsql.Append(" where d.CorpNum='" + request.QuotationCompNumber + "'");
            }
            else if (!string.IsNullOrEmpty(request.QuotationCompNumber) && !request.AlreadyQutation)
            {
                //已报
                
                SearchConditionsql = new StringBuilder("select distinct a.id,a.a,a.c,a.subject,a.FinalDateTime,a.createtime,a.Address,a.state,"
+ "c.n as CompName,quotations,eid_usrname,a.anonymouscode,g.corpnum from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a join sal_quotationcomp d on a.id = d.mid join gl_acntsystems g on g.id=c.a");
                CountConditionsql = new StringBuilder("select count(distinct a.id) from pur_inquiry a left join pur_inquirydetail b on a.id=b.mid left join udoc_comp c on a.a=c.a");
                //SearchConditionsql.Append(" where a.id  in (select mid from sal_quotationcomp where CorpNum = '" + request.QuotationCompNumber + "' ) and b.a!=" + request.AccountID);
                SearchConditionsql.Append(" where  a.inquirytype!=1 and  d.corpnum='" + request.QuotationCompNumber + "'");
                CountConditionsql.Append(" where a.id not in (select mid from sal_quotationcomp CorpNum = '" + request.QuotationCompNumber + "' )");

            }
            #endregion
            List<string> standardCodes = new List<string>();
            //try
            //{
            //先搜索属性
            if (request.AttCondition != null && request.AttCondition.Count > 0)
            {
                StringBuilder attSql = new StringBuilder("select distinct standard_c as c from udoc_enterprise_item item join udoc_enterprise_attribute attr on item.id=attr.itemid");
                if (request.AccountID > 0)
                {
                    attSql = new StringBuilder("select distinct  standard_c as c from udoc_enterprise_item item join udoc_enterprise_attribute attr on item.id=attr.itemid");
                }
                Queue<string> listName = new Queue<string>();//用来存储别名
                listName.Enqueue("attr");
                //根据属性的个数拼自连接
                for (int j = 0; j < request.AttCondition.Count - 1; j++)
                {
                    attSql.AppendFormat(" join udoc_enterprise_attribute {0} on attr.itemid={0}.itemid", "a" + (j + 1));
                    listName.Enqueue("a" + (j + 1));
                }
                attSql.Append(" where 1=1 and ");
                if (request.AccountID > 0)
                {
                    attSql.Append("  item.a!=" + request.AccountID + " and");
                }
                // string sql = attSql.ToString();
                int i = 0;//判断是不是有多个不同的属性 
                int k = 0;
                foreach (var obj in request.AttCondition)
                {
                    if (obj.operatortype == operatorEnum.lk)
                    {
                        string name=listName.Dequeue();
                        foreach (string value in obj.Value)
                        {
                            attSql.Append(string.Format("({0}.attribute_class='{1}' and {0}.attribute_value like '%{2}%') or",name, obj.SearchKey, value));
                        }
                        attSql.Remove(attSql.Length - 2, 2);
                        //  sql = attSql.ToString().Substring(0, attSql.Length - 2);
                    }
                    else if (obj.operatortype == operatorEnum.eq)
                    {
                        if (i != 0 || k != 0)
                        {
                            attSql.Append(" and");
                        }
                        string name=listName.Dequeue();
                        foreach (string value in obj.Value)
                        {
                            attSql.Append(string.Format("({0}.attribute_class='{1}' and {0}.attribute_value = '{2}') or", name,obj.SearchKey, value));
                        }
                        attSql.Remove(attSql.Length - 2, 2);
                        i++;
                    }
                    else if (obj.operatortype == operatorEnum.bt)
                    {
                        if (k != 0)
                        {
                            attSql.Append(string.Format(" and ({0}.attribute_class='{1}' and to_number({0}.attribute_value,'999,999') >= '{2}' and to_number({0}.attribute_value,'999,999') <= '{3}') ", listName.Dequeue(), obj.SearchKey, obj.Value[0], obj.Value[1]));
                        }
                        else
                        {
                            if (i != 0)
                            {
                                attSql.Append(" and");
                            }
                            attSql.Append(string.Format("({0}.attribute_class='{1}' and to_number({0}.attribute_value,'999,999') >= '{2}' and to_number({0}.attribute_value,'999,999') <= '{3}') ", listName.Dequeue(), obj.SearchKey, obj.Value[0], obj.Value[1]));
                        }

                        k++;
                    }
                }

                // string sqlpage = "";
                standardCodes = this.Db.Query<string>(attSql.ToString()).ToList();
            }
            long eid = -1;
            //搜索关键字
            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                int InquiryState = 0;
             
                foreach (var obj in request.SearchCondition)
                {
                    if (obj.SeacheKey == SearchEnum.Subtype)
                    {
                        SearchConditionsql.Append(string.Format(" and d.subtype={0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and d.subtype={0}", obj.Value));
                    }

                    if (obj.SeacheKey == SearchEnum.CategoryCode)
                    {
                        SearchConditionsql.Append(string.Format(" and b.categorythirdcode='{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and b.categorythirdcode='{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.ItemorCode)
                    {
                        SearchConditionsql.Append(string.Format(" and ( b.standardItemCode like '%{0}%' or standardItemName like '%{0}%')", obj.Value));
                        CountConditionsql.Append(string.Format(" and ( b.standardItemCode like '%{0}%' or standardItemName like '%{0}%')", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.InquiryCodeorSubject)
                    {
                        SearchConditionsql.Append(string.Format(" and ( a.c like '%{0}%' or a.Subject like '%{0}%')", obj.Value));
                        CountConditionsql.Append(string.Format(" and ( a.c like '%{0}%' or a.Subject like '%{0}%')", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.BeginDateTime)
                    {
                        SearchConditionsql.Append(string.Format(" and a.createtime >= '{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and a.createtime >= '{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.EndDateTime)
                    {
                        SearchConditionsql.Append(string.Format(" and a.createtime <= '{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and a.createtime <= '{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.InquiryState)
                    {
                        InquiryState = Convert.ToInt32(obj.Value);
                        if (InquiryState == 1)//询价中
                        {
                            SearchConditionsql.Append(string.Format(" and a.FinalDateTime>now()"));
                            CountConditionsql.Append(string.Format(" and a.FinalDateTime>now()"));
                        }
                        if (InquiryState == 4)//已截止
                        {
                            SearchConditionsql.Append(string.Format(" and a.FinalDateTime<now()"));
                            CountConditionsql.Append(string.Format(" and a.FinalDateTime<now()"));
                        }
                    }
                    else if ((obj.SeacheKey == SearchEnum.EID ||obj.SeacheKey == SearchEnum.EAndLowerID )&& !string.IsNullOrEmpty(obj.Value))
                    {
                        long.TryParse(obj.Value, out eid);
                    }
                    
                    else if (obj.SeacheKey == SearchEnum.Address)
                    {
                        SearchConditionsql.Append(string.Format(" and a.Address like '%{0}%'", obj.Value));
                        CountConditionsql.Append(string.Format(" and a.Address like '%{0}%'", obj.Value));
                    }

                }
                if (InquiryState != 0)
                {
                    SearchConditionsql.Append(string.Format(" and a.State = {0}", InquiryState));
                    CountConditionsql.Append(string.Format(" and a.State = {0}", InquiryState));
                }
            }
            if (request.IsSearchAll && request.AccountID>0)
            {
                //所有的询价单
                string name = this.Db.Where<NdtechStaffInfo>(s => s.ID == eid).Select(x => x.SysCode).FirstNonDefault();
                if (name != "S001")//非管理员  查询自己的和下级的采购订单
                {
                    GetLowerEmployeeListID getEmpList = this.GetAppHost().GetContainer().TryResolveNamed<GetLowerEmployeeListID>("LowerEmployeeList");
                    List<long> listID = getEmpList.GetListBuyId(eid);
                    listID.Add(eid);
                    string str = string.Join(",", listID.Select(x => x.ToString()).ToArray());
                    SearchConditionsql.AppendFormat(" and a.eid in ({0})", str);
                    CountConditionsql.Append(string.Format(" and a.eid in ({0})", str));
                }
                else
                {
                    SearchConditionsql.Append(" and a.eid <>-2");
                    CountConditionsql.Append(" and a.eid <>-2");
                }
            }
            else if(eid !=-1 &&request.AccountID>0)
            {//我的询价单
                SearchConditionsql.Append(string.Format(" and a.eid = '{0}'",    eid));
                CountConditionsql.Append(string.Format(" and a.eid = '{0}'", eid));
            }
            //拼接搜索属性查询结果
            if (standardCodes.Count > 0)
            {
                //standardCodes.ForEach(s => s = string.Format("'{0}'", s));
                standardCodes = standardCodes.ConvertAll(s => string.Format("'{0}'", s));
                //StringBuilder sqlids = new StringBuilder();
                string str = String.Join(",", standardCodes.Select(x => x.ToString()).ToArray());
                SearchConditionsql.Append(" and b.standarditemcode in (" + str + ")");
                CountConditionsql.Append(" and b.standarditemcode in (" + str + ")");
            }
            if (standardCodes.Count == 0 && request.AttCondition != null && request.AttCondition.Count > 0)
            {
                SearchConditionsql.Append(" and b.standarditemcode = null");
                CountConditionsql.Append(" and b.standarditemcode  = null");
            }
            if (request.orders != null && request.orders.Count > 0)
            {
                StringBuilder ordersql = new StringBuilder(" order by ");
                foreach (var order in request.orders)
                {
                    if (order.sortKey == SortKey.Ascending)
                    {
                        ordersql.Append(order.orderKey + " asc");
                    }
                    else
                    {
                        ordersql.Append(order.orderKey + " desc");
                    }
                }
                SearchConditionsql.Append(ordersql.ToString().Substring(0, ordersql.ToString().Length));
            }
            response.RowsCount = this.Db.Query<PurInquirySearchView>(string.Format(SearchConditionsql.ToString())).Count;
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
            var list = this.Db.Query<PurInquiryList>(string.Format(SearchConditionsql.ToString()));
            //long count = this.Db.QuerySingle<long>(string.Format(CountConditionsql.ToString()));
            //  List<PurInquiryList> listModel = new List<PurInquiryList>();
            //foreach (var item in list)
            //{
            //    PurInquiryList aa = item.TranslateTo<PurInquiryList>();
            //    listModel.Add(aa);
            //}

            //response.RowsCount = count;
            if (!string.IsNullOrEmpty(request.QuotationCompNumber) && request.AlreadyQutation)
            {
                 //获取该公司是否已经被收藏
                foreach (var item in list)
                {
                    var subRec = this.Db.FirstOrDefault<SubscribeReceive>(s => s.FromID == item.ID);
                    if (subRec != null)
                    {
                        item.Substate = subRec.Substate;
                    }
                    else
                    {
                        item.Substate = 0;
                    }
                }
            }
            response.Data = list;
            response.Success = true;

            // }
            //catch (Exception ex)
            //{

            //    response.Success = false;
            //}
            return response;
        }
    }
}
