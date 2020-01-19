using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowsAPI.SetupAPI
{
	/// <summary>
	/// setupapi.dll
	/// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/setupapi/ </para>
	/// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/ff549791%28v%3dvs.85%29 </para>
	/// </summary>
	public static partial class SetupAPI
    {
		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern int SetupDiCreateDeviceInfoList(ref Guid ClassGuid, int hwndParent);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport(DLL_NAME, SetLastError = true)]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		/// <summary>
		/// 该函数返回的句柄，设备信息集，其中包含本地计算机请求的设备信息元素。
		/// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw </para>
		/// </summary>
		/// <param name="ClassGuid">指向设备设置类或设备接口类的 GUID 的指针。该指针是可选的，可以为 NULL。</param>
		/// <param name="Enumerator">指向以 NULL 结尾的字符串的指针，该字符串指定以下选项。该指针是可选的，可以为NULL。如果未使用枚举值选择设备，则将 Enumerator 设置为 NULL
		///		<para>1.即插即用（PnP）枚举器的标识符（ID）。该ID可以是值的全局唯一标识符（GUID）或符号名。例如，“ PCI”可用于指定PCI PnP值。PnP值的符号名称的其他示例包括“ USB”，“ PCMCIA”和“ SCSI”。</para>
		///		<para>2.PnP 设备实例ID。指定PnP设备实例ID时，必须在Flags参数中设置DIGCF_DEVICEINTERFACE。</para>
		/// </param>
		/// <param name="hwndParent">顶层窗口的句柄，用于与在设备信息集中安装设备实例相关联的用户界面。该句柄是可选的，可以为 NULL。</param>
		/// <param name="Flags"><see cref="DIGCF"/> 此参数可以是零个或多个以下标志的按位或。 </param>
		/// <returns></returns>
		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, DIGCF Flags);

		[DllImport(DLL_NAME, SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

		[DllImport(DLL_NAME, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);
	}
}
