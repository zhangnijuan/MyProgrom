using Ndtech.PortalModel.ViewModel;
using ServiceStack.Common;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ndtech.PortalService.Extensions;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.FluentValidation.Internal;
using ServiceStack.FluentValidation.Results;
using ServiceStack.OrmLite;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.ServiceInterface.Auth;
using Ndtech.PortalModel;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;


namespace Ndtech.PortalService.service.management.mySale
{
    public class ImportPurchasePlanService : Service, IPost<ImportPurchasePlanRequest>
    {
        public object Post(ImportPurchasePlanRequest request)
        {
            ImportPurchasePlanResponse response = new ImportPurchasePlanResponse();
            if (this.Request.Files.Length == 0)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No such file exists";
                response.Success = false;
                throw new FileNotFoundException("UploadError", "No such file exists");
            }
            if (this.Request.Files[0].ContentLength == 0)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No such file exists";
                response.Success = false;
                throw new FileNotFoundException("UploadError", "No such file exists");
            }

            IFile file = this.Request.Files[0];
            bool flag;
            DataTable tb = GetDataTable(file,out flag);

            if (tb != null && tb.Rows.Count < 1)
            {
                response.ResponseStatus.ErrorCode = "false";
                response.ResponseStatus.Message = "No Data Import";
                response.Success = false;
                throw new FileNotFoundException("UploadError", "No Data Import");
            }
            //if (!flag)
            //{
            //    response.ResponseStatus.Message = "数据不完整 请填写完整再导入";
            //    response.Success = false;
            //    return response;
            //}
            //生成主表数据
            PurPlan plan = new PurPlan();
            plan.ID = RecordIDService.GetRecordID(1);
            plan.AccountID = request.AccountID;
            plan.PlanCode = RecordSnumService.GetSnum(this, request.AccountID, SnumType.PurRequest);
            plan.state = 2;
            plan.StateEnum = "草稿";
            
            //生成明细表数据
            List<PurPlanDetail> listDetail = new List<PurPlanDetail>();
            List<PurchasePlanDetail> listData = new List<PurchasePlanDetail>();//存前端返回到客户端的model
            foreach (DataRow item in tb.Rows)
            {
                PurPlanDetail planDetail = new PurPlanDetail();
                planDetail.ID = RecordIDService.GetRecordID(1);
                var product = this.Db.FirstOrDefault<EnterpriseItem>(n => n.ItemCode == item[0].ToString()&&n.AccountID==request.AccountID);
                if (product != null)
                {

                    planDetail.ItemID = product.ID;
                    planDetail.StandardItemCode = product.StandardItemCode;
                    planDetail.StandardItemName = product.StandardItemName;
                    planDetail.CategoryID = product.Category3ID;
                    planDetail.CategoryCode = product.Category3Code;
                    planDetail.CategoryName = product.Category3Name;
                    planDetail.Remark = product.Remark;
                    planDetail.UnitCode = product.UnitCode;
                    DateTime time;
                    if (DateTime.TryParse(item[3].ToString(),out time)==false)
                    {
                        response.ResponseStatus.Message = "时间格式不正确请重新导入";
                        return response;
                    }
                    planDetail.DeliveryDate = time;
                    planDetail.Quantity = Convert.ToDecimal(item[2]);
                    //string sql = string.Format("select a.id from pur_inquiry a join pur_inquirydetail b on a.id=b.mid where a.a={0} and b.i_c='{1}' and a.state=1", request.AccountID, item[0].ToString());
                    //var InquiryDetail = this.Db.Query<PurInquiryDetail>(sql);
                    //if (InquiryDetail.Count > 0)
                    //{
                    //    planDetail.state = 1;
                    //    planDetail.StateEnum = "已询价";
                    //}
                    //else
                    //{
                    //    planDetail.state = 0;
                    //}
                    //string orderSql = string.Format("select a.id from pur_order a join pur_orderdetail b on a.id=b.mid where a.a={0} and b.i_c='{1}' and (a.state=1 or a.state=2)", request.AccountID, item[0].ToString());
                    //var orderDetail = this.Db.Query<PurOrder>(orderSql);
                    //if (orderDetail.Count > 0)
                    //{
                    //    planDetail.state = 1;
                    //    planDetail.StateEnum = "已下单";
                    //}
                    //else
                    //{
                    //    planDetail.state = 0;
                    //}
                    //查询属性
                    //var attr = this.Db.Where<EnterpriseItemAttribute>();

                    var attr = this.Db.Dictionary<string, string>("select attribute_class,attribute_value from udoc_enterprise_attribute where itemid=" + product.ID);
                    if (attr.Count > 0)
                    {
                        StringBuilder propertyName = new StringBuilder();
                        foreach (var ar in attr)
                        {
                            propertyName.AppendFormat("{0}:{1};", ar.Key, ar.Value);
                        }
                        planDetail.PropertyName = propertyName.ToString();
                    }
                    planDetail.ItemCode = item[0].ToString();
                    planDetail.ItemName = item[1].ToString();
                    planDetail.MID = plan.ID;
                    planDetail.AccountID = request.AccountID;
                    planDetail.Enabled = 0;
                    listDetail.Add(planDetail);

                    listData.Add(planDetail.TranslateTo<PurchasePlanDetail>());
                }
                else
                {
                    response.Success = false;
                   response.ResponseStatus.Message = string.Format("{0}:尚未创建产品档案,请先创建产品档案",item[1].ToString());
                    
                    return response;
                }

            }
            //开启事务保存到2张表中
            using (var trans = this.Db.BeginTransaction())
            {
                this.Db.Insert<PurPlan>(plan);
                this.Db.InsertAll<PurPlanDetail>(listDetail);
                trans.Commit();
                response.Success = true;
                response.Data = listData;
            }

            //把数据转回前端model
            return response;
        }
        /// <summary>
        /// 获取导入的exelc 文件中的数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static DataTable GetDataTable(IFile file,out bool flag)
        {
            flag = true;
            DataTable tb = new DataTable();
            HSSFWorkbook workbook = new HSSFWorkbook(file.InputStream);
            //获取excel的第一个sheet
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
            //获取sheet的首行
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                tb.Columns.Add(column);
            }
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < rowCount + 1; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = tb.NewRow();
                if (row == null) continue;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null && !string.IsNullOrEmpty(row.GetCell(j).ToString()))
                        dataRow[j] = row.GetCell(j).ToString();
                    else 
                    {
                        flag = false;
                        break;
                    }
                    if (j == 3)
                    {
                       
                        tb.Rows.Add(dataRow);
                    }
                }


            }

            workbook = null;
            sheet = null;
            return tb;
        }

    }
}
