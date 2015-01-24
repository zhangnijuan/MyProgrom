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
using ServiceStack.Text;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 获取所有平台标准产品分类Service
    /// add by yangshuo 2015/01/09
    /// </summary>
    public class GetAllCategoryService : Service, IGet<GetAllCategoryRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(GetAllCategoryService));

        public object Get(GetAllCategoryRequest request)
        {
            GetCategoryResponse response = new GetCategoryResponse();

            //获取一二三级产品分类
            return GetMethod(request);
        }

        private GetCategoryResponse GetMethod(GetAllCategoryRequest request)
        {
            GetCategoryResponse response = new GetCategoryResponse();

            //1.查询平台所有产品分类
            var dbAll = this.Db.Select<NdtechItemCategory>();
            if (dbAll != null && dbAll.Count > 0)
            {
                //2.查询平台一级产品分类
                List<NdtechItemCategory> tempFirstList = dbAll.Where<NdtechItemCategory>(x => x.ParentID == -1).ToList();

                if (tempFirstList.Count > 0)
                {
                    List<NdtechItemCategory> tempSecondList = new List<NdtechItemCategory>();
                    List<NdtechItemCategory> tempThirdList = new List<NdtechItemCategory>();
                    CategoryFirst tempFirst = new CategoryFirst();
                    CategorySecond tempSecond = new CategorySecond();
                    CategoryThird tempThird = new CategoryThird();

                    List<CategoryFirst> categoryFirst = new List<CategoryFirst>();
                    List<CategorySecond> categorySecond = null;
                    List<CategoryThird> categoryThird = null;

                    //循环一级分类
                    foreach (var firstItem in tempFirstList)
                    {
                        tempFirst = firstItem.TranslateTo<CategoryFirst>();

                        //查询对应二级分类
                        tempSecondList = dbAll.Where<NdtechItemCategory>(x => x.ParentID == firstItem.ID).ToList();

                        if (tempSecondList.Count > 0)
                        {
                            //循环二级分类
                            categorySecond = new List<CategorySecond>();
                            foreach (var secondItem in tempSecondList)
                            {
                                tempSecond = secondItem.TranslateTo<CategorySecond>();

                                //查询对应三级分类
                                tempThirdList = dbAll.Where<NdtechItemCategory>(x => x.ParentID == secondItem.ID).ToList();

                                if (tempThirdList.Count > 0)
                                {
                                    categoryThird = new List<CategoryThird>();
                                    foreach (var thirdItem in tempThirdList)
                                    {
                                        tempThird = thirdItem.TranslateTo<CategoryThird>();
                                        categoryThird.Add(tempThird);
                                    }

                                    tempSecond.CategoryThird = categoryThird;
                                }

                                //三级分类赋值
                                categorySecond.Add(tempSecond);
                            }

                            //二级分类赋值
                            tempFirst.CategorySecond = categorySecond;
                        }

                        //一级分类赋值
                        categoryFirst.Add(tempFirst);
                    }

                    //3.一二三级分类赋值
                    response.Data = categoryFirst;
                    response.Success = true;

                    //4.生成产品分类文件
                    Write(response.ToJson());
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoData";
                    response.ResponseStatus.Message = "No CategoryFirst Data In DB";
                    return response;
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoData";
                response.ResponseStatus.Message = "No Category Data In DB";
                return response;
            }

            return response;
        }

        #region 生成产品分类文件

        /// <summary>
        /// E:\Procurement\program\app\PortalWeb\log
        /// </summary>
        /// <param name="err"></param>
        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(
                dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "Category" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }

        #endregion
    }
}
