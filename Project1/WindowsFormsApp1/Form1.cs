using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HTA.Cpp.Arithmetic ar = new HTA.Cpp.Arithmetic();

        public IEnumerable<int> RandomInt
            (int lowBoundary, int highBoundary, int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return ar.Rnd(lowBoundary, highBoundary);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rnd = RandomInt(lowBoundary: 1, highBoundary: 10, length: 2).ToArray();
            richTextBox1.AppendText(
                $"{rnd[0]} + {rnd[1]} = {HTA.Cpp.Arithmetic.Add(rnd[0], rnd[1]).ToString("00")} " + "\t" +
                $"{rnd[0]} - {rnd[1]} = {HTA.Cpp.Arithmetic.Sub(rnd[0], rnd[1]).ToString("00;'-'0' '")} " + "\t" +
                $"{rnd[0]} * {rnd[1]} = {HTA.Cpp.Arithmetic.Mul(rnd[0], rnd[1])} " + "\n"
                );
            // Scrolls the contents of the control to the current caret position.
            richTextBox1.ScrollToCaret();
        }
    }
}
