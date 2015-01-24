using System;
using System.IO;
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
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 企业产品新增Service(采购产品or供应产品)
    /// add by yangshuo 2014/12/13
    /// </summary>
    public class NewEnterpriseItemService : Service, IPost<NewEnterpriseItemRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(NewEnterpriseItemService));

        public IValidator<NewEnterpriseItemRequest> ItemValidator { get; set; }

        public object Post(NewEnterpriseItemRequest request)
        {
            EnterpriseItemResponse response = new EnterpriseItemResponse();

            //第一步:校验前端的数据合法性
            ValidationResult result = ItemValidator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            //第二步:new + 返回response
            return PostMethod(request);
        }

        private EnterpriseItemResponse PostMethod(NewEnterpriseItemRequest request)
        {
            EnterpriseItemResponse response = new EnterpriseItemResponse();

            #region 1.获取insert产品分类资料

            //查询该公司所有产品分类
            List<EnterpriseItemCategory> categoryList = this.Db.Where<EnterpriseItemCategory>(x => x.AccountID == request.AccountID);

            long category1ID = -1;
            long category2ID = -1;
            long category3ID = -1;

            EnterpriseItemCategory category1 = null;
            EnterpriseItemCategory category2 = null;
            EnterpriseItemCategory category3 = null;
            bool insertAllCategory = false;

            if (categoryList != null && categoryList.Count > 0)
            {
                //根据分类code查询分类是否存在
                EnterpriseItemCategory category = categoryList.Find(x => x.CategoryCode == request.Category1Code);
                if (category != null)
                {
                    category1ID = category.ID;
                    category = categoryList.Find(x => x.CategoryCode == request.Category2Code);
                    if (category != null)
                    {
                        category2ID = category.ID;
                        category = categoryList.Find(x => x.CategoryCode == request.Category3Code);
                        if (category != null)
                        {
                            category3ID = category.ID;
                        }
                        else
                        {
                            //组织三级分类资料,insert使用
                            category3 = new EnterpriseItemCategory();
                            category3.AccountID = request.AccountID;
                            category3 = GetCategory(category3, request.Category3Code, request.Category3Name, category2ID);
                            category3ID = category3.ID;
                        }
                    }
                    else
                    {
                        //组织二级分类资料,insert使用
                        category2 = new EnterpriseItemCategory();
                        category2.AccountID = request.AccountID;
                        category2 = GetCategory(category2, request.Category2Code, request.Category2Name, category1ID);
                        category2ID = category2.ID;

                        //组织三级分类资料,insert使用
                        category3 = new EnterpriseItemCategory();
                        category3.AccountID = request.AccountID;
                        category3 = GetCategory(category3, request.Category3Code, request.Category3Name, category2.ID);
                        category3ID = category3.ID;
                    }
                }
                else
                {
                    insertAllCategory = true;
                }
            }
            else
            {
                insertAllCategory = true;
            }

            if (insertAllCategory)
            {
                //组织一二三级分类资料,insert使用
                category1 = new EnterpriseItemCategory();
                category1.AccountID = request.AccountID;
                category1 = GetCategory(category1, request.Category1Code, request.Category1Name, -1);
                category1ID = category1.ID;

                category2 = new EnterpriseItemCategory();
                category2.AccountID = request.AccountID;
                category2 = GetCategory(category2, request.Category2Code, request.Category2Name, category1.ID);
                category2ID = category2.ID;

                category3 = new EnterpriseItemCategory();
                category3.AccountID = request.AccountID;
                category3 = GetCategory(category3, request.Category3Code, request.Category3Name, category2.ID);
                category3ID = category3.ID;
            }

            #endregion

            #region 2.组织资料待insert

            //2.1 企业产品
            EnterpriseItem itemNew = request.TranslateTo<EnterpriseItem>();
            itemNew.Category1ID = category1ID;
            itemNew.Category2ID = category2ID;
            itemNew.Category3ID = category3ID;
            itemNew.ID = RecordIDService.GetRecordID(1);
            itemNew.CreateDate = DateTime.Now;
            itemNew.ModifyDate = itemNew.CreateDate;

            //2.2 属性集合
            List<EnterpriseItemAttribute> attributeList = new List<EnterpriseItemAttribute>();
            foreach (var attributeView in request.ItemAttributeViewList)
            {
                //属性view转换为dataModel
                attributeList.Add(attributeView.TranslateTo<EnterpriseItemAttribute>());
            }

            //2.3 图片集合
            List<Resources> resourcesList = null;
            if (request.PicResourcesList != null)
            {
                resourcesList = new List<Resources>();
                foreach (var picView in request.PicResourcesList)
                {
                    //图片资源view转换为dataModel
                    resourcesList.Add(picView.TranslateTo<Resources>());
                }
            }

            //2.4 创建供应产品
            EnterpriseItemPrice itemPrice = null;
            if (itemNew.BizType == 2 && itemNew.State == 1)
            {
                //需要add企业产品价格表
                itemPrice = new EnterpriseItemPrice();
                itemPrice.ID = RecordIDService.GetRecordID(1);
                itemPrice.AccountID = itemNew.AccountID;
                itemPrice.ItemID = itemNew.ID;
                itemPrice.Price = itemNew.SalPrc;
                itemPrice.PriceStartDate = itemNew.CreateDate;

                //产品发布时间赋值
                itemNew.PublishDate = itemNew.CreateDate;
            }

            #endregion

            #region 3.事务insert

            int releasesCount = 0;

            //3.开启事物insert
            using (var trans = this.Db.BeginTransaction())
            {
                #region 3.1 insert企业产品分类

                if (category1 != null)
                {
                    this.Db.Insert(category1);
                }

                if (category2 != null)
                {
                    this.Db.Insert(category2);
                }

                if (category3 != null)
                {
                    this.Db.Insert(category3);
                }

                #endregion

                //3.2 insert企业产品
                this.Db.Insert(itemNew);

                #region 产品图片资源、属性值

                //3.3 循环insert图片资源对象
                if (request.PicResourcesList != null)
                {
                    foreach (var resourceNew in resourcesList)
                    {
                        resourceNew.Id = RecordIDService.GetRecordID(1);
                        resourceNew.DocumentID = itemNew.ID;
                        resourceNew.AccountID = request.AccountID;
                        this.Db.Insert(resourceNew);
                    }
                }

                //3.4 insert属性值档案
                //属性值.itemid循环赋值
                foreach (var attributeNew in attributeList)
                {
                    attributeNew.ID = RecordIDService.GetRecordID(1);
                    attributeNew.AccountID = request.AccountID;
                    attributeNew.ItemID = itemNew.ID;
                    this.Db.Insert(attributeNew);
                }

                #endregion

                if (itemPrice != null)
                {
                    //3.5 insert企业产品价格表
                    this.Db.Insert(itemPrice);
                }

                //更新公司档案发布产品数量
                if (itemNew.BizType == 2)
                {
                    //3.6 查询已发布产品数量并更新公司档案 add 2015/01/06
                    releasesCount = Convert.ToInt32(this.Db.Count<EnterpriseItem>(x => x.AccountID == itemNew.AccountID && x.BizType == 2 && x.State == 1));
                    this.Db.Update<NdtechCompany>(string.Format("releases = {0} where a = {1}", releasesCount, itemNew.AccountID));
                }

                trans.Commit();
            }

            #region insert平台价格档案

            //发布供应产品需更新平台价格
            if (request.BizType == 2 && request.State == 1)
            {
                //1.获取平台产品id
                NdtechItem ndtechItem = this.Db.FirstOrDefault<NdtechItem>(x => x.ItemCode == request.StandardItemCode);
                if (ndtechItem != null)
                {
                    //2.根据平台标准代码查询产品平均价格做为平台平均价格
                    decimal avgPrc = this.Db.Where<EnterpriseItem>(x => x.StandardItemCode == request.StandardItemCode).Average(x => x.SalPrc);

                    //3.新增至平台产品价格档案
                    NdtechItemPrice ndtechPrice = new NdtechItemPrice();
                    ndtechPrice.ID = RecordIDService.GetRecordID(1);
                    ndtechPrice.AccountID = 0;
                    ndtechPrice.ItemID = ndtechItem.ID;
                    ndtechPrice.Price = avgPrc;
                    ndtechPrice.PriceStartDate = itemNew.CreateDate;
                    this.Db.Insert(ndtechPrice);
                }
                else
                {
                    //no平台产品资料
                    response.ResponseStatus.ErrorCode = "No Data";
                    response.ResponseStatus.Message = "No PlatFormItem Info";
                    return response;
                }
            }

            #endregion

            #endregion

            //4.返回
            response.Success = true;

            return response;
        }

        /// <summary>
        /// 组织分类资料,insert使用
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categoryCode"></param>
        /// <param name="categoryName"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private EnterpriseItemCategory GetCategory(EnterpriseItemCategory category, string categoryCode, string categoryName, long parentID)
        {
            category.CategoryCode = categoryCode;
            category.CategoryName = categoryName;
            category.ParentID = parentID;
            category.ID = RecordIDService.GetRecordID(1);
            return category;
        }
    }

    #region Validate

    public class NewEnterpriseItemValidator : AbstractValidator<NewEnterpriseItemRequest>
    {
        public IDbConnectionFactory db { set; get; }

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(NewEnterpriseItemRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.StandardItemName))
            {
                //平台产品名称不能为空
                result.Errors.Add(new ValidationFailure("StandardItemName", Const.Err_SItemNameIsNull, "Err_SItemNameIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.StandardItemCode))
            {
                //平台产品代码不能为空
                result.Errors.Add(new ValidationFailure("StandardItemCode", Const.Err_SItemCodeIsNull, "Err_SItemCodeIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Category1Code) || string.IsNullOrEmpty(instance.Category1Name))
            {
                //产品分类1Code和Name不能为空
                result.Errors.Add(new ValidationFailure("Category1Code", Const.Err_Category1IsNull, "Err_Category1IsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Category2Code) || string.IsNullOrEmpty(instance.Category2Name))
            {
                //产品分类2Code和Name不能为空
                result.Errors.Add(new ValidationFailure("Category2Code", Const.Err_Category2IsNull, "Err_Category2IsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Category3Code) || string.IsNullOrEmpty(instance.Category3Name))
            {
                //产品分类3Code和Name不能为空
                result.Errors.Add(new ValidationFailure("Category3Code", Const.Err_Category3IsNull, "Err_Category3IsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.ItemName))
            {
                //我的产品名称不能为空
                result.Errors.Add(new ValidationFailure("ItemName", Const.Err_ERPItemNameIsNull, "Err_ERPItemNameIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.ItemCode))
            {
                //我的产品代码不能为空
                result.Errors.Add(new ValidationFailure("ItemCode", Const.Err_ERPItemCodeIsNull, "Err_ERPItemCodeIsNull"));
                return result;
            }

            if (instance.ItemAttributeViewList == null)
            {
                //产品属性不能为空
                result.Errors.Add(new ValidationFailure("ItemAttributeViewList", Const.Err_AttributeIsNull, "Err_AttributeIsNull"));
                return result;
            }

            //供应物品
            if (instance.BizType == 2)
            {
                if (instance.SalPrc == 0)
                {
                    //市场价不能为空
                    result.Errors.Add(new ValidationFailure("SalPrc", Const.Err_ERPSalPrcIsNull, "Err_ERPSalPrcIsNull"));
                    return result;
                }

                if (string.IsNullOrEmpty(instance.Description))
                {
                    //详细说明不能为空
                    result.Errors.Add(new ValidationFailure("Description", Const.Err_ERPDescIsNull, "Err_ERPDescIsNull"));
                    return result;
                }
            }

            //我的产品代码唯一
            using (var conn = db.OpenDbConnection())
            {
                EnterpriseItem itemExist = conn.QuerySingle<EnterpriseItem>(
                    string.Format("select c from udoc_enterprise_item where a = {0} and (c = '{1}' or c = '{2}');",
                        instance.AccountID, instance.ItemCode.ToUpper(), instance.ItemCode.ToLower()));

                if (itemExist != null)
                {
                    //我的产品代码已存在
                    result.Errors.Add(new ValidationFailure("ItemCode", Const.Err_ERPItemCodeExist, "Err_ERPItemCodeExist"));
                    return result;
                }
            }
            return base.Validate(instance);
        }
    }

    #endregion
}
