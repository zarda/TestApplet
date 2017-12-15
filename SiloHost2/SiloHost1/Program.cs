using System;
using System.Threading;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace SiloHost1
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for Orleans Silo to start. Press Enter to proceed...");
            //Console.ReadLine();

            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);

            GrainClient.Initialize(config);

            var hello = GrainClient.GrainFactory.GetGrain<GrainInterfaces1.IGrain1>("remoteGrain2");

            Random rnd = new System.Random();

            Thread t = new Thread(() => {
                for (int i = 0; i < 500; i++)
                {
                    Console.WriteLine(hello.SayHello(i.ToString()).Result);
                    System.Threading.Thread.Sleep(rnd.Next(0, 10));
                }
            });
            t.Start();

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(hello.SayHello(i.ToString()).Result);
                System.Threading.Thread.Sleep(rnd.Next(0,5));
                //int num = 1 / (i - 500);
            }

        }
        
    }
}
