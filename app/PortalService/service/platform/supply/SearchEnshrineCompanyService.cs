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
using Ndtech.PortalModel.ViewModel.management.corpManagement;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.service.platform.supply
{
    public class SearchEnshrineCompanyService : Service, IPost<SearchEnshrineCompanyRequest>
    {
        public object Post(SearchEnshrineCompanyRequest request)
        {
            SearchEnshrineCompanyResponse response = new SearchEnshrineCompanyResponse();
            StringBuilder sbSql = new StringBuilder("select c.id,c.a,g.corpnum,c.n,c.scale,c.releases,c.inquirynumber,a.subdatetime from udoc_Subscribe_receive a join udoc_Subscribe_contact b on a.to_a=b.to_a  join udoc_comp c on a.from_a=c.a left join udoc_comp_qualification d on c.id=d.mid join gl_acntsystems g on c.a=g.id where a.substate=1");
            #region 采购商
            //采购商
            if (request.CounterParty == EnterpriseEnum.Purchaser)
            {
                sbSql.AppendFormat(" and a.subtype=2 and b.subscriber={0}", request.ID);
                if (request.SearchCondition != null)
                {
                    foreach (var item in request.SearchCondition)
                    {
                        if (item.SeacheKey == SearchEnum.Address)
                        {
                            sbSql.AppendFormat(" and d.address like '{0}'", item.Value);
                        }
                        if (item.SeacheKey == SearchEnum.ItemorCode)
                        {
                            sbSql.AppendFormat(" and (c.n like '%{0}%' or  g.corpnum like '%{0}%' )", item.Value);
                        }
                    }
                }
                sbSql.AppendFormat(" group by c.id,c.a,g.corpnum,c.n,c.scale,c.releases,c.inquirynumber,a.subdatetime");
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

                response.RowsCount = this.Db.Query<SupplierInfo>(sbSql.ToString()).Count;
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
                var data = this.Db.Query<SupplierInfo>(sbSql.ToString());
                response.Data = data;
            } 
            #endregion
               
            
            else//供应商
            {
                sbSql = new StringBuilder("select c.id,c.a,g.corpnum,c.n,c.scale,c.releases,c.inquirynumber,a.subdatetime from udoc_Subscribe_receive a join udoc_Subscribe_contact b on a.to_a=b.to_a  join udoc_comp c on a.from_a=c.a left join udoc_comp_qualification d on c.id=d.mid join gl_acntsystems g on c.a=g.id left join udoc_comp_category e on e.mid=c.id where a.substate=1");
                sbSql.AppendFormat(" and a.subtype=3 and b.subscriber={0}", request.ID);
                if (request.SearchCondition != null)
                {
                    foreach (var item in request.SearchCondition)
                    {
                        if (item.SeacheKey == SearchEnum.Address)
                        {
                            sbSql.AppendFormat(" and d.address like '{0}'", item.Value);
                        }
                        if (item.SeacheKey == SearchEnum.ItemorCode)
                        {
                            sbSql.AppendFormat(" and (c.n like '%{0}%' or g.corpnum like  '%{0}%' or e.n like '%{0}%')", item.Value);
                        }
                    }
                }
                sbSql.AppendFormat(" group by c.id,c.a,g.corpnum,c.n,c.scale,c.releases,c.inquirynumber,a.subdatetime");
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
                response.RowsCount = this.Db.Query<SupplierInfo>(sbSql.ToString()).Count;
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
                var data = this.Db.Query<SupplierInfo>(sbSql.ToString());
                //获得主营产品
                foreach (var item in data)
                {
                    item.MainProduct = this.Db.Query<MainProducts>(string.Format("select n from udoc_comp_category where mid={0}", item.ID));
                }
                response.Data = data;
            }

            response.Success = true;
            
            return response;
        }
    }
}
