using System;
using System.Diagnostics;
using System.Threading;

namespace SpinWaitUntil
{
    class Program
    {
        static void Main(string[] args)
        {
            var tim = Stopwatch.StartNew();
            var span = new TimeSpan(100);
            //SpinWait.SpinUntil(() => false, span);
            Thread.Sleep(1);

            var result = tim.ElapsedTicks;

            Console.WriteLine(result.ToString());

            
        }
    }
}
