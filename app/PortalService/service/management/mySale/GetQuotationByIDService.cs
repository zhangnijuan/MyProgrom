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
using ServiceStack.OrmLite;
using ServiceStack.Common;
using Ndtech.PortalModel;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    public class GetQuotationByIDValidator : AbstractValidator<GetQuotationByIDRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(GetQuotationByIDRequest instance)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(instance.ID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("ID", Const.Err_QuotationIDIsNull, "Err_QuotationIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.AccountId.ToString()))
            {
                result.Errors.Add(new ValidationFailure("AccountID", Const.Err_AccountIDIsNull, "Err_AccountIDIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }
    public class GetQuotationByIDService : Service, IGet<GetQuotationByIDRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetQuotationByIDService));
        public object Get(GetQuotationByIDRequest request)
        {
            GetQuotationByIDResponse response = new GetQuotationByIDResponse();

            //询价和报价会共用此接口，所以暂时将帐套去除 modify by yangshuo
            var Quotation = this.Db.FirstOrDefault<SalQuotation>(n => n.ID == request.ID);
            if (Quotation != null)
            {
                response.Data = ToQuotationInfo(Quotation);//重要

                List<QuotationDetList> QuotationDetModel = new List<QuotationDetList>();
                var QuotationDet = this.Db.Where<SalQuotationDetail>(n => n.MID == Quotation.ID);
                if (QuotationDet != null && QuotationDet.Count > 0)
                {
                    //查询该企业所有发布产品
                    List<EnterpriseItem> itemList = this.Db.Where<EnterpriseItem>(x => x.AccountID == Quotation.AccountID && x.BizType == 2 && x.State == 1);
                    bool searchItemCode = false;
                    if (itemList != null && itemList .Count > 0)
                    {
                        searchItemCode = true;
                    }

                    EnterpriseItem itemExist = null;
                    foreach (var quot in QuotationDet)
                    {
                        QuotationDetList detail = ToQuotationDet(quot);
                        detail.Price = quot.Prc;
                        detail.Amount = quot.Amt;
                        detail.ItemIsPublish = false;

                        if (searchItemCode)
                        {
                            //查询产品是否在该企业发布
                            itemExist = itemList.FirstOrDefault(x => x.StandardItemCode == detail.StandardItemCode);
                            if (itemExist != null)
                            {
                                detail.ItemIsPublish = true;

                                //报价方产品描述赋值
                                detail.SRemark = itemExist.Remark;
                            }
                        }

                        QuotationDetModel.Add(detail);
                        foreach (var item in QuotationDetModel)
                        {
                            List<Ndtech.PortalModel.ReturnPicResources> listModelResources = new List<Ndtech.PortalModel.ReturnPicResources>();

                            //根据询价明细档id查询询价明细附件 modify by yangshuo 
                            var Resources = this.Db.Where<Resources>(n => n.DocumentID == Convert.ToInt64(item.Inquirydid));
                            foreach (var res in Resources)
                            {
                                //参数为询价帐套
                                listModelResources.Add(ToResources(res, res.AccountID));
                            }

                            item.PicResources = listModelResources;
                        }
                    }
                }

                //报价主档附件
                var mainDataRes = this.Db.Where<Resources>(n => n.DocumentID == request.ID);
                List<ReturnPicResources> mainPicRes = new List<ReturnPicResources>();
                foreach (var mainPic in mainDataRes)
                {
                    mainPicRes.Add(ToResources(mainPic, mainPic.AccountID));
                }

                response.Data.PicResources = mainPicRes;

                //根据询价主档id查询询价主档附件 add by yangshuo
                mainDataRes = this.Db.Where<Resources>(n => n.DocumentID == Quotation.InquiryID);
                mainPicRes = new List<ReturnPicResources>();
                foreach (var mainPic in mainDataRes)
                {
                    mainPicRes.Add(ToResources(mainPic, mainPic.AccountID));
                }
                response.Data.PurPicResources = mainPicRes;

                response.Data.ItemList = QuotationDetModel;

                //已报价企业数量
                var purInquiry = this.Db.FirstOrDefault<PurInquiry>(n => n.ID == Quotation.InquiryID);
                if (purInquiry != null)
                {
                    response.Data.Quotations = purInquiry.Quotations;

                    //取出询价方主档备注add by ys 2015/01/13
                    response.Data.PMM = purInquiry.MM;
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
        private QuotationDetList ToQuotationDet(SalQuotationDetail quotdet)
        {
            return quotdet.TranslateTo<QuotationDetList>();
        }
        private QuotationInfoList ToQuotationInfo(SalQuotation quot)
        {
            return quot.TranslateTo<QuotationInfoList>();
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
