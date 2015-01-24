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
    public class SearchPurEvaluationService : Service
    {

        public object Any(SearchPurEvaluationRequest request)
        {
            SearchPurEvaluationResponse response = new SearchPurEvaluationResponse();
            //根据条件拼接SQL语句
            StringBuilder sbSql = new StringBuilder("select u.id,u.c,u.ename,u.createtime ,q.c as ordercode,sum(o.level)  as level from pur_evaluation u join pur_order q on u.mid=q.id join pur_evaluationdetail p on p.mid=u.id join udoc_pro_options o on o.id=p.optionID  where 1=1 ");
            if (request.IdentityType == 1 && request.EvaluationType == 1)//身份类型 1 采购商 评价类型 1 登录人对对方
            {
                 sbSql.Append(string.Format("  and u.a = {0} and q.a={0}", request.AccountID));
            }
            else if(request.IdentityType == 1 && request.EvaluationType ==2)//身份类型 1 采购商 评价类型 2 对方对登录人
            {
                 sbSql.Append(string.Format("  and u.a = {0} and q.a={1}", request.OtherAccountID,request.AccountID));
            }
            else if(request.IdentityType == 2 && request.EvaluationType ==2)//身份类型 2 供应商 评价类型 1 登录人对对方
            {
                 sbSql.Append(string.Format("  and u.a = {0} and q.a={1}", request.AccountID,request.OtherAccountID));
            }
            else if(request.IdentityType == 1 && request.EvaluationType ==2)//身份类型 2 供应商 评价类型 2 对方对登录人
            {
                 sbSql.Append(string.Format("  and u.a = {0} and q.a={0}", request.OtherAccountID));
            }

                if (request.SearchCondition != null)
            {
                foreach (var item in request.SearchCondition)
                {
                    if (item.SeacheKey == SearchPurEvaluationEnum.BeginDateTime)
                    {
                        sbSql.Append(string.Format(" and u.createtime >= '{0}'", item.Value));
                    }
                    else if (item.SeacheKey == SearchPurEvaluationEnum.EndDateTime)
                    {
                        sbSql.Append(string.Format(" and u.createtime <= '{0}'", item.Value));
                    }
                    else if (item.SeacheKey == SearchPurEvaluationEnum.OrderCodeOrEvaluationCode)
                    {
                        sbSql.AppendFormat(" and (q.c like '%{0}%' or u.c like '%{0}%')", item.Value);
                    }
                   
                }
            }

            sbSql.AppendFormat(" and q.su_c='" + request.CorpNum + "' group by u.id,u.c,u.ename,u.createtime ,q.c order by id desc");
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            //查询
            response.RowsCount = this.Db.Query<PurEvaluationInfo>(sbSql.ToString()).Count;
            sbSql.AppendFormat(" limit {0} offset {1}", request.PageSize, (request.PageIndex - 1) * request.PageSize);
            var data = this.Db.Query<PurEvaluationInfo>(sbSql.ToString());
            response.Data = data;
            response.Success = true;
            return response;
        }
    }
}
