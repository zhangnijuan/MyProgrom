using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同修改企业产品状态接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/compitem/state/modify", HttpMethods.Post, Notes = "批量修改企业产品状态")]
    [DataContract]
    public class ModifyERPItemStatusRequest : IReturn<EnterpriseItemResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
               ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "产品ID集合(用英文逗号隔开)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 4)]
        public string IDs { get; set; }

        [ApiMember(Description = "产品状态(0草稿 1已发布 2已下架 3已删除)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 5)]
        public int State { get; set; }

        [ApiMember(Description = "修改人ID(供应产品修改时需传此参数)",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 6)]
        public long EID { get; set; }

        [ApiMember(Description = "修改人代码(供应产品修改时需传此参数)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 7)]
        public string EIDCode { get; set; }

        [ApiMember(Description = "修改人名称(供应产品修改时需传此参数)",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 8)]
        public string EIDName { get; set; }

        [ApiMember(Description = "业务类型(0不分类 1采购物品 2 供应物品)",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 9)]
        public int BizType { get; set; }

        [ApiMember(Description = "平台产品代码集合(用英文逗号隔开)-供应产品删除前检核使用",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 10)]
        public string StandardItemCodes { get; set; }

        [ApiMember(Description = "我的产品代码(用英文逗号隔开)-供应产品删除前检核使用",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 11)]
        public string ItemCodes { get; set; }

        [ApiMember(Description = "发布状态集合(用英文逗号隔开)-供应产品删除前检核使用",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public string States { get; set; }
    }
}
