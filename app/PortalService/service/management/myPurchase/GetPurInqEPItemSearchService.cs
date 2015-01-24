using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Common;

namespace Ndtech.PortalService.Auth
{
    public class GetPurInqEPItemSearchService : Service, IPost<GetPurInqEPItemSearchRequest>
    {
        public IDbConnectionFactory db { set; get; }
        private EPItemSearchList ToEntity(EnterpriseItem item)
        {
            return item.TranslateTo<EPItemSearchList>();
        }
        public object Post(GetPurInqEPItemSearchRequest request)
        {
            GetPurInqEPItemSearchResponse response = new GetPurInqEPItemSearchResponse();
            //this.Db.Where<EnterpriseItem>()
            //((n.ItemCode != null && n.ItemCode.Contains(request.ItemCode)) || (n.ItemName != null && n.ItemName.Contains(request.ItemName))) 周一添加ItemName
            var list = this.Db.Where<EnterpriseItem>(n => n.AccountID == request.AccountID &&n.ItemCode!=null&&n.ItemCode.Contains(request.ItemCode));//&& (n.ItemCode == request.ItemCode||n.ItemName == request.ItemName)
            #region
            //if(!string.IsNullOrEmpty(request.ItemCode))
            //{
            //    //
            //    list = this.Db.Where<EnterpriseItem>(n => n.AccountId == request.AccountID && n.ItemCode == request.ItemCode);
            //}
            //if (!string.IsNullOrEmpty(request.ItemName))
            //{
            //    list = this.Db.Where<EnterpriseItem>(n => n.AccountId == request.AccountID && n.ItemName == request.ItemName);
            //}
            //var list = this.Db.Where<EnterpriseItem>(n => n.a == request.AccountID && n.State == 0);
            #endregion
            if (list != null)
            {

                //分页
                var data = list.OrderBy(n => n.ID);
                    //.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize).ToList();

                //转换为前端model
                List<EPItemSearchList> listModel = new List<EPItemSearchList>();
                foreach (var item in data)
                {
                    var EPItem = ToEntity(item);
                    listModel.Add(EPItem);
                }
                response.Data = listModel;
            }
            return response;
            #region
            //using (var conn = db.OpenDbConnection())
            //{
            //    //var Itemcode = request.SearchObject.ItemCode;
            //    //var Itemname = request.SearchObject.ItemName;
            //   // var test = this.Db.Where<PurInquiryDetail>();

            //    //if(request.SearchObject!=null)
            //    //{
                    
            //    //List<EnterpriseItem> EnterpriseItem = new List<EnterpriseItem>() ;

            //        //EnterpriseItem Item = conn.QuerySingle<EnterpriseItem>(string.Format("select * from udoc_ enterprise_item  where a = '{0}' and (c like '%{1}%' or n like '%{2}%')", request.AccountID, request.SearchObject.ItemCode, request.SearchObject.ItemName));
            //        var data = conn.QuerySingle<EnterpriseItem>(string.Format("select * from udoc_ enterprise_item  where a = '{0}' and (c like '%{1}%' or n like '%{2}%')", request.AccountID, request.SearchObject.ItemCode, request.SearchObject.ItemName));
            //        if (data != null)
            //        {
            //            //List<EPItemList> listModel = new List<EPItemList>();
            //            //foreach (var item in data)
            //            //{
            //            //    var EPItem = ToEntity(item);

            //            //    listModel.Add(EPItem);
            //            //}
            //            //response.Data = listModel;

            //        }

            //   // }


            //}
            #endregion


        }
    }
}
