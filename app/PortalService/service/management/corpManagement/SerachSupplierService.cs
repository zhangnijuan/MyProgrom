using Ndtech.PortalModel.ViewModel;
using ServiceStack.Common;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalService.Extensions;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.FluentValidation.Internal;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    public class SerachSupplierService : Service
    {
        public IDbConnectionFactory db { set; get; }
        public object Any(SerachSupplierRequest request)
        {
            SerachSupplierResponse response = new SerachSupplierResponse();
            //全网
            StringBuilder sql = new StringBuilder();
            if (request.SupplierEnum == SerachSupplierEnum.All)
            {
                sql.Append("select a.*,b.nature from udoc_comp  a left join udoc_comp_qualification  b on b.a=a.a where a.approved=1 and a.a!=" + request.AccountID);
              
            }
            //已收藏
            if (request.SupplierEnum == SerachSupplierEnum.Favorites)
            {

                sql.Append("select a.*,b.nature from udoc_comp a left join udoc_comp_qualification  b on b.a=a.a join udoc_subscribe_receive c on a.id=c.from_id where c.substate=1 and  a.approved=1 and a.a!=" + request.AccountID + " and c.to_a=" + request.AccountID);
            }
            //已交易
            if (request.SupplierEnum == SerachSupplierEnum.Deal)
            {
                sql.Append("select  a.*,b.nature from udoc_comp a left join udoc_comp_qualification  b on b.a=a.a join pur_order c on a.a=c.sa where a.approved=1 and a.a!=" + request.AccountID + " and c.a=" + request.AccountID);
                //sql.Append("select * from udoc_comp a join udoc_supplier b on a.id=b.comp where a.approved=1 and b.state=1 and a.a!=" + request.AccountID+" and b.a="+ request.AccountID);
            }
            if (request.CompanyName != null)
            {
                sql.AppendFormat(" and a.n like '%{0}%'", request.CompanyName);
            }
            sql.Append(" group by a.id,b.nature order by convert_to(a.n,'gbk')");
            response.RowsCount = this.Db.Query<CompanyInfoReturn>(sql.ToString()).Count;
            request.PageIndex = request.PageIndex == 0 ? 1 : request.PageIndex;
            sql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
            List<CompanyInfoReturn> list = this.Db.Query<CompanyInfoReturn>(sql.ToString());
            response.Success = true;
            response.Data = list;
            return response;
        }


    }
}
