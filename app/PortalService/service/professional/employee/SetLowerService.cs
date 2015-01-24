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
    public class SetLowerService : Service, IPost<SetLowerRequest>
    {

        public object Post(SetLowerRequest request)
        {
            SetLowerResopnse response = new SetLowerResopnse();
            LowerEmployee lower = null;
           
            List<LowerEmployee> lowers = new List<LowerEmployee>();
            List<SuperiorEmployee> superiors = new List<SuperiorEmployee>();
            this.Db.Delete<LowerEmployee>(s => s.Where(q => q.StaffId == request.StaffId));
            this.Db.Delete<SuperiorEmployee>(s => s.Where(q => q.EId == request.StaffId));
            if (request.Superiors != null && request.Superiors.Count > 0)
            {
                //把前端接收的数据转化后装到list集合中
                foreach (var item in request.Superiors)
                {
                    //转换model
                    ReceiveSuperior receivesuperior = new ReceiveSuperior();
                    receivesuperior.EId = item.ID;
                    receivesuperior.EIdCode = item.SysCode;
                    receivesuperior.EIdName = item.SysName;
                    lower = ToEntityLast<LowerEmployee, ReceiveSuperior>(receivesuperior);
                    lower.Id = RecordIDService.GetRecordID(1);
                    lower.StaffId = request.StaffId;
                    lowers.Add(lower);
                    var employee = this.Db.FirstOrDefault<NdtechStaffInfo>(s => s.ID == request.StaffId);
                    SuperiorEmployee superior = new SuperiorEmployee();
                    superior.Id = RecordIDService.GetRecordID(1);
                    superior.StaffId = lower.EId;
                    superior.EId = request.StaffId;
                    superior.EIdCode = employee.SysCode;
                    superior.EIdName = employee.SysName;
                    superiors.Add(superior);
                    
                }
                //开启事物循环插入到下级关系表中
                using (var trans = this.Db.BeginTransaction())
                {       //删除下级关系和上级关系

                    this.Db.InsertAll<LowerEmployee>(lowers);
                    this.Db.InsertAll<SuperiorEmployee>(superiors);
                    trans.Commit();
                }
               
            }
           

            response.ResponseStatus.Message = "设置成功";
            response.Success = true;
            return response;
        }
        private T ToEntityLast<T, U>(U item)
            where T : class,new()
            where U : class,new()
        {
            return item.TranslateTo<T>();
        }
    }
}
