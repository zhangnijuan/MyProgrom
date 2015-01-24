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
using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;

namespace Ndtech.PortalService.Auth
{
    public class UpdateStaffValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public IDbConnectionFactory db { set; get; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(UpdateEmployeeRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(instance.SysName))
            {
                result.Errors.Add(new ValidationFailure("SysName", Const.Err_SysNameIsNull, "Err_SysNameIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.SysCode))
            {
                result.Errors.Add(new ValidationFailure("SysCode", Const.Err_SysCodeIsNull, "Err_SysCodeIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.UserName))
            {
                result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameIsnull, "Err_UserNameIsnull"));
                return result;
            }
            
            //if (string.IsNullOrEmpty(instance.Office))
            //{
            //    result.Errors.Add(new ValidationFailure("Office", Const.Err_OfficeIsNull, "Err_OfficeIsNull"));
            //    return result;
            //}
        
            if (string.IsNullOrEmpty(instance.Role_Enum))
            {
                result.Errors.Add(new ValidationFailure("Role_Enum", Const.Err_RoleIdIsNull, "Err_RoleIdIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.TelNum))
            {
                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelCodeIsnull, "Err_TelCodeIsnull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", Const.Err_EmailIsNull, "Err_EmailIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }
    public class UpdateEmployee : Service, IPost<UpdateEmployeeRequest>
    {
        public object Post(UpdateEmployeeRequest request)
        {
            //验证数据
            UpdateStaffValidator asvalidator = new UpdateStaffValidator();
            ValidationResult result = asvalidator.Validate(request);
            UpdateEmployeeResponse response = new UpdateEmployeeResponse();
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //转换model
            NdtechStaffInfo taffInfo = ToEntity(request);
            var oldTaffInfo = this.Db.FirstOrDefault<NdtechStaffInfo>(n => n.ID == request.ID);
            taffInfo.CreateTime = oldTaffInfo.CreateTime;
            taffInfo.AccountID = oldTaffInfo.AccountID;
            taffInfo.Salt = oldTaffInfo.Salt;
            taffInfo.PassWord = oldTaffInfo.PassWord;
            taffInfo.Eid = oldTaffInfo.Eid;
            taffInfo.EidCode = oldTaffInfo.EidCode;
            taffInfo.EidName = oldTaffInfo.EidName;
            this.Db.Save<NdtechStaffInfo>(taffInfo);
            
            Resources resources = ToEntity(request.PicResources);
            if (resources!=null)
            {
                var data = this.Db.FirstOrDefault<Resources>(r => r.Id == resources.Id);
                if (data!=null)
                {
                    this.Db.Save<Resources>(resources);
                }
                else
                {
                    resources.Id = RecordIDService.GetRecordID(1);
                    resources.AccountID = taffInfo.AccountID;
                    resources.DocumentID = taffInfo.ID;
                    this.Db.Insert(resources);
                }
               
            }
           
            response.Success = true;
            return response;

        }

      
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private NdtechStaffInfo ToEntity(UpdateEmployeeRequest request)
        {
            return request.TranslateTo<NdtechStaffInfo>();
        }
        private Resources ToEntity(UpdatePicResources picResources)
        {
            return picResources.TranslateTo<Resources>();
        }

    }
}

