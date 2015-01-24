using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class GetPublishCountByCompIdService : Service, IGet<GetPublishCountByCompIdRequest>
    {
        public object Get(GetPublishCountByCompIdRequest request)
        {
            PublishCountResponce response = new PublishCountResponce();

            return response;
        }
    }
}
