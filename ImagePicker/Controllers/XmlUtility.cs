using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ImagePicker.Entities;

namespace ImagePicker.Controllers
{
    public class XmlUtility
    {
        private static XmlDocument Doc;
        private static string path;

        static XmlUtility()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper() == "devenv".ToUpper()) return;

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images.xml");
            Doc = new XmlDocument();
            Doc.Load(path);
        }

        public static void Save()
        {
            Doc.Save(path);
        }

        public static IList<Image> Images
        {
            get
            {
                var images = new List<Image>();

                var xPath = string.Format("root/images/image");
                var nodes = Doc.SelectNodes(xPath);

                var image = new Image();
                foreach (XmlNode node in nodes)
                {
                    image = new Image();
                    image.ImageUrl = node.Attributes["url"].Value.ToString();
                    image.ImageStatus = node.Attributes["status"].Value.ToString();

                    images.Add(image);
                }

                return images;
            }
        }

        public static bool Add(Image image)
        {
            var res = true;

            var newNode = Doc.CreateElement("image");
            newNode.SetAttribute("url", image.ImageUrl);
            newNode.SetAttribute("status", "0");
            newNode.SetAttribute("savePath", "");

            var rootNode = Doc.SelectSingleNode("root/images");

            rootNode.AppendChild(newNode);

            return res;
        }

        public static void UpdateStatus(Image image)
        {
            var xPath = string.Format("root/images/image[@url='{0}']", image.ImageUrl);
            var node = Doc.SelectSingleNode(xPath);

            node.Attributes["status"].Value = image.ImageStatus.ToString();

            Doc.Save(path);
        }

        public static void UpdateSaved(Image image)
        {
            var xPath = string.Format("root/images/image[@url='{0}']", image.ImageUrl);
            var node = Doc.SelectSingleNode(xPath);

            node.Attributes["status"].Value = image.ImageStatus;
            node.Attributes["savePath"].Value = image.ImageSavePath;

            Doc.Save(path);
        }

        public static void Delete(Image image)
        {
            var xPath = string.Format("root/images/image[@url='{0}']", image.ImageUrl);
            var node = Doc.SelectSingleNode(xPath);

            var rootNode = Doc.SelectSingleNode("root/images");

            rootNode.RemoveChild(node);

            Doc.Save(path);
        }
    }
}
