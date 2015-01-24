using EtoolTech.MongoDB.Mapper;
using EtoolTech.MongoDB.Mapper.Attributes;
using MongoDB.Bson;
using MongoDB.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMongoDB
{
    class Program
    {
        static void Main(string[] args)
        {

            bool istrue = BsonDocumentManager.Instance.InsertDocument<BsonDocument>("SystemUser", "{\"m_dv\" : 1,\"ID\" : 5,\"Name\" : \"wangwu\",\"children\" : {\"cID\" : 1,\"cName\" : \"cesih2\",\"children2\" : {\"cID\" : 1,\"cName\" : \"cesih22\"}}}");

            Dictionary<string, string> WhereStrings = new Dictionary<string, string>();
            WhereStrings.Add("children.children2.cName", "%ces%");
            List<MongoDB.Bson.BsonDocument> list = BsonDocumentManager.Instance.QueryDocument("SystemUser", WhereStrings, null);
            //var c = new SystemUser { ID = 1, Name = "zhangsan", sex=0 };
            //   c.Save();
            //     c = new SystemUser { ID = 2, Name = "lisi", sex = 0 };
            //    c.Save();
            // c = new SystemUser { ID = 3, Name = "wangwu", sex = 0 };
            //c.Save();
            //var b = new SystemUser { ID = 1 };
            //b.Delete();
        }
    }
    [Serializable]
    [MongoKey(KeyFields = "ID")]
    [MongoCollectionName(Name = "Users")]
    public class SystemUser : MongoMapper
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int sex { get; set; }
    }
}
