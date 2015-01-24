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
    /// 生成购物车
    /// add by liuzhiqiang 2014/12/30
    /// </summary>
    public class CreatePurShoppingCartValidator : AbstractValidator<CreatePurShoppingCartRequest>
    {
        public override ValidationResult Validate(CreatePurShoppingCartRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //建档人ID
            if (instance.Eid==0)
            {
                result.Errors.Add(new ValidationFailure("Err_SysIDIsNull", Const.Err_SysIDIsNull, "Err_SysIDIsNull"));
                return result;
            }

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            //产品ID
            if (instance.ItemID == 0)
            {
                result.Errors.Add(new ValidationFailure("ERPID", Const.Err_ERPIDIsNull, "Err_ERPIDIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class CreatePurShoppingCartService : Service, IPost<CreatePurShoppingCartRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<CreatePurShoppingCartRequest> CreatePurShoppingCartValidator { get; set; }
       
       

        public object Post(CreatePurShoppingCartRequest request)
        {
            CreatePurShoppingCartResponse response = new CreatePurShoppingCartResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = CreatePurShoppingCartValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:生成购物车处理
          
            CreatePurShoppingCart(request,response);
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void CreatePurShoppingCart(CreatePurShoppingCartRequest request,CreatePurShoppingCartResponse response)
        {
            StringBuilder propertysb = new StringBuilder();
            var pscItemInfo = this.Db.FirstOrDefault<PurShoppingCart>(n => n.EID == request.Eid && n.ItemID == request.ItemID&& n.State ==1);
            if (pscItemInfo != null)//已选产品，累加数量和金额
            {
                pscItemInfo.SelectQty += request.SelectQty;
                pscItemInfo.Amt += request.SelectQty * pscItemInfo.Prc;
                pscItemInfo.CreateDate = DateTime.Now;
                Db.Save(pscItemInfo);
            }
            else//新增产品
            {
                //根据产品ID获取企业产品信息
                var eItemInfo = this.Db.FirstOrDefault<EnterpriseItem>(n => n.ID == request.ItemID);
                //企业产品信息转成购物车产品
                PurShoppingCart purShoppingCart = eItemInfo.TranslateTo<PurShoppingCart>();
                //新增产品信息
                purShoppingCart.ID = RecordIDService.GetRecordID(1);
                purShoppingCart.AccountID = request.AccountID;
                purShoppingCart.EID = request.Eid;
                purShoppingCart.EIDCode = request.EidCode;
                purShoppingCart.EIDName = request.EidName;
                purShoppingCart.ItemID = eItemInfo.ID;
                purShoppingCart.SelectQty = request.SelectQty;
                purShoppingCart.Prc = eItemInfo.SalPrc;
                purShoppingCart.Amt = eItemInfo.SalPrc * request.SelectQty;
                purShoppingCart.TotalAmt = purShoppingCart.Amt;
                //根据供应商企业标识获取企业名称和云ID
                var ndtechAcntSystem = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.ID == eItemInfo.AccountID);
                purShoppingCart.CompID = ndtechAcntSystem.CorpNum;
                purShoppingCart.CompName = ndtechAcntSystem.CompName;
                //根据产品ID获取属性
                List<EnterpriseItemAttribute> itemAttributeData = this.Db.Where<EnterpriseItemAttribute>(n => n.ItemID == request.ItemID);
                if (itemAttributeData != null)
                {
                    foreach (EnterpriseItemAttribute itemAttribute in itemAttributeData)
                    {
                        propertysb.Append(itemAttribute.AttributeClass + ":" + itemAttribute.AttributeValue + ",");
                    }

                }
                purShoppingCart.PropertyName = propertysb.ToString();
                purShoppingCart.State = 1;
                purShoppingCart.CreateDate = DateTime.Now;
                Db.Insert(purShoppingCart);
            }
            //更改总报价金额
            StringBuilder updatestr = new StringBuilder("select id,compid,amt from pur_shopping_cart where eid='" + request.Eid
                + "' and compid='" + request.Supply + "' and a=" + request.AccountID + " and state = 1");
            var updateList = this.Db.Query<PurShoppingCart>(string.Format(updatestr.ToString()));
            if (updateList != null && updateList.Count > 0)
            {
                decimal totalamt = 0;
                foreach (var upitem in updateList)
                {
                    totalamt += upitem.Amt;
                }
                using (var trans = this.Db.BeginTransaction())
                {
                    foreach (var upitem in updateList)
                    {
                        this.Db.Update<PurShoppingCart>(string.Format("totalamt={0}  where id = {1} ", totalamt, upitem.ID));
                    }
                    trans.Commit();
                }
            }

            response.Success = true;

        }

        #endregion
         
    }
}
