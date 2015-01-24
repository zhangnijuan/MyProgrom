using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 获取平台标准产品Service
    /// add by yangshuo 2014/12/05
    /// </summary>
    public class GetItemService : Service, IPost<GetItemRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemService));

        public IValidator<GetItemRequest> ItemsValidator { get; set; }

        #region Post

        public object Post(GetItemRequest request)
        {
            GetItemResponse response = new GetItemResponse();

            //查询集合时需校验PageIndex和PageSize
            if (request.ID == 0)
            {
                //第一步：校验前端的数据合法性
                ValidationResult result = ItemsValidator.Validate(request);
                if (!result.IsValid)
                {
                    response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                    response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                    return response;
                }
            }

            //第二步：自定义条件查询产品(含分页) + 返回response
            return PostMethod(request);
        }

        private GetItemResponse PostMethod(GetItemRequest request)
        {
            GetItemResponse response = new GetItemResponse();

            //1.平台取帐套为0的产品资料
            int accountId = 0;

            //2.自定义条件查询产品(含分页)
            List<NdtechItem> itemlist = null;

            if (!string.IsNullOrEmpty(request.ItemInfo) && !string.IsNullOrEmpty(request.CategoryID))
            {
                //模糊查询产品代码or产品名称 + 底层分类ID
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.Category3ID == Convert.ToInt64(request.CategoryID) &&
                    (x.ItemCode.Contains(request.ItemInfo) || x.ItemName.Contains(request.ItemInfo)));
            }
            else if (!string.IsNullOrEmpty(request.ItemInfo))
            {
                //模糊查询产品代码or产品名称
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.ItemCode.Contains(request.ItemInfo) || x.ItemName.Contains(request.ItemInfo));
            }
            else if (!string.IsNullOrEmpty(request.CategoryID))
            {
                //根据产品分类ID查询
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.Category3ID == Convert.ToInt64(request.CategoryID));
            }
            else if (!string.IsNullOrEmpty(request.CategoryCode))
            {
                //根据产品分类code查询
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.Category3Code.Contains(request.CategoryCode));
            }
            else if (!string.IsNullOrEmpty(request.CategoryName))
            {
                //根据产品分类name查询
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.Category3Name.Contains(request.CategoryName));
            }
            else if (request.ID > 0)
            {
                //根据产品id获取一笔产品资料,前台只传ID即可
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId && x.ID == request.ID);
            }
            else
            {
                //查询平台所有产品
                itemlist = this.Db.Where<NdtechItem>(x => x.AccountId == accountId);
            }

            if (itemlist != null && itemlist.Count > 0)
            {
                if (request.PageIndex > 0)
                {
                    //3.获取总行数
                    response.RowsCount = itemlist.Count;

                    //4.分页
                    itemlist = itemlist.OrderBy(x => x.ItemCode).Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();
                }

                //5.查询属性值 + 企业产品转化成前台model集合
                List<ItemView> itemViewModelList = GetViewModelList(itemlist, accountId);

                //6.返回对象
                response.Data = itemViewModelList;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No data";
            }

            response.Success = true;
            return response;
        }

        #endregion

        #region 公共方法

        private List<ItemView> GetViewModelList(List<NdtechItem> itemlist, int accountId)
        {
            List<ItemView> itemViewModelList = new List<ItemView>();
            foreach (var itemData in itemlist)
            {
                ItemView itemView = itemData.TranslateTo<ItemView>();

                //根据产品id获取属性值资料
                List<NdtechItemAttribute> itemAttributeData = this.Db.Where<NdtechItemAttribute>(x => x.AccountID == accountId
                                && x.ItemID == itemData.ID);
                if (itemAttributeData != null)
                {
                    List<ItemAttributeView> itemAttributeView = new List<ItemAttributeView>();
                    foreach (var itemAttribute in itemAttributeData)
                    {
                        //存属性值
                        ItemAttributeView attributeView = itemAttribute.TranslateTo<ItemAttributeView>();
                        itemAttributeView.Add(attributeView);
                    }
                    itemView.ItemAttributeViewList = itemAttributeView;
                }

                itemViewModelList.Add(itemView);
            }

            return itemViewModelList;
        }

        #endregion
    }

    #region Validate

    public class ItemValidator : AbstractValidator<GetItemRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(GetItemRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (instance.PageSize <= 0)
            {
                result.Errors.Add(new ValidationFailure("PageSize", "PageSize must larger than zero", "Err_PageSize"));
                return result;
            }

            if (instance.PageIndex <= 0)
            {
                result.Errors.Add(new ValidationFailure("PageIndex", "PageIndex must larger than zero", "Err_PageIndex"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    #endregion
}
