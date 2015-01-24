using System;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 企业产品档案(采购物品or供应产品)
    /// add by yangshuo 2014/12/11
    /// </summary>
    [Alias("udoc_enterprise_item")]
    public class EnterpriseItem
    {
        /// <summary>
        /// 产品id
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
        /// 平台产品代码
        /// </summary>
        [Alias("standard_c")]
        [StringLengthAttribute(64)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台产品名称
        /// </summary>
        [Alias("standard_n")]
        [StringLengthAttribute(1024)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(64)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Alias("n")]
        [StringLengthAttribute(1024)]
        public string ItemName { get; set; }

        /// <summary>
        /// 一级产品分类id
        /// </summary>
        [Alias("category1")]
        public long Category1ID { get; set; }

        /// <summary>
        /// 一级产品分类代码
        /// </summary>
        [Alias("category1_c")]
        [StringLengthAttribute(32)]
        public string Category1Code { get; set; }

        /// <summary>
        /// 一级产品分类名称
        /// </summary>
        [Alias("category1_n")]
        [StringLengthAttribute(64)]
        public string Category1Name { get; set; }

        /// <summary>
        /// 二级产品分类id
        /// </summary>
        [Alias("category2")]
        public long Category2ID { get; set; }

        /// <summary>
        /// 二级产品分类代码
        /// </summary>
        [Alias("category2_c")]
        [StringLengthAttribute(32)]
        public string Category2Code { get; set; }

        /// <summary>
        /// 二级产品分类名称
        /// </summary>
        [Alias("category2_n")]
        [StringLengthAttribute(64)]
        public string Category2Name { get; set; }

        /// <summary>
        /// 三级产品分类id
        /// </summary>
        [Alias("category3")]
        public long Category3ID { get; set; }

        /// <summary>
        /// 三级产品分类代码
        /// </summary>
        [Alias("category3_c")]
        [StringLengthAttribute(32)]
        public string Category3Code { get; set; }

        /// <summary>
        /// 三级产品分类名称
        /// </summary>
        [Alias("category3_n")]
        [StringLengthAttribute(64)]
        public string Category3Name { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 参考价格(市场价)
        /// </summary>
        [Alias("prc")]
        [DecimalLength(19, 6)]
        public decimal SalPrc { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        [Alias("u")]
        public long UnitID { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [Alias("u_c")]
        [StringLengthAttribute(32)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Alias("u_n")]
        [StringLengthAttribute(32)]
        public string UnitName { get; set; }

        /// <summary>
        /// 价格说明
        /// </summary>
        [Alias("pricedes")]
        [StringLengthAttribute(1024)]
        public string PriceDes { get; set; }

        /// <summary>
        /// 图片资源外键
        /// </summary>
        [Alias("itemrid")]
        public long ItemResources { get; set; }

        /// <summary>
        /// 详细说明
        /// </summary>
        [Alias("description")]
        public string Description { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Alias("contacts")]
        [StringLengthAttribute(32)]
        public string Contacts { get; set; }

        /// <summary>
        /// 联系人手机号
        /// </summary>
        [Alias("contactstel")]
        [StringLengthAttribute(32)]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 付款方式说明
        /// </summary>
        [Alias("payment")]
        [StringLengthAttribute(1024)]
        public string Payment { get; set; }

        /// <summary>
        /// 所在区
        /// </summary>
        [Alias("district")]
        [StringLengthAttribute(32)]
        public string District { get; set; }

        /// <summary>
        /// 所在市
        /// </summary>
        [Alias("city")]
        [StringLengthAttribute(32)]
        public string City { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Alias("province")]
        [StringLengthAttribute(32)]
        public string Province { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(1120)]
        public string Address { get; set; }

        /// <summary>
        /// 发货地址ID
        /// </summary>
        [Alias("addressid")]
        public long AddressID { get; set; }

        /// <summary>
        /// 业务分类
        /// 0 不分类
        /// 1 采购 
        /// 2 销售
        /// </summary>
        [Alias("biztype")]
        public int BizType { get; set; }

        /// <summary>
        /// 物品状态
        /// 0草稿 1已发布 2已下架 3已删除
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 物品状态_枚举
        /// </summary>
        [Alias("state_enum")]
        [StringLengthAttribute(8)]
        public string StateEnum { get; set; }

        /// <summary>
        /// 建档日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        [Alias("modifydate")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 发布时间
        /// 发布供应产品使用
        /// </summary>
        [Alias("publishdate")]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 是否认证
        /// 1认证 0未认证
        /// </summary>
        [Alias("iscertification")]
        public int IsCertification { get; set; }
    }
}