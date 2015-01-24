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
    public class SetSuperiorService : Service, IPost<SetSuperiorRequest>, IGet<SetSuperiorRequest>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(SetSuperiorRequest request)
        {
            //    //查出上级表中上级所有员工id
            //    var listSuperior = this.Db.Where<SuperiorEmployee>(s => s.StaffId == request.StaffId).Select(s => s.EId);
            //    //查出下级表中下级所有员工id

            List<NdtechStaffInfo> data = null;
            if (request.EmpolyeeEnum == SearchEmpolyeeEnum.Superior)
            {
                var list = this.Db.Where<LowerEmployee>(s => s.StaffId == request.StaffId).Select(s => s.EId);
                data = this.Db.Where<NdtechStaffInfo>(s => s.AccountID == request.AccountId && s.ID != request.StaffId && s.SysCode != "S001").Where(s=>list.Contains(s.ID)==false).ToList();
            }
            else
            {
                var list = this.Db.Where<SuperiorEmployee>(s => s.StaffId == request.StaffId).Select(s => s.EId);
                data = this.Db.Where<NdtechStaffInfo>(s => s.AccountID == request.AccountId && s.ID != request.StaffId && s.SysCode != "S001" ).Where(s => list.Contains(s.ID) == false).ToList();
            }

            //    //合并2个集合
            //    var listName = listSuperior.Concat(listLower);
            //    //塞选上下级表中与该员工都没有上下级关系的所有员工


            SetSuperiorResopnse response = new SetSuperiorResopnse();
            if (data != null && data.Count() > 0)
            {
                //response.RowsCount = data.Count();
                data = data.OrderBy(n => n.ID).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                List<Superior> listModel = new List<Superior>();
                foreach (var item in data)
                {
                    listModel.Add(ToEntity(item));
                }
                response.Data = new Data { SuperiorList = listModel, Page = new PortalModel.Page { PageIndex = 0, PageNumber = 10, RowCount = data.Count() } };
                response.Success = true;
            }

            return response;
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Post(SetSuperiorRequest request)
        {
            SetSuperiorResopnse response = new SetSuperiorResopnse();
            SuperiorEmployee superior = null;
            //开启事物循环插入到上下级关系表中
            using (var trans = this.Db.BeginTransaction())
            {
                //把改员工的上级全部删除  下级表中也删除
                this.Db.Delete<SuperiorEmployee>(s => s.Where(q => q.StaffId == request.StaffId));
                this.Db.Delete<LowerEmployee>(s => s.Where(q=>q.EId==request.StaffId));
                //遍历转换为后端model 并插入到2个数据库中
                if (request.Superiors != null && request.Superiors.Count > 0)
                {
                    foreach (var item in request.Superiors)
                    {

                        //转换model
                        ReceiveSuperior receivesuperior = new ReceiveSuperior();
                        receivesuperior.EId = item.ID;
                        receivesuperior.EIdCode = item.SysCode;
                        receivesuperior.EIdName = item.SysName;
                        superior = ToEntityLast<SuperiorEmployee, ReceiveSuperior>(receivesuperior);
                        superior.Id = RecordIDService.GetRecordID(1);
                        superior.StaffId = request.StaffId;
                        var employee = this.Db.FirstOrDefault<NdtechStaffInfo>(s => s.ID == request.StaffId);
                        ////生成下级表实体
                        LowerEmployee lower = new LowerEmployee();
                        lower.Id = RecordIDService.GetRecordID(1);
                        lower.StaffId = superior.EId;
                        lower.EId = request.StaffId;
                        lower.EIdCode = employee.SysCode;
                        lower.EIdName = employee.SysName;
                        //插入到数据库
                        this.Db.Insert(superior);
                         this.Db.Insert(lower);
                    }
                }

                trans.Commit();
            }
            response.ResponseStatus.Message = "设置成功";
            response.Success = true;
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
        /// <summary>
        /// model转换
        /// </summary>
        /// <typeparam name="T">转换后的Model类型</typeparam>
        /// <typeparam name="U">转换前的Model类型</typeparam>
        /// <param name="item"></param>
        /// <returns></returns>

        private T ToEntityLast<T, U>(U item)
            where T : class,new()
            where U : class,new()
        {
            return item.TranslateTo<T>();
        }

    }
}
