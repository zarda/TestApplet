using System.Threading.Tasks;

using Orleans;

using PylonC.NET;
using System;

namespace GrainInterfaces1
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
    public interface IPylon1 : IGrainWithStringKey
    {
        /// <summary>
        /// Initialized Device
        /// </summary>
        /// <returns></returns>
        Task InitializeDdevice();

        /// <summary>
        /// Take single frame shot
        /// </summary>
        /// <returns><see cref="PylonData"/></returns>
        Task<PylonData> RunOneShot();

        /// <summary>
        /// Close the connection to device
        /// </summary>
        /// <returns></returns>
        Task CloseDevice();

        /// <summary>
        /// Report Device Status
        /// </summary>
        /// <returns></returns>
        Task<bool> IsInitialized();
    }

    /// <summary>
    /// Pylon Data Collection
    /// </summary>
    public class PylonData
    {
        public PylonGrabResult_t PylonGrabResultData { get; set; } = new PylonGrabResult_t();
        public uint NumDevices { get; set; } = 0;    /* Number of available devices. */
        public bool IsAvail { get; set; } = false;
        public PYLON_DEVICE_HANDLE HDev { get; set; } = null; /* Handle for the pylon device. */
        public Byte[] Buffer { get; set; } = null;
    }

    /// <summary>
    /// Simple data class for holding image data
    /// </summary>
    public class Image
    {
        public readonly int Width; /* The width of the image. */
        public readonly int Height; /* The height of the image. */
        public readonly Byte[] Buffer; /* The raw image data. */
        public readonly bool Color; /* If false the buffer contains a Mono8 image. Otherwise, RGBA8packed is provided. */

        public Image()
        {
            Width = 0;
            Height = 0;
            Buffer = null;
            Color = false;
        }

        public Image(int newWidth, int newHeight, Byte[] newBuffer, bool color)
        {
            Width = newWidth;
            Height = newHeight;
            Buffer = newBuffer;
            Color = color;
        }

    }
}
