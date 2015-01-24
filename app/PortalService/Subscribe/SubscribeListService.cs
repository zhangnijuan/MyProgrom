using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Subscribe
{
    public class SubscribeListService : Service, IPost<SubscribeViewListRequest>
    {
        public object Post(SubscribeViewListRequest request)
        {
            SubscribeViewListResponse response = new SubscribeViewListResponse();
            if (request.SubscriList != null && request.SubscriList.Count > 0)
            {
                foreach (var item in request.SubscriList)
                {
                    SubscribeAbstract subscribe = null;
                    if (item.Subtype == 0) //收藏采购信息
                    {
                        subscribe = this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Inquiry");
                    }
                    else if (item.Subtype == 1) // 收藏产品信息
                    {
                        subscribe = this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Product");
                    }
                    else //收藏企业信息
                    {
                        subscribe = this.GetAppHost().GetContainer().TryResolveNamed<SubscribeAbstract>("Company");
                    }
                    bool sucess = subscribe.ReceiveSubscribe(Convert.ToInt64(item.FromDataID), item.Substate, item.Subtype, item.AccountID, Convert.ToInt64(item.Subsciber), item.SubscriberCode, item.SubscriberName);
                    response.Success = sucess;
                }
            }
            return response;

        }
    }
}
