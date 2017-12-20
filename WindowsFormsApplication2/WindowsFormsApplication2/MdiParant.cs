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

namespace WindowsFormsApplication2
{
    public partial class FormBase : Form
    {

        public int ChildFormNumber;
        private Queue<MdiChild> MdiQueue2;
        private Queue<MdiChild> MdiQueue3;
        Semaphore semaphoreMdiQueue3;
        bool QueueSwitch = true;

        delegate void MdiParentProcess(MdiChild MdiTemp);


        public FormBase()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ThisFormClosing);
            PrepareQueue2(10);
            DequeueProcess2();
            PrepareQueue3(10);
            Thread DequeueThread3 = new Thread(DequeueProcess3);
            DequeueThread3.Start();
            ChildFormNumber = 1;
        }

        private void ThisFormClosing(object sender, FormClosingEventArgs e)
        {
            QueueSwitch = false;
            semaphoreMdiQueue3.Release();
        }
        
        private void newForm3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MdiQueue3.Enqueue(new MdiChild());
            semaphoreMdiQueue3.Release();
        }

        private void PrepareQueue3(int v)
        {            
            MdiQueue3 = new Queue<MdiChild>(v);
            semaphoreMdiQueue3 = new Semaphore(0, 10);
        }

        private void DequeueProcess3()
        {
            while (QueueSwitch)
            {
                semaphoreMdiQueue3.WaitOne(-1);
                if (MdiQueue3.Count > 0)
                {
                    var MdiTemp = MdiQueue3.Dequeue();
                    SetMdiParent(MdiTemp);
                    ChildFormNumber++;
                }
            }
        }

        private void SetMdiParent(MdiChild MdiTemp)
        {
            if (this.InvokeRequired)
            {
                MdiParentProcess dequeueProcess = new MdiParentProcess(SetMdiParent);
                this.Invoke(dequeueProcess, new object[] { MdiTemp });
            }
            else
            {
                MdiTemp.Text = $"Child Form #{ChildFormNumber}";
                MdiTemp.MdiParent = this;
                MdiTemp.Show();
            }
        }

        private async void DequeueProcess2()
        {
            while (QueueSwitch)
            {
                while (MdiQueue2.Count == 0)
                {
                    await Task.Delay(1);
                    if (!QueueSwitch)
                    {
                        return;
                    }
                }
                var MdiTemp = MdiQueue2.Dequeue();
                MdiTemp.MdiParent = this;
                MdiTemp.Text = $"Child Form #{ChildFormNumber}";
                MdiTemp.Show();
                ChildFormNumber++; 
            }
        }

        private void PrepareQueue2(int v)
        {
            MdiQueue2 = new Queue<MdiChild>(v);            
        }

        private void newFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MdiQueue2.Enqueue(new MdiChild());
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() < 100)
            {
                MdiChild newChild = new MdiChild();
                newChild.MdiParent = this;                
                newChild.Text = $"Child Form #{ChildFormNumber}";
                newChild.Show();
                ChildFormNumber++;
            }
            else
            {
                MessageBox.Show("Too much windows.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void arrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void horizonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }
    }
}
