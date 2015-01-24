using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与数据库有关
using ServiceStack.Common;


namespace Ndtech.PortalService.Auth
{
    public class SearchCategoryService : Service, IPost<SearchCategoryRequest>
    {
        public object Post(SearchCategoryRequest request)
        {
            SearchCategoryResponse response = new SearchCategoryResponse();

            List<CategorySecondInfo> CategoryInfoList = new List<CategorySecondInfo>();

            //获取顶级分类的ID值
            List<long> listID = new List<long>();
            StringBuilder SearchSql = new StringBuilder("select id from udoc_enterprise_Category where 1=1 and a = " + request.AccountId + " and pid=-1 ");//给定的查询条件为顶级分类名称或编码
            StringBuilder SearchSqlThird = new StringBuilder("select * from udoc_enterprise_Category where 1=1 and a = " + request.AccountId);
            StringBuilder tempsql = new StringBuilder();
            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                foreach (var obj in request.SearchCondition)
                {
                    if (obj.SeacheKey == SearchCategoryEnum.CategoryCode)
                    {
                        SearchSql.Append(string.Format(" and ( c like '%{0}%' or n like '%{0}%')", obj.Value));
                    }
                }
            }
            #region
            listID = this.Db.Query<long>(string.Format(SearchSql.ToString()));
            if (listID.Count > 0)
            {
                StringBuilder sqlids = new StringBuilder();
                string str = String.Join(",", listID.Select(x => x.ToString()).ToArray());
                tempsql.Append(" and pid in (" + str + ")");
                #region 获取二级分类
                var list = this.Db.Query<NdtechItemCategory>(string.Format(SearchSqlThird.ToString() + tempsql.ToString()));
                //
                tempsql.Remove(0, tempsql.Length);


                foreach (var item in list)
                {
                    var CategoryInfo = ToCategory(item);

                    StringBuilder sqlsecondids = new StringBuilder();
                    ////获取二级分类的ID                   
                    sqlsecondids.Append(" and pid in (" + item.ID.ToString() + ")");//(cateinfo.ID.ToString());//+= cateinfo.ID + ",";
                    var listThird = this.Db.Query<NdtechItemCategory>(string.Format(SearchSqlThird.ToString() + sqlsecondids.ToString()));

                    List<CategoryThirdInfo> CategoryThirdInfoList = new List<CategoryThirdInfo>();
                    foreach (var listThirdinfo in listThird)
                    {
                        CategoryThirdInfoList.Add(ToCategoryThird(listThirdinfo));

                    }

                    CategoryInfo.DataThird = CategoryThirdInfoList;
                    CategoryInfoList.Add(CategoryInfo);
                }
                #endregion
                response.Data = CategoryInfoList;
                response.Success = true;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No data";
                response.Success = true;
                return response;
            }

            #endregion

            return response;
        }
        private CategorySecondInfo ToCategory(NdtechItemCategory ItemCate)
        {
            return ItemCate.TranslateTo<CategorySecondInfo>();
        }
        private CategoryThirdInfo ToCategoryThird(NdtechItemCategory ItemCate)
        {
            return ItemCate.TranslateTo<CategoryThirdInfo>();
        }


    }
}
