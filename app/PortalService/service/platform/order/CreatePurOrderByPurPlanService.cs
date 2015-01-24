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

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 采购申请生成订单
    /// add by liuzhiqiang 2015-01-17
    /// </summary>
    public class CreatePurOrderByPurPlanValidator : AbstractValidator<CreatePurOrderByPurPlanRequest>
    {
        public override ValidationResult Validate(CreatePurOrderByPurPlanRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //登录人 
            if (instance.Eid == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }


            return base.Validate(instance);
        }
    }

    public class CreatePurOrderByPurPlanService : Service, IPost<CreatePurOrderByPurPlanRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<CreatePurOrderByPurPlanRequest> CreatePurOrderByPurPlanValidator { get; set; }
        

        public object Post(CreatePurOrderByPurPlanRequest request)
        {
            CreatePurOrderByPurPlanResponse response = new CreatePurOrderByPurPlanResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = CreatePurOrderByPurPlanValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            CreatePurOrderByPurPlan(request, response);


            return response;
        }

        #region 逻辑处理

        private void CreatePurOrderByPurPlan(CreatePurOrderByPurPlanRequest request, CreatePurOrderByPurPlanResponse response)
        {
            //总计
            decimal qty = 0;

            //供应商
            List<PurOrder> purOrderList = new List<PurOrder>();
            List<PurOrderDetail> purOrderDetailList = new List<PurOrderDetail>();
            List<PurPlanDetail> purPlanDetailList = new List<PurPlanDetail>();
            long recordID = RecordIDService.GetRecordID(1);

            foreach (var item in request.SupplierList)
            {
                //订单主表
                PurOrder purOrder = new PurOrder();
                purOrder.ID = recordID;
                purOrder.AccountID = request.AccountID;
                purOrder.OrderCode = RecordSnumService.GetSnum(this, request.AccountID, SnumType.PurSelect);
                //根据供应商云ID查询企业信息
                var ndtechAcntSystem = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.CorpNum == item.SupplierID);
                int sAccountID = 0;
                if (ndtechAcntSystem != null)
                {
                    purOrder.SupplyCode = ndtechAcntSystem.CompCode;
                    purOrder.SupplyName = ndtechAcntSystem.CompName;
                    purOrder.SAccountID = ndtechAcntSystem.ID;
                    sAccountID = ndtechAcntSystem.ID;
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoSupplyInfoByID";
                    response.ResponseStatus.Message = Const.Err_NoSupplyInfoByID;
                    response.Success = false;
                    return;
                }

                //其它信息
                purOrder.EID = request.Eid;
                purOrder.EIDCode = request.EidCode;
                purOrder.EIDName = request.EidName;
                purOrder.CreateTime = DateTime.Now;
                purOrder.State = 0;
                purOrder.LinkManID = -1;

                //订单子表
                PurOrderDetail purOrderDetail = new PurOrderDetail();

                foreach (var pur in item.ItemIDList)
                {
                    var purPlanDetail = this.Db.FirstOrDefault<PurPlanDetail>(n => n.ID == pur.PlanDetailID);
                    if (purPlanDetail != null)
                    {
                        purPlanDetailList.Add(purPlanDetail);

                        purOrderDetail = purPlanDetail.TranslateTo<PurOrderDetail>();
                        purOrderDetail.ID = RecordIDService.GetRecordID(1);
                        purOrderDetail.MID = recordID;
                        purOrderDetail.SAccountID = sAccountID;
                        purOrderDetail.Plandid = purPlanDetail.ID;
                        purOrderDetail.UnitName = purPlanDetail.UnitCode;
                        purOrderDetailList.Add(purOrderDetail);
                        qty += purOrderDetail.Quantity;

                    }
                }

                //保存供应商档案
                NdtechCompany company = Db.FirstOrDefault<NdtechCompany>(n => n.AccountID == sAccountID);
                if (company != null)
                {
                    UdocSupplier supplier = Db.QuerySingle<UdocSupplier>(string.Format("SELECT * FROM udoc_supplier WHERE Comp ={0}", company.ID));
                    if (supplier != null)
                    {
                        //反写订单供应商档案ID
                        purOrder.Supply = supplier.ID;
                    }

                }
                purOrder.Quantity = qty;
                //累加订单主档
                purOrderList.Add(purOrder);
                qty = 0;
            }

            using (var trans = Db.BeginTransaction())
            {
                if (purOrderList.Count > 0)
                {
                    foreach (var purOrder in purOrderList)
                    {
                        List<PurOrderDetail> purOrderDetails = purOrderDetailList.Where(x => x.MID == purOrder.ID).ToList();
                        foreach (var purOrderDetail in purOrderDetails)
                        {
                            //新增明细表
                            this.Db.Insert(purOrderDetail);
                        }
                        //新增订单主表
                        this.Db.Save(purOrder);
                    }
                    foreach (var purPlanDetail in purPlanDetailList)
                    {
                        //反写采购申请产品状态为已下单 liuzhiqiang 2015-01-17
                        if (purPlanDetail.ID != 0)
                        {
                            this.Db.Update<PurPlanDetail>(string.Format("pland_state = 2,pland_state_enum='已下单' where pland_id = {0}", purPlanDetail.ID));
                        }

                    }
                    //反写采购申请主表状态为已完成 liuzhiqiang 2015-01-20
                    if (request.PlanID != 0)
                    {
                        var list = this.Db.Where<PurPlanDetail>(n => n.state == 0 && n.MID == request.PlanID);
                        if (list.Count==0)
                        {
                            this.Db.Update<PurPlan>(string.Format("plan_state = 1,plan_state_enum='已完成' where plan_id = {0}", request.PlanID));
                        }
                    }
                }
                 
                trans.Commit();
            }
            PurEvaluationID pe = new PurEvaluationID();
            pe.ID = recordID;
            response.Data = pe;
            response.Success = true;

        }
        #endregion

    }
}
