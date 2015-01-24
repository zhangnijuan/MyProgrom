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
using ServiceStack.ServiceInterface.Auth;
namespace Ndtech.PortalService.Auth
{
    public class PossWordValidator : AbstractValidator<RestPossWordRequest>
    {
        //public PossWordValidator()
        //{
        //    RuleFor(p => p.PassWord).Length(6, 16).WithMessage(Const.Err_PassWordTooLongOrShort);
        //}
        public override ValidationResult Validate(RestPossWordRequest instance)
        {

            ValidationResult result = new ValidationResult();

            if (instance.PassWord.Length > 16)
            {
                result.Errors.Add(new ValidationFailure("PassWord", Const.Err_PassWordTooLongOrShort, "Err_PassWordTooLongOrShort"));
                return result;
            }
            return base.Validate(instance);
        }
    
    }

    public class RestPossWordService:Service,IPost<RestPossWordRequest>
    {

        public IValidator<RestPossWordRequest> PossWordValidator { get; set; }
        public object Post(RestPossWordRequest request)
        {
            //验证数据
            RestPossWordResponse response = new RestPossWordResponse();
            ValidationResult result = PossWordValidator.Validate(request);
            if (!result.IsValid)
            {
                response.IsSucess = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
           
            var oldEmployeeInfo = this.Db.FirstOrDefault<NdtechStaffInfo>(s => s.ID == request.Id);
            if (oldEmployeeInfo!=null)
            {
                //对密码进行加密
                string salt;
                string hashPwd;
                IHashProvider passwordHasher = new SaltedHash();
                passwordHasher.GetHashAndSaltString(request.PassWord, out hashPwd, out salt);
                oldEmployeeInfo.Salt = salt;
                oldEmployeeInfo.PassWord = hashPwd;
                this.Db.Save<NdtechStaffInfo>(oldEmployeeInfo);
                response.IsSucess = true;
            }
            else
            {
                response.IsSucess = false;
                response.ResponseStatus.Message = "修改失败";
            }

            return response;
        }
    }
}
