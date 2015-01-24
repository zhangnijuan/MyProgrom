using System;
using System.IO;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 新增企业地址薄Service
    /// add by yangshuo 2014/12/16
    /// </summary>
    public class NewEnterpriseAddressService : Service, IPost<NewEnterpriseAddressRequest>
    {
        public IValidator<NewEnterpriseAddressRequest> Validator { get; set; }

        public object Post(NewEnterpriseAddressRequest request)
        {
            EnterpriseAddressResponse response = new EnterpriseAddressResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = Validator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            //第二步:new企业地址薄 + 返回response
            return PostMethod(request);
        }

        private EnterpriseAddressResponse PostMethod(NewEnterpriseAddressRequest request)
        {
            EnterpriseAddressResponse response = new EnterpriseAddressResponse();

            try
            {
                //1.企业地址薄前台model转成dataModel
                EnterpriseAddress newInfo = request.TranslateTo<EnterpriseAddress>();

                //2.add为非默认地址时需检核企业是否已有资料
                if (newInfo.IsDef == 0)
                {
                    EnterpriseAddress existInfo = this.Db.FirstOrDefault<EnterpriseAddress>(x => x.AccountID == request.AccountID);
                    if (existInfo == null)
                    {
                        //如果没有地址簿资料,此笔为默认地址
                        newInfo.IsDef = 1;
                    }
                }

                newInfo.ID = RecordIDService.GetRecordID(1);

                EnterpriseDefAddress detailInfo = null;
                if (request.UserCode != "S001" && request.IsDef == 1)
                {
                    //非管理员使用明细地址薄做为默认地址
                    newInfo.IsDef = 0;

                    //查询员工原来是否有默认地址
                    detailInfo = this.Db.FirstOrDefault<EnterpriseDefAddress>(x => x.AccountID == request.AccountID && x.EID == request.UserID);
                    if (detailInfo != null)
                    {
                        //update
                        detailInfo.MID = newInfo.ID;
                    }
                    else
                    {
                        //add
                        detailInfo = new EnterpriseDefAddress();
                        detailInfo.ID = RecordIDService.GetRecordID(1);
                        detailInfo.AccountID = request.AccountID;
                        detailInfo.MID = newInfo.ID;
                        detailInfo.EID = request.UserID;
                    }
                }

                //3.insert
                using (var trans = this.Db.BeginTransaction())
                {
                    //insert
                    this.Db.Save(newInfo);

                    //设置默认地址
                    //管理员更新主档地址为默认地址
                    if (request.UserCode == "S001" && newInfo.IsDef == 1)
                    {
                        //update其它地址为非默认地址
                        this.Db.Update<EnterpriseAddress>(string.Format("isdef = 0 where a = {0} and id != {1}", newInfo.AccountID, newInfo.ID));
                    }
                    else
                    {
                        if (request.IsDef == 1)
                        {
                            //普通员工新增or更新明细地址为默认地址
                            this.Db.Save(detailInfo);
                        }
                    }
                    trans.Commit();
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }

            return response;
        }

        #region Log日志

        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error"
                + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }

        #endregion
    }

    #region Validate

    public class NewEnterpriseAddressValidator : AbstractValidator<NewEnterpriseAddressRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(NewEnterpriseAddressRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.CompName))
            {
                //公司名称不能为空
                result.Errors.Add(new ValidationFailure("CompName", Const.Err_CompNameIsNull, "Err_CompNameIsNull"));
                return result;
            }

            //省市区地址检核
            if (string.IsNullOrEmpty(instance.Province))
            {
                result.Errors.Add(new ValidationFailure("Province", Const.Err_ProvinceIsNull, "Err_ProvinceIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.City))
            {
                result.Errors.Add(new ValidationFailure("City", Const.Err_CityIsNull, "Err_CityIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.District))
            {
                result.Errors.Add(new ValidationFailure("District", Const.Err_DistrictIsNull, "Err_DistrictIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Address))
            {
                result.Errors.Add(new ValidationFailure("Address", Const.Err_AddressIsNull, "Err_AddressIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    #endregion
}
