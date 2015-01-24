using EtoolTech.MongoDB.Mapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper
{
    public class BsonDocumentManager
    {
        static readonly object islock = new object();
        private static BsonDocumentManager instance;
        public static BsonDocumentManager Instance
        {
            get
            {
                lock (islock)
                {
                    if (instance == null)
                    {
                        instance = new BsonDocumentManager();
                    }
                    return instance;
                }
            }
        }
        private BsonDocumentManager()
        { }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool InsertDocument<T>(string collectionName, string json)
        {
            //var document = BsonSerializer.Deserialize<BsonDocument>("{\"m_dv\" : 1,\"ID\" : 5,\"Name\" : \"wangwu\",\"children\" : {\"cID\" : 1,\"cName\" : \"cesih2\",\"children2\" : {\"cID\" : 1,\"cName\" : \"cesih22\"}}}");
            var document = BsonSerializer.Deserialize<BsonDocument>(json);
            WriteConcernResult result = CollectionsManager.GetCollection(collectionName).Insert<BsonDocument>(document);
            if (result.ErrorMessage == null)
            {
                return true;
            }
            return false;
        }
         /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <param name="WhereStrings"></param>
        /// <returns></returns>
        public List<BsonDocument> QueryDocument(string collectionName, Dictionary<string, string> WhereStrings, PagerInfo pagerInfo)
        {
            IMongoQuery query = null;
            List<BsonDocument> result = new List<BsonDocument>();
            if (WhereStrings.Count > 0)
            {
                query = Query.And(WhereStrings.Select(keyValue => MongoQuery.Eq(keyValue.Key, keyValue.Value)).ToArray());
            }
            MongoCursor<BsonDocument> myCursor = CollectionsManager.GetCollection(collectionName).FindAs<BsonDocument>(query);
            if (pagerInfo != null)
            {
                foreach (BsonDocument entity in myCursor.SetSkip((pagerInfo.Page - 1) * pagerInfo.PageSize).SetLimit(pagerInfo.PageSize))
                {
                    result.Add(entity);
                }
            }
            else
            {
                foreach (BsonDocument entity in myCursor)
                {
                    result.Add(entity);
                }
            }
            return result;
        }
    }
}
