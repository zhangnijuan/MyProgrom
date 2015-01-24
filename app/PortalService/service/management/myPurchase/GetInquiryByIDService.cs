using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与lambda表达式有关
using ServiceStack.Common;
using Ndtech.PortalModel;


namespace Ndtech.PortalService.Auth
{
    public class GetInquiryByIDValidator : AbstractValidator<GetInquiryByIDRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(GetInquiryByIDRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (instance.ID <= 0)
            {
                result.Errors.Add(new ValidationFailure("ID", Const.Err_InquiryIDIsNull, "Err_InquiryIDIsNull"));
                return result;
            }
            if (instance.AccountId <= 0)
            {
                result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountIDIsNull, "Err_AccountIDIsNull"));
                return result;
            }
            return base.Validate(instance);
        }

    }
    public class GetInquiryByIDService : Service, IPost<GetInquiryByIDRequest>
    {
        public object Post(GetInquiryByIDRequest request)
        {
            GetInquiryByIDResponse response = new GetInquiryByIDResponse();

            //(我的询价和公开询价搜索调用) 先不加帐套条件 modify by yangshuo 2014/12/27
            //var InquiryInfo = this.Db.FirstOrDefault<PurInquiry>(n => n.ID == request.ID && n.AccountID == request.AccountId);
            var InquiryInfo = this.Db.FirstOrDefault<PurInquiry>(n => n.ID == request.ID);
            if (InquiryInfo != null)
            {
                var InquiryDetInfo = this.Db.Where<PurInquiryDetail>(n => n.MID == InquiryInfo.ID).OrderByDescending(n => n.ID);
                response.Data = ToInquiryInfo(InquiryInfo);//重要

                if (request.AccountId == InquiryInfo.AccountID)
                {
                    //查询优选资料
                    var purSelect = this.Db.FirstOrDefault<PurSelect>(n => n.InquiryID == request.ID && n.AccountID == InquiryInfo.AccountID);
                    if (purSelect != null)
                    {
                        response.Data.PurSelectID = purSelect.ID.ToString();
                        response.Data.PurSelectMM = purSelect.MM;
                    }
                }

                List<InquiryDetList> InquiryDetListModel = new List<InquiryDetList>();
                foreach (var Inquirydet in InquiryDetInfo)
                {
                    InquiryDetListModel.Add(ToInquiryDet(Inquirydet));
                    //已经转换为ViewModel 遍历ViewModel
                    foreach (var item in InquiryDetListModel)
                    {
                        List<ReturnPicResources> listModelResources = new List<ReturnPicResources>();
                        var Resources = this.Db.Where<Resources>(n => n.DocumentID == Convert.ToInt64(item.ID) && n.AccountID == item.AccountID);
                        foreach (var res in Resources)
                        {
                            listModelResources.Add(ToResources(res, item.AccountID));
                        }
                        item.PicResources = listModelResources;
                    }
                }

                //主表附件资源集合
                var mainDataRes = this.Db.Where<Resources>(n => n.DocumentID == request.ID && n.AccountID == InquiryInfo.AccountID);
                List<ReturnPicResources> mainPicRes = new List<ReturnPicResources>();
                foreach (var mainPic in mainDataRes)
                {
                    mainPicRes.Add(ToResources(mainPic, InquiryInfo.AccountID));
                }

                response.Data.PicResources = mainPicRes;
                response.Data.PurItemList = InquiryDetListModel;
                //response.Data.SupplierList = QuotationCompModel;

                //add询价公司名称 打开画面进行公开报价使用 2015/01/07
                var company = this.Db.FirstOrDefault<NdtechCompany>(x => x.AccountID == InquiryInfo.AccountID);
                if (company != null)
                {
                    response.Data.Purchaser = company.CompName;
                }
                response.Success = true;
            }
            else
            {
                //根据ID未找到资料
                response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                response.ResponseStatus.Message = Const.Err_NoInfoByID;
                response.Success = false;
            }

            return response;


        }
        private InquiryInfoList ToInquiryInfo(PurInquiry purInquiry)
        {
            return purInquiry.TranslateTo<InquiryInfoList>();
        }
        private InquiryDetList ToInquiryDet(PurInquiryDetail purInquiryDet)
        {
            return purInquiryDet.TranslateTo<InquiryDetList>();
        }
        private Ndtech.PortalModel.ReturnPicResources ToResources(Resources res, int accountID)
        {
            //return res.TranslateTo<ReturnPicResources>();
            var result = res.TranslateTo<Ndtech.PortalModel.ReturnPicResources>();
            result.FileUrl = string.Format("/fileuploads/{0}", accountID);
            return result;
        }
    }


}
