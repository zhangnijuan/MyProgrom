using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Subscribe
{
    public abstract class SubscribeAbstract : Isubscribe
    {
        protected readonly IDbConnectionFactory dbFactory;
        public SubscribeAbstract(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public SubscribeContact addSubscriber(DataModel.SubscribeContact p_SubscribeContact)
        {
            using (var conn = this.dbFactory.OpenDbConnection())
            {
                try
                {
                    SubscribeContact contact = conn.Where<SubscribeContact>(x =>
                     x.FromAccountID == p_SubscribeContact.FromAccountID && x.ToAccountID == p_SubscribeContact.ToAccountID && x.Subscriber == p_SubscribeContact.Subscriber).FirstOrDefault<SubscribeContact>();
                    if (contact == null)
                    {
                        //this.dbFactory.Run(db => db.Insert<SubscribeContact>(p_SubscribeContact));


                        long id = conn.InsertParam<SubscribeContact>(p_SubscribeContact, selectIdentity: true);

                        return conn.GetById<SubscribeContact>(id);
                    }
                    else
                    {

                        return contact;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
           
        }

        public SubscribeFilter addSubscribeFilter(DataModel.SubscribeFilter p_SubscribeFilter)
        {
            using (var conn=this.dbFactory.OpenDbConnection())
            {
                try
                {
                    long id = conn.InsertParam<SubscribeFilter>(p_SubscribeFilter, selectIdentity: true);
                    return conn.GetById<SubscribeFilter>(id);
                }
               
                finally
                {
                    conn.Close();
                }
            }
           
        }

        public bool ReceiveSubscribe(SubscribeReceive p_SubscribeReceive)
        {

            using (var conn = this.dbFactory.OpenDbConnection())
            {
                try
                {
                    SubscribeReceive receive = conn.Where<SubscribeReceive>(x => x.FromID == p_SubscribeReceive.FromID &&
               x.FromAccountID == p_SubscribeReceive.FromAccountID && x.ToAccountID == p_SubscribeReceive.ToAccountID && x.SubscriberCode == p_SubscribeReceive.SubscriberCode && x.Subtype == p_SubscribeReceive.Subtype).FirstOrDefault<SubscribeReceive>();
                    if (null != receive)
                    {
                        if (receive.Substate == 1 && p_SubscribeReceive.Substate == 1)
                        {
                            return false;
                        }
                        receive.Substate = p_SubscribeReceive.Substate;
                        receive.SubDateTime = DateTime.Now;
                        conn.Save<SubscribeReceive>(receive);
                        return true;
                    }

                    conn.Insert<SubscribeReceive>(p_SubscribeReceive);
                    return true;
                }
            
                finally
                {
                    conn.Close();
                }
            }
          
        }


        public virtual bool ReceiveSubscribe(long p_FromDataID, int p_Substate, int p_SubType, int p_ToAccountID, long p_Subsciber, string p_SubscriberCode, string p_SubscriberName)
        {
            return false;
        }
    }

}
