using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowsAPI.HID
{
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

	public static partial class HID
    {
		/// <summary>
		/// dll name
		/// </summary>
		public const string DLL_NAME = "hid.dll";

		public const int MAX_USB_DEVICES = 64;
	}
}
