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
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 2;
        }

        enum BilinearAlgorithm
        {
            Bilinear,
            Bilinear_dir,
            Bilinear_enhanced,
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            if (path.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;

            //Task.Run(async () => {
            //    Application.UseWaitCursor = true;
            //    await ProcessDirectory(path.SelectedPath);
            //    Application.UseWaitCursor = false;
            //});            
            ProcessDirectory(path.SelectedPath, comboBox1.SelectedItem.ToString());
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public void ProcessDirectory(string targetDirectory, string algo)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                Task.Run( () => { ProcessFile(fileName, algo); });
            //ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, algo);
            
        }

        private void ProcessFile(string fileName, string algo)
        {            
            // Local iconic variables 

            HObject ho_Cam00 = new HObject(), ho_RGBImage = new HObject();

            // Initialize local and output iconic variables 
            try
            {                
                HOperatorSet.ReadImage(out ho_Cam00, fileName);
                HOperatorSet.CfaToRgb(ho_Cam00, out ho_RGBImage, "bayer_rg", algo);
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
