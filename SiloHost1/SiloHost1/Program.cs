using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

using PylonC.NET;
using GrainInterfaces1;

namespace SiloHost1
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

            //DoSomeWork();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
        }

        /// <summary>
        /// Do Some Job
        /// </summary>
        static void DoSomeWork()
        {
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var grainFactory = GrainClient.GrainFactory;
            var p1 = grainFactory.GetGrain<GrainInterfaces1.IPylon1>("Pylon1");

            // Initialize
            p1.InitializeDdevice().Wait();
            Console.WriteLine(p1.IsInitialized().Result);
            // Grab image
            //for (int i = 1; i <= 30; i++)
            //{
            //    PylonData result = p1.RunOneShot().Result;
            //    if (result.PylonGrabResultData.Status == EPylonGrabStatus.Grabbed)
            //    {
            //        /* Display image */
            //        Pylon.ImageWindowDisplayImage<Byte>(0, new PylonBuffer<Byte>(result.Buffer), result.PylonGrabResultData);
            //        Console.WriteLine(result.PylonGrabResultData.Status.ToString() + " " + i + "th image");
            //    }
            //    else if (result.PylonGrabResultData.Status == EPylonGrabStatus.Failed)
            //    {
            //        Console.Error.WriteLine("Frame {0} wasn't grabbed successfully.  Error code = {1}", i + 1, result.PylonGrabResultData.ErrorCode);
            //    }
            //}
            // Disconnect
            //p1.CloseDevice().Wait();

            GC.Collect();

        }

        /// <summary>
        /// The initial process of a Silo
        /// </summary>
        /// <param name="args"></param>
        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
            siloHost.ConfigFileName = "OrleansConfiguration.xml";

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format($"Failed to start Orleans silo '{siloHost.Name}' as a {siloHost.Type} node"));

        }

        /// <summary>
        /// The process to release a Silo
        /// </summary>
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
