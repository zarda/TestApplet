using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsMainNameSpace
{
    public class BaseClass
    {
        [System.ComponentModel.Category("Basic information")]
        [System.ComponentModel.Description("Who's name as a key.")]
        public string Name { set; get; }
        [System.ComponentModel.Category("Basic information")]
        [System.ComponentModel.Description("Who's age by year.")]
        public int Age { set; get; }
        [System.ComponentModel.Category("Basic information")]
        [System.ComponentModel.Description("Who's resident address.")]
        public string Address { set; get; }

        // Initialize BaseClass
        public BaseClass()
        {
            Age = 0;
        }
        // Status of BaseClass
        [System.ComponentModel.Category("Status")]
        [System.ComponentModel.Description("Is this running?")]
        public bool isRun
        {
            set
            {
                if (value)
                {
                    Run();
                    this.StatusRun = false;
                }
                else
                {
                    this.StatusRun = value;
                }
            }
            get
            {
                return this.StatusRun;
            }
        }
        private bool StatusRun;
        // Run a process
        private void Run()
        {
            System.Threading.Thread.Sleep(200);
            Age++;
            Address += ".";
        }

    }
}
