using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rex_WordFilter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            /// XML ocupied
            List<string> forbidden = new List<string>();
            forbidden.Add(" ");
            forbidden.Add("<");
            forbidden.Add(">");
            forbidden.Add("&");
            forbidden.Add("\'");
            forbidden.Add("\"");
            forbidden.Add("\\");
            forbidden.Add("/");
            /// Numbers
            List<string> number = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                number.Add(i.ToString());
            }
            //textBox2.Text = VerifyWord1(textBox1.Text, " ").ToString();
            //textBox2.Text = VerifyWord2(textBox1.Text, " ").ToString();
            //textBox2.Text = VerifyWord3(textBox1.Text, " ").ToString();
            //textBox2.Text = WordContain(textBox1.Text, forbidden).ToString();
            //textBox2.Text = WordStartWith(textBox1.Text, forbidden).ToString();
            button1.Enabled = !(WordStartWith(textBox1.Text, number)| WordContain(textBox1.Text, forbidden));
        }

        private bool WordStartWith(string text, List<string> forbidden)
        {
            bool result = false;
            foreach (var check in forbidden)
            {
                result |= text.StartsWith(check);
            }
            return result;
        }

        private bool WordContain(string text, List<string> forbidden)
        {
            bool result = false;
            foreach (var check in forbidden)
            {
                result |= text.Contains(check);
            }
            return result;
        }

        private bool VerifyWord3(string text, string check)
        {
            return text.StartsWith(check);
        }

        private bool VerifyWord2(string text, string check)
        {
            Regex Check = new Regex(check);
            return Check.IsMatch(text);
        }

        private bool VerifyWord1(string text, string chip)
        {
            return text.Contains(chip);
        }


    }
}
