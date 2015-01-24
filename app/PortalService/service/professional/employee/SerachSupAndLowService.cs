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

namespace Ndtech.PortalService.Auth
{
    public class SerachSupAndLowService : Service, IPost<SerachSupAndLowRequest>
    {
        public object Post(SerachSupAndLowRequest request)
        {
            ////查出上级表中上级所有员工id
            //var listSuperior = this.Db.Where<SuperiorEmployee>(s => s.StaffId == request.StaffId).Select(s => s.EId);
            ////查出下级表中下级所有员工id
            //var listLower = this.Db.Where<LowerEmployee>(s => s.StaffId == request.StaffId).Select(s => s.EId);
            ////合并2个集合
            //var listName = listSuperior.Concat(listLower);
            ////塞选上下级表中与该员工都没有上下级关系的所有员工
            var data = this.Db.Where<NdtechStaffInfo>(s => s.AccountID == request.AccountId && s.SysCode != "S001");
            //根据员工编号和姓名过滤

            if (!string.IsNullOrEmpty(request.SysCode))
            {
                data = data.Where(s => s.SysCode.Contains(request.SysCode)).ToList() ;
            }
            if (!string.IsNullOrEmpty(request.SysName))
            {
                data = data.Where(s => s.SysName.Contains(request.SysName)).ToList();
            }

            SetSuperiorResopnse response = new SetSuperiorResopnse();
            if (data.Count() > 0)
            {
                // response.RowsCount = data.Count();

                List<Superior> listModel = new List<Superior>();
                foreach (var item in data)
                {
                    listModel.Add(ToEntity(item));
                }
                //response.Data = new Data { SuperiorList = listModel, Page = new PortalModel.Page { PageIndex = 0, PageNumber = 10, RowCount = data.Count() } };
                response.Success = true;
            }
            return response;
        }
        /// <summary>
        /// 后端model转换为前端model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Superior ToEntity(NdtechStaffInfo item)
        {
            return item.TranslateTo<Superior>();
        }
    }
}
