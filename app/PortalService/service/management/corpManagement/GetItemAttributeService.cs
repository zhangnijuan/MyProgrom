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
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 根据产品类别获取产品属性资料
    /// add by yangshuo 2014/12/08
    /// </summary>
    public class GetItemAttributeService : Service, IGet<GetItemAttributeRequest>
    {
        public IValidator<GetItemAttributeRequest> ItemPropertyValidator { get; set; }

        #region Get

        public object Get(GetItemAttributeRequest request)
        {
            GetItemAttributeResponse response = new GetItemAttributeResponse();

            //第一步：校验前端的数据合法性
            ValidationResult result = ItemPropertyValidator.Validate(request);
            if (!result.IsValid)
            {
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }

            //第二步：根据产品类别查询属性 + 返回response
            return GetMethod(request);
        }

        private GetItemAttributeResponse GetMethod(GetItemAttributeRequest request)
        {
            GetItemAttributeResponse response = new GetItemAttributeResponse();

            //try
            //{
            //    //1.根据产品类别查询属性
            //    var list =  this.Db.Where<NdtechItemAttribute>(x => x.CategoryID == Convert.ToInt64(request.CategoryID));

            //    if (list.Count > 0)
            //    {
            //        //2.转换为前端model集合
            //        List<ItemProperty> listModel = new List<ItemProperty>();
            //        foreach (var item in list)
            //        {
            //            listModel.Add(item.TranslateTo<ItemProperty>());
            //        }

            //        //3.返回对象
            //        response.Data = listModel;
            //        response.Success = true;
            //    }
            //    else
            //    {
            //        response.ResponseStatus.ErrorCode = "Err_NoData";
            //        response.ResponseStatus.Message = "No data";
            //        response.Success = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Write(ex.ToString());
            //}

            return response;
        }

        #endregion

        #region  Log日志

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

    #region Validate

    public class ItemPropertyValidator : AbstractValidator<GetItemAttributeRequest>
    {
        public override ValidationResult Validate(GetItemAttributeRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.CategoryID) || instance.CategoryID == "-1")
            {
                //产品类别不能为空
                result.Errors.Add(new ValidationFailure("CategoryCode", Const.Err_CategoryIDIsNull, "Err_CategoryIDIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    #endregion
}
