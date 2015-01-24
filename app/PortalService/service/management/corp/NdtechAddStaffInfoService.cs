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
    public class AddStaffValidator : AbstractValidator<NdtechAddStaffInfoRequest>
    {
        public IDbConnectionFactory db { set; get; }

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(NdtechAddStaffInfoRequest instance)
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
            if (string.IsNullOrEmpty(instance.PassWord))
            {
                result.Errors.Add(new ValidationFailure("PassWord", Const.Err_PasswordIsnull, "Err_PasswordIsnull"));
                return result;
            }
            if (instance.PassWord.Length > 16)
            {
                result.Errors.Add(new ValidationFailure("PassWord", Const.Err_PassWordTooLongOrShort, "Err_PassWordTooLongOrShort"));
                return result;
            }
            if ("正则表达式" == instance.PassWord)
            {
                result.Errors.Add(new ValidationFailure("PassWord", Const.Err_PassWordIsBroken, "Err_PassWordIsBroken"));
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
                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNumIsnull, "Err_TelNumIsnull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Email))
            {
                result.Errors.Add(new ValidationFailure("Email", Const.Err_EmailIsNull, "Err_EmailIsNull"));
                return result;
            }

            using (var conn = db.OpenDbConnection())
            {
                NdtechAddStaffInfoRequest exsituser = conn.QuerySingle<NdtechAddStaffInfoRequest>(string.Format("select n from udoc_staff where loginname = '{0}'",  instance.UserName.Trim()));

                if (exsituser != null)
                {
                    result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameExisting, "Err_UserNameExisting"));
                    return result;
                }

                exsituser = conn.QuerySingle<NdtechAddStaffInfoRequest>(string.Format("select tel from udoc_staff where a = {0} and tel = '{1}'", 0, instance.TelNum));

                if (exsituser != null)
                {
                    result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNumExisting, "Err_TelNumExisting"));
                    return result;
                }

                exsituser = conn.QuerySingle<NdtechAddStaffInfoRequest>(string.Format("select email from udoc_staff where a = {0} and email = '{1}'", 0, instance.Email));

                if (exsituser != null)
                {
                    result.Errors.Add(new ValidationFailure("Email", Const.Err_EmailExisting, "Err_EmailExisting"));
                    return result;
                }
            }

            return base.Validate(instance);
        }
    }
    public class AddStaffService : Service, IPost<NdtechAddStaffInfoRequest>
    {
        public IValidator<NdtechAddStaffInfoRequest> asvalidator { get; set; }
        public object Post(NdtechAddStaffInfoRequest request)
        {

            //第一步:校验前端的数据合法性
            ValidationResult result = asvalidator.Validate(request);
            NdtechAddStaffResponse response = new NdtechAddStaffResponse();
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            //第二步:前台model转成dataModel
            //开启事物插入2张表中
            using (var trans=this.Db.BeginTransaction())
            {
                //生成员工表实体
                var newMember = TranslateEntity(request);
                string salt;
                string hashPwd;
                IHashProvider passwordHasher = new SaltedHash();
                passwordHasher.GetHashAndSaltString(request.PassWord, out hashPwd, out salt);
                newMember.Salt = salt;
                newMember.PassWord = hashPwd;
                //查出当前管理员信息
                var system = this.Db.Where<NdtechStaffInfo>(n => n.AccountID == request.AccountID && n.SysCode=="S001").OrderBy(n=>n.ID).FirstNonDefault();
                if (system!=null)
                {
                    newMember.Eid = system.ID;
                    newMember.EidName = system.SysName;
                    newMember.EidCode = system.SysCode;
                }
                
                this.Db.Insert(newMember);
                //生成资源表实体
                Resources resource = TranslateEntity(request.PicResources);
                if (resource!=null)
                {
                    resource.Id = RecordIDService.GetRecordID(1);
                    resource.AccountID = request.AccountID;
                    resource.DocumentID = newMember.ID;
                    this.Db.Insert(resource);
                }
                
                trans.Commit();
            }
            
            //第四步:返回response对象
            response.ResponseStatus.ErrorCode = "true";
            response.ResponseStatus.Message = "新增成功";
            response.Success = true;
            return response;

        }
        private object Authenticate(NdtechAddStaffInfoRequest request, string provider, INdtechAuthSession session, INdtechAuthProvider oAuthConfig)
        {
            NdtechAddStaffInfoRequest response = null;
            //if (!oAuthConfig.IsAuthorized(session, request))
            //{
            //    response = oAuthConfig.Authenticate(this, session, request);
            //}
            return response;
        }




        /// <summary>
        /// 前后台模型转化，补充默认值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private NdtechStaffInfo TranslateEntity(NdtechAddStaffInfoRequest request)
        {
            //自动转换同名的属性
            var newMember = request.TranslateTo<NdtechStaffInfo>();
            //获取ID
            newMember.ID = RecordIDService.GetRecordID(1);

            newMember.CreateTime = DateTime.Now;
            //返回对象
            return newMember;
        }
        private Resources TranslateEntity(PicResources request)
        {
            //自动转换同名的属性
            return request.TranslateTo<Resources>();
          
        }
    }
}
