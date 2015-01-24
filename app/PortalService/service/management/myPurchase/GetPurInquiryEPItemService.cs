using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    public class GetPurInquiryEPItemService : Service, IGet<GetPurInquiryEPItemRequest>
    {
        #region Validate

        ///// <summary>
        ///// 重写验证方法
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public override ValidationResult Validate(GetPurInquiryEPItemRequest instance)
        //{

        
        //}
        #endregion
        private EPItemList ToEntity(EnterpriseItem item)
        {
            return item.TranslateTo<EPItemList>();
        }
        public object Get(GetPurInquiryEPItemRequest request)
        {
            GetPurInquiryEPItemResponse response = new GetPurInquiryEPItemResponse();
            var list = this.Db.Where<EnterpriseItem>("A", request.AccountID);
            if (list != null)
            {
                var data = list.OrderBy(n => n.ID);
                List<EPItemList> listModel = new List<EPItemList>();
                foreach (var item in data)
                {
                    var EPItem = ToEntity(item);
                    //var resources = this.Db.FirstOrDefault<Resources>(r => r.DocumentID == item.ID);
                    //staff.PicResources = ToEntity(resources);
                    listModel.Add(EPItem);
                }
                response.Data = listModel;
            }
            return response;
        }
    }
}
