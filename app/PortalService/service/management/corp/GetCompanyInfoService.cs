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
  public  class GetCompanyInfoService:Service,IGet<GetCompanyInfoRequest>
    {

        public object Get(GetCompanyInfoRequest request)
        {
            GetCompanyInfoResponse response = new GetCompanyInfoResponse();
            var companyInfo = this.Db.FirstOrDefault<NdtechCompany>(c => c.AccountID == request.AccountId);
            if (companyInfo!=null)
            {
                //获取图片资源
                var picResources = this.Db.FirstOrDefault<Resources>(r => r.Id == companyInfo.LogoResources);
                ReturnPicResources picres = new ReturnPicResources();
                picres = ToAuth(picResources);
                if (picres != null)
                {
                    picres.FileUrl = "/fileuploads/" + request.AccountId;
                }

                response.Data = ToEntity(companyInfo);
                response.Data.PicResources = picres;
                //获取主营产品分类资源
                List<ReturnMainProduct> mainProduct = new List<ReturnMainProduct>();
                var product = this.Db.Where<CompanyMainProduct>(c => c.AccountID == request.AccountId);
                foreach (var item in product)
                {
                    mainProduct.Add(ToEntity(item));
                }
              var certifi=  this.Db.FirstOrDefault<CompanyCertification>(c => c.Accountid == request.AccountId);
              if (certifi!=null)
              {
                  response.Data.CompNature = certifi.CompNature;
                  string adress = certifi.CompAddress;
                  if (!string.IsNullOrEmpty(adress))
                  {
                      int index = adress.IndexOf("市");
                      if (index != -1)
                      {
                          response.Data.CompAddress = adress.Substring(0, index + 1);
                      }
                      else
                      {
                          response.Data.CompAddress = adress.Substring(0, 2);
                      }

                  }
              }
             
             
                
                response.Data.MainProducts = mainProduct;
                response.Success = true;
            }
           
            return response;
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
    }
}
