using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1_siso
{
    public partial class Gbe_Simple : Form
    {
        public Gbe_Simple()
        {
            InitializeComponent();

            button_Connect.Enabled = true;
            button_Disconnect.Enabled = false;
            button_DoWork.Enabled = false;
        }

        private Fg_Struct fg = null;
        private BoardHandle board = null;
        //private int CameraCount = 0;
        private CameraHandle cameraHandle = null;
        private int error = 0;
        private uint camPort = (uint)SiSoCsRt.PORT_A;
        

        private void DeviceStart()
        {
            fg = SiSoCsRt.Fg_Init("QuadAreaGray8", 0);
            if (fg == null)
            {
                throw new Exception( "Fg_Init : " + SiSoCsRt.Fg_getLastErrorDescription(null)); 
            }

            board = SiSoCsRt.Gbe_initBoard(0, 0, out error);
            SisoGbeErrorThrow(error, "Gbe_initBoard");

            error = SiSoCsRt.Gbe_scanNetwork(board, -1, 200);
            SisoGbeErrorThrow(error, "Gbe_scanNetwork");

            //for (int i = 0; i < 4; i++)
            //{
            //    CameraCount = SiSoCsRt.Gbe_getCameraCount(board, i);
            //    SisoErrorThrow(CameraCount, "Gbe_getCameraCount");
            //    richTextBox.Text += $"Port {i} Camera Count = {CameraCount} \n"; 
            //}

            cameraHandle = SiSoCsRt.Gbe_getFirstCamera(board, (int)camPort, out error);
            SisoGbeErrorThrow(error, "Gbe_getFirstCamera");

        }
        private void SisoGbeErrorThrow(int error, string command)
        {
            if (error < 0)
            {
                throw new Exception(command + " : " + SiSoCsRt.Gbe_getErrorDescription(error));
            }
        }
        private void SisoFgErrorThrow(int error, string command)
        {
            if (error < 0)
            {
                throw new Exception(command + " : " + SiSoCsRt.Fg_getErrorDescription(error));
            }
        }

        private void GetInformation()
        {
            var camInfo = SiSoCsRt.Gbe_getCameraInfo(cameraHandle);
            SisoGbeErrorThrow(error, "Gbe_getCameraInfo");
            richTextBox.Text += $"Camera Vendor Name = {camInfo.manufactor_name} \n";
            richTextBox.Text += $"Camera IP = {camInfo.ipv4} \n";
            richTextBox.Text += $"Camera User Defined Name = {camInfo.user_name} \n";
            richTextBox.Text += $"Camera Model Name = {camInfo.model_name} \n";
            richTextBox.Text += $"Camera Device Version = {camInfo.device_version} \n";

            richTextBox.Text += $"Camera Gain Range = {GetGainRange().minimum} ~ {GetGainRange().maximum} \n";
            richTextBox.Text += $"Camera Gain = {GetGain()} \n";

            richTextBox.Text += $"Camera ExposureTime Range = {GetExposureTimeRange().minimum} ~ {GetExposureTimeRange().maximum} \n";
            richTextBox.Text += $"Camera ExposureTime = {GetExposureTime()} \n";

            richTextBox.Text += $"Camera Width Range = {GetWidthRange().minimum} ~ {GetWidthRange().maximum} by {GetWidthRange().increase} \n";
            richTextBox.Text += $"Camera Width = {GetWidth()} \n";

            richTextBox.Text += $"Camera Height Range = {GetHeightRange().minimum} ~ {GetHeightRange().maximum} by {GetHeightRange().increase} \n";
            richTextBox.Text += $"Camera Height = {GetHeight()} \n";

            richTextBox.Text += $"Camera OffsetX Range = {GetOffsetXRange().minimum} ~ {GetOffsetXRange().maximum} by {GetOffsetXRange().increase} \n";
            richTextBox.Text += $"Camera OffsetX = {GetOffsetX()} \n";

            richTextBox.Text += $"Camera OffsetY Range = {GetOffsetYRange().minimum} ~ {GetOffsetYRange().maximum} by {GetOffsetYRange().increase} \n";
            richTextBox.Text += $"Camera OffsetY = {GetOffsetY()} \n";
        }

        private (double maximum, double minimum) GetGainRange()
        {
            double Min = 0, Max = 0;
            error = SiSoCsRt.Gbe_getFloatValueLimits(cameraHandle, "Gain", ref Min, ref Max);
            SisoGbeErrorThrow(error, "Gbe_getFloatValueLimits(Gain)");
            return (Max, Min);
        }
        private double GetGain()
        {
            double value = 0;
            error = SiSoCsRt.Gbe_getFloatValue(cameraHandle, "Gain", ref value);
            SisoGbeErrorThrow(error, "Gbe_getFloatValue(Gain)");
            return value;
        }
        private double SetGain(double value)
        {
            value = GetGainRange().minimum < value ? value < GetGainRange().maximum ? value : GetGainRange().maximum :GetGainRange().minimum;
            error = SiSoCsRt.Gbe_setFloatValue(cameraHandle, "Gain", value);
            SisoGbeErrorThrow(error, "Gbe_getFloatValue(Gain)");
            return value;
        }

        private (double maximum, double minimum) GetExposureTimeRange()
        {
            double Min = 0, Max = 0;
            error = SiSoCsRt.Gbe_getFloatValueLimits(cameraHandle, "ExposureTime", ref Min, ref Max);
            SisoGbeErrorThrow(error, "Gbe_getFloatValueLimits(ExposureTime)");
            return (Max, Min);
        }
        private double GetExposureTime()
        {
            double value = 0;
            error = SiSoCsRt.Gbe_getFloatValue(cameraHandle, "ExposureTime", ref value);
            SisoGbeErrorThrow(error, "Gbe_getFloatValue(ExposureTime)");
            return value;
        }
        private double SetExposureTime(double value)
        {
            value = GetExposureTimeRange().minimum < value ? value < GetExposureTimeRange().maximum ? value : GetExposureTimeRange().maximum : GetExposureTimeRange().minimum;
            error = SiSoCsRt.Gbe_setFloatValue(cameraHandle, "ExposureTime", value);
            SisoGbeErrorThrow(error, "Gbe_getFloatValue(ExposureTime)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetWidthRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "Width", ref Min, ref Max, ref Inc);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValueLimits(Width)");
            return (Max, Min, Inc);
        }
        private long GetWidth()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "Width", ref value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(Width)");
            return value;
        }
        private long SetWidth(long value)
        {
            value -= value % GetWidthRange().increase;
            value = GetWidthRange().minimum < value ? value < GetWidthRange().maximum ? value : GetWidthRange().maximum : GetWidthRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "Width", value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(Width)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetHeightRange()
        {
            long Min = 0, Max = 0, Inc=0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "Height", ref Min, ref Max, ref Inc);
            SisoGbeErrorThrow(error, "Gbe_getFloatValueLimits(Height)");
            return (Max, Min, Inc);
        }
        private long GetHeight()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "Height", ref value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(Height)");
            return value;
        }
        private long SetHeight(long value)
        {
            value -= value % GetHeightRange().increase;
            value = GetHeightRange().minimum < value ? value < GetHeightRange().maximum ? value : GetHeightRange().maximum : GetHeightRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "Height", value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(Height)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetOffsetXRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "OffsetX", ref Min, ref Max, ref Inc);
            SisoGbeErrorThrow(error, "Gbe_getFloatValueLimits(OffsetX)");
            return (Max, Min, Inc);
        }
        private long GetOffsetX()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "OffsetX", ref value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(OffsetX)");
            return value;
        }
        private long SetOffsetX(long value)
        {
            value -= value % GetOffsetXRange().increase;
            value = GetOffsetXRange().minimum < value ? value < GetOffsetXRange().maximum ? value : GetOffsetXRange().maximum : GetOffsetXRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetX", value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(OffsetX)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetOffsetYRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "OffsetY", ref Min, ref Max, ref Inc);
            SisoGbeErrorThrow(error, "Gbe_getFloatValueLimits(OffsetY)");
            return (Max, Min, Inc);
        }
        private long GetOffsetY()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "OffsetY", ref value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(OffsetY)");
            return value;
        }
        private long SetOffsetY(long value)
        {
            value -= value % GetOffsetYRange().increase;
            value = GetOffsetYRange().minimum < value ? value < GetOffsetYRange().maximum ? value : GetOffsetYRange().maximum : GetOffsetYRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetY", value);
            SisoGbeErrorThrow(error, "Gbe_getIntegerValue(OffsetY)");
            return value;
        }

        private void DoWork()
        {
            
            TestGrab();

        }

        private void TestGrab()
        {
            int nrOfPicturesToGrab = 100;
            int nbBuffers = 5;
            uint width  = (uint)GetWidth();
            uint height = (uint)GetHeight();
            int samplePerPixel = 1;
            uint bytePerSample = 1;

            uint imageSize = (uint)(bytePerSample * samplePerPixel * width * height);

            // Create Display
            //int dispId0 = SiSoCsRt.CreateDisplay((uint)(8 * bytePerSample * samplePerPixel), width, height);
            //SiSoCsRt.SetBufferWidth(dispId0, width, height);

            // AllocateDMA
            uint totalBufferSize = (uint)(width * height * samplePerPixel * bytePerSample * nbBuffers);
            dma_mem memHandle = SiSoCsRt.Fg_AllocMemEx(fg, totalBufferSize, nbBuffers);

            //var imageBuffer = SiSoCsRt.Fg_AllocMem(fg, totalBufferSize, 1, (uint)camPort);

            error = SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_WIDTH, width, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithUInt(FG_WIDTH)");
            error = SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_HEIGHT, height, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithUInt(FG_HEIGHT)");
            SiSoCsRt.Fg_setParameterWithInt(fg, SiSoCsRt.FG_BITALIGNMENT, SiSoCsRt.FG_LEFT_ALIGNED, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithInt(FG_BITALIGNMENT)");

            error = SiSoCsRt.Fg_AcquireEx(fg, camPort, nrOfPicturesToGrab, SiSoCsRt.ACQ_STANDARD, memHandle);
            SisoFgErrorThrow(error, "Fg_AcquireEx");

            StartGbeAcquisition();

            int last_pic_nr = 0;
            int cur_pic_nr = 0;
            while ((cur_pic_nr = SiSoCsRt.Fg_getLastPicNumberBlockingEx(fg, last_pic_nr + 1, camPort, 100, memHandle)) < nrOfPicturesToGrab)
            //while ((cur_pic_nr = SiSoCsRt.Fg_getLastPicNumberEx(fg, camPort, memHandle)) < nrOfPicturesToGrab)
            {
                last_pic_nr = cur_pic_nr;
                //SiSoCsRt.DrawBuffer(dispId0, SiSoCsRt.Fg_getImagePtrEx(fg, last_pic_nr, (uint)camPort, memHandle), last_pic_nr, "");

                SisoImage img = SiSoCsRt.Fg_getImagePtrEx(fg, cur_pic_nr, camPort, memHandle);
                using (var hImage = new HalconDotNet.HImage("byte", (int)width, (int)height, img.asPtr()))
                {
                    var hImageRgb = hImage.CfaToRgb("bayer_rg", "bilinear");
                    //hImage.GenImageInterleaved(img.asPtr(), "bgr", (int)width, (int)height, 0, "byte", 0, 0, 0, 0, -1, 0);

                    hWindowControl_color.SetFullImagePart(hImage);
                    hImageRgb.DispColor(hWindowControl_color.HalconWindow);
                    //hImage.DispImage(hWindowControl_color.HalconWindow);

                    using (var hImageTemp1 = hImageRgb.Decompose3(out var hImageTemp2, out var hImageTemp3))
                    {
                        hWindowControl_R.SetFullImagePart(hImageTemp1);
                        hWindowControl_G.SetFullImagePart(hImageTemp2);
                        hWindowControl_B.SetFullImagePart(hImageTemp3);
                        hImageTemp1.DispImage(hWindowControl_R.HalconWindow);
                        hImageTemp2.DispImage(hWindowControl_G.HalconWindow);
                        hImageTemp3.DispImage(hWindowControl_B.HalconWindow);
                    }
                }
                if (cur_pic_nr % nbBuffers == 0)
                {
                    GC.Collect();
                }
            }

            error = SiSoCsRt.Fg_stopAcquireEx(fg, camPort, memHandle, SiSoCsRt.FG_STOP_TIMEOUT);
            SisoFgErrorThrow(error, "Fg_stopAcquireEx");

            StopGbeAcquisition();

            //error = SiSoCsRt.Fg_FreeMem(fg, (uint)camPort);
            //SisoFgErrorThrow(error, "Fg_FreeMem");

            error = SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
            SisoFgErrorThrow(error, "Fg_FreeMemEx");
        }

        private void SoftwareTrigger()
        {
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "On");
            SisoGbeErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:On)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerSource", "Software");
            SisoGbeErrorThrow(error, "Gbe_setEnumerationValue(TriggerSource:Software)");
            error = SiSoCsRt.Gbe_executeCommand(cameraHandle, "TriggerSoftware");
            SisoGbeErrorThrow(error, "Gbe_executeCommand(TriggerSoftware)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "Off");
            SisoGbeErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:Off)");
        }

        private void StopGbeAcquisition()
        {
            error = SiSoCsRt.Gbe_stopAcquisition(cameraHandle);
            SisoGbeErrorThrow(error, "Gbe_stopAcquisition");
        }

        private void StartGbeAcquisition()
        {
            error = SiSoCsRt.Gbe_startAcquisition(cameraHandle);
            SisoGbeErrorThrow(error, "Gbe_startAcquisition");
        }

        private void DisconnectDevice()
        {
            if (cameraHandle != null)
            {
                error = SiSoCsRt.Gbe_disconnectCamera(cameraHandle);
                SisoGbeErrorThrow(error, "Gbe_disconnectCamera"); 
            }
        }

        private void ConnectDevice()
        {
            error = SiSoCsRt.Gbe_connectCamera(cameraHandle);
            SisoGbeErrorThrow(error, "Gbe_connectCamera");
        }

        private void DeviceOff()
        {
            if (cameraHandle != null)
            {
                SiSoCsRt.Gbe_freeCamera(cameraHandle);
            }
            if (board != null)
            {
                SiSoCsRt.Gbe_freeBoard(board);
            }
            if (fg != null)
            {
                SiSoCsRt.Fg_FreeGrabber(fg);
            }
        }

        private void timer1000_Tick(object sender, EventArgs e)
        {
            if (SiSoCsRt.Gbe_isServiceConnectionValid(board)>0)
            {
                label_connect.BackColor = Color.Green;
                label_connect.Text = "Connected";
                button_Connect.Enabled = false;
                button_Disconnect.Enabled = true;
                button_DoWork.Enabled = true;
            }
            else
            {
                label_connect.BackColor = Color.Gray;
                label_connect.Text = "Disconnected";
                button_Connect.Enabled = true;
                button_Disconnect.Enabled = false;
                button_DoWork.Enabled = false;
            }
        }

        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            DisconnectDevice();
            DeviceOff();
            richTextBox.Clear();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            DeviceStart();
            ConnectDevice();
            GetInformation();

        }

        private void button_DoWork_Click(object sender, EventArgs e)
        {
            DoWork();
        }

        private void Gbe_Simple_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SiSoCsRt.Gbe_isServiceConnectionValid(board) > 0)
            {
                button_Disconnect_Click(this, e);
            }
        }

        private void Gbe_Simple_ResizeEnd(object sender, EventArgs e)
        {
            hWindowControl_color.Refresh();
        }
    }
}
