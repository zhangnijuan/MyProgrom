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

namespace Ndtech.PortalService.Auth
{
    public class GetEnterpriseCategoryService : Service, IGet<GetEnterpriseCategoryRequest>
    {
        #region Get

        public object Get(GetEnterpriseCategoryRequest request)
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

        private GetItemCategoryResponse GetMethodByParentID(GetEnterpriseCategoryRequest request)
        {
            GetItemCategoryResponse response = new GetItemCategoryResponse();

            try
            {
                //1.根据上级类别ID查询类别资料
                var list = this.Db.Query<EnterpriseItemCategory>(string.Format("select * from udoc_enterprise_Category where PID={0}", Convert.ToInt64(request.ParentID)));

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
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }

            return response;
        }

        private GetItemCategoryResponse GetMethodByCategoryName(GetEnterpriseCategoryRequest request)
        {
            GetItemCategoryResponse response = new GetItemCategoryResponse();

            try
            {
                //1.产品分类取帐套
                int accountId = request.AccountID;

                //2.模糊查询最底层分类集合
                //2.1 先根据分类名称模糊查询id集合
                List<long> listID = this.Db.Query<long>(string.Format("select ID from udoc_enterprise_Category where PID!=-1 and a={0} and N like '%{1}%'", request.AccountID, request.CategoryName)).ToList();
                if (listID != null && listID.Count > 0)
                {
                    //2.2 再根据分类名称模糊查询parentid集合
                    var listParendID = this.Db.Query<long>
                                            (string.Format("select PID from udoc_enterprise_Category where PID!=-1 and a={0} and N like '%{1}%'", request.AccountID, request.CategoryName)).ToList();

                    //2.3 去除listID.id = listParendID.parendid =>得到要查询的最底层id集合
                    List<long> resultID = listID.Except(listParendID).ToList();
                    if (resultID != null && resultID.Count > 0)
                    {
                        List<Category> categoryList = new List<Category>();

                        foreach (var categoryID in resultID)
                        {
                            Category category = categoryID.TranslateTo<Category>();

                            //2.4 根据最底层分类id查询底层分类资料
                            EnterpriseItemCategory categoryThird = this.Db.QuerySingle<EnterpriseItemCategory>(
                                                                       string.Format("select * from udoc_enterprise_Category where a={0} and ID={1}", Convert.ToInt64(request.AccountID), categoryID));
                            category.CategoryThird = categoryThird.TranslateTo<ItemCategory>();

                            if (categoryThird.ParentID != -1)
                            {
                                //3.根据最底层分类的父id查询第二层分类
                                EnterpriseItemCategory categorySecond = this.Db.QuerySingle<EnterpriseItemCategory>(
                                                                        string.Format("select * from udoc_enterprise_Category where a={0} and ID={1}", Convert.ToInt64(request.AccountID), categoryThird.ParentID));
                                if (categorySecond != null)
                                {
                                    category.CategorySecond = categorySecond.TranslateTo<ItemCategory>();
                                    if (categorySecond.ParentID != -1)
                                    {
                                        //4.根据第二层父id查询第一层分类
                                        EnterpriseItemCategory categoryFirst = this.Db.QuerySingle<EnterpriseItemCategory>(
                                                                        string.Format("select * from udoc_enterprise_Category where a={0} and ID={1}", Convert.ToInt64(request.AccountID), categorySecond.ParentID));
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

                        //6.response返回
                        response.Data = categoryList;
                        response.Success = true;
                    }
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoData";
                    response.ResponseStatus.Message = "No data";
                    response.Success = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }

            return response;
        }

        #endregion

        #region Log日志

        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error" + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }

        #endregion
    }
}
