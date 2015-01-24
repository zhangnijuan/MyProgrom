using Ndtech.WarningEngine;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using ServiceStack.WebHost.Endpoints;
using System.Text;

namespace Ndtech.PortalService.Warning
{
    internal class Inquiry {
        public long Count { get; set; }
        public int AccountID { get; set; }
    }
    public class InquiryOffDate : AbstractListener
    {
        public override string ListenedChannel
        {
            get
            {
                return "InquiryOffDate";
            }
        }
        protected override void Process(Info p_Info)
        {
          IDbConnectionFactory factory=  EndpointHost.AppHost.GetContainer().TryResolve<IDbConnectionFactory>();
          using (var conn = factory.OpenDbConnection())
          {
              var trans = conn.BeginTransaction(); 
              try
              {
                  
                  conn.ExecuteNonQuery(" update pur_inquiry set State = 4,state_enum='已截止'  where FinalDateTime <now()");
                  trans.Commit();
              }
              catch (Exception ex)
              {
                  trans.Rollback();
              }
              finally
              {
                  conn.Close();
              }
          }
          List<Inquiry> inquiryList = null;
          StringBuilder sb = new StringBuilder();
          using (var conn = factory.OpenDbConnection())
          {
              try
              {
                  inquiryList = conn.Query<Inquiry>("select  count(a) as count ,max(a) as AccountID from pur_inquiry where state = 1 and FinalDateTime >now() group by a");
              }
              catch (Exception ex)
              { }
              finally
              {
                  conn.Close();
              }
          }
          if (inquiryList != null && inquiryList.Count > 0)
          {
              inquiryList.ForEach(a => sb.AppendFormat("update udoc_comp set inquirynumber = {1} where a = {0};", a.AccountID, a.Count));
              if (sb.Length > 0)
              {
                  using (var conn = factory.OpenDbConnection())
                  {
                      try
                      {
                          var trans = conn.BeginTransaction();
                          conn.ExecuteNonQuery(sb.ToString());
                          trans.Commit();
                      }
                      finally {
                          conn.Close();
                      }
                  }

              }
          }
        }
    }
}
