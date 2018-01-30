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
                richTextBox1.AppendText(item + "-> " + (idx < fsm.cmd.Count ? fsm.cmd[idx++].ToString() : "") + " \n");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            FSM1();
        }
        private static bool IsTrigger = false;
        Func<bool> OnTrigger = () =>  IsTrigger;
        Appccelerate.StateMachine.PassiveStateMachine<ProcessState, Command> fsm =
            new Appccelerate.StateMachine.PassiveStateMachine<ProcessState, Command>();
        private async void FSM2()
        {           
            fsm.In(ProcessState.Inactive)
               .On(Command.Exit).Goto(ProcessState.Terminated).Execute(() => { richTextBox1.AppendText("Exit..\n"); })
               .On(Command.Begin).Goto(ProcessState.Active);
            fsm.In(ProcessState.Active)
               .ExecuteOnEntry(() => { richTextBox1.AppendText("Ready to Run..\n"); })
               .ExecuteOnExit(() => { richTextBox1.AppendText("Complete..\n"); })
               .On(Command.End).Goto(ProcessState.Inactive).Execute(() => { richTextBox1.AppendText("-> Goto Inactive\n"); })
               .On(Command.Pause).Goto(ProcessState.Paused).Execute(() => { richTextBox1.AppendText("-> Goto Pause\n"); });
            fsm.In(ProcessState.Paused)
               .ExecuteOnEntry(() => { richTextBox1.AppendText("Paused..\n");  })
               .On(Command.End).If(OnTrigger).Goto(ProcessState.Inactive).Execute(() => { richTextBox1.AppendText("-> Goto Inactive\n"); })
                               .Otherwise().Goto(ProcessState.Paused).Execute(() => { richTextBox1.AppendText("-> Goto Pause\n"); })
               .On(Command.Resume).Goto(ProcessState.Active).Execute(() => { richTextBox1.AppendText("-> Goto Active\n"); });
            fsm.Initialize(ProcessState.Inactive);
            fsm.Start();

            fsm.Fire(Command.Begin);
            fsm.Fire(Command.Pause);
            await WaitTrigger();            
            fsm.Fire(Command.End);
            fsm.Fire(Command.Begin);
            fsm.Fire(Command.Pause);
            fsm.Fire(Command.Resume);
            fsm.Fire(Command.End);
            fsm.Fire(Command.Exit);

            fsm.Stop();
        }

        private async Task WaitTrigger()
        {
            while (!IsTrigger)
            {                
                await Task.Delay(300);
                fsm.Fire(Command.End);
            }
        }

        enum ProcessState
        {
            Inactive,
            Active,
            Paused,
            Terminated
        }

        enum Command
        {
            Begin,
            End,
            Pause,
            Resume,
            Exit
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            FSM2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsTrigger = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }
    }
}
