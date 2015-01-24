using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using NPOI.HSSF.UserModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 导入物品分类服务类
    /// </summary>
    public class ImportItemService : Service, IPost<ImportItemRequest>
    {

        public object Post(ImportItemRequest request)
        {
            ImportItemResponse response = new ImportItemResponse();
            try
            {
                #region Check

                string errorMessage = "";
                if (this.Request.Files.Length == 0)
                {
                    response.ResponseStatus.ErrorCode = "false";
                    response.ResponseStatus.Message = "No such file exists";
                    response.Sucess = false;
                    throw new FileNotFoundException("UploadError", "No such file exists");
                }
                if (this.Request.Files[0].ContentLength == 0)
                {
                    response.ResponseStatus.ErrorCode = "false";
                    response.ResponseStatus.Message = "No such file exists";
                    response.Sucess = false;
                    throw new FileNotFoundException("UploadError", "No such file exists");
                }
                var file = this.Request.Files[0];
                //此处不限制Excel大小
                if (file.ContentLength > 2097152)
                {
                    response.ResponseStatus.ErrorCode = "false";
                    response.ResponseStatus.Message = "more then 2M";
                    response.Sucess = false;
                    throw new NotSupportedException("不允许上传超过2M的文件！");
                }

                //流转换成DataSet
                DataTable itemClassDT = new DataTable();
                itemClassDT = changeToDataSet(file);

                //判断是table是否有数据
                if (itemClassDT.Rows.Count < 1)
                {
                    response.ResponseStatus.ErrorCode = "false";
                    response.ResponseStatus.Message = "No Data Import";
                    response.Sucess = false;
                    throw new FileNotFoundException("UploadError", "No Data Import");
                }

                //验证物品唯一性
                errorMessage = ValidateDATADT(itemClassDT);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.ResponseStatus.ErrorCode = "Validate false";
                    response.ResponseStatus.Message = errorMessage;
                    return response;
                }

                #endregion

                #region 定义变量

                string c = string.Empty;
                string n = string.Empty;
                long third_id = -1;
                string third_c = string.Empty;
                string third_n = string.Empty;
                long second_id = -1;
                string second_c = string.Empty;
                string second_n = string.Empty;
                long first_id = -1;
                string first_c = string.Empty;
                string first_n = string.Empty;
                string remark = string.Empty;
                string unitName = string.Empty;

                DateTime dtNow = DateTime.Now;
                NdtechItem item = null;

                #endregion

                //查询产品分类
                List<NdtechItemCategory> clist = this.Db.Query<NdtechItemCategory>("select * from udoc_category;");

                if (clist != null && clist.Count > 0)
                {
                    NdtechItemCategory categorySearch = null;
                    using (var conn = Db.BeginTransaction())
                    {
                        //判断分类是否存在，不存在新增
                        foreach (DataRow dr in itemClassDT.Rows)
                        {
                            c = dr["我的产品编码"].ToString().Trim();
                            n = dr["我的产品名称"].ToString().Trim();
                            third_id = -1;
                            third_c = dr["产品分类编码"].ToString().Trim();
                            third_n = dr["产品分类名称"].ToString().Trim();
                            second_id = -1;
                            second_c = dr["上级产品分类编码"].ToString().Trim();
                            second_n = dr["上级产品分类名称"].ToString().Trim();
                            first_id = -1;
                            first_c = "";
                            first_n = "";
                            remark = dr["备注"].ToString().Trim();
                            unitName = dr["单位"].ToString().Trim();

                            #region 一二三级分类赋值

                            //根据三级分类代码获取三级分类id和二级分类id
                            categorySearch = clist.FirstOrDefault<NdtechItemCategory>(x => x.CategoryCode == third_c);
                            if (categorySearch != null)
                            {
                                third_id = categorySearch.ID;
                                second_id = categorySearch.ParentID;

                                //根据二级分类id查询一级分类资料
                                categorySearch = clist.FirstOrDefault<NdtechItemCategory>(x => x.ID == categorySearch.ParentID);
                                if (categorySearch != null)
                                {
                                    first_id = categorySearch.ParentID;
                                    categorySearch = clist.FirstOrDefault<NdtechItemCategory>(x => x.ID == first_id);
                                    if (categorySearch != null)
                                    {
                                        first_c = categorySearch.CategoryCode;
                                        first_n = categorySearch.CategoryName;
                                    }
                                }
                            }

                            #endregion

                            #region insert

                            item = new NdtechItem();
                            item.ID = RecordIDService.GetRecordID(1);
                            item.AccountId = request.AccountID;
                            item.Createdate = dtNow;
                            item.ItemCode = c;
                            item.ItemName = n;
                            item.Remark = remark;
                            item.Category3ID = third_id;
                            item.Category3Code = third_c;
                            item.Category3Name = third_n;
                            item.Category2ID = second_id;
                            item.Category2Code = second_c;
                            item.Category2Name = second_n;
                            item.Category1ID = first_id;
                            item.Category1Code = first_c;
                            item.Category1Name = first_n;
                            item.UnitID = -1;
                            item.UnitCode = unitName;
                            item.UnitName = unitName;

                            Db.Insert(item);

                            #endregion
                        }
                        conn.Commit();
                    }
                }
                else
                {
                    //先上传标准产品分类
                    response.ResponseStatus.ErrorCode = "Error";
                    response.ResponseStatus.Message = "Please upload Category First";
                    return response;
                }

                response.ResponseStatus.ErrorCode = "true";
                response.ResponseStatus.Message = "Import Sucess";
                response.Sucess = true;
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }
            return new { sucess = true, data = true };
        }

        /// <summary>
        /// 数据源数据验证
        /// </summary>
        /// <param name="itemClassDT">源数据</param>
        /// <returns>错误信息</returns>
        private string ValidateDATADT(DataTable itemClassDT)
        {
            Dictionary<string, bool> column = new Dictionary<string, bool>();
            column.Add("我的产品编码", false);
            column.Add("我的产品名称", false);
            column.Add("单位", false);
            column.Add("上级产品分类编码", false);
            column.Add("上级产品分类名称", false);
            column.Add("产品分类编码", false);
            column.Add("产品分类名称", false);
            column.Add("备注", false);
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
            foreach (DataRow dr in itemClassDT.Rows)
            {
                string c = dr["我的产品编码"].ToString();
                if (string.IsNullOrEmpty(c))
                {
                    break;
                }
                int count = 0;
                foreach (DataRow ddr in itemClassDT.Rows)
                {
                    string cc = ddr["我的产品编码"].ToString();
                    if (string.IsNullOrEmpty(cc))
                    {
                        break;
                    }
                    if (c.Equals(cc))
                    {
                        if (count > 0)
                        {
                            return string.Format("产品编码[{0}]重复", c);
                        }
                        count++;
                    }
                }
            }

            //产品代码不能和db重复
            string code = string.Empty;
            NdtechItem ndtechItem = null;
            StringBuilder sError = new StringBuilder();
            List<NdtechItem> itemList = this.Db.Query<NdtechItem>("select * from udoc_item;");
            foreach (DataRow dr in itemClassDT.Rows)
            {
                code = dr["我的产品编码"].ToString();
                ndtechItem = itemList.FirstOrDefault<NdtechItem>(x => x.ItemCode == code);
                if (ndtechItem != null)
                {
                    sError.Append("我的产品编码【").Append(code).Append("】已经存在；\r\n");
                }
            }
            if (sError.Length > 0)
            {
                string error = sError.ToString().Substring(0, sError.ToString().Length - 2);
                return error;
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
