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
            this.button_connect = new System.Windows.Forms.Button();
            this.button_DoWork = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(12, 12);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(311, 238);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // label_connect
            // 
            this.label_connect.AutoSize = true;
            this.label_connect.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_connect.Location = new System.Drawing.Point(335, 12);
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
            this.button_Disconnect.Location = new System.Drawing.Point(339, 209);
            this.button_Disconnect.Name = "button_Disconnect";
            this.button_Disconnect.Size = new System.Drawing.Size(95, 30);
            this.button_Disconnect.TabIndex = 2;
            this.button_Disconnect.Text = "Disconnect";
            this.button_Disconnect.UseVisualStyleBackColor = true;
            this.button_Disconnect.Click += new System.EventHandler(this.button_Disconnect_Click);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(339, 173);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(95, 30);
            this.button_connect.TabIndex = 2;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_DoWork
            // 
            this.button_DoWork.Location = new System.Drawing.Point(339, 137);
            this.button_DoWork.Name = "button_DoWork";
            this.button_DoWork.Size = new System.Drawing.Size(95, 30);
            this.button_DoWork.TabIndex = 2;
            this.button_DoWork.Text = "Run";
            this.button_DoWork.UseVisualStyleBackColor = true;
            this.button_DoWork.Click += new System.EventHandler(this.button_DoWork_Click);
            // 
            // Gbe_Simple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 262);
            this.Controls.Add(this.button_DoWork);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.button_Disconnect);
            this.Controls.Add(this.label_connect);
            this.Controls.Add(this.richTextBox);
            this.Name = "Gbe_Simple";
            this.Text = "Gbe_Simple";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Gbe_Simple_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Label label_connect;
        private System.Windows.Forms.Timer timer1000;
        private System.Windows.Forms.Button button_Disconnect;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_DoWork;
    }
}