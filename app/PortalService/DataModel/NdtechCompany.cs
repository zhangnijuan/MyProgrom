using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 注册会员_公司档案
    /// add by yangshuo 2014/12/03
    /// </summary>
    [Alias("udoc_comp")]
    public class NdtechCompany
    {
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string CompCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(100)]
        public string CompName { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        [Alias("tel")]
        [StringLengthAttribute(50)]
        public string TelPhone { get; set; }

        /// <summary>
        /// 公司传真
        /// </summary>
        [Alias("fax")]
        [StringLengthAttribute(50)]
        public string Fax { get; set; }

        /// <summary>
        /// 公司网站
        /// </summary>
        [Alias("url")]
        [StringLengthAttribute(100)]
        public string WebUrl { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("conper")]
        [StringLengthAttribute(50)]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [Alias("contel")]
        [StringLengthAttribute(50)]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 公司规模id
        /// </summary>
        [Alias("sid")]
        public int ScaleId { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        [Alias("scale")]
        [StringLengthAttribute(100)]
        public string CompanyScale { get; set; }

        /// <summary>
        /// 公司面积
        /// </summary>
        [Alias("area")]
        [StringLengthAttribute(100)]
        public string CompanyArea { get; set; }
       

      
        /// <summary>
        /// 是否已经认证
        /// </summary>
         [Alias("approved")]
        public int Approved { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        [Alias("createdate")]
        public DateTime Createdate { get; set; }
        /// <summary>
        /// 收藏次数(人气)
        /// </summary>
         [Alias("favorites")]
        public int Favorites { get; set; }
         /// <summary>
         /// 发布产品数量
         /// </summary>
         [Alias("releases")]
         public int Releases { get; set; }
        /// <summary>
        /// 询价单数量
        /// </summary>
        [Alias("inquirynumber")]
        public int inquiryNumber { get; set; }
        /// <summary>
        /// Logo资源ID
        /// </summary>
        [Alias("logo_rid")]
        public long LogoResources { get; set; }
    }
}
