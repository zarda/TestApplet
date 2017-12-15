using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitializeAppTimer();
            InitializeToolStripStatus();
        }

        private void FormMain_MouseEnter(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                richTextBox1.BackColor = Color.DarkGray;
                AppTimer.Enabled = false;
                
            }
        }

        private void FormMain_MouseLeave(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                richTextBox1.BackColor = Color.LightGray;
                AppTimer.Enabled = true; 
            }
        }

        private void InitializeToolStripStatus()
        {
            toolStripStatus.Text = "App Ready";
        }

        private void InitializeAppTimer()
        {
            AppTimer.Enabled = true;
        }
        private void AppTimer_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now.Second % 5 == 0)
            {
                toolStripStatusTimer.ForeColor = Color.Blue;
            }
            if (DateTime.Now.Second % 5 == 1)
            {
                toolStripStatusTimer.ForeColor = Color.Black;
            }

            toolStripStatusTimer.Text = DateTime.Now.ToString();
            
        }
        
        private void btnAddMessage_Click(object sender, EventArgs e)
        {
            toolStripStatus.Text = "Trigger a message box";
            MessageBox.Show("Message Done...", "Reply:", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolStripStatus.Text = "App Ready";
        }

        private void btnChangeBGColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = richTextBox1.BackColor;
            toolStripStatus.Text = "Background Color Sellecting";
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog.Color;
            }
            toolStripStatus.Text = "Background Color Sellected";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                btnChangeBGColor.Enabled = true;
                toolStripStatus.Text = "Background Color Sellection";
            }
            else
            {
                btnChangeBGColor.Enabled = false;
                toolStripStatus.Text = "Mouse Changes Color";
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatus.Text = "Opening...";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                toolStripStatus.Text = "Open a File";
            }
            else
            {
                richTextBox1.Clear();
                toolStripStatus.Text = "No File be Opened.";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
