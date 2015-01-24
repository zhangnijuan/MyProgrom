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
    /// 导入物品属性值
    /// </summary>
    public class ImportItemAttributeService : Service, IPost<ImportItemAttributeRequest>
    {

        public object Post(ImportItemAttributeRequest request)
        {
            ImportItemAttributeResponse response = new ImportItemAttributeResponse();
            try
            {
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
                //string[] fileArray = file.FileName.Split('.');
                //if (fileArray.Length != 2)
                //    throw new NotSupportedException("ThrowError");

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
                //errorMessage = ValidateDATADT(itemClassDT);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new { sucess = false, data = errorMessage };
                }

                //根据条件查询数据
                List<NdtechItem> clist = this.Db.Query<NdtechItem>("select id,c from udoc_item;");
                NdtechItem ndtechItem = null;
                long itemID = -1;
                using (var conn = Db.BeginTransaction())
                {
                    foreach (DataRow dr in itemClassDT.Rows)
                    {
                        string prop_n = dr["属性类名称"].ToString().Trim();
                        string itemCode = dr["产品编码"].ToString().Trim();
                        string n = dr["产品名称"].ToString().Trim();
                        string prop_value = dr["属性值"].ToString().Trim();
                        string unitname = dr["属性单位"].ToString().Trim();
                        if (string.IsNullOrEmpty(prop_n))
                        {
                            break;
                        }
                        itemID = -1;
                        ndtechItem = clist.FirstOrDefault<NdtechItem>(x => x.ItemCode == itemCode);
                        if (ndtechItem != null)
                        {
                            itemID = ndtechItem.ID;
                        }
                        NdtechItemAttribute niAttribute = new NdtechItemAttribute();
                        niAttribute.AccountID = request.AccountID;
                        niAttribute.AttributeClass = prop_n;
                        niAttribute.ItemID = itemID;
                        niAttribute.ID = RecordIDService.GetRecordID(1);
                        niAttribute.AttributeValue = prop_value;
                        niAttribute.UnitName = unitname;
                        Db.Insert(niAttribute);
                    }
                    conn.Commit();
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

        #region Log日志

        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error"
                + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }

        #endregion

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
            //添加产品ID列
            DataColumn CIdColumn = new DataColumn();
            CIdColumn.ColumnName = "itemid";
            table.Columns.Add(CIdColumn);
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
