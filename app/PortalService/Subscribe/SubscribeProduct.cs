using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Subscribe
{
 public   class SubscribeProduct:SubscribeAbstract
    {
      public SubscribeProduct(IDbConnectionFactory dbFactory) : base(dbFactory) { }

     /// <summary>
     /// 收藏产品
     /// </summary>
     /// <param name="p_FromDataID">产品ID</param>
     /// <param name="p_Substate">0 取消收藏 1 收藏</param>
     /// <param name="p_ToAccountID">收藏人企业标示</param>
     /// <param name="p_Subsciber">收藏人ID</param>
     /// <param name="p_SubscriberCode">收藏人编号</param>
     /// <param name="p_SubscriberName">收藏人名称</param>
     /// <returns></returns>
      public override bool ReceiveSubscribe(long p_FromDataID, int p_Substate,int p_SubType, int p_ToAccountID, long p_Subsciber, string p_SubscriberCode, string p_SubscriberName)
         {
             using (var conn = this.dbFactory.OpenDbConnection())
             {
                 try
                 {
                     EnterpriseItem item = conn.GetById<EnterpriseItem>(p_FromDataID);
                     if (null == item)
                         return false;
                     SubscribeContact contact = this.addSubscriber(new SubscribeContact()
                     {
                         FromAccountID = item.AccountID,
                         ToAccountID = p_ToAccountID,
                         Subscriber = p_Subsciber,
                         SubscriberCode = p_SubscriberCode,
                         SubscriberName = p_SubscriberName
                     });
                     SubscribeReceive receive = new SubscribeReceive() { FromAccountID = contact.FromAccountID, Subtype = 1, FromID = p_FromDataID, Substate = p_Substate, ToAccountID = contact.ToAccountID, SubscriberCode = contact.SubscriberCode, SubDateTime = DateTime.Now };

                     return this.ReceiveSubscribe(receive);
                 }
               
                 finally
                 {
                     conn.Close();
                 }
             }
           
           
         }
     
    }
}
