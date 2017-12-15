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
        private System.Collections.ArrayList listObject = new System.Collections.ArrayList();
        public BaseClass TestClass = new BaseClass();
        public void InitializeManual()
        {
            
            listBox1.Items.Add("1: BaseClass");
            listObject.Add(TestClass);

            listBox1.Items.Add("2: A New Windows Form");
            listObject.Add(new System.Windows.Forms.Form());

            listBox1.Items.Add("3: HTuple");
            listObject.Add(new HTuple());

            listBox1.Items.Add("4: ArrayList");
            listObject.Add(new System.Collections.ArrayList());

            listBox1.Items.Add("4: FormMain");
            listObject.Add(this);

            var ControlList = ControlGetAll(this);
            int index = 6;
            foreach (var item in ControlList)
            {
                listBox1.Items.Add(index +": " + item.ToString());
                listObject.Add(item);
                index++;
            }

        }

        public IEnumerable<Control> ControlGetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => ControlGetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
        public IEnumerable<Control> ControlGetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => ControlGetAll(ctrl)).Concat(controls);
        }

    }
}
