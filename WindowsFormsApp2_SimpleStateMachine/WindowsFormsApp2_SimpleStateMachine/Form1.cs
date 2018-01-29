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

namespace WindowsFormsApp2_SimpleStateMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void FSM1()
        {
            EnumerableFSM fsm = new EnumerableFSM();
            fsm.DoJob += () => { Thread.Sleep(300); };

            fsm.cmd = new List<EnumerableFSM.Cmd>()
            {
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Idle,
                EnumerableFSM.Cmd.Resume,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Idle,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Idle,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Exit,
            };
            
            foreach (var item in fsm.Work())
            {
                richTextBox1.AppendText(item + "\n");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            FSM1();
        }
    }
}
