using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
     [Alias("udoc_snum")]
   public class DocumentNumber
    {
       [PrimaryKey]
       [Alias("id")]
       [AutoIncrement]
       public long ID { get; set; }

         [Alias("a")]
       public int AccountID { get; set; }


         [Alias("snumbertype")]
         [StringLengthAttribute(8)]
         public string SnumType { get; set; }

         [Alias("snumber")]
         public int Number { get; set; }

    }
}
