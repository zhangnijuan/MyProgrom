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
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using Ndtech.PortalService.Extensions;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    public class SearchProductByItemValidator : AbstractValidator<SearchEnterpriseItemRequest>
    {
        public override ValidationResult Validate(SearchEnterpriseItemRequest instance)
        {
            ValidationResult result = new ValidationResult();
            SQLInjectionFilter.CheckOutList<SearchField>(instance.SearchCondition, result);
            SQLInjectionFilter.CheckOutList<SearchAttribute>(instance.AttCondition, result);
            SQLInjectionFilter.CheckOutList<Order>(instance.orders, result);
            return result;
        }
    }
    /// <summary>
    /// 产品搜索 根据产品代码|名称模糊搜索
    /// </summa
    public class SearchProductByItemService : Service, IPost<SearchEnterpriseItemRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(SearchProductByItemService));

        public object Post(SearchEnterpriseItemRequest request)
        {
            SearchProductByItemValidator asvalidator = new SearchProductByItemValidator();
            ValidationResult result = asvalidator.Validate(request);
            ProductByItemResponse response = new ProductByItemResponse();
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            StringBuilder SearchConditionsql = new StringBuilder("select a.*,b.n as companyname,g.corpnum from udoc_enterprise_item a LEFT JOIN udoc_comp b ON a.a = b.a  join gl_acntsystems g on g.id=b.a where 1=1 and a.state != 3 and a.biztype = 2");
            StringBuilder CountConditionsql = new StringBuilder("select count(id) from udoc_enterprise_item where 1=1 and state != 3");
            if (request.AccountID > 0 && request.ComparePrice != "Y")
            {
                SearchConditionsql = new StringBuilder("select a.*,b.n as companyname,g.corpnum  from udoc_enterprise_item a LEFT JOIN udoc_comp b ON a.a = b.a  join gl_acntsystems g on g.id=b.a  where 1=1 and a.state != 3 and a.a=" + request.AccountID);
                CountConditionsql = new StringBuilder("select count(id) from udoc_enterprise_item  where state != 3 and a=" + request.AccountID);
                //SearchConditionsql.Append(");
                //CountConditionsql.Append(" where state != 3 and a=" + request.AccountID);
                if (request.SubscribeProduct != null)
                {
                    SearchConditionsql = new StringBuilder("select distinct a.*,b.n as companyname,subdatetime,g.corpnum  from udoc_enterprise_item a LEFT JOIN udoc_comp b ON a.a = b.a join udoc_subscribe_receive d on a.id=d.from_id join udoc_Subscribe_contact e on d.to_a=e.to_a join gl_acntsystems g on g.id=b.a where state != 3 and d.to_a=" + request.AccountID);

                    CountConditionsql = new StringBuilder("select count(udoc_enterprise_item.id) from udoc_enterprise_item join udoc_subscribe_receive d on udoc_enterprise_item.id=d.from_id join udoc_Subscribe_contact e on d.to_a=e.to_a where state != 3 and d.to_a=" + request.AccountID);
                    SearchConditionsql.Append(" and d.substate=1 and d.subtype=1");
                    CountConditionsql.Append(" and d.substate=1 and d.subtype=1");
                }
            }

            List<long> listID = new List<long>();


            if (request.ComparePrice == "Y")
            {
                #region 产品商家比价查询

                if (request.AccountID > 0)
                {
                    SearchConditionsql = new StringBuilder(string.Format(
                                                          @"select a.*,b.n as companyname, g.corpnum from udoc_enterprise_item a 
                                                            left join udoc_comp b on a.a = b.a 
                                                            left join gl_acntsystems g on g.id = b.a
                                                            where a.a != {0} and a.state = 1 and a.biztype = 2", request.AccountID));
                    CountConditionsql = new StringBuilder(string.Format("select count(id) from udoc_enterprise_item where a != {0} and state = 1 and a.biztype = 2", request.AccountID));
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_AccountIDIsNull";
                    response.ResponseStatus.Message = "No AccountID Parameter";
                    return response;
                }

                #endregion
            }
            else
            {

                //    //先搜索属性
                if (request.AttCondition != null && request.AttCondition.Count > 0)
                {
                    StringBuilder attSql = new StringBuilder("select  a0.itemid from udoc_enterprise_attribute a0 ");     
                    if (request.AccountID > 0)
                    {
                        attSql = new StringBuilder("select a0.itemid from udoc_enterprise_attribute a0");
                    }
                    Queue<string> listName = new Queue<string>();//用来存储别名
                    listName.Enqueue("a0");
                    //根据属性的个数拼自连接
                    for (int j = 0; j < request.AttCondition.Count - 1; j++)
                    {
                        attSql.AppendFormat(" join udoc_enterprise_attribute {0} on a0.itemid={0}.itemid", "a" + (j + 1));
                        listName.Enqueue("a" + (j + 1));
                    }
                    attSql.Append(" where");
                    if (request.AccountID > 0)
                    {
                        attSql.AppendFormat("  a={0} and ",request.AccountID);
                    }
                    int i = 0;
                    int k = 0;
                    foreach (var obj in request.AttCondition)
                    {
                        if (obj.operatortype == operatorEnum.lk)
                        {
                            string name=listName.Dequeue();
                            foreach (var value in obj.Value)
                            {
                                attSql.Append(string.Format("({0}.attribute_class='{0}' and {0}.attribute_value like '%{1}%') or",name ,obj.SearchKey, obj.Value[0]));
                            }
                            attSql.Remove(attSql.Length - 2, 2);
                        }
                        else if (obj.operatortype == operatorEnum.eq)
                        {
                            if (i != 0 || k != 0)
                            {
                                attSql.Append(" and");
                            }
                            string name=listName.Dequeue();
                            foreach (var value in obj.Value)
                            {
                                attSql.Append(string.Format("({0}.attribute_class='{1}' and {0}.attribute_value = '{2}') or",name, obj.SearchKey, value));
                            }
                            attSql.Remove(attSql.Length - 2, 2);
                            i++;
                        }
                        else if (obj.operatortype == operatorEnum.bt)
                        {
                            if (k != 0)
                            {
                                attSql.Append(string.Format(" and ({0}.attribute_class='{1}' and to_number({0}.attribute_value,'999,999') >= '{2}' and to_number({0}.attribute_value,'999,999') <= '{3}') ",listName.Dequeue(), obj.SearchKey, obj.Value[0], obj.Value[1]));
                            }
                            else
                            {
                                if (i != 0)
                                {
                                    attSql.Append(" and");
                                }
                                attSql.Append(string.Format("({0}.attribute_class='{1}' and to_number({0}.attribute_value,'999,999') >= '{2}' and to_number({0}.attribute_value,'999,999') <= '{3}') ",listName.Dequeue(), obj.SearchKey, obj.Value[0], obj.Value[1]));
                            }
                            k++;
                        }
                    }
                    //string sql = attSql.ToString().Substring(0, attSql.Length - 2);
                    //string sqlpage = "";
                    //if (request.page != null && request.page.PageIndex >= 0 && request.page.PageNumber > 0)
                    //{
                    //    sqlpage="limit " + request.page.PageNumber + " offset " + request.page.PageIndex * request.page.PageNumber + ";";
                    //}
                    //else
                    //{
                    //    sqlpage = "limit 5 offset 0;";
                    //}
                    listID = this.Db.Query<long>(attSql.ToString()).ToList();
                }
            }

            #region 搜索关键字

            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                foreach (var obj in request.SearchCondition)
                {
                    if (obj.SeacheKey == SearchEnum.CategoryCode)
                    {
                        SearchConditionsql.Append(string.Format(" and Category_3_c='{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and Category_3_c='{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.ItemorCode)
                    {
                        //平台标准产品代码或名称
                        SearchConditionsql.Append(string.Format(" and ( Standard_c like '%{0}%' or Standard_n like '%{0}%')", obj.Value));
                        CountConditionsql.Append(string.Format(" and ( Standard_c like '%{0}%' or Standard_n like '%{0}%')", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.ItemInfoOrCodeInfo)
                    {
                        //[平台标准产品代码或名称]或[我的产品代码或名称]
                        SearchConditionsql.Append(string.Format(" and ( standard_c like '%{0}%' or standard_n like '%{0}%' or a.c like '%{0}%' or a.n like '%{0}%')", obj.Value));
                        CountConditionsql.Append(string.Format(" and ( standard_c like '%{0}%' or standard_n like '%{0}%' or a.c like '%{0}%' or a.n like '%{0}%')", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.ItemState)
                    {
                        SearchConditionsql.Append(string.Format(" and state={0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and state={0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.Address)
                    {
                        SearchConditionsql.Append(string.Format(" and address like '%{0}%'", obj.Value));
                        CountConditionsql.Append(string.Format(" and address like '%{0}%'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.MaxPrice)
                    {
                        SearchConditionsql.Append(string.Format(" and prc <= {0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and prc <= {0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.MinPrice)
                    {
                        SearchConditionsql.Append(string.Format(" and prc >= {0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and prc >= {0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.BizType)
                    {
                        //业务类型(0不分类 1采购物品 2 供应物品)
                        SearchConditionsql.Append(string.Format(" and biztype = {0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and biztype = {0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.Subtype)
                    {
                        SearchConditionsql.Append(string.Format(" and d.subtype={0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and d.subtype={0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.StandardItemCode)
                    {
                        SearchConditionsql.Append(string.Format(" and standard_c = '{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and standard_c = '{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.StandardItemName)
                    {
                        SearchConditionsql.Append(string.Format(" and standard_n = '{0}'", obj.Value));
                        CountConditionsql.Append(string.Format(" and standard_n = '{0}'", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.IsCertification)
                    {
                        #region 是否认证

                        if (obj.Value == "1" || obj.Value == "0")
                        {
                            //已认证or未认证
                            SearchConditionsql.Append(string.Format(" and iscertification = {0}", obj.Value));
                            CountConditionsql.Append(string.Format(" and iscertification = {0}", obj.Value));
                        }

                        #endregion
                    }
                    else if (obj.SeacheKey == SearchEnum.CategoryThirdID)
                    {
                        SearchConditionsql.Append(string.Format(" and category_3 = {0}", obj.Value));
                        CountConditionsql.Append(string.Format(" and category_3 = {0}", obj.Value));
                    }
                    else if (obj.SeacheKey == SearchEnum.ExceptStandardItemCodeIsNull)
                    {
                        if (obj.Value.ToUpper() == "TRUE")
                        {
                            //去除平台标准产品代码为空的产品(询价发布选择产品使用,导入采购产品时,还没有对应平台产品关系)
                            SearchConditionsql.Append(" and standard_c != ''");
                            CountConditionsql.Append(" and standard_c != ''");
                        }
                    }
                }
            }

            #endregion

            //拼接搜索属性查询结果
            if (listID.Count > 0)
            {
                StringBuilder sqlids = new StringBuilder();
                string str = String.Join(",", listID.Select(x => x.ToString()).ToArray());
                SearchConditionsql.Append(" and a.id in (" + str + ")");
                CountConditionsql.Append(" and a.id in (" + str + ")");
            }
            if (listID.Count == 0 && request.AttCondition != null && request.AttCondition.Count > 0)
            {
                SearchConditionsql.Append(" and a.id =-1");
                CountConditionsql.Append(" and a.id =-1");
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
            long count = this.Db.Query<EnterpriseItemBaseView>(string.Format(SearchConditionsql.ToString())).Count;
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
            var list = this.Db.Query<EnterpriseItemBaseView>(string.Format(SearchConditionsql.ToString()));
            // long count = this.Db.QuerySingle<long>(string.Format(CountConditionsql.ToString()));
            string ids = String.Join(",", list.Select(x => x.ID.ToString()).ToArray());
            string sqlAtt = "select * from udoc_enterprise_attribute where itemid in (" + ids + ")";
            List<EnterpriseItemAttribute> listAtt = new List<EnterpriseItemAttribute>();
            if (list.Count > 0)
            {
                listAtt = this.Db.Query<EnterpriseItemAttribute>(sqlAtt);
            }
            List<ProductList> listModel = new List<ProductList>();

            #region 取该公司所有产品质量认证资料

            StringBuilder sqlCertification = new StringBuilder();
            List<ItemCertificationInfo> certificationInfoAll = null;
            if (request.SearchItemCertificationFlag == "Y")
            {
                //该公司所有产品质量认证资料,去除不合格的
                sqlCertification.Append(string.Format(@"select m.ca, m.certificationname, 
                                                          case when d.results is null then -99 else d.results end as results, c.colorpicid, c.graypicid,
                                                          case when d.results = 1 and m.state = 1 then colorpicid when d.results != 0 then graypicid else -1 end as picid, 
                                                          d.a, d.i, d.id as did, m.state
                                                          from udoc_certification_appdetail as d 
                                                          left join udoc_certification_application as m on m.id = d.mid
                                                          left join udoc_certification as c on c.a = m.ca
                                                          where d.a = {0}", request.AccountID));
                if (request.SearchHasCertificationFlag == "Y")
                {
                    //取认证完成合格的
                    sqlCertification.Append(string.Format(" and d.results = 1 and m.state = 1;"));
                }
                else
                {
                    //去除不合格的
                    sqlCertification.Append(string.Format(" and d.results != 0;"));
                }
                certificationInfoAll = this.Db.Query<ItemCertificationInfo>(sqlCertification.ToString());
            }

            //存该产品认证logo集合
            List<ItemCertificationInfo> certificationInfo = new List<ItemCertificationInfo>();
            Resources cerResource = null;

            #endregion

            foreach (var item in list)
            {
                var Atts = listAtt.Where(x => x.ItemID == item.ID).ToList<EnterpriseItemAttribute>();
                List<ItemAttribute> Attributes = new List<ItemAttribute>();

                //add属性单位 2015/01/14
                foreach (var obj in Atts)
                {
                    Attributes.Add(new ItemAttribute() { PropertyName = obj.AttributeClass, PropertyValue = obj.AttributeValue, UnitName = obj.UnitName });
                }
                ProductList aa = item.TranslateTo<ProductList>();
                aa.CompName = this.Db.Where<NdtechCompany>(n => n.AccountID == item.AccountID).Select(n => n.CompName).FirstOrDefault();

                aa.PropertyList = Attributes;

                if (request.SearchItemCertificationFlag == "Y" && certificationInfoAll != null && certificationInfoAll.Count > 0)
                {
                    #region 取出每个产品的质量认证logo集合

                    certificationInfo = certificationInfoAll.Where(x => x.ItemID == item.ID).ToList();
                    if (certificationInfo.Count > 0)
                    {
                        foreach (var cerInfo in certificationInfo)
                        {
                            //循环取logo图片=>认证公司的colorpicid or graypicid = Resources.id
                            cerResource = this.Db.FirstOrDefault<Resources>(x => x.Id == cerInfo.CertificationPicID);
                            if (cerResource != null)
                            {
                                cerInfo.CertificationResources = ToResources(cerResource, cerResource.AccountID);
                            }
                        }
                    }

                    //质量认证资料赋值给产品
                    aa.ItemCertificationInfo = certificationInfo;

                    #endregion
                }

                listModel.Add(aa);
            }
            if (request.AccountID > 0 && request.ComparePrice != "Y")
            {
                //获取该产品是否已经被收藏
                foreach (var item in listModel)
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

            response.RowsCount = count;

            response.Data = listModel;
            response.Success = true;

            //}
            //catch (Exception ex)
            //{

            //    response.Success = false;
            //}
            return response;
        }

        private ReturnPicResources ToResources(Resources res, int accountID)
        {
            var result = res.TranslateTo<ReturnPicResources>();
            result.FileUrl = string.Format("/fileuploads/{0}", accountID);
            return result;
        }
    }
}
