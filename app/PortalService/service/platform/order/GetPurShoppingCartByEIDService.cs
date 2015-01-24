using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface.Validation;
using Ndtech.PortalService.Extensions;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;
using System.Collections;
using Ndtech.PortalModel;


namespace Ndtech.PortalService.Auth
{
    class GetPurShoppingCartByEIDValidator : AbstractValidator<GetPurShoppingCartByEIDRequest>
    {
        public override ValidationResult Validate(GetPurShoppingCartByEIDRequest instance)
        {
            ValidationResult result = new ValidationResult();

            //帐套
            if (string.IsNullOrEmpty(instance.AccountID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("ItemList", Const.Err_AccountID, "Err_AccountID"));
                return result;
            }

            //产品ID
            if (string.IsNullOrEmpty(instance.EID.ToString()))
            {
                result.Errors.Add(new ValidationFailure("EID", Const.Err_EIDIsNull, "Err_EIDIsNull"));
                return result;
            }

            return base.Validate(instance);
        }
    }

    public class GetPurShoppingCartByEIDService : Service, IPost<GetPurShoppingCartByEIDRequest>
    {
        //public IDbConnectionFactory db { set; get; }
        public IValidator<GetPurShoppingCartByEIDRequest> GetPurShoppingCartByEIDValidator { get; set; }
        //ArrayList categoryList = new ArrayList();
        //StringBuilder propertysb = new StringBuilder();
        
        
        public object Post(GetPurShoppingCartByEIDRequest request)
        {
            List<ShoppingCartItemGrop> CartItemInfo = new List<ShoppingCartItemGrop>();
            GetPurShoppingCartByEIDResponse response = new GetPurShoppingCartByEIDResponse();
            //第一步:校验前端的数据合法性
            ValidationResult result = GetPurShoppingCartByEIDValidator.Validate(request);
            if (!result.IsValid)
            {
                response.Success = false;
                response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                return response;
            }
            //第二步:获取购物车信息

            GetPurShoppingCartByEID(request, CartItemInfo, response);

            response.Data = CartItemInfo;
            return response;
        }

        #region 逻辑处理

        /// <summary>
        /// 我的购物车信息显示
        /// </summary>
        /// <param name="newMember"></param>
        /// <param name="request"></param>
        private void GetPurShoppingCartByEID(GetPurShoppingCartByEIDRequest request, List<ShoppingCartItemGrop> CartItemGrop, GetPurShoppingCartByEIDResponse response)
        {

            StringBuilder searchsql = new StringBuilder("select b.a,compID,compname,count(compID),a.createdate from pur_shopping_cart a left join udoc_comp b on b.c=a.compID  where a.a=" + request.AccountID + " and state=1 and eid='" + request.EID + "' group by compID,compname,b.a,a.createdate order by a.createdate desc");
            var list = this.Db.Query<PurShoppingCart>(string.Format(searchsql.ToString()));
            StringBuilder supplierlist = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (supplierlist.ToString().IndexOf (item.CompID)>-1)
                    {
                        continue;
                    }
                    supplierlist.Append(","+item.CompID+",");

                    ShoppingCartItemGrop ShopCartItemGrop = new ShoppingCartItemGrop();
                    ShopCartItemGrop.CompID = item.CompID;
                    ShopCartItemGrop.CompName = item.CompName;
                    ShopCartItemGrop.CompAccountID = item.AccountID;
                    //进入分组
                    StringBuilder searchstr = new StringBuilder("select * from pur_shopping_cart where compid='" + item.CompID + "' and a=" + request.AccountID + " and eid=" + request.EID + " and state=1 order by createdate desc ");
                    var CartList = this.Db.Query<PurShoppingCart>(string.Format(searchstr.ToString()));
                    List<ShoppingCartItem> ShoppingCartItem = new List<ShoppingCartItem>();
                    if (CartList != null && CartList.Count > 0)
                    {
                        foreach (var cartitem in CartList)
                        {
                            ShoppingCartItem shopcart = ToShopCartItem(cartitem);
                            StringBuilder s = new StringBuilder("select * from udoc_enterprise_attribute where 1=1 ");
                            if (!string.IsNullOrEmpty(cartitem.ItemID.ToString()))//企业物品ID
                            {
                                s.Append(" and itemid=" + cartitem.ItemID);
                            }
                            List<ItemAttribute> ItemAttribute = new List<PortalModel.ViewModel.ItemAttribute>();
                            var att = this.Db.Query<EnterpriseItemAttribute>(string.Format(s.ToString()));
                            if (att != null && att.Count > 0)
                            {
                                foreach (var attitem in att)
                                {
                                    ItemAttribute ItemAtt = new ItemAttribute();
                                    ItemAtt.PropertyName = attitem.AttributeClass;
                                    ItemAtt.PropertyValue = attitem.AttributeValue;
                                    ItemAttribute.Add(ItemAtt);
                                }
                            }
                            shopcart.PropertyList = ItemAttribute;
                            ShoppingCartItem.Add(shopcart);
                        }

                    }
                    ShopCartItemGrop.ShoppingCartItem = ShoppingCartItem;
                    CartItemGrop.Add(ShopCartItemGrop);
                    //response.Data = CartItem;                    
                }

                response.Success = true;
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoDate";
                response.ResponseStatus.Message = "购物车中无商品信息";
                response.Success = false;
            }
        }
        private ShoppingCartItem ToShopCartItem(PurShoppingCart cart)
        {
            return cart.TranslateTo<ShoppingCartItem>();
        }
        #endregion

    }

}
