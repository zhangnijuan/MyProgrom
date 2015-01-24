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

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 保存报价
    /// add by liuzhiqiang 2014/12/18
    /// </summary>
    public class GetSalQuotationStateValidator : AbstractValidator<GetSalQuotationStateRequest>
    {
        public override ValidationResult Validate(GetSalQuotationStateRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //帐套
            if (instance.AccountId == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            //询价编号
            if (string.IsNullOrEmpty(instance.InquiryCode.ToString()))
            {
                result.Errors.Add(new ValidationFailure("Err_InquiryCodeIsNull", Const.Err_InquiryCodeIsNull, "Err_InquiryCodeIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class GetSalQuotationStateService : Service, IPost<GetSalQuotationStateRequest>
    {
        public IValidator<GetSalQuotationStateRequest> GetSalQuotationStateValidator { get; set; }
        GetSalQuotationStateResponse response = new GetSalQuotationStateResponse();

        public object Post(GetSalQuotationStateRequest request)
        {
            GetSalQuotationStateResponse response = new GetSalQuotationStateResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = GetSalQuotationStateValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:逻辑处理
            InsertGetSalQuotationState(request, response);
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 是否已报价
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void InsertGetSalQuotationState(GetSalQuotationStateRequest request, GetSalQuotationStateResponse response)
        {
            var salQuotation = this.Db.FirstOrDefault<SalQuotation>(n => n.AccountID == request.AccountId && n.InquiryCode == request.InquiryCode && n.State ==1);
            if (salQuotation == null)//没有报价
            {
                response.Success = true;
            }
        }

        #endregion
    }
}
