using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace WindowsFormsApplication5
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            //InitializeComputeDevice();
        }

        ~FormMain()
        {
            ho_Image.Dispose();
            ho_HImage.Dispose();
            ho_ImageGauss.Dispose();
            ho_ImageZoom.Dispose();
        }

        static ComputeDevice Device;

        public void InitializeComputeDevice()
        {
            Device = new ComputeDevice();

            Device.DeviceIndex = 1;
            Device.StartDevice();

            HOperatorSet.InitComputeDevice(Device.hv_DeviceHandle, "zoom_image_factor");
            HOperatorSet.InitComputeDevice(Device.hv_DeviceHandle, "derivate_gauss");
            HOperatorSet.InitComputeDevice(Device.hv_DeviceHandle, "sobel_amp");
            HOperatorSet.InitComputeDevice(Device.hv_DeviceHandle, "rgb1_to_gray");

            toolStripLabel2.Text = "Device Ready";

        }

        private void HobjectToHimage(HObject hobject, ref HImage image)
        {
            HTuple pointer, type, width, height;

            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
        }

        HObject ho_Image = new HObject();
        HImage ho_HImage = new HImage();
        HObject ho_ImageZoom = new HObject();
        HObject ho_ImageNoZoom = new HObject();
        HObject ho_ImageGauss = new HObject();
        HObject ho_ImageGray = new HObject();
        HObject ho_ImageEdgeAmp = new HObject();

        static double ZoomScale = 1.0;
        const double ZoomScaleFactor = 0.1;
        static double ZoomScaleHControl = 1.0;

        private bool ho_ImageEmpty = true;

        HTuple hv_Width = null, hv_Height = null;

        private Point MouseLocationOnPanel = new Point();
        private Point MouseLocationOnHWindows = new Point();

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageNoZoom);
            hWindowsCenter();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.ReadImage(out ho_Image, openFileDialog.FileName);

                ho_ImageEmpty = false;

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                ZoomScale = 1.0d;

                hWindowControl1.Size = new Size(hv_Width, hv_Height);

                HobjectToHimage(ho_Image, ref ho_HImage);
                hWindowControl1.SetFullImagePart(ho_HImage);

                hWindowControl1.HalconWindow.ClearWindow();
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);

                HOperatorSet.CopyImage(ho_Image, out ho_ImageNoZoom);

                toolStripLabel2.Text = "Image be Load.";
            }

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (!ho_ImageEmpty)
            {
                hWindowsCenter();
            }
        }

        /*
        static void Delay(int ms, EventHandler action)
        {
            var tmp = new Timer { Interval = ms };
            tmp.Tick += new EventHandler((o, e) => tmp.Enabled = false);
            tmp.Tick += action;
            tmp.Enabled = true;
        }
        */


        private void zoomPlusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomPlus();
            hWindowsCenter();
        }

        private void zoomMinusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomMinus();
            hWindowsCenter();
        }

        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (!ho_ImageEmpty)
            {
                double ScaleTemp = 1;
                if (e.Delta > 0)
                {
                    if (ZoomScaleHControl < 20)
                    {
                        ZoomScaleHControl += ZoomScaleFactor;
                        ScaleTemp = ZoomScaleHControl / (ZoomScaleHControl - ZoomScaleFactor);
                    }
                }
                else if (e.Delta < 0)
                {
                    if (ZoomScaleHControl > 2 * ZoomScaleFactor)
                    {
                        ZoomScaleHControl -= ZoomScaleFactor;
                        ScaleTemp = ZoomScaleHControl / (ZoomScaleHControl + ZoomScaleFactor);
                    }
                }
                UpdateHalconControl(ZoomScaleHControl);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);

                hWindowControl1.Left += (int)(MouseLocationOnHWindows.X * (1 - ScaleTemp));
                hWindowControl1.Top += (int)(MouseLocationOnHWindows.Y * (1 - ScaleTemp));

                toolStripLabel3.Text = "Zoom: " + ZoomScaleHControl;
            }
        }

        private void gaussFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ho_ImageEmpty)
            {

                lock (this)
                {
                    HOperatorSet.DerivateGauss(ho_Image, out ho_ImageGauss, 15, "none"); 
                }
                hWindowControl1.HalconWindow.ClearWindow();

                HOperatorSet.DispObj(ho_ImageGauss, hWindowControl1.HalconWindow);
                ho_Image.Dispose();
                HOperatorSet.CopyImage(ho_ImageGauss, out ho_Image);
                ho_ImageGauss.Dispose();

            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hWindowControl1.HalconWindow.ClearWindow();
            ZoomScale = 1.0;
            ZoomScaleHControl = 1.0;
            ho_ImageEmpty = true;
            toolStripLabel2.Text = "Ready";
            ho_Image.Dispose();
            ho_HImage.Dispose();
            ho_ImageZoom.Dispose();
            ho_ImageGauss.Dispose();
            ho_ImageEdgeAmp.Dispose();
            hWindowsCenter();
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ho_ImageEmpty)
            {
                lock (this)
                {
                    HOperatorSet.Rgb1ToGray(ho_Image, out ho_ImageGray); 
                }

                HOperatorSet.DispObj(ho_ImageGray, hWindowControl1.HalconWindow);
                HOperatorSet.CopyImage(ho_ImageGray, out ho_Image);
                ho_ImageGray.Dispose();
            }
        }

        public void zoomPlus()
        {
            if (ZoomScale < 20 && !ho_ImageEmpty)
            {
                ZoomScale += ZoomScaleFactor;
                ZoomScaleHControl = 1;
                ho_ImageZoom.Dispose();
                lock (this)
                {
                    HOperatorSet.ZoomImageFactor(ho_ImageNoZoom, out ho_ImageZoom, ZoomScale, ZoomScale, "weighted");

                }
                hWindowControl1.HalconWindow.ClearWindow();

                //UpdateHalconControl();

                HImage ho_HImageZoom = new HImage();
                HobjectToHimage(ho_ImageZoom, ref ho_HImageZoom);
                hWindowControl1.SetFullImagePart(ho_HImageZoom);
                HTuple hv_WidthZoom = null, hv_HeightZoom = null;
                HOperatorSet.GetImageSize(ho_ImageZoom, out hv_WidthZoom, out hv_HeightZoom);
                hWindowControl1.Size = new Size(hv_WidthZoom, hv_HeightZoom);

                HOperatorSet.CopyImage(ho_ImageZoom, out ho_Image);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_ImageZoom.Dispose();

                toolStripLabel4.Text = " Image Size:" + hv_Width * ZoomScale + "x" + hv_Height * ZoomScale;

            }

        }

        public void zoomMinus()
        {

            if (ZoomScale > 2 * ZoomScaleFactor & !ho_ImageEmpty)
            {
                ZoomScale -= ZoomScaleFactor;
                ZoomScaleHControl = 1;
                ho_ImageZoom.Dispose();
                lock (this)
                {
                    HOperatorSet.ZoomImageFactor(ho_ImageNoZoom, out ho_ImageZoom, ZoomScale, ZoomScale, "weighted"); 
                }
                hWindowControl1.HalconWindow.ClearWindow();

                //UpdateHalconControl();

                HImage ho_HImageZoom = new HImage();
                HobjectToHimage(ho_ImageZoom, ref ho_HImageZoom);
                hWindowControl1.SetFullImagePart(ho_HImageZoom);
                HTuple hv_WidthZoom = null, hv_HeightZoom = null;
                HOperatorSet.GetImageSize(ho_ImageZoom, out hv_WidthZoom, out hv_HeightZoom);
                hWindowControl1.Size = new Size(hv_WidthZoom, hv_HeightZoom);

                HOperatorSet.CopyImage(ho_ImageZoom, out ho_Image);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                ho_ImageZoom.Dispose();

                toolStripLabel4.Text = " Image Size:" + hv_Width * ZoomScale + "x" + hv_Height * ZoomScale;

            }

        }

        private void sobelAmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ho_ImageEmpty)
            {
                lock (this)
                {
                    HOperatorSet.SobelAmp(ho_Image, out ho_ImageEdgeAmp, "sum_sqrt", 9); 
                }
                HOperatorSet.CopyImage(ho_ImageEdgeAmp, out ho_Image);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);

                ho_ImageEdgeAmp.Dispose();

            }
        }

        private void UpdateHalconControl(double ControlScale)
        {
            hWindowControl1.Size = new Size((int)(int.Parse(hv_Width.ToString()) * (ControlScale)), (int)(int.Parse(hv_Height.ToString()) * (ControlScale)));

        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseLocationOnPanel = e.Location;
        }

        private void hWindowControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseLocationOnHWindows = e.Location;
            }
        }
        private void hWindowControl1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripLabel1.Text = "(" + e.X + ", " + e.Y + ")";

            if (e.Button == MouseButtons.Left)
            {
                hWindowControl1.Left += e.X - MouseLocationOnHWindows.X;
                hWindowControl1.Top += e.Y - MouseLocationOnHWindows.Y;
            }
            else
            {
                MouseLocationOnHWindows = e.Location;
            }

        }

        private void noZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            hWindowsCenter();
            toolStripLabel4.Text = " Image Size:" + hv_Width * ZoomScale + "x" + hv_Height * ZoomScale;
        }

        private void splitContainer1_Panel2_MouseEnter(object sender, EventArgs e)
        {
            hWindowsCenter();
        }

        private void hWindowsCenter()
        {
            hWindowControl1.Location = new Point((splitContainer1.Panel2.ClientSize.Width - hWindowControl1.Size.Width) / 2, (splitContainer1.Panel2.ClientSize.Height - hWindowControl1.Size.Height) / 2);
        }
    }
}
