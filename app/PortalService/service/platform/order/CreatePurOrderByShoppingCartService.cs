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
    /// 生成订单
    /// add by liuzhiqiang 2014/12/25
    /// </summary>
    public class CreatePurOrderByShoppingCartValidator : AbstractValidator<CreatePurOrderByShoppingCartRequest>
    {
        public override ValidationResult Validate(CreatePurOrderByShoppingCartRequest instance)
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

    public class CreatePurOrderByShoppingCartService : Service, IPost<CreatePurOrderByShoppingCartRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<CreatePurOrderByShoppingCartRequest> CreatePurOrderByShoppingCartValidator { get; set; }
        //ArrayList categoryList = new ArrayList();
        //StringBuilder propertysb = new StringBuilder();
        //CreatePurOrderByShoppingCartResponse response = new CreatePurOrderByShoppingCartResponse();

        public object Post(CreatePurOrderByShoppingCartRequest request)
        {
            CreatePurOrderByShoppingCartResponse response = new CreatePurOrderByShoppingCartResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = CreatePurOrderByShoppingCartValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            CreatePurOrderByShoppingCart(request, response);


            return response;
        }

        #region 逻辑处理

        private void CreatePurOrderByShoppingCart(CreatePurOrderByShoppingCartRequest request,CreatePurOrderByShoppingCartResponse response )
        {
            //总计
            decimal qty = 0;

            //供应商
            //var compID = this.Db.Where<PurShoppingCart>(n => n.EID == request.Eid && n.State == 1).GroupBy(n => n.CompID);
            List<PurOrder> purOrderList = new List<PurOrder>();
            List<PurOrderDetail> purOrderDetailList = new List<PurOrderDetail>();
            List<PurShoppingCart> purShoppingCartList = new List<PurShoppingCart>();
            foreach (var item in request.SupplierList)
            {
                long recordID = RecordIDService.GetRecordID(1);
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

                List<string> itemIDs = new List<string>();
                foreach (var itemID in item.ItemIDList)
                {
                    itemIDs.Add(itemID.ItemID.ToString ());
                }
                string list = itemIDs.SqlInValues().ToSqlInString();
                //查询制单人等于登陆人，并且供应商等于前端传入供应商，并且范围为所选产品的金额合计
                var cartList = this.Db.Query<PurShoppingCart>(string.Format("select * from pur_shopping_cart where compid ='{0}' and eid = {1} and a = {2} and i in ({3}) and state = 1", item.SupplierID, request.Eid, request.AccountID, list));
                purOrder.TotalAmt = cartList.Sum(x => x.Amt);

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
                    //添加eid和state=1  modify 2015/01/14
                    var purShoppingCart = this.Db.FirstOrDefault<PurShoppingCart>(n => n.CompID == item.SupplierID && n.ItemID == pur.ItemID 
                        && n.State == 1 && n.EID == request.Eid);
                    if (purShoppingCart != null)
                    {
                        purShoppingCartList.Add(purShoppingCart);

                        purOrderDetail = purShoppingCart.TranslateTo<PurOrderDetail>();
                        purOrderDetail.ID = RecordIDService.GetRecordID(1);
                        purOrderDetail.MID = recordID;
                        purOrderDetail.SAccountID = sAccountID;
                        purOrderDetail.Quantity = purShoppingCart.SelectQty;
                        purOrderDetail.UnitCode = purShoppingCart.UnitName;
                        purOrderDetail.ShoppingCartdid = purShoppingCart.ID;
                        purOrderDetailList.Add(purOrderDetail);
                        qty += purOrderDetail.SelectQty;

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

                }

                trans.Commit();
            }


            response.Success = true;

        }
        #endregion

    }
}
