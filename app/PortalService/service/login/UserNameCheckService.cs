using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 注册时校验会员登录名唯一
    /// </summary>
    public class UserNameCheckValidator : AbstractValidator<UserNameCheckRequest>
    {

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(UserNameCheckRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.UserName))
            {
                //用户名不能为空
                result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameIsnull, "Err_UserNameIsnull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class UserNameCheckService : Service,IGet<UserNameCheckRequest>
    {
        public IValidator<UserNameCheckRequest> UserNameValidator { get; set; }

        public object Get(UserNameCheckRequest request)
        {
            UserNameCheckResponse response = new UserNameCheckResponse();

            //第一步：校验用户名是否存在
            ValidationResult result = UserNameValidator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
 
                NdtechStaffInfo exsituser = this.Db.QuerySingle<NdtechStaffInfo>(
                    string.Format("select loginname from udoc_staff where loginname = '{0}';", request.UserName));

                if (exsituser != null)
                {
                    response.IsExist = true;
                }
                else
                {
                    response.IsExist = false;
                }
            response.Success = true;
            return response;
        }
    }
}
