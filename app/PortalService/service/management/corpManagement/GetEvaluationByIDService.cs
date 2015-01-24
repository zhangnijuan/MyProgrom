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

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 生成评价
    /// add by liuzhiqiang 2014/12/25
    /// </summary>
    public class GetEvaluationByIDValidator : AbstractValidator<GetEvaluationByIDRequest>
    {
        public override ValidationResult Validate(GetEvaluationByIDRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //评价单号
            if (instance.ID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_IDIsNull", Const.Err_IDIsNull, "Err_IDIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }

    public class GetEvaluationByIDService : Service, IGet<GetEvaluationByIDRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<GetEvaluationByIDRequest> GetEvaluationByIDValidator { get; set; }
        GetEvaluationByIDResponse response = new GetEvaluationByIDResponse();

        public object Get(GetEvaluationByIDRequest request)
        {
            GetEvaluationByIDResponse response = new GetEvaluationByIDResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = GetEvaluationByIDValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            GetEvaluationByID(request, response);
            return response;
        }

        #region 逻辑处理

        private void GetEvaluationByID(GetEvaluationByIDRequest request, GetEvaluationByIDResponse response)
        {
            //评价主表
            var evaluationInfo = this.Db.FirstOrDefault<EvaluationInfo>("select mm from pur_evaluation where id=" + request.ID);
            //评价题目集合
            evaluationInfo.EvaluationDetail = this.Db.Query<Evaluation>("select b.id,b.n,a.optionmm,optionid from pur_evaluationdetail a join udoc_project b on a.pid=b.id where a.mid=" + request.ID);
            foreach (var item in evaluationInfo.EvaluationDetail)
            {
                //评价选项集合
                item.ProjectOptionList = this.Db.Query<ProjectOption>(string.Format("select id,n from udoc_pro_options where id={0}", item.OptionID));
            }
            response.Data = evaluationInfo;
            response.Success = true;
        }
        #endregion

    }
}
