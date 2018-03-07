using HalconDotNet;
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

namespace CfaToRGB
{
    public partial class Form1 : Form
    {
        private string FoldPath;

        public Form1()
        {
            InitializeComponent();
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();

            ProcessDirectory(path.SelectedPath);
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        private static void ProcessFile(string fileName)
        {            
            // Local iconic variables 

            HObject ho_Cam00 = new HObject(), ho_RGBImage = new HObject();
            HImage obj = new HImage();

            // Initialize local and output iconic variables 
            try
            {                
                HOperatorSet.ReadImage(out ho_Cam00, fileName);
                HOperatorSet.CfaToRgb(ho_Cam00, out ho_RGBImage, "bayer_rg", "bilinear");
                HOperatorSet.WriteImage(ho_RGBImage, "bmp", 0, fileName);
            }
            catch(Exception e)
            {

            }

            ho_Cam00.Dispose();
            ho_RGBImage.Dispose();
        }
    }
}
