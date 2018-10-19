using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImagePicker.Controllers;
using ImagePicker.Entities;

namespace ImagePicker
{
    public partial class Form1 : Form
    {
        private IList<Image> imageList;

        private Dictionary<int, Image> ImageDic;

        private long currentIndex = 0;

        public Form1()
        {
            InitializeComponent();

            InitializeImageList();

            InitializeDisplay();
        }

        private void InitializeDisplay()
        {
            ptbMian.SizeMode = PictureBoxSizeMode.CenterImage;
            foreach (KeyValuePair<int, Image> item in ImageDic)
            {
                if (item.Value.ImageStatus.Equals("0"))
                {
                    ptbMian.ImageLocation = item.Value.ImageUrl;
                    item.Value.ImageStatus = "1";
                    XmlUtility.UpdateStatus(item.Value);
                    currentIndex = item.Key;
                    break;
                }
            }
        }

        private void InitializeImageList()
        {
            ImageDic = new Dictionary<int, Image>();
            imageList = XmlUtility.Images;

            var index = 0;
            foreach (var image in imageList)
            {
                ImageDic.Add(index++, image);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MovePrevious();
        }

        private void MovePrevious()
        {
            if ((currentIndex - 1) < ImageDic.Keys.Min()) return;

            var image = ImageDic.Where(i => (i.Key == currentIndex - 1)).FirstOrDefault().Value as Image;

            ptbMian.ImageLocation = image.ImageUrl;
            image.ImageStatus = "1";
            XmlUtility.UpdateStatus(image);
            currentIndex--;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveNext();
        }

        private void MoveNext()
        {
            if ((currentIndex + 1) > ImageDic.Keys.Max()) return;

            var image = ImageDic.Where(i => (i.Key == currentIndex + 1)).FirstOrDefault().Value as Image;

            ptbMian.ImageLocation = image.ImageUrl;
            image.ImageStatus = "1";
            XmlUtility.UpdateStatus(image);
            currentIndex++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void SaveImage()
        {
            ImageDownloadUtility.SaveImage(ImageDic.Where(i => (i.Key == currentIndex)).FirstOrDefault().Value as Image);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //65 110 96

            if (e.KeyValue == 65)
            {
                MovePrevious();
            }
            else if (e.KeyValue == 110)
            {
                MoveNext();
            }
            else if (e.KeyValue == 96)
            {
                SaveImage();
            }
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            ImageDownloadUtility.Download();
        }
    }
}
