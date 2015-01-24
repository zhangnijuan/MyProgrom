using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;


namespace Ndtech.PortalService.service.management.corpManagement
{
    public class ModifyCustomerStateValidator : AbstractValidator<ModifyCustomerStateRequest>
    {
        public override ValidationResult Validate(ModifyCustomerStateRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //ID
            if (instance.ID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_IDIsNull", Const.Err_IDIsNull, "Err_IDIsNull"));
                return result;
            }

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            //状态
            if (instance.State == null)
            {
                result.Errors.Add(new ValidationFailure("Err_StateIsNull", Const.Err_StateIsNull, "Err_StateIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }
    public class ModifyCustomerStateService : Service, IPost<ModifyCustomerStateRequest>
    {
        public IValidator<ModifyCustomerStateRequest> ModifyCustomerStateValidator { get; set; }
        public object Post(ModifyCustomerStateRequest request)
        {
            ModifyCustomerStateResponse response = new ModifyCustomerStateResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = ModifyCustomerStateValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            this.Db.Update<UdocCustomer>(string.Format("State = {0} where id = {1}", request.State, request.ID));
            response.Success = true;
            return response;
        }

    }
}
