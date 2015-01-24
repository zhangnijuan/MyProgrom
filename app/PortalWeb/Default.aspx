<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="jquery-2.1.1.js"></script>
     <script type="text/javascript" src="jquery.cookie.js"></script>
    <script type="text/javascript" src="ajaxfileupload.js"></script>

</head>
<body>
    <div Visible="false" id="phAuthenticated" runat="server" enableviewstate=false style="color:green;">Authenticated.</div>
    <form id="form1" runat="server">
    <div>
        User: <asp:TextBox runat="server" ID="tbUser" CssClass="userName" Text="sssss" /> (or "user") <br />
        Password: <asp:TextBox runat="server" ID="tbPassword" CssClass="password" Text="ssssss" /> <br />

        
    </div>
    </form>
     <button onclick="getList()">根据ID获取员工</button> 
        <button onclick="ajaxLogin()">Ajax Login</button> 
    <button onclick="telcode()">短信验证码</button> 

     <button onclick="ajaxPurInquiry()">发布询价</button>
    <button onclick="ajaxUpdatePurInquiryState()">更新询价状态</button>
    <button onclick="ajaxGetPurInquiry()">查看询价</button>
    <button onclick="ajaxSalQuotation()">发布报价</button>
    <button onclick="ajaxSaveSalQuotation()">保存报价</button>
       <p><input type="file" id="file1" name="file" /></p>
   <button onclick="ajaxFileUpload()">图片上传</button> 
    <button onclick="ajaxFileUpload1('UdocItem')">标准产品上传</button> 
    <button onclick="ajaxFileUpload1('UdocEPItem')">企业产品上传</button> 
    <p><img id="img1" alt="上传成功啦" src="" /></p>

    <script>
        function ajaxFileUpload() {
            $.ajaxFileUpload
            (//E:\Procurement\program\app\PortalWeb\fileUpload\staff\aa6323966f554f9cb827841ac6fbe581_ico.jpg
                {
                    url: '<%=ResolveUrl("~/api/11/22/fileuploads/714")%>', //用于文件上传的服务器端请求地址
                    secureuri: false, //是否需要安全协议，一般设置为false
                    fileElementId: 'file1', //文件上传域的ID
                    dataType: 'xml', //返回值类型 一般设置为json
                    data: '{"CustomerName":"tupina","CustomerId":"jks"}',
                    success: function (data, status)  //服务器成功响应处理函数
                    {
                       
                        <%--$("#img1").attr("src", '<%=ResolveUrl("~/api/111/222/file/staff/jks/jpg")%>');--%>
                        $("#img1").attr("src", data.fiePath);
                        if (typeof (data.error) != 'undefined') {
                            if (data.error != '') {
                                alert(data.error);
                            } else {
                                alert(data.msg);
                            }
                        }
                    },
                    error: function (data, status, e)//服务器响应失败处理函数
                    {
                        
                        alert(e);
                    }
                }
            )
            return false;
        }
        function ajaxFileUpload1(str) {
            $.ajaxFileUpload
            (
                {
                    url: '<%=ResolveUrl("~/api/111/222/Import/")%>' + str, //用于文件上传的服务器端请求地址
                    secureuri: false, //是否需要安全协议，一般设置为false
                    fileElementId: 'file1', //文件上传域的ID
                    dataType: 'xml', //返回值类型 一般设置为json
                    data: '{"AccountID":"750"}',
                    success: function (data, status)  //服务器成功响应处理函数
                    {

                        <%--$("#img1").attr("src", '<%=ResolveUrl("~/api/111/222/file/staff/1/jpg")%>');
                        if (typeof (data.error) != 'undefined') {
                            if (data.error != '') {
                                alert(data.error);
                            } else {
                                alert(data.msg);
                            }
                        }--%>
                    },
                    error: function (data, status, e)//服务器响应失败处理函数
                    {

                        alert(e);
                    }
                }
            )
                    return false;
                }
        
        function getList() {
                    InvokeService('<%=ResolveUrl("~/api/122/123/employee/list/1")%>',
                        '{ "TelNum": "13522292630" }',
                        function (data, textStatus, jqXHR) {
                            //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                            if (data.Success)
                                alert(data.TelCode);
                            else {
                                alert("用户名(UserName)=" + data.UserName + "；电话(TelNum)=" + data.TelNum + "；邮箱(Email)=" + data.Email + "；公司名称(CompName)=" + data.CompName + "；企业号(Corpnum)=" + data.Corpnum + "；角色(RoleID)=" + data.RoleID);
                            }
                        }
                    );

                    return false;
                }
        function telcode() {
            var txpro = document.getElementById('<%=tbUser.ClientID %>').value;
                    var txpwd = document.getElementById('<%=tbPassword.ClientID %>').value;
                    InvokeService('<%=ResolveUrl("~/api/telCode")%>',
                        '{ "TelNum": "13522292630" }',
                        function (data, textStatus, jqXHR) {
                            //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                            if (data.Success)
                                alert(data.TelCode);
                            else {
                                alert("用户名(UserName)=" + data.UserName + "；电话(TelNum)=" + data.TelNum + "；邮箱(Email)=" + data.Email + "；公司名称(CompName)=" + data.CompName + "；企业号(Corpnum)=" + data.Corpnum + "；角色(RoleID)=" + data.RoleID);
                            }
                        }
                    );

                    return false;
                }
        function ajaxLogin() {
            var txpro = document.getElementById('<%=tbUser.ClientID %>').value; 
            var txpwd = document.getElementById('<%=tbPassword.ClientID %>').value; 
            InvokeService('<%=ResolveUrl("~/api/login")%>',
                '{ "UserName": "' + txpro + '","Password": "' + txpwd + '","Provide":"User" }',
                function (data, textStatus, jqXHR) {
                    //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                    if (data.ResponseStatus.Message != undefined)
                        alert(data.ResponseStatus.Message);
                    else {
                        alert("用户名(UserName)=" + data.UserName + "；电话(TelNum)=" + data.TelNum + "；邮箱(Email)=" + data.Email + "；公司名称(CompName)=" + data.CompName + "；企业号(Corpnum)=" + data.Corpnum + "；角色(RoleID)=" + data.RoleID + "；校验方式(Secretkey)=" + data.Secretkey + "；客户端类型(AppKey)=" + data.AppKey);
                    }
                }
            );

            return false;
        }

        function ajaxPurInquiry() {
              InvokeService('<%=ResolveUrl("~/api/a/b/23/inquiry/new")%>',
                  '{"RoleID":"1","Role_Enum":"S001","Eid":"157242822125703","EidCode":"S001","EidName":"100205","Subject":"询价主题","PicResources":[{"OriginalName":"询价附件1","NewName":"询价附件1","Suffix":"aspx","FileLength":3},{"OriginalName":"附件2","NewName":"附件2","Suffix":"aspx","FileLength":9}],"FinalDateTime":"2014-12-08","PayType":"付款方","InvoiceType":"发票类型","FreightTypeCode":"1","AddressCode":"2","MM":"备注","SupplierList":[{"CompId":27,"CompCode":"供应","AccountID":"27","CompName":"供应","SLinkman":"String","SMobile":"String","SEmailInfo":"String","SFixedLine":"String","SFax":"String"}],"Linkman":"联系人","Mobile":"手机","EmailInfo":"邮箱","FixedLine":"固定电话","Fax":"传真","AnonymousCode":"匿名代码","AllPublic":"1","InvitePublic":"2","Accountid":3,"PurItemList":[{"PicResources":[{"OriginalName":"询价子附件1","NewName":"子附件1","Suffix":"aspx","FileLength":3},{"OriginalName":"子附件2","NewName":"子附件2","Suffix":"aspx","FileLength":9}],"Amount":0,"DeliveryDate":"2014-12-08","MM":"描述","ItemID":2,"ItemCode":"产品代码","ItemName":"产品名称","UnitID":"8","UnitCode":"单位代码","UnitName":"单位名称","CategoryID":9,"CategoryCode":"产品类别代码","CategoryName":"产品类别名称","PropertyID":7,"PropertyCode":"产品属性代码","PropertyName":"产品属性名称","CategoryMID":"1000000000000200011","StandardItemCode":"1100000000000","StandardItemName":"sdaf","PropertyList":[{"PropertyName":"cpu","PropertyValue":"i7"},{"PropertyName":"内存","PropertyValue":"200"}]},{"Amount":1,"DeliveryDate":"2014-12-08","MM":"描述2","ItemID":0,"ItemCode":"产品代码","ItemName":"产品名称","UnitID":2,"UnitCode":"单位代码2","UnitName":"单位名称","CategoryID":0,"CategoryCode":"产品类别代码","CategoryName":"产品类别名称","PropertyID":0,"PropertyCode":"产品属性代码","PropertyName":"产品属性名称","CategoryMID":"1100000000000111005"}],"AppKey":"String","Secretkey":"String","Provide":"User"}',
                  function (data, textStatus, jqXHR) {
                      //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                      if (data.ResponseStatus.Message != undefined)
                          alert(data.ResponseStatus.Message);
                      else {
                          alert(data.Success);
                      }
                  }
              );

              return false;
        }

        function ajaxSalQuotation() {
            InvokeService('<%=ResolveUrl("~/api/a/b/23/quotation/draft/new")%>',
                '{"InquiryID":"157351568910341","Subject":"询价主题","InquiryCode":"33","FinalDateTime":"2014-12-08","PayType":"付款方","InvoiceType":"发票类型","FreightTypeCode":"1","AddressCode":"2","MM":"备注","SupplierList":[{"CompId":27,"CompCode":"供应","AccountID":"27","CompName":"供应","SLinkman":"String","SMobile":"String","SEmailInfo":"String","SFixedLine":"String","SFax":"String"},{"CompId":28,"CompCode":"供应2","AccountID":"28","CompName":"供应2","SLinkman":"String","SMobile":"String","SEmailInfo":"String","SFixedLine":"String","SFax":"String"}],"Linkman":"联系人","Mobile":"手机","EmailInfo":"邮箱","FixedLine":"固定电话","Fax":"传真","AnonymousCode":"匿名代码","AllPublic":"1","InvitePublic":"2","Accountid":3,"PurItemList":[{"InquiryCode":"1","PicResources":[{"OriginalName":"子附件1","NewName":"子附件1","Suffix":"aspx","FileLength":3},{"OriginalName":"子附件2","NewName":"子附件2","Suffix":"aspx","FileLength":9}],"Amount":0,"DeliveryDate":"2014-12-08","MM":"描述","ItemID":2,"ItemCode":"产品代码","ItemName":"产品名称","UnitID":"8","UnitCode":"单位代码","UnitName":"单位名称","CategoryID":9,"CategoryCode":"产品类别代码","CategoryName":"产品类别名称","PropertyID":7,"PropertyCode":"产品属性代码","PropertyName":"产品属性名称","CategoryMID":"1100000000000111005","StandardItemCode":"1100000000000","StandardItemName":"sdaf","PropertyList":[{"PropertyName":"cpu","PropertyValue":"i7"},{"PropertyName":"内存","PropertyValue":"200"}]},{"Amount":1,"DeliveryDate":"2014-12-08","MM":"描述2","ItemID":0,"ItemCode":"产品代码","ItemName":"产品名称","UnitID":2,"UnitCode":"单位代码2","UnitName":"单位名称","CategoryID":0,"CategoryCode":"产品类别代码","CategoryName":"产品类别名称","PropertyID":0,"PropertyCode":"产品属性代码","PropertyName":"产品属性名称","CategoryMID":"1100000000000111005"}],"AppKey":"String","Secretkey":"String","Provide":"User"}',
                function (data, textStatus, jqXHR) {
                    //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                    if (data.ResponseStatus.Message != undefined)
                        alert(data.ResponseStatus.Message);
                    else {
                        alert(data.Success);
                    }
                }
            );
             
            return false;
        }

        function ajaxSaveSalQuotation() {
            InvokeService('<%=ResolveUrl("~/api/a/b/23/quotation/new")%>',
                  '{"SendAddressID":"1","CreateTime":"2014-12-18","SCode":"2014-12-18 下0001","InquiryID":"157351568910341","Eid":"157242822125703","EidCode":"S001","EidName":"100205","SFinalDateTime":"2014-12-1","ID":"157384560590081","TotalAmt":"3","QuoteExplain":"说明","SCompanyName":"报价企业","SLinkman":"联系人","SMobile":"手机","SFixedLine":"固定电话","SFax":"传真","SAddress":"联系地址","PicResources":[{"ID":"157382462202241","OriginalName":"报价附件1","NewName":"报价附件1","Suffix":"aspx","FileLength":3}],"ItemList":[{"ID":"157383316870658","Price":"2","Remark":"备注","Amount":"9",{"ID":"157383316904325","Quantity":"6","Price":"2","Remark":"备注2","Amount":"29"}],"AppKey":"String","Secretkey":"String","Provide":"User"}',
                  function (data, textStatus, jqXHR) {
                      //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                      if (data.ResponseStatus.Message != undefined)
                          alert(data.ResponseStatus.Message);
                      else {
                          alert(data.Success);
                      }
                  }
              );

              return false;
        }

        function ajaxGetPurInquiry() {
            InvokeService('<%=ResolveUrl("~/api/a/b/purchase/list/157293699947265")%>',
                        '{"Provide":"User","InquiryCode":"157293699947265"}',
                        function (data, textStatus, jqXHR) {
                            //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                            if (data.ResponseStatus.Message != undefined)
                                alert(data.ResponseStatus.Message);
                            else {
                                alert(data);
                            }
                        }
                    );

                    return false;
                }

        function ajaxUpdatePurInquiryState() {
            InvokeService('<%=ResolveUrl("~/api/a/b/purchase/updatestate")%>',
                          '{"Provide":"User","State":2,"State_Enum":"关闭","InquiryCode":"157293699947265"}',
                          function (data, textStatus, jqXHR) {
                              //$.cookie('ss-id', data.SessionId, {expires: 7, path: '/', domain: 'jquery.com', secure: true}); // 存储一个带7天期限的 cookie 
                              if (data.ResponseStatus.Message != undefined)
                                  alert(data.ResponseStatus.Message);
                              else {
                                  alert(data.Success);
                              }
                          }
                      );

              return false;
        }

        function helloService() {
            InvokeService('<%=ResolveUrl("~/api/hello")%>',
                { name: userName = $('.userName').val(), SessionId: $.cookie('ss-id') },
                function (data, textStatus, jqXHR) {
                    alert(data.Result);
                }
            );
        }

        function InvokeService(url, data, success) {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                cookie: $.cookie('ss-id'),
                dataType: "json",
                data: data,
                url: url,
                success: success,
                error: function (xhr, textStatus, errorThrown) {
                    var data = $.parseJSON(xhr.responseText);
                    if (data === null)
                        alert(textStatus + " HttpCode:" + xhr.status);
                    else
                        alert(data.message);
                }
            });
        }
    </script>
</body>
</html>
