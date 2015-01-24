using Ndtech.PortalService.Auth;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ndtech.PortalService.Extensions
{
 public static   class NdtechServiceExtensions
    {
         public static void SaveSession(this IServiceBase service, INdtechAuthSession session, TimeSpan? expiresIn = null)
         {
             if (service == null) return;

             service.RequestContext.Get<IHttpRequest>().SaveSession(session, expiresIn);
         }
         public static void SaveSession(this IHttpRequest httpReq, INdtechAuthSession session, TimeSpan? expiresIn = null)
         {
             if (httpReq == null) return;

             using (var cache = httpReq.GetCacheClient())
             {
                 var sessionKey = SessionFeature.GetSessionKey(httpReq.GetSessionId());
                 cache.CacheSet(sessionKey, session, expiresIn ?? SessionFeature.DefaultSessionExpiry);
             }

             httpReq.Items[RequestItemsSessionKey] = session;
         }
         public static INdtechAuthSession GetNdtechSession(this IServiceBase service, bool reload = false)
         {
             return service.RequestContext.Get<IHttpRequest>().GetNdtechSession(reload);
         }
        public static INdtechAuthSession GetNdtechSession(this Service service, bool reload = false)
        {
            var req = service.RequestContext.Get<IHttpRequest>();
            if (req.GetSessionId() == null)
                service.RequestContext.Get<IHttpResponse>().CreateSessionIds(req);
            return req.GetNdtechSession(reload);
        }

        public const string RequestItemsSessionKey = "__session";
        public static INdtechAuthSession GetNdtechSession(this IHttpRequest httpReq, bool reload = false)
        {
            if (httpReq == null) return null;

            object oSession = null;
            if (!reload)
                httpReq.Items.TryGetValue(RequestItemsSessionKey, out oSession);

            if (oSession != null)
                return (INdtechAuthSession)oSession;

            using (var cache = httpReq.GetCacheClient())
            {
                var sessionId = httpReq.GetSessionId();
                var session = cache.Get<INdtechAuthSession>(SessionFeature.GetSessionKey(sessionId));
                if (session == null)
                {
                    session = LoginService.CurrentSessionFactory();
                    session.SessionID = sessionId;
                    session.CreatedAt = session.LastModified = DateTime.Now;
                    session.OnCreated(httpReq);
                }

                if (httpReq.Items.ContainsKey(RequestItemsSessionKey))
                    httpReq.Items.Remove(RequestItemsSessionKey);

                httpReq.Items.Add(RequestItemsSessionKey, session);
                return session;
            }
        }
    }

 public class HttpUtility
 {
     /// <summary>
     /// 发送请求
     /// </summary>
     /// <param name="url">Url地址</param>
     /// <param name="data">数据</param>
     public static string SendHttpRequest(string url, string data)
     {
         return SendPostHttpRequest(url, "application/x-www-form-urlencoded", data);
     }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="url"></param>
     /// <returns></returns>
     public static string GetData(string url)
     {
         return SendGetHttpRequest(url, "application/x-www-form-urlencoded");
     }

     /// <summary>
     /// 发送请求
     /// </summary>
     /// <param name="url">Url地址</param>
     /// <param name="method">方法（post或get）</param>
     /// <param name="method">数据类型</param>
     /// <param name="requestData">数据</param>
     public static string SendPostHttpRequest(string url, string contentType, string requestData)
     {
         WebRequest request = (WebRequest)HttpWebRequest.Create(url);
         request.Method = "POST";
         byte[] postBytes = null;
         request.ContentType = contentType;
         postBytes = Encoding.UTF8.GetBytes(requestData);
         request.ContentLength = postBytes.Length;
         using (Stream outstream = request.GetRequestStream())
         {
             outstream.Write(postBytes, 0, postBytes.Length);
         }
         string result = string.Empty;
         using (WebResponse response = request.GetResponse())
         {
             if (response != null)
             {
                 using (Stream stream = response.GetResponseStream())
                 {
                     using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                     {
                         result = reader.ReadToEnd();
                     }
                 }

             }
         }
         return result;
     }

     /// <summary>
     /// 发送请求
     /// </summary>
     /// <param name="url">Url地址</param>
     /// <param name="method">方法（post或get）</param>
     /// <param name="method">数据类型</param>
     /// <param name="requestData">数据</param>
     public static string SendGetHttpRequest(string url, string contentType)
     {
         WebRequest request = (WebRequest)HttpWebRequest.Create(url);
         request.Method = "GET";
         request.ContentType = contentType;
         string result = string.Empty;
         using (WebResponse response = request.GetResponse())
         {
             if (response != null)
             {
                 using (Stream stream = response.GetResponseStream())
                 {
                     using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                     {
                         result = reader.ReadToEnd();
                     }
                 }
             }
         }
         return result;
     }
 }
}
