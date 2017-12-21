namespace ConsoleApp1_siso
{
    partial class Gbe_Simple
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.label_connect = new System.Windows.Forms.Label();
            this.timer1000 = new System.Windows.Forms.Timer(this.components);
            this.button_Disconnect = new System.Windows.Forms.Button();
            this.button_Connect = new System.Windows.Forms.Button();
            this.button_DoWork = new System.Windows.Forms.Button();
            this.hWindowControl_color = new HalconDotNet.HWindowControl();
            this.hWindowControl_R = new HalconDotNet.HWindowControl();
            this.hWindowControl_G = new HalconDotNet.HWindowControl();
            this.hWindowControl_B = new HalconDotNet.HWindowControl();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(12, 147);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(234, 321);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // label_connect
            // 
            this.label_connect.AutoSize = true;
            this.label_connect.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_connect.Location = new System.Drawing.Point(8, 6);
            this.label_connect.Name = "label_connect";
            this.label_connect.Size = new System.Drawing.Size(76, 21);
            this.label_connect.TabIndex = 1;
            this.label_connect.Text = "Connect";
            // 
            // timer1000
            // 
            this.timer1000.Enabled = true;
            this.timer1000.Tick += new System.EventHandler(this.timer1000_Tick);
            // 
            // button_Disconnect
            // 
            this.button_Disconnect.Location = new System.Drawing.Point(12, 105);
            this.button_Disconnect.Name = "button_Disconnect";
            this.button_Disconnect.Size = new System.Drawing.Size(95, 36);
            this.button_Disconnect.TabIndex = 2;
            this.button_Disconnect.Text = "Disconnect";
            this.button_Disconnect.UseVisualStyleBackColor = true;
            this.button_Disconnect.Click += new System.EventHandler(this.button_Disconnect_Click);
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(12, 69);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(95, 36);
            this.button_Connect.TabIndex = 2;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_DoWork
            // 
            this.button_DoWork.Location = new System.Drawing.Point(12, 33);
            this.button_DoWork.Name = "button_DoWork";
            this.button_DoWork.Size = new System.Drawing.Size(95, 36);
            this.button_DoWork.TabIndex = 2;
            this.button_DoWork.Text = "Run";
            this.button_DoWork.UseVisualStyleBackColor = true;
            this.button_DoWork.Click += new System.EventHandler(this.button_DoWork_Click);
            // 
            // hWindowControl_color
            // 
            this.hWindowControl_color.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_color.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_color.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl_color.Location = new System.Drawing.Point(252, 6);
            this.hWindowControl_color.Name = "hWindowControl_color";
            this.hWindowControl_color.Size = new System.Drawing.Size(460, 460);
            this.hWindowControl_color.TabIndex = 3;
            this.hWindowControl_color.WindowSize = new System.Drawing.Size(460, 460);
            // 
            // hWindowControl_R
            // 
            this.hWindowControl_R.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_R.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_R.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl_R.Location = new System.Drawing.Point(718, 6);
            this.hWindowControl_R.Name = "hWindowControl_R";
            this.hWindowControl_R.Size = new System.Drawing.Size(150, 150);
            this.hWindowControl_R.TabIndex = 4;
            this.hWindowControl_R.WindowSize = new System.Drawing.Size(150, 150);
            // 
            // hWindowControl_green
            // 
            this.hWindowControl_G.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_G.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_G.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl_G.Location = new System.Drawing.Point(718, 162);
            this.hWindowControl_G.Name = "hWindowControl_green";
            this.hWindowControl_G.Size = new System.Drawing.Size(150, 150);
            this.hWindowControl_G.TabIndex = 4;
            this.hWindowControl_G.WindowSize = new System.Drawing.Size(150, 150);
            // 
            // hWindowControl_blue
            // 
            this.hWindowControl_B.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_B.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_B.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl_B.Location = new System.Drawing.Point(718, 318);
            this.hWindowControl_B.Name = "hWindowControl_blue";
            this.hWindowControl_B.Size = new System.Drawing.Size(150, 150);
            this.hWindowControl_B.TabIndex = 4;
            this.hWindowControl_B.WindowSize = new System.Drawing.Size(150, 150);
            // 
            // Gbe_Simple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 478);
            this.Controls.Add(this.hWindowControl_B);
            this.Controls.Add(this.hWindowControl_G);
            this.Controls.Add(this.hWindowControl_R);
            this.Controls.Add(this.hWindowControl_color);
            this.Controls.Add(this.button_DoWork);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.button_Disconnect);
            this.Controls.Add(this.label_connect);
            this.Controls.Add(this.richTextBox);
            this.MinimumSize = new System.Drawing.Size(893, 516);
            this.Name = "Gbe_Simple";
            this.Text = "Gbe_Simple";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Gbe_Simple_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.Gbe_Simple_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Label label_connect;
        private System.Windows.Forms.Timer timer1000;
        private System.Windows.Forms.Button button_Disconnect;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Button button_DoWork;
        private HalconDotNet.HWindowControl hWindowControl_color;
        private HalconDotNet.HWindowControl hWindowControl_R;
        private HalconDotNet.HWindowControl hWindowControl_G;
        private HalconDotNet.HWindowControl hWindowControl_B;
    }
}