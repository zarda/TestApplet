using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] t = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                string strTemp = i + ": ";
                t[i] = new Thread(() => Work(strTemp + System.DateTime.Now.Millisecond.ToString()));
            }

            foreach (var item in t)
            {
                item.Start();
                Thread.Sleep(1);
            }
            Work("10: " + System.DateTime.Now.Millisecond.ToString());
        }
        static void Work(string message)
        {
            Console.WriteLine(message + " done.");
        }
    }
}
