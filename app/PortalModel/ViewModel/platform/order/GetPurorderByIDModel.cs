using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同显示订单详情接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/{counterparty}/purorder/{ID}", HttpMethods.Get, Notes = "显示采购订单")]
    [DataContract]
    public class GetPurorderByIDRequest : IReturn<GetPurorderByIDResponse>
    {
        [DataMember(Order = 1)]
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string AppKey { get; set; }

        [DataMember(Order = 2)]
        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
      ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Secretkey { get; set; }

        [DataMember(Order = 3)]
        [ApiMember(Description = "账套Id",
      ParameterType = "json", DataType = "int", IsRequired = true)]
        public int AccountId { get; set; }

        [DataMember(Order = 4)]
        [ApiMember(Description = "判断是采购方还是供应方",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string counterparty { get; set; }

        [DataMember(Order = 5)]
        [ApiMember(Description = "标识",
      ParameterType = "json", DataType = "long", IsRequired = true)]
        public long ID { get; set; }


        
    }
    [DataContract]
    public class GetPurorderByIDResponse
    {
        public GetPurorderByIDResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }

        [DataMember(Order = 3)]
        public PurorderInfoList Data { get; set; }
    }
    [DataContract]
    public class PurorderInfoList
    {
        #region 订单主表信息
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }


        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        [DataMember(Order = 3)]
        public string OrderCode { get; set; }


        /// <summary>
        /// 订单主题
        /// </summary>
        [DataMember(Order = 4)]
        public string Subject { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [DataMember(Order = 5)]
        public long Supply { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [DataMember(Order = 6)]
        public string SupplyCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DataMember(Order = 7)]
        public string SupplyName { get; set; }

        /// <summary>
        /// 供应方标识
        /// </summary>
        [DataMember(Order = 8)]
        public int SAccountID { get; set; }

        /// <summary>
        /// 质检信息
        /// </summary>
        [DataMember(Order = 9)]
        public int CheckType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [DataMember(Order = 10)]
        public decimal TaxRate { get; set; }


        #region 采购方信息

        /// <summary>
        /// 采购方联系人ID
        /// </summary>
        [DataMember(Order = 11)]
        public string SysID { get; set; }

        /// <summary>
        /// 采购方联系人编码
        /// </summary>
        [DataMember(Order = 12)]
        public string SysCode { get; set; }

        /// <summary>
        /// 采购方联系人名称
        /// </summary>
        [DataMember(Order = 13)]
        public string SysName { get; set; }

        /// <summary>
        /// 采购方手机
        /// </summary>
        [DataMember(Order = 14)]
        public string Mobile { get; set; }


        /// <summary>
        /// 采购方邮箱
        /// </summary>
        [DataMember(Order = 15)]
        public string EmailInfo { get; set; }

        #endregion

        #region 付款信息

        /// <summary>
        /// 数量总计
        /// </summary>
        [DataMember(Order = 16)]
        public decimal Quantity { get; set; }


        /// <summary>
        /// 付款方式
        /// </summary>
        [DataMember(Order = 17)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [DataMember(Order = 18)]
        public string InvoiceType { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(Order = 19)]
        public string MM { get; set; }

        #endregion

        /// <summary>
        /// 评价
        /// </summary>
        [DataMember(Order = 20)]
        public string Evaluation { get; set; }

        /// <summary>
        /// 供应方报价人ID
        /// </summary>
        [DataMember(Order = 21)]
        public string LinkManID { get; set; }

        /// <summary>
        /// 供应方报价人编码
        /// </summary>
        [DataMember(Order = 22)]
        public string LinkManCode { get; set; }

        /// <summary>
        /// 供应方报价人名称
        /// </summary>
        [DataMember(Order = 23)]
        public string LinkManName { get; set; }

        /// <summary>
        /// 发货方联系人
        /// </summary>
      [DataMember(Order = 24)]
        public string SLinkName { get; set; }
        /// <summary>
        /// 发货方联系人手机
        /// </summary>
        [DataMember(Order = 25)]
        public string SMobile { get; set; }
        /// <summary>
        /// 发货方联系人邮箱
        /// </summary>
        [DataMember(Order = 26)]
        public string SEmailInfo { get; set; }

        /// <summary>
        /// 合计信息
        /// </summary>
        [DataMember(Order = 27)]
        public decimal TotalAmt { get; set; }


        #region 单证信息

        /// <summary>
        /// 单证状态 0 订单草稿 1 待供应方确认 2 交易中 3 已取消 4 已完成
        /// </summary>
        [DataMember(Order = 28)]
        public int State { get; set; }

        /// <summary>
        /// 单证状态 
        /// </summary>
        [DataMember(Order = 29)]
        public string State_Enum { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        [DataMember(Order = 30)]
        public long EID { get; set; }

        /// <summary>
        /// 制单人code
        /// </summary>
        [DataMember(Order = 31)]
        public string EIDCode { get; set; }

        /// <summary>
        /// 制单人Name
        /// </summary>
        [DataMember(Order = 32)]
        public string EIDName { get; set; }

        /// <summary>
        /// 制单日期
        /// </summary>
        [DataMember(Order = 33)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 员工角色
        /// </summary>
        [DataMember(Order = 34)]
        public int RoleID { get; set; }

        /// <summary>
        /// 员工角色名称
        /// </summary>
        [DataMember(Order = 35)]
        public string Role_Enum { get; set; }



        #endregion


        /// <summary>
        /// 收货地址id
        /// </summary>
        [DataMember(Order = 36)]
        public long AddressID { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [DataMember(Order = 37)]
        public string Address { get; set; }

        /// <summary>
        /// 发货地址id
        /// </summary>
        [DataMember(Order = 38)]
        public long SAddressID { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        [DataMember(Order = 39)]
        public string SAddress { get; set; }

        /// <summary>
        ///出厂报告资源ID
        /// </summary>
        [DataMember(Order = 40)]
        public long FactoryReportResources { get; set; }

        /// <summary>
        /// 第三方质检报告资源ID
        /// </summary>
        [DataMember(Order = 41)]
        public long ThirdReportResources { get; set; }

        /// <summary>
        /// 已到货状态
        /// </summary>
        [DataMember(Order = 42)]
        public int ArrivalState { get; set; }

        /// <summary>
        /// 已收款状态
        /// </summary>
        [DataMember(Order = 43)]
        public int ReceivingState { get; set; }

        /// <summary>
        /// 本次收款金额
        /// </summary>
        [DataMember(Order = 44)]
        public decimal MyGetAmt { get; set; }

        /// <summary>
        /// 本次收款备注
        /// </summary>
        [DataMember(Order = 44)]
        public string MyMM { get; set; }

        /// <summary>
        /// 出厂报告资源对象
        /// </summary>
        [DataMember(Order = 45)]
        public List<ReturnPicResources> FactoryReportData { get; set; }

        /// <summary>
        /// 第三方质检报告资源对象
        /// </summary>
        [DataMember(Order = 46)]
        public List<ReturnPicResources> ThirdReportData { get; set; }


        /// <summary>
        /// 已发货总计
        /// </summary>
        [DataMember(Order = 47)]
        public decimal TotalSalQty { get; set; }

        /// <summary>
        /// 已付款总计
        /// </summary>
        [DataMember(Order = 48)]
        public decimal TotalPayAmt { get; set; }

        /// <summary>
        /// 已收款总计
        /// </summary>
        [DataMember(Order = 48)]
        public decimal TotalRecvAmt { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        [DataMember(Order = 49)]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 流转状态
        /// </summary>
        [DataMember(Order = 50)]
        public string CirculationState { get; set; }

#endregion

        //订单明细
        [DataMember(Order = 47)]
        public List<PurOrderDetailList> ItemList { get; set; }

        //收货地址
        [DataMember(Order = 48)]
        public List<EnterpriseAddressList> GetAddressList { get; set; }

        //发货地址
        [DataMember(Order = 49)]
        public List<EnterpriseAddressList> SendAddressList { get; set; }

        ////收发货记录
        //[DataMember(Order = 42)]
        //public List<SalQuotDetailList> SalQuotList { get; set; }

        ////付款收款
        //[DataMember(Order = 46)]
        //public List<ArapReceiveList> ArapReceList { get; set; }

        //历史付款记录
        [DataMember(Order = 50)]
        public List<ArapReceiveList> PayArapReceList { get; set; }

        //历史收款记录
        [DataMember(Order = 51)]
        public List<ArapReceiveList> GetArapReceList { get; set; }

        /// <summary>
        /// 采购方公司名称
        /// </summary>
        [DataMember(Order = 13)]
        public string PurName { get; set; }

    }
    /// <summary>
    /// 订单明细
    /// </summary>
    [DataContract]
    public class PurOrderDetailList
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [DataMember(Order = 1)]
        public string ID { get; set; }

        /// <summary>
        /// 采购方企业标识
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 主表外键
        /// </summary>
        [DataMember(Order = 3)]
        public long MID { get; set; }

        /// <summary>
        /// 平台标准产品编码
        /// </summary>
        [DataMember(Order = 4)]
        public string StandardItemCode { get; set; }

        /// <summary>
        /// 平台标准产品名称
        /// </summary>
        [DataMember(Order = 5)]
        public string StandardItemName { get; set; }

        /// <summary>
        /// 企业产品分类编码
        /// </summary>
        [DataMember(Order = 6)]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 企业产品分类名称
        /// </summary>
        [DataMember(Order = 7)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMember(Order = 8)]
        public long ItemID { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [DataMember(Order = 9)]
        public string ItemCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Order = 10)]
        public string ItemName { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember(Order = 11)]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [DataMember(Order = 12)]
        public string UnitName { get; set; }

        /// <summary>
        /// 询价数量
        /// </summary>
        [DataMember(Order = 13)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 优选数量
        /// </summary>
        [DataMember(Order = 14)]
        public decimal SelectQty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember(Order = 15)]
        public decimal Prc { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember(Order = 16)]
        public decimal Amt { get; set; }

        /// <summary>
        /// 产品属性值
        /// </summary>
        [DataMember(Order = 17)]
        public string PropertyName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 18)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 询价单明细ID
        /// </summary>
        [DataMember(Order = 19)]
        public long Inquirydid { get; set; }

        /// <summary>
        /// 报价单明细ID
        /// </summary>
        [DataMember(Order = 20)]
        public long Quotationdid { get; set; }

        /// <summary>
        /// 购物车明细ID
        /// </summary>
        [DataMember(Order = 21)]
        public long ShoppingCartdid { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [DataMember(Order = 22)]
        public string Remark { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(Order = 23)]
        public string MM { get; set; }

        /// <summary>
        /// 供应方标识
        /// </summary>
        [DataMember(Order = 24)]
        public int SAccountID { get; set; }

        /// <summary>
        /// 累计到货数量
        /// </summary>
        [DataMember(Order = 25)]

        public decimal Arrivalqty { get; set; }
        /// <summary>
        /// 累计收款金额
        /// </summary>
        [DataMember(Order = 26)]
        public decimal ReceivingAmt { get; set; }


        /// <summary>
        /// 本次收货数量
        /// </summary>
        [DataMember(Order = 27)]

        public decimal ThisGetqty { get; set; }


        /// <summary>
        /// 本次发货备注
        /// </summary>
        [DataMember(Order = 28)]

        public string ThisMM { get; set; }

        /// <summary>
        /// 已发货
        /// </summary>
        [DataMember(Order = 29)]
        public decimal TotalDetSalQty { get; set; }

        /// <summary>
        /// 已收货
        /// </summary>
        [DataMember(Order = 30)]
        public decimal TotalDetGetQty { get; set; }

        ////收发货记录
        //[DataMember(Order = 25)]
        //public List<SalQuotDetailList> SalQuotList { get; set; }

        //历史收货记录
        [DataMember(Order = 31)]
        public List<SalQuotDetailList> ReceiveSalQuotList { get; set; }

        //历史发货记录
        [DataMember(Order = 32)]
        public List<SalQuotDetailList> SendSalQuotList { get; set; }

        //明细附件资源对象
        [DataMember(Order = 33)]
        public List<ReturnPicResources> FileUploadData { get; set; }
    }

    /// <summary>
    /// 收发货地址信息
    /// </summary>
    [DataContract]
    public class EnterpriseAddressList
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }

        /// <summary>
        /// 帐套
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [DataMember(Order = 3)]
        public long CompID { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [DataMember(Order = 4)]
        public string CompName { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [DataMember(Order = 5)]
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [DataMember(Order = 6)]
        public string City { get; set; }

        /// <summary>
        /// 所在地区
        /// </summary>
        [DataMember(Order = 7)]
        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember(Order = 8)]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember(Order = 9)]
        public string ZipCode { get; set; }

        /// <summary>
        /// 0 否 1 是
        /// </summary>
        [DataMember(Order = 10)]
        public int IsDef { get; set; }

        /// <summary>
        /// 总地址
        /// </summary>
        [DataMember(Order = 11)]
        public string FullAddress { get; set; }
 
    }
    /// <summary>
    /// 收发货记录表
    /// </summary>
    [DataContract]
    public class SalQuotDetailList
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }


        /// <summary>
        /// 采购方企业标识
        /// </summary>
        [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 供应方企业标识
        /// </summary>
        [DataMember(Order = 3)]
        public int SAccountID { get; set; }

        /// <summary>
        /// 订单明细ID
        /// </summary>
        [DataMember(Order = 4)]
        public long DetailID { get; set; }


        /// <summary>
        /// 收货日期
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 发货备注
        /// </summary>
        [DataMember(Order = 6)]
        public string MM { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        [DataMember(Order = 7)]
        public decimal DeliveryQty { get; set; }

        /// <summary>
        /// 收货数量
        /// </summary>
        [DataMember(Order = 8)]
        public decimal ArrivalQty { get; set; }
    }
    /// <summary>
    /// 付款收款记录
    /// </summary>
    [DataContract]
    public class ArapReceiveList
    {
        /// <summary>
        /// id
        /// </summary>
        [DataMember(Order = 1)]
        public long ID { get; set; }


        /// <summary>
        /// 采购方企业标识
        /// </summary>
         [DataMember(Order = 2)]
        public int AccountID { get; set; }

        /// <summary>
        /// 供应方企业标识
        /// </summary>
        [DataMember(Order = 3)]
        public int SAccountID { get; set; }

        /// <summary>
        /// 订单主表ID
        /// </summary>
        [DataMember(Order = 4)]
        public long OrderID { get; set; }


        /// <summary>
        /// 付款日期
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款备注
        /// </summary>
        [DataMember(Order = 6)]
        public string MM { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        [DataMember(Order = 7)]
        public decimal Collection { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [DataMember(Order = 8)]
        public decimal Payment { get; set; }
    }

}
