using System;
using EtoolTech.MongoDB.Mapper.Interfaces;

namespace EtoolTech.MongoDB.Mapper
{
    [Serializable]
    public abstract class MongoMapperChild : IMongoMapperChildIdeable
    {
        #region Public Properties

        public long _mid { get; set; }

        #endregion
    }
}