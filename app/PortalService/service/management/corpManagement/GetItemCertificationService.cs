using System;
using System.Text;
using System.Linq;
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
    /// 认证信息列表Service
    /// add by yangshuo 2015/01/06
    /// </summary>
    public class GetItemCertificationService : Service, IPost<GetItemCertificationRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemCertificationService));

        #region Post列表

        public object Post(GetItemCertificationRequest request)
        {
            ItemCertificationResponse response = new ItemCertificationResponse();

            if (request.PageIndex > 0 && request.PageSize > 0 && (request.AccountID > 0 || request.CAccountID > 0))
            {
                if (request.AccountID > 0 && request.CAccountID > 0)
                {
                    response.ResponseStatus.ErrorCode = "Error";
                    response.ResponseStatus.Message = "AccountID and CAccountID ,there can be only one appearance";
                    return response;
                }
                //第二步:search列表 + response
                return PostMethod(request);
            }
            else
            {
                #region error

                if (request.AccountID <= 0)
                {
                    response.ResponseStatus.ErrorCode = "Err_AccountIDIsNull";
                    response.ResponseStatus.Message = "No AccountID Parameter";
                }
                else if (request.CAccountID <= 0)
                {
                    response.ResponseStatus.ErrorCode = "Err_CAccountIDIsNull";
                    response.ResponseStatus.Message = "No CAccountID Parameter";
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_ParameterIsNull";
                    response.ResponseStatus.Message = "No PageIndex or PageSize Parameter";
                }
                return response;

                #endregion
            }
        }

        private ItemCertificationResponse PostMethod(GetItemCertificationRequest request)
        {
            ItemCertificationResponse response = new ItemCertificationResponse();

            //1.拼接sql
            StringBuilder sSql = new StringBuilder("select * from udoc_certification_application");
            if (request.AccountID > 0)
            {
                sSql.Append(string.Format(" where a = {0}", request.AccountID));
            }
            else if (request.CAccountID > 0)
            {
                sSql.Append(string.Format(" where ca = {0}", request.CAccountID));
            }

            if (!string.IsNullOrEmpty(request.CertificationName))
            {
                //受理机构名称筛选
                sSql.Append(string.Format(" and certificationname like '%{0}%'", request.CertificationName));
            }

            if (request.State >= 0)
            {
                if (request.State == 1)
                {
                    //已处理
                    sSql.Append(string.Format(" and state = {0}", request.State));
                }
                else
                {
                    //未处理、待处理
                    sSql.Append(string.Format(" and state != 1", request.State));
                }
            }

            if (!string.IsNullOrEmpty(request.CodeOrWhatsoever))
            {
                sSql.Append(string.Format(" and (c like '%{0}%' or whatsoever like '%{0}%')", request.CodeOrWhatsoever));
            }

            sSql.Append(";");

            //2.查询质量认证列表
            List<ItemCertification> list = this.Db.Query<ItemCertification>(sSql.ToString());
            if (list != null && list.Count > 0)
            {
                //3.总笔数
                response.RowsCount = list.Count;

                //4.分页
                if (request.orders != null && request.orders.Count > 0)
                {
                    request.PageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
                    //日期排序
                    foreach (var order in request.orders)
                    {
                        if (order.orderKey == OrderKey.createdate)
                        {
                            if (order.sortKey == SortKey.Ascending)
                            {
                                list = list.OrderBy(x => x.CreateDate).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.CreateDate).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                            }
                        }
                    }
                }
                else
                {
                    list = list.OrderByDescending(x => x.CreateDate).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                }

                long count = 0;
                foreach (var item in list)
                {
                    //5.循环查询认证产品个数
                    count = this.Db.Count<EnterpriseItemsCertificationDetail>(x => x.Mid == item.ID);
                    item.AcceptCount = Convert.ToInt32(count);
                }

                response.Data = list;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Msg_NoData";
                response.ResponseStatus.Message = "No Data";
            }
            response.Success = true;
            return response;
        }

        #endregion
    }
}
