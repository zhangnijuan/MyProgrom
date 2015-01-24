
using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Subscribe
{

    public class SubscribeValidator : AbstractValidator<SubscribeViewModelRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(SubscribeViewModelRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.FromDataID))
            {
                result.Errors.Add(new ValidationFailure("FromDataID", Const.Err_FromDataIDIsNull, "Err_FromDataIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.AccountID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Subsciber))
            {
                result.Errors.Add(new ValidationFailure("Subsciber", Const.Err_SubsciberIsNull, "Err_SubsciberIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.SubscriberCode))
            {
                result.Errors.Add(new ValidationFailure("SubscriberCode", Const.Err_SubsciberIsNull, "Err_SubsciberIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.SubscriberName))
            {
                result.Errors.Add(new ValidationFailure("SubscriberName", Const.Err_SubsciberIsNull, "Err_SubsciberIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Substate.ToString()))
            {
                result.Errors.Add(new ValidationFailure("Substate", Const.Err_SubstateIsNull, "Err_SubstateIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Subtype.ToString()))
            {
                result.Errors.Add(new ValidationFailure("Subtype", Const.Err_SubtypeIsNull, "Err_SubtypeIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }
    public class SubscribeService : Service, IPost<SubscribeViewModelRequest>
    {
        public IValidator<SubscribeViewModelRequest> SubscribeValidator { get; set; }
        public object Post(SubscribeViewModelRequest request)
        {
            SubscribeResponse response = new SubscribeResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = SubscribeValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            SubscribeAbstract subscribe = null;
            if (request.Subtype == 0) //收藏采购信息
            {
               subscribe =  this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Inquiry");
            }
            else if (request.Subtype == 1) // 收藏产品信息
            {
                subscribe = this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Product");
            }
            else //收藏企业信息
            {
                subscribe = this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Company");
            }
          bool sucess =   subscribe.ReceiveSubscribe(Convert.ToInt64(request.FromDataID), request.Substate, request.Subtype, request.AccountID, Convert.ToInt64(request.Subsciber), request.SubscriberCode, request.SubscriberName);
          response.Success = sucess;
          return response;
        }
    }
}
