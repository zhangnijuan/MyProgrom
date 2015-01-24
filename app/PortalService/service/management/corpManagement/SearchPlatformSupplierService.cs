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

    public class SearchPlatformSupplierValidator : AbstractValidator<SearchPlatformSupplierRequest>
    {
        public override ValidationResult Validate(SearchPlatformSupplierRequest instance)
        {

            ValidationResult result = new ValidationResult();
            SQLInjectionFilter.CheckOutList<SearchSupplierField>(instance.SearchCondition, result);
            return result;
        }
    }


    public class SearchPlatformSupplierService : Service
    {

        public object Any(SearchPlatformSupplierRequest request)
        {

            SearchPlatformSupplierValidator asvalidator = new SearchPlatformSupplierValidator();
            ValidationResult result = asvalidator.Validate(request);
            SearchPlatformSupplierResponse response = new SearchPlatformSupplierResponse();
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }


            //根据条件拼接SQL语句
            StringBuilder sbSql = new StringBuilder("select u.id,u.a,g.corpnum,u.n,u.scale,u.releases,(select count(1)  from pur_inquiry  where a=u.a and State=1 and AnonymousCode=0) as inquirynumber from udoc_comp u join gl_acntsystems g on u.a=g.id left join udoc_comp_qualification q on u.id=q.mid left join udoc_comp_category c on c.a=u.a where u.approved=1 ");
            if (request.AccountID > 0)
            {
                sbSql.AppendFormat(" and u.a!={0}", request.AccountID);
            }
            if (request.EnterpriseType == EnterpriseEnum.Supplier)
            {
                sbSql.AppendFormat(" and u.releases>0");
            }
            else
            {

                sbSql.AppendFormat(" and u.inquirynumber>0");
            }
            if (request.SearchCondition != null)
            {
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchSupplierEnum.CompanyScale)
                    {
                        sbSql.AppendFormat(" and u.scale='{0}'", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSupplierEnum.CompNameOrProductOrCorpnum)
                    {
                        sbSql.AppendFormat(" and (u.n like '%{0}%' or c.n like '%{0}%' or g.corpnum like '%{0}%')", item.Value[0]);
                    }
                    if (item.SeacheKey == SearchSupplierEnum.CompNature)
                    {
                        sbSql.Append(" and (");
                        foreach (var va in item.Value)
                        {
                            sbSql.AppendFormat(" q.nature='{0}' or", va);
                        }
                        sbSql.Remove(sbSql.Length - 2, 2);
                        sbSql.Append(")");
                    }
                    if (item.SeacheKey == SearchSupplierEnum.MainProduct)
                    {
                        sbSql.AppendFormat(" and (c.n like '%{0}%')", item.Value[0]);
                    }
                }
            }
            sbSql.Append(" group by u.id,u.a,g.corpnum,u.n,u.scale,u.releases");
            if (request.Order == "desc")
            {
                sbSql.AppendFormat(" order by u.favorites {0}", request.Order);
            }
            else
            {


                sbSql.AppendFormat(" order by u.favorites");
            }
            //查询
            response.RowsCount = this.Db.Query<SupplierInfo>(sbSql.ToString()).Count;
            request.PageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
            sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
            var data = this.Db.Query<SupplierInfo>(sbSql.ToString());
            //查询主营产品
            foreach (var item in data)
            {
                item.MainProduct = this.Db.Query<MainProducts>(string.Format("select n from udoc_comp_category where mid={0}", item.ID));
                var subRec = this.Db.FirstOrDefault<SubscribeReceive>(s => s.FromID == item.ID);
                if (subRec != null)
                {
                    item.SubState = subRec.Substate;
                }
                else
                {
                    item.SubState = 0;
                }
            }
         
            response.Data = data;
            response.Success = true;
            return response;
        }
    }
}
