using System.Xml.Serialization;

namespace EtoolTech.MongoDB.Mapper.Interfaces
{
    public interface IMongoMapperChildIdeable
    {
        #region Public Properties

        [XmlIgnore]
        long _mid { get; set; }

        #endregion
    }
}