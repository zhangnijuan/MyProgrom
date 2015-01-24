using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 员工上级
    /// </summary>
     [Alias("udoc_superior")]
    public  class SuperiorEmployee
    {
         /// <summary>
         /// 主键id
         /// </summary>
         [PrimaryKey]
         [Alias("id")]
         public long Id { get; set; }
         /// <summary>
         /// 员工Id
         /// </summary>
         [Alias("mid")]
         public long StaffId { get; set; }
         /// <summary>
         /// 上级员工id
         /// </summary>
         [Alias("eid")]
         public long EId { get; set; }
         /// <summary>
         /// 上级员工编号
         /// </summary>
          [Alias("eid_syscode")]
          [StringLengthAttribute(100)]
         public string EIdCode { get; set; }
         /// <summary>
         /// 上级员工姓名
         /// </summary>
          [Alias("eid_username")]
          [StringLengthAttribute(100)]
          public string EIdName { get; set; }
          
    }
}
