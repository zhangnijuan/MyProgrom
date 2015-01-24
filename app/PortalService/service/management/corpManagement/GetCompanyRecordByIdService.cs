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
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{

   public  class GetCompanyRecordByIdService:Service,IGet<GetCompanyRecordByIdRequest>
    {
        public object Get(GetCompanyRecordByIdRequest request)
        {
            GetCompanyRecordByIdResponse response = new GetCompanyRecordByIdResponse();
            CompanyRecord companyRecord = new CompanyRecord();

            NdtechAcntSystem ndtechAcnt = null;
            if (!string.IsNullOrEmpty(request.CorpNum))
            {
                ndtechAcnt = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.CorpNum == request.CorpNum);
            }
            else
            {
                ndtechAcnt = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.ID == request.AccoutID);
            }

            if (ndtechAcnt!=null)
            {
                int accountId = ndtechAcnt.ID;
                //获取公司信息表数据
                GetCompInfo(accountId, companyRecord);
                //获取企业认证信息
                GetCompCertificationInfo(accountId, companyRecord);
                //获取企业简介信息
                GetCompIntro(accountId, companyRecord);
                
                response.Success = true;
            }
            response.Data = companyRecord;
            return response;
        }
       /// <summary>
       /// 获取企业简介信息
       /// </summary>
       /// <param name="request"></param>
       /// <param name="companyRecord"></param>
        private void GetCompIntro(int accountId, CompanyRecord companyRecord)
        {
            var companyIntro = this.Db.FirstOrDefault<CompanyIntro>(c => c.AccountID == accountId);
            if (companyIntro != null)
            {
                CompanyIntroInfo comIntro = ToEntity(companyIntro);
                var resources = this.Db.Where<Resources>(r => r.DocumentID == companyIntro.ID);
                if (resources != null)
                {
                    List<ReturnPicResources> picResources = new List<ReturnPicResources>();

                    foreach (var item in resources)
                    {
                        var res = ToAuth(item);
                        if (res != null)
                        {
                            res.FileUrl = "/fileuploads/" + accountId;
                        }

                        picResources.Add(res);
                    }
                    comIntro.PicResources = picResources;

                }
                companyRecord.CompanyIntro = comIntro;
            }
        }
       /// <summary>
       /// 获取企业认证信息
       /// </summary>
       /// <param name="request"></param>
       /// <param name="companyRecord"></param>
        private void GetCompCertificationInfo(int accountId, CompanyRecord companyRecord)
        {
            var compCertification = this.Db.FirstOrDefault<CompanyCertification>(c => c.Accountid == accountId);
            if (compCertification != null)
            {
                companyRecord.CompanyCertification = compCertification.TranslateTo<CompCertification>();
                //获取认证图片
                var licenseResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.LicenseResources);
                ReturnPicResources picres = new ReturnPicResources();
                picres = ToAuth(licenseResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                companyRecord.CompanyCertification.LicenseResources = picres;

                var taxResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.TaxResources);
                picres = ToAuth(taxResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                companyRecord.CompanyCertification.TaxResources = picres;

                var organizationResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.OrganizationResources);
                picres = ToAuth(organizationResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                companyRecord.CompanyCertification.OrganizationResources = picres;

                var opensResources = this.Db.FirstOrDefault<Resources>(r => r.Id == compCertification.OpensResources);
                picres = ToAuth(opensResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
                companyRecord.CompanyCertification.OpensResources = picres;
            }
        }
       /// <summary>
       /// 获取公司信息
       /// </summary>
       /// <param name="request"></param>
       /// <param name="companyRecord"></param>
        private void GetCompInfo(int accountId, CompanyRecord companyRecord)
        {
            var companyInfo = this.Db.FirstOrDefault<NdtechCompany>(c => c.AccountID == accountId);
            if (companyInfo != null)
            {
                //获取图片资源
                var picResources = this.Db.FirstOrDefault<Resources>(r => r.Id  == companyInfo.LogoResources);
                ReturnPicResources picres = new ReturnPicResources();
                picres = ToAuth(picResources);
                if (picres!=null)
                {
                    picres.FileUrl = "/fileuploads/" + accountId;
                }
               

                companyRecord.CompanyInfo = ToEntity(companyInfo);
                companyRecord.CompanyInfo.PicResources = picres;
                //获取主营产品分类资源
                List<ReturnMainProduct> mainProduct = new List<ReturnMainProduct>();
                var product = this.Db.Where<CompanyMainProduct>(c => c.AccountID == accountId);
                if (product!=null)
                {
                    foreach (var item in product)
                    {
                        mainProduct.Add(ToEntity(item));
                    }
                }
              
                companyRecord.CompanyInfo.MainProducts = mainProduct;
            }
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ReturnCompanyInfo ToEntity(NdtechCompany request)
        {
            return request.TranslateTo<ReturnCompanyInfo>();
        }
        private ReturnPicResources ToAuth(Resources picResources)
        {
            return picResources.TranslateTo<ReturnPicResources>();
        }
        private ReturnMainProduct ToEntity(CompanyMainProduct item)
        {
            return item.TranslateTo<ReturnMainProduct>();
        }
        private CompanyIntroInfo ToEntity(CompanyIntro companyIntro)
        {
            return companyIntro.TranslateTo<CompanyIntroInfo>();
        }
     
    }
}
