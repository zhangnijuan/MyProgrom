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
using System.Collections;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.service.management.mySale
{
    public class SavePurchasePlanService : Service, IPost<SavePurchasePlanRequest>
    {
        public object Post(SavePurchasePlanRequest request)
        {
            SavePurchasePlanResponse response = new SavePurchasePlanResponse();
            //生成主表数据
            var plan = this.Db.FirstOrDefault<PurPlan>(p => p.ID == request.ID);
            plan.MM = request.MM;
            plan.CreateTime = DateTime.Now;
            plan.PlanSubject = request.PlanSubject;
            plan.EID = request.EID;
            plan.ECode = request.ECode;
            plan.EName = request.EName;
            plan.state = 0;
            plan.StateEnum = "未完成";
            //生成明细表数据
            List<PurPlanDetail> planDetailList = new List<PurPlanDetail>();
            if (request.PurchsePlanList != null && request.PurchsePlanList.Count > 0)
            {
                foreach (var item in request.PurchsePlanList)
                {
                    PurPlanDetail planDetail = item.TranslateTo<PurPlanDetail>();
                    planDetail.Enabled = 1;
                    planDetailList.Add(planDetail);
                }
            }
            //开启事务统一修改
            using (var trans=this.Db.BeginTransaction())
            {
                this.Db.Update<PurPlan>(plan);
                this.Db.UpdateAll<PurPlanDetail>(planDetailList);
                trans.Commit();
                response.Success = true;
            }
            return response;
        }
    }
}
