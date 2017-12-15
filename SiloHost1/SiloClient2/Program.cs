using System;
using System.Threading;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

using GrainInterfaces1;
using PylonC.NET;
using System.Threading.Tasks;

namespace SiloClient2
{
    /// <summary>
    /// Orleans test silo client
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo(30000);
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            bool IniState = true;
            while (IniState)
            {
                try
                {
                    client.Connect().Wait();

                    if (client.IsInitialized)
                    {
                        IniState = false;
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                    client = new ClientBuilder().UseConfiguration(clientConfig).Build();
                }

            }

            var p1 = client.GetGrain<GrainInterfaces1.IPylon1>("Pylon1");

            //Console.WriteLine("Is Device Avilable: " + p1.IsInitialized().Result);
            //p1.CloseDevice().Wait();
            IniState = true;
            while (IniState)
            {
                try
                {
                    p1.InitializeDdevice().Wait();
                    IniState = false;
                }
                catch
                {
                    Thread.Sleep(1000);
                    p1 = client.GetGrain<GrainInterfaces1.IPylon1>("Pylon1");
                }
            }

            Console.WriteLine("Device is ready.");

            Console.ReadLine();

            if (p1.IsInitialized().Result)
            {
                // Disconnect
                p1.CloseDevice().Wait();
            }

            //Shut down
            client.Close();
        }
    }
}
