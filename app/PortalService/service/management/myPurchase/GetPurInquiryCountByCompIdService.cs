using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class GetPurInquiryCountByCompIdService : Service, IGet<GetPurInquiryCountByCompIdRequest>
    {
        public object Get(GetPurInquiryCountByCompIdRequest request)
        {
            PurInquiryCountResponce response = new PurInquiryCountResponce();

            return response;
        }
    }
}
