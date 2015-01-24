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
    public class EidtPurchasePlanByIdService : Service,IGet<EidtPurchasePlanByIdRequest>
    {
   
        /// <summary>
        /// 根据id获取采购申请详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(EidtPurchasePlanByIdRequest request)
        {
            EidtPurchasePlanByIdResponse response = new EidtPurchasePlanByIdResponse();
            var plan = this.Db.FirstOrDefault<PurPlan>(p => p.ID == request.ID);
            PurchasePlan purPlan = null;
            if (plan!=null)
            {
                purPlan = plan.TranslateTo<PurchasePlan>();
            }
            List<PurchasePlanDetail> list = this.Db.Query<PurchasePlanDetail>(string.Format("select * from pur_plandetail where pland_mid={0}", request.ID));
            response.Success = true;
            response.Data = new ReturnData() { PurPlan = purPlan, PurPlanDetailList = list }; 
            return response;
        }
    }
}
