using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using ServiceStack.Common;
using System.IO;
using ServiceStack.Logging;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 获取平台标准产品分类Service
    /// add by yangshuo 2014/12/05
    /// </summary>
    public class GetItemCategoryService : Service, IGet<GetItemCategoryRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetItemCategoryService));

        #region Get

        public object Get(GetItemCategoryRequest request)
        {
            GetItemCategoryResponse response = new GetItemCategoryResponse();

            #region /category/list/{ParentID}

            //第一步:校验前端的数据合法性
            if (!string.IsNullOrEmpty(request.ParentID))
            {
                //第二步:根据上级分类ID获取产品分类信息 + 返回response
                return GetMethodByParentID(request);
            }
            else
            {
                if (string.IsNullOrEmpty(request.LikeSearch))
                {
                    response.ResponseStatus.ErrorCode = "Error_ParentID";
                    response.ResponseStatus.Message = "ParentID is null";
                    response.Success = false;
                    return response;
                }
            }

            #endregion

            #region /category/list/{LikeSearch}/{CategoryName}

            if (!string.IsNullOrEmpty(request.LikeSearch) && request.LikeSearch.ToUpper() == "Y" && !string.IsNullOrEmpty(request.CategoryName))
            {
                //根据分类名称模糊搜索产品分类
                return GetMethodByCategoryName(request);
            }
            else
            {
                response.ResponseStatus.ErrorCode = "CategoryName";
                response.ResponseStatus.Message = "LikeSearch or CategoryName is null";
                response.Success = false;
                return response;
            }

            #endregion
        }

        private GetItemCategoryResponse GetMethodByParentID(GetItemCategoryRequest request)
        {
            GetItemCategoryResponse response = new GetItemCategoryResponse();

            //1.根据上级类别ID查询类别资料
            var list = this.Db.Where<NdtechItemCategory>(x => x.ParentID == Convert.ToInt64(request.ParentID));

            if (list.Count > 0)
            {
                //2.转换为前端model集合
                List<Category> listModel = new List<Category>();
                foreach (var item in list)
                {
                    listModel.Add(item.TranslateTo<Category>());
                }

                //3.返回对象
                response.Data = listModel;
                response.Success = true;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No data";
                response.Success = false;
            }

            return response;
        }

        private GetItemCategoryResponse GetMethodByCategoryName(GetItemCategoryRequest request)
        {
            GetItemCategoryResponse response = new GetItemCategoryResponse();

            //1.产品分类取帐套为0的资料
            int accountId = 0;

            //2.模糊查询最底层分类集合
            //先获取db平台产品分类
            List<NdtechItemCategory> dbCategory = this.Db.Where<NdtechItemCategory>(x => x.AccountID == accountId);

            if (dbCategory != null && dbCategory.Count > 0)
            {
                List<NdtechItemCategory> categorySearch = dbCategory.Where<NdtechItemCategory>(
                    x => x.CategoryName.Contains(request.CategoryName) && x.ParentID != -1).ToList();

                if (categorySearch != null && categorySearch.Count > 0)
                {
                    //2.1 先根据分类名称模糊查询id集合
                    List<long> listID = categorySearch.Select(x => x.ID).ToList();

                    if (listID != null && listID.Count > 0)
                    {
                        //2.2 再根据分类名称模糊查询parentid集合
                        var listParendID = categorySearch.Select(x => x.ParentID).ToList();

                        //2.3 去除listID.id = listParendID.parendid =>得到要查询的最底层id集合
                        List<long> resultID = listID.Except(listParendID).ToList();

                        if (resultID != null && resultID.Count > 0)
                        {
                            #region 获取产品分类集合

                            List<Category> categoryList = new List<Category>();

                            foreach (var categoryID in resultID)
                            {
                                Category category = categoryID.TranslateTo<Category>();

                                //2.4 根据最底层分类id查询底层分类资料
                                NdtechItemCategory categoryThird = dbCategory.FirstOrDefault<NdtechItemCategory>(x => x.ID == categoryID);
                                category.CategoryThird = categoryThird.TranslateTo<ItemCategory>();

                                if (categoryThird.ParentID != -1)
                                {
                                    //3.根据最底层分类的父id查询第二层分类
                                    NdtechItemCategory categorySecond = dbCategory.FirstOrDefault<NdtechItemCategory>(x => x.ID == categoryThird.ParentID);
                                    if (categorySecond != null)
                                    {
                                        category.CategorySecond = categorySecond.TranslateTo<ItemCategory>();
                                        if (categorySecond.ParentID != -1)
                                        {
                                            //4.根据第二层父id查询第一层分类
                                            NdtechItemCategory categoryFirst = dbCategory.FirstOrDefault<NdtechItemCategory>(x => x.ID == categorySecond.ParentID);
                                            if (categoryFirst != null)
                                            {
                                                category.CategoryFirst = categoryFirst.TranslateTo<ItemCategory>();
                                            }
                                        }
                                    }
                                }

                                //5.累加分类三层集合
                                categoryList.Add(category);
                            }

                            #endregion

                            //6.response返回
                            response.Data = categoryList;
                            response.Success = true;
                        }
                    }
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoData";
                    response.ResponseStatus.Message = "No data";
                    return response;
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No data";
                return response;
            }

            return response;
        }

        #endregion
    }
}