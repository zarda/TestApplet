using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

namespace WindowsFormsMainNameSpace
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitializeManual();
        }


        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listObject[listBox1.SelectedIndex];
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            this.TestClass.isRun = false;
            //this.propertyGrid1.Refresh();
        }
    }


}
