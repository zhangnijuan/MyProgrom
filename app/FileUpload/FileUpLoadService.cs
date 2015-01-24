using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Common.Utils;
using System.IO;
using ServiceStack.Common.Web;
using System.Drawing;
using System.Data;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Common;

namespace Ndtech.FileUpload
{
    /// <summary>
    /// 上传图片服务类
    /// </summary>
    public class FileUpLoadService : Service, IPost<FileUploadRequest>, IGet<FileUploadRequest>
    {
        public object Get(FileUploadRequest request)
        {
            if (string.IsNullOrEmpty(request.RelativePath))
            {
                return new FileUploadResponse() { ResponseStatus = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus { ErrorCode = "pathIsNull", Message = "相对路径不能为空" } };
            }

            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dirPath = dirInfo.FullName + "fileuploads" + Path.DirectorySeparatorChar + request.RelativePath.MapServerPath();
            string fileName = request.NewName;
            if (request.Thumbnail != null)
            {

            }
            string filePath = dirPath + Path.DirectorySeparatorChar + fileName + "." + request.Suffix;

            if (!File.Exists(filePath))
                throw new FileNotFoundException(request.RelativePath);

            var result = new HttpResult(new FileInfo(filePath));
            return result;
        }
        /// <summary>
        /// 缩略图计划
        /// </summary>
        private Thumbnail[] ThumbnailList = new Thumbnail[] { 
            new Thumbnail{ Height=100, Width=100, Name="100_100"},
            new Thumbnail{ Height=70, Width=70, Name="70_70"},
            new Thumbnail{ Height=50, Width=50,Name="50_50"}
        };

        /// <summary>
        /// POS请求实现
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Post(FileUploadRequest request)
        {
            if (this.Request.Files.Length == 0)
                throw new FileNotFoundException("UploadError", "No such file exists");

            if (request.RelativePath == "ThrowError")
                throw new NotSupportedException("ThrowError");

            var file = this.Request.Files[0];
            if (file.ContentLength > 2097152)
            {
                throw new NotSupportedException("不允许上传超过2M的文件！");
            }
            FileUploadResponse response = new FileUploadResponse();
            string[] fileArray = file.FileName.Split('.');

            if (fileArray.Length < 2)
                throw new NotSupportedException(file.FileName);
            int r = file.FileName.LastIndexOf('.');
            fileArray[0] = file.FileName.Substring(0, r);
            fileArray[1] = file.FileName.Substring(r + 1);
            string fileName = Guid.NewGuid().ToString("N");
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string dirPath = dirInfo.FullName + "fileuploads" + Path.DirectorySeparatorChar + request.AccountID;
            string filePath = dirPath + Path.DirectorySeparatorChar + fileName + "." + fileArray[1];
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (File.Exists(filePath))
            {
                File.Move(filePath, dirPath + Path.DirectorySeparatorChar + request.NewName + "-" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "." + fileArray[1]);
                foreach (Thumbnail item in ThumbnailList)
                {
                    if (File.Exists(dirPath + Path.DirectorySeparatorChar + string.Format("{0}_{1}", fileName, item.Name) + "." + fileArray[1]))
                    {
                        File.Delete(dirPath + Path.DirectorySeparatorChar + string.Format("{0}_{1}", fileName, item.Name) + "." + fileArray[1]);
                    }
                }

            }

            if ((file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp") && !request.NoThumbnail)
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
                image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                foreach (Thumbnail item in ThumbnailList)
                {
                    MakeThumbnail(image, dirPath + Path.DirectorySeparatorChar + string.Format("{0}_{1}", fileName, item.Name) + "." + fileArray[1], item.Width, item.Height, Mode.Cut);
                }

                image.Dispose();
            }
            else
            {
                //Create只是创建没有内容
                //File.Create(filePath,1024).Close();
                file.SaveTo(filePath);
            }
            //声明返回data数据
            ReturnData rdata = new ReturnData();
            ReturnResources resources = new ReturnResources();
            resources.OriginalName = fileArray[0];
            resources.FileLength = file.ContentLength.ToString();
            resources.ContentType = file.ContentType;
            resources.NewName = fileName;
            resources.CustomerName = request.NewName;
            resources.CreatedDate = DateTime.Now;
            resources.FileUrl = string.Format("/fileuploads/{0}", request.AccountID); // dirPath + Path.DirectorySeparatorChar + string.Format("{0}.{1}", fileName, fileArray[1]);
            resources.Suffix = "." + fileArray[1];
            rdata.PicResources = resources;
            response.Data = rdata;
            response.Success = true;
            this.Response.ContentType = "application/json";
            return response;
        }



        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, Mode mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case Mode.HW://指定高宽缩放（可能变形）                
                    break;
                case Mode.W://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case Mode.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case Mode.Cut://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }

}
