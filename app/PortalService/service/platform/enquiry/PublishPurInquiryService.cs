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
    /// 保存询价
    /// add by liuzhiqiang 2014/12/8
    /// </summary>
    public class PurInquiryValidator : AbstractValidator<PurInquiryRequest>
    {
        public override ValidationResult Validate(PurInquiryRequest instance)
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
            if (instance.PurItemList.Count == 0)
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

            foreach (PurItem pi in instance.PurItemList)
            {
                //产品代码
                if (string.IsNullOrEmpty(pi.ItemCode))
                {
                    result.Errors.Add(new ValidationFailure("ItemCode", Const.Err_ItemCodeIsNull, "Err_ItemCodeIsNull"));
                    return result;
                }
                //产品名称
                if (string.IsNullOrEmpty(pi.ItemName))
                {
                    result.Errors.Add(new ValidationFailure("ItemName", Const.Err_ItemNameIsNull, "Err_ItemNameIsNull"));
                    return result;
                }
                //产品类别
                if (string.IsNullOrEmpty(pi.CategoryThirdID.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("CategoryID", Const.Err_CategoryIDIsNull, "Err_CategoryIDIsNull"));
                    return result;
                }
                //数量
                if (string.IsNullOrEmpty(pi.Qty.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("Amount", Const.Err_AmountIsNull, "Err_AmountIsNull"));
                    return result;
                }
                //单位
                if (string.IsNullOrEmpty(pi.UnitID.ToString()))
                {
                    result.Errors.Add(new ValidationFailure("UnitID", Const.Err_UnitIDIsNull, "Err_UnitIDIsNull"));
                    return result;
                }
                //交货期
                if (string.IsNullOrEmpty(pi.DeliveryDate.ToString()))
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

    public class PublishPurInquiryService : Service, IPost<PurInquiryRequest>
    {
        public IDbConnectionFactory db { set; get; }
        public IValidator<PurInquiryRequest> PurInquiryValidator { get; set; }
        ArrayList categoryList = new ArrayList();
        StringBuilder propertysb = new StringBuilder();
        public object Post(PurInquiryRequest request)
        {
            PurInquiryResponse response = new PurInquiryResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = PurInquiryValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:逻辑处理
            InsertPurInquiry(request);

            response.Success = true;
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void InsertPurInquiry(PurInquiryRequest request)
        {
            long recordID = RecordIDService.GetRecordID(1);
            string inquiryCode= RecordSnumService.GetSnum(this, request.AccountID, SnumType.InquiryRelease);
            using (var trans = Db.BeginTransaction())
            {
                //前台主表model转成dataModel
                PurInquiry pi = request.TranslateTo<PurInquiry>();
                pi.ID = recordID;
                pi.CreateTime = DateTime.Now;
                pi.InquiryCode = inquiryCode;
                pi.State = 1;
                pi.State_Enum = "询价中";
                Db.Insert(pi);
                if (request.PicResources != null)
                {
                    //附件
                    foreach (PicResources pic in request.PicResources)
                    {
                        Resources resourceNew = ToEntity(pic);
                        if (resourceNew != null && !string.IsNullOrEmpty(pic.OriginalName))
                        {
                            //2.2 insert物品图片资源
                            resourceNew.Id = RecordIDService.GetRecordID(1);
                            resourceNew.DocumentID = recordID;
                            resourceNew.AccountID = request.AccountID;
                            this.Db.Insert(resourceNew);
                        }
                    }
                }
                //前台明细表转换为datamodel
                long recordDetailID = 0;
                string categoryFirstCode = string.Empty;
                long categoryFirstID = 0;
                string categoryFirstName = string.Empty;
                string categorySecondCode = string.Empty;
                long categorySecondID = 0;
                string categorySecondName = string.Empty;
                int i = 0;
                if (request.PurItemList != null)
                {
                    foreach (PurItem pitem in request.PurItemList)
                    {
                        recordDetailID = RecordIDService.GetRecordID(1);
                        //获取分类
                        SelectCategory(pitem.CategoryMID);
                        if (categoryList.ToArray().Length != 0)
                        {
                            categoryFirstCode = categoryList[1].ToString().Split('&')[1];
                            categoryFirstID = long.Parse(categoryList[1].ToString().Split('&')[0]);
                            categoryFirstName = categoryList[1].ToString().Split('&')[2];
                            categorySecondCode = categoryList[0].ToString().Split('&')[1];
                            categorySecondID = long.Parse(categoryList[0].ToString().Split('&')[0]);
                            categorySecondName = categoryList[0].ToString().Split('&')[2];
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
                                    resourceNew.AccountID = request.AccountID;
                                    this.Db.Insert(resourceNew);
                                }
                            }
                        }
                        request.PurItemList[i].ID = recordDetailID;
                        i++;
                        propertysb.Append(pitem.PropertyName);
                        PurInquiryDetail pid = pitem.TranslateTo<PurInquiryDetail>();
                        pid.ID = recordDetailID;
                        pid.AccountID = request.AccountID;
                        pid.CategoryFirstCode = categoryFirstCode;
                        pid.CategoryFirstID = categoryFirstID;
                        pid.CategoryFirstName = categoryFirstName;
                        pid.CategorySecondCode = categorySecondCode;
                        pid.CategorySecondID = categorySecondID;
                        pid.CategorySecondName = categorySecondName;
                        pid.PropertyName = propertysb.ToString();
                        pid.MID = recordID;
                        pid.Qty = pitem.Qty;
                        Db.Insert<PurInquiryDetail>(pid);
                     
                        propertysb.Length = 0;
                        categoryList.Clear();
                        //反写采购申请产品状态为已询价 liuzhiqiang 2015-01-17
                        if (pitem.PlanDetailID  != 0)
                        {
                            this.Db.Update<PurPlanDetail>(string.Format("pland_state = 1,pland_state_enum='已询价' where pland_id = {0}", pitem.PlanDetailID));
                        }
                    }
                       //反写采购申请主表状态为已完成 liuzhiqiang 2015-01-20
                    if (request.PlanID != 0)
                    {
                        var planList = this.Db.Where<PurPlanDetail>(n => n.state == 0 && n.MID == request.PlanID);
                        if (planList.Count == 0)
                        {
                            this.Db.Update<PurPlan>(string.Format("plan_state = 1,plan_state_enum='已完成' where plan_id = {0}", request.PlanID));
                        }
                    }
                    i = 0;
                }
                trans.Commit();
                //更新企业询价中数量
                var list = this.Db.Where<PurInquiry>(n => n.AccountID == request.AccountID && n.State == 1);

                var companyInfo = this.Db.FirstOrDefault<NdtechCompany>(n => n.AccountID == request.AccountID);
                if (companyInfo != null)
                {
                    companyInfo.inquiryNumber = list.Count;
                    this.Db.Save<NdtechCompany>(companyInfo);
                }
                if (request.SupplierList != null && (request.InquiryType==1||request.InquiryType==2))
                {
                    SaveSalQuotationDraftService salService =  this.TryResolve<SaveSalQuotationDraftService>();
                    SaveSalQuotationDraftRequest  salrequest = new SaveSalQuotationDraftRequest();
                    salrequest.AccountID = request.AccountID;
                    salrequest.Address = request.Address;
                    salrequest.AddressID = request.AddressID;
                    salrequest.AnonymousCode = request.AnonymousCode;
                    salrequest.AppKey = request.AppKey;
                    salrequest.EmailInfo = request.EmailInfo;
                    salrequest.Fax = request.Fax;
                    salrequest.FinalDateTime = request.FinalDateTime;
                    salrequest.FixedLine = request.FixedLine;
                    salrequest.FreightTypeCode = request.FreightTypeCode;
                    salrequest.InquiryCode = inquiryCode;
                    salrequest.InquiryID = recordID;
                    salrequest.InquiryType = request.InquiryType;
                    salrequest.InvoiceType = request.InvoiceType;
                    salrequest.Linkman = request.Linkman;
                    salrequest.Mobile = request.Mobile;
                    salrequest.PayType = request.PayType;
                    salrequest.PicResources = request.PicResources;
                    salrequest.PurItemList = request.PurItemList;
                    salrequest.Role_Enum = request.Role_Enum;
                    salrequest.RoleID = request.RoleID;
                    salrequest.Secretkey = request.Secretkey;
                    salrequest.Subject = request.Subject;
                    salrequest.SupplierList = request.SupplierList;
                    salrequest.InquiryType = request.InquiryType;
                    salrequest.Purchaser = request.EidCompName;
                    salService.Post(salrequest);
                }

            }
        }

        //获取分类
        private void SelectCategory(long pid)
        {
            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    if (categoryList.IndexOf(pid) == -1)
                    {
                        NdtechItemCategory nic = conn.QuerySingle<NdtechItemCategory>(string.Format("select id,c,n,pid from udoc_enterprise_category where id= '{0}'", pid));
                        if (nic != null)
                        {
                            categoryList.Add(nic.ID + "&" + nic.CategoryCode + "&" + nic.CategoryName);
                            if (nic.ParentID != -1)
                            {
                                SelectCategory(nic.ParentID);

                            }
                        }
                        else
                        {
                            return;

                        }

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

        private Resources ToEntity(PicResources item)
        {
            return item.TranslateTo<Resources>();
        }
        #endregion
    }
}
