namespace CShapeTCP
{
    partial class Form_TCPIP_Demo
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Connect = new System.Windows.Forms.Button();
            this.TB_Receive = new System.Windows.Forms.TextBox();
            this.Button_Disconnect = new System.Windows.Forms.Button();
            this.GB_Type = new System.Windows.Forms.GroupBox();
            this.RB_Client = new System.Windows.Forms.RadioButton();
            this.RB_Server = new System.Windows.Forms.RadioButton();
            this.TB_SendText = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.LV_History = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.Button_Send = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.TB_IP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LB_ConnectList = new System.Windows.Forms.ListBox();
            this.skinEngine = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.GB_Type.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Connect
            // 
            this.Button_Connect.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button_Connect.Location = new System.Drawing.Point(288, 6);
            this.Button_Connect.Margin = new System.Windows.Forms.Padding(4);
            this.Button_Connect.Name = "Button_Connect";
            this.Button_Connect.Size = new System.Drawing.Size(133, 75);
            this.Button_Connect.TabIndex = 0;
            this.Button_Connect.Text = "連線";
            this.Button_Connect.UseVisualStyleBackColor = true;
            this.Button_Connect.Click += new System.EventHandler(this.Button_Connect_Click);
            // 
            // TB_Receive
            // 
            this.TB_Receive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(193)))), ((int)(((byte)(185)))));
            this.TB_Receive.Dock = System.Windows.Forms.DockStyle.Right;
            this.TB_Receive.Location = new System.Drawing.Point(288, 0);
            this.TB_Receive.Margin = new System.Windows.Forms.Padding(4);
            this.TB_Receive.Multiline = true;
            this.TB_Receive.Name = "TB_Receive";
            this.TB_Receive.ReadOnly = true;
            this.TB_Receive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Receive.Size = new System.Drawing.Size(279, 185);
            this.TB_Receive.TabIndex = 1;
            // 
            // Button_Disconnect
            // 
            this.Button_Disconnect.Enabled = false;
            this.Button_Disconnect.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button_Disconnect.Location = new System.Drawing.Point(429, 6);
            this.Button_Disconnect.Margin = new System.Windows.Forms.Padding(4);
            this.Button_Disconnect.Name = "Button_Disconnect";
            this.Button_Disconnect.Size = new System.Drawing.Size(133, 75);
            this.Button_Disconnect.TabIndex = 3;
            this.Button_Disconnect.Text = "中斷";
            this.Button_Disconnect.UseVisualStyleBackColor = true;
            this.Button_Disconnect.Click += new System.EventHandler(this.Button_Disconnect_Click);
            // 
            // GB_Type
            // 
            this.GB_Type.Controls.Add(this.RB_Client);
            this.GB_Type.Controls.Add(this.RB_Server);
            this.GB_Type.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GB_Type.Location = new System.Drawing.Point(17, 6);
            this.GB_Type.Margin = new System.Windows.Forms.Padding(4);
            this.GB_Type.Name = "GB_Type";
            this.GB_Type.Padding = new System.Windows.Forms.Padding(4);
            this.GB_Type.Size = new System.Drawing.Size(263, 79);
            this.GB_Type.TabIndex = 5;
            this.GB_Type.TabStop = false;
            this.GB_Type.Text = "通訊類別";
            // 
            // RB_Client
            // 
            this.RB_Client.AutoSize = true;
            this.RB_Client.Dock = System.Windows.Forms.DockStyle.Right;
            this.RB_Client.Location = new System.Drawing.Point(166, 31);
            this.RB_Client.Margin = new System.Windows.Forms.Padding(4);
            this.RB_Client.Name = "RB_Client";
            this.RB_Client.Size = new System.Drawing.Size(93, 44);
            this.RB_Client.TabIndex = 1;
            this.RB_Client.Text = "客戶端";
            this.RB_Client.UseVisualStyleBackColor = true;
            // 
            // RB_Server
            // 
            this.RB_Server.AutoSize = true;
            this.RB_Server.Checked = true;
            this.RB_Server.Dock = System.Windows.Forms.DockStyle.Left;
            this.RB_Server.Location = new System.Drawing.Point(4, 31);
            this.RB_Server.Margin = new System.Windows.Forms.Padding(4);
            this.RB_Server.Name = "RB_Server";
            this.RB_Server.Size = new System.Drawing.Size(93, 44);
            this.RB_Server.TabIndex = 0;
            this.RB_Server.TabStop = true;
            this.RB_Server.Text = "伺服器";
            this.RB_Server.UseVisualStyleBackColor = true;
            // 
            // TB_SendText
            // 
            this.TB_SendText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(202)))), ((int)(((byte)(231)))));
            this.TB_SendText.Dock = System.Windows.Forms.DockStyle.Left;
            this.TB_SendText.Location = new System.Drawing.Point(0, 0);
            this.TB_SendText.Margin = new System.Windows.Forms.Padding(4);
            this.TB_SendText.Multiline = true;
            this.TB_SendText.Name = "TB_SendText";
            this.TB_SendText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_SendText.Size = new System.Drawing.Size(279, 185);
            this.TB_SendText.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.LV_History);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 471);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(567, 149);
            this.panel4.TabIndex = 11;
            // 
            // LV_History
            // 
            this.LV_History.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(250)))), ((int)(((byte)(216)))));
            this.LV_History.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.LV_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LV_History.ForeColor = System.Drawing.Color.Black;
            this.LV_History.GridLines = true;
            this.LV_History.Location = new System.Drawing.Point(0, 0);
            this.LV_History.Margin = new System.Windows.Forms.Padding(4);
            this.LV_History.Name = "LV_History";
            this.LV_History.Size = new System.Drawing.Size(567, 149);
            this.LV_History.TabIndex = 0;
            this.LV_History.UseCompatibleStateImageBehavior = false;
            this.LV_History.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "R/S";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "時間";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "訊息";
            this.columnHeader3.Width = 300;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TB_SendText);
            this.panel1.Controls.Add(this.TB_Receive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 286);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 185);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.Button_Send);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 242);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 44);
            this.panel2.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(139)))), ((int)(((byte)(122)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(287, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(280, 44);
            this.button1.TabIndex = 11;
            this.button1.Text = "接收";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Button_Send
            // 
            this.Button_Send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.Button_Send.Dock = System.Windows.Forms.DockStyle.Left;
            this.Button_Send.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button_Send.Location = new System.Drawing.Point(0, 0);
            this.Button_Send.Margin = new System.Windows.Forms.Padding(4);
            this.Button_Send.Name = "Button_Send";
            this.Button_Send.Size = new System.Drawing.Size(280, 44);
            this.Button_Send.TabIndex = 10;
            this.Button_Send.Text = "傳送";
            this.Button_Send.UseVisualStyleBackColor = false;
            this.Button_Send.Click += new System.EventHandler(this.Button_Send_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TB_Port);
            this.groupBox1.Controls.Add(this.TB_IP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(12, 92);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(268, 139);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "設定";
            // 
            // TB_Port
            // 
            this.TB_Port.Location = new System.Drawing.Point(83, 82);
            this.TB_Port.Margin = new System.Windows.Forms.Padding(4);
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(161, 34);
            this.TB_Port.TabIndex = 4;
            this.TB_Port.Text = "12345";
            // 
            // TB_IP
            // 
            this.TB_IP.Location = new System.Drawing.Point(61, 32);
            this.TB_IP.Margin = new System.Windows.Forms.Padding(4);
            this.TB_IP.Name = "TB_IP";
            this.TB_IP.Size = new System.Drawing.Size(183, 34);
            this.TB_IP.TabIndex = 3;
            this.TB_IP.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP：";
            // 
            // LB_ConnectList
            // 
            this.LB_ConnectList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(250)))), ((int)(((byte)(216)))));
            this.LB_ConnectList.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LB_ConnectList.FormattingEnabled = true;
            this.LB_ConnectList.ItemHeight = 25;
            this.LB_ConnectList.Location = new System.Drawing.Point(291, 101);
            this.LB_ConnectList.Margin = new System.Windows.Forms.Padding(4);
            this.LB_ConnectList.Name = "LB_ConnectList";
            this.LB_ConnectList.Size = new System.Drawing.Size(271, 129);
            this.LB_ConnectList.TabIndex = 10;
            // 
            // skinEngine
            // 
            this.skinEngine.SerialNumber = "";
            this.skinEngine.SkinFile = null;
            // 
            // Form_TCPIP_Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(210)))), ((int)(((byte)(168)))));
            this.ClientSize = new System.Drawing.Size(567, 620);
            this.Controls.Add(this.LB_ConnectList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.GB_Type);
            this.Controls.Add(this.Button_Disconnect);
            this.Controls.Add(this.Button_Connect);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form_TCPIP_Demo";
            this.Text = "TCP/IP-通訊";
            this.Load += new System.EventHandler(this.Form_TCPIP_Demo_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_TCPIP_Demo_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_TCPIP_Demo_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_TCPIP_Demo_MouseUp);
            this.Resize += new System.EventHandler(this.SetWindowRegion);
            this.GB_Type.ResumeLayout(false);
            this.GB_Type.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button Button_Connect;
        private System.Windows.Forms.TextBox TB_Receive;
        private System.Windows.Forms.Button Button_Disconnect;
        private System.Windows.Forms.GroupBox GB_Type;
        private System.Windows.Forms.RadioButton RB_Server;
        private System.Windows.Forms.RadioButton RB_Client;
        private System.Windows.Forms.TextBox TB_SendText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_Port;
        private System.Windows.Forms.TextBox TB_IP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Button_Send;
        private System.Windows.Forms.ListBox LB_ConnectList;
        private System.Windows.Forms.ListView LV_History;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private Sunisoft.IrisSkin.SkinEngine skinEngine;
    }
}

