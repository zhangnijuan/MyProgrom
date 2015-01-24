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
using System.Collections;

namespace Ndtech.PortalService.Auth
{
    public class GetPurInquiryService : Service, IGet<GetPurInquiryRequest>
    {
        public object Get(GetPurInquiryRequest request)
        {

            var purInquiry = this.Db.FirstOrDefault<PurInquiry>(n => n.InquiryCode == request.InquiryCode);
            var purInquiryDetail = this.Db.Where<PurInquiryDetail>(n => n.MID == purInquiry.ID);
            GetPurInquiryResponse response = new GetPurInquiryResponse();
            response.Data = ToPurInquiry(purInquiry);
            //转换为前端model
            List<PurItem> listModel = new List<PurItem>();
            foreach (var item in purInquiryDetail)
            {
                listModel.Add(ToPurInquiryDetail(item));
            }
            response.Data.PurItemList = listModel;
            response.Success = true;
            return response;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private PurInquiryInfo ToPurInquiry(PurInquiry purInquiry)
        {

            return purInquiry.TranslateTo<PurInquiryInfo>();
        }

        private PurItem ToPurInquiryDetail(PurInquiryDetail item)
        {
            return item.TranslateTo<PurItem>();
        }
    }
}
