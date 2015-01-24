using System;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using Ndtech.PortalModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 认证明细信息详情Service
    /// add by yangshuo 2015/01/08
    /// </summary>
    public class GetItemCertificationByDIDService : Service, IGet<GetItemCertificationByDIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemCertificationByDIDService));

        #region Get单笔详情

        public object Get(GetItemCertificationByDIDRequest request)
        {
            GetItemCertificationByDIDResponse response = new GetItemCertificationByDIDResponse();

            if (request.ID > 0)
            {
                //第二步:search单笔明细 + 返回response
                return GetMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_IDIsNull";
                response.ResponseStatus.Message = "No ID Parameter";
                return response;
            }
        }

        private GetItemCertificationByDIDResponse GetMethod(GetItemCertificationByDIDRequest request)
        {
            GetItemCertificationByDIDResponse response = new GetItemCertificationByDIDResponse();

            //查询质量认证明细和认证机构名称
            string sql = string.Format(@"select d.*, m.certificationname 
                                         from udoc_certification_appdetail as d 
                                         inner join udoc_certification_application as m on m.id = d.mid where d.id = {0};", request.ID);
            var detail = this.Db.QuerySingle<ItemCertificationDetailInfo>(sql);
            if (detail != null)
            {
                //认证报告附件
                List<Resources> resourceDB = this.Db.Where<Resources>(x => x.DocumentID == request.ID);

                if (resourceDB != null && resourceDB.Count > 0)
                {
                    ReturnPicResources resource = new ReturnPicResources();
                    List<ReturnPicResources> resourceList = new List<ReturnPicResources>();
                    foreach (var reportInfo in resourceDB)
                    {
                        resource = ToResources(reportInfo, reportInfo.AccountID);
                        resourceList.Add(resource);
                    }
                    detail.PicResources = resourceList;
                }
                response.Data = detail;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                response.ResponseStatus.Message = Const.Err_NoInfoByID;
                return response;
            }
            response.Success = true;
            return response;
        }

        private ReturnPicResources ToResources(Resources res, int accountID)
        {
            var result = res.TranslateTo<ReturnPicResources>();
            result.FileUrl = string.Format("/fileuploads/{0}", accountID);
            return result;
        }

        #endregion
    }
}
