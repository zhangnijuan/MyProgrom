using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ndtech.PortalService.DataModel
{
    /// <summary>
    /// 订单主表
    /// add by liuzhiqiang 2014/12/24
    /// </summary>
    [Alias("pur_order")]
    class PurOrder
    {
        /// <summary>
        /// id
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
        /// 订单编码
        /// </summary>
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string OrderCode { get; set; }

       
        /// <summary>
        /// 订单主题
        /// </summary>
        [Alias("subject")]
        [StringLengthAttribute(64)]
        public string Subject { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Alias("su")]
        public long Supply { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [Alias("su_c")]
        [StringLengthAttribute(32)]
        public string SupplyCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Alias("su_n")]
        [StringLengthAttribute(32)]
        public string SupplyName { get; set; }

        /// <summary>
        /// 供应方标识
        /// </summary>
        [Alias("sa")]
        public int SAccountID { get; set; }

        /// <summary>
        /// 质检信息
        /// </summary>
        [Alias("checktype")]
        public int CheckType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Alias("taxrate")]
        public decimal TaxRate { get; set; }

       
        #region 采购方信息

        /// <summary>
        /// 采购方联系人ID
        /// </summary>
        [Alias("sysid")]
        [StringLengthAttribute(32)]
        public long SysID { get; set; }

        /// <summary>
        /// 采购方联系人编码
        /// </summary>
        [Alias("syscode")]
        [StringLengthAttribute(32)]
        public string SysCode { get; set; }

        /// <summary>
        /// 采购方联系人名称
        /// </summary>
        [Alias("sysname")]
        [StringLengthAttribute(64)]
        public string SysName { get; set; }

        /// <summary>
        /// 采购方手机
        /// </summary>
        [Alias("mobile")]
        [StringLengthAttribute(32)]
        public string Mobile { get; set; }


        /// <summary>
        /// 采购方邮箱
        /// </summary>
        [Alias("emialinfo")]
        [StringLengthAttribute(32)]
        public string EmailInfo { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 数量总计
        /// </summary>
        [Alias("qty")]
        public decimal Quantity { get; set; }


        /// <summary>
        /// 付款方式
        /// </summary>
        [Alias("paytype")]
        [StringLengthAttribute(8)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [Alias("invoicetype")]
        [StringLengthAttribute(8)]
        public string InvoiceType { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [Alias("mm")]
        [StringLengthAttribute(1024)]
        public string MM { get; set; }

        #endregion

        /// <summary>
        /// 评价
        /// </summary>
        [Alias("evaluation")]
        public int Evaluation { get; set; }

        /// <summary>
        /// 供应方报价人ID
        /// </summary>
        [Alias("linkmanid")]
        [StringLengthAttribute(32)]
        public long LinkManID { get; set; }

        /// <summary>
        /// 供应方报价人编码
        /// </summary>
        [Alias("linkmancode")]
        [StringLengthAttribute(32)]
        public string LinkManCode { get; set; }

        /// <summary>
        /// 供应方报价人名称
        /// </summary>
        [Alias("linkmanname")]
        [StringLengthAttribute(64)]
        public string LinkManName { get; set; }
        /// <summary>
        /// 发货方联系人
        /// </summary>
        [Alias("slinkname")]
        [StringLengthAttribute(64)]
        public string SLinkName { get; set; }
        /// <summary>
        /// 发货方联系人手机
        /// </summary>
        [Alias("smobile")]
        [StringLengthAttribute(64)]
        public string SMobile { get; set; }
        /// <summary>
        /// 发货方联系人邮箱
        /// </summary>
        [Alias("semailinfo")]
        [StringLengthAttribute(64)]
        public string SEmailInfo { get; set; }
        /// <summary>
        /// 合计信息
        /// </summary>
        [Alias("totalamt")]
        public decimal TotalAmt { get; set; }


        #region 单证信息

        /// <summary>
        /// 单证状态 0 报价草稿 1 已报价 2 已优选 3 已下单 4已发货 5 关闭
        /// </summary>
        [Alias("state")]
        public int State { get; set; }

        /// <summary>
        /// 单证状态 
        /// </summary>
        [Alias("state_enum")]
        [StringLengthAttribute(32)]
        public string State_Enum { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [Alias("eid")]
        public long EID { get; set; }

        /// <summary>
        /// 制单人code
        /// </summary>
        [Alias("eid_syscode")]
        [StringLengthAttribute(32)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人Name
        /// </summary>
        [Alias("eid_usrname")]
        [StringLengthAttribute(50)]
        public string EIDName { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [Alias("createdate")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 员工角色
        /// </summary>
        [Alias("roleid")]
        public int RoleID { get; set; }

        /// <summary>
        /// 员工角色名称
        /// </summary>
        [Alias("role_enum")]
        [StringLengthAttribute(32)]
        public string Role_Enum { get; set; }



        #endregion

      
        /// <summary>
        /// 收货地址id
        /// </summary>
        [Alias("addressid")]
        public long AddressID { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Alias("address")]
        [StringLengthAttribute(1024)]
        public string Address { get; set; }

        /// <summary>
        /// 发货地址id
        /// </summary>
        [Alias("saddressid")]
        public long SAddressID { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        [Alias("saddress")]
        [StringLengthAttribute(1024)]
        public string SAddress { get; set; }

        /// <summary>
        ///出厂报告资源ID
        /// </summary>
        [Alias("factoryreportresources")]
        public long FactoryReportResources { get; set; }

        /// <summary>
        /// 第三方质检报告资源ID
        /// </summary>
        [Alias("thirdreportresources")]
        public long ThirdReportResources { get; set; }

        /// <summary>
        /// 已到货状态
        /// </summary>
        [Alias("arrivalstate")]
        public int ArrivalState { get; set; }

        /// <summary>
        /// 已收款状态
        /// </summary>
        [Alias("receivingstate")]
        public int ReceivingState { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        [Alias("orderdate")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 流转状态
        /// </summary>
        [Alias("circulationstate")]
        public int CirculationState { get; set; }


    }
}
