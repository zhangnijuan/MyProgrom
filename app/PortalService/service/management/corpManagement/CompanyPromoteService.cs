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
    public class CompanyPromoteService : Service, IGet<CompanyPromoteRequest>, IPost<CompanyPromoteRequest>
    {

        public object Get(CompanyPromoteRequest request)
        {
            CompanyPromoteResponse response = new CompanyPromoteResponse();
            var companyPromotes = this.Db.Where<CompanyPromote>(c => c.AccountID == request.AccountID);
            List<CompanyPromoteInfo> list = new List<CompanyPromoteInfo>();
            if (companyPromotes.Count > 0)
            {

                foreach (var item in companyPromotes)
                {

                    CompanyPromoteInfo comInfo = ToEntity(item);
                    var resources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == item.ID);
                    comInfo.PicResources = ToEntity(resources);
                    if (comInfo.PicResources != null)
                    {
                        comInfo.PicResources.FileUrl = "/fileuploads/" + request.AccountID;
                    }

                    list.Add(comInfo);
                }
                response.Success = true;
            }
            response.Data = list;

            return response;
        }



        public object Post(CompanyPromoteRequest request)
        {
            CompanyPromoteResponse response = new CompanyPromoteResponse();
  
            this.Db.Delete<CompanyPromote>(c => c.Where(q => q.AccountID == request.AccountID));
            if (request.CompanyPromote != null)
            {
                int i = 0;
                foreach (var item in request.CompanyPromote)
                {
                    //var oldPro = this.Db.FirstOrDefault<CompanyPromote>(c => c.ID == item.ID);
                    CompanyPromote comPro = ToEntity(item);
                    //if (oldPro != null)
                    //{
                    //    //更新 
                    //    if (request.PicResources != null)
                    //    {
                    //        Resources resource = ToEntity(request.PicResources[i]);
                    //        resource.Id = Convert.ToInt64(request.PicResources[i].Id);
                    //        resource.DocumentID = oldPro.ID;
                    //        this.Db.Save<Resources>(resource);
                    //    }
                       
                    //    comPro.AccountID = request.AccountID;
                    //    this.Db.Save<CompanyPromote>(comPro);
                    //}
                    //else
                  //  {
                        //增加
                        comPro.ID = RecordIDService.GetRecordID(1);
                        comPro.AccountID = request.AccountID;
                        //保存图片资源
                        if (request.PicResources != null)
                        {
                            Resources resource = ToEntity(request.PicResources[i]);
                            if (resource != null)
                            {
                                resource.Id = RecordIDService.GetRecordID(1);
                                resource.AccountID = request.AccountID;
                                resource.DocumentID = comPro.ID;
                                this.Db.Insert<Resources>(resource);
                            }


                            comPro.ItemResources = resource.Id;
                        }


                        this.Db.Insert<CompanyPromote>(comPro);
                        i++;
                        response.Success = true;
                   // }

                }
            }

            return response;
        }


        /// <summary>
        /// 转换model
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private CompanyPromote ToEntity(CompanyPromoteInfo item)
        {
            return item.TranslateTo<CompanyPromote>();
        }
        private Resources ToEntity(ReturnPicResources returnPicResources)
        {
            return returnPicResources.TranslateTo<Resources>();
        }
        private ReturnPicResources ToEntity(Resources resources)
        {
            return resources.TranslateTo<ReturnPicResources>();
        }
        private CompanyPromoteInfo ToEntity(CompanyPromote item)
        {
            return item.TranslateTo<CompanyPromoteInfo>();
        }
    }
}
