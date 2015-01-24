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
    /// <summary>
    /// 保存报价草稿
    /// add by liuzhiqiang 2014/12/17
    /// </summary>
    public class SalQuotationValidator : AbstractValidator<SalQuotationRequest>
    {
        public override ValidationResult Validate(SalQuotationRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class SalQuotationService : Service, IPost<SalQuotationRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<SalQuotationRequest> SalQuotationValidator { get; set; }

        StringBuilder propertysb = new StringBuilder();
        SalQuotationResponse response = new SalQuotationResponse();

        #region Post

        public object Post(SalQuotationRequest request)
        {
            //第一步:校验前端的数据合法性
            ValidationResult result = SalQuotationValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:逻辑处理
            SaveSalQuotation(request);
            if ( response.Success == true)
            {
                //保存交易日志
                SaveDealLog(request);
                //更新报价企业数量
                UpdateSalCompCount(request);
            }

            return response;
        }

        #endregion

        #region 逻辑处理

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void SaveSalQuotation(SalQuotationRequest request)
        {
            //1.2 组织产品图片待insert
            List<Resources> resourcesList = new List<Resources>();

            if (request.PicResources != null)
            {
                Resources resourceNew = null;

                foreach (var picView in request.PicResources)
                {
                    resourceNew = picView.TranslateTo<Resources>();
                    if (resourceNew.Id <= 0)
                    {
                        //新上传的图片,直接insert
                     
                        resourceNew.Id = RecordIDService.GetRecordID(1);
                        resourceNew.DocumentID = request.ID;
                        resourceNew.AccountID = request.AccountID;
                        resourcesList.Add(resourceNew);
                    }
                  
                }

            }

            using (var trans = Db.BeginTransaction())
            {
                var salQuotation = this.Db.FirstOrDefault<SalQuotation>(n => n.ID == request.ID && n.State != 1);
                if (salQuotation != null)//没有报价
                {
                    salQuotation.QuoteExplain = request.QuoteExplain;
                    salQuotation.SFinalDateTime = request.SFinalDateTime;
                    salQuotation.SCompanyName = request.SCompanyName;
                    salQuotation.SLinkman = request.SLinkman;
                    salQuotation.SMobile = request.SMobile;
                    salQuotation.SFixedLine = request.SFixedLine;
                    salQuotation.SFax = request.SFax;
                    salQuotation.SAddress = request.SAddress;
                    salQuotation.TotalAmt = request.TotalAmt;
                    salQuotation.Eid  = request.Eid;
                    salQuotation.EidCode = request.EidCode;
                    salQuotation.EidName = request.EidName;
                    salQuotation.SendAddressID = request.SendAddressID;
                    salQuotation.State = 1;
                    salQuotation.CreateTime = DateTime.Now;
                    this.Db.Save<SalQuotation>(salQuotation);

                    if (request.PicResources != null)
                    {
                        //2.1.2 insert新上传的图片
                        foreach (var resourceNew in resourcesList)
                        {
                            this.Db.Insert(resourceNew);
                        }

                    }

                    //明细转换为datamodel
                    if (request.ItemList!=null)
                    {
                        foreach (SalItem pitem in request.ItemList)
                        {

                            var salQuotationDetail = this.Db.FirstOrDefault<SalQuotationDetail>(n => n.ID == pitem.ID);
                            if (salQuotationDetail != null)
                            {
                                salQuotationDetail.Prc = pitem.Price;
                                salQuotationDetail.Remark = pitem.Remark;
                                salQuotationDetail.Amt = pitem.Amount;
                                salQuotationDetail.SRemark = pitem.SRemark;
                                salQuotationDetail.SMM = pitem.SMM;
                                this.Db.Save<SalQuotationDetail>(salQuotationDetail);

                            }


                        }
                    }

                    //供应商是否是黑名单
                    SaveSupplierItem(request);
                    //供应商不是黑名单
                    if (response.Success == true)
                    {

                        //保存报价企业信息
                        //查询是否有报价企业
                        List<SalQuotationComp> salQuotationComp = null;
                        salQuotationComp = this.Db.Where<SalQuotationComp>(x => x.MID == request.InquiryID && x.OfferID == request.ID);
                        if (salQuotationComp.Count == 0)
                        {
                            long recordID = RecordIDService.GetRecordID(1);

                            var purInquiry = this.Db.FirstOrDefault<PurInquiry>(n => n.ID == request.InquiryID);
                            var ndtechAcntSystem = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.ID == request.AccountID);
                            SalQuotationComp pi = new SalQuotationComp
                            {
                                ID = recordID,
                                AccountID = purInquiry.AccountID,
                                MID = request.InquiryID,
                                CompName = request.SCompanyName,
                                CorpNum = ndtechAcntSystem.CorpNum,
                                Amount = request.TotalAmt,
                                OfferID = request.ID,
                                OfferSnum = request.SCode,
                                SFinalDateTime = request.SFinalDateTime,
                                CreateDate = DateTime.Now
                            };
                            Db.Insert(pi);

                        }
                    }
                    trans.Commit();
                    response.Success = true;
                }
                else
                {
                    response.ResponseStatus.Message = "已报价";
                    response.Success = false;
                }
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private SalQuotation ToEntity(SalQuotationRequest request)
        {
            return request.TranslateTo<SalQuotation>();
        }

        private SalQuotationDetail ToEntity(SalItem request)
        {
            return request.TranslateTo<SalQuotationDetail>();
        }


        private Resources ToEntity(PicResources item)
        {
            return item.TranslateTo<Resources>();
        }
        #endregion

        #region 保存交易日志  add by 苏阳 2014-12-18

        private void SaveDealLog(SalQuotationRequest request)
        {


            string InquiryID = request.InquiryID.ToString();
            NdtechCompany PurCompany = GetPurUdoc_comp(InquiryID);
            NdtechCompany SalCompany = GetSalUdoc_comp(request.AccountID);

            if (PurCompany == null || SalCompany == null)
            {
                return;
            }
            foreach (SalItem pitem in request.ItemList)
            {
                //询价单的编码
                string PurSCode = request.InquiryCode;
                //企业表示
                int AccountID = request.AccountID;
                //物品编码
                string SalItemCode = pitem.ItemCode;
                NdtechDealLog deallog = this.Db.QuerySingle<NdtechDealLog>(string.Format("select * from udoc_deallog where scode = '{0}' and a={1} and salitem_c='{2}'", PurSCode, AccountID, SalItemCode));
                //如果没有交易日志则新增
                if(deallog==null)
                {
                    InsertDealLog(PurCompany, SalCompany, pitem, request);
                }
                //如果有交易日志则更新
                else
                {
                    UpdateDealLog(PurCompany, SalCompany, pitem, request, deallog.ID);
                }

            }
        }
        private void InsertDealLog(NdtechCompany PurCompany, NdtechCompany SalCompany, SalItem pitem, SalQuotationRequest request)
        {
            NdtechDealLog deallog = new NdtechDealLog();
            deallog.ID = RecordIDService.GetRecordID(1);
            deallog.AccountID = request.AccountID;
            if (PurCompany != null)
            {
                deallog.PurCode = PurCompany.CompCode;
                deallog.PurName = PurCompany.CompName;
            }
            deallog.SCode = request.InquiryCode;
            deallog.Subject = request.Subject;

            if (SalCompany != null)
            {
                deallog.Supply = SalCompany.ID;
                deallog.SupplyCode = SalCompany.CompCode;
                deallog.SupplyName = SalCompany.CompName;
            }
            deallog.StandardItemCode = pitem.StandardItemCode;
            deallog.StandardItemName = pitem.StandardItemName;
            deallog.PurItemCode = pitem.ItemCode;
            deallog.PurItemName = pitem.ItemName;
            deallog.SalItemCode = pitem.ItemCode;//
            deallog.SalItemName = pitem.ItemName;//
            deallog.PropertyName = pitem.PropertyName;
            deallog.CategoryCode = pitem.CategoryCode;
            deallog.CategoryName = pitem.CategoryName;
            deallog.UName = pitem.UnitName;
            deallog.PurQty = pitem.Quantity;
            deallog.SalPrc = pitem.Price;
            deallog.SalAmt = pitem.Amount;

            this.Db.Save<NdtechDealLog>(deallog);
        }
        private void UpdateDealLog(NdtechCompany PurCompany, NdtechCompany SalCompany, SalItem pitem, SalQuotationRequest request, long ID)
        {
            NdtechDealLog deallog = new NdtechDealLog();
            deallog.ID = ID;
            deallog.AccountID = request.AccountID;
            if (PurCompany != null)
            {
                deallog.PurCode = PurCompany.CompCode;
                deallog.PurName = PurCompany.CompName;
            }
            if (SalCompany != null)
            {
                deallog.Supply = SalCompany.ID;
                deallog.SupplyCode = SalCompany.CompCode;
                deallog.SupplyName = SalCompany.CompName;
            }
            deallog.SCode = request.InquiryCode;
            deallog.Subject = request.Subject;
            deallog.StandardItemCode = pitem.StandardItemCode;
            deallog.StandardItemName = pitem.StandardItemName;
            deallog.PurItemCode = pitem.ItemCode;
            deallog.PurItemName = pitem.ItemName;
            deallog.SalItemCode = pitem.ItemCode;//
            deallog.SalItemName = pitem.ItemName;//
            deallog.PropertyName = pitem.PropertyName;
            deallog.CategoryCode = pitem.CategoryCode;
            deallog.CategoryName = pitem.CategoryName;
            deallog.UName = pitem.UnitName;
            deallog.PurQty = pitem.Quantity;
            deallog.SalPrc = pitem.Price;
            deallog.SalAmt = pitem.Amount;

            this.Db.Save<NdtechDealLog>(deallog);
        }
        /// <summary>
        /// 取出询价单的企业信息
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        private NdtechCompany GetPurUdoc_comp(string InquiryID)
        {
            NdtechCompany company = Db.QuerySingle<NdtechCompany>(string.Format("SELECT * FROM udoc_comp WHERE A =(SELECT A FROM pur_inquiry WHERE ID={0})", InquiryID));
            if (company != null)
            {
                return company;
            }
            return null;
        }
        /// <summary>
        /// 取出供应商企业信息
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        private NdtechCompany GetSalUdoc_comp(int accountID)
        {
            NdtechCompany company = Db.QuerySingle<NdtechCompany>(string.Format("SELECT * FROM udoc_comp WHERE A ={0}", accountID));
            if (company != null)
            {
                return company;
            }
            return null;
        }
        /// <summary>
        /// 取出询价单信息
        /// </summary>
        /// <param name="InquiryID"></param>
        /// <returns></returns>
        //private NdtechCompany GetPurInquiry(string InquiryID)
        //{
        //    NdtechCompany company = Db.QuerySingle<NdtechCompany>(string.Format("SELECT * FROM pur_inquiry WHERE ID={0}", InquiryID));
        //    if (company != null)
        //    {
        //        return company;
        //    }
        //    return null;
        //}
        #endregion

        #region 更新询价企业数量  add by 苏阳 2014-12-19

        private void UpdateSalCompCount(SalQuotationRequest request)
        {
            long InquiryID = request.InquiryID;
            var list = this.Db.Where<SalQuotation>(n => n.InquiryID == InquiryID && n.State == 1);
            int count = list.Count;

            string sql = string.Format("UPDATE pur_inquiry SET quotations={0} WHERE ID={1}", count, InquiryID);
            this.Db.ExecuteNonQuery(sql);
        }

        #endregion

        #region 保存供应商档案

        private void SaveSupplierItem(SalQuotationRequest request)
        {
            //保存供应商档案
            NdtechCompany company = Db.FirstOrDefault<NdtechCompany>(n => n.AccountID == request.AccountID); 
            if (company != null)
            {
                var ndtechAcntSystem = this.Db.FirstOrDefault<NdtechAcntSystem>(n => n.ID == request.AccountID);
                PurInquiry purInquiry = Db.QuerySingle<PurInquiry>(string.Format("SELECT A FROM pur_inquiry WHERE ID={0}", request.InquiryID));
                UdocSupplier supplier = Db.QuerySingle<UdocSupplier>(string.Format("SELECT * FROM udoc_supplier WHERE Comp ={0}and a={1}", company.ID, purInquiry.AccountID));
                if (supplier == null)
                {
                    supplier = new UdocSupplier();
                    supplier.ID = RecordIDService.GetRecordID(1);
                    supplier.AccountID = purInquiry.AccountID;
                    supplier.Comp = company.ID;
                    supplier.Cropnum = ndtechAcntSystem.CorpNum;
                    supplier.CompName = company.CompName;
                    supplier.State = 1;
                    this.Db.Save<UdocSupplier>(supplier);
                }
                else if (supplier.State == 0)//黑名单
                {
                    response.Success = false;
                }
                response.Success = true;

            }

        }
        
        #endregion
    }
}
