using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface.Validation;
using Ndtech.PortalService.Extensions;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;

namespace Ndtech.PortalService.Auth
{
    public class RetrieveVdTelCodeService : Service, IGet<ValidateTelCodeRequest>
    {
        #region IPost<RegisteredRequest> 成员
        public IDbConnectionFactory db { set; get; }
        public object Get(ValidateTelCodeRequest request)
        {
            ValidateTelCodeRespones response = new ValidateTelCodeRespones();
            ValiDateTelCode(request, response);
            return response;
        }
        private void ValiDateTelCode(ValidateTelCodeRequest request, ValidateTelCodeRespones response)
        {
            string TelCode = request.TelCode;
            string UserName = request.UserName;
            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    NdtechStaffInfo exsituser = conn.QuerySingle<NdtechStaffInfo>(string.Format("select * from udoc_staff where loginname = '{0}';", UserName));
                    if (exsituser != null)
                    {
                        string Phone = exsituser.TelNum;
                        NdtechTelCode exsittelcode = conn.QuerySingle<NdtechTelCode>(string.Format("select * from gl_telcode where validatecode = '{0}' and tel='{1}'", TelCode, Phone));
                        if (exsittelcode != null)
                        {
                            response.Success = true;
                        }
                        else
                            response.Success = false;
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
                finally
                {
                    conn.Close();
                }  
            }
        }
        #endregion
    }
}
