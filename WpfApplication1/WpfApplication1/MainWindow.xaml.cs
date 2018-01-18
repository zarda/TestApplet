using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EventTrigger += OnTrigger;
        }

        private void OnTrigger(object sender, ProgressBar e)
        {
            e.Value++;
        }

        static int fontSizeChange = 6;
        private void button0_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize += fontSizeChange;
        }

        private void button0_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize -= fontSizeChange;
        }
        private void button1_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize += fontSizeChange;
        }

        private void button1_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize -= fontSizeChange;
        }
        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize += fontSizeChange;
        }

        private void button2_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize -= fontSizeChange;
        }
        private void button3_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize += fontSizeChange;
        }

        private void button3_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)e.Source).FontSize -= fontSizeChange;
        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            doWork(e, Status0);
        }
        private event EventHandler<ProgressBar> EventTrigger;
        public async void doWork(RoutedEventArgs e, ProgressBar p)
        {
            textBox.Text += ((Button)e.Source).Name + " is Running\n";
            ((Button)e.Source).IsEnabled = false;
            p.Value = 0;
            for (int i = 0; i < 100; i++)
            {                
                await Task.Delay(50);
                EventTrigger?.Invoke(this, p); // p.Value = i;
            }
            ((Button)e.Source).IsEnabled = true;
            textBox.Text += ((Button)e.Source).Name + " is Stop\n";
            textBox.ScrollToEnd();
            //await Dispatcher.BeginInvoke(new Action(async () =>
            // {
            //     while (p.Value < 100)
            //     {
            //         await Task.Delay(50);
            //         EventTrigger?.Invoke(this, p);
            //         if (p.Value == 100)
            //         {
            //             ((Button)e.Source).IsEnabled = true;
            //             textBox.Text += ((Button)e.Source).Name + " is Stop\n";
            //             textBox.ScrollToEnd();
            //         }
            //     }
            // }));

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            doWork(e, Status1);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            doWork(e, Status2);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            doWork(e, Status3);
        }
        public void setCursorState(ProgressBar S, Button B, TextBlock T)
        {
            if (S.Value!=0&&S.Value!=100)
            {
                S.Cursor = Cursors.AppStarting;
                B.Cursor = Cursors.AppStarting;
                T.Cursor = Cursors.AppStarting;
            }
            else
            {
                S.Cursor = Cursors.Arrow;
                B.Cursor = Cursors.Arrow;
                T.Cursor = Cursors.Arrow;
            }
        }
        private void Status1_MouseEnter(object sender, MouseEventArgs e) => setCursorState(Status1, Button1, Status1_textBlock);
        private void Status0_MouseEnter(object sender, MouseEventArgs e) => setCursorState(Status0, Button0, Status0_textBlock);
        private void Status2_MouseEnter(object sender, MouseEventArgs e) => setCursorState(Status2, Button2, Status2_textBlock);
        private void Status3_MouseEnter(object sender, MouseEventArgs e) => setCursorState(Status3, Button3, Status3_textBlock);

    }
}
