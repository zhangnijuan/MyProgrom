using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Route("/hello")]
    public class HelloRequest : IReturn<HelloResponse>
    {
        public string name { get; set; }
    }
    public class HelloResponse { 
    }
}
