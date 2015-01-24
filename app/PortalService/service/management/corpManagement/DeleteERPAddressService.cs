using System;
using System.IO;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 企业地址簿删除Service
    /// add by yangshuo 2014/12/16
    /// </summary>
    public class DeleteERPAddressService : Service, IPost<DeleteERPAddressRequest>
    {
        public object Post(DeleteERPAddressRequest request)
        {
            EnterpriseAddressResponse response = new EnterpriseAddressResponse();

            //第一步:校验前端的数据合法性
            if (request.ID <= 0)
            {
                response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                response.ResponseStatus.Message = Const.Err_NoInfoByID;
                return response;
            }
            else
            {
                //第二步:delete + 返回response
                return PostMethod(request);
            }
        }

        private EnterpriseAddressResponse PostMethod(DeleteERPAddressRequest request)
        {
            EnterpriseAddressResponse response = new EnterpriseAddressResponse();

            try
            {
                //删除地址簿
                this.Db.Delete<EnterpriseAddress>(string.Format("a = {0} and id = {1}", request.AccountID, request.ID));
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
