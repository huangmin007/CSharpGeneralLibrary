using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SpaceCG.WindowsAPI.HID
{
	#region Enumerations
	#endregion


	#region Structures
	public struct HIDD_ATTRIBUTES
	{
		public int Size;

		public ushort VendorID;

		public ushort ProductID;

		public ushort VersionNumber;

		public static HIDD_ATTRIBUTES Create()
		{
			return new HIDD_ATTRIBUTES()
			{
				Size = Marshal.SizeOf(typeof(HIDD_ATTRIBUTES)),
			};
		}

		public override string ToString()
		{
			return $"[HIDD_ATTRIBUTES]Size:{Size}, VendorID:{VendorID}, ProductID:{ProductID}, VersionNumber:{VersionNumber}";
		}
	}

	public struct HIDP_CAPS
	{
		public short Usage;

		public short UsagePage;

		public short InputReportByteLength;

		public short OutputReportByteLength;

		public short FeatureReportByteLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public short[] Reserved;

		public short NumberLinkCollectionNodes;

		public short NumberInputButtonCaps;

		public short NumberInputValueCaps;

		public short NumberInputDataIndices;

		public short NumberOutputButtonCaps;

		public short NumberOutputValueCaps;

		public short NumberOutputDataIndices;

		public short NumberFeatureButtonCaps;

		public short NumberFeatureValueCaps;

		public short NumberFeatureDataIndices;

		public override string ToString()
		{
			return $"[HIDP_CAPS]OutputReportByteLength:{OutputReportByteLength}, InputReportByteLength:{InputReportByteLength}, NumberInputValueCaps:{NumberInputValueCaps}";
		}
	}
	#endregion


	#region Deletages
	#endregion


	#region Notifications
	#endregion


	/// <summary>
	/// hdi.dll Human Interface Devices (HID)
	/// <para>参考：https://docs.microsoft.com/zh-cn/windows-hardware/drivers/hid/ </para>
	/// <para>参考：https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/_hid/ </para>
	/// </summary>
	public static partial class HID
    {
		public const int MAX_USB_DEVICES = 64;


        #region Functions

        #region 设备发现和设置
        #endregion

        #region 数据移动
        #endregion

        #region 报表创建和解释
        #endregion

        [DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FlushQueue(SafeFileHandle HidDeviceObject);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern void HidD_GetHidGuid(ref Guid HidGuid);

		[DllImport("hid.dll", SetLastError = true)]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetFeature(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetFeature(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetOutputReport(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetInputReport(SafeFileHandle HidDeviceObject, byte[] lpReportBuffer, int ReportBufferLength);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, int NumberBuffers);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref int NumberBuffers);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern bool HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);

		[DllImport("hid.dll", SetLastError = true)]
		public static extern int HidP_GetValueCaps(int ReportType, byte[] ValueCaps, ref int ValueCapsLength, IntPtr PreparsedData);
        #endregion
    }

	/// <summary>
	/// WindowsAPI HID 库，扩展常用/通用，功能/函数，扩展示例，以及使用方式
	/// </summary>
	public static partial class HIDExtension
	{
	}
}
