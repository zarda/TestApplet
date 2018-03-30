using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var fileName = "D:\\test.txt";
            // Check if file exist.
            if (!System.IO.File.Exists(fileName))
            {
                // Write an array of strings to a file.
                // Create a string array that consists of three lines.
                string[] lines = { "Form closing log:" };
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().
                System.IO.File.WriteAllLines(fileName, lines);
            }
            // Append new text to an existing file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine($"Form shutdown at {DateTime.Now}");
            }

        }

        private void buttonError_Click(object sender, EventArgs e)
        {
            var zero = 0;
            var tt = 1 / zero;
        }
    }
}
