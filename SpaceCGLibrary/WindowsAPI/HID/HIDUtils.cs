using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using SpaceCG.WindowsAPI.Kernel32;
using SpaceCG.WindowsAPI.SetupAPI;

namespace SpaceCG.WindowsAPI.HID
{
    /// <summary>
    /// HID 
    /// </summary>
    public static class HIDUtils
    {
        /// <summary>
        /// 获取 HID 设备路径列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetHIDDevicesPath()
        {
            Guid HidGuid = Guid.Empty;
            HID.HidD_GetHidGuid(ref HidGuid);
            List<string> deviceList = new List<string>(64);

            IntPtr HidInfoSet = SetupAPI.SetupAPI.SetupDiGetClassDevs(ref HidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF.PRESENT | DIGCF.DEVICEINTERFACE);
            if (HidInfoSet == IntPtr.Zero) return deviceList.ToArray();

            bool hasNext;
            int index = 0;
            SP_DEVICE_INTERFACE_DATA interfaceInfo = SP_DEVICE_INTERFACE_DATA.Create();

            do
            {
                hasNext = SetupAPI.SetupAPI.SetupDiEnumDeviceInterfaces(HidInfoSet, IntPtr.Zero, ref HidGuid, index, ref interfaceInfo);

                if (hasNext)
                {
                    int bufferSize = 0;
                    SetupAPI.SetupAPI.SetupDiGetDeviceInterfaceDetail(HidInfoSet, ref interfaceInfo, IntPtr.Zero, bufferSize, ref bufferSize, IntPtr.Zero);

                    IntPtr pDetail = Marshal.AllocHGlobal(bufferSize);
                    Marshal.WriteInt32(pDetail, (IntPtr.Size == 4) ? (Marshal.SystemDefaultCharSize + 4) : 8);
                    var result = SetupAPI.SetupAPI.SetupDiGetDeviceInterfaceDetail(HidInfoSet, ref interfaceInfo, pDetail, bufferSize, ref bufferSize, IntPtr.Zero);

                    if (result)
                    {
                        IntPtr ptr = new IntPtr(pDetail.ToInt32() + 4);
                        deviceList.Add(Marshal.PtrToStringAuto(ptr));
                        //Console.WriteLine(Marshal.PtrToStringAuto(ptr));
                        Marshal.FreeHGlobal(pDetail);
                    }

                    index++;
                }
            }
            while (hasNext);

            SetupAPI.SetupAPI.SetupDiDestroyDeviceInfoList(HidInfoSet);

            return deviceList.ToArray();
        }

        public static string GetHidUsage(HIDP_CAPS capabilities)
        {
            int num = capabilities.UsagePage * 256 + capabilities.Usage;

            if (num == 0x102) return "mouse";
            if (num == 0x106) return "keyboard";

            return null;
        }


        #region Get Attributes
        public static bool GetHIDDeviceAttributes(SafeFileHandle hidHandle, ref HIDD_ATTRIBUTES attribute)
        {
            bool result = HID.HidD_GetAttributes(hidHandle, ref attribute);
            return result;
        }

        public static bool GetHIDDeviceAttributes(string devicePath, ref HIDD_ATTRIBUTES attribute)
        {
            if (string.IsNullOrWhiteSpace(devicePath)) throw new ArgumentNullException(nameof(devicePath), "设备路径不能为空");

            SafeFileHandle handle = Kernel32.Kernel32.CreateFile(devicePath,
                AccessRights.DEFAULT,
                ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE,
                IntPtr.Zero,
                CreationDisposition.OPEN_EXISTING,
                0,
                IntPtr.Zero);

            bool result = HID.HidD_GetAttributes(handle, ref attribute);
            handle.Close();

            return result;
        }

        public static HIDD_ATTRIBUTES GetHIDDeviceAttributes(string devicePath)
        {
            HIDD_ATTRIBUTES attribute = HIDD_ATTRIBUTES.Create();
            GetHIDDeviceAttributes(devicePath, ref attribute);

            return attribute;
        }
        #endregion

        #region Get Capabilities        
        public static bool GetHIDDeviceCapabilities(SafeFileHandle hidHandle, ref HIDP_CAPS caps)
        {
            int length = -1;
            IntPtr preparsedData = IntPtr.Zero;
            bool result = HID.HidD_GetPreparsedData(hidHandle, ref preparsedData);

            if (result)
            {
                length = HID.HidP_GetCaps(preparsedData, ref caps);
                HID.HidD_FreePreparsedData(preparsedData);
            }
            return result && length > 0;
        }

        public static bool GetHIDDeviceCapabilities(string devicePath, ref HIDP_CAPS caps)
        {
            if (string.IsNullOrWhiteSpace(devicePath)) throw new ArgumentNullException(nameof(devicePath), "设备路径不能为空");

            SafeFileHandle hidHandle = Kernel32.Kernel32.CreateFile(devicePath,
                AccessRights.DEFAULT,
                ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE,
                IntPtr.Zero,
                CreationDisposition.OPEN_EXISTING,
                0,
                IntPtr.Zero);

            bool result = GetHIDDeviceCapabilities(hidHandle, ref caps);
            hidHandle.Close();

            return result;
        }

        public static HIDP_CAPS GetHIDDeviceCapabilities(string devicePath)
        {
            if (string.IsNullOrWhiteSpace(devicePath)) throw new ArgumentNullException(nameof(devicePath), "设备路径不能为空");

            SafeFileHandle hidHandle = Kernel32.Kernel32.CreateFile(devicePath,
                AccessRights.DEFAULT,
                ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE,
                IntPtr.Zero,
                CreationDisposition.OPEN_EXISTING,
                0,
                IntPtr.Zero);

            HIDP_CAPS caps = new HIDP_CAPS();
            bool result = GetHIDDeviceCapabilities(hidHandle, ref caps);

            return caps;
        }
        #endregion


        public static DeviceInfo GetDevice(int vid, int pid)
        {
            string[] paths = HIDUtils.GetHIDDevicesPath();

            for (int i = 0; i < paths.Length; i++)
            {
                Console.WriteLine(paths[i]);
                SafeFileHandle handle = Kernel32.Kernel32.CreateFile(paths[i], 3221225472u, 3, IntPtr.Zero, 3, 0x40000000, IntPtr.Zero);
                if(handle.IsInvalid)
                {
                    Console.WriteLine("SafeFileHandle IsInvalid");
                    Console.WriteLine(Kernel32Extension.GetSysErrroMessage("CreateFile"));
                    continue;
                }

                HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
                bool result1 = HIDUtils.GetHIDDeviceAttributes(handle, ref attributes);
                if(!result1)
                {
                    Console.WriteLine("GetHIDDeviceAttributes Error");
                    continue;
                }
                Console.WriteLine(attributes);

                HIDP_CAPS caps = new HIDP_CAPS();
                bool result2 = HIDUtils.GetHIDDeviceCapabilities(handle, ref caps);
                if (!result2)
                {
                    Console.WriteLine("GetHIDDeviceCapabilities Error.");
                    continue;
                }

                if (attributes.VendorID == vid && attributes.ProductID == pid && caps.InputReportByteLength > 0)
                {
                    Console.WriteLine("{0}  {1}", HIDUtils.GetHidUsage(caps), caps);
                    if(caps.InputReportByteLength > 0)
                    {
                        Console.WriteLine("Index::{0}", i);
                    }

                    int numberBuffer = 0;
                    bool result3 = HID.HidD_GetNumInputBuffers(handle, ref numberBuffer);
                    Console.WriteLine("result3:{0} {1}", result3, numberBuffer);

                    byte[] inputReportBuffer = new byte[caps.InputReportByteLength];
                    bool result4 = HID.HidD_GetInputReport(handle, inputReportBuffer, inputReportBuffer.Length + 1);
                    Console.WriteLine("result4:{0}", result4);

                    DeviceInfo device = new DeviceInfo();
                    device.Path = paths[i];
                    device.Capabilities = caps;
                    device.Attributes = attributes;
                    device.HidUsage = GetHidUsage(caps);
                    /* 
                     device.HidHandle = Kernel32.Kernel32.CreateFile(paths[i],
                         0u,//AccessRights.GENERIC_READ | AccessRights.GENERIC_WRITE,
                         ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE,
                         IntPtr.Zero,
                         CreationDisposition.OPEN_EXISTING,
                         0,
                         IntPtr.Zero);
                         */
                    device.HidHandle = handle;
                    device.InputReportLength = caps.InputReportByteLength;
                    //bool gib = HID.HidD_GetNumInputBuffers(handle, ref device.InputReportLength);
                    //Console.WriteLine("GNIB:{0} {1}", gib, device.InputReportLength);
                    
                    device.FileStream = new FileStream(device.HidHandle, FileAccess.ReadWrite, caps.InputReportByteLength, true);
                    HID.HidD_FlushQueue(device.HidHandle);
                
                    return device;
                }
            }

            return null;// deviceList.ToArray();
        }

    }

    public class DeviceInfo
    {
        public string Path;

        public HIDD_ATTRIBUTES Attributes;

        public HIDP_CAPS Capabilities;

        public int InputReportLength;

        public FileStream FileStream;

        public SafeFileHandle HidHandle;

        public string HidUsage;

        public override string ToString()
        {
            return $"Path:{Path}, vid:{Attributes.VendorID}, pid:{Attributes.ProductID}, Usage:{HidUsage}";
        }
    }
}
