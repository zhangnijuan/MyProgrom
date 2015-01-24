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
    /// 根据ID查询评价
    /// add by liuzhiqiang 2015/1/9
    /// </summary>
    public class GetEvaluationByTypeValidator : AbstractValidator<GetEvaluationByTypeRequest>
    {
        public override ValidationResult Validate(GetEvaluationByTypeRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //评价类型
            if (instance.Type == null)
            {
                result.Errors.Add(new ValidationFailure("Err_EvaluationTypeIsNull", Const.Err_EvaluationTypeIsNull, "Err_EvaluationTypeIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }

    public class GetEvaluationByTypeService : Service, IPost<GetEvaluationByTypeRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<GetEvaluationByTypeRequest> GetEvaluationByTypeValidator { get; set; }
        GetEvaluationByTypeResponse response = new GetEvaluationByTypeResponse();

        public object Post(GetEvaluationByTypeRequest request)
        {
            GetEvaluationByTypeResponse response = new GetEvaluationByTypeResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = GetEvaluationByTypeValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            GetEvaluationByType(request, response);
            return response;
        }

        #region 逻辑处理

        private void GetEvaluationByType(GetEvaluationByTypeRequest request, GetEvaluationByTypeResponse response)
        {
            var udocProjectlist = this.Db.Query<Evaluation>("select id,n from udoc_project where type=" + request.Type);
            foreach (var item in udocProjectlist)
            {
                item.ProjectOptionList = this.Db.Query<ProjectOption>(string.Format("select id,n from udoc_pro_options where projectid={0}", item.ProjectID));
            }
            response.Data = udocProjectlist;
            response.Success = true;
        }
        #endregion

    }
}
