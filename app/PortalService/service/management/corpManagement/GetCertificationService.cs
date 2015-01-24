using System;
using System.Linq;
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

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 查询认证机构列表Service
    /// add by yangshuo 2015/01/05
    /// </summary>
    public class GetCertificationService : Service, IPost<GetCertificationRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetCertificationService));

        public object Post(GetCertificationRequest request)
        {
            GetCertificationResponse response = new GetCertificationResponse();
            return PostMethod(request);
        }

        private GetCertificationResponse PostMethod(GetCertificationRequest request)
        {
            GetCertificationResponse response = new GetCertificationResponse();
            
            //1.查询认证机构列表
            StringBuilder sSql = new StringBuilder("select * from udoc_certification");
            if (!string.IsNullOrEmpty(request.CertificationName))
            {
                sSql.Append(string.Format(" where n like '%{0}%';",request.CertificationName));
            }
            else
            {
                sSql.Append(";");
            }

            List<Certification> certificationlist = this.Db.Query<Certification>(sSql.ToString());
            if (certificationlist != null && certificationlist.Count > 0)
            {
                //2.获取总行数
                response.RowsCount = certificationlist.Count;
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                if (request.PageIndex  > 0 && request.PageSize > 0)
                {
                    //3.分页
                    certificationlist = certificationlist.OrderBy(x => x.CertificationName).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                }
                
                //4.data赋值
                response.Data = certificationlist;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Mes_NoData";
                response.ResponseStatus.Message = "No Data";
            }

            response.Success = true;
            return response;
        }
    }
}
