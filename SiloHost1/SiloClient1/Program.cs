using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

using GrainInterfaces1;
using PylonC.NET;
using System.Threading;

namespace SiloClient1
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

            while (!p1.IsInitialized().Result)
            {
                Thread.Sleep(1000);
            }

            //GrainClient.Initialize(clientConfig);
            //var grainFactory = GrainClient.GrainFactory;

            for (int i = 1; i <= 100; i++)
            {
                PylonData result = p1.RunOneShot().Result;
                if (result.PylonGrabResultData.Status == EPylonGrabStatus.Grabbed)
                {
                    /* Display image */
                    Pylon.ImageWindowDisplayImage<Byte>(0, new PylonBuffer<Byte>(result.Buffer), result.PylonGrabResultData);
                    Console.WriteLine(result.PylonGrabResultData.Status.ToString() + " " + i + "th image");
                }
                else if (result.PylonGrabResultData.Status == EPylonGrabStatus.Failed)
                {
                    Console.Error.WriteLine("Frame {0} wasn't grabbed successfully.  Error code = {1}", i + 1, result.PylonGrabResultData.ErrorCode);
                }
            }

            GC.Collect();


            // Shut down
            client.Close();
        }
    }
}
