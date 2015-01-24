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
    /// 企业地址簿修改Service
    /// add by yangshuo 2014/12/16
    /// </summary>
    public class ModifyERPAddressService : Service, IPost<ModifyERPAddressRequest>
    {
        public IValidator<ModifyERPAddressRequest> Validator { get; set; }

        public object Post(ModifyERPAddressRequest request)
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

            //第二步:modify + 返回response
            return PostMethod(request);
        }

        private EnterpriseAddressResponse PostMethod(ModifyERPAddressRequest request)
        {
            EnterpriseAddressResponse response = new EnterpriseAddressResponse();

            try
            {
                //1.查询原资料
                EnterpriseAddress oldInfo = this.Db.FirstOrDefault<EnterpriseAddress>(x => x.AccountID == request.AccountID && x.ID == request.ID);

                if (oldInfo != null)
                {
                    //2.前台model转成dataModel
                    EnterpriseAddress newInfo = request.TranslateTo<EnterpriseAddress>();

                    EnterpriseDefAddress detailInfo = null;

                    #region 普通员工设置默认地址

                    if (request.UserCode != "S001" && request.IsDef == 1)
                    {
                        //先查询管理员默认地址,若是该笔不做操作,不是该笔将前段传的默认地址字段清成0,普通员工使用明细mid对接取默认地址
                        if (oldInfo.IsDef != 1)
                        {
                            newInfo.IsDef = 0;
                        }

                        //查询员工原来是否有默认地址
                        detailInfo = this.Db.FirstOrDefault<EnterpriseDefAddress>(x => x.AccountID == request.AccountID && x.EID == request.UserID);
                        if (detailInfo != null)
                        {
                            //update明细
                            detailInfo.MID = newInfo.ID;
                        }
                        else
                        {
                            //add明细
                            detailInfo = new EnterpriseDefAddress();
                            detailInfo.ID = RecordIDService.GetRecordID(1);
                            detailInfo.AccountID = request.AccountID;
                            detailInfo.MID = newInfo.ID;
                            detailInfo.EID = request.UserID;
                        }
                    }

                    #endregion

                    using (var trans = this.Db.BeginTransaction())
                    {
                        //3.insert
                        this.Db.Save(newInfo);

                        //设置默认地址
                        if (request.IsDef == 1)
                        {
                            //管理员更新主档地址为默认地址
                            if (request.UserCode == "S001")
                            {
                                //4.update其它地址为非默认地址
                                this.Db.Update<EnterpriseAddress>(string.Format("isdef = 0 where a = {0} and id != {1}", request.AccountID, request.ID));
                            }
                            else
                            {
                                //普通员工新增or更新明细地址为默认地址
                                this.Db.Save(detailInfo);
                            }
                        }
                        trans.Commit();
                    }

                    response.Success = true;
                }
                else
                {
                    //根据ID未找到资料
                    response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                    response.ResponseStatus.Message = Const.Err_NoInfoByID;
                    response.Success = false;
                }
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

    public class ModifyEnterpriseAddressValidator : AbstractValidator<ModifyERPAddressRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(ModifyERPAddressRequest instance)
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

            if (instance.ID <= 0)
            {
                //ID不能为空
                result.Errors.Add(new ValidationFailure("ID", Const.Err_IDIsNull, "Err_IDIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    #endregion
}
