using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using NPOI.HSSF.UserModel;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using Ndtech.PortalModel.ViewModel;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 导入物品分类服务类
    /// </summary>
    public class ImportItemCategoryService : Service, IPost<ImportItemCategoryRequest>
    {
        public object Post(ImportItemCategoryRequest request)
        {
            ImportItemCategoryResponse response = new ImportItemCategoryResponse();

            #region Check

            string errorMessage = "";
            if (this.Request.Files.Length == 0)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No such file exists";
                return response;
            }
            if (this.Request.Files[0].ContentLength == 0)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No such file exists";
                return response;
            }
            var file = this.Request.Files[0];
            //此处不限制Excel大小
            if (file.ContentLength > 2097152)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "more then 2M";
                return response;
            }

            //流转换成DataSet
            DataTable itemClassDT = new DataTable();
            itemClassDT = changeToDataSet(file);

            //判断是table是否有数据
            if (itemClassDT.Rows.Count < 1)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No Data Import";
                return response;
            }

            //验证物品唯一性
            errorMessage = ValidateDATADT(itemClassDT);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = errorMessage;
                return response;
            }

            #endregion

            #region 组织新增集合

            //查询平台产品分类
            List<NdtechItemCategory> dbList = this.Db.Select<NdtechItemCategory>("select id, pid, c from udoc_category;");
            List<NdtechItemCategory> addList = new List<NdtechItemCategory>();

            string c_c = string.Empty;
            string c_n = string.Empty;
            string p_c = string.Empty;
            string p_n = string.Empty;
            NdtechItemCategory categoryExist = null;
            NdtechItemCategory itemc = null;
            NdtechItemCategory itempc = null;
            bool addParentInfo = true;

            //判断分类是否存在，不存在新增
            foreach (DataRow dr in itemClassDT.Rows)
            {
                c_c = dr["产品分类编码"].ToString().Trim();
                if (dbList != null && dbList.Count > 0)
                {
                    categoryExist = dbList.FirstOrDefault(x => x.CategoryCode == c_c);
                    if (categoryExist != null)
                    {
                        //已存在的产品分类资料不需新增
                        continue;
                    }
                }
                c_n = dr["产品分类名称"].ToString().Trim();
                p_c = dr["上级产品分类编码"].ToString().Trim();
                p_n = dr["上级产品分类名称"].ToString().Trim();
                itemc = new NdtechItemCategory();
                itemc.AccountID = Convert.ToInt32(request.AccountID);
                itemc.ParentID = -1;
                addParentInfo = true;
                if (dbList != null && dbList.Count > 0)
                {
                    //查询上级产品分类db是否存在
                    categoryExist = dbList.FirstOrDefault(x => x.CategoryCode == p_c);
                    if (categoryExist != null)
                    {
                        itemc.ParentID = categoryExist.ID;
                        addParentInfo = false;
                    }
                }

                if (addParentInfo)
                {
                    #region 父级

                    itempc = new NdtechItemCategory();
                    itempc.AccountID = Convert.ToInt32(request.AccountID);
                    itempc.ID = RecordIDService.GetRecordID(1);
                    itempc.ParentID = -1;
                    itempc.CategoryCode = p_c;
                    itempc.CategoryName = p_n;
                    itemc.ParentID = itempc.ID;
                    addList.Add(itempc);

                    //追加至dbList集合,下次比较使用
                    dbList.Add(itempc);

                    #endregion
                }

                #region

                #region 子级

                //add产品分类
                itemc.CategoryCode = c_c;
                itemc.CategoryName = c_n;
                itemc.ID = RecordIDService.GetRecordID(1);
                addList.Add(itemc);
                dbList.Add(itemc);

                #endregion

                #endregion
            }

            #endregion

            if (addList.Count >0)
            {
                using (var trans = Db.BeginTransaction())
                {
                    foreach (var item in addList)
                    {
                        this.Db.Insert(item);
                    }
                    trans.Commit();
                }
            }
            
            response.Sucess = true;
            return response;
        }

        /// <summary>
        /// 数据源数据验证
        /// </summary>
        /// <param name="itemClassDT">源数据</param>
        /// <returns>错误信息</returns>
        private string ValidateDATADT(DataTable itemClassDT)
        {
            Dictionary<string, bool> column = new Dictionary<string, bool>();
            column.Add("上级产品分类编码", false);
            column.Add("上级产品分类名称", false);
            column.Add("产品分类编码", false);
            column.Add("产品分类名称", false);
            foreach (DataColumn dc in itemClassDT.Columns)
            {
                if (column.ContainsKey(dc.ColumnName))
                {
                    column[dc.ColumnName] = true;
                }
            }
            foreach (string str in column.Keys)
            {
                if (!column[str])
                {
                    return string.Format("导入模板不规范");
                }
            }
            //foreach (DataRow dr in itemClassDT.Rows)
            //{
            //    //如果都为空的话删除行
            //    if(string.IsNullOrEmpty(dr[0].ToString())&&string.IsNullOrEmpty(dr[1].ToString())&&string.IsNullOrEmpty(dr[2].ToString())&&string.IsNullOrEmpty(dr[3].ToString()))
            //    {
            //        dr.Delete();
            //    }
            //}
            foreach (DataRow dr in itemClassDT.Rows)
            {
                string c = dr["产品分类编码"].ToString();
                if (string.IsNullOrEmpty(c))
                {
                    break;
                }
                int count = 0;
                foreach (DataRow ddr in itemClassDT.Rows)
                {
                    string cc = ddr["产品分类编码"].ToString();
                    if (string.IsNullOrEmpty(cc))
                    {
                        break;
                    }
                    if (c.Equals(cc))
                    {
                        if (count > 0)
                        {
                            return string.Format("产品分类编码[{0}]重复", c);
                        }
                        count++;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 数据流转换DataTable
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static DataTable changeToDataSet(IFile file)
        {
            ///获取文件流
            //byte[] buffer = Convert.FromBase64String(file.ToString());
            //MemoryStream memory = new MemoryStream(buffer);
            //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
            HSSFWorkbook workbook = new HSSFWorkbook(file.InputStream);

            //获取excel的第一个sheet
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);

            DataTable table = new DataTable();

            //获取sheet的首行
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);

            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;
            //循环添加列标题
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            //添加ID列
            DataColumn IdColumn = new DataColumn();
            IdColumn.ColumnName = "ID";
            table.Columns.Add(IdColumn);
            //添加分类ID列
            DataColumn CIdColumn = new DataColumn();
            CIdColumn.ColumnName = "CID";
            table.Columns.Add(CIdColumn);
            //添加分类ID列
            DataColumn CPIdColumn = new DataColumn();
            CPIdColumn.ColumnName = "CPID";
            table.Columns.Add(CPIdColumn);
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;
            //循环添加行数据
            for (int i = (sheet.FirstRowNum + 1); i < rowCount + 1; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            workbook = null;
            sheet = null;
            return table;
        }
    }
}
