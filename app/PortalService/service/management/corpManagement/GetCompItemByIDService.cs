using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;//与lambda表达式有关
using ServiceStack.Common;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;//与转化有关

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 根据ID获取企业产品详情Service
    /// </summary>
    public class GetCompItemByIDService : Service, IGet<GetCompItemByIDRequest>
    {
        public object Get(GetCompItemByIDRequest request)
        {
            GetCompItemByIDResponse response = new GetCompItemByIDResponse();

            if (request.ID > 0)
            {
                //根据ID获取企业产品信息，平台供应产品详情与企业产品详情共用 so去除帐套条件
                var EpItem = this.Db.FirstOrDefault<EnterpriseItem>(n => n.ID == request.ID);

                if (EpItem != null)
                {
                    //根据ID值获取属性信息
                    var EpItemAttribute = this.Db.Where<EnterpriseItemAttribute>(n => n.ItemID == EpItem.ID && n.AccountID == EpItem.AccountID);

                    //根据ID值获取价格
                    var EpItemPrice = this.Db.Where<EnterpriseItemPrice>(n => n.ItemID == EpItem.ID && n.AccountID == EpItem.AccountID);

                    //企业产品图片资源
                    var PicResources = this.Db.Where<Resources>(n => n.DocumentID == EpItem.ID && n.AccountID == EpItem.AccountID);

                    //DataModel 转换成ViewModel
                    response.Data = ToEpItem(EpItem);
                    response.Data.AccountId = EpItem.AccountID;

                    List<AttributeList> listModel = new List<AttributeList>();
                    foreach (var item in EpItemAttribute)
                    {
                        listModel.Add(ToEpItemAttribute(item));
                    }

                    List<PriceList> listModelPrice = new List<PriceList>();
                    foreach (var itemP in EpItemPrice)
                    {
                        listModelPrice.Add(ToEpItemPrice(itemP));
                    }

                    List<ItemCertificationInfo> certificationInfo = new List<ItemCertificationInfo>();

                    if (EpItem.BizType == 2)
                    {
                        #region 产品质量认证logo查询

                        //查询该产品认证通过和待审核的信息
                        string cerSql = string.Format(@"select m.ca, m.certificationname, 
                                                        case when d.results is null then -99 else d.results end as results, c.colorpicid, c.graypicid,
                                                        case when d.results = 1 and m.state = 1 then colorpicid when d.results != 0 then graypicid else -1 end as picid, 
                                                        d.a, d.i, d.id as did, m.state
                                                        from udoc_certification_appdetail as d 
                                                        left join udoc_certification_application as m on m.id = d.mid
                                                        left join udoc_certification as c on c.a = m.ca
                                                        where d.results != 0 and d.i = {0};", request.ID);
                        certificationInfo = this.Db.Query<ItemCertificationInfo>(cerSql);
                        if (certificationInfo.Count > 0)
                        {
                            Resources cerResource = null;
                            foreach (var cerInfo in certificationInfo)
                            {
                                //循环取logo图片=>认证公司的colorpicid or graypicid = Resources.id
                                cerResource = this.Db.FirstOrDefault<Resources>(x => x.Id == cerInfo.CertificationPicID);
                                if (cerResource != null)
                                {
                                    cerInfo.CertificationResources = ToResources(cerResource, cerResource.AccountID);
                                }
                            }
                        }

                        #endregion
                    }
                    
                    List<Ndtech.PortalModel.ReturnPicResources> ListDatePic = new List<Ndtech.PortalModel.ReturnPicResources>();
                    foreach (var itemPic in PicResources)
                    {
                        ListDatePic.Add(ToResources(itemPic, EpItem.AccountID));
                    }

                    //属性
                    response.Data.ItemAttributeViewList = listModel;
                    response.Data.DataPrice = listModelPrice;

                    response.Data.ItemCertificationInfo = certificationInfo;
                    response.Data.PicResourcesList = ListDatePic;
                    response.Success = true;
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                    response.ResponseStatus.Message = Const.Err_NoInfoByID;
                    return response;
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_IDIsNull";
                response.ResponseStatus.Message = "No ID Parameter";
                return response;
            }

            return response;
        }
        private CompItemList ToEpItem(EnterpriseItem EpItem)
        {
            return EpItem.TranslateTo<CompItemList>();
        }
        private AttributeList ToEpItemAttribute(EnterpriseItemAttribute EpItemAttribute)
        {
            return EpItemAttribute.TranslateTo<AttributeList>();
        }
        private PriceList ToEpItemPrice(EnterpriseItemPrice EpItemPrice)
        {
            return EpItemPrice.TranslateTo<PriceList>();
        }
        
        private Ndtech.PortalModel.ReturnPicResources ToResources(Resources res, int accountID)
        {
            var result = res.TranslateTo<Ndtech.PortalModel.ReturnPicResources>();
            result.FileUrl = string.Format("/fileuploads/{0}", accountID);
            return result;
        }
    }
}
