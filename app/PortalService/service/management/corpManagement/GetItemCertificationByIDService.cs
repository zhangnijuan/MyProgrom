using System;
using System.Text;
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
    /// 认证信息详情Service
    /// add by yangshuo 2015/01/07
    /// </summary>
    public class GetItemCertificationByIDService : Service, IGet<GetItemCertificationByIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemCertificationByIDService));

        #region Get单笔详情

        public object Get(GetItemCertificationByIDRequest request)
        {
            GetItemCertificationByIDResponse response = new GetItemCertificationByIDResponse();

            if (request.ID > 0)
            {
                //第二步:search单笔 + 返回response
                return GetMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_IDIsNull";
                response.ResponseStatus.Message = "No ID Parameter";
                return response;
            }
        }

        private GetItemCertificationByIDResponse GetMethod(GetItemCertificationByIDRequest request)
        {
            GetItemCertificationByIDResponse response = new GetItemCertificationByIDResponse();

            //查询质量认证和认证机构详情
            string sql = string.Format(@"select m.*, c.linkman as clinkman, c.phone as cphone, c.zipcode as czipcode, c.address as caddress, c.mm as cmm 
                                         from udoc_certification_application as m 
                                         left join udoc_certification as c on m.certificationname = c.n and m.ca = c.a where m.id = {0};", request.ID);
            var main = this.Db.QuerySingle<ItemCertification>(sql);
            if (main != null)
            {
                response.Data = main;

                //认证明细
                var detail = this.Db.Query<ItemCertificationDetail>(string.Format("select * from udoc_certification_appdetail where mid = {0}", request.ID));
                if (detail != null && detail.Count > 0)
                {
                    #region 认证报告附件

                    if (main.State == 1 || main.State == 2)
                    {
                        List<Resources> resourceDB = null;
                        List<ReturnPicResources> resourceList = new List<ReturnPicResources>();
                        ReturnPicResources resource = new ReturnPicResources();

                        //认证报告附件
                        foreach (var item in detail)
                        {
                            resourceList = new List<ReturnPicResources>();
                            resourceDB = this.Db.Where<Resources>(x => x.DocumentID == item.ID);
                            if (resourceDB != null && resourceDB.Count > 0)
                            {
                                foreach (var reportInfo in resourceDB)
                                {
                                    resource = ToResources(reportInfo, reportInfo.AccountID);
                                    resourceList.Add(resource);
                                }
                                item.PicResources = resourceList;
                            }
                        }
                    }

                    #endregion
                }
                response.Data.DetailData = detail;
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
