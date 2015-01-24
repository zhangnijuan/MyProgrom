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
using ServiceStack.Logging;


namespace Ndtech.PortalService.Auth
{
    public class GetPurorderByIDValidator : AbstractValidator<GetPurorderByIDRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(GetPurorderByIDRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(instance.ID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("ID", Const.Err_IDIsNull, "Err_Err_IDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.AccountId.ToString()))
            {
                result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountIDIsNull, "Err_AccountIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.counterparty))
            {
                result.Errors.Add(new ValidationFailure("CounterParty", Const.Err_CounterPartyIsNull, "Err_CounterPartyIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }
    public class GetPurorderByIDService : Service, IGet<GetPurorderByIDRequest>
    {
        ILog log = LogManager.GetLogger(typeof(GetPurorderByIDService));
        public object Get(GetPurorderByIDRequest request)
        {

            GetPurorderByIDResponse response = new GetPurorderByIDResponse();

            PurorderInfoList PurorderInfo = new PurorderInfoList();

            StringBuilder SearchSql = new StringBuilder("select * from pur_order  where 1=1 and id=" + request.ID);

            if (request.counterparty.ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//供应方
            {
                SearchSql.Append(" and sa="+request.AccountId);
            }
            else
            {
                SearchSql.Append(" and a=" + request.AccountId);//采购方
            }
            var list = this.Db.Query<PurOrder>(string.Format(SearchSql.ToString()));
         
            if (list.Count > 0)
            {
                #region
                foreach (var item in list)
                {
                    PurorderInfo = ToEntity(item);//主表
                    NdtechStaffCompany sc = this.Db.QuerySingle<NdtechStaffCompany>(string.Format("select b.n as compname from pur_order a join udoc_comp b on a.a=b.a where a.id= '{0}'", request.ID));
                    PurorderInfo.PurName = sc.CompName;
                    //订单主表中涉及的图片资源信息
                    List<ReturnPicResources> FactoryReportDataList = new List<ReturnPicResources>();
                    List<ReturnPicResources> ThirdReportDataList = new List<ReturnPicResources>();
                    //出厂资源信息
                    StringBuilder searchfacres = new StringBuilder("select * from udoc_resources where mid=" + PurorderInfo.FactoryReportResources);
                    var resfaclist = this.Db.Query<Resources>(string.Format(searchfacres.ToString()));
                    foreach (var resitem in resfaclist)
                    {
                        FactoryReportDataList.Add(ToResources(resitem, PurorderInfo.SAccountID));                        
                    }
                    PurorderInfo.FactoryReportData = FactoryReportDataList;
                    //第三方资源信息
                    StringBuilder searchthirdres = new StringBuilder("select * from udoc_resources where mid=" + PurorderInfo.ThirdReportResources);
                    var resthirdlist = this.Db.Query<Resources>(string.Format(searchthirdres.ToString()));
                    foreach (var resitem in resthirdlist)
                    {
                        ThirdReportDataList.Add(ToResources(resitem, PurorderInfo.SAccountID));
                    }
                    PurorderInfo.ThirdReportData = ThirdReportDataList;

                    //订单明细
                    List<PurOrderDetailList> PurOrderDetail = new List<PurOrderDetailList>();
                    StringBuilder purdet = new StringBuilder("select * from pur_orderdetail where mid=" + item.ID);

                    var detlist = this.Db.Query<PurOrderDetail>(string.Format(purdet.ToString()));
                    foreach (var detitem in detlist)
                    {
                        var purdetlist = ToPurOrderDetail(detitem);
                        ////收发货记录
                        //List<SalQuotDetailList> ListModelSalQuotDetail = new List<SalQuotDetailList>();
                        //StringBuilder OutNoticeDet = new StringBuilder("select * from sal_outnoticedetail where detailid=" + purdetlist.ID);
                        //var SalQuotDetailList = this.Db.Query<SalOutNoticeDetail>(string.Format(OutNoticeDet.ToString()));
                        //foreach (var salquotdetitem in SalQuotDetailList)
                        //{
                        //    ListModelSalQuotDetail.Add(ToSalQuotDet(salquotdetitem));
                        //}
                        //purdetlist.SalQuotList = ListModelSalQuotDetail;//单个明细中的对象
                        //

                        //历史收货记录
                        List<SalQuotDetailList> ReceiveSalQuotData = new List<SalQuotDetailList>();
                        if (request.counterparty.ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//采购方
                        {
                            StringBuilder SendHistory = new StringBuilder("select * from sal_outnoticedetail where detailid='" + purdetlist.ID + "' and a=" + PurorderInfo.AccountID+" and state=1");

                            var history = this.Db.Query<SalOutNoticeDetail>(string.Format(SendHistory.ToString()));
                            foreach (var hisitem in history)
                            {
                                ReceiveSalQuotData.Add(ToSalQuotDet(hisitem));
                            }
                        }
                        purdetlist.ReceiveSalQuotList = ReceiveSalQuotData;

                        //历史发货记录
                        List<SalQuotDetailList> SendSalQuotData = new List<SalQuotDetailList>();
                        if (request.counterparty.ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//销售方
                        {
                            StringBuilder SendHistory = new StringBuilder("select * from sal_outnoticedetail where detailid='" + purdetlist.ID + "' and sa=" + PurorderInfo.SAccountID);

                            var history = this.Db.Query<SalOutNoticeDetail>(string.Format(SendHistory.ToString()));
                            foreach (var hisitem in history)
                            {
                                SendSalQuotData.Add(ToSalQuotDet(hisitem));
                            }
                        }
                        purdetlist.SendSalQuotList = SendSalQuotData;

                        //明细附件资源信息
                        List<ReturnPicResources> DetPicRes = new List<ReturnPicResources>();
                        StringBuilder detpic = new StringBuilder("select * from udoc_resources where a=" + purdetlist.AccountID+ " and mid="+purdetlist.ID);
                        var detpiclist = this.Db.Query<Resources>(string.Format(detpic.ToString()));
                        foreach (var detresitem in detpiclist)
                        {
                            DetPicRes.Add(ToResources(detresitem, purdetlist.AccountID));
                        }
                        purdetlist.FileUploadData = DetPicRes;

                        //本次收货数量ThisGetqty
                        StringBuilder thisgetamt = new StringBuilder("select sum(deliveryqty) as thisgetqty from sal_outnoticedetail where state=0 and sa=" + purdetlist.SAccountID + " and detailid=" + purdetlist .ID+ " and createdate<'" + DateTime.Now + "'");

                        decimal thiscount = this.Db.QuerySingle<decimal>(string.Format(thisgetamt.ToString()));
                        if (thiscount > 0)
                        {
                            purdetlist.ThisGetqty = thiscount;
                        }
                        //本次发货备注
                        StringBuilder thisMM = new StringBuilder("select * from sal_outnoticedetail where state=0 and mm<>'' and sa=" + purdetlist.SAccountID + " and detailid=" + purdetlist.ID + " and createdate<'" + DateTime.Now + "'");

                        var thismmdet = this.Db.Query<SalOutNoticeDetail>(string.Format(thisMM.ToString()));
                        List<string> thisMMList = new List<string>();
                        if (thismmdet.Count > 0)
                        {
                            foreach (var mmitem in thismmdet)
                            {
                                thisMMList.Add(mmitem.MM);
                            }
                        }
                        purdetlist.ThisMM = thisMMList.Count > 0 ? string.Join(",", thisMMList.ToArray()) : string.Empty; ;
                       

                        //已发货
                        StringBuilder totalsalqty = new StringBuilder("select sum(deliveryqty) as totalsalqty from sal_outnoticedetail where sa=" + purdetlist.SAccountID + " and detailid ='" + purdetlist.ID + "' and createdate<'" + DateTime.Now + "'");

                        decimal totalsaldet = this.Db.QuerySingle<decimal>(string.Format(totalsalqty.ToString()));
                        if (totalsaldet > 0)
                        {
                            purdetlist.TotalDetSalQty = totalsaldet;
                        }
                        //已收货
                        StringBuilder totalgetqty = new StringBuilder("select sum(arrivalqty) as totalgetqty from sal_outnoticedetail where a=" + purdetlist.AccountID + " and detailid ='" + purdetlist.ID + "' and createdate<'" + DateTime.Now + "'");

                        decimal totalgetdet = this.Db.QuerySingle<decimal>(string.Format(totalgetqty.ToString()));
                        if (totalgetdet > 0)
                        {
                            purdetlist.TotalDetGetQty = totalgetdet;
                        }


                        PurOrderDetail.Add(purdetlist);
                    }
                    PurorderInfo.ItemList = PurOrderDetail;
                    //收货地址
                    List<EnterpriseAddressList> ListModelReceiveAddress = new List<EnterpriseAddressList>();
                    StringBuilder recadd = new StringBuilder("select * from udoc_enterprise_address where id=" + item.AddressID);
                    var ReceiveAddress = this.Db.Query<EnterpriseAddress>(string.Format(recadd.ToString()));
                    foreach (var recadditem in ReceiveAddress)
                    {
                        ListModelReceiveAddress.Add(ToAddress(recadditem));
                    }
                    PurorderInfo.GetAddressList = ListModelReceiveAddress;

                    //发货地址
                    List<EnterpriseAddressList> ListModelSendAddress = new List<EnterpriseAddressList>();
                    StringBuilder sendadd = new StringBuilder("select * from udoc_enterprise_address where id=" + item.SAddressID);
                    var SendAddress = this.Db.Query<EnterpriseAddress>(string.Format(sendadd.ToString()));
                    foreach (var sendadditem in SendAddress)
                    {
                        ListModelSendAddress.Add(ToAddress(sendadditem));
                    }
                    PurorderInfo.SendAddressList = ListModelSendAddress;

                    ////付款收款
                    //List<ArapReceiveList> ListModelArapReceive = new List<ArapReceiveList>();
                    //StringBuilder arapreceive = new StringBuilder("select * from arap_receiving where orderid=" + item.ID);
                    //var ArapReceiveList = this.Db.Query<ArapReceiving>(string.Format(arapreceive.ToString()));
                    //foreach (var arapreceitem in ArapReceiveList)
                    //{
                    //    ListModelArapReceive.Add(ToArapReceive(arapreceitem));
                    //}
                    //PurorderInfo.ArapReceList = ListModelArapReceive;

                    //历史收款记录
                    List<ArapReceiveList> GetArapReceivedata = new List<ArapReceiveList>();
                    StringBuilder ArapReceivedata = new StringBuilder("select * from arap_receiving where orderid=" + item.ID + " and sa=" + PurorderInfo.SAccountID+" and state=1");
                    var GetArapReceiveList = this.Db.Query<ArapReceiving>(string.Format(ArapReceivedata.ToString()));
                    if (request.counterparty.ToUpper() == CounterPartyEnum.Sell.ToString().ToUpper())//销售方
                    {
                        foreach (var arapreceitem in GetArapReceiveList)
                        {
                            GetArapReceivedata.Add(ToArapReceive(arapreceitem));
                        }
                    }
                    PurorderInfo.GetArapReceList = GetArapReceivedata;

                    //历史付款记录
                    List<ArapReceiveList> PayArapReceivedata = new List<ArapReceiveList>();
                    StringBuilder PayArapReceive = new StringBuilder("select * from arap_receiving where orderid=" + item.ID + " and a=" + PurorderInfo.AccountID);
                    var PayArapReceiveList = this.Db.Query<ArapReceiving>(string.Format(PayArapReceive.ToString()));
                    if (request.counterparty.ToUpper() == CounterPartyEnum.Purchase.ToString().ToUpper())//销售方
                    {
                        foreach (var arapreceitem in PayArapReceiveList)
                        {
                            PayArapReceivedata.Add(ToArapReceive(arapreceitem));
                        }
                    }                    
                    PurorderInfo.PayArapReceList = PayArapReceivedata;

                    //本次收款金额=已付款金额（未收款的）的总和
                    StringBuilder mygetamt = new StringBuilder("select sum(payment) as mytotal from arap_receiving where state=0 and orderid=" + PurorderInfo.ID + " and a=" + PurorderInfo.AccountID + " and createdate<'" + DateTime.Now + "'");
                   
                    decimal count = this.Db.QuerySingle<decimal>(string.Format(mygetamt.ToString()));
                    if(count>0)
                    {
                        PurorderInfo.MyGetAmt = count;
                    }
                    //本次收款备注
                    StringBuilder myMM = new StringBuilder("select * from arap_receiving where state=0 and mm<>'' and orderid=" + PurorderInfo.ID + " and a=" + PurorderInfo.AccountID + " and createdate<'" + DateTime.Now + "'");
                    List<string> myMMlist = new List<string>();
                    var thismymm = this.Db.Query<ArapReceiving>(string.Format(myMM.ToString()));
                    string smymm = "";
                    if (thismymm.Count > 0)
                    {
                        foreach (var mymmitem in thismymm)
                        {
                            myMMlist.Add(mymmitem.MM);
                        }
                    }
                    PurorderInfo.MyMM = myMMlist.Count>0?string.Join(",",myMMlist.ToArray()):string.Empty;



                    //已发货总计
                    StringBuilder totalsalqtystr = new StringBuilder("select sum(deliveryqty) as totalsalqty from sal_outnoticedetail where sa=" + PurorderInfo.SAccountID + " and detailid in(select id from pur_orderdetail where mid='" + PurorderInfo.ID+ "') and createdate<'" + DateTime.Now + "'");

                    decimal totalsal = this.Db.QuerySingle<decimal>(string.Format(totalsalqtystr.ToString()));
                    if (totalsal > 0)
                    {
                        PurorderInfo.TotalSalQty = totalsal;
                    }
                    //已付款总计 获取该订单所有的付款记录
                    StringBuilder totalpayamt = new StringBuilder("select sum(payment) as totalpayamt from arap_receiving where orderid='" + PurorderInfo .ID+ "' and a=" + PurorderInfo.AccountID + " and createdate<'" + DateTime.Now + "'");

                    decimal totalpay = this.Db.QuerySingle<decimal>(string.Format(totalpayamt.ToString()));
                    if (totalpay > 0)
                    {
                        PurorderInfo.TotalPayAmt = totalpay;
                    }

                    //已收款总计 获取该订单已收款的付款记录
                    StringBuilder totalrecvamt = new StringBuilder("select sum(payment) as totalrecvamt from arap_receiving where state = 1 and  orderid='" + PurorderInfo.ID + "' and a=" + PurorderInfo.AccountID + " and createdate<'" + DateTime.Now + "'");

                    decimal totalrec = this.Db.QuerySingle<decimal>(string.Format(totalrecvamt.ToString()));
                    if (totalrec > 0)
                    {
                        PurorderInfo.TotalRecvAmt = totalrec;
                    }
                }
                response.Data = PurorderInfo;
                response.Success = true;
                #endregion
            }          

            return response;
        }
        private PurorderInfoList ToEntity(PurOrder pur)
        {
            return pur.TranslateTo<PurorderInfoList>();
        }
        private PurOrderDetailList ToPurOrderDetail(PurOrderDetail purdet)
        {
            return purdet.TranslateTo<PurOrderDetailList>();
        }
        private EnterpriseAddressList ToAddress(EnterpriseAddress entadd)
        {
            return entadd.TranslateTo<EnterpriseAddressList>();
        }
        private ArapReceiveList ToArapReceive(ArapReceiving arap)
        {
            return arap.TranslateTo<ArapReceiveList>();
        }
        private SalQuotDetailList ToSalQuotDet(SalOutNoticeDetail saldet)
        {
            return saldet.TranslateTo<SalQuotDetailList>();
        }

        private ReturnPicResources ToResources(Resources res, int accountID)
        {
            var result = res.TranslateTo<ReturnPicResources>();
            result.FileUrl = string.Format("/fileuploads/{0}", accountID);
            return result;

        }
    }

}
