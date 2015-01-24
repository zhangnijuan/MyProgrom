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
{/// <summary>
    /// 生成订单
    /// add by liuzhiqiang 2014/12/25
    /// </summary>
    public class DeletePurShoppingCartItemValidator : AbstractValidator<DeletePurShoppingCartItemRequest>
    {
        public override ValidationResult Validate(DeletePurShoppingCartItemRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (instance.DeleteCondition != null)
            {
                foreach (var item in instance.DeleteCondition)
                {
                    //帐套
                    if (string.IsNullOrEmpty(item.AccountID.ToString()))
                    {
                        result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountID, "Err_AccountID"));
                        return result;
                    }

                    //标识ID
                    if (string.IsNullOrEmpty(item.ID.ToString()))
                    {
                        result.Errors.Add(new ValidationFailure("ID", Const.Err_IDIsNull, "Err_IDIsNull"));
                        return result;
                    }
                    ////产品ID
                    //if (string.IsNullOrEmpty(item..ToString()))
                    //{
                    //    result.Errors.Add(new ValidationFailure("ItemID", Const.Err_ERPIDIsNull, "Err_ERPIDIsNull"));
                    //    return result;
                    //}
                    //建档人
                    if (string.IsNullOrEmpty(item.EID.ToString()))
                    {
                        result.Errors.Add(new ValidationFailure("EID", Const.Err_EIDIsNull, "Err_EIDIsNull"));
                        return result;
                    }
                    ////供应企业云ID
                    //if (string.IsNullOrEmpty(item.CompID.ToString()))
                    //{
                    //    result.Errors.Add(new ValidationFailure("CompID", Const.Err_CompIDIsNull, "Err_CompIDIsNull"));
                    //    return result;
                    //}
                }

            }
            ////数量
            //if (instance.SelectQty != null && instance.SelectQty.Trim()!="")
            //{
            //    result.Errors.Add(new ValidationFailure("SelectQty", Const.Err_AmountIsNull, "Err_SelectQtyIsNull"));
            //    return result;
            //}

            return base.Validate(instance);
        }
    }

    public class DeletePurShoppingCartItemService : Service, IPost<DeletePurShoppingCartItemRequest>
    {
        public IValidator<DeletePurShoppingCartItemRequest> DeletePurShoppingCartItemValidator { get; set; }
        //ArrayList categoryList = new ArrayList();
        //StringBuilder propertysb = new StringBuilder();
        

        public object Post(DeletePurShoppingCartItemRequest request)
        {
            DeletePurShoppingCartItemResponse response = new DeletePurShoppingCartItemResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = DeletePurShoppingCartItemValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:删除购物车产品
            DeletePurShoppingCartItem(request, response);//, responseTemp

            return response;

        }

        #region 逻辑处理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void DeletePurShoppingCartItem(DeletePurShoppingCartItemRequest request, DeletePurShoppingCartItemResponse response)//, DeletePurShoppingCartItemResponse responseTemp
        {
            //修改物品数量
            if (request.SelectQty != null && request.SelectQty.Trim() != "")
            {

                ModifyItem(request,response);

            }
            else
            {
                //删除购物车物品
                DeleteAllItem(request,response);
            }


        }

        private void DeleteAllItem(DeletePurShoppingCartItemRequest request, DeletePurShoppingCartItemResponse response)
        {
            if (request.DeleteCondition.Count > 0)
            {
                string updateSql = "state=0  where  id in ({0}) ";
                List<string> itemIDs = new List<string>();
                foreach (var item in request.DeleteCondition)
                {
                    itemIDs.Add(item.ID);
                    //compIDs.Add(item.CompID);
                }

                 this.Db.Update<PurShoppingCart>(string.Format(updateSql,itemIDs.SqlInValues().ToSqlInString()));
                 response.Success = true;
            }
        }

        private void ModifyItem(DeletePurShoppingCartItemRequest request, DeletePurShoppingCartItemResponse response)
        {
            decimal amt = 0;
            if (string.IsNullOrEmpty(request.Amt))
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = "Err_AmtIsNull";
                response.ResponseStatus.Message = Const.Err_AmtIsNull;
                return;
            }
            decimal.TryParse(request.Amt, out amt);
            if (amt == 0)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = "Err_AmtIsNull";
                response.ResponseStatus.Message = Const.Err_AmtIsNull;
                return;
            }
            List<string> updateList = new List<string>();
            string updateSql = "sqty={3},amt={4} where  eid='{0}' and a='{1}'and state=1 and id = {2} ";
            //修改物品的数量和金额 及时提交到数据库
            this.Db.Update<PurShoppingCart>(string.Format(updateSql,request.EID,request.AccountID,request.ID,request.SelectQty,request.Amt));
            response.Success = true;
        }

        #endregion

    }
}
