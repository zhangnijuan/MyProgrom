using System;
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

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 产品价格趋势Service
    /// add by yangshuo 2015/01/16
    /// </summary>
    public class GetItemPriceTrendService : Service, IPost<GetItemPriceTrendRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemPriceTrendService));

        public object Post(GetItemPriceTrendRequest request)
        {
            GetItemPriceTrendResponse response = new GetItemPriceTrendResponse();

            if (request.ID > 0 && !string.IsNullOrEmpty(request.StandardItemCode))
            {
                //search + 回response
                return PostMethod(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Error_ParameterIsNull";
                response.ResponseStatus.Message = "No ID or StandardItemCode Parameter";
                return response;
            }
        }

        private GetItemPriceTrendResponse PostMethod(GetItemPriceTrendRequest request)
        {
            GetItemPriceTrendResponse response = new GetItemPriceTrendResponse();

            #region db中企业和平台产品价格资料

            //结束时间默认为系统时间-1天,精确到23:59:59
            DateTime dtEnd = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            if (!string.IsNullOrEmpty(request.OrderTime))
            {
                //若有下单时间参数,则做为结束时间,下单时间要精确到时间点
                dtEnd = Convert.ToDateTime(request.OrderTime);
            }

            //1.根据企业产品id和截止日期查询6个月的价格资料
            //先查询小于dtEnd的产品价格资料
            List<EnterpriseItemPrice> erpPrcList = this.Db.Where<EnterpriseItemPrice>(x => x.ItemID == request.ID && x.PriceStartDate <= dtEnd);
            List<EnterpriseItemPrice> erpPrcDescList = null;
            if (erpPrcList != null && erpPrcList.Count > 0)
            {
                //1.1 企业产品价格->按日期降序排列
                erpPrcDescList = erpPrcList.OrderByDescending(x => x.PriceStartDate).ToList();
            }

            //2.平台产品价格->按日期升序排列
            List<NdtechItemPrice> platPrcList = null;
            List<NdtechItemPrice> platDescPrcList = null;

            //2.0先获取平台产品id
            NdtechItem item = this.Db.FirstOrDefault<NdtechItem>(x => x.ItemCode == request.StandardItemCode.Trim());
            if (item != null)
            {
                platPrcList = this.Db.Where<NdtechItemPrice>(x => x.ItemID == item.ID && x.PriceStartDate <= dtEnd);
                if (platPrcList != null && platPrcList.Count > 0)
                {
                    //2.1 平台产品价格->按日期降序排列
                    platDescPrcList = platPrcList.OrderByDescending(x => x.PriceStartDate).ToList();
                }
            }
            else
            {
                //平台无该产品
                response.ResponseStatus.ErrorCode = "No Data";
                response.ResponseStatus.Message = "No ItemInfo in PlatForm";
                return response;
            }

            #endregion

            List<ItemPriceTrend> trendList = new List<ItemPriceTrend>();
            ItemPriceTrend trend = null;
            EnterpriseItemPrice erpPrice = null;
            NdtechItemPrice platPrice = null;

            if ((erpPrcList != null && erpPrcList.Count > 0) || (platPrcList != null && platPrcList.Count > 0))
            {
                //3.先获取近半年一共有多少天
                DateTime dtStart = dtEnd.AddMonths(-6);
                TimeSpan tspan = dtEnd.Subtract(dtStart);
                int count = tspan.Days + 1;

                //3.筛选or补充获取近半年产品价格资料,按天呈现
                for (int i = 0; i < count; i++)
                {
                    trend = new ItemPriceTrend();
                    trend.Date = dtStart.AddDays(i).ToShortDateString();

                    #region 企业价格

                    if (erpPrcList != null && erpPrcList.Count > 0)
                    {
                        //取<=当前查询日期最近一笔的价格
                        erpPrice = erpPrcDescList.Find(x => x.PriceStartDate <= dtStart.AddDays(i));
                        if (erpPrice != null)
                        {
                            //企业价格
                            trend.ERPPrice = erpPrice.Price;
                        }
                    }

                    #endregion

                    #region 平台价格

                    if (platPrcList != null && platPrcList.Count > 0)
                    {
                        //取本月价格资料
                        platPrice = platDescPrcList.Find(x => x.PriceStartDate <= dtStart.AddDays(i));
                        if (platPrice != null)
                        {
                            //平台价格
                            trend.PlatPrice = platPrice.Price;
                        }
                    }

                    #endregion

                    //累加资料集合
                    trendList.Add(trend);
                }

                response.Data = trendList;
                response.Success = true;
            }
            else
            {
                //无企业产品价格资料or无平台价格资料
                response.ResponseStatus.ErrorCode = "No Data";
                response.ResponseStatus.Message = "No EnterpriseItemPrice and PlatFormItemPrice Info";
            }

            return response;
        }
    }
}
