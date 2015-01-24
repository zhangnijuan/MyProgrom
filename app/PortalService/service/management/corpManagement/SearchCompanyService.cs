using Ndtech.PortalModel.ViewModel;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class SearchCompanyService : Service
    {
        public object Any(SearchCompanyRequest request)
        {



            SearchCompanyResponce response = new SearchCompanyResponce();

            return response;

        }
    }
}
