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
    public class CompanyIntroSercice : Service, IGet<CompanyIntroRequest>, IPost<CompanyIntroRequest>
    {
        public object Get(CompanyIntroRequest request)
        {
            CompanyIntroResponse response = new CompanyIntroResponse();
            var companyIntro = this.Db.FirstOrDefault<CompanyIntro>(c => c.AccountID == request.AccountID);
            if (companyIntro != null)
            {
                CompanyIntroInfo comIntro = ToEntity(companyIntro);
                var resources = this.Db.Where<Resources>(r => r.DocumentID == companyIntro.ID);
                if (resources != null)
                {
                    List<ReturnPicResources> picResources = new List<ReturnPicResources>();

                    foreach (var item in resources)
                    {
                        var res = ToEntity(item);
                        if (res != null)
                        {
                            res.FileUrl = "/fileuploads/" + request.AccountID;
                        }

                        picResources.Add(res);
                    }
                    comIntro.PicResources = picResources;
                    response.Success = true;
                }
                response.Data = comIntro;
            }
            return response;
        }





        public object Post(CompanyIntroRequest request)
        {
            CompanyIntro companyIntro = ToEntity(request);
            CompanyIntroResponse response = new CompanyIntroResponse();
            using (var trans = this.Db.BeginTransaction())
            {
                var oldIntro = this.Db.FirstOrDefault<CompanyIntro>(c => c.AccountID == request.AccountID);
               
                if (oldIntro != null)
                {
                   
                    companyIntro.ID = oldIntro.ID;
                    //修改
                    //删除资源表中所有的图片
                    this.Db.Delete<Resources>(r => r.Where(p => p.DocumentID == oldIntro.ID));
                    Resources resource = null;
                    if (request.ResourcesImages != null)
                    {
                        foreach (var item in request.ResourcesImages)
                        {
                            resource = ToEntity(item);
                            long id = Convert.ToInt64(item.Id);
                            //var oldResource = this.Db.FirstOrDefault<Resources>(r => r.Id == id);
                            //this.Db.Delete<Resources>(r => r.Where(p => p.Id == id));
                            resource.Id = RecordIDService.GetRecordID(1);
                            resource.AccountID = request.AccountID;
                            resource.DocumentID = companyIntro.ID;
                            this.Db.Insert(resource);

                        }
                        this.Db.Save<CompanyIntro>(companyIntro);
                    }
                    response.Success = true;
                }
                else
                {
                    //新增
                    companyIntro.ID = RecordIDService.GetRecordID(1);
                    Resources resource = null;
                    if (request.ResourcesImages != null)
                    {
                        foreach (var item in request.ResourcesImages)
                        {
                            resource = ToEntity(item);
                            if (resource != null)
                            {
                                resource.Id = RecordIDService.GetRecordID(1);
                                resource.AccountID = request.AccountID;
                                resource.DocumentID = companyIntro.ID;
                                this.Db.Insert(resource);
                            }


                        }
                        var list = this.Db.Where<Resources>(r => r.DocumentID == companyIntro.ID).Select(r => r.Id).ToList();
                        try
                        {
                            companyIntro.CompanyOneResources = list[0];
                            companyIntro.CompanyTwoResources = list[1];
                            companyIntro.CompanyThreeResources = list[2];
                            companyIntro.CompanyFourResources = list[3];
                            companyIntro.CompanyFiveResources = list[4];
                        }
                        catch
                        {
                        }

                        this.Db.Insert<CompanyIntro>(companyIntro);

                    }

                    response.Success = true;
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
        private CompanyIntro ToEntity(CompanyIntroRequest request)
        {
            return request.TranslateTo<CompanyIntro>();
        }
        private Resources ToEntity(ReturnPicResources item)
        {
            return item.TranslateTo<Resources>();
        }
        private ReturnPicResources ToEntity(Resources item)
        {
            return item.TranslateTo<ReturnPicResources>();
        }
        private CompanyIntroInfo ToEntity(CompanyIntro companyIntro)
        {
            return companyIntro.TranslateTo<CompanyIntroInfo>();
        }
    }
}
