using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Drawing.Drawing2D;
using HTA_Libs;

namespace CShapeTCP
{

    public partial class Form_TCPIP_Demo : Form
    {
        public Form_TCPIP_Demo()
        {
            InitializeComponent();
            //委派指定方法
            UI_Delegate = UpdateMeathod;
            List_Delegate = UpdateConnectList;
        }

        //Form1 UI委派
        public delegate void UpdateForm(string text);
        public UpdateForm UI_Delegate;
        public delegate void UpdateUserList(List<TCPIP.Server.ClientNode> info);
        public UpdateUserList List_Delegate;

        //實作委派方法
        public void UpdateMeathod(string param1)
        {
            //string time = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:fff");
            string time = DateTime.Now.ToString("HH:mm:ss:fff");
            TB_Receive.AppendText(time + "=> " + param1 + Environment.NewLine);

            ListViewItem item = new ListViewItem("R"); //ID   
            item.SubItems.Add(time); //Time
            item.SubItems.Add(param1); //Data
            LV_History.Items.Add(item);
            if (LV_History.Items.Count > 200)
                LV_History.Items.RemoveAt(0);
            LV_History.Items[LV_History.Items.Count - 1].EnsureVisible();
        }

        public void UpdateConnectList(List<TCPIP.Server.ClientNode> info)
        {
            LB_ConnectList.Items.Clear();
            for (int i = 0; i < info.Count; i++)
            {
                LB_ConnectList.Items.Add(info[i].ip + ":" + info[i].port);
            }
        }

        TCPIP.Server tcpip_server = new TCPIP.Server();
        TCPIP.Client tcpip_client = new TCPIP.Client();

        //範例檔案
        class IP1000_Server : TCPIP.Server
        {
            //若有介面UI控制，加入Form的class
            public Form_TCPIP_Demo mainform;
            //建構子傳入Form
            public IP1000_Server(Form_TCPIP_Demo target_form)
            { mainform = target_form; }

            //指令解析
            public override string DoWork(string[] data)
            {
                string reply = "";
                /*
                【範例】
                對方傳送至我方server『cmd 1 2』
                解析data為：
                data[0] = cmd
                data[1] = 1
                data[2] = 2
                 */
                Show(string.Join(this.DelimiterChar.ToString(), data));
                switch (data[0])
                {
                    case "echo":
                        var temp = new List<string>(data);
                        temp.RemoveAt(0);
                        reply = string.Join(this.DelimiterChar.ToString(), temp.ToArray());
                        break;
                    default:
                        break;
                    case "OS":
                        // TCP_CMD_OS();
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString()+"1";
                        break;
                    case "NL":
                        // TCP_CMD_NL();
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "FL":
                        // TCP_CMD_FL();
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "PL":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "SP":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "ZR":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "SS":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "SL":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "EL":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "RL":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "RP":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "SC":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "IP":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "NI":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "CC":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "CS":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "ST":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                    case "FW":
                        reply += string.Join(this.DelimiterChar.ToString(), data);
                        reply += this.DelimiterChar.ToString() + "1"; break;
                }
                return reply;
            }

            //呼叫介面UI委派(連線清單)
            public override void UpdateList(List<ClientNode> info)
            { mainform.Invoke(mainform.List_Delegate, info); }

            //呼叫介面UI委派(訊息顯示)
            public void Show(string text)
            { mainform.Invoke(mainform.UI_Delegate, text); }
        }

        class IP1000_Client : TCPIP.Client
        {
            //若有介面UI控制，加入Form的class
            public Form_TCPIP_Demo mainform;
            //建構子傳入Form
            public IP1000_Client(Form_TCPIP_Demo target_form)
            { mainform = target_form; }

            //指令解析
            public override string DoWork(string[] data)
            {
                Show(string.Join(this.DelimiterChar.ToString(), data));
                switch (data[0])
                {
                    case "CMD":
                        switch (data[1])
                        {
                            case "Test":
                                MessageBox.Show("[CMD:Test]");
                                break;
                        }
                        break;
                }
                return "";
            }
            //呼叫介面UI委派(訊息顯示)
            public void Show(string text)
            { mainform.Invoke(mainform.UI_Delegate, text); }
        }

        //----------------------------------------按鍵事件----------------------------------------
        Color Color_Button_Enable = Color.ForestGreen;
        Color Color_Button_Disable = Color.WhiteSmoke;
        private void Button_Connect_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(TB_IP.Text, out ipAddress) != true)
            {
                MessageBox.Show("Wrong IP format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int t_port = Convert.ToInt32(TB_Port.Text);

            if (RB_Server.Checked == true)
            {
                tcpip_server = new IP1000_Server(this) { Port = t_port, DelimiterChar = ' ' };
                tcpip_server.Connect();
            }
            if (RB_Client.Checked == true)
            {
                tcpip_client = new IP1000_Client(this) { Ip = TB_IP.Text, Port = t_port, DelimiterChar = ' ', Retry = true };
                if (!tcpip_client.Connect().Result)
                {
                    MessageBox.Show("No server on this IP", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            RB_Client.Enabled = false;
            RB_Server.Enabled = false;
            TB_IP.Enabled = false;
            TB_Port.Enabled = false;
            Button_Connect.Enabled = false;
            Button_Disconnect.Enabled = true;
            Button_Connect.ForeColor = Color_Button_Disable;
            Button_Disconnect.ForeColor = Color_Button_Enable;
        }

        private void Button_Disconnect_Click(object sender, EventArgs e)
        {
            if (RB_Server.Checked == true)
            {
                tcpip_server.Disconnect();
                LB_ConnectList.Items.Clear();
            }
            if (RB_Client.Checked == true)
            {
                tcpip_client.Disconnect().Wait();
            }
            RB_Client.Enabled = true;
            RB_Server.Enabled = true;
            TB_IP.Enabled = true;
            TB_Port.Enabled = true;
            Button_Connect.Enabled = true;
            Button_Disconnect.Enabled = false;
            Button_Connect.ForeColor = Color_Button_Enable;
            Button_Disconnect.ForeColor = Color_Button_Disable;
        }

        private void Button_Send_Click(object sender, EventArgs e)
        {
            string data = TB_SendText.Text;
            if (data != "")
            {
                if (RB_Server.Checked == true)
                {
                    tcpip_server.Send(LB_ConnectList.SelectedIndex, data);
                }
                if (RB_Client.Checked == true)
                {
                    tcpip_client.Send(data);
                }
                string time = DateTime.Now.ToString("HH:mm:ss:fff");
                ListViewItem item = new ListViewItem("S"); //ID   
                item.SubItems.Add(time); //Time
                item.SubItems.Add(data); //Data
                LV_History.Items.Add(item);
                if (LV_History.Items.Count > 200)
                    LV_History.Items.RemoveAt(0);
                LV_History.Items[LV_History.Items.Count - 1].EnsureVisible();
                TB_SendText.Text = "";
            }
        }

        //----------------------------------------視窗效果----------------------------------------
        public Point downPoint = Point.Empty;
        private void Form_TCPIP_Demo_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(
                this.Left + e.X - downPoint.X,
                this.Top + e.Y - downPoint.Y);
            this.Location = location;
        }

        private void Form_TCPIP_Demo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        private void Form_TCPIP_Demo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }

        public void SetWindowRegion(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 20);
            this.Region = new Region(FormPath);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            // 左上角
            path.AddArc(arcRect, 180, 90);
            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            //閉合曲線
            path.CloseFigure();
            return path;
        }

        private void Form_TCPIP_Demo_Load(object sender, EventArgs e)
        {

            this.skinEngine = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));

            this.skinEngine.SkinFile = "MSN.ssk";
        }

    }
}
