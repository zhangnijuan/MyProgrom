using System;
using System.Linq;
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
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 企业地址薄查询Service
    /// add by yangshuo 2014/12/16
    /// </summary>
    public class GetERPAddressService : Service, IPost<GetEnterpriseAddressRequest>
    {

        public object Post(GetEnterpriseAddressRequest request)
        {
            GetERPAddressResponse response = new GetERPAddressResponse();

            //第一步:校验前端的数据合法性
            //第二步:search企业地址薄 + 返回response
            return PostMethod(request);
        }

        private GetERPAddressResponse PostMethod(GetEnterpriseAddressRequest request)
        {
            GetERPAddressResponse response = new GetERPAddressResponse();

            try
            {
                List<EnterpriseAddress> listInfo = this.Db.Where<EnterpriseAddress>(x => x.AccountID == request.AccountID);

                if (listInfo != null && listInfo.Count > 0)
                {
                    //3.获取总行数
                    response.RowsCount = listInfo.Count;

                    if (request.UserCode != "S001")
                    {
                        //非管理员从明细档案查询默认地址
                        EnterpriseDefAddress detail = this.Db.FirstOrDefault<EnterpriseDefAddress>(x => x.AccountID == request.AccountID && x.EID == request.UserID);
                        if (detail != null)
                        {
                            //修改id = detail.mid的isdef为默认
                            if (listInfo.Find(x => x.ID == detail.MID) != null)
                            {
                                if (listInfo.Find(x => x.IsDef == 1) != null)
                                {
                                    //将管理员的默认地址改成0
                                    listInfo.Find(x => x.IsDef == 1).IsDef = 0;
                                }
                                //取出员工的默认地址
                                listInfo.Find(x => x.ID == detail.MID).IsDef = 1;
                            }
                        }
                    }

                    if (request.PageIndex > 0 && request.PageSize > 0)
                    {
                        //4.分页
                        listInfo = listInfo.OrderByDescending(x => x.ID).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                    }

                    if (request.IsDefFirst == "Y")
                    {
                        //默认地址排在第一位 询价、报价、发布供应产品绑定使用
                        listInfo = listInfo.OrderByDescending(x => x.IsDef).ToList();
                    }

                    //5.转化成前台model集合
                    List<ERPAddressView> viewList = new List<ERPAddressView>();
                    foreach (var itemData in listInfo)
                    {
                        ERPAddressView itemView = itemData.TranslateTo<ERPAddressView>();
                        viewList.Add(itemView);
                    }

                    response.Data = viewList;
                    response.Success = true;
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoData";
                    response.ResponseStatus.Message = "No data";
                    response.Success = true;
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

