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
using Ndtech.PortalModel;
namespace Ndtech.PortalService.Auth
{

    public class GetAllEmployeeService : Service, IPost<GetAllEmployeeRequest>, IGet<GetAllEmployeeRequest>
    {

        public object Post(GetAllEmployeeRequest request)
        {

            GetAllEmployeeResponse response = new GetAllEmployeeResponse();
            //查询所有数据
            var list = request.RoleID == -1 ? this.Db.Where<NdtechStaffInfo>(n => n.AccountID == request.AccountID && n.State == request.State) : this.Db.Where<NdtechStaffInfo>(n => n.AccountID == request.AccountID && n.State == request.State && n.RoleID == request.RoleID);
            //拼多条件查询
            if (!string.IsNullOrEmpty(request.SysName))
            {
                list = list.Where<NdtechStaffInfo>(n => n.SysName.Contains(request.SysName) || n.SysCode.Contains(request.SysCode)).ToList();
            }
            if (!string.IsNullOrEmpty(request.SysCode))
            {
                list = list.Where<NdtechStaffInfo>(n => n.SysCode.Contains(request.SysCode)|| n.SysName.Contains(request.SysName)).ToList();
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                list = list.Where(n => n.UserName.Contains(request.UserName)).ToList();
            }
            if (list != null)
            {
                response.RowsCount = list.Count;
                //分页
                var data = list.OrderBy(n => n.ID).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                
                //转换为前端model
                List<ReturnList> listModel = new List<ReturnList>();
                
                foreach (var item in data)
                {
                    List<ReturnSupLowEmployee> superiors = new List<ReturnSupLowEmployee>();
                    List<ReturnSupLowEmployee> lowers = new List<ReturnSupLowEmployee>();
                    var staff = ToEntity(item);
                    var resources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == item.ID);
                    staff.PicResources = ToEntity(resources);
                    if (staff.PicResources!=null)
                    {
                        staff.PicResources.FileUrl = "/fileuploads/" + request.AccountID;
                    }
                   
                    //获取当前员工上下级员工
                    GetSupLowEmp(superiors, lowers, item);
                    staff.Superiors = superiors;
                    staff.Lowers = lowers;
                    listModel.Add(staff);
                    
                }
                response.Data = listModel;
             
            }

            return response;
        }
        /// <summary>
        /// 后端model转换为前端model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ReturnList ToEntity(NdtechStaffInfo item)
        {
            return item.TranslateTo<ReturnList>();
        }
        private ReturnPicResources ToEntity(Resources item)
        {
            return item.TranslateTo<ReturnPicResources>();
        }


        public object Get(GetAllEmployeeRequest request)
        {
            GetAllEmployeeResponse response = new GetAllEmployeeResponse();
            var list = this.Db.Where<NdtechStaffInfo>(n => n.AccountID == request.AccountID && n.State == 1);
            if (list != null)
            {
                response.RowsCount = list.Count;
                //分页
                var data = list.OrderBy(n => n.ID).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();

                //转换为前端model
                List<ReturnList> listModel = new List<ReturnList>();
                
                foreach (var item in data)
                {
                    List<ReturnSupLowEmployee> superiors = new List<ReturnSupLowEmployee>();
                    List<ReturnSupLowEmployee> lowers = new List<ReturnSupLowEmployee>();
                    var staff = ToEntity(item);
                    var resources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == item.ID);
                    staff.PicResources = ToEntity(resources);
                    if (staff.PicResources != null)
                    {
                        staff.PicResources.FileUrl = "/fileuploads/" + request.AccountID;
                    }
                    //获取当前员工上级员工
                    GetSupLowEmp(superiors, lowers, item);
                    staff.Superiors = superiors;
                    staff.Lowers = lowers;
                    listModel.Add(staff);
                    
                }

                response.Data = listModel;
              
            }
            return response;
        }
        /// <summary>
        /// 获取上下级员工
        /// </summary>
        /// <param name="superiors"></param>
        /// <param name="lowers"></param>
        /// <param name="item"></param>
        private void GetSupLowEmp(List<ReturnSupLowEmployee> superiors, List<ReturnSupLowEmployee> lowers, NdtechStaffInfo item)
        {
            var superiorsList = this.Db.Where<SuperiorEmployee>(s => s.StaffId == item.ID).ToList();
            if (superiorsList.Count>0)
            {
                foreach (var super in superiorsList)
                {
                    superiors.Add(super.TranslateTo<ReturnSupLowEmployee>());
                }
            }
            //获取下级员工

            var lowersList = this.Db.Where<LowerEmployee>(s => s.StaffId == item.ID).ToList();
            if (lowersList.Count > 0)
            {
                foreach (var super in lowersList)
                {
                    lowers.Add(super.TranslateTo<ReturnSupLowEmployee>());
                }
            }
        }
    }
}
