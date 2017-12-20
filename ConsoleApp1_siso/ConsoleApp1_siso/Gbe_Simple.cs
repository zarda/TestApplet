using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        }

        private BoardHandle board = null;
        //private int CameraCount = 0;
        private CameraHandle cameraHandle = null;
        private int error = 0;
        private void DeviceStart()
        {
            board = SiSoCsRt.Gbe_initBoard(0, 0, out error);
            SisoErrorThrow(error, "Gbe_initBoard");

            error = SiSoCsRt.Gbe_scanNetwork(board, -1, 200);
            SisoErrorThrow(error, "Gbe_scanNetwork");

            //for (int i = 0; i < 4; i++)
            //{
            //    CameraCount = SiSoCsRt.Gbe_getCameraCount(board, i);
            //    SisoErrorThrow(CameraCount, "Gbe_getCameraCount");
            //    richTextBox.Text += $"Port {i} Camera Count = {CameraCount} \n"; 
            //}

            cameraHandle = SiSoCsRt.Gbe_getFirstCamera(board, 0, out error);
            SisoErrorThrow(error, "Gbe_getFirstCamera");

        }
        private void SisoErrorThrow(int error, string command)
        {
            if (error < 0)
            {
                throw new Exception(command + " : " + SiSoCsRt.Gbe_getErrorDescription(error));
            }
        }

        private void GetInformation()
        {
            var camInfo = SiSoCsRt.Gbe_getCameraInfo(cameraHandle);
            SisoErrorThrow(error, "Gbe_getCameraInfo");
            richTextBox.Text += $"Camera Vendor Name = {camInfo.manufactor_name} \n";
            richTextBox.Text += $"Camera IP = {camInfo.ipv4} \n";
            richTextBox.Text += $"Camera User Defined Name = {camInfo.user_name} \n";
            richTextBox.Text += $"Camera Model Name = {camInfo.model_name} \n";
            richTextBox.Text += $"Camera Device Version = {camInfo.device_version} \n";

            richTextBox.Text += $"Camera Gain Range = {GetGainRange().minimum} ~ {GetGainRange().maximum} \n";
            richTextBox.Text += $"Camera Gain = {GetGain()} \n";

        }

        private (double maximum, double minimum) GetGainRange()
        {
            double Min = 0, Max = 0;
            error = SiSoCsRt.Gbe_getFloatValueLimits(cameraHandle, "Gain", ref Min, ref Max);
            SisoErrorThrow(error, "Gbe_stopAcquisition");
            return (Max, Min);
        }
        private double GetGain()
        {
            double value = 0;
            error = SiSoCsRt.Gbe_getFloatValue(cameraHandle, "Gain", ref value);
            SisoErrorThrow(error, "Gbe_stopAcquisition");
            return value;
        }

        private void DoWork()
        {
            StartAcquisition();
            //Thread.Sleep(500);
            

            StopAcquisition();
            SoftwareTrigger();
        }

        private void SoftwareTrigger()
        {
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "On");
            SisoErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:On)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerSource", "Software");
            SisoErrorThrow(error, "Gbe_setEnumerationValue(TriggerSource:Software)");
            error = SiSoCsRt.Gbe_executeCommand(cameraHandle, "TriggerSoftware");
            SisoErrorThrow(error, "Gbe_executeCommand(TriggerSoftware)");
            error = SiSoCsRt.Gbe_setEnumerationValue(cameraHandle, "TriggerMode", "Off");
            SisoErrorThrow(error, "Gbe_setEnumerationValue(TriggerMode:Off)");
        }

        private void StopAcquisition()
        {
            error = SiSoCsRt.Gbe_stopAcquisition(cameraHandle);
            SisoErrorThrow(error, "Gbe_stopAcquisition");
        }

        private void StartAcquisition()
        {
            error = SiSoCsRt.Gbe_startAcquisition(cameraHandle);
            SisoErrorThrow(error, "Gbe_startAcquisition");
        }

        private void DisconnectDevice()
        {
            error = SiSoCsRt.Gbe_disconnectCamera(cameraHandle);
            SisoErrorThrow(error, "Gbe_disconnectCamera");
        }

        private void ConnectDevice()
        {
            error = SiSoCsRt.Gbe_connectCamera(cameraHandle);
            SisoErrorThrow(error, "Gbe_connectCamera");
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
        }

        private void timer1000_Tick(object sender, EventArgs e)
        {
            if (SiSoCsRt.Gbe_isServiceConnectionValid(board)>0)
            {
                label_connect.BackColor = Color.Green;
                label_connect.Text = "Connected";
            }
            else
            {
                label_connect.BackColor = Color.Gray;
                label_connect.Text = "Disconnected";
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
    }
}
