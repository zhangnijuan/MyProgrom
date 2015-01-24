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
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 生成单证操作日志表
    /// add by liuzhiqiang 2015/1/8
    /// </summary>
    public class GetStateLogValidator : AbstractValidator<GetStateLogRequest>
    {
        public override ValidationResult Validate(GetStateLogRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //订单 
            if (string.IsNullOrEmpty(instance.SRCID))
            {
                result.Errors.Add(new ValidationFailure("Err_PurOrderIDIsNull", Const.Err_PurOrderIDIsNull, "Err_PurOrderIDIsNull"));
                return result;
            }


            return base.Validate(instance);
        }
    }

    public class GetStateLogSercice : Service, IGet<GetStateLogRequest>
    {
        public IValidator<GetStateLogRequest> GetStateLogValidator { get; set; }

        public object Get(GetStateLogRequest request)
        {
            GetStateLogResponse response = new GetStateLogResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = GetStateLogValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            var orderStateLog = this.Db.FirstOrDefault<OrderStateLog>(string.Format("select id,sid,eid,eid_syscode,eid_usrname,createdate,state,firstmm,secondmm from udoc_statelog  where sid='{0}' order by createdate desc", request.SRCID)); 
            response.Data = orderStateLog;
            response.Success = true;

            return response;
        }

    }
}
