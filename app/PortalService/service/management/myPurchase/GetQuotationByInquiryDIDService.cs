using System;
using System.IO;
using System.Linq;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.FluentValidation.Results;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 根据询价明细ID查询报价企业资料(优选单)
    /// add by yangshuo 2014/12/23
    /// </summary>
    public class GetQuotationByInquiryDIDService : Service, IPost<GetQuotationByInquiryDIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetQuotationByInquiryDIDService));

        public object Post(GetQuotationByInquiryDIDRequest request)
        {
            GetQuotationByInquiryDIDResponse response = new GetQuotationByInquiryDIDResponse();

            if (request.Inquirydid > 0)
            {
                //search + 返回response
                return GetMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_InquirydidIsNull";
                response.ResponseStatus.Message = "Don't have Inquirydid";
                return response;
            }
        }

        #region 私有方法

        private GetQuotationByInquiryDIDResponse GetMethod(GetQuotationByInquiryDIDRequest request)
        {
            GetQuotationByInquiryDIDResponse response = new GetQuotationByInquiryDIDResponse();

            //1.1 根据询价明细ID查询询价单资料
            QuotationOptimization data = this.Db.QuerySingle<QuotationOptimization>(
                string.Format(@"select i, i_c, i_n, d.standarditemcode, d.standarditemname, d.propertyname, d.remark, d.qty, d.u_c, d.u_n,
                                       d.deliverydate, d.mm, 0.00 as lastprc, '' as lastcompname, '' as lastcompid, 
                                       d.category, d.category_c, d.category_n, m.subject, m.id as inquiryid, m.c as inquirycode,
                                       p.id
                                    from pur_inquirydetail as d inner join pur_inquiry as m on m.id = d.mid
                                    left join pur_select as p on p.inquiryid = m.id
                                    where d.id = {0};", request.Inquirydid));

            if (data != null)
            {
                //查询产品交易历史
                GetPurOrderHistoryRequest historyRequest = new GetPurOrderHistoryRequest();
                historyRequest.AccountId = request.AccountID;
                historyRequest.StandardItemCode = data.StandardItemCode;
                historyRequest.StandardItemName = data.StandardItemName;
                GetPurOrderHistoryService historyService = new GetPurOrderHistoryService();
                GetPurOrderHistoryResponse historyResponse = historyService.Post(historyRequest).TranslateTo<GetPurOrderHistoryResponse>();

                if (historyResponse.Data != null && historyResponse.Data.Count > 0)
                {
                    //上次成交价、上次成交供应商取第一笔
                    data.LastPrc = historyResponse.Data[0].Prc;
                    data.LastCompName = historyResponse.Data[0].SupplyName;
                }
                response.Data = data;

                //1.2 根据询价明细ID查询报价企业集合->取出明细产品资料,保存优选汇总使用
                //同一报价单报价一笔明细另一笔未报价,但主档state已经是已报价状态,通过取报价产品档有该产品显示该供应商 modify2015/01/15
                List<QuotationOptimizationDetail> detailData = this.Db.Query<QuotationOptimizationDetail>(
                    string.Format(@"select distinct r.id, r.a, r.mid,
                                               c.compname, c.corpnum, d.smm, 0.00 as lastprc, d.prc, d.amt as quoamt,
                                               i.mm, m.id as quotationid, d.i, d.inquirydid, d.id as quotationdid, 
                                               r.sqty, r.amt, r.totalamt,
                                               d.standarditemcode, d.standarditemname, d.i_c, d.i_n, d.propertyname, d.qty,
                                               d.u_c, d.u_n, d.deliverydate, d.category_c, d.category_n, i.id as quoitemid,
                                               m.a as sa
                                        from sal_quotationdetail d 
                                        inner join sal_quotation as m
                                            on m.id = d.mid
                                        inner join sal_quotationcomp as c 
                                            on c.sid = m.id
                                        left join pur_select_results as r
                                            on r.quotationdid = d.id and r.compid = c.corpnum
                                        left join udoc_enterprise_item as i
                                            on i.a = m.a and i.standard_c = d.standarditemcode
                                            and i.state = 1 and i.biztype = 2
                                        where d.inquirydid = {0} and d.prc > 0;", request.Inquirydid));

                if (detailData != null && detailData.Count > 0)
                {
                    //2.获取总行数
                    response.RowsCount = detailData.Count;

                    //3.拼接与每个供应商的上次成交价
                    if (historyResponse.Data != null && historyResponse.Data.Count > 0)
                    {
                        PurOrderHistory lastHistory = null;
                        foreach (var item in detailData)
                        {
                            //查询与该供应商的上次成交价
                            lastHistory = historyResponse.Data.FirstOrDefault(x => x.SAccountID == item.SAccountID);
                            if (lastHistory != null)
                            {
                                item.LastPrc = lastHistory.Prc;
                            }
                        }
                    }

                    if (request.PageIndex > 0 && request.PageSize > 0)
                    {
                        //4.分页
                        detailData = detailData.OrderBy(x => x.Quotationdid).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                    }
                    response.Data.DetailData = detailData;
                    response.Success = true;
                }
                else
                {
                    //no data
                    response.ResponseStatus.ErrorCode = "Err_NoData";
                    response.ResponseStatus.Message = "Don't have sal_quotationdetail or sal_quotationcomp by inquirydid";
                    response.Success = false;
                }
            }
            else
            {
                //no data
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No data";
                response.Success = false;
            }

            return response;
        }

        #endregion
    }
}
