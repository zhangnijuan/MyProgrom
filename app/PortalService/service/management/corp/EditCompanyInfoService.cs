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
using Ndtech.PortalModel;
namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 验证
    /// </summary>
    public class CompanyInfoValidator : AbstractValidator<EditCompanyInfoRequest>
    {
        public override ValidationResult Validate(EditCompanyInfoRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(instance.CompName))
            {
                result.Errors.Add(new ValidationFailure("CompName", Const.Err_CompNameIsNull, "Err_CompNameIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.TelPhone))
            {
                result.Errors.Add(new ValidationFailure("TelPhone", Const.Err_TelPhoneInNull, ".Err_TelPhoneInNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.CompanyScale))
            {
                result.Errors.Add(new ValidationFailure("CompanyScale", Const.Err_CompanyScaleIsNull, "Err_CompanyScaleIsNull"));
                return result;
            }

            //if (instance.MainProducts == null)
            //{
            //    result.Errors.Add(new ValidationFailure("MainProduct", Const.Err_MainProductIsNull, "Err_MainProductIsNull"));
            //    return result;
            //}
            return base.Validate(instance);
        }
    }
    public class EditCompanyInfoService : Service, IPost<EditCompanyInfoRequest>
    {
        public IValidator<EditCompanyInfoRequest> CompanyInfoValidator { get; set; }
        public object Post(EditCompanyInfoRequest request)
        {
            EditCompanyInfoResponse response = new EditCompanyInfoResponse();

            //校验前端的数据合法性
            ValidationResult result = CompanyInfoValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            using (var trans = this.Db.BeginTransaction())  
            {
                //转换为datamodel        
                NdtechCompany companyInfo = ToEntity(request);

                var oldCompanyInfo = this.Db.FirstOrDefault<NdtechCompany>(n => n.AccountID == request.AccountID);
                if (oldCompanyInfo != null)
                {
                    if (request.CompName != oldCompanyInfo.CompName)
                    {
                        companyInfo.Approved = 0;
                    }
                    else
                    {
                        companyInfo.Approved = oldCompanyInfo.Approved;

                    }
                    companyInfo.ID = oldCompanyInfo.ID;
                    companyInfo.Createdate = oldCompanyInfo.Createdate;
                    companyInfo.CompCode = oldCompanyInfo.CompCode;
                    companyInfo.Favorites = oldCompanyInfo.Favorites;
                    companyInfo.inquiryNumber = oldCompanyInfo.inquiryNumber;
                    companyInfo.Releases = oldCompanyInfo.Releases;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    return response;
                }
                //插入或者更新logo资源表
                Resources resources = ToEntity(request.PicResources);
                if (resources != null)
                {
                    var data = this.Db.FirstOrDefault<Resources>(r => r.Id == oldCompanyInfo.LogoResources);
                    if (data != null)
                    {
                        resources.Id = oldCompanyInfo.LogoResources;
                        resources.DocumentID = oldCompanyInfo.ID;
                        resources.AccountID = oldCompanyInfo.AccountID;
                        resources.FileLength =string.IsNullOrEmpty( resources.FileLength)?"0":resources.FileLength;
                        this.Db.Update<Resources>(resources);
                    }
                    else
                    {
                        resources.Id = RecordIDService.GetRecordID(1);
                        resources.AccountID = request.AccountID;
                        resources.DocumentID = oldCompanyInfo.ID;
                        resources.FileLength = string.IsNullOrEmpty(resources.FileLength) ? "0" : resources.FileLength;
                        this.Db.Insert(resources);
                    }
                    companyInfo.LogoResources = resources.Id;
                }

                this.Db.Save<NdtechCompany>(companyInfo);

                //删除原来数据
                //var list = this.Db.Where<CompanyMainProduct>(c => c.CompID == oldCompanyInfo.ID);
                //this.Db.DeleteAll(list);
                this.Db.Delete<CompanyMainProduct>(c => c.Where(p => p.CompID == oldCompanyInfo.ID));
                if (request.MainProducts != null)
                {
                    foreach (var item in request.MainProducts)
                    {
                        CompanyMainProduct mainPro = ToEntity(item);
                        //var data = this.Db.FirstOrDefault<CompanyMainProduct>(c => c.Id == mainPro.Id);
                        mainPro.AccountID = request.AccountID;

                        mainPro.Id = RecordIDService.GetRecordID(1);
                        mainPro.CompID = oldCompanyInfo.ID;
                        this.Db.Insert<CompanyMainProduct>(mainPro);

                    }
                }

                trans.Commit();
            }

            return response;
        }


        /// <summary>
        /// 转换model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private NdtechCompany ToEntity(EditCompanyInfoRequest request)
        {
            return request.TranslateTo<NdtechCompany>();
        }
        private Resources ToEntity(ReturnPicResources picResources)
        {
            return picResources.TranslateTo<Resources>();
        }
        private CompanyMainProduct ToEntity(MainProduct item)
        {
            return item.TranslateTo<CompanyMainProduct>();
        }
    }
}
