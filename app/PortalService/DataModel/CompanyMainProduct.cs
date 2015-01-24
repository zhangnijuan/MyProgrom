using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业主营产品档案
    /// </summary>
    [Alias("udoc_comp_category")]
   public  class CompanyMainProduct
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long Id { get; set; }
        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }
        /// <summary>
        /// 产品类别编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(100)]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 产品类别名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(100)]
        public string CategoryName { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        [Alias("pid")]
        public long ParentID { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [Alias("mid")]
        public long CompID { get; set; }

    }
}
