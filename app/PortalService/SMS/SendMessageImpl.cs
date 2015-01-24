using Ndtech.SMSService;
using ServiceStack.Logging;
using ServiceStack.Plugins.ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.SMS
{
    public   class SendMessageImpl:ISendMessage
    {
        private static ILog log = LogManager.GetLogger(typeof(SendMessageImpl));
        public string URL { set; get; }
        public SendMessageImpl(string p_Url)
        {
            this.URL = p_Url;
        }
        public string SendSms(string p_PhoneNumber, string p_Content)
        {
            try
            {

                var client = new ProtoBufServiceClient(this.URL);
                client.HttpMethod = "GET";

                return response;
            }
            catch (Exception ex)
            {
                //log.Debug("sendmessage", ex);
                Write(ex.ToString());
            }
            return string.Empty;
        }
        private void Write(string err)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dir = dirInfo.FullName;
            System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error" + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(err);
            writer.Flush();
            file.Close();
        }


        public string Regist()
        {
            //string url = "http://sdkhttp.eucp.b2m.cn/sdkproxy/regist.action?cdkey=3SDK-EMY-0130-JIVRN&password=966365";
            string url = "http://wwww.baidu.com";
            try
            {
               return  HttpUtility.GetData(url);
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }
            return string.Empty;
        }
    }
}
