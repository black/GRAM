using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatView
{
    public partial class Form1 : Form
    {

        List<String> allImages = new List<String>();
        int k = 0;
        String filepath = null;

        public Form1()
        {
            InitializeComponent(); 
        }

        public Form1(String[] file)
        {
            InitializeComponent(); 
            if (file.Length != 0)
                filepath = file[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (filepath != null)
            {
                String pathDirectory = Path.GetDirectoryName(filepath);
                allImages = GetImagesPath(pathDirectory);
                k = allImages.IndexOf(filepath);
                setFileName(pathDirectory);
                fitImage(filepath);
            }
        }

        private void openFile(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // ofd.Filter = "jpg (*.jpg)|*.jpg|bmp (*bmp)|*.bmp|png (*.png)|*png|gif (*.gif)|*.gif";

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
            {
                String path = ofd.FileName;
                String pathDirectory = Path.GetDirectoryName(path);
                allImages = GetImagesPath(pathDirectory);
                setFileName(ofd.FileName);
                k = allImages.IndexOf(path);
                fitImage(path);
                //  Dispose();
            }
        }

        public List<String> GetImagesPath(String folderName)
        {
            List<String> imagesList = new List<String>();
            var extensionList = new List<string> { "*.jpg", "*.gif", "*.png", "*.bmp", "*.jpeg" };
            foreach (String fileExtension in extensionList)
            {
                foreach (String file in Directory.GetFiles(folderName, fileExtension))  /* to get all files  from sub directories:   foreach (String file in Directory.GetFiles(folderName, fileExtension, SearchOption.AllDirectories)) */

                {
                    imagesList.Add(file);
                }
            }

            return imagesList;
        }

        private void fitImage(String path)
        {
            pictureBox1.Image = Image.FromFile(path);
            int imgWidth = pictureBox1.Image.Size.Width;
            int imgHeight = pictureBox1.Image.Size.Height;
            int pBoxWidth = pictureBox1.Size.Width;
            int pBoxHeight = pictureBox1.Size.Height;

            if (imgWidth > pBoxWidth || imgHeight > pBoxHeight)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }


        private void prevButton_Click(object sender, EventArgs e)
        {
            goPrev();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            goNext();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Right)
            {
                goNext();
            }
            if (keyData == Keys.Left)
            {
                goPrev();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            String thiFile = allImages[k];
            TryToDelete(thiFile);
            allImages.Remove(thiFile); 
            k++;
            getPicture(k);
        }

        private void setAsDesktop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("set as deskptop");
            String thiFile = allImages[k];
        }

        private void fitToScreen_Click(object sender, EventArgs e)
        {

        }

        void goNext()
        {
            if (k < allImages.Count - 1 && allImages.Count > 0)
            {
                k++;
                getPicture(k);
            }
            else k = 0;
        }

        void goPrev()
        {
            if (0 < k && allImages.Count > 0)
            {
                k--;
                getPicture(k);
            }
            else k = allImages.Count - 1;
        }

        void getPicture(int m)
        {
            String nextFile = allImages[m];
            fitImage(nextFile);
            setFileName(nextFile);
        }

        void setFileName(String filePath)
        {
            String str = Path.GetFileName(filePath);
            this.Text = "G R A M > " + str;
        }

        static bool TryToDelete(string f)
        {
            try
            {
                Console.WriteLine(f);
                if (System.IO.File.Exists(f))
                {
                    File.Delete(f);  
                    //FileSystem.DeleteFile(f);
                    Console.WriteLine("deleted");
                     
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }


                return true;
            }
            catch (IOException)
            {
                Console.WriteLine("Can't delete file");
                return false;
            }
        }


        /*
        void SetWallpaper(String path)
        {
            // DLL Import
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int SystemParametersInfo(
            Int32 uAction,
            Int32 uParam,
            String lpvParam,
            Int32 fuWinIni);

            // Consts
            private const Int32 SPI_SETDESKWALLPAPER = 20;
            private const Int32 SPIF_UPDATEINIFILE = 0x01;
            private const Int32 SPIF_SENDWININICHANGE = 0x02;

            // Changing the background.

            SystemParametersInfo(
            SPI_SETDESKWALLPAPER,
            0,
            path, // desktopBackground is the bmp location
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    */

    }
}
