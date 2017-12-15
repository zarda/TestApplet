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

namespace WindowsFormsApp1_BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsPause = false;
        //System.Resources.ResourceManager LocRM = new System.Resources.ResourceManager(typeof(Form1));

        private void button_start_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                this.IsPause = false;
                this.progressBar.UseWaitCursor = true;
                this.toolStripStatusLabel_status.Text = Resources.Status.Running;
                this.progressBar.Value = 0;
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 101; i++)
            {
                while (this.IsPause)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    Thread.Sleep(1);
                }
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                backgroundWorker.ReportProgress(i);
                Thread.Sleep(40);

            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            this.label_progress.Text = e.ProgressPercentage.ToString();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.toolStripStatusLabel_status.Text = Resources.Status.Cancel;
            }
            else
                if (e.Error != null)
            {
                this.toolStripStatusLabel_status.Text = "Error: " + e.Error;
            }
            else
            {
                this.toolStripStatusLabel_status.Text = Resources.Status.Complete;
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation)
            {
                this.progressBar.UseWaitCursor = false;
                backgroundWorker.CancelAsync();
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                this.IsPause = true;
                this.toolStripStatusLabel_status.Text = Resources.Status.Pause;
            }
        }
        private void button_continue_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                this.IsPause = false;
                this.toolStripStatusLabel_status.Text = Resources.Status.Running;
            }
        }
    }
}
