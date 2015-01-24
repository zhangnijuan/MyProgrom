using Ndtech.PortalModel.ViewModel;
using Ndtech.PortalService.DataModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface.Validation;
using Ndtech.PortalService.Extensions;
using Ndtech.PortalService.SystemService;
using ServiceStack.OrmLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging; 
using System.IO;
using System.Web;

namespace Ndtech.PortalService.Auth
{
    public class GetWebValidateCodeService : Service, IPost<GetWebValidateCodeRequest>, IGet<GetWebValidateCodeRequest>
    {
        public IDbConnectionFactory db { set; get; }
        #region IPost<RegisteredRequest> 成员

        public object Post(GetWebValidateCodeRequest request)
        {
            GetWebValidateCodeResponse response = new GetWebValidateCodeResponse();
            string code = CreateValidateCode(4);
            Stream stream = CreateValidateGraphic(code);
           return stream;
 
        }

        
        #endregion

        #region 生成验证码

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        //C# MVC 升级版
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        public Stream CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion

        public object Get(GetWebValidateCodeRequest request)
        {
            GetWebValidateCodeResponse response = new GetWebValidateCodeResponse();
            string code = CreateValidateCode(4);
            Stream stream = CreateValidateGraphic(code);
            var session = this.GetNdtechSession();
            InsertWebCode(session.SessionID, code);
            return stream;
        }
        private void InsertWebCode(string UserName, string WebCode)
        {
            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    NdtechWebCode CodeInfo = conn.QuerySingle<NdtechWebCode>(string.Format("select * from udoc_webcode where UserName = '{0}';", UserName));
                    if (CodeInfo != null)
                    {
                        string sql = string.Format("UPDATE udoc_webcode SET WebCode='{0}' WHERE UserName='{1}'", WebCode, UserName);
                        conn.ExecuteNonQuery(sql);
                    }
                    else
                    {
                        long ID = RecordIDService.GetRecordID(1);
                        string sql = string.Format("INSERT INTO udoc_webcode (ID,UserName,WebCode) VALUES ({0},'{1}','{2}')", ID, UserName, WebCode);
                        conn.ExecuteNonQuery(sql);
                    }
                }
                finally {
                    conn.Close();
                }
            }
        }

    }
}
