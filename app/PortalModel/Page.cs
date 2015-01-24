using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel
{
    [DataContract]
    public class Page
    {
       [DataMember(Order = 1)]
        public int RowCount { get; set; }
       [DataMember(Order = 2)]
        public int PageNumber { get; set; }
       [DataMember(Order = 3)]
        public int PageIndex { get; set; }
    }
}
