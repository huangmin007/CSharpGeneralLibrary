﻿using System;
using SpaceCG.WindowsAPI.WinUser;
using System.Runtime.InteropServices;

namespace SpaceCG.WindowsAPI.DBT
{

    /// <summary>
    /// Device Broadcast Type
    /// <para><see cref="MessageType.WM_DEVICECHANGE"/> wParam Data, device-change event</para>
    /// <para>参考：C:\Program Files (x86)\Windows Kits\10\Include\10.0.18362.0\um  Dbt.h </para>
    /// </summary>
    public enum DeviceBroadcastType
    {
        /// <summary>
        /// appy begin. lParam  = (not used)
        /// </summary>
        DBT_APPYBEGIN = 0x0000,
        /// <summary>
        /// appy end. lParam  = (not used)
        /// </summary>
        DBT_APPYEND = 0x0001,
        /// <summary>
        /// 当 configmg 完成进程树批处理时发送. lParam  = 0
        /// </summary>
        DBT_DEVNODES_CHANGED = 0x0007,
        /// <summary>
        /// sent to ask if a config change is allowed. lParam  = 0
        /// </summary>
        DBT_QUERYCHANGECONFIG = 0x0017,
        /// <summary>
        /// sent when a config has changed, lParam  = 0
        /// </summary>
        DBT_CONFIGCHANGED = 0x0018,
        /// <summary>
        /// someone cancelled the config change, lParam  = 0
        /// </summary>
        DBT_CONFIGCHANGECANCELED = 0x0019,
        /// <summary>
        /// this message is sent when the display monitor has changed and the system should change the display mode to match it.
        /// <para>lParam  = new resolution to use (LOWORD=x, HIWORD=y) if 0, use the default res for current config</para>
        /// </summary>
        DBT_MONITORCHANGE = 0x001B,
        /// <summary>
        /// The shell has finished login on: VxD can now do Shell_EXEC.  lParam  = 0
        /// </summary>
        DBT_SHELLLOGGEDON = 0x0020,
        /// <summary>
        /// lParam  = CONFIGMG API Packet, CONFIGMG ring 3 call.
        /// </summary>
        DBT_CONFIGMGAPI32 = 0x0022,
        /// <summary>
        /// CONFIGMG ring 3 call. lParam  = 0
        /// </summary>
        DBT_VXDINITCOMPLETE = 0x0023,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKQUERYLOCK = 0x8041,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKLOCKTAKEN = 0x8042,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKLOCKFAILED = 0x8043,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKQUERYUNLOCK = 0x8044,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKLOCKRELEASED = 0x8045,
        /// <summary>
        /// Message = WM_DEVICECHANGE, wParam  = DBT_VOLLOCK*, lParam  = pointer to VolLockBroadcast structure described below.
        /// <para>Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE. All these messages pass a pointer to a struct which has no pointers.</para>
        /// </summary>
        DBT_VOLLOCKUNLOCKFAILED = 0x8046,
        /// <summary>
        /// Message issued by IFS manager when it detects that a drive is run out of free space. lParam = drive number of drive that is out of disk space (1-based)
        /// </summary>
        DBT_NO_DISK_SPACE = 0x0047,
        /// <summary>
        /// lParam  = drive number of drive that is low on disk space (1-based)
        /// </summary>
        DBT_LOW_DISK_SPACE = 0x0048,
        /// <summary>
        /// configmg private
        /// </summary>
        DBT_CONFIGMGPRIVATE = 0x7FFF,
        /// <summary>
        /// system detected a new device
        /// </summary>
        DBT_DEVICEARRIVAL = 0x8000,
        /// <summary>
        /// wants to remove, may fail
        /// </summary>
        DBT_DEVICEQUERYREMOVE = 0x8001,
        /// <summary>
        /// removal aborted
        /// </summary>
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
        /// <summary>
        /// Device Move Complete
        /// </summary>
        DBT_DEVICEREMOVECOMPLETE = 0x8004,
        /// <summary>
        /// type specific event
        /// </summary>
        DBT_DEVICETYPESPECIFIC = 0x8005,
        /// <summary>
        /// user-defined event
        /// </summary>
        DBT_CUSTOMEVENT = 0x8006,
        /// <summary>
        /// (WIN7) system detected a new device
        /// </summary>
        DBT_DEVINSTENUMERATED = 0x8007,
        /// <summary>
        /// (WIN7) device installed and started
        /// </summary>
        DBT_DEVINSTSTARTED = 0x8008,
        /// <summary>
        /// (WIN7) device removed from system
        /// </summary>
        DBT_DEVINSTREMOVED = 0x8009,
        /// <summary>
        /// (WIN7) a property on the device changed
        /// </summary>
        DBT_DEVINSTPROPERTYCHANGED = 0x800A,
        /// <summary>
        /// User defined
        /// </summary>
        DBT_USERDEFINED = 0xFFFF,
    }

    /// <summary>
    /// <see cref="DEV_BROADCAST_HDR"/> 结构体字段 <see cref="DEV_BROADCAST_HDR.dbch_devicetype"/> 的值之一
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/dbt/ns-dbt-dev_broadcast_hdr </para>
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// oem-defined device type, OEM 或 IHV 定义的设备类型。此结构是 <see cref="DEV_BROADCAST_OEM"/> 结构。
        /// </summary>
        DBT_DEVTYP_OEM = 0x00000000,
        /// <summary>
        /// devnode number
        /// </summary>
        DBT_DEVTYP_DEVNODE = 0x00000001,
        /// <summary>
        /// logical volume, 逻辑卷。此结构是 <see cref="DEV_BROADCAST_VOLUME"/> 结构。
        /// </summary>
        DBT_DEVTYP_VOLUME = 0x00000002,
        /// <summary>
        /// serial/parallel, 端口设备（串行或并行）。此结构是 <see cref="DEV_BROADCAST_PORT"/> 结构。
        /// </summary>
        DBT_DEVTYP_PORT = 0x00000003,
        /// <summary>
        /// network resource
        /// </summary>
        DBT_DEVTYP_NET = 0x00000004,
        /// <summary>
        /// device interface class, 设备类别。此结构是 <see cref="DEV_BROADCAST_DEVICEINTERFACE"/> 结构。
        /// </summary>
        DBT_DEVTYP_DEVICEINTERFACE = 0x00000005,
        /// <summary>
        /// file system handle, 文件系统句柄。此结构是 <see cref="DEV_BROADCAST_HANDLE"/> 结构。
        /// </summary>
        DBT_DEVTYP_HANDLE = 0x00000006,
        /// <summary>
        /// device instance
        /// </summary>
        DBT_DEVTYP_DEVINST = 0x00000007,
    }


    /// <summary>
    /// 用作与通过 <see cref="MessageType.WM_DEVICECHANGE"/> 消息报告的设备事件相关的信息的标准标头 。
    /// <para>WM_DEVICECHANGE lParam Data, event-specific data</para>
    /// <para>由于此结构包含可变长度字段，因此可以将其用作创建指向用户定义结构的指针的模板。请注意，该结构不得包含指针。示：<see cref="DEV_BROADCAST_USERDEFINED"/>, <see cref="DEV_BROADCAST_PORT"/>, <see cref="DEV_BROADCAST_VOLUME"/>, <see cref="DEV_BROADCAST_OEM"/> 等</para>
    /// <para> DEV_BROADCAST_HDR 结构的成员 包含在每个设备管理结构中。要确定您通过 WM_DEVICECHANGE 接收到的结构，请将其视为 DEV_BROADCAST_HDR 结构并检查其 dbch_devicetype 成员。</para>
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/dbt/ns-dbt-dev_broadcast_hdr </para>
    /// </summary>
    public struct DEV_BROADCAST_HDR
    {
        /// <summary>
        /// 此结构的大小，以字节为单位。
        /// <para>如果这是用户定义的事件，则此成员必须是此标头的大小，加上 _DEV_BROADCAST_USERDEFINED 结构中的可变长度数据的 大小。</para>
        /// </summary>
        public int dbch_size;
        /// <summary>
        /// 设备类型，确定跟随前三个成员的事件特定信息。
        /// </summary>
        public DeviceType dbch_devicetype;
        /// <summary>
        /// 保留，不使用。
        /// </summary>
        public int dbch_reserved;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{size:{dbch_size}, type:{dbch_devicetype}({(int)dbch_devicetype})}}";
        }
    }

    /// <summary>
    /// 包含用户定义的事件以及与 DBT_USERDEFINED 设备事件关联的可选数据 。
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/dbt/ns-dbt-_dev_broadcast_userdefined </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEV_BROADCAST_USERDEFINED
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息 。因为 DEV_BROADCAST_USERDEFINED 是可变长度，则mdbch_sizem所述的构件mdbud_dbhm结构必须在整个结构的字节数，包括可变长度部分的大小。
        /// </summary>
        public DEV_BROADCAST_HDR dbud_head;
        /// <summary>
        /// 指向区分大小写，以空字符结尾的字符串的指针，该字符串命名消息。该字符串必须由供应商名称，反斜杠和后面的任意用户定义的以空字符结尾的文本组成。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string dbud_szName;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbud_head}, szName:{dbud_szName}";
        }
    };


    /// <summary>
    /// 包含有关调制解调器，串行或并行端口的信息。
    /// <para>DEV_BROADCAST_PORT_A, * PDEV_BROADCAST_PORT_A;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/dbt/ns-dbt-dev_broadcast_port_a </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEV_BROADCAST_PORT
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息。
        /// </summary>
        public DEV_BROADCAST_HDR dbcp_head;
        /// <summary>
        /// 以空值结尾的字符串，用于指定端口或连接到该端口的设备的友好名称。友好名称旨在帮助用户快速准确地识别设备-例如，"COM1" 和 "Standard 28800 bps Modem" 被视为友好名称。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string dbcp_name;        
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbcp_head}, name:{dbcp_name}";
        }
    }


    /// <summary>
    /// 包含有关逻辑卷的信息。
    /// <para>尽管dbcv_unitmask成员可以在任何消息中指定多个卷，但这不能保证为指定事件仅生成一个消息。多个系统功能部件可以同时独立地为逻辑卷生成消息。</para>
    /// <para>仅在支持软弹出机制的设备中为媒体发送用于媒体到达和删除的消息。例如，应用程序将看不到软盘的与介质相关的卷消息。每当发出网络命令时，就不会发送网络驱动器到达和卸下的消息，而是当网络连接由于硬件事件而消失时发送。</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/dbt/ns-dbt-dev_broadcast_volume </para>
    /// </summary>
    public struct DEV_BROADCAST_VOLUME
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息。
        /// </summary>
        public DEV_BROADCAST_HDR dbcv_head;
        /// <summary>
        /// 逻辑单元掩码标识一个或多个逻辑单元。掩码中的每一位对应一个逻辑驱动器。位0代表驱动器A，位1代表驱动器B，依此类推。
        /// </summary>
        public int dbcv_unitmask;
        /// <summary>
        /// 此参数可以是下列值之一。
        /// <para>DBTF_MEDIA  0x0001 更改会影响驱动器中的介质。如果未设置，则更改会影响物理设备或驱动器。</para>
        /// <para>DBTF_NET    0x0002  指示的逻辑卷是网络卷。</para>
        /// </summary>
        public ushort dbcv_flags;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbcv_head}, mask:{dbcv_unitmask}, flags:{dbcv_flags}";
        }
    }


    /// <summary>
    /// 包含有关OEM定义的设备类型的信息。
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/dbt/ns-dbt-dev_broadcast_oem </para>
    /// </summary>
    public struct DEV_BROADCAST_OEM
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息。
        /// </summary>
        public DEV_BROADCAST_HDR dbco_head;
        /// <summary>
        /// 设备的OEM特定标识符。
        /// </summary>
        public int dbco_identifier;
        /// <summary>
        /// OEM特定的功能值。可能的值取决于设备。
        /// </summary>
        public int dbco_suppfunc;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbco_head}, identifier:{dbco_identifier}, suppfunc:{dbco_suppfunc}";
        }
    }


    /// <summary>
    /// 包含有关一类设备的信息。
    /// <para>DEV_BROADCAST_DEVICEINTERFACE_A, * PDEV_BROADCAST_DEVICEINTERFACE_A;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/dbt/ns-dbt-dev_broadcast_deviceinterface_a </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEV_BROADCAST_DEVICEINTERFACE
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息。
        /// </summary>
        public DEV_BROADCAST_HDR dbcc_head;
        /// <summary>
        /// 接口设备类的GUID。
        /// </summary>
        public Guid dbcc_classguid;
        /// <summary>
        /// 以空值结尾的字符串，用于指定设备的名称。
        /// <para>通过 WM_DEVICECHANGE 消息将此结构返回到窗口时，dbcc_name 字符串将适当地转换为 ANSI。服务始终会收到 Unicode 字符串，无论它们调用 RegisterDeviceNotificationW 还是 RegisterDeviceNotificationA。</para>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string dbcc_name;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbcc_head}, guid:{dbcc_classguid}, name:{dbcc_name}";
        }
    }


    /// <summary>
    /// 包含有关文件系统句柄的信息。
    /// <para>DEV_BROADCAST_HANDLE, * PDEV_BROADCAST_HANDLE;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/dbt/ns-dbt-dev_broadcast_handle </para>
    /// </summary>
    public struct DEV_BROADCAST_HANDLE
    {
        /// <summary>
        /// 有关受 DEV_BROADCAST_HDR 结构指定的 WM_DEVICECHANGE 消息影响的设备的信息。
        /// </summary>
        public DEV_BROADCAST_HDR dbch_head;
        /// <summary>
        /// 要检查的设备的句柄。
        /// </summary>
        public IntPtr dbch_handle;
        /// <summary>
        /// 设备通知的句柄。该句柄由 RegisterDeviceNotification 返回 。
        /// </summary>
        public IntPtr dbch_hdevnotify;
        /// <summary>
        /// 自定义事件的GUID。有关更多信息，请参见 设备事件。仅对 DBT_CUSTOMEVENT 有效。
        /// </summary>
        public Guid dbch_eventguid;
        /// <summary>
        /// 可选字符串缓冲区的偏移量。仅对 DBT_CUSTOMEVENT 有效。
        /// </summary>
        public long dbch_nameoffset;
        /// <summary>
        /// 可选的二进制数据。该成员仅对 DBT_CUSTOMEVENT 有效。
        /// </summary>
        public byte dbch_data;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"head:{dbch_head}, handle:{dbch_handle}, guid:{dbch_eventguid}";
        }
    }


    /// <summary>
    /// Device Broadcast Type Define, Dbt.h
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/dbt/ </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/devio/device-management </para>
    /// </summary>
    public static class DBTDef
    {
    }
}
