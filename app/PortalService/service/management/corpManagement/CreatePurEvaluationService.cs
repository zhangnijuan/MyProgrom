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
    /// 生成评价
    /// add by liuzhiqiang 2014/12/25
    /// </summary>
    public class CreatePurEvaluationValidator : AbstractValidator<CreatePurEvaluationRequest>
    {
        public override ValidationResult Validate(CreatePurEvaluationRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //订单 
            if (instance.OrderID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_PurOrderIDIsNull", Const.Err_PurOrderIDIsNull, "Err_PurOrderIDIsNull"));
                return result;
            }


            return base.Validate(instance);
        }
    }

    public class CreatePurEvaluationService : Service, IPost<CreatePurEvaluationRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<CreatePurEvaluationRequest> CreatePurEvaluationValidator { get; set; }

        CreatePurEvaluationResponse response = new CreatePurEvaluationResponse();
        public object Post(CreatePurEvaluationRequest request)
        {
            CreatePurEvaluationResponse response = new CreatePurEvaluationResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = CreatePurEvaluationValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            CreatePurEvaluation(request,response);


            return response;
        }

        #region 逻辑处理

        private void CreatePurEvaluation(CreatePurEvaluationRequest request,CreatePurEvaluationResponse response)
        {
            List<PurEvaluation> purEvaluationList = new List<PurEvaluation>();
            List<PurEvaluationDetail> purEvaluationDetailList = new List<PurEvaluationDetail>();

            long recordID = RecordIDService.GetRecordID(1);
            PurEvaluation purEvaluation = request.TranslateTo<PurEvaluation>();
            purEvaluation.AccountID = request.AccountID;
            purEvaluation.Code = RecordSnumService.GetSnum(this, request.AccountID, SnumType.PurEvaluation);
            purEvaluation.Ename = request.Ename;
            purEvaluation.MM = request.MM;
            purEvaluation.OrderID = request.OrderID;
            purEvaluation.ID = recordID;
            purEvaluation.CreateTime = DateTime.Now;
            purEvaluationList.Add(purEvaluation);

            foreach (var evaluation in request.EvaluationList)
            {
                PurEvaluationDetail purEvaluationDetail = evaluation.TranslateTo<PurEvaluationDetail>();
                purEvaluationDetail.ID = RecordIDService.GetRecordID(1);
                purEvaluationDetail.MID = recordID;
                //累加评价明细
                purEvaluationDetailList.Add(purEvaluationDetail);
            }
            using (var trans = Db.BeginTransaction())
            {
                if (purEvaluationList.Count > 0)
                {
                    foreach (var purEvaluations in purEvaluationList)
                    {
                        List<PurEvaluationDetail> purEvaluationDetails = purEvaluationDetailList.Where(x => x.MID == purEvaluations.ID).ToList();
                        foreach (var purEvaluationDetail in purEvaluationDetails)
                        {
                            //新增明细表
                            this.Db.Insert(purEvaluationDetail);
                        }
                        //新增主表
                        this.Db.Save(purEvaluations);
                    }
                 
                }
                 
                trans.Commit();
            }


            response.Success = true;

        }
        #endregion

    }
}
