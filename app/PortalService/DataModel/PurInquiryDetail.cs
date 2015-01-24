using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;
using ServiceStack.ServiceHost;

namespace Ndtech.PortalService.DataModel
{
    [Alias("pur_inquirydetail")]
   public class PurInquiryDetail
    {
        /// <summary>
        /// 标示ID
        /// </summary>
        [PrimaryKey]
        [Alias("id")]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [Alias("a")]
        public int AccountID { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        [Alias("mid")]
        public long MID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Alias("i")]
        public long ItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("i_c")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("i_n")]
        public string ItemName { get; set; }


        /// <summary>
        /// 单位id
        /// </summary>
        [Alias("u")]
        public long UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [Alias("u_c")] 
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
       [Alias("u_n")] 
        public string UnitName { get; set; }

       /// <summary>
       /// 产品分类ID
       /// </summary>
       [Alias("category")]
       public long CategoryID { get; set; }


       /// <summary>
       /// 产品分类编码
       /// </summary>
       [Alias("category_c")]
       [StringLengthAttribute(32)]
       public string CategoryCode { get; set; }

       /// <summary>
       /// 产品分类编码
       /// </summary>
       [Alias("category_n")]
       [StringLengthAttribute(64)]
       public string CategoryName { get; set; }

       /// <summary>
       /// 产品一级类别id
       /// </summary>
       [Alias("categoryfirstid")]
       public long CategoryFirstID { get; set; }


       /// <summary>
       /// 产品一级类别代码
       /// </summary>
       [Alias("categoryfirstcode")]
       [StringLengthAttribute(32)]
       public string CategoryFirstCode { get; set; }

       /// <summary>
       /// 产品一级类别名称
       /// </summary>
       [Alias("categoryfirstname")]
       [StringLengthAttribute(100)]
       public string CategoryFirstName { get; set; }

       /// <summary>
       /// 产品二级类别id
       /// </summary>
       [Alias("categorysecondid")]
       public long CategorySecondID { get; set; }


       /// <summary>
       /// 产品二级类别代码
       /// </summary>
       [Alias("categorysecondcode")]
       [StringLengthAttribute(32)]
       public string CategorySecondCode { get; set; }

       /// <summary>
       /// 产品二级类别名称
       /// </summary>
       [Alias("categorysecondname")]
       [StringLengthAttribute(100)]
       public string CategorySecondName { get; set; }

       /// <summary>
       /// 产品三级类别id
       /// </summary>
       [Alias("categorythirdid")]
       public long CategoryThirdID { get; set; }


       /// <summary>
       /// 产品三级类别代码
       /// </summary>
       [Alias("categorythirdcode")]
       [StringLengthAttribute(32)]
       public string CategoryThirdCode { get; set; }

       /// <summary>
       /// 产品三级类别名称
       /// </summary>
       [Alias("categorythirdname")]
       [StringLengthAttribute(100)]
       public string CategoryThirdName { get; set; }
       /// <summary>
       /// 数量
       /// </summary>
       [Alias("qty")]
       public long Qty { get; set; }


       /// <summary>
       /// 产品属性名称
       /// </summary>
       [Alias("propertyname")]
       [StringLengthAttribute(1024)]
       public string PropertyName { get; set; }


       /// <summary>
       /// 交货期
       /// </summary>
       [Alias("deliverydate")]
       [StringLengthAttribute(100)]
       public DateTime DeliveryDate { get; set; }

       /// <summary>
       /// 备注
       /// </summary>
       [Alias("mm")]
       [StringLengthAttribute(1024)]
       public string MM { get; set; }

       /// <summary>
       /// 平台标准产品编码
       /// </summary>
       [Alias("standarditemcode")]
       public string StandardItemCode { get; set; }

       /// <summary>
       /// 平台标准产品名称
       /// </summary>
       [Alias("standarditemname")]
       public string StandardItemName { get; set; }

       /// <summary>
       /// 优选结果
       /// 拼接格式-》公司名称:数量；
       /// 优选点击"确定"更新此字段
       /// </summary>
       [Alias("selectresults")]
       [StringLengthAttribute(1024)]
       public string SelectResults { get; set; }

       /// <summary>
       /// 优选说明
       /// 优选点击"生成订单"更新此字段
       /// </summary>
       [Alias("selectmm")]
       [StringLengthAttribute(1024)]
       public string SelectMM { get; set; }

       /// <summary>
       /// 产品描述
       /// </summary>
       [Alias("remark")]
       [StringLengthAttribute(1024)]
       public string Remark { get; set; }
    }
}
