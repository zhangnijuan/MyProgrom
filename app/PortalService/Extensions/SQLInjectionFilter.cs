using ServiceStack.FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ndtech.PortalService.Extensions
{
    /// <summary>
    /// 防止sql注入
    /// </summary>
    public class SQLInjectionFilter
    {
        /// <summary>
        /// 验证集合数据的合法性
        /// </summary>
        /// <typeparam name="T">集合元素的类型</typeparam>
        /// <param name="list">需要验证的集合</param>
        /// <param name="result">验证的结果</param>
        public static void CheckOutList<T>(List<T> list, ValidationResult result)
        {

            if (list != null && list.Count > 0)
            {

                foreach (var item in list)
                {
                    foreach (var info in item.GetType().GetProperties())
                    {
                        MethodInfo method = info.GetGetMethod();
                        if (method != null)
                        {

                            if (info.PropertyType == Type.GetType("System.String") || info.PropertyType == Type.GetType("System.Int64") || info.PropertyType == Type.GetType("System.Int32") || info.PropertyType == Type.GetType("System.Boolean") || info.PropertyType == Type.GetType("System.Enum"))
                            {
                                object obj = method.Invoke(item, null);
                                if (obj != null)
                                {
                                    string value = obj.ToString();
                                    if (!string.IsNullOrEmpty(value) && !ProcessSqlStr(value))
                                    {
                                        result.Errors.Add(new ValidationFailure(value, string.Format("{0}是非法关键字", value), "Error"));

                                    }
                                }
                            }
                        }
                    }

                }
            }


        }
        /// <summary>
        /// 验证是否有非法字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private static bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string SqlStr =
                    "select*|and'|or'|insertinto|deletefrom|altertable|update|createtable|createview|dropview|createindex|dropindex|createprocedure|dropprocedure|createtrigger|droptrigger|createschema|dropschema|createdomain|alterdomain|dropdomain|);|select@|declare@|print@|char(|select|'";
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0 || ss.IndexOf(Str) >= 0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
    }
}
