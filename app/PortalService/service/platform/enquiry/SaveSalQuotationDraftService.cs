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
    /// 保存报价
    /// add by liuzhiqiang 2014/12/18
    /// </summary>
    public class SaveSalQuotationDraftValidator : AbstractValidator<SaveSalQuotationDraftRequest>
    {
        public override ValidationResult Validate(SaveSalQuotationDraftRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //询价主题
            if (string.IsNullOrEmpty(instance.Subject))
            {
                result.Errors.Add(new ValidationFailure("Subject", Const.Err_SubjectIsNull, "Err_SubjectIsNull"));
                return result;
            }
            //报价截止日期
            if (string.IsNullOrEmpty(instance.FinalDateTime.ToString()))
            {
                result.Errors.Add(new ValidationFailure("FinalDateTime", Const.Err_FinalDateTimeIsNull, "Err_FinalDateTimeIsNull"));
                return result;
            }
            //至少有一条产品数据
            if (instance.PurItemList==null)
            {
                result.Errors.Add(new ValidationFailure("PurItemList", Const.Err_PurItemListIsNull, "Err_PurItemListIsNull"));
                return result;
            }

            //帐套
            if (instance.AccountID == 0)
            {
                result.Errors.Add(new ValidationFailure("Err_AccountID", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            foreach (PurItem sq in instance.PurItemList)
            {
                //产品代码
                if (string.IsNullOrEmpty(sq.ItemCode))
                {
                    result.Errors.Add(new ValidationFailure("ItemCode", Const.Err_ItemCodeIsNull, "Err_ItemCodeIsNull"));
                    return result;
                }
                //产品名称
                if (string.IsNullOrEmpty(sq.ItemName))
                {
                    result.Errors.Add(new ValidationFailure("ItemName", Const.Err_ItemNameIsNull, "Err_ItemNameIsNull"));
                    return result;
                }
                //产品类别
                if (string.IsNullOrEmpty(sq.CategoryThirdID.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("CategoryID", Const.Err_CategoryIDIsNull, "Err_CategoryIDIsNull"));
                    return result;
                }
                //数量
                if (string.IsNullOrEmpty(sq.Qty.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("Amount", Const.Err_AmountIsNull, "Err_AmountIsNull"));
                    return result;
                }
                //单位
                if (string.IsNullOrEmpty(sq.UnitID.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("UnitID", Const.Err_UnitIDIsNull, "Err_UnitIDIsNull"));
                    return result;
                }
                //交货期
                if (string.IsNullOrEmpty(sq.DeliveryDate.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("DeliveryDate", Const.Err_DeliveryDateIsNull, "Err_DeliveryDateIsNull"));
                    return result;
                }
            }
            //收货地址
            if (instance.AddressID <= 0)
            {
                result.Errors.Add(new ValidationFailure("AddressID", Const.Err_AddressIDIsNull, "Err_AddressIDIsNull"));
                return result;
            }
            if (string.IsNullOrEmpty(instance.Address))
            {
                result.Errors.Add(new ValidationFailure("Address", Const.Err_AddressNameIsNull, "Err_AddressNameIsNull"));
                return result;
            }
            //发布方式
            if (string.IsNullOrEmpty(instance.InquiryType.ToString()))
            {
                result.Errors.Add(new ValidationFailure("PublicType", Const.Err_PublicTypeIsnull, "Err_PublicTypeIsnull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class SaveSalQuotationDraftService : Service, IPost<SaveSalQuotationDraftRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<SaveSalQuotationDraftRequest> SaveSalQuotationDraftValidator { get; set; }
        ArrayList categoryList = new ArrayList();
        StringBuilder propertysb = new StringBuilder();
        int i = 0;
        SaveSalQuotationDraftResponse response = new SaveSalQuotationDraftResponse();

        public object Post(SaveSalQuotationDraftRequest request)
        {


                //第一步:校验前端的数据合法性
                ValidationResult result = SaveSalQuotationDraftValidator.Validate(request);
                if (!result.IsValid)
                {
                    response.Success = false;
                    response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                    response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                    return response;
                }
                //第二步:逻辑处理
                InsertSaveSalQuotationDraft(request);
            

                //if (i > 0)
                //{
                //    response.Success = false;
                //}
                
                //i = 0;
                return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 报价草稿
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void InsertSaveSalQuotationDraft(SaveSalQuotationDraftRequest request)
        {
            using (var trans = Db.BeginTransaction())
            {
               
                if (request.SupplierList != null)
                {
                    response.SaveSalQuotationDraftReturn = request;
                    response.Success = true;

                    foreach (Supplier supp in request.SupplierList)
                    {
                        //客户是否是黑名单
                        SaveCustomerItem(request, supp);
                        if (response.Success ==false)
                        {
                            continue;
                        }
                        var salQuotation = this.Db.FirstOrDefault<SalQuotation>(n => n.AccountID == supp.AccountID && n.InquiryCode == request.InquiryCode);
                        long recordID = RecordIDService.GetRecordID(1);

                        if (salQuotation==null)//没有报价
                        {
                            //前台主表model转成dataModel
                            SalQuotation sq = request.TranslateTo<SalQuotation>();
                            sq.ID = recordID;
                            sq.AccountID = supp.AccountID;
                            sq.CreateTime = DateTime.Now;
                            sq.InquiryDateTime = DateTime.Now;
                            sq.SCode = RecordSnumService.GetSnum(this, supp.AccountID, SnumType.OfferRelease);
                            sq.SCompanyName = supp.CompName;
                            sq.SLinkman = supp.SLinkman;
                            sq.SMobile = supp.SMobile;
                            sq.State = 0;
                          
                            Db.Insert(sq);

                            //明细转换为datamodel

                            foreach (PurItem pitem in request.PurItemList)
                            {
                                long recordDetailID = RecordIDService.GetRecordID(1);

                                if (!string.IsNullOrEmpty (request.InquiryCode))
                                {
                                    propertysb.Append(pitem.PropertyName);
                                }
                                else
                                {
                                    //获取属性
                                    SelectProperty(pitem);

                                }
                                if (pitem.PicResources != null)
                                {
                                    //附件
                                    foreach (PicResources pic in pitem.PicResources)
                                    {
                                        Resources resourceNew = ToEntity(pic);
                                        if (resourceNew != null && !string.IsNullOrEmpty(pic.OriginalName))
                                        {
                                            //2.2 insert物品图片资源
                                            resourceNew.Id = RecordIDService.GetRecordID(1);
                                            resourceNew.DocumentID = recordDetailID;
                                            resourceNew.AccountID = supp.AccountID;
                                            this.Db.Insert(resourceNew);
                                        }
                                    }
                                }
                                SalQuotationDetail pid = pitem.TranslateTo<SalQuotationDetail>();
                                pid.ID = recordDetailID;
                                pid.AccountID = supp.AccountID;
                                pid.Quantity = pitem.Qty;
                                pid.PropertyName = propertysb.ToString();
                                pid.MID = recordID;
                                pid.Inquirydid = pitem.ID;
                                Db.Insert<SalQuotationDetail>(pid);
                                propertysb.Length = 0;
                                categoryList.Clear();
                            }
                            response.SaveSalQuotationDraftReturn.ID = recordID;
                        }
                        else if (request.Eid!=0)
	                    {
                            response.SaveSalQuotationDraftReturn.ID = salQuotation.ID ;
                            response.Success = false;
	                    }
                    }
                    trans.Commit();

                }
                

            }
        }

        //产品属性
        private void SelectProperty(PurItem pitem)
        {
            if (pitem.PropertyList != null)
            {
                foreach (Property prop in pitem.PropertyList)
                {
                    propertysb.Append(prop.PropertyName + ":" + prop.PropertyValue + ",");
                }
            }

        }

        #region 保存客户档案 

        private void SaveCustomerItem(SaveSalQuotationDraftRequest request, Supplier supp)
        {
            NdtechCompany company = Db.QuerySingle<NdtechCompany>(string.Format("SELECT * FROM udoc_comp WHERE A =(SELECT A FROM pur_inquiry WHERE ID={0})", request.InquiryID));
            if (company != null)
            {
                UdocCustomer customer = Db.QuerySingle<UdocCustomer>(string.Format("SELECT * FROM udoc_customer WHERE Comp ={0} and a={1}", company.ID, supp.AccountID));
                if (customer == null)
                {
                    customer = new UdocCustomer();
                    customer.ID = RecordIDService.GetRecordID(1);
                    customer.AccountID = supp.AccountID;
                    customer.Comp = company.ID;  
                    customer.Cropnum = company.CompCode;
                    customer.CompName = company.CompName;
                    customer.State = 1;
                    this.Db.Save<UdocCustomer>(customer);
                    response.Success = true;

                }
                else if (customer.State == 0)//黑名单
                {
                    response.ResponseStatus.ErrorCode = "Err_BlackCustomer";
                    response.ResponseStatus.Message = Const.Err_BlackCustomer;
                    response.Success = false;
                }
                else if (customer.State == 1)//白名单
                {
                    response.Success = true;
                }
            }
           
        }


        #endregion

        private Resources ToEntity(PicResources item)
        {
            return item.TranslateTo<Resources>();
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
        #endregion
    }
}
