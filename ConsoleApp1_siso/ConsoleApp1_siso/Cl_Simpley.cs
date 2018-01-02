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

            button_Connect.Visible = true;
            button_Disconnect.Visible = false;
            button_DoWork.Visible = false;
            
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
            using (e)
            {   
                using (var hImageRgb = e.CfaToRgb("bayer_rg", "bilinear"))
                {
                    hWindowControl_color.SetFullImagePart(hImageRgb);
                    //hImage.GenImageInterleaved(img.asPtr(), "bgr", (int)width, (int)height, 0, "byte", 0, 0, 0, 0, -1, 0);

                    hImageRgb.DispColor(hWindowControl_color.HalconWindow);
                    //hImage.DispImage(hWindowControl_color.HalconWindow);

                    var hImageTemp1 = hImageRgb.Decompose3(out var hImageTemp2, out var hImageTemp3);
                    hWindowControl_R.SetFullImagePart(hImageTemp1);
                    hWindowControl_G.SetFullImagePart(hImageTemp2);
                    hWindowControl_B.SetFullImagePart(hImageTemp3);

                    using (hImageTemp1)
                    {
                        hImageTemp1.DispImage(hWindowControl_R.HalconWindow);
                    }
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

            UInt64 eventScanMask = 0x01;
            while (eventScanMask > 0)
            {
                var eventName = SiSoCsRt.Fg_getEventName(fg, eventScanMask);
                if (eventName!=null)
                {
                    richTextBox.Text += $"Event name     =  {eventName} \n";   
                }
                /// query for the next event
                eventScanMask <<= 1;
            }
            //var eventMask = SiSoCsRt.Fg_getEventMask(fg, "CamPortATransferEnd");
            //

            //foreach (var id in Enum.GetValues(typeof(Fg_parameterID)))
            //{
            //    richTextBox.Text += $"{(Fg_parameterID)id} = {GetParameterValue((int)id)} ( {GetParameterMin((int)id)} - {GetParameterMax((int)id)} ) \n";
            //}

            richTextBox.Text += $"Gain     = {GetGain()} ({GetGainRange().minimum}-{GetGainRange().maximum} / {GetGainRange().step}) \n";
            richTextBox.Text += $"Width     = {GetWidth()} ({GetWidthRange().minimum}-{GetWidthRange().maximum} / {GetWidthRange().increase}) \n";
            richTextBox.Text += $"Height     = {GetHeight()} ({GetHeightRange().minimum}-{GetHeightRange().maximum} / {GetHeightRange().increase}) \n";
            richTextBox.Text += $"OffsetX     = {GetOffsetX()} ({GetOffsetXRange().minimum}-{GetOffsetXRange().maximum} / {GetOffsetXRange().increase}) \n";
            richTextBox.Text += $"OffsetY     = {GetOffsetY()} ({GetOffsetYRange().minimum}-{GetOffsetYRange().maximum} / {GetOffsetYRange().increase}) \n";

        }
        #region parameter
        public static int GetBoardNum()
        {
            int nrOfBoards = 0;
            byte[] buffer = new byte[256];
            uint buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getSystemInformation(
                    null,
                    Fg_Info_Selector.INFO_NR_OF_BOARDS,
                    FgProperty.PROP_ID_VALUE,
                    0,
                    buffer,
                    ref buflen))
            {
                nrOfBoards = int.Parse(Encoding.ASCII.GetString(buffer));
            }
            return nrOfBoards;
        }
        
        public string GetParameterName(int parameterID)
        {
            string value = string.Empty;
            byte[] buffer = new byte[512];
            int buflen = 512;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    parameterID,
                    FgProperty.PROP_ID_NAME,
                    buffer,
                    ref buflen))
            {
                value = Encoding.Default.GetString(buffer);
            }
            return value;
        }
        public long GetWidthMax()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_WIDTH,
                    FgProperty.PROP_ID_MAX,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetWidthMin()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_WIDTH,
                    FgProperty.PROP_ID_MIN,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetWidthIncrease()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_WIDTH,
                    FgProperty.PROP_ID_STEP,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetHeightMax()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_HEIGHT,
                    FgProperty.PROP_ID_MAX,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetHeighthMin()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_HEIGHT,
                    FgProperty.PROP_ID_MIN,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetHeightIncrease()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_HEIGHT,
                    FgProperty.PROP_ID_STEP,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetXMax()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_XOFFSET,
                    FgProperty.PROP_ID_MAX,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetXMin()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_XOFFSET,
                    FgProperty.PROP_ID_MIN,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetXIncrease()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_XOFFSET,
                    FgProperty.PROP_ID_STEP,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetYMax()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_YOFFSET,
                    FgProperty.PROP_ID_MAX,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetYMin()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_YOFFSET,
                    FgProperty.PROP_ID_MIN,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        public long GetOffsetYIncrease()
        {
            long value = 0;
            byte[] buffer = new byte[256];
            int buflen = 256;
            buffer[0] = 0;

            if (SiSoCsRt.FG_OK == SiSoCsRt.Fg_getParameterProperty(
                    fg,
                    (int)Fg_parameterID.FG_YOFFSET,
                    FgProperty.PROP_ID_STEP,
                    buffer,
                    ref buflen))
            {
                value = long.Parse(Encoding.Default.GetString(buffer));
            }
            return value;
        }
        private (double maximum, double minimum, double step) GetGainRange()
        {            
            return (255, 0, 0.001);
        }

        private double GetGain()
        {
            error = SiSoCsRt.Fg_getParameterWithDouble(fg, SiSoCsRt.FG_PROCESSING_GAIN, out double value, camPort);
            SisoClErrorThrow(error, "Fg_getParameterWithDouble(FG_PROCESSING_GAIN)");
            return value;
        }
        
        private void SetGain(double value)
        {
            value = GetGainRange().minimum < value ? value < GetGainRange().maximum ? value : GetGainRange().maximum : GetGainRange().minimum;
            error = SiSoCsRt.Fg_setParameterWithDouble(fg, SiSoCsRt.FG_PROCESSING_GAIN, value, camPort);
            SisoClErrorThrow(error, "Fg_setParameterWithDouble(FG_PROCESSING_GAIN)");
        }

        private (double maximum, double minimum) GetExposureTimeRange()
        {
            return (10, 0);
        }
        private double GetExposureTime()
        {
            error = SiSoCsRt.Fg_getParameterWithDouble(fg, SiSoCsRt.FG_EXPOSURE, out double value, camPort);
            SisoFgErrorThrow(error, "Fg_getParameterWithDouble(FG_EXPOSURE)");
            return value;
        }
        private void SetExposureTime(double value)
        {
            //value = GetExposureTimeRange().minimum < value ? value < GetExposureTimeRange().maximum ? value : GetExposureTimeRange().maximum : GetExposureTimeRange().minimum;
            error = SiSoCsRt.Fg_setParameterWithDouble(fg, SiSoCsRt.FG_EXPOSURE, value, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithDouble(FG_EXPOSURE)");
        }

        private (long maximum, long minimum, long increase) GetWidthRange()
        {            
            return (GetWidthMax(), GetWidthMin(), GetWidthIncrease());
        }
        private long GetWidth()
        {
            long value = 0;
            error = SiSoCsRt.Fg_getParameterWithLong(fg, SiSoCsRt.FG_WIDTH, out value, camPort);
            SisoFgErrorThrow(error, "Fg_getParameterWithLong(FG_WIDTH)");
            return value;
        }
        private void SetWidth(long value)
        {
            value -= value % GetWidthRange().increase;
            value = GetWidthRange().minimum < value ? value < GetWidthRange().maximum ? value : GetWidthRange().maximum : GetWidthRange().minimum;
            error = SiSoCsRt.Fg_setParameterWithLong(fg, SiSoCsRt.FG_WIDTH, value, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithLong(FG_WIDTH)");
        }

        private (long maximum, long minimum, long increase) GetHeightRange()
        {  
            return (GetHeightMax(), GetHeighthMin(), GetHeightIncrease());
        }
        private long GetHeight()
        {
            long value = 0;
            error = SiSoCsRt.Fg_getParameterWithLong(fg, SiSoCsRt.FG_HEIGHT, out value, camPort);
            SisoFgErrorThrow(error, "Fg_getParameterWithLong(FG_HEIGHT)");
            return value;
        }
        private void SetHeight(long value)
        {
            value -= value % GetHeightRange().increase;
            value = GetHeightRange().minimum < value ? value < GetHeightRange().maximum ? value : GetHeightRange().maximum : GetHeightRange().minimum;
            error = SiSoCsRt.Fg_setParameterWithLong(fg, SiSoCsRt.FG_HEIGHT, value, camPort);
            SisoFgErrorThrow(error, "Fg_setParameterWithUInt(FG_HEIGHT)");
        }

        private (long maximum, long minimum, long increase) GetOffsetXRange()
        {
            return (GetOffsetXMax(), GetOffsetXMin(), GetOffsetXIncrease());
        }
        private long GetOffsetX()
        {
            long value = 0;
            error = SiSoCsRt.Fg_getParameterWithLong(fg, SiSoCsRt.FG_XOFFSET, out value, camPort);
            SisoFgErrorThrow(error, "Fg_getParameterWithLong(FG_XOFFSET)");
            return value;
        }
        private void SetOffsetX(long value)
        {
            value -= value % GetOffsetXRange().increase;
            value = GetOffsetXRange().minimum < value ? value < GetOffsetXRange().maximum ? value : GetOffsetXRange().maximum : GetOffsetXRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetX", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetX)");
        }

        private (long maximum, long minimum, long increase) GetOffsetYRange()
        {
            return (GetOffsetYMax(), GetOffsetYMin(), GetOffsetYIncrease());
        }
        private long GetOffsetY()
        {
            long value = 0;
            error = SiSoCsRt.Fg_getParameterWithLong(fg, SiSoCsRt.FG_YOFFSET, out value, camPort);
            SisoFgErrorThrow(error, "Fg_getParameterWithLong(FG_YOFFSET)");
            return value;
        }
        private void SetOffsetY(long value)
        {
            value -= value % GetOffsetYRange().increase;
            value = GetOffsetYRange().minimum < value ? value < GetOffsetYRange().maximum ? value : GetOffsetYRange().maximum : GetOffsetYRange().minimum;
            error = SiSoCsRt.Gbe_setIntegerValue(cameraHandle, "OffsetY", value);
            SisoClErrorThrow(error, "Gbe_getIntegerValue(OffsetY)");
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

            //uint imageSize = (uint)(bytePerSample * samplePerPixel * width * height);

            // AllocateDMA
            uint totalBufferSize = (uint)(width * height * samplePerPixel * bytePerSample * nbBuffers);
            dma_mem memHandle = SiSoCsRt.Fg_AllocMemEx(fg, totalBufferSize, nbBuffers);

            SetWidth(width);
            SetHeight(height);
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
            OnGrabDoneEvent -= new EventHandler<HalconDotNet.HImage>(OnGrabDoneProcess);
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
            button_Connect.Visible = true;
            button_Disconnect.Visible = false;
            button_DoWork.Visible = false;

        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            DeviceStart();
            GetInformation();
            button_Connect.Visible = false;
            button_Disconnect.Visible = true;
            button_DoWork.Visible = true;
        }

        private async void button_DoWork_Click(object sender, EventArgs e)
        {
            switch (isGrab)
            {
                case true:
                    isGrab = false;
                    button_DoWork.Visible = false;
                    break;
                case false:
                    isGrab = true;
                    button_Disconnect.Visible = false;
                    button_DoWork.Text = "Stop";
                    await Task.Run(() => DoWork());
                    button_DoWork.Text = "Run";
                    button_Disconnect.Visible = true;
                    button_DoWork.Visible = true;
                    break;                
            }
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
