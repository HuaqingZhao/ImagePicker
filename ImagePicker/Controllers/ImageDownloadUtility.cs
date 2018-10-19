using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ImagePicker.Entities;

namespace ImagePicker.Controllers
{
    public class ImageDownloadUtility
    {
        private static string imageSavePath;

        static ImageDownloadUtility()
        {
            imageSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            if (!Directory.Exists(imageSavePath))
            {
                Directory.CreateDirectory(imageSavePath);
            }
        }

        public static void Download()
        {
            Task.Factory.StartNew(() =>
                {
                    using (WebClient client = new WebClient())
                    {
                        var source = client.DownloadString("http://www.qq.com");

                        var imageList = ExtractImagePath(source);
                        foreach (var image in imageList)
                        {
                            XmlUtility.Add(new Entities.Image() { ImageUrl = image });
                        }

                        XmlUtility.Save();
                    }
                });
        }

        public static IList<string> ExtractImagePath(string source)
        {
            var res = new List<string>();
            res.Add(@"https://inews.gtimg.com/newsapp_bt/0/5866633952/1000");
            return res;
        }

        public static void SaveImage(Image image)
        {
            if (!string.IsNullOrEmpty(image.ImageSavePath)) return;

            using (WebClient client = new WebClient())
            {
                var savePath = Path.Combine(imageSavePath, Guid.NewGuid().ToString() + ".jpeg");
                client.DownloadFileAsync(new Uri(image.ImageUrl), savePath,null);
                image.ImageStatus = "2";
                image.ImageSavePath = savePath;
                XmlUtility.UpdateSaved(image);
            }
        }
    }
}
