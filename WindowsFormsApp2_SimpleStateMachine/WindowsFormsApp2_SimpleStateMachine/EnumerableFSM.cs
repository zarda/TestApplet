using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2_SimpleStateMachine
{
    class EnumerableFSM
    {
        public Action DoJob;
        public List<Cmd> cmd = new List<Cmd>();

        private int Idx = 0;

        public IEnumerable<State> Work()
        {
            InActive:
            yield return State.InActive;
            switch (cmd[Idx++])
            {
                case Cmd.Run:
                    goto Active;
                default:
                    goto Exit;
            }

            Active:
            yield return State.Active;
            if (DoJob != null)
            {
                Task.Run(DoJob);
            }
            //DoJob?.Invoke();
            switch (cmd[Idx++])
            {
                case Cmd.End:
                    goto InActive;
                case Cmd.Idle:
                    goto Idle;
                default:
                    goto Exit;
            }

            Idle:
            yield return State.Idle;
            Thread.Sleep(100);
            switch (cmd[Idx++])
            {
                case Cmd.End:
                    goto InActive;
                case Cmd.Resume:
                    goto Active;
                default:
                    goto Exit;
            }

            Exit:
            yield return State.Exit;
        }
        public enum Cmd
        {
            Run,
            End,
            Idle,
            Resume,
            Exit,
        }
        public enum State
        {
            Active,
            InActive,
            Idle,
            Exit,
        }
    }
}
