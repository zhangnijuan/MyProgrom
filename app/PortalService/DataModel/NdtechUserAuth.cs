using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Text;
using ServiceStack.ServiceHost;

namespace Ndtech.PortalService.DataModel
{
    [Alias("sys_staff")]
    public class NdtechUserAuth 
    {

        [PrimaryKey]
        public long ID { get; set; }
        [Alias("n")]
        [StringLengthAttribute(64)]
  
        public string UserName { get; set; }
        [Alias("c")]
        [StringLengthAttribute(32)]
        public string UserCode { get; set; }
        [Alias("pwd")]
        [StringLengthAttribute(128)]

        public string PassWord { get; set; }

        [Alias("salt")]
        [StringLengthAttribute(128)]
        public string Salt { get; set; }
        public  Dictionary<string, string> Meta { get; set; }
        public virtual T Get<T>()
        {
            string str = null;
            if (Meta != null) Meta.TryGetValue(typeof(T).Name, out str);
            return str == null ? default(T) : TypeSerializer.DeserializeFromString<T>(str);
        }

        public virtual void Set<T>(T value)
        {
            if (Meta == null) Meta = new Dictionary<string, string>();
            Meta[typeof(T).Name] = TypeSerializer.SerializeToString(value);
        }

    }
}
