using Ndtech.PortalModel;
using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using Ndtech.PortalService.SystemService;

namespace Ndtech.PortalService.Auth
{
    public class ModifyPurOrderStateValidator : AbstractValidator<ModifyPurOrderStateRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(ModifyPurOrderStateRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //if (string.IsNullOrEmpty(instance.State.ToString()))
            //{
            //    result.Errors.Add(new ValidationFailure("State", Const.Err_StateIsNull, "Err_Err_StateIsNull"));
            //    return result;
            //}
            if (string.IsNullOrEmpty(instance.AccountId.ToString()))
            {
                result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountIDIsNull, "Err_AccountIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.counterparty.ToString()))
            {
                result.Errors.Add(new ValidationFailure("CounterParty", Const.Err_CounterPartyIsNull, "Err_CounterPartyIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.PurOrder.ID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("ID", Const.Err_PurOrderIDIsNull, "Err_PurOrderIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Eid))
            {
                result.Errors.Add(new ValidationFailure("Eid", Const.Err_PurOrderEidIsNull, "Err_PurOrderEidIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.ECode))
            {
                result.Errors.Add(new ValidationFailure("ECode", Const.Err_PurOrderECodeIsNull, "Err_PurOrderECodeIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.EName))
            {
                result.Errors.Add(new ValidationFailure("EName", Const.Err_PurOrderENameIsNull, "Err_PurOrderENameIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }
    public class ModifyPurOrderStateService : Service, IPost<ModifyPurOrderStateRequest>
    {
        public IValidator<ModifyPurOrderStateRequest> ModifyPurOrderStateValidator { get; set; }
        public object Post(ModifyPurOrderStateRequest request)
        {
            ModifyPurOrderStateResponse response = new ModifyPurOrderStateResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = ModifyPurOrderStateValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:逻辑处理
            if (request.PurOrder != null)
            {
                StringBuilder SearchSql = new StringBuilder("select * from pur_order where 1=1 and id=" + request.PurOrder.ID);
                StringBuilder UpdateStr = new StringBuilder();
                StringBuilder AddressStr = new StringBuilder();


                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方
                {
                    SearchSql.Append(" and sa=" + request.AccountId);
                    UpdateStr.Append(" sa=");
                    AddressStr.Append("saddressid={0},saddress='{1}'");
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())
                {
                    SearchSql.Append(" and a=" + request.AccountId);//采购方
                    UpdateStr.Append(" a=");
                    AddressStr.Append("addressid={0},address='{1}'");
                }
                PurOrder purOrder = this.Db.QuerySingle<PurOrder>(string.Format(SearchSql.ToString())); //订单主表

                List<PurOrderDetail> purOrderDetailList = null;
                if (request.PurOrder.PurorderDetail != null && request.PurOrder.PurorderDetail.Count > 0)
                {
                    purOrderDetailList=this.Db.Where<PurOrderDetail>(n => n.MID == purOrder.ID); ;//订单明细(string.Format("mid = '{0}'",purOrder.ID))
                }
                

                List<Resources> detailRes = new List<Resources>();//明细附件

                List<SalOutNoticeDetail> noticeDetail = new List<SalOutNoticeDetail>(); //收发货记录
               

                #region 修改主表信息
                
             
                ModifyRootTable(request, response, purOrder, detailRes);
                if (!response.Success)
                {
                    return response;
                }
                #endregion

                #region 修改明细数据
                
                
                ModifyDetailTable(request, response,purOrder, purOrderDetailList, detailRes, noticeDetail);
                if (!response.Success)
                {
                    return response;
                }
                #endregion

                #region 添加收付款记录
                
               
                AddArapRecord(request, purOrder,noticeDetail);
                #endregion

                #region 添加收发货记录
                AddNoticeDetail(request, purOrder, noticeDetail);
                #endregion




                response.Success = true;
            }
            
            return response;
        }

        private void AddArapRecord(ModifyPurOrderStateRequest request, PurOrder purOrder, List<SalOutNoticeDetail> noticeDetail)
        {
            ArapReceiving arap = null;//收付款信息
            #region 新增收付款记录

            //收付款信息
            if (request.PurOrder.Amt != null && request.PurOrder.Amt.Trim() != "")
            {
                #region  付款收款记录
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方收款信息
                {
                    //收款信息
                    arap = new ArapReceiving();
                    arap.ID = RecordIDService.GetRecordID(1);
                    //arap.AccountID = purorderitem.AccountID;
                    arap.SAccountID = purOrder.SAccountID;
                    arap.OrderID = long.Parse(request.PurOrder.ID);
                    arap.Collection = decimal.Parse(request.PurOrder.Amt);
                    arap.MM = request.PurOrder.ArapMM;
                    arap.CreateDate = DateTime.Now;
                    arap.State = 1;

                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方付款信息
                {
                    //付款信息
                    arap = new ArapReceiving();
                    arap.ID = RecordIDService.GetRecordID(1);
                    arap.AccountID = purOrder.AccountID;
                    //arap.SAccountID = purorderitem.SAccountID;
                    arap.OrderID = long.Parse(request.PurOrder.ID);
                    arap.Payment = decimal.Parse(request.PurOrder.Amt);
                    arap.MM = request.PurOrder.ArapMM;
                    arap.CreateDate = DateTime.Now;//付款日期
                    arap.State = 0;//未收款
                }
                #endregion
                //再把收款记录以及订单状态的修改在同一个数据库事务中提交
                using (var trans = this.Db.BeginTransaction())
                {
                    if (arap != null)
                    {
                        this.Db.Insert<ArapReceiving>(arap);
                        if (request.BizType == BizTypeEnum.ReceConfrim && arap.Collection > 0)
                        {
                            //供应方收款后更新采购方付款状态为1
                            this.Db.Update<ArapReceiving>(string.Format("state=1  where orderid = {0} and createdate<'{1}' and state=0 ", request.PurOrder.ID, arap.CreateDate));
                            //更新订单明细累计收款金额
                            this.Db.Update<PurOrderDetail>(string.Format("receivingamt= receivingamt+{0} where mid = {1}", request.PurOrder.Amt, request.PurOrder.ID));

                            //供应方收款后更新状态为已收款
                            this.Db.Update<PurOrder>(string.Format(" receivingstate = 2 where  id = {0} and (receivingstate=1 or receivingstate = 11)", purOrder.ID));
                            //供应方收款后更新流转状态为未流转
                            this.Db.Update<PurOrder>(string.Format("circulationstate = 0 where  id = {0}", purOrder.ID));
                            
                        }
                        if (request.BizType == BizTypeEnum.PayConfrim &&  arap.Payment > 0)
                        {
                            //付款后更新订单状态为 待收款
                            this.Db.Update<PurOrder>(string.Format(" receivingstate = 11 where  id = {0} and receivingstate = 2", purOrder.ID));
                            this.Db.Update<PurOrder>(string.Format(" receivingstate = 1 where  id = {0} and receivingstate = 0", purOrder.ID));
                            //付款后更新订单状态为 交易中待收款
                            this.Db.Update<PurOrder>(string.Format("circulationstate = 2 where  id = {0}", purOrder.ID));
                        }
                    }
                    trans.Commit();

                }
            }
            
            #endregion
        }



        private void AddNoticeDetail(ModifyPurOrderStateRequest request, PurOrder purOrder, List<SalOutNoticeDetail> noticeDetail)
        {
            if (noticeDetail.Count > 0)
            {
                using (var trans = this.Db.BeginTransaction())
                {
                    this.Db.InsertAll<SalOutNoticeDetail>(noticeDetail);
                    if (request.BizType == BizTypeEnum.ArrivalConfrim)
                    {
                        foreach (SalOutNoticeDetail item in noticeDetail)
                        {

                            //把收发货表中供应方的状态改为已收1
                            this.Db.Update<SalOutNoticeDetail>(string.Format("state=1  where detailid = {0} and createdate<'{1}' and state=0 ", item.DetailID, item.CreateDate));
                            //更新明细表累计到货数量
                            this.Db.Update<PurOrderDetail>(string.Format("arrivalqty =arrivalqty +{0} where id = {1}", item.ArrivalQty, item.DetailID));
      

                        }
                    }
                    trans.Commit();
                }
            }
        }

        private  void ModifyDetailTable(ModifyPurOrderStateRequest request, ModifyPurOrderStateResponse response,PurOrder purOrder, List<PurOrderDetail> purOrderDetailList, List<Resources> detailRes, List<SalOutNoticeDetail> noticeDetail)
        {
            #region 修改明细数据
            if (request.PurOrder.PurorderDetail != null && request.PurOrder.PurorderDetail.Count > 0)//purOrderDetailList != null
            {
                
                foreach (PurOrderDetail detailItem in purOrderDetailList)//
                {
                    PurorderDetailView detailView = request.PurOrder.PurorderDetail.Find(delegate(PurorderDetailView d) { return d.ID == detailItem.ID; });
                    //采购方确认订单校验明细数量不为0
                    if (request.BizType == BizTypeEnum.PurConfrim)
                    {
                        decimal qty = 0; //订单数量 默认=询价数量
                        decimal.TryParse(detailView.PurQuantity, out qty);
                        if (qty == 0)
                        {
                            response.Success = false;
                            response.ResponseStatus.ErrorCode = "Err_PurOrderQtyIsNull";
                            response.ResponseStatus.Message = Const.Err_PurOrderQtyIsNull;
                            break;
                        }

                        detailItem.Quantity = qty;
                        decimal prc = 0; //单价
                        decimal.TryParse(detailView.PurPrc, out prc);
                        detailItem.Prc = prc;
                        decimal amt = 0; //金额
                        decimal.TryParse(detailView.PurAmt, out amt);
                        detailItem.Amt = amt == 0 ? qty * prc : amt;
                        detailItem.MM = detailView.PurMM;//备注
                        if (!string.IsNullOrEmpty(detailView.PurDeliveryDate))
                        {
                            detailItem.DeliveryDate = Convert.ToDateTime(detailView.PurDeliveryDate);
                        }
                        //采购方确认订单 保存上传附件
                        if (detailView.FileUploadData != null)
                        {
                            foreach (var resitem in detailView.FileUploadData)
                            {
                                Resources pic = new Resources();

                                //插入资源表中信息
                                pic.Id = RecordIDService.GetRecordID(1);
                                pic.DocumentID = detailItem.ID;
                                pic.AccountID = detailItem.AccountID;//订单主表的供应方账套
                                pic.OriginalName = resitem.OriginalName;
                                pic.NewName = resitem.NewName;
                                pic.FileLength =resitem.FileLength;
                                pic.Suffix = resitem.Suffix;
                                detailRes.Add(pic);
                            }
                        }
                    }
                    //供应方发货校验发货数量不为0

                    if (request.BizType == BizTypeEnum.OutConfrim || request.BizType == BizTypeEnum.ArrivalConfrim)
                    {
                        decimal outqty = 0; //发货数量 
                        decimal.TryParse(detailView.Quantity, out outqty);
                        if (outqty == 0)
                        {
                            response.Success = false;
                            response.ResponseStatus.ErrorCode = "Err_PurOrderOutQtyIsNull";
                            response.ResponseStatus.Message = Const.Err_PurOrderOutQtyIsNull;
                            break;
                        }
                        SalOutNoticeDetail sal = new SalOutNoticeDetail();
                        //供应方发货
                        if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper() && outqty>0)
                        {
                            //发货后更改订单主表状态为 待收货
                            purOrder.ArrivalState = purOrder.ArrivalState == 2 ? 11 : 1;
                            //发货更新订单流转状态 为 交易中待收货
                            purOrder.CirculationState = 1;
                            sal.ID = RecordIDService.GetRecordID(1);
                            sal.SAccountID = request.AccountId;
                            //sal.AccountID = purdet.AccountID;
                            sal.DetailID = detailItem.ID;
                            sal.DeliveryQty = outqty;
                            sal.MM = detailView.OutMM;
                            sal.CreateDate = DateTime.Now;
                            sal.State = 0;//未收货
                          
                            noticeDetail.Add(sal);
                        }
                        //采购方收货
                        if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper() && outqty>0)
                        {
        
                            //收货后，更改订单主表的Arrivalstate状态
                            purOrder.ArrivalState = 2;
                            //收货后更改订单主表的流转状态
                            purOrder.CirculationState = 0;
                            sal.ID = RecordIDService.GetRecordID(1);
                            sal.AccountID = request.AccountId;
                            //sal.SAccountID = purdet.SAccountID;
                            sal.DetailID = detailItem.ID;
                            sal.ArrivalQty = outqty;
                            sal.MM = detailView.OutMM;
                            sal.CreateDate = DateTime.Now;//收货日期
                            sal.State = 1;//已收货
                            noticeDetail.Add(sal);
                        }
                    }
                }
            }
            //先把订单的修改结果提交到数据库
            using (var trans = this.Db.BeginTransaction())
            {
                this.Db.Update<PurOrder>(purOrder);
                if (purOrderDetailList != null)
                {
                    foreach (PurOrderDetail item in purOrderDetailList)
                    {
                        this.Db.Update<PurOrderDetail>(item);
                    }
                }
                foreach (Resources item in detailRes)
                {
                    this.Db.Insert<Resources>(item);
                }
                #region 添加操作日志
                if (request.BizType == BizTypeEnum.SalCancel || request.BizType == BizTypeEnum.PurCancel || request.BizType == BizTypeEnum.SalComplete || request.BizType == BizTypeEnum.PurComplete)
                {
                    StateLog statelog = new StateLog();
                    statelog.ID = RecordIDService.GetRecordID(1);
                    statelog.SRCID = purOrder.ID;
                    statelog.EID = long.Parse(request.Eid);
                    statelog.ECODE = request.ECode;
                    statelog.ENAME = request.EName;
                    statelog.CreateDate = DateTime.Now;
                    statelog.FirstMM = request.PurOrder.CancelMM;
                    statelog.SecondMM = "";
                    statelog.State = purOrder.State;
                    this.Db.Insert<StateLog>(statelog);
                }
                #endregion
                if (request.BizType == BizTypeEnum.PurConfrim)
                {
                    #region  采购方确认订单删除购物车数据
                    foreach (PurOrderDetail item in purOrderDetailList)
                    {
                        if (item.ShoppingCartdid != 0)
                        {
                            this.Db.Delete<PurShoppingCart>(s => s.Where(q => q.ID == item.ShoppingCartdid));
                        }
                    }

                    #endregion
                }
                trans.Commit();
            }
            response.Success = true;
            #endregion
        }

        private void ModifyRootTable(ModifyPurOrderStateRequest request, ModifyPurOrderStateResponse response, PurOrder purOrder, List<Resources> detailRes)
        {
            #region 状态修改
            if (!string.IsNullOrEmpty(request.PurOrder.State.ToString()) && request.PurOrder.State != StateEnum.StateStart)
            {

                string state = "0";//默认状态
                string state_enum = "订单草稿";
                switch (request.PurOrder.State)
                {
                    case StateEnum.StateStart: state = "0"; state_enum = "订单草稿";
                        break;
                    case StateEnum.StateTemp: state = "1"; state_enum = "待供应方确认";
                        break;
                    case StateEnum.StateMiddle: state = "2"; state_enum = "交易中";
                        break;
                    case StateEnum.StateCancel: state = "3"; state_enum = "已取消";
                        break;
                    case StateEnum.StateEnd: state = "4"; state_enum = "已完成";
                        break;
                }
                purOrder.State = int.Parse(state);
                purOrder.State_Enum = state_enum;
                if (request.BizType == BizTypeEnum.PurConfrim)//采购方确认订单
                {
                    purOrder.OrderTime = DateTime.Now;

                }

            }
            #endregion
            #region 修改联系人信息

            //更新主表联络人信息
            if (request.PurOrder.LinkName != null && request.PurOrder.LinkName.Trim() != "")//!string.IsNullOrEmpty(listpurorder.LinkName.ToString())
            {
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方
                {
                    purOrder.SLinkName = request.PurOrder.LinkName;
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方
                {
                    purOrder.SysName = request.PurOrder.LinkName;

                }

            }
            if (request.PurOrder.Mobile != null && request.PurOrder.Mobile.Trim() != "")//!string.IsNullOrEmpty(listpurorder.Mobile.ToString())
            {
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                {
                    purOrder.SMobile = request.PurOrder.Mobile;
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())
                {
                    purOrder.Mobile = request.PurOrder.Mobile;
                }

            }
            if (request.PurOrder.EmailInfo != null && request.PurOrder.EmailInfo.Trim() != "")//!string.IsNullOrEmpty(listpurorder.EmailInfo.ToString())
            {
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())
                {
                    purOrder.SEmailInfo = request.PurOrder.EmailInfo;
                }
                if (request.counterparty.ToString().ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())
                {
                    purOrder.EmailInfo = request.PurOrder.EmailInfo;
                }

            }
            if (request.BizType == BizTypeEnum.SalConfrim || request.BizType == BizTypeEnum.SalCancel)//供应方确认订单时,更新供应方报价人信息
            {
                purOrder.LinkManID =long.Parse(request.Eid);
                purOrder.LinkManCode = request.ECode;
                purOrder.LinkManName = request.EName;

            }
            if (request.BizType == BizTypeEnum.PurConfrim||  request.BizType == BizTypeEnum.PurCancel)
            {
                purOrder.EID = long.Parse(request.Eid);
                purOrder.EIDCode = request.ECode;
                purOrder.EIDName = request.EName;
                purOrder.LinkManID = 0;
                purOrder.LinkManCode = string.Empty;
                purOrder.LinkManName = string.Empty;
            }
            #endregion

            #region 修改主表信息
            //付款方式
            if (request.PurOrder.PayType != null && request.PurOrder.PayType.ToString().Trim() != "")//!string.IsNullOrEmpty(listpurorder.PayType.ToString())
            {
                if (request.PurOrder.PayType.Length < 32)
                {
                    purOrder.PayType = request.PurOrder.PayType;
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Error_PayType";
                    response.ResponseStatus.Message = "付款方式长度过长";
                    response.Success = false;
                    //break;
                }

            }
            //发票类型
            if (request.PurOrder.InvoiceType != null && request.PurOrder.InvoiceType.Trim() != "")//!string.IsNullOrEmpty(listpurorder.InvoiceType.ToString())
            {
                purOrder.InvoiceType = request.PurOrder.InvoiceType;

            }
            //税率
            if (request.PurOrder.TaxRate != null && request.PurOrder.TaxRate.Trim() != "")//!string.IsNullOrEmpty(listpurorder.TaxRate.ToString())
            {
                purOrder.TaxRate = decimal.Parse(request.PurOrder.TaxRate);
            }
            //质检信息
            if (request.PurOrder.CheckType != null && request.PurOrder.CheckType.ToString().Trim() != "")//!string.IsNullOrEmpty(listpurorder.CheckType.ToString())
            {
                purOrder.CheckType = int.Parse(request.PurOrder.CheckType);

            }
            //订单主表数量总计
            if (request.PurOrder.TotalQuantity != null && request.PurOrder.TotalQuantity.ToString().Trim() != "")//!string.IsNullOrEmpty(listpurorder.TotalQuantity.ToString())
            {
                purOrder.Quantity = decimal.Parse(request.PurOrder.TotalQuantity);

            }
            #region 修改主表总计金额
            if (request.PurOrder.TotalAmt != null && request.PurOrder.TotalAmt.ToString().Trim() != "")
            {
                purOrder.TotalAmt = decimal.Parse(request.PurOrder.TotalAmt);
            }
            #endregion
            //主表描述
            if (request.PurOrder.TotalMM != null && request.PurOrder.TotalMM.Trim() != "")//!string.IsNullOrEmpty(listpurorder.TotalMM.ToString())
            {
                purOrder.MM = request.PurOrder.TotalMM;
            }
            #endregion
            #region 收发货地址信息更新
            //收货地址对象
            if (request.PurOrder.ReceiveAddressData != null)
            {
                foreach (var item in request.PurOrder.ReceiveAddressData)
                {
                    EnterpriseAddress EntAdd = this.Db.FirstOrDefault<EnterpriseAddress>(x => x.ID == item.AddressID);
                    if (EntAdd != null)
                    {
                        purOrder.AddressID = item.AddressID;
                        purOrder.Address = item.Address;
                    }
                }
            }

            //发货地址对象

            if (request.PurOrder.SendAddressData != null)
            {
                foreach (var item in request.PurOrder.SendAddressData)
                {
                    EnterpriseAddress EntAdd = this.Db.FirstOrDefault<EnterpriseAddress>(x => x.ID == item.AddressID);
                    if (EntAdd != null)
                    {
                        purOrder.SAddressID = item.AddressID;
                        purOrder.SAddress = item.Address;
                    }
                }
            }
            #endregion
            #region  资源对象
            //出厂质检报告资源对象
            if (request.PurOrder.factoryReportData != null)
            {
                foreach (var item in request.PurOrder.factoryReportData)
                {
                    Resources pic = new Resources();
                    long resourceid = 0;
                    //查询订单主表中是否有资源ID，没有的话先创建一个，否则利用
                    //if (purOrder.FactoryReportResources!=null)
                    //{
                    if (purOrder.FactoryReportResources != 0)
                    {
                        resourceid = purOrder.FactoryReportResources;
                    }
                    else
                    {
                        resourceid = RecordIDService.GetRecordID(1);
                        //更新主表第三方质检报告id
                        purOrder.FactoryReportResources = resourceid;
                    }
                    //}
                    //else
                    //{
                    //    resourceid = RecordIDService.GetRecordID(1);
                    //    purOrder.FactoryReportResources = resourceid;
                    //}

                    //插入资源表中信息
                    pic.Id = RecordIDService.GetRecordID(1);
                    pic.DocumentID = resourceid;
                    pic.AccountID = purOrder.SAccountID;//订单主表的供应方账套
                    pic.OriginalName = item.OriginalName;
                    pic.NewName = item.NewName;
                    pic.FileLength = item.FileLength;
                    pic.Suffix = item.Suffix;
                    detailRes.Add(pic);

                }
            }
            //第三方质检报告对象
            if (request.PurOrder.thirdReportData != null)
            {
                foreach (var item in request.PurOrder.thirdReportData)
                {
                    Resources pic = new Resources();
                    //查询订单主表中是否有资源ID，没有的话先创建一个，否则利用
                    long resourceid = 0;
                    //if (purOrder.ThirdReportResources != null)
                    //{

                    if (purOrder.ThirdReportResources != 0)
                    {
                        resourceid = purOrder.ThirdReportResources;
                    }
                    else
                    {
                        resourceid = RecordIDService.GetRecordID(1);
                        purOrder.ThirdReportResources = resourceid;
                    }

                    //}
                    //else
                    //{
                    //    resourceid = RecordIDService.GetRecordID(1);
                    //    purOrder.ThirdReportResources = resourceid;
                    //}

                    //插入资源表中信息
                    pic.Id = RecordIDService.GetRecordID(1);
                    pic.DocumentID = resourceid;
                    pic.AccountID = purOrder.SAccountID;//订单主表的供应方账套
                    pic.OriginalName = item.OriginalName;
                    pic.NewName = item.NewName;
                    pic.FileLength = item.FileLength;
                    pic.Suffix = item.Suffix;
                    detailRes.Add(pic);

                }
            }
            #endregion

            response.Success = true;
        }

        private EnterpriseAddress ToAddressEntity(EnterpriseAddressList endaddress)
        {
            return endaddress.TranslateTo<EnterpriseAddress>();
        }
        private Resources ToResource(PicResources pic)
        {
            return pic.TranslateTo<Resources>();
        }
    }
}

