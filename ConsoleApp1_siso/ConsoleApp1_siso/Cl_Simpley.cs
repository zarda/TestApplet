using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1_siso
{
    public partial class Cl_Simple : Form
    {
        public Cl_Simple()
        {
            InitializeComponent();

            button_Connect.Enabled = true;
            button_Disconnect.Enabled = false;
            button_DoWork.Enabled = false;
        }

        private Fg_Struct fg = null;
        private CLSerialRef board = null;
        
        private CameraHandle cameraHandle = null;
        private int error = 0;
        private uint camPort = (uint)SiSoCsRt.PORT_A;
        private uint boardIndex = 1;
        private uint serialIndex = 0;
        private bool isGrab = false ;

        public EventHandler<HalconDotNet.HImage> OnGrabDoneEvent;
        
        private void DeviceStart()
        {
            //SiSoCsRt.Fg_InitLibraries(null);
            //SisoFgErrorThrow(error, "Fg_InitLibraries");

            fg = SiSoCsRt.Fg_InitConfig(Config_Name: "E:\\C#_test\\STC-SPC123BPCL-Mono.mcf", BoardIndex: boardIndex);
            if (fg == null)
            {
                throw new Exception("Fg_InitConfig : " + SiSoCsRt.Fg_getLastErrorDescription(null)); 
            }

            board = SiSoCsRt.clSerialInit(serialIndex, out error);
            SisoClErrorThrow(error, "clSerialInit");

            OnGrabDoneEvent += new EventHandler<HalconDotNet.HImage>(OnGrabDoneProcess);
        }

        private void OnGrabDoneProcess(object sender, HalconDotNet.HImage e)
        {
            using (var hImage = e)
            {
                using (var hImageRgb = hImage.CfaToRgb("bayer_rg", "bilinear"))
                {
                    
                        hWindowControl_color.SetFullImagePart(hImage);

                    //hImage.GenImageInterleaved(img.asPtr(), "bgr", (int)width, (int)height, 0, "byte", 0, 0, 0, 0, -1, 0);

                    hImageRgb.DispColor(hWindowControl_color.HalconWindow);
                    //hImage.DispImage(hWindowControl_color.HalconWindow);

                    using (var hImageTemp1 = hImageRgb.Decompose3(out var hImageTemp2, out var hImageTemp3))
                    {

                            hWindowControl_R.SetFullImagePart(hImageTemp1);
                            hWindowControl_G.SetFullImagePart(hImageTemp2);
                            hWindowControl_B.SetFullImagePart(hImageTemp3);

                        hImageTemp1.DispImage(hWindowControl_R.HalconWindow);
                        using (hImageTemp2)
                        {
                            hImageTemp2.DispImage(hWindowControl_G.HalconWindow);
                        }
                        using (hImageTemp3)
                        {
                            hImageTemp3.DispImage(hWindowControl_B.HalconWindow);
                        }
                    }
                }
            }
        }

        private void SisoClErrorThrow(int error, string command)
        {
            if (error < 0)
            {              
                throw new Exception(command + " : " + ((ClError)error).ToString());
            }
        }
        private void SisoFgErrorThrow(int error, string command)
        {
            if (error < 0)
            {
                throw new Exception(command + " : " + SiSoCsRt.Fg_getErrorDescription(error));
            }
        }

        [DllImport("SiSoCsRt.dll")]
        public static extern int CSharp_clSerialRead
            (CLSerialRef board, ref string strBuffer, ref uint numBrff, int timeOut);
        [DllImport("SiSoCsRt.dll")]
        public static extern int CSharp_clGetErrorText
            (int errorCode, ref IntPtr[] errorText, ref uint errorTextLength);

        private void GetInformation()
        {
            error = SiSoCsRt.clGetNumSerialPorts(out uint numSerialPort);
            SisoClErrorThrow(error, "clGetNumSerialPorts");
            richTextBox.Text += $"Number of serial port     = {numSerialPort} \n";

            for (int i = 0; i < 64; i++)
            {
                var eventName = SiSoCsRt.Fg_getEventName(fg, 1UL << i);
                if (eventName!=null)
                {
                    richTextBox.Text += $"Event name     =  {eventName} \n";   
                }
            }
            var eventMask = SiSoCsRt.Fg_getEventMask(fg, "CamPortATransferEnd");
            richTextBox.Text += $"Event Mask     = {eventMask} \n";

        }
        #region parameter
        private (double maximum, double minimum) GetGainRange()
        {
            double Min = 0, Max = 0;
            error = SiSoCsRt.Gbe_getFloatValueLimits(cameraHandle, "Gain", ref Min, ref Max);
            SisoClErrorThrow(error, "Gbe_getFloatValueLimits(Gain)");
            return (Max, Min);
        }
        private double GetGain()
        {
            double value = 0;
            error = SiSoCsRt.Gbe_getFloatValue(cameraHandle, "Gain", ref value);
            SisoClErrorThrow(error, "Gbe_getFloatValue(Gain)");
            return value;
        }
        private double SetGain(double value)
        {
            value = GetGainRange().minimum < value ? value < GetGainRange().maximum ? value : GetGainRange().maximum :GetGainRange().minimum;
            error = SiSoCsRt.Gbe_setFloatValue(cameraHandle, "Gain", value);
            SisoClErrorThrow(error, "Gbe_getFloatValue(Gain)");
            return value;
        }

        private (double maximum, double minimum) GetExposureTimeRange()
        {
            double Min = 0, Max = 0;
            error = SiSoCsRt.Gbe_getFloatValueLimits(cameraHandle, "ExposureTime", ref Min, ref Max);
            SisoClErrorThrow(error, "Gbe_getFloatValueLimits(ExposureTime)");
            return (Max, Min);
        }
        private double GetExposureTime()
        {
            double value = 0;
            error = SiSoCsRt.Gbe_getFloatValue(cameraHandle, "ExposureTime", ref value);
            SisoClErrorThrow(error, "Gbe_getFloatValue(ExposureTime)");
            return value;
        }
        private double SetExposureTime(double value)
        {
            value = GetExposureTimeRange().minimum < value ? value < GetExposureTimeRange().maximum ? value : GetExposureTimeRange().maximum : GetExposureTimeRange().minimum;
            error = SiSoCsRt.Gbe_setFloatValue(cameraHandle, "ExposureTime", value);
            SisoClErrorThrow(error, "Gbe_getFloatValue(ExposureTime)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetWidthRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "Width", ref Min, ref Max, ref Inc);
            SisoClErrorThrow(error, "Gbe_getIntegerValueLimits(Width)");
            return (Max, Min, Inc);
        }
        private long GetWidth()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "Width", ref value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(Width)");
            return value;
        }
        private long SetWidth(long value)
        {
            value -= value % GetWidthRange().increase;
            value = GetWidthRange().minimum < value ? value < GetWidthRange().maximum ? value : GetWidthRange().maximum : GetWidthRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "Width", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(Width)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetHeightRange()
        {
            long Min = 0, Max = 0, Inc=0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "Height", ref Min, ref Max, ref Inc);
            SisoClErrorThrow(error, "Gbe_getFloatValueLimits(Height)");
            return (Max, Min, Inc);
        }
        private long GetHeight()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "Height", ref value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(Height)");
            return value;
        }
        private long SetHeight(long value)
        {
            value -= value % GetHeightRange().increase;
            value = GetHeightRange().minimum < value ? value < GetHeightRange().maximum ? value : GetHeightRange().maximum : GetHeightRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "Height", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(Height)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetOffsetXRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "OffsetX", ref Min, ref Max, ref Inc);
            SisoClErrorThrow(error, "Gbe_getFloatValueLimits(OffsetX)");
            return (Max, Min, Inc);
        }
        private long GetOffsetX()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "OffsetX", ref value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetX)");
            return value;
        }
        private long SetOffsetX(long value)
        {
            value -= value % GetOffsetXRange().increase;
            value = GetOffsetXRange().minimum < value ? value < GetOffsetXRange().maximum ? value : GetOffsetXRange().maximum : GetOffsetXRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetX", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetX)");
            return value;
        }

        private (long maximum, long minimum, long increase) GetOffsetYRange()
        {
            long Min = 0, Max = 0, Inc = 0;
            error = SiSoCsRt.Gbe_getIntegerValueLimits(cameraHandle, "OffsetY", ref Min, ref Max, ref Inc);
            SisoClErrorThrow(error, "Gbe_getFloatValueLimits(OffsetY)");
            return (Max, Min, Inc);
        }
        private long GetOffsetY()
        {
            long value = 0;
            error = SiSoCsRt.Gbe_getIntegerValue(cameraHandle, "OffsetY", ref value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetY)");
            return value;
        }
        private long SetOffsetY(long value)
        {
            value -= value % GetOffsetYRange().increase;
            value = GetOffsetYRange().minimum < value ? value < GetOffsetYRange().maximum ? value : GetOffsetYRange().maximum : GetOffsetYRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetY", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetY)");
            return value;
        }
        #endregion
        private void DoWork()
        {
            TestGrab();

        }

        private void TestGrab()
        {
            int nrOfPicturesToGrab = 50;
            int nbBuffers = 50;
            uint width  = 4096;
            uint height = 3000;
            int samplePerPixel = 1;
            uint bytePerSample = 1;

            uint imageSize = (uint)(bytePerSample * samplePerPixel * width * height);

            // AllocateDMA
            uint totalBufferSize = (uint)(width * height * samplePerPixel * bytePerSample * nbBuffers);
            dma_mem memHandle = SiSoCsRt.Fg_AllocMemEx(fg, totalBufferSize, nbBuffers);

            error = SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_WIDTH, width, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithUInt(FG_WIDTH)");
            error = SiSoCsRt.Fg_setParameterWithUInt(fg, SiSoCsRt.FG_HEIGHT, height, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithUInt(FG_HEIGHT)");
            SiSoCsRt.Fg_setParameterWithInt(fg, SiSoCsRt.FG_BITALIGNMENT, SiSoCsRt.FG_LEFT_ALIGNED, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithInt(FG_BITALIGNMENT)");

            while (isGrab)
            {
                error = SiSoCsRt.Fg_AcquireEx(fg, camPort, nrOfPicturesToGrab, SiSoCsRt.ACQ_STANDARD, memHandle);
                SisoFgErrorThrow(error, "Fg_AcquireEx");

                int last_pic_nr = 0;
                int cur_pic_nr = 0;

                while ((cur_pic_nr = SiSoCsRt.Fg_getLastPicNumberBlockingEx(fg, last_pic_nr + 1, camPort, 1000, memHandle)) < nrOfPicturesToGrab)
                {
                    //cur_pic_nr = SiSoCsRt.Fg_getLastPicNumberBlockingEx(fg, last_pic_nr + 1, camPort, 1000, memHandle);
                    last_pic_nr = cur_pic_nr;
                    
                    SisoImage img = SiSoCsRt.Fg_getImagePtrEx(fg, cur_pic_nr, camPort, memHandle);
                    OnGrabDoneEvent(this, new HalconDotNet.HImage("byte", (int)width, (int)height, img.asPtr()));
                }

                error = SiSoCsRt.Fg_stopAcquireEx(fg, camPort, memHandle, SiSoCsRt.FG_STOP_TIMEOUT);
                SisoFgErrorThrow(error, "Fg_stopAcquireEx");
            }
            //error = SiSoCsRt.Fg_FreeMem(fg, (uint)camPort);
            //SisoFgErrorThrow(error, "Fg_FreeMem");

            error = SiSoCsRt.Fg_FreeMemEx(fg, memHandle);
            SisoFgErrorThrow(error, "Fg_FreeMemEx");
            
        }

        private void SoftwareTrigger()
        {
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "On");
            SisoClErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:On)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerSource", "Software");
            SisoClErrorThrow(error, "Gbe_setEnumerationValue(TriggerSource:Software)");
            error = SiSoCsRt.Gbe_executeCommand(cameraHandle, "TriggerSoftware");
            SisoClErrorThrow(error, "Gbe_executeCommand(TriggerSoftware)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "Off");
            SisoClErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:Off)");
        }

        private bool IsGrab()
        {
            var result = 0;
            if (fg!=null)
            {
                error = SiSoCsRt.Fg_getParameterWithInt(fg, SiSoCsRt.GRAB_ACTIVE, out result, camPort);
                SisoFgErrorThrow(error, "Fg_setParameterWithUInt(GRAB_ACTIVE)"); 
            }
            return result > 0;
        }

        private void DeviceOff()
        {
            if (board != null)
            {
                //SiSoCsRt.clSerialClose(board);
            }
            if (fg != null)
            {
                SiSoCsRt.Fg_FreeGrabber(fg);
            }
            //SiSoCsRt.Fg_FreeLibraries();
        }
        
        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            DeviceOff();
            richTextBox.Clear();
            button_Connect.Enabled = true;
            button_Disconnect.Enabled = false;
            button_DoWork.Enabled = false;

        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            DeviceStart();
            GetInformation();
            button_Connect.Enabled = false;
            button_Disconnect.Enabled = true;
            button_DoWork.Enabled = true;
        }

        private async void button_DoWork_Click(object sender, EventArgs e)
        {
            switch (isGrab)
            {
                case true:
                    isGrab = false;
                    button_DoWork.Enabled = false;
                    break;
                case false:
                    isGrab = true;
                    button_Disconnect.Enabled = false;
                    button_DoWork.Text = "Stop";
                    await Task.Run(() => DoWork());
                    button_DoWork.Text = "Run";
                    button_Disconnect.Enabled = true;
                    button_DoWork.Enabled = true;
                    break;                
            }
        }
        
        private void Cl_Simple_ResizeEnd(object sender, EventArgs e)
        {
            hWindowControl_color.Refresh();
        }

        enum ClError
        {
            CL_ERR_BAUD_RATE_NOT_SUPPORTED = -10008,
            CL_ERR_BUFFER_TOO_SMALL = -10001,
            CL_ERR_ERROR_NOT_FOUND = -10007,
            CL_ERR_FUNCTION_NOT_FOUND = -10099,
            CL_ERR_INVALID_ARG = -10097,
            CL_ERR_INVALID_INDEX = -10005,
            CL_ERR_INVALID_REFERENCE = -10006,
            CL_ERR_MANU_DOES_NOT_EXIST = -10002,
            CL_ERR_NO_ERR = 0,
            CL_ERR_OUT_OF_MEMORY = -10009,
            CL_ERR_PORT_IN_USE = -10003,
            CL_ERR_TIMEOUT = -10004,
            CL_ERR_UNABLE_TO_LOAD_DLL = -10098,
        }
        
    }
}
