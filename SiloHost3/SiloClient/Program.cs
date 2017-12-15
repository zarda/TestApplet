using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

using GrainInterfaces1;

namespace SiloClient
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var grainFactory = GrainClient.GrainFactory;
            var e0 = grainFactory.GetGrain<IEmployee>("e0 Employee");
            var e1 = grainFactory.GetGrain<IEmployee>("e1 Employee");
            var e2 = grainFactory.GetGrain<IEmployee>("e2 Employee");
            var e3 = grainFactory.GetGrain<IEmployee>("e3 Employee");
            var e4 = grainFactory.GetGrain<IEmployee>("e4 Employee");

            var m0 = grainFactory.GetGrain<IManager>("m0 Manager");
            var m1 = grainFactory.GetGrain<IManager>("m1 Manager");
            var m0e = m0.AsEmployee().Result;
            var m1e = m1.AsEmployee().Result;

            m0e.Promote(10);
            m1e.Promote(11);

            var TimeStart = DateTime.Now.Millisecond;
            m0.AddDirectReport(e0).Wait();
            m0.AddDirectReport(e1).Wait();
            m0.AddDirectReport(e2).Wait();
            Console.WriteLine($"teme elapsed={DateTime.Now.Millisecond - TimeStart}");

            Console.WriteLine("List of manager m0's manbers: ");

            var m0list = m0.GetDirectReports().Result;
            foreach (var item in m0list)
            {
                Console.Write(item.GetPrimaryKeyString().ToString() + ", ");
            }
            Console.WriteLine("");

            TimeStart = DateTime.Now.Millisecond;
            m1.AddDirectReport(m0e).Wait();
            m1.AddDirectReport(e3).Wait();
            m1.AddDirectReport(e4).Wait();
            Console.WriteLine($"teme elapsed={DateTime.Now.Millisecond - TimeStart}");

            Console.WriteLine("List of manager m1's manbers: ");
            var m1list = m1.GetDirectReports().Result;
            foreach (var item in m0list)
            {
                Console.Write(item.GetPrimaryKeyString().ToString() + ", ");
            }
            Console.WriteLine("");
            Console.WriteLine();

            TimeStart = DateTime.Now.Millisecond;
            for (int i = 0; i < 2; i++)
            {
                var en = grainFactory.GetGrain<IEmployee>($"e{i} Employee");
                m1.AddDirectReport(en).Wait();
            }
            Console.WriteLine($"teme elapsed={DateTime.Now.Millisecond - TimeStart}");

            //  Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();
        }
    }
}
