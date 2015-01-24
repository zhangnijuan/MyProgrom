using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Ndtech.PortalModel.ViewModel
{
    [Api("恩维协同生成订单前更新优选说明接口")]
    [Route("/{AppKey}/{Secretkey}/{AccountID}/optimization/modify", HttpMethods.Post, Notes = "更新优选说明并调用生成订单接口")]
    [DataContract]
    public class ModifyPurSelectRequest : IReturn<SavePurSelectResponse>
    {
        [ApiMember(Description = "请求权限账户，成功登陆后可以获取",
               ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 1)]
        public string AppKey { get; set; }

        [ApiMember(Description = "请求权限密码，成功登陆后可以获取",
                    ParameterType = "path", DataType = "string", IsRequired = true)]
        [DataMember(Order = 2)]
        public string Secretkey { get; set; }

        [ApiMember(Description = "帐套ID",
                  ParameterType = "json", DataType = "int", IsRequired = true)]
        [DataMember(Order = 3)]
        public int AccountID { get; set; }

        [ApiMember(Description = "询价主档ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 4)]
        public long InquiryID { get; set; }

        [ApiMember(Description = "优选主档ID",
                  ParameterType = "json", DataType = "long", IsRequired = true)]
        [DataMember(Order = 5)]
        public long PurSelectID { get; set; }

        [ApiMember(Description = "优选主档说明->table.mm",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 6)]
        public string PurSelectMM { get; set; }

        [DataMember(Order = 7)]
        [ApiMember(Description = "建档人ID",
            ParameterType = "json", DataType = "long", IsRequired = true)]
        public long Eid { set; get; }

        [DataMember(Order = 8)]
        [ApiMember(Description = "建档人编码",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidCode { set; get; }

        [DataMember(Order = 9)]
        [ApiMember(Description = "建档人名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string EidName { set; get; }

        [DataMember(Order = 10)]
        [ApiMember(Description = "角色权限ID",
            ParameterType = "json", DataType = "int", IsRequired = true)]
        public int RoleID { get; set; }

        [DataMember(Order = 11)]
        [ApiMember(Description = "用户权限名称",
            ParameterType = "json", DataType = "string", IsRequired = true)]
        public string Role_Enum { get; set; }

        [ApiMember(Description = "明细优选说明->存入询价单明细档",
                  ParameterType = "json", DataType = "string", IsRequired = true)]
        [DataMember(Order = 12)]
        public List<PurInquiryDetailRemarks> DetailRemarks { get; set; }
    }

    [DataContract]
    public class PurInquiryDetailRemarks
    {
        /// <summary>
        /// 询价明细id
        /// </summary>
        [DataMember(Order = 1)]
        public long InquiryDID { get; set; }

        /// <summary>
        /// 询价明细优选说明
        /// </summary>
        [DataMember(Order = 2)]
        public string SelectMM { get; set; }
    }
}
