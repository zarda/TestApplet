using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;

namespace WindowsFormsApplication5
{
    class ComputeDevice
    {
        public HTuple hv_DeviceIdentifier = null;
        public HTuple hv_Index = null;
        public HTuple hv_DeviceName = new HTuple();
        public HTuple hv_DeviceVendor = new HTuple();
        public HTuple hv_DeviceHandle = new HTuple();
        public int DeviceIndex { set; get; } = 0;
        public string DeviceName = "";
        public ComputeDevice()
        {
            //Get list of all available compute devices.
            HOperatorSet.QueryAvailableComputeDevices(out hv_DeviceIdentifier);

            //Display basic information on detected devices.
            for (hv_Index = 0; 
                (int)hv_Index <= (int)((new HTuple(hv_DeviceIdentifier.TupleLength())) - 1);
                hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.GetComputeDeviceInfo(
                    hv_DeviceIdentifier.TupleSelect(hv_Index),
                    "name", out hv_DeviceName);
                HOperatorSet.GetComputeDeviceInfo(
                    hv_DeviceIdentifier.TupleSelect(hv_Index),
                    "vendor", out hv_DeviceVendor); 
            }
        }

        ~ComputeDevice()
        {
            StopDevice();
            
        }

        public void StartDevice()
        {
            //Open device.
            HOperatorSet.OpenComputeDevice(
                hv_DeviceIdentifier.TupleSelect(DeviceIndex),
                out hv_DeviceHandle);
            HOperatorSet.ActivateComputeDevice(hv_DeviceHandle);
            //DeviceName = hv_DeviceName.TupleSelect(DeviceIndex);
            //HOperatorSet.SetComputeDeviceParam(hv_DeviceHandle, "alloc_pinned", "false");
            //HOperatorSet.SetComputeDeviceParam(hv_DeviceHandle, "asynchronous_execution", "true");
            //HOperatorSet.SetComputeDeviceParam(hv_DeviceHandle, "image_cache_capacity", 0);
            //HOperatorSet.SetComputeDeviceParam(hv_DeviceHandle, "buffer_cache_capacity", 0);
        }

        public void StopDevice()
        {
            //Deactivate the device
            HOperatorSet.DeactivateComputeDevice(hv_DeviceHandle);
        }
        
    }
}
