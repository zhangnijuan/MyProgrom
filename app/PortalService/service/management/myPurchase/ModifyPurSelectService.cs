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
using Ndtech.PortalModel;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 生成订单前更新优选说明Service
    /// add by yangshuo 2014/12/26
    /// </summary>
    public class ModifyPurSelectService : Service, IPost<ModifyPurSelectRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(ModifyPurSelectService));

        public object Post(ModifyPurSelectRequest request)
        {
            SavePurSelectResponse response = new SavePurSelectResponse();
            if (request.InquiryID > 0 && request.PurSelectID > 0)
            {
                if (request.DetailRemarks != null)
                {
                    //更新优选主档和询价明细当优选说明
                    return PostMethod(request);    
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoParameter";
                    response.ResponseStatus.Message = "no DetailRemarks Parameter";
                    response.Success = false;
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoID";
                response.ResponseStatus.Message = "Don't have InquiryID or PurSelectID";
                response.Success = false;
            }
            return response;
        }

        private SavePurSelectResponse PostMethod(ModifyPurSelectRequest request)
        {
            SavePurSelectResponse response = new SavePurSelectResponse();
            try
            {
                using (var trans = this.Db.BeginTransaction())
                {
                    //更新优选单主档
                    this.Db.Update<PurSelect>(string.Format("mm = '{0}' where a = {1} and id = {2}",
                                                    request.PurSelectMM, request.AccountID, request.PurSelectID));

                    foreach (var detail in request.DetailRemarks)
                    {
                        //更新询价明细优选说明
                        this.Db.Update<PurInquiryDetail>(string.Format("selectmm = '{0}' where a = {1} and id = {2}",
                                                                    detail.SelectMM, request.AccountID, detail.InquiryDID));
                    }

                    trans.Commit();
                }

                //调用生成订单接口
                CreatePurOrderService createPurOrderService = this.TryResolve<CreatePurOrderService>();
                CreatePurOrderRequest orderRequest = request.TranslateTo<CreatePurOrderRequest>();
                orderRequest.Source = SourceEnum.Select;
                createPurOrderService.Post(orderRequest);
                response.Success = true;
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
