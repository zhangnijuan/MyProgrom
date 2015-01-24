using Ndtech.PortalModel.ViewModel;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
     [Alias("gl_appdata")]
   public class AppData
    {
         [PrimaryKey]
         [Alias("id")]
         [AutoIncrement]
         public long ID { get; set; }
        
         [Alias("appkey")]
         [StringLengthAttribute(128)]
       public string AppKey{get;set;}
        
         [Alias("secretkey")]
         [StringLengthAttribute(128)]
       public string Secretkey{get;set;}
        
         [Alias("a")]
       public int A{get;set;}
         [Alias("provide")]
         public AuthProvide Provide { get; set; }

         [Alias("uid")]
         public long UserID { get; set; }
    }
}
