using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.service.management.myPurchase
{
    /// <summary>
    /// 获取所有下级员工的id集合
    /// </summary>
    public class GetLowerEmployeeListID
    {
        public GetLowerEmployeeListID(IDbConnectionFactory p_dbFactory)
        {
            this.dbFactory = p_dbFactory;
        }
        private IDbConnectionFactory dbFactory { get; set; }
        public  List<long> GetListBuyId(long id)
        {
            using (var conn = this.dbFactory.OpenDbConnection())
            {
                try
                { 
                    //获取该员工的下级员工
                    return conn.Where<LowerEmployee>(x => x.StaffId == id).Select(s => s.EId).ToList();
                }

                finally
                {
                    conn.Close();
                }
            }
           
        }
    }
}
