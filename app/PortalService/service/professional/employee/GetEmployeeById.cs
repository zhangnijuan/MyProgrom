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
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    public class GetEmployeeById : Service, IGet<GetEmployeeByIdRequest>
    {
        public object Get(GetEmployeeByIdRequest request)
        {
            var employee = this.Db.FirstOrDefault<NdtechStaffInfo>(n => n.ID == request.Id);
            var picResources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == employee.ID);
            GetEmployeeByIdResponse response= new GetEmployeeByIdResponse();
           //获取图片资源
            ReturnPicResources picres = new ReturnPicResources();
            picres = ToAuth(picResources);
            if (picres!=null)
            {
                picres.FileUrl = "/fileuploads/" + employee.AccountID;
            }
            response.Data = ToAuth(employee);
            response.Data.PicResources = picres;
            //获取上下级员工
            List<ReturnSupLowEmployee> superiors = new List<ReturnSupLowEmployee>();
            List<ReturnSupLowEmployee> lowers = new List<ReturnSupLowEmployee>();
            var superiorsList = this.Db.Where<SuperiorEmployee>(s => s.StaffId == request.Id).ToList();
            if (superiorsList.Count > 0)
            {
                foreach (var super in superiorsList)
                {
                    superiors.Add(super.TranslateTo<ReturnSupLowEmployee>());
                }
            }
            //获取下级员工
            response.Data.Superiors = superiors;
            var lowersList = this.Db.Where<LowerEmployee>(s => s.StaffId == request.Id).ToList();
            if (superiorsList.Count > 0)
            {
                foreach (var super in lowersList)
                {
                    lowers.Add(super.TranslateTo<ReturnSupLowEmployee>());
                }
            }
            response.Data.Lowers=lowers;
            response.Success = true;
            return  response;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private ReturnList ToAuth(NdtechStaffInfo employee)
        {

            return employee.TranslateTo<ReturnList>();
        }
        private ReturnPicResources ToAuth(Resources picResources)
        {
            return picResources.TranslateTo<ReturnPicResources>();
        }
        
    }
}
