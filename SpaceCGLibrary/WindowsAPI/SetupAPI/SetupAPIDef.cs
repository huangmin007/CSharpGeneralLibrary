using System;
using System.Runtime.InteropServices;

namespace SpaceCG.WindowsAPI.SetupAPI
{

	#region Enumerations	
	/// <summary>
	/// <see cref="SetupAPI.SetupDiGetClassDevs"/> 函数参考的 零个或多个标志
	/// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw </para>
	/// </summary>
	[Flags]
	public enum DIGCF:uint
	{
		/// <summary>
		/// 对于指定的设备接口类，仅返回与系统默认设备接口关联的设备（如果已设置）。
		/// </summary>
		DEFAULT = 0x00000001,
		/// <summary>
		/// 仅返回系统中当前存在的设备。
		/// </summary>
		PRESENT = 0x00000002,
		/// <summary>
		/// 返回所有设备设置类或所有设备接口类的已安装设备的列表。
		/// </summary>
		ALLCLASSES = 0x00000004,
		/// <summary>
		/// 仅返回属于当前硬件配置文件的设备。
		/// </summary>
		PROFILE = 0x00000008,
		/// <summary>
		/// 返回支持指定设备接口类的设备接口的设备。如果 Enumerator 参数指定设备实例 ID，则必须在 Flags 参数中设置此标志。
		/// </summary>
		DEVICEINTERFACE = 0x00000010,
	}
	#endregion


	#region Structures
	/// <summary>
	/// 定义设备信息集中的设备接口。
	/// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_device_interface_data </para>
	/// </summary>
	public struct SP_DEVICE_INTERFACE_DATA
	{
		public uint cbSize;

		public Guid InterfaceClassGuid;

		public uint Flags;

		public IntPtr Reserved;

		/// <summary>
		/// <see cref="SP_DEVICE_INTERFACE_DATA"/> 结构大小
		/// </summary>
		public readonly static uint Size = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

		public static SP_DEVICE_INTERFACE_DATA Create()
		{
			return new SP_DEVICE_INTERFACE_DATA()
			{
				cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA)),
			};
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		public uint cbSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DevicePath;

		/// <summary>
		/// <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> 结构大小
		/// </summary>
		public readonly static uint Size = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));


		public static SP_DEVICE_INTERFACE_DETAIL_DATA Create()
		{
			return new SP_DEVICE_INTERFACE_DETAIL_DATA()
			{
				cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA)),
			};
		}

		/// <summary>
		/// @ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"cbSize:{cbSize}, DevicePath:{DevicePath}";
		}

	}

	public struct SP_DEVINFO_DATA
	{
		public uint cbSize;

		public Guid ClassGuid;

		public uint DevInst;

		public IntPtr Reserved;

		/// <summary>
		/// <see cref="SP_DEVINFO_DATA"/> 结构大小
		/// </summary>
		public readonly static uint Size = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

	}

	
	#endregion


	#region Deletages
	#endregion


	#region Notifications
	#endregion


	public static partial class SetupAPI
    {
		
	}
}
