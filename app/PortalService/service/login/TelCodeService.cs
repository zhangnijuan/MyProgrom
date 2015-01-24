using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;
using Ndtech.PortalModel.ViewModel;
using System.Text;
using System.Collections.Generic;
using ServiceStack.Common;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using Ndtech.PortalService.SMS;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 生成手机验证码
    /// add by yangshuo 2014/12/03
    /// </summary>
    public class TelCodeValidator : AbstractValidator<TelCodeRequest>
    {
        public IDbConnectionFactory db { set; get; }

        #region Validate

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(TelCodeRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (instance.Flag != "FindPassWord" && string.IsNullOrEmpty(instance.UserName))
            {
                result.Errors.Add(new ValidationFailure("UserName", Const.Err_UserNameIsnull, "Err_UserNameIsnull"));
                return result;
            }
            //1.手机号码不能为空
            if (instance.Flag != "FindPassWord" && string.IsNullOrEmpty(instance.TelNum))
            {
                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNumIsnull, "Err_TelNumIsnull"));
                return result;
            }
            if (!string.IsNullOrEmpty(instance.UserName) && instance.Flag == "FindPassWord")
            {
                //2.找回密码时校验登录名和绑定手机号码一致
                using (var conn = db.OpenDbConnection())
                {
                    try
                    {
                        string existTelNum = conn.QuerySingle<string>(string.Format("select tel from udoc_staff where  loginname = '{0}';",
                       instance.UserName));

                        if (string.IsNullOrEmpty(existTelNum))
                        {
                            //登录名和绑定的手机号码不一致
                            result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNum, "Err_TelNum"));
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
            }
            else if (instance.Flag == "Register")
            {
                if (instance.UserName.IndexOf("ndtechtest_") == -1)
                {
                    //2.注册时校验手机号码唯一性
                    using (var conn = db.OpenDbConnection())
                    {
                        try
                        {
                            string existTelNum = conn.QuerySingle<string>(string.Format("select tel from udoc_staff where tel = '{0}';", instance.TelNum));

                            if (!string.IsNullOrEmpty(existTelNum))
                            {
                                //该手机号码已经被绑定
                                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_TelNumExisting, "Err_TelNumExisting"));
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
                }
            }
            else
            {
                result.Errors.Add(new ValidationFailure("TelNum", Const.Err_Flag, "Err_Flag"));
                return result;
            }
            return base.Validate(instance);
        }

        #endregion
    }

    public class TelCodeService : Service, IPost<TelCodeRequest>, IGet<TelCodeRequest>
    {
        public IValidator<TelCodeRequest> TelCodeValidator { get; set; }

        //发短信接口
        public ISendMessage sendMessage { get; set; }

        public IDbConnectionFactory db { set; get; }

        public object Post(TelCodeRequest request)
        {
            TelCodeResponse response = new TelCodeResponse();
                //第一步:校验前端的数据合法性
            ValidationResult result = TelCodeValidator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                response.Success = false;
                return response;
            }
              
            //第二步:生成6位随机验证码
            string randnum = GetRandomString("0", 6);

            if (!string.IsNullOrEmpty(randnum))
            {
                //第三步:逻辑处理
                TelCodeHandle(randnum, request);

                //第四步:返回response对象
                response.TelCode = randnum;
                response.TelNumber = request.TelNum;
                response.Success = true;
            }
            else
            {
                //生成手机验证码错误
                response.ResponseStatus.ErrorCode = "Err_TelCodeRandNum";
                response.ResponseStatus.Message = Const.Err_TelCodeRandNum;
            }
            return response;
        }

        #region 逻辑处理

        private string GetTelNumByUsername(string UserName)
        {
            using (var conn = db.OpenDbConnection())
            {
                NdtechStaffInfo exsituser = conn.QuerySingle<NdtechStaffInfo>(string.Format("select * from udoc_staff where loginname = '{0}';", UserName));
                if (exsituser != null)
                {
                    return exsituser.TelNum;
                }
                return null;
            }
        }
        private void TelCodeHandle(string randnum, TelCodeRequest request)
        {
            string sql = string.Format("select * from gl_telcode where tel = '{0}';", request.TelNum);
            if (request.Flag == "FindPassWord")
            {
                string tel =  Db.QuerySingle<string>(string.Format("select tel from udoc_staff where loginname = '{0}'",request.UserName));
                if (request.Flag == "FindPassWord")
                {
                    request.TelNum = tel;
                }
                sql = string.Format("select * from gl_telcode where tel = '{0}';", tel);
            }
            //1.先查询资料是否存在
            var oldTelCode = Db.QuerySingle<NdtechTelCode>(sql);
            if (oldTelCode == null)
            {
                //1.1 不存在->add
                var newTelCode = request.TranslateTo<NdtechTelCode>();
                newTelCode.ID = RecordIDService.GetRecordID(1);
                newTelCode.TelCode = randnum;
                newTelCode.Createdate = DateTime.Now;
                Db.Insert(newTelCode);
            }
            else
            {
                //1.2 存在->update
                oldTelCode.TelCode = randnum;
                oldTelCode.Createdate = DateTime.Now;
                Db.Save(oldTelCode);
            }

            //sendMessage.SendSms(request.TelNum, "欢迎您注册账号，验证码" + randnum);
        }

        /// <summary>
        /// 生成随机验证码(数字，大小写字母)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string GetRandomString(string type, int length)
        {
            //生成一个随机密码{[type:0:包含数字],[1:包含小写字母],[2:包含大写字母]}
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }
            Dictionary<string, string> source = new Dictionary<string, string>();
            source.Add("0", "0123456789");
            source.Add("1", "abcdefghigklmnopkrstuvwxyz");
            source.Add("2", "ABCDEFGHIGKLMNOPQRSTUVWXYZ");

            StringBuilder currSourceBuilder = new StringBuilder();
            if (type.Equals("0"))
            {
                currSourceBuilder.Append(source["0"]);
            }
            if (type.Equals("1"))
            {
                currSourceBuilder.Append(source["1"]);
            }
            if (type.Equals("2"))
            {
                currSourceBuilder.Append(source["2"]);
            }

            string currSource = currSourceBuilder.ToString();
            Random random = new Random();
            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int number = random.Next(currSource.Length);
                resultBuilder.Append(currSource.Substring(number, 1));
            }
            return resultBuilder.ToString();
        }

        #endregion

        public object Get(TelCodeRequest request)
        {
            return sendMessage.SendSms(request.TelNum, "欢迎您注册账号，验证码");
            //return RecordSnumService.GetSnum(this,1, SnumType.InquiryRelease);
        }
    }
}
