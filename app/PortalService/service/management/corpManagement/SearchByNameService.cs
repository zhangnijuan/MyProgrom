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
    /// <summary>
    /// 根据姓名模糊搜索
    /// </summary>
    public class SearchByNameService : Service
    {

        public object Any(SearchByNameRequest request)
        {
            //获取session中的accountid
            //long accountId = this.SessionAs<INdtechAuthSession>().AccountID;
            
            //根据条件查询出数据
            var list = this.Db.Where<NdtechStaffInfo>(n => n.AccountID ==request.AccountID && n.SysName.Contains(request.Name));
            GetAllEmployeeResponse response = new GetAllEmployeeResponse();
            //转换为前端model
            if (list != null)
            {
                //获取总行数
                response.RowsCount = list.Count;
                //分页
                var data = list.OrderBy(n => n.SysCode).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                List<ReturnList> listModel = new List<ReturnList>();
                foreach (var item in data)
                {

                    var staff = ToEntity(item);
                    var resources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == item.ID);
                    staff.PicResources = ToEntity(resources);
                    if (staff.PicResources!=null)
                    {
                        staff.PicResources.FileUrl = "/fileuploads/" + request.AccountID;
                    }
                    
                    //获取上下级员工
                    List<ReturnSupLowEmployee> superiors = new List<ReturnSupLowEmployee>();
                    List<ReturnSupLowEmployee> lowers = new List<ReturnSupLowEmployee>();

                    var superiorsList = this.Db.Where<SuperiorEmployee>(s => s.StaffId == item.ID).ToList();
                    if (superiorsList.Count > 0)
                    {
                        foreach (var super in superiorsList)
                        {
                            superiors.Add(super.TranslateTo<ReturnSupLowEmployee>());
                        }
                    }
                    //获取下级员工
                   
                    var lowersList = this.Db.Where<LowerEmployee>(s => s.StaffId == item.ID).ToList();
                    if (superiorsList.Count > 0)
                    {
                        foreach (var super in superiorsList)
                        {
                            lowers.Add(super.TranslateTo<ReturnSupLowEmployee>());
                        }
                    }
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

    }
}
