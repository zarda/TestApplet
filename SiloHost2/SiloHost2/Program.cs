using System;
using System.Threading;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace SiloHost2
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static SiloHost siloHost;

        static void Main(string[] args)
        {
            // Orleans should run in its own AppDomain, we set it up like this
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });

            //Thread t = new Thread(DoSomeClientWork);
            //t.Start();
            //DoSomeClientWork();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
        }


        static void DoSomeClientWork()
        {
            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var friend = GrainClient.GrainFactory.GetGrain<GrainInterfaces1.IGrain1>("hostGrain2");
            
            var result = friend.SayHello("Goodbye").Result;
            Console.WriteLine(result);

            for (int i = 1; i < 10; i++)
            {
                string strTemp = i.ToString();
                Console.WriteLine(friend.SayHello(strTemp, i).Result);
            }
            for (int i = 10; i < 15; i++)
            {
                if (true)
                {
                    var friendTemp = GrainClient.GrainFactory.GetGrain<GrainInterfaces1.IGrain1>(i.ToString());
                    Console.WriteLine(friendTemp.SayHello(i.ToString()).Result);
                }
            }

        }

        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
            siloHost.ConfigFileName = "OrleansConfiguration.xml";

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", siloHost.Name, siloHost.Type));

        }

        static void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }
    }
}
