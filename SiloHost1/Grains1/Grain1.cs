using System;
using System.Threading.Tasks;

using Orleans;

using GrainInterfaces1;

using PylonC.NET;

namespace Grains1
{
    /// <summary>
    /// Grain implementation class Pylon1.
    /// </summary>
    public class Pylon1 : Grain, IPylon1
    {
        private PylonData _pylonData = new PylonData();
        private PylonBuffer<Byte> imgBuf = null;

        /// <summary>
        /// Initialize Pylon for One Shot
        /// </summary>
        /// <returns></returns>
        Task IPylon1.InitializeDdevice()
        {
            try
            {
                /* Before using any pylon methods, the pylon runtime must be initialized. */
                Pylon.Initialize();
                /* Enumerate all camera devices. You must call
                    PylonEnumerateDevices() before creating a device. */
                _pylonData.NumDevices = Pylon.EnumerateDevices();
                if (0 == _pylonData.NumDevices)
                {
                    throw new Exception("No devices found.");
                }
                /* Get a handle for the first device found.  */
                _pylonData.HDev = Pylon.CreateDeviceByIndex(0);

                /* Before using the device, it must be opened. Open it for configuring
                parameters and for grabbing images. */
                Pylon.DeviceOpen(_pylonData.HDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                /* Set the pixel format to Mono8, where gray values will be output as 8 bit values for each pixel. */
                /* ... Check first to see if the device supports the Mono8 format. */
                if (Pylon.DeviceFeatureIsAvailable(_pylonData.HDev, "EnumEntry_PixelFormat_Mono8"))
                {
                    /* ... Set the pixel format to Mono8. */
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "PixelFormat", "Mono8");
                }
                else
                {
                    /* Feature is not available. */
                    throw new Exception("Device doesn't support the Mono8 pixel format.");
                }

                /* Disable acquisition start trigger if available. */
                if (Pylon.DeviceFeatureIsAvailable(_pylonData.HDev, "EnumEntry_TriggerSelector_AcquisitionStart"))
                {
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerSelector", "AcquisitionStart");
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerMode", "Off");
                }

                /* Disable frame burst start trigger if available */
                if (Pylon.DeviceFeatureIsAvailable(_pylonData.HDev, "EnumEntry_TriggerSelector_FrameBurstStart"))
                {
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerSelector", "FrameBurstStart");
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerMode", "Off");
                }

                /* Disable frame start trigger if available */
                if (Pylon.DeviceFeatureIsAvailable(_pylonData.HDev, "EnumEntry_TriggerSelector_FrameStart"))
                {
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerSelector", "FrameStart");
                    Pylon.DeviceFeatureFromString(_pylonData.HDev, "TriggerMode", "Off");
                }

                /* For GigE cameras, we recommend increasing the packet size for better
                  performance. If the network adapter supports jumbo frames, set the packet
                  size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                  to 1500. */
                /* ... Check first to see if the GigE camera packet size parameter is supported
                    and if it is writable. */
                if (Pylon.DeviceFeatureIsWritable(_pylonData.HDev, "GevSCPSPacketSize"))
                {
                    /* ... The device supports the packet size feature. Set a value. */
                    Pylon.DeviceSetIntegerFeature(_pylonData.HDev, "GevSCPSPacketSize", 1500);
                }
                _pylonData.IsAvail = Pylon.DeviceIsOpen(_pylonData.HDev);
            }
            catch (System.Exception e)
            {

                return Task.FromException(e);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Run One Shot
        /// </summary>
        /// <returns>The Grab Result if Success</returns>
        Task<PylonData> IPylon1.RunOneShot()
        {
            if (Pylon.DeviceIsOpen(_pylonData.HDev))
            {
                // Grab one single frame from stream channel 0. The
                // camera is set to "single frame" acquisition mode.
                // Wait up to 500 ms for the image to be grabbed.
                // If imgBuf is null a buffer is automatically created with the right size.
                if (Pylon.DeviceGrabSingleFrame(_pylonData.HDev, 0, ref imgBuf, out PylonGrabResult_t _pylonGrabResultData, 2000))
                {
                    _pylonData.Buffer = imgBuf.Array;
                    _pylonData.PylonGrabResultData = _pylonGrabResultData;
                    //Pylon.ImageWindowDisplayImage<Byte>(-1, imgBuf, _pylonGrabResultData);
                    return Task.FromResult(_pylonData);
                }
                else
                {
                    /* Timeout occurred. */
                    //return Task.FromException(new Exception("Acquire Timeout."));
                    return Task.FromResult(_pylonData);

                }
            }
            else
            {
                return Task.FromResult(new PylonData());
            }
        }

        /// <summary>
        /// Disconnect to device
        /// </summary>
        /// <returns></returns>
        Task IPylon1.CloseDevice()
        {
            if (Pylon.DeviceIsOpen(_pylonData.HDev))
            {
                /* Release the buffer. */
                if (imgBuf != null)
                {
                    imgBuf.Dispose();
                }
                imgBuf = null;

                /* Clean up. Close and release the pylon device. */
                Pylon.DeviceClose(_pylonData.HDev);
                Pylon.DestroyDevice(_pylonData.HDev);

                /* Free memory for grabbing. */
                GC.Collect();

                /* Shut down the pylon runtime system. Don't call any pylon method after
                   calling Pylon.Terminate(). */
                Pylon.Terminate();

                _pylonData.IsAvail = false;
                _pylonData = new PylonData();
            }
            return Task.CompletedTask;
        }

        Task<bool> IPylon1.IsInitialized()
        {
            if (_pylonData.HDev!=null)
            {
                _pylonData.IsAvail = Pylon.DeviceIsOpen(_pylonData.HDev); 
            }
            return Task.FromResult(_pylonData.IsAvail);
        }
    }
}
