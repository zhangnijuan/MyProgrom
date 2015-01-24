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
    public class CreatePurOrderValidator : AbstractValidator<CreatePurOrderRequest>
    {
        public override ValidationResult Validate(CreatePurOrderRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //询价ID
            if (string.IsNullOrEmpty(instance.InquiryID.ToString ()))
            {
                result.Errors.Add(new ValidationFailure("InquiryID", Const.Err_InquiryIDIsNull, "Err_InquiryIDIsNull"));
                return result;
            }

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            //优选单ID
            if (string.IsNullOrEmpty(instance.PurSelectID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("SelectID", Const.Err_SelectIDIsNull, "Err_SelectIDIsNull"));
                return result;
            }
      
            return base.Validate(instance);
        }
    }

    public class CreatePurOrderService : Service, IPost<CreatePurOrderRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<CreatePurOrderRequest> CreatePurOrderValidator { get; set; }
        //ArrayList categoryList = new ArrayList();
        //StringBuilder propertysb = new StringBuilder();
        

        public object Post(CreatePurOrderRequest request)
        {
            CreatePurOrderResponse response = new CreatePurOrderResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = CreatePurOrderValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:优选生成订单处理
            if (request.Source == SourceEnum.Select)
            {
                CreatePurOrder(request, response);
            }
           
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void CreatePurOrder(CreatePurOrderRequest request, CreatePurOrderResponse response)
        {
            //总计
            decimal  qty = 0;
            //优选主表
            var purSelect = this.Db.FirstOrDefault<PurSelect>(n => n.ID == request.PurSelectID);
            if (purSelect != null)
            {
                //供应商
                var compID = this.Db.Where<PurSelectResults>(n => n.MID == request.PurSelectID).GroupBy(n => n.CompID);
                var purInquiry = this.Db.FirstOrDefault<PurInquiry>(n => n.ID == request.InquiryID);
                List<PurOrder> purOrderList = new List<PurOrder>();
                List<PurOrderDetail> purOrderDetailList = new List<PurOrderDetail>();
                    foreach (var item in compID)
                    {
                        long recordID = RecordIDService.GetRecordID(1);
                        //订单主表
                        PurOrder purOrder = new PurOrder();
                        purOrder.ID = recordID;
                        purOrder.AccountID = purSelect.AccountID;
                        purOrder.Subject = purSelect.Subject;
                        purOrder.OrderCode = RecordSnumService.GetSnum(this, request.AccountID, SnumType.PurSelect);
                        //根据供应商云ID查询企业信息
                        var ndtechAcntSystem = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.CorpNum == item.Key);
                        int sAccountID = 0;
                        if (ndtechAcntSystem != null)
                        {
                            purOrder.SupplyCode = ndtechAcntSystem.CorpNum;
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
                        //根据询价表查询采购方联系人
                       
                        if (purInquiry != null)
                        {
                            purOrder.SysName = purInquiry.Linkman;
                            purOrder.Mobile = purInquiry.Mobile;
                            purOrder.EmailInfo = purInquiry.EmailInfo;
                            purOrder.Address = purInquiry.Address;
                            purOrder.AddressID = purInquiry.AddressID;
                            //反写询价表状态
                            purInquiry.State = 5;
                            purInquiry.State_Enum = "已下单";
                        }
                        else
                        {
                            response.ResponseStatus.ErrorCode = "Err_NoInquiryInfoByID";
                            response.ResponseStatus.Message = Const.Err_NoInquiryInfoByID;
                            response.Success = false;
                            return;

                        }
                        //优选子表查询总计 
                        var totalAmt = this.Db.FirstOrDefault<PurSelectResults>(n => n.CompID == item.Key && n.MID == request.PurSelectID);
                        purOrder.TotalAmt = totalAmt.TotalAmt;
                        //根据询价单ID和供应商企业标识查询供应方报价人
                        StringBuilder SearchConditionsql = new StringBuilder("select eid,eid_syscode,eid_usrname from sal_quotation  where inquiryid=" + request.InquiryID + " and a=" + sAccountID);
                        SalQuotation salQuotation = this.Db.QuerySingle<SalQuotation>(SearchConditionsql.ToString());
                        if (salQuotation != null)
                        {
                            purOrder.LinkManID = salQuotation.Eid;
                            purOrder.LinkManCode = salQuotation.EidCode;
                            purOrder.LinkManName = salQuotation.EidName;
                        }
                        else
                        {
                            response.ResponseStatus.ErrorCode = "Err_NoQuotationInfoByID";
                            response.ResponseStatus.Message = Const.Err_NoQuotationInfoByID;
                            response.Success = false;
                            return;
                        }
                        //其它信息
                        purOrder.State = 0;
                        purOrder.EID = request.Eid;
                        purOrder.EIDCode = request.EidCode;
                        purOrder.EIDName = request.EidName;
                        purOrder.RoleID = request.RoleID;
                        purOrder.Role_Enum = request.Role_Enum;
                        purOrder.CreateTime = DateTime.Now;

                        //订单子表
                        var purSelectResults = this.Db.Where<PurSelectResults>(n => n.CompID == item.Key && n.MID == request.PurSelectID);
                        PurOrderDetail purOrderDetail = new PurOrderDetail();

                        foreach (var pur in purSelectResults)
                        {
                            purOrderDetail = pur.TranslateTo<PurOrderDetail>();
                            purOrderDetail.ID = RecordIDService.GetRecordID(1);
                            purOrderDetail.MID = recordID;
                            purOrderDetail.SAccountID = sAccountID;
                            purOrderDetail.Quantity = pur.SelectQty;
                            purOrderDetailList.Add(purOrderDetail);
                            qty += purOrderDetail.SelectQty;
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
                            if (purInquiry != null)
                            {
                                //更新询价单
                                Db.Save(purInquiry);
                            }
                        }
                        
                        trans.Commit();
                    }
                    
                //}
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoSelectInfoByID";
                response.ResponseStatus.Message = Const.Err_NoSelectInfoByID;
                response.Success = false;
                return;
            }
            response.Success = true;

        }

        #endregion

    }
}
