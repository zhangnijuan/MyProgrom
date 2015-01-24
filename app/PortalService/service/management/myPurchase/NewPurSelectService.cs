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
using System.Text;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 新增、修改优选Service
    /// add by yangshuo 2014/12/24
    /// </summary>
    public class NewPurSelectService : Service, IPost<NewPurSelectRequest>
    {
        public object Post(NewPurSelectRequest request)
        {
            SavePurSelectResponse response = new SavePurSelectResponse();
            return PostMethod(request);
        }

        private SavePurSelectResponse PostMethod(NewPurSelectRequest request)
        {
            SavePurSelectResponse response = new SavePurSelectResponse();

            try
            {
                List<PurSelectResults> detailList = new List<PurSelectResults>();
                PurSelectResults detailTemp = null;

                //询价明细优选结果集合
                StringBuilder selectResultsTemp = new StringBuilder();
                if (request.DetailData != null)
                {
                    //1.前台model转成dataModel
                    PurSelect mainNew = request.TranslateTo<PurSelect>();

                    //2.根据询单主档ID 查询询价单是否有优选主档资料
                    var purOldSelect = this.Db.FirstOrDefault<PurSelect>(x => x.ID == request.ID && x.AccountID == request.AccountID);
                    if (purOldSelect != null)
                    {
                        mainNew.ID = purOldSelect.ID;
                    }
                    else
                    {
                        mainNew.ID = RecordIDService.GetRecordID(1);
                    }

                    //需删除的优选明细ID
                    StringBuilder deleteDIDs = new StringBuilder();

                    foreach (var detail in request.DetailData)
                    {
                        detailTemp = detail.TranslateTo<PurSelectResults>();

                        //优选产品ID存供应商产品ID
                        detailTemp.ItemID = detail.QuoItemID;

                        //优选数量大于0的生成优选结果
                        if (detailTemp.SelectQty > 0)
                        {
                            if (detailTemp.ID <= 0)
                            {
                                detailTemp.ID = RecordIDService.GetRecordID(1);
                            }

                            detailTemp.AccountID = request.AccountID;
                            detailTemp.MID = mainNew.ID;
                            detailTemp.Amt = detailTemp.Prc * detailTemp.SelectQty;

                            //优选供应商产品总金额->待更新同一优选主单相同供应商优选明细档案的总金额
                            detailTemp.TotalAmt = detailTemp.Amt;
                            detailList.Add(detailTemp);

                            //拼接优选结果 待更新询价单优选结果
                            selectResultsTemp.Append(detail.CompName).Append(":").Append(detail.SelectQty).Append("；");
                        }
                        else
                        {
                            if (detailTemp.ID > 0)
                            {
                                //累加优选数量为0且db已有资料的明细id->待删除
                                deleteDIDs.Append(detailTemp.ID).Append(",");
                            }
                        }
                    }

                    //根据询价单明细ID查询询价单明细档案
                    var purInquiryDetail = this.Db.FirstOrDefault<PurInquiryDetail>(x => x.ID == request.Inquirydid && x.AccountID == request.AccountID);

                    //有优选结果才生成优选单
                    if (selectResultsTemp.Length > 0 || deleteDIDs.Length > 0)
                    {
                        //新增or修改
                        if (selectResultsTemp.Length > 0)
                        {
                            #region 操作db前准备

                            //去除最后一个分号
                            string selectResults = selectResultsTemp.ToString().Substring(0, selectResultsTemp.Length - 1);

                            if (purOldSelect == null)
                            {
                                //0优选
                                mainNew.State = 0;
                                mainNew.Code = RecordSnumService.GetSnum(this, request.AccountID, SnumType.PurSelect);
                                mainNew.AccountID = request.AccountID;
                                mainNew.CreateTime = DateTime.Now;
                            }

                            //获取询价主档
                            var purInquiry = this.Db.FirstOrDefault<PurInquiry>(x => x.ID == request.InquiryID && x.AccountID == request.AccountID);

                            #endregion

                            #region Save

                            //2.使用事物insert
                            using (var trans = this.Db.BeginTransaction())
                            {
                                if (purOldSelect == null)
                                {
                                    //1 add优选主档
                                    this.Db.Insert(mainNew);
                                }

                                for (int i = 0; i < detailList.Count; i++)
                                {
                                    //2 add or update优选结果明细档
                                    this.Db.Save(detailList[i]);
                                }

                                if (purInquiryDetail != null)
                                {
                                    //3 更新询价明细单优选结果
                                    purInquiryDetail.SelectResults = selectResults;
                                    this.Db.Save(purInquiryDetail);

                                    if (purInquiry != null)
                                    {
                                        //4 更新询价单主档状态为已优选
                                        purInquiry.State = 2;
                                        purInquiry.State_Enum = "已优选";
                                        this.Db.Save(purInquiry);
                                    }
                                }

                                if (deleteDIDs.Length > 0)
                                {
                                    //优选数量修改为0且db已有资料时删除明细
                                    string delDIDs = deleteDIDs.ToString();
                                    delDIDs = delDIDs.Substring(0, delDIDs.Length - 1);

                                    this.Db.Delete<PurSelectResults>(string.Format("id in ({0})", delDIDs));
                                }

                                //提交事物
                                trans.Commit();
                            }

                            #endregion
                        }
                        else
                        {
                            using (var trans = this.Db.BeginTransaction())
                            {
                                //1.优选数量修改为0且db已有资料时删除明细
                                string delDIDs = deleteDIDs.ToString();
                                delDIDs = delDIDs.Substring(0, delDIDs.Length - 1);

                                this.Db.Delete<PurSelectResults>(string.Format("id in ({0})", delDIDs));

                                if (purInquiryDetail != null)
                                {
                                    //2.清空询价明细单优选结果
                                    purInquiryDetail.SelectResults = "";
                                    this.Db.Save(purInquiryDetail);
                                }

                                //提交事物
                                trans.Commit();
                            }
                        }

                        if (purOldSelect != null)
                        {
                            foreach (var detail in request.DetailData)
                            {
                                //根据供应商企业云ID+MID获取明细总金额->优选供应商产品总金额
                                decimal totalAmt = 0;
                                totalAmt = this.Db.Where<PurSelectResults>(x => x.MID == purOldSelect.ID && x.CompID == detail.CompID).Sum(x => x.Amt);

                                //更新优选供应商产品总金额
                                this.Db.Update<PurSelectResults>(string.Format("totalamt = {0} where compid = '{1}' and  mid = {2}",
                                    totalAmt, detail.CompID, purOldSelect.ID));
                            }
                        }
                    }

                    response.Success = true;
                }
                else
                {
                    //无优选明细资料
                    response.ResponseStatus.ErrorCode = "Err_NoOptimization";
                    response.ResponseStatus.Message = "no DetailData by request";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }

            return response;
        }

        #region Log日志

        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error"
                + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }

        #endregion

    }
}
