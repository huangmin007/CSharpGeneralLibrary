using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SpaceCG.WindowsAPI.HID
{
	/// <summary>
	/// hdi.dll Human Interface Devices (HID)
	/// <para>参考：https://docs.microsoft.com/zh-cn/windows-hardware/drivers/hid/ </para>
	/// <para>参考：https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/_hid/ </para>
	/// </summary>
	public static partial class HID
    {
		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_FlushQueue(SafeFileHandle HidDeviceObject);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern void HidD_GetHidGuid(ref Guid HidGuid);

		[DllImport(DLL_NAME, SetLastError = true)]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern int HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_SetFeature(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_GetFeature(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_SetOutputReport(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_GetInputReport(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, int NumberBuffers);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref int NumberBuffers);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern bool HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern int HidP_GetValueCaps(int ReportType, byte[] ValueCaps, ref int ValueCapsLength, IntPtr PreparsedData);
	}
}
