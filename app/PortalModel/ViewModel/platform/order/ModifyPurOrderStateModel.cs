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
    [Api("恩维协同确认订单更改状态接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountId}/{counterparty}/purorder/modify", HttpMethods.Post, Notes = "确认订单更改状态")]
    [DataContract]
    public class ModifyPurOrderStateRequest : IReturn<ModifyPurOrderStateResponse>
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
        public CounterPartyEnum counterparty { get; set; }

        //  [DataMember(Order = 5)]
        //  [ApiMember(Description = "更改状态",
        //ParameterType = "json", DataType = "int", IsRequired = true)]
        //  public int State { get; set; }


        [DataMember(Order = 5)]
        [ApiMember(Description = "修改采购订单状态，收发货修改发货地址，添加发货历史记录",
      ParameterType = "json", DataType = "PurOrderView", IsRequired = true)]
        public PurOrderView PurOrder { get; set; }


        [DataMember(Order = 6)]
        [ApiMember(Description = "业务处理类型-1 =销售方确认订单; 0 = 采购方确认订单 ; 1 =销售方确认发货;2 = 采购方确认收货;3 = 销售方收款 ;4 = 采购方付款;5 = 销售方取消;6 = 采购方取消;7 =销售方完成;8=采购方完成",
      ParameterType = "json", DataType = "BizTypeEnum", IsRequired = true)]
        public BizTypeEnum BizType { get; set; }


        [DataMember(Order = 7)]
        [ApiMember(Description = "操作人ID",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Eid { get; set; }

        [DataMember(Order = 8)]
        [ApiMember(Description = "操作人编码",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string ECode { get; set; }


        [DataMember(Order = 9)]
        [ApiMember(Description = "操作人名称",
      ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EName { get; set; }


    }

   
    [DataContract]
    public class ModifyPurOrderStateResponse
    {
        public ModifyPurOrderStateResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }


        [ApiMember(Description = "请求响应对象", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public ResponseStatus ResponseStatus { get; set; }

        [ApiMember(Description = "请求响应结果", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }

    //业务操作
    [DataContract]
    public enum BizTypeEnum
    {

        /// <summary>
        /// 销售方确认订单
        /// </summary>
        [DataMember(Order = 0)]
        SalConfrim = -1,
        /// <summary>
        /// 采购方确认订单
        /// </summary>
        [DataMember(Order = 1)]
        PurConfrim = 0,

        /// <summary>
        /// 销售方发货确认
        /// </summary>
        [DataMember(Order = 2)]
        OutConfrim = 1,

        /// <summary>
        /// 采购方收货确认
        /// </summary>
        [DataMember(Order = 3)]
        ArrivalConfrim = 2,
        /// <summary>
        /// 销售方收款
        /// </summary>
        [DataMember(Order = 4)]
        ReceConfrim = 3,

        /// <summary>
        /// 采购方付款
        /// </summary>
        [DataMember(Order = 5)]
        PayConfrim = 4,

        /// <summary>
        /// 销售方取消
        /// </summary>
        [DataMember(Order = 6)]
        SalCancel = 5,

        /// <summary>
        /// 采购方取消
        /// </summary>
        [DataMember(Order = 6)]
        PurCancel = 6,

        /// <summary>
        /// 销售方完成
        /// </summary>
        [DataMember(Order = 6)]
        SalComplete= 7,

        /// <summary>
        /// 采购方完成
        /// </summary>
        [DataMember(Order = 6)]
        PurComplete = 8,
    }

    /// <summary>
    /// 销售或者采购状态
    /// </summary>
    [DataContract]
    public enum StateEnum
    {
        /// <summary>
        /// 订单草稿
        /// </summary>
        [DataMember(Order = 1)]
        StateStart = 0,
        /// <summary>
        ///待供应方确认
        /// </summary>
        [DataMember(Order = 2)]
        StateTemp = 1,
        /// <summary>
        /// 交易中
        /// </summary>
        [DataMember(Order = 3)]
        StateMiddle = 2,
        /// <summary>
        /// 已取消
        /// </summary>
        [DataMember(Order = 4)]
        StateCancel = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        [DataMember(Order = 5)]
        StateEnd = 4

    }

    /// <summary>
    /// 订单主表信息
    /// </summary>
    [DataContract]
    public class PurOrderView
    {
        [DataMember(Order = 1)]
        public string ID { get; set; }

        [DataMember(Order = 2)]
        public StateEnum State { get; set; }

        /// 收发货方联系人
        /// </summary>
        [DataMember(Order = 3)]
        public string LinkName { get; set; }
        /// <summary>
        /// 收发货方联系人手机
        /// </summary>
        [DataMember(Order = 4)]
        public string Mobile { get; set; }
        /// <summary>
        /// 收发货方联系人邮箱
        /// </summary>
        [DataMember(Order = 5)]
        public string EmailInfo { get; set; }

        /// <summary>
        /// 合计信息
        /// </summary>
        [DataMember(Order = 6)]
        public string TotalAmt { get; set; }//加了注释

        /// <summary>
        /// 付款方式
        /// </summary>
        [DataMember(Order = 7)]
        public string PayType { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        [DataMember(Order = 8)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [DataMember(Order = 9)]
        public string TaxRate { get; set; }

        /// <summary>
        /// 质检信息
        /// </summary>
        [DataMember(Order = 10)]
        public string CheckType { get; set; }

        /// <summary>
        /// 订单主表数量总计
        /// </summary>
        [DataMember(Order = 11)]
        public string TotalQuantity { get; set; }

        /// <summary>
        /// 主表描述
        /// </summary>
        [DataMember(Order = 12)]
        public string TotalMM { get; set; }

        /// <summary>
        /// 收付款金额
        /// </summary>
        [DataMember(Order = 18)]
        public string Amt { get; set; }//本次付款金额不为空，存入付款收款记录

        /// <summary>
        /// 收付款备注
        /// </summary>
        [DataMember(Order = 19)]
        public string ArapMM { get; set; }//本次付款金额不为空，存入付款收款记录

        /// <summary>
        /// 操作人ID
        /// </summary>
        [DataMember(Order = 20)]
        public string EID { get; set; }

        /// <summary>
        /// 操作人编码
        /// </summary>
        [DataMember(Order = 21)]
        public string ECODE { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [DataMember(Order = 22)]
        public string ENAME { get; set; }

        /// <summary>
        /// 取消理由
        /// </summary>
        [DataMember(Order = 23)]
        public string CancelMM { get; set; }



        //采购方收货地址对象
        [DataMember(Order = 13)]
        public List<EnterpriseAddressObject> ReceiveAddressData { get; set; }

        //供应方发货地址对象

        [DataMember(Order = 14)]
        public List<EnterpriseAddressObject> SendAddressData { get; set; }

        //供应方出厂质检报告资源对象

        [DataMember(Order = 15)]
        public List<PicResources> factoryReportData { get; set; }

        //供应方第三方质检报告对象

        [DataMember(Order = 16)]
        public List<PicResources> thirdReportData { get; set; }

        //明细对象

        [DataMember(Order = 17)]
        public List<PurorderDetailView> PurorderDetail { get; set; }

        ///// <summary>
        ///// 下单日期
        ///// </summary>
        //[DataMember(Order = 18)]
        //public string OrderTime { get; set; }

    }
    /// <summary>
    /// 收发货地址对象
    /// </summary>
    [DataContract]
    public class EnterpriseAddressObject
    {
        [DataMember(Order = 1)]
        public long AddressID { get; set; }

        [DataMember(Order = 2)]
        public string Address { get; set; }
    }

    public class PurorderDetailView
    {
        //订单明细ID
        [DataMember(Order = 1)]
        public long ID { get; set; }
        //收发货、收付款信息
        [DataMember(Order = 2)]
        public string Quantity { get; set; }//本次发货数量不为空，存入收发货记录表

        //[DataMember(Order = 3)]
        //public string Amt { get; set; }//本次付款金额不为空，存入付款收款记录

        [DataMember(Order = 4)]
        public string OutMM { get; set; }//收发货、收付款的备注

        //以下为订单明细表数量、单价、总计的修改
        [DataMember(Order = 5)]
        public string PurQuantity { get; set; }

        [DataMember(Order = 6)]
        public string PurPrc { get; set; }

        [DataMember(Order = 7)]
        public string PurAmt { get; set; }

        [DataMember(Order = 8)]
        public string PurMM { get; set; }  

        /// <summary>
        /// 交货日期
        /// </summary>
        [DataMember(Order = 9)]
        public string PurDeliveryDate { get; set; }

        /// <summary>
        /// 确认订单中的上传附件
        /// </summary>
        [DataMember(Order = 10)]
        public List<PicResources> FileUploadData { get; set; }


    }
}
