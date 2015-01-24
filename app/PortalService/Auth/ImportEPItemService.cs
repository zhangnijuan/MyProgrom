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
    /// 导入企业产品(采购产品or供应产品)
    /// </summary>
    public class ImportEPItemService : Service, IPost<ImportEPItemRequest>
    {

        public object Post(ImportEPItemRequest request)
        {
            ImportEPItemResponse response = new ImportEPItemResponse();
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
            errorMessage = ValidateDATADT(itemClassDT, request);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = errorMessage;
                return response;
            }

            string priceTitle = "市场价";
            if (request.BizType == 1)
            {
                //采购产品
                priceTitle = "参考价格";
            }

            string c = string.Empty;
            string n = string.Empty;
            string prc = "0";
            using (var conn = Db.BeginTransaction())
            {
                foreach (DataRow dr in itemClassDT.Rows)
                {
                    c = dr["我的产品代码"].ToString().Trim();
                    n = dr["我的产品名称"].ToString().Trim();
                    prc = dr[priceTitle].ToString().Trim();

                    EnterpriseItem eitem = new EnterpriseItem();
                    eitem.ID = RecordIDService.GetRecordID(1);
                    eitem.AccountID = request.AccountID;
                    eitem.ItemCode = c;
                    eitem.ItemName = n;
                    eitem.SalPrc = Convert.ToDecimal(prc);
                    eitem.CreateDate = DateTime.Now;
                    eitem.State = 0;

                    //业务类型:0不分类
                    eitem.BizType = request.BizType;
                    Db.Insert(eitem);
                }
                conn.Commit();
            }

            response.Success = true;
            return response;
        }

        /// <summary>
        /// 数据源数据验证
        /// </summary>
        /// <param name="itemClassDT">源数据</param>
        /// <returns>错误信息</returns>
        private string ValidateDATADT(DataTable itemClassDT, ImportEPItemRequest request)
        {
            Dictionary<string, bool> column = new Dictionary<string, bool>();
            column.Add("我的产品代码", false);
            column.Add("我的产品名称", false);
            if (request.BizType == 1)
            {
                //上传采购产品列名
                column.Add("参考价格", false);
            }
            else
            {
                //上传供应列名
                column.Add("市场价", false);
            }

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

            //查询公司产品,去除删除的
            List<EnterpriseItem> itemList = this.Db.Where<EnterpriseItem>(x => x.AccountID == request.AccountID && x.BizType == request.BizType && x.State != 3);
            EnterpriseItem item = null;
            foreach (DataRow dr in itemClassDT.Rows)
            {
                string c = dr["我的产品代码"].ToString().Trim();
                if (string.IsNullOrEmpty(c))
                {
                    return "我的产品代码不能为空";
                }
                int count = 0;
                foreach (DataRow ddr in itemClassDT.Rows)
                {
                    string cc = ddr["我的产品代码"].ToString().Trim();
                    if (c.Equals(cc))
                    {
                        if (count > 0)
                        {
                            //excel重复
                            return string.Format("我的产品代码[{0}]重复", c);
                        }
                        count++;
                    }
                }

                //检核产品代码在db是否重复
                if (itemList != null && itemList.Count > 0)
                {
                    item = itemList.Find(x => x.ItemCode == c);
                    if (item != null)
                    {
                        //db重复
                        return string.Format("我的产品代码[{0}]已存在", c);
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
