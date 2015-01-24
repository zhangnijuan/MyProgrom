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

namespace Ndtech.PortalService.Auth
{
    public class RetrievePasswordService : Service, IPost<RetrievePwdRequest>
    {
        #region IPost<RegisteredRequest> 成员
        public IValidator<RetrievePwdRequest> RetrievePwdValidator { get; set; }
        public IDbConnectionFactory db { set; get; }
        public object Post(RetrievePwdRequest request)
        {
            RetrievePwdResponse response = new RetrievePwdResponse();
            //验证手机验证码
            bool flag = ValidateTelCode(request);
            //保存
            if (flag)
            {
                int i = SavePassWord(request);
                if (i > 0)
                {
                    response.Success = true;
                }
            }
            return response;
        }
        private bool ValidateTelCode(RetrievePwdRequest request)
        {
            string TelCode = request.TelCode;
            string UserName = request.UserName;
            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    NdtechStaffInfo exsituser = conn.QuerySingle<NdtechStaffInfo>(string.Format("select * from udoc_staff where loginname = '{0}';", UserName));
                    if (exsituser != null)
                    {
                        string Phone = exsituser.TelNum;
                        NdtechTelCode exsittelcode = conn.QuerySingle<NdtechTelCode>(string.Format("select * from gl_telcode where validatecode = '{0}' and tel='{1}'", TelCode, Phone));
                        if (exsittelcode != null)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                catch (Exception)
                {
                    
                    throw;
                }
                finally
                {
                    conn.Close();
                }  
            }

        }
        private int SavePassWord(RetrievePwdRequest request)
        {
            string salt;
            string hashPwd;
            IHashProvider passwordHasher = new SaltedHash();
            passwordHasher.GetHashAndSaltString(request.PassWord, out hashPwd, out salt);
            using (var trans = Db.BeginTransaction())
            {
                using (var conn = db.OpenDbConnection())
                {
                    try
                    {
                        int i = conn.ExecuteNonQuery(string.Format("UPDATE UDOC_STAFF SET loginpwd='{0}',salt='{1}' WHERE loginname='{2}'",
                      hashPwd, salt, request.UserName));
                        if (i > 0)
                            trans.Commit();
                        else
                            trans.Rollback();
                        return i;
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }  

                }

            }


        }
        #endregion
    }
    public class RetrievePasswordValidator : AbstractValidator<RetrievePwdRequest>
    {
        /// <summary>
        /// 验证传过来的数据
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(RetrievePwdRequest instance)
        {

            return base.Validate(instance);
        }
    }
}
