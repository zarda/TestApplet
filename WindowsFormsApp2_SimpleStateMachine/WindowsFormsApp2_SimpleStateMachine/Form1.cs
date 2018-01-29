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
            fsm.DoActive += () => { richTextBox1.AppendText("Active..." + "\n"); };
            fsm.DoIdle += () => { richTextBox1.AppendText("Idle..." + "\n"); };
            fsm.DoInActive += () => { richTextBox1.AppendText("Inactive..." + "\n"); };

            fsm.cmd = new List<EnumerableFSM.Cmd>()
            {
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Pause,
                EnumerableFSM.Cmd.Resume,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Pause,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Run,
                EnumerableFSM.Cmd.Pause,
                EnumerableFSM.Cmd.End,
                EnumerableFSM.Cmd.Exit,
            };

            int idx = 0;
            foreach (var item in fsm.Work())
            {
                richTextBox1.AppendText(item + "-> " + (idx < fsm.cmd.Count ? fsm.cmd[idx++].ToString():"") + " \n");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            FSM1();
        }
    }
}
