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
    /// <summary>
    /// 采购申请生成订单
    /// add by liuzhiqiang 2015-01-17
    /// </summary>
    public class GetPlanDetailByIDValidator : AbstractValidator<GetPlanDetailByIDRequest>
    {
        public override ValidationResult Validate(GetPlanDetailByIDRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //至少有一条产品数据
            if (instance.Condition==null)
            {
                result.Errors.Add(new ValidationFailure("PurItemList", Const.Err_PurItemListIsNull, "Err_PurItemListIsNull"));
                return result;
            }

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class GetPlanDetailByIDService : Service, IPost<GetPlanDetailByIDRequest>
    {
        public IValidator<GetPlanDetailByIDRequest> GetPlanDetailByIDValidator { get; set; }


        public object Post(GetPlanDetailByIDRequest request)
        {

            GetPlanDetailByIDResponse response = new GetPlanDetailByIDResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = GetPlanDetailByIDValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            List<string> itemIDs = new List<string>();
            foreach (var item in request.Condition)
            {
                itemIDs.Add(item.ID);
            }
            //根据条件拼接SQL语句

            var data = this.Db.Query<PurPlanDetail>("select a.pland_id,a.pland_i,a.pland_i_c,a.pland_i_n,a.pland_standarditemcode,a.pland_standarditemname,a.pland_categorythirdid,a.pland_categorythirdcode,a.pland_categorythirdname,a.pland_propertyname,a.pland_remark,a.pland_u_n,a.pland_mm,a.pland_qty,b.pid as pland_categorythirpid,a.pland_requirdate from pur_plandetail a join udoc_enterprise_Category b on a.pland_categorythirdid=b.id where pland_id in (" + itemIDs.SqlInValues().ToSqlInString() + ")");
            List<PlanDetail> listModel = new List<PlanDetail>();
            foreach (var item in data)
            {
                listModel.Add(item.TranslateTo<PlanDetail>());
            }
            response.Data = listModel;
            response.PlanID = request.PlanID;
            response.Success = true;
            return response;
        }
    }
}
