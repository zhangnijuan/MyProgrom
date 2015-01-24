using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业简介
    /// </summary>
    [Alias("udoc_comp_introduction")]
    public class CompanyIntro
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
        /// 公司id
        /// </summary>
        [Alias("mid")]
        public long CompId { get; set; }
        /// <summary>
        /// 企业简介
        /// </summary>
        [Alias("comdetails")]
        public string CompanyDetails { get; set; }
        /// <summary>
        /// 第一张图片id
        /// </summary>
        [Alias("comrid1")]
        public long CompanyOneResources { get; set; }
        /// <summary>
        /// 第二张图片id
        /// </summary>
        [Alias("comrid2")]
        public long CompanyTwoResources { get; set; }
        /// <summary>
        /// 第三张图片id
        /// </summary>
        [Alias("comrid3")]
        public long CompanyThreeResources { get; set; }
        /// <summary>
        /// 第四张图片id
        /// </summary>
        [Alias("comrid4")]
        public long CompanyFourResources { get; set; }
        /// <summary>
        /// 第五张图片id
        /// </summary>
        [Alias("comrid5")]
        public long CompanyFiveResources { get; set; }

    }
}
