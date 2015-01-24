using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Auth;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 注册会员
    /// add by yangshuo 2014/12/03
    /// </summary>
    public class RegisteredValidator : AbstractValidator<RegisteredRequest>
    {
        public IDbConnectionFactory db { set; get; }

        #region Validate

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(RegisteredRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.UserName))
            {
                //用户名不能为空
                result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameIsnull, "Err_UserNameIsnull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Password))
            {
                //密码不能为空
                result.Errors.Add(new ValidationFailure("Password", Const.Err_PasswordIsnull, "Err_PasswordIsnull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.CompName))
            {
                //公司名称不能为空
                result.Errors.Add(new ValidationFailure("CompName", Const.Err_CompNameIsNull, "Err_CompNameIsNull"));
                return result;
            }
            
            if (string.IsNullOrEmpty(instance.TelNum))
            {
                //手机号码不能为空
                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNumIsnull, "Err_TelNumIsnull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.TelCode))
            {
                //手机验证码不能为空
                result.Errors.Add(new ValidationFailure("TelCode", Const.Err_TelCodeIsnull, "Err_TelCodeIsnull"));
                return result;
            }

            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    NdtechStaffInfo exsituser = conn.QuerySingle<NdtechStaffInfo>(
                    string.Format("select loginname from udoc_staff where loginname = '{0}';", instance.UserName.Trim()));

                    if (exsituser != null)
                    {
                        //用户名已经存在
                        result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameExisting, "Err_UserNameExisting"));
                        return result;
                    }
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

            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    NdtechTelCode telCode = conn.QuerySingle<NdtechTelCode>(
                  string.Format("select validatecode from gl_telcode where tel = '{0}' and validatecode = '{1}';", instance.TelNum, instance.TelCode));

                    if (telCode == null)
                    {
                        //手机验证码不正确
                        result.Errors.Add(new ValidationFailure("TelCode", Const.Err_TelCode, "Err_TelCode"));
                        return result;
                    }
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

            return base.Validate(instance);
        }

        #endregion
    }

    public class RegisteredService : Service, IPost<RegisteredRequest>
    {
        public IValidator<RegisteredRequest> RegisterValidator { get; set; }

        public object Post(RegisteredRequest request)
        {
            RegisteredResponse response = new RegisteredResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = RegisterValidator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            //第二步:前台model转成dataModel
            var newMember = TranslateEntity(request);

            //第三步:逻辑处理
            Register(newMember, request);

            //第四步:返回response对象
            response.Data = request.TranslateTo<UserInfo>();
            response.Data.CompInfo = newMember.TranslateTo<CompInfo>();
            response.Data.CompInfo.TelNum = request.TelNum;
            response.Data.CompInfo.AccountID = newMember.ID;
            response.Success = true;
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void Register(NdtechAcntSystem newMember, RegisteredRequest request)
        {
            string salt;
            string hashPwd;
            IHashProvider passwordHasher = new SaltedHash();

            //同一个数据库事务下保存到数据库
            using (var trans = Db.BeginTransaction())
            {
                //1.insert公司档案
                NdtechCompany company = new NdtechCompany
                {
                    ID = RecordIDService.GetRecordID(1),
                    AccountID = newMember.ID,
                    CompCode = newMember.CompCode,
                    CompName = newMember.CompName,
                    Createdate = newMember.Createdate,
                    //TelPhone = request.TelNum,

                    //暂时默认为1认证
                    Approved = 1
                };

                Db.Insert(company);

                //2.insert员工档案-系统管理员
                passwordHasher.GetHashAndSaltString(request.Password, out hashPwd, out salt);
                NdtechStaffInfo staff = new NdtechStaffInfo
                {
                    ID = RecordIDService.GetRecordID(1),
                    AccountID = newMember.ID,
                    Salt = salt,
                    SysCode = "S001",
                    SysName = "管理员",
                    UserName = request.UserName,
                    PassWord = hashPwd,
                    Office = "系统管理员",
                    RoleID = 0,
                    Role_Enum = "所有权限",
                    TelNum = request.TelNum,
                    Email = request.EmailInfo,
                    State = 1,
                    State_Enum = "启用",
                    CreateTime = newMember.Createdate
                };
                Db.Insert(staff);

                //3:update帐套档案
                var oldAcntSystem = Db.QuerySingle<NdtechAcntSystem>(string.Format("select * from gl_acntsystems where id = {0};", newMember.ID));
                if (oldAcntSystem != null)
                {
                    oldAcntSystem.CompId = company.ID;
                    oldAcntSystem.CompCode = company.CompCode;
                    oldAcntSystem.CompName = company.CompName;
                    Db.Save(oldAcntSystem);
                }
                var AppData = new AppData()
                {
                    A = newMember.ID,
                    AppKey = Guid.NewGuid().ToString("N"),
                    Secretkey = Guid.NewGuid().ToString("N"),
                    Provide = request.Provide
                };
                Db.Insert(AppData);
                trans.Commit();
            }
        }

        /// <summary>
        /// 前台model转成dataModel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private NdtechAcntSystem TranslateEntity(RegisteredRequest request)
        {
            var newMember = request.TranslateTo<NdtechAcntSystem>();
            newMember.Createdate = DateTime.Now;

            //生成帐套、企业云ID and insert db
            int accountid = -1;
            string corpnum;
            CreateAcntSystem(out accountid, out corpnum, newMember.Createdate);
            newMember.ID = accountid;

            //公司档案code存云ID modify by yangshuo 2015/01/08
            newMember.CompCode = corpnum;
            newMember.CorpNum = corpnum;

            return newMember;
        }

        /// <summary>
        /// 生成帐套档案
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="corpnum"></param>
        /// <param name="dtNow"></param>
        private void CreateAcntSystem(out int accountid, out string corpnum, DateTime dtNow)
        {
            lock (this)
            {
                //从数据库取最后一条记录的帐套和企业ID,然后+1,先保存到帐套表
                //remark：使用聚合函数后需要重新as这个名字,否则赋值给实体类对应不上,获取不到值
                NdtechAcntSystem lastAcntSystem = Db.QuerySingle<NdtechAcntSystem>
                    ("select max(id) as id, max(corpnum) as corpnum from gl_acntsystems;");
                if (lastAcntSystem != null && !string.IsNullOrEmpty(lastAcntSystem.CorpNum))
                {
                    accountid = lastAcntSystem.ID + 1;
                    corpnum = (Convert.ToInt64(lastAcntSystem.CorpNum) + 1).ToString();
                }
                else
                {
                    accountid = 10;
                    corpnum = "200000";
                }

                //insert帐套档案
                NdtechAcntSystem acntSystem = new NdtechAcntSystem
                {
                    ID = accountid,
                    CorpNum = corpnum,
                    CompId = -1,
                    Createdate = dtNow
                };
                Db.Insert(acntSystem);
            }
        }

        private UserInfo ToUserInfo(NdtechStaffCompany userInfo)
        {
            return userInfo.TranslateTo<UserInfo>();
        }

        private CompInfo ToCompInfo(NdtechStaffCompany compInfo)
        {
            return compInfo.TranslateTo<CompInfo>();
        }

        #endregion
    }
}
