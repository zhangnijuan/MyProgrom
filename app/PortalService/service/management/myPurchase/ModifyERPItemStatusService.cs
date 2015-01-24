using System;
using System.IO;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.Logging;
using System.Text;
using ServiceStack.Text;
using ServiceStack.DataAnnotations;
using System.Linq;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 批量修改企业产品状态Service
    /// add by yangshuo 2014/12/29
    /// </summary>
    public class ModifyERPItemStatusService : Service, IPost<ModifyERPItemStatusRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetQuotationByIDService));

        #region Post批量企业产品状态

        public object Post(ModifyERPItemStatusRequest request)
        {
            EnterpriseItemResponse response = new EnterpriseItemResponse();

            //第一步:校验前端的数据合法性
            if (!string.IsNullOrEmpty(request.IDs))
            {
                //第二步:modify产品状态 + 返回response
                return PostMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoIDsParameter";
                response.ResponseStatus.Message = "No Parameter of IDs";
                return response;
            }
        }

        private EnterpriseItemResponse PostMethod(ModifyERPItemStatusRequest request)
        {
            EnterpriseItemResponse response = new EnterpriseItemResponse();

            long itemID = -1;

            #region 删除前检核

            if (request.State == 3)
            {
                List<ErrorIDs> idsList = new List<ErrorIDs>();
                ErrorIDs ids = new ErrorIDs();
                if (request.BizType == 1)
                {
                    #region 采购产品删除前校验

                    StringBuilder sError = new StringBuilder();
                    PurInquiryDetail pDetail = null;
                    foreach (var item in request.IDs.Split(','))
                    {
                        itemID = Convert.ToInt64(item);

                        //已询价的不能删除
                        pDetail = this.Db.FirstOrDefault<PurInquiryDetail>(x => x.ItemID == itemID);
                        if (pDetail != null)
                        {
                            sError.Append(pDetail.StandardItemCode).Append("(").Append(pDetail.ItemCode).Append(")产品处于已询价状态，不能删除；").Append("\r\n");

                            //拼接错误id
                            ids = new ErrorIDs();
                            ids.ErrorID = item;
                            idsList.Add(ids);
                        }
                    }

                    if (sError.Length > 0)
                    {
                        string error = sError.ToString().Substring(0, sError.Length - 2);
                        response.ResponseStatus.ErrorCode = "Error_Delete";
                        response.ResponseStatus.Message = error;
                        response.Data = idsList;
                        return response;
                    }

                    #endregion
                }
                else if (request.BizType == 2)
                {
                    #region 供应产品删除前校验

                    if (!string.IsNullOrEmpty(request.States) && !string.IsNullOrEmpty(request.StandardItemCodes) && !string.IsNullOrEmpty(request.ItemCodes))
                    {
                        StringBuilder sError = new StringBuilder();

                        //1.查询公司所有报价的供应产品
                        string dSql = string.Format(@"select d.id, d.standarditemcode from sal_quotationdetail as d inner join sal_quotation as m on d.mid = m.id 
                                                        where d.a = {0} and (m.state = 1 or m.state = 2 or m.state = 3 or m.state = 4);", request.AccountID);
                        List<EnterpriseItemInfo> supItem = this.Db.Query<EnterpriseItemInfo>(dSql);
                        List<EnterpriseItemInfo> supItemTemp = null;

                        //2.查询公司所有认证中的供应产品
                        string cSql = string.Format(@"select d.id, d.standarditemcode, d.i from sal_quotationdetail as d inner join sal_quotation as m on d.mid = m.id
                                                        where d.a = {0} and m.state = 2;", request.AccountID);
                        List<EnterpriseItemInfo> cerItem = this.Db.Query<EnterpriseItemInfo>(cSql);
                        List<EnterpriseItemInfo> cerItemTemp = null;

                        //3.查询公司所有采购订单资料
                        string oSql = string.Format(@"select d.id, d.standarditemcode, d.i from pur_orderdetail as d inner join pur_order as m on d.mid = m.id
                                                        where d.a = {0} and (m.state = 1 or m.state = 2 or m.state = 4);", request.AccountID);
                        List<EnterpriseItemInfo> orderItem = this.Db.Query<EnterpriseItemInfo>(oSql);
                        List<EnterpriseItemInfo> orderItemTemp = null;

                        int rowIndex = 0;
                        string standardItemCode = string.Empty;
                        string itemCode = string.Empty;
                        foreach (var item in request.IDs.Split(','))
                        {
                            standardItemCode = request.StandardItemCodes.Split(',')[rowIndex];

                            #region Check

                            //有平台对照关系的产品才需check,导入产品未对照平台时可直接delete
                            if (standardItemCode != "")
                            {
                                itemID = Convert.ToInt64(item);
                                itemCode = request.ItemCodes.Split(',')[rowIndex];
                                ids = new ErrorIDs();

                                //1.已发布不能删除
                                if (request.States.Split(',')[rowIndex] == "1")
                                {
                                    sError.Append(standardItemCode).Append("(").Append(itemCode).Append(")产品处于已发布状态，不能删除；").Append("\r\n");

                                    //拼接错误id
                                    ids.ErrorID = item;
                                    idsList.Add(ids);
                                }
                                else
                                {
                                    //2.正在认证中 不能删除
                                    cerItemTemp = cerItem.FindAll(x => x.ItemID == itemID);
                                    if (cerItemTemp != null && cerItemTemp.Count > 0)
                                    {
                                        sError.Append(standardItemCode).Append("(").Append(itemCode).Append(")产品正在认证中，不能删除；").Append("\r\n");

                                        //拼接错误id
                                        ids.ErrorID = item;
                                        idsList.Add(ids);
                                    }
                                    else
                                    {
                                        //3.已报价 不能删除
                                        supItemTemp = supItem.FindAll(x => x.StandardItemCode == standardItemCode);

                                        if (supItemTemp != null && supItemTemp.Count > 0)
                                        {
                                            sError.Append(standardItemCode).Append("(").Append(itemCode).Append(")产品处于已报价状态，不能删除；").Append("\r\n");

                                            //拼接错误id
                                            ids.ErrorID = item;
                                            idsList.Add(ids);
                                        }
                                        else
                                        {
                                            //4.已经存在交易记录 不能删除
                                            orderItemTemp = orderItem.FindAll(x => x.StandardItemCode == standardItemCode);
                                            if (orderItemTemp != null && orderItemTemp.Count > 0)
                                            {
                                                sError.Append(standardItemCode).Append("(").Append(itemCode).Append(")产品已经存在交易记录，不能删除；").Append("\r\n");

                                                //拼接错误id
                                                ids.ErrorID = item;
                                                idsList.Add(ids);
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            rowIndex++;
                        }

                        if (sError.Length > 0)
                        {
                            string error = sError.ToString().Substring(0, sError.Length - 2);
                            response.ResponseStatus.ErrorCode = "Error_Delete";
                            response.ResponseStatus.Message = error;
                            response.Data = idsList;
                            return response;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(request.States) || string.IsNullOrEmpty(request.ItemCodes))
                        {
                            //参数缺少
                            response.ResponseStatus.ErrorCode = "Error_ParameterIsNull";
                            response.ResponseStatus.Message = "No States or ItemCodes Parameter";
                            return response;
                        }
                    }

                    #endregion
                }
            }

            #endregion

            DateTime dtNow = DateTime.Now;

            //1.拼接更新产品sql
            StringBuilder sql = new StringBuilder(string.Format("state = {0}, modifydate = '{1}' ", request.State, dtNow));
            if (request.State == 1)
            {
                //发布
                sql.Append(string.Format(", publishdate = '{0}' ", dtNow));
            }
            sql.Append(string.Format("where id in ({0});", request.IDs));

            string updateSql = sql.ToString();

            #region 发布供应产品,拼接产品修改日志实体

            List<EnterpriseItemLog> itemLogList = new List<EnterpriseItemLog>();
            if (request.BizType == 2 && request.State == 1)
            {
                //2.发布供应产品,拼接产品修改日志实体
                EnterpriseItemLog itemLog = null;
                GetCompItemByIDResponse itemOldResponse = null;
                GetCompItemByIDRequest itemRequest = new GetCompItemByIDRequest();
                GetCompItemByIDService itemService = this.TryResolve<GetCompItemByIDService>();
                foreach (var item in request.IDs.Split(','))
                {
                    itemID = Convert.ToInt64(item);
                    itemRequest.ID = itemID;
                    itemOldResponse = itemService.Get(itemRequest).TranslateTo<GetCompItemByIDResponse>();
                    if (itemOldResponse != null && itemOldResponse.Data != null)
                    {
                        itemLog = new EnterpriseItemLog();
                        itemLog.ID = RecordIDService.GetRecordID(1);
                        itemLog.AccountID = request.AccountID;
                        itemLog.ItemID = itemID;
                        itemLog.EID = request.EID;
                        itemLog.EIDCode = request.EIDCode;
                        itemLog.EIDName = request.EIDName;

                        //将修改前实体资料转成json
                        itemLog.Obj = itemOldResponse.Data.ToJson();
                        itemLog.CreateDate = dtNow;
                        itemLogList.Add(itemLog);
                    }
                }
            }

            #endregion

            int releasesCount = 0;

            using (var trans = this.Db.BeginTransaction())
            {
                //1.更新产品状态
                this.Db.Update<EnterpriseItem>(updateSql);

                //2.insert产品修改日志
                if (itemLogList.Count > 0)
                {
                    foreach (var log in itemLogList)
                    {
                        this.Db.Insert(log);
                    }
                }

                //供应产品操作
                if (request.BizType == 2)
                {
                    //3.查询已发布产品数量并更新公司档案 add 2015/01/06
                    releasesCount = Convert.ToInt32(this.Db.Count<EnterpriseItem>(x => x.AccountID == request.AccountID && x.BizType == 2 && x.State == 1));
                    this.Db.Update<NdtechCompany>(string.Format("releases = {0} where a = {1}", releasesCount, request.AccountID));
                }

                trans.Commit();
            }

            #region insert平台价格档案

            //发布、下架、批量发布、批量下架、批量删除都需重新计算平台价格
            if (request.BizType == 2 && !string.IsNullOrEmpty(request.StandardItemCodes))
            {
                //平台产品档案
                List<NdtechItem> ndtechItemList = this.Db.Select<NdtechItem>();

                if (ndtechItemList != null && ndtechItemList.Count > 0)
                {
                    #region 循环计算平台价格

                    NdtechItem ndtechItem = null;
                    decimal avgPrc = 0;
                    List<NdtechItemPrice> ndtechPriceList = new List<NdtechItemPrice>();
                    NdtechItemPrice ndtechPrice = null;
                    foreach (var standardItemCode in request.StandardItemCodes.Split(','))
                    {
                        //有平台对照关系的产品才需check,导入产品未对照平台时可直接delete
                        if (standardItemCode != "")
                        {
                            //1.获取平台产品id
                            ndtechItem = ndtechItemList.Find(x => x.ItemCode == standardItemCode);

                            if (ndtechItem != null)
                            {
                                //2.根据平台标准代码查询产品平均价格做为平台平均价格
                                avgPrc = this.Db.Where<EnterpriseItem>(x => x.StandardItemCode == standardItemCode).Average(x => x.SalPrc);

                                //3.新增至平台产品价格档案
                                ndtechPrice = new NdtechItemPrice();
                                ndtechPrice.ID = RecordIDService.GetRecordID(1);
                                ndtechPrice.AccountID = 0;
                                ndtechPrice.ItemID = ndtechItem.ID;
                                ndtechPrice.Price = avgPrc;
                                ndtechPrice.PriceStartDate = dtNow;
                                ndtechPriceList.Add(ndtechPrice);
                            }
                            else
                            {
                                //no平台产品资料
                                response.ResponseStatus.ErrorCode = "No Data";
                                response.ResponseStatus.Message = "No PlatFormItem Info";
                                return response;
                            }
                        }
                    }

                    #endregion

                    //循环新增平台价格
                    if (ndtechPriceList.Count > 0)
                    {
                        using (var trans = this.Db.BeginTransaction())
                        {
                            foreach (var item in ndtechPriceList)
                            {
                                this.Db.Insert(item);
                            }
                            trans.Commit();
                        }
                    }
                }
                else
                {
                    //no平台产品资料集合
                    response.ResponseStatus.ErrorCode = "No Data";
                    response.ResponseStatus.Message = "No PlatFormItemList Info";
                    return response;
                }
            }

            #endregion

            response.Success = true;
            return response;
        }

        #endregion
    }

    public class EnterpriseItemInfo
    {
        /// <summary>
        /// 报价明细id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 平台产品代码
        /// </summary>
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 我的产品id
        /// </summary>
        [Alias("i")]
        public long ItemID { get; set; }
    }
}
