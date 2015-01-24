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
    public class RetrieveVdWebCodeService : Service, IGet<ValidateWebCodeRequest>
    {
        public IDbConnectionFactory db { set; get; }
        #region IGet<RegisteredRequest> 成员

        public object Get(ValidateWebCodeRequest request)
        {
            bool flag = false;
            ValidateWebCodeResponse data = new ValidateWebCodeResponse();
            ValiDateUser(request.UserName, request.WebCode, data, ref flag);
            if (flag==true)
            {
                data.Success = true;
            }
            return data;
        }
        private void ValiDateUser(string UserName, string WebCode, ValidateWebCodeResponse response, ref bool flag)
        {
            response.Phone = "";
            try
            {
                using (var conn = db.OpenDbConnection())
                {
                    var session = this.GetNdtechSession();
                    NdtechWebCode CodeInfo = conn.QuerySingle<NdtechWebCode>(string.Format("select * from udoc_webcode where UserName = '{0}' AND WebCode='{1}';", session.SessionID, WebCode));
                    //判断页面验证码是否正确，如果不是直接返回
                    if (CodeInfo == null)
                    {
                        response.CodeStatus = false;
                        response.ResponseStatus.ErrorCode = ReturnValue.UserNameIsError.ToString();
                        flag = false;
                    }
                    else
                    {
                        //判断用户名是否存在
                        NdtechStaffInfo exsituser = conn.QuerySingle<NdtechStaffInfo>(string.Format("select * from udoc_staff where loginname = '{0}';", UserName));
                        if (exsituser == null)
                        {
                            response.CodeStatus = true;
                            response.ResponseStatus.ErrorCode = ReturnValue.UserNameIsError.ToString();
                            flag = false;
                        }
                        else
                        {
                            //判断用户名是否为管理员
                            if (exsituser.SysCode != "S001")
                            {
                                response.CodeStatus = true;
                                response.ResponseStatus.ErrorCode = ReturnValue.UserIsNotAdmin.ToString();
                                flag = false;
                            }
                            else
                            {
                                //成功则返回手机号

                                string PhoneStart = exsituser.TelNum.Substring(0, 3);
                                string PhoneEnd = exsituser.TelNum.Substring(7, 4);

                                string Phone = PhoneStart + "****" + PhoneEnd;
                                response.ResponseStatus.ErrorCode = ReturnValue.UserIsTrue.ToString();
                                response.Phone = Phone;
                                response.CodeStatus = true;
                                flag = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseStatus.ErrorCode = ex.ToErrorCode();
                response.ResponseStatus.Message = ex.ToString();
            }
        }
        private enum ReturnValue
        {
            UserNameIsError = 1,//用户名错误或者不存在或者验证码错误
            UserIsNotAdmin = 2,//用户名不是管理员
            UserIsTrue = 3,//用户名正确
        }
        #endregion

    }
}
