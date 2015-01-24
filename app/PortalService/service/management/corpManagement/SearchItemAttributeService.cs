using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与数据库有关
using ServiceStack.Common;
using ServiceStack.Text;

namespace Ndtech.PortalService.Auth
{
    public class SearchItemAttributeService : Service, IPost<SearchItemAttributeRequest>
    {
        public object Post(SearchItemAttributeRequest request)
        {
            SearchItemAttributeResponse response = new SearchItemAttributeResponse();
            //List<CategoryAttributeClass> CategoryAttributeClass = new List<CategoryAttributeClass>();
            if (request.SearchCondition != null && request.SearchCondition.Count > 0)
            {
                //企业属性搜索有问题待修改 2015/01/12
                string categorySql = "select  distinct attribute_class from item_attribute_view c where 1=1 and c.category3code like '%{0}%'";
                string attributeSql = "select  attribute_value from item_attribute_view c where 1=1 and c.attribute_Class like '%{0}%'";
                if (request.SearchType == PortalModel.SearchType.platform)
                {
                    //平台属性搜索
                    categorySql = "select distinct a.c, isscope, scopeinfo from udoc_attribute a join udoc_Category b on a.CategoryID = b.id where b.c  = '{0}' ";
                }
                foreach (var obj in request.SearchCondition)
                {
                    if (obj.SeacheKey == SearchAttributeEnum.CategoryCode)
                    {

                        //先查询出属性类名+是否可范围搜索
                        List<Attribute> listAttriClassInfo = this.Db.Query<Attribute>(string.Format(categorySql, obj.Value)).ToList();

                        List<CategoryAttributeClass> ListAttriClass = new List<CategoryAttributeClass>();
                        foreach (var item in listAttriClassInfo)
                        {
                            //将其转化为ViewModel中CategoryAttributeClass
                            var AttributeClass = ToAttributeClass(item.C);
                            List<CategoryAttributeValue> CategoryAttributeValue = new List<CategoryAttributeValue>();

                            //范围属性类读属性值区间
                            if (item.IsScope == 1)
                            {
                                if (!string.IsNullOrEmpty(item.ScopeInfo))
                                {
                                    //范围搜索区域值集合 2015/01/13add
                                    CategoryAttributeValue = item.ScopeInfo.FromJson<List<CategoryAttributeValue>>();
                                    AttributeClass.ClassValueList = CategoryAttributeValue;
                                }
                            }
                            else
                            {
                                //非范围属性读取固定值
                                #region 获取属性值列表

                                //根据属性名获得属性值列表
                                List<string> listAttriValue = this.Db.Query<string>(string.Format(attributeSql, item.C)).ToList();

                                foreach (var attribute in listAttriValue)
                                {
                                    //将属性值付给属性值viewmodel
                                    var attributeInfo = ToAttribute(attribute);

                                    attributeInfo.ClassValue = attribute;
                                    CategoryAttributeValue.Add(attributeInfo);
                                }

                                AttributeClass.ClassValueList = CategoryAttributeValue;

                                #endregion
                            }
                            
                            AttributeClass.ClassName = item.C;
                            AttributeClass.IsScope = item.IsScope;
                            
                            ListAttriClass.Add(AttributeClass);
                        }
                        response.Data = ListAttriClass;
                        response.Success = true;
                    }
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoCategoryCode";
                response.ResponseStatus.Message = "No data";
                response.Success = false;
                return response;
            }
            return response;
        }

        private CategoryAttributeValue ToAttribute(string item)
        {
            return item.TranslateTo<CategoryAttributeValue>();
        }
        private CategoryAttributeClass ToAttributeClass(string item)
        {
            return item.TranslateTo<CategoryAttributeClass>();
        }
    }

    /// <summary>
    /// 平台属性类
    /// </summary>
    public class Attribute
    {
        public string C { get; set; }

        /// <summary>
        /// 是否可范围搜索
        /// 默认0
        /// 0否 1是
        /// </summary>
        public int IsScope { get; set; }

        /// <summary>
        /// 范围搜索区域值
        /// </summary>
        public string ScopeInfo { get; set; }
    }
}
