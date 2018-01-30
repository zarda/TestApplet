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
            writeTextBox += OnWriteTextBox;
            Fire += CmdEnd;
        }

        private Task CmdEnd()
        {
            fsm.Fire(Command.End);
            return Task.CompletedTask;
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
        Func<bool> OnTrigger = () => IsTrigger;
        Appccelerate.StateMachine.PassiveStateMachine<ProcessState, Command> fsm;
        int count = 0;
        private async void FSM2()
        {
            fsm = new Appccelerate.StateMachine.PassiveStateMachine<ProcessState, Command>();
            fsm.In(ProcessState.Inactive)
                    .ExecuteOnEntry(() => { writeTextBox(this, "Inactived..\n"); })
               .On(Command.Exit).Goto(ProcessState.Terminated)
                                .Execute(() => { writeTextBox(this, "Exit..\n"); })
               .On(Command.Begin).Goto(ProcessState.Active)
                                 .Execute(() => { writeTextBox(this, "-> Goto Active\n"); });
            fsm.In(ProcessState.Active)
                    .ExecuteOnEntry(() => { writeTextBox(this, "Ready to Run..\n"); })
                    .ExecuteOnExit(() => { writeTextBox(this, "Complete..\n"); })
               .On(Command.End).Goto(ProcessState.Inactive)
                               .Execute(() => { writeTextBox(this, "-> Goto Inactive\n"); })
               .On(Command.Pause).Goto(ProcessState.Paused)
                                 .Execute(() => { writeTextBox(this, "-> Goto Pause\n"); });
            fsm.In(ProcessState.Paused)
                    .ExecuteOnEntry(() => { writeTextBox(this, "Paused..\n"); })
               .On(Command.End).Goto(ProcessState.Inactive)
                               .Execute(() => { writeTextBox(this, "-> Goto Inactive\n"); IsTrigger = false; })
               .On(Command.Resume).If(OnTrigger).Goto(ProcessState.Active)
                                                .Execute(() => { writeTextBox(this, "-> Goto Active\n"); })
                                  .Otherwise().Goto(ProcessState.Paused)
                                              .Execute(() => { writeTextBox(this, "-> Retrive Pause\n"); })
                                              .Execute(() => { if (count < 3){ fsm.Fire(Command.Resume); count++; } else count = 0; });

            fsm.Initialize(ProcessState.Inactive);
            fsm.Start();
            
            fsm.Fire(Command.Begin);
            fsm.Fire(Command.Pause);
            fsm.Fire(Command.Resume);
            fsm.Fire(Command.End);

            fsm.Fire(Command.Begin);
            fsm.Fire(Command.Pause);
            fsm.Fire(Command.End);

            fsm.Fire(Command.Begin);
            fsm.Fire(Command.End);

            fsm.Fire(Command.Exit);
            
            fsm.Stop();
        }
        event EventHandler<string> writeTextBox;
        Func<Task> Fire;        

        private void OnWriteTextBox(object sender, string text)
        {
             richTextBox1.AppendText(text); 
            
        }

        enum ProcessState
        {
            Inactive,
            Active,
            Paused,
            Terminated,
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
