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
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 检验数据
    /// </summary>
    public class CompanyCertificationValidator : AbstractValidator<CompanyCertificationRequest>
    {
        public CompanyCertificationValidator()
        {

         

        }
        public override ValidationResult Validate(CompanyCertificationRequest instance)
        {
            ValidationResult result = new ValidationResult();
            RuleFor(c => c.CompNature).NotEmpty().WithMessage(Const.Err_CompNatureIsNull);
            RuleFor(c => c.CompAddress).NotEmpty().WithMessage(Const.Err_CompAddressIsNull);
            RuleFor(c => c.Capital).NotEmpty().WithMessage(Const.Err_RegMoneyIsNull);
            RuleFor(c => c.LegalPerson).NotEmpty().WithMessage(Const.Err_LegalPersonIsNull);
            RuleFor(c => c.RegistrationAuthority).NotEmpty().WithMessage(Const.Err_RegOfficeIsNull);
            RuleFor(c => c.CreateTime).NotEmpty().WithMessage(Const.Err_RegTimeIsNull);
            RuleFor(c => c.AnnualInspection).NotEmpty().WithMessage(Const.Err_AnnualTimeIsNull);
            RuleFor(c => c.BusinessStart).NotEmpty().WithMessage(Const.Err_BusnissTimeIsNull);
            RuleFor(c => c.BusinessEnd).NotEmpty().WithMessage(Const.Err_BusnissTimeIsNull);
            RuleFor(c => c.BusinessScope).NotEmpty().WithMessage(Const.Err_BusinessScopeIsNull);
            RuleFor(c => c.License).NotEmpty().WithMessage(Const.Err_LicenseIdIsNull);
            RuleFor(c => c.LicenseResources).NotNull().WithMessage(Const.Err_LicenseResourcesIsNull);
            RuleFor(c => c.Tax).NotEmpty().WithMessage(Const.Err_TaxIdIsNull);
            RuleFor(c => c.TaxResources).NotNull().WithMessage(Const.Err_TaxResourcesIsNull);
            RuleFor(c => c.OpenResources).NotNull().WithMessage(Const.Err_OpenResourcesIsNull);
            if (instance.CompNature != "个体经营")
            {
                if (string.IsNullOrEmpty(instance.OrganizationCode))
                {
                    result.Errors.Add(new ValidationFailure("OrganizationCode", Const.Err_OrganizationCodeIsNull, "Err_OrganizationCodeIsNull"));
                    return result;
                }
                if (instance.OrganizationResources == null)
                {
                    result.Errors.Add(new ValidationFailure("OrganizationResources", Const.Err_OrganizationResourcesIsNull, "Err_OrganizationResourcesIsNull"));
                    return result;
                }
            }

            return base.Validate(instance);
        }
    }
    public class CompanyCertificationService : Service, IPost<CompanyCertificationRequest>, IGet<CompanyCertificationRequest>
    {
        public IValidator<CompanyCertificationRequest> CompanyCertificationValidator { get; set; }
        public object Post(CompanyCertificationRequest request)
        {
            ////验证
           
            CompanyCertificationResponse response = new CompanyCertificationResponse();
            ValidationResult result = CompanyCertificationValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //开启事物  

            var certifi = this.Db.FirstOrDefault<CompanyCertification>(c => c.CompId == request.CompId);
            if (certifi != null)
            {
                this.Db.Delete<CompanyCertification>(certifi);
            }
            using (var trans = this.Db.BeginTransaction())
            {
                this.Db.Update<NdtechCompany>(set: "approved = {0}".Params("1"),
                where: " id= {0}".Params(request.CompId));
                CompanyCertification company = ToEntity(request);
                company.ID = RecordIDService.GetRecordID(1);
                company.Accountid = request.AccountID;
                company.CompId = request.CompId;
                //把图片资源信息插入到资源表中

                //保存营业执照资源
                Resources resource = ToEntity(request.LicenseResources);
                if (resource != null)
                {
                    resource.Id = RecordIDService.GetRecordID(1);
                    resource.AccountID = request.AccountID;
                    resource.DocumentID = company.ID;
                    this.Db.Insert(resource);
                    company.LicenseResources = resource.Id;
                }


                //保存税务登记证资源
                resource = ToEntity(request.TaxResources);
                if (resource != null)
                {

                    resource.Id = RecordIDService.GetRecordID(1);
                    resource.AccountID = request.AccountID;
                    resource.DocumentID = company.ID;
                    this.Db.Insert(resource);
                    company.TaxResources = resource.Id;
                }


                //保存组织机构代码证资源
                resource = ToEntity(request.OrganizationResources);
                if (resource != null)
                {
                    resource.Id = RecordIDService.GetRecordID(1);
                    resource.AccountID = request.AccountID;
                    resource.DocumentID = company.ID;
                    this.Db.Insert(resource);
                    company.OrganizationResources = resource.Id;
                }


                //保存开户许可证资源
                resource = ToEntity(request.OpenResources);
                if (resource != null)
                {
                    resource.Id = RecordIDService.GetRecordID(1);
                    resource.AccountID = request.AccountID;
                    resource.DocumentID = company.ID;
                    this.Db.Insert(resource);
                    company.OpensResources = resource.Id;
                }


                //保存认证信息
                this.Db.Insert<CompanyCertification>(company);
                trans.Commit();
                response.Success = true;
            }


            return response;
        }
        public object Get(CompanyCertificationRequest request)
        {

            CompanyCertificationResponse response = new CompanyCertificationResponse();
            response.Data = GetCompCertificationInfo(request.AccountID);
            if (response.Data!=null)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
           
            return response;
        }
        /// <summary>
        /// 获取企业认证信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="companyRecord"></param>
        private CompCertification GetCompCertificationInfo(int accountId)
        {
            var compCertification = this.Db.FirstOrDefault<CompanyCertification>(c => c.Accountid == accountId);
            CompCertification compCer = null;
            if (compCertification != null)
            {
                compCer = compCertification.TranslateTo<CompCertification>(); ;

                //获取认证图片
                var licenseResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.LicenseResources);
                ReturnPicResources picres = new ReturnPicResources();
                picres = ToAuth(licenseResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                compCer.LicenseResources = picres;

                var taxResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.TaxResources);
                picres = ToAuth(taxResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                compCer.TaxResources = picres;

                var organizationResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.OrganizationResources);
                picres = ToAuth(organizationResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                compCer.OrganizationResources = picres;

                var opensResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.OpensResources);
                picres = ToAuth(opensResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                compCer.OpensResources = picres;
            }
            return compCer;
        }
        /// <summary>
        /// 转换model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private CompanyCertification ToEntity(CompanyCertificationRequest request)
        {
            return request.TranslateTo<CompanyCertification>();
        }
        private Resources ToEntity(PicResources picResources)
        {
            return picResources.TranslateTo<Resources>();
        }
        private ReturnPicResources ToAuth(Resources picResources)
        {
            return picResources.TranslateTo<ReturnPicResources>();
        }


    }
}
