using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService
{
  public  class Const
  {
      #region 员工信息提示
      public const string Err_UserNameIsnull = "用户名不能为空";
      public const string Err_PasswordIsnull = "密码不能为空";
      public const string Err_TelNumIsnull = "手机号码不能为空";
      public const string Err_TelNumExisting = "该手机号码已经被绑定";
      public const string Err_TelCodeIsnull = "手机验证码不能为空";
      public const string Err_TelCodeRandNum = "生成手机验证码错误";
      public const string Err_TelCode = "手机验证码不正确";
      public const string Err_EmailExisting = "该邮箱已经注册过";
      public const string Err_UserNameExisting = "该用户名已被使用，请使用其它帐号";
      public const string Err_UserNameNotExisting = "用户名不存在";
      public const string Err_PasswordIsError = "用户密码错误";
      public const string Err_TwicePwdNotSame = "两次输入的密码不一致";
      public const string Err_LoginError = "用户名或密码错误";
      public const string Err_SysNameIsNull = "员工姓名不能为空";
      public const string Err_SysCodeIsNull = "员工代码不能为空";
      public const string Err_OfficeIsNull = "公司职位不能为空";
      public const string Err_RoleIdIsNull = "员工角色不能为空";
      public const string Err_EmailIsNull = "电子邮箱不能为空";
      public const string Err_UserNameState = "用户已停用";
      public const string Err_PassWordTooLongOrShort = "密码超出了长度限制";
      public const string Err_PassWordIsBroken = "密码存在非法字符";
      public const string Err_ValudateUserNameIsNull = "找回密码的用户名不存在";
      public const string Err_CompNameIsNull = "公司名称不能为空";
      public const string Err_TelPhoneInNull = "公司电话号码不能为空";
      public const string Err_CompanyScaleIsNull = "公司规模不能为空";
      public const string Err_PurItemListIsNull = "请先添加产品";
      public const string Err_MainProductIsNull = "公司主营产品不能为空";
      public const string Err_TelNum = "登录名和绑定的手机号码不一致";
      public const string Err_CompNameIsExist = "公司名称已经存在";
      public const string Err_Flag = "Flag 参数错误";
      #endregion

      #region AnyOtherMessageHereOrAdd

      public const string Err_AppKey = "获取不到appkey";

      #endregion

      #region 询价信息提示
      public const string Err_SubjectIsNull = "询价主题不能为空";
      public const string Err_FinalDateTimeIsNull = "报价截止日期不能为空";
      public const string Err_ItemCodeIsNull = "产品代码不能为空";
      public const string Err_ItemNameIsNull = "产品名称不能为空";
      public const string Err_CategoryIDIsNull = "产品类别不能为空";
      public const string Err_AmountIsNull = "数量不能为空";
      public const string Err_UnitIDIsNull = "单位不能为空";
      public const string Err_DeliveryDateIsNull = "交货期不能为空";
      public const string Err_AddressIDIsNull = "收货地址ID不能为空";
      public const string Err_AddressNameIsNull = "收货地址不能为空";
      public const string Err_PublicTypeIsnull = "发布方式不能为空";
      public const string Err_PurItemListIsnull = "至少有一条产品数据";
      public const string Err_BlackCustomer = "客户状态为黑名单";
      public const string Err_InquiryCodeIsNull = "询价编号不能为空";
      #endregion

      #region 帐套

      public const string Err_AccountID = "获取不到帐套";

      #endregion

      #region 公司认证信息提示
      public const string Err_CompNatureIsNull = "公司性质不能为空";
      public const string Err_CompAddressIsNull = "公司地址不能为空";
      public const string Err_RegMoneyIsNull = "注册资金不能为空";
      public const string Err_LegalPersonIsNull = "法人代表不能为空";
      public const string Err_RegOfficeIsNull = "登记机关不能为空";
      public const string Err_RegTimeIsNull = "公司成立时间不能为空";
      public const string Err_AnnualTimeIsNull = "年检时间不能为空";
      public const string Err_BusnissTimeIsNull = "营业期限不能为空";
      public const string Err_BusinessScopeIsNull = "经营范围不能为空";
      public const string Err_LicenseIdIsNull = "营业执照编号不能为空";
      public const string Err_TaxIdIsNull = "税务登记证编号不能为空";
      public const string Err_OrganizationCodeIsNull = "组织机构代码不能为空";
      public const string Err_OrganizationResourcesIsNull = "组织机构代码证图片资源对象不能为空";
      public const string Err_LicenseResourcesIsNull = "营业执照照片资源对象不能为空";
      public const string Err_TaxResourcesIsNull = "税务登记证照片资源对象不能为空";
      public const string Err_OpenResourcesIsNull = "开户许可证照片资源对象不能为空";
      #endregion

      #region 企业产品

      public const string Err_SItemNameIsNull = "平台产品名称不能为空";
      public const string Err_SItemCodeIsNull = "平台产品代码不能为空";
      public const string Err_ERPItemNameIsNull = "我的产品名称不能为空";
      public const string Err_ERPItemCodeIsNull = "我的产品代码不能为空";
      public const string Err_ERPItemCodeExist = "我的产品代码已存在";
      public const string Err_Category1IsNull = "产品分类1Code和Name不能为空";
      public const string Err_Category2IsNull = "产品分类2Code和Name不能为空";
      public const string Err_Category3IsNull = "产品分类3Code和Name不能为空";
      public const string Err_AttributeIsNull = "产品属性不能为空";
      public const string Err_ERPSalPrcIsNull = "市场价不能为空";
      public const string Err_ERPDescIsNull = "详细说明不能为空";
      public const string Err_ERPIDIsNull = "产品ID不能为空";
      public const string Err_AccountIDIsNull = "帐套不能为空";
      public const string Err_ERPAddressIDIsNull = "发货地址ID不能为空";
      public const string Err_ERPAddressIsNull = "发货地址不能为空";

      public const string Err_ProvinceIsNull = "省份/直辖市不能为空";
      public const string Err_CityIsNull = "城市不能为空";
      public const string Err_DistrictIsNull = "城地区不能为空";
      public const string Err_AddressIsNull = "详细地址不能为空";
      public const string Err_QuotationIDIsNull = "报价ID不能为空";
      public const string Err_InquiryIDIsNull = "询价ID不能为空";

      #endregion

      #region 共用

      public const string Err_IDIsNull = "ID不能为空";
      public const string Err_NoInfoByID = "根据ID未找到资料";
      public const string Err_CounterPartyIsNull = "区分采购方供应方标识";
      public const string Err_StateIsNull = "更新状态不能为空";
      public const string Err_EIDIsNull = "建档人ID不能为空";
      public const string Err_CompIDIsNull = "供应企业云ID不能为空";
      public const string Err_SearchCondition = "查询条件为空";
      #endregion

      #region 订单
      public const string Err_SelectIDIsNull = "优选单ID不能为空";
      public const string Err_QuotationdidIsNull = "报价单明细ID不能为空";
      public const string Err_NoSelectInfoByID = "根据优选ID未找到资料";
      public const string Err_NoSupplyInfoByID = "根据供应商ID未找到资料";
      public const string Err_NoInquiryInfoByID = "根据询价ID未找到资料";
      public const string Err_NoQuotationInfoByID = "根据报价ID未找到资料";
      public const string Err_PurOrderIDIsNull = "订单ID不能为空";
      public const string Err_PurOrderDetailIsNull = "订单明细为空";
      public const string Err_PurOrderQtyIsNull = "采购方确认订单订单明细数量为空";
      public const string Err_PurOrderOutQtyIsNull = "供应方确认发货订单明细发货数量为空";
      public const string Err_PurOrderEidIsNull = "操作人ID为空";
      public const string Err_PurOrderECodeIsNull = "操作人编码为空";
      public const string Err_PurOrderENameIsNull = "操作人名称为空";
      #endregion

      #region 订阅提示
      public const string Err_FromDataIDIsNull = "数据来源ID不能为空";
      public const string Err_SubsciberIsNull = "订阅人信息不能为空";
      public const string Err_SubstateIsNull = "订阅取消订阅参数为空";
      public const string Err_SubtypeIsNull = "订阅类型参数为空";
      #endregion

      #region 购物车
      public const string Err_SysIDIsNull = "登录人ID不能为空";
      public const string Err_AmtIsNull = "金额不能为空";
      public const string Err_TotalAmtIsNull = "总金额不能为空";
      #endregion

      #region 评价
      public const string Err_EvaluationCodeIsNull = "评价单号不能为空";
      public const string Err_EvaluationTypeIsNull = "评价类型不能为空";
      #endregion
  }
}
