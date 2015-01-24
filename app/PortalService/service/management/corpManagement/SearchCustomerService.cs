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

namespace Ndtech.PortalService.Auth
{
    public class SearchCustomerService : Service
    {

        public object Any(SearchCustomerRequest request)
        {
            SearchCustomerResponse response = new SearchCustomerResponse();
            //根据条件拼接SQL语句
            StringBuilder sbSql = new StringBuilder("select u.comp,u.id,u.comp_n,u.comp_c ,q.address,u.state,a.id as a from udoc_customer u join udoc_comp_qualification q on q.mid=u.comp join gl_acntsystems a on a.c=u.comp_c where  u.a=" + request.AccountID);
            if (request.SearchCondition != null)
            {
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchCustomerEnum.CompNameOrCode)
                    {
                        sbSql.AppendFormat(" and (u.comp_n like '%{0}%' or u.comp_c  like '%{0}%')", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchCustomerEnum.Address)
                    {
                        sbSql.AppendFormat(" and (q.address like '%{0}%')", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchCustomerEnum.State)
                    {
                        sbSql.AppendFormat(" and (u.state= {0})", item.Value[0]);
                    }
                }
            }

            sbSql.AppendFormat(" order by id desc");
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            //查询
            response.RowsCount = this.Db.Query<DealSupplierInfo>(sbSql.ToString()).Count;
            sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
            var data = this.Db.Query<CustomerInfo>(sbSql.ToString());
            //查询主营产品
            foreach (var item in data)
            {
                item.MainProduct = this.Db.Query<MainProducts>(string.Format("select n from udoc_comp_category where mid={0}", item.CompID));
            }
            response.Data = data;
            response.Success = true;
            return response;
        }
    }
}
