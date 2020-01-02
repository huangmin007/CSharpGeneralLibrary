using SpaceCG.WindowsAPI.WinUser;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SpaceCG.Extension
{
    /// <summary>
    /// SerialPort 串口 扩展/实用/通用 函数
    /// </summary>
    public static class SerialPortExtension
    {
        /// <summary>
        /// 串口 Serial_ErrorReceived 事件的错误描述信息
        /// </summary>
        public static readonly Dictionary<SerialError, string> SerialErrorDescriptions = new Dictionary<SerialError, string>(8)
        {
            { SerialError.RXOver,   "发生输入缓冲区溢出，输入缓冲区空间不足，或在文件尾 (EOF) 字符之后接收到字符"},
            { SerialError.Overrun,  "发生字符缓冲区溢出，下一个字符将丢失" },
            { SerialError.RXParity, "硬件检测到奇偶校验错误"},
            { SerialError.Frame,    "硬件检测到一个组帧错误"},
            { SerialError.TXFull,   "应用程序尝试传输一个字符，但是输出缓冲区已满"},
        };


        /// <summary>
        /// 根据构造函数配置，创建一个串口实例，并简单设置/监听相关参数。如果只关心数据的接收/处理，适用此方法，其它事件状态会记录在日志中。
        /// <para>建议使用 <see cref="CloseAndDispose(ref SerialPort, log4net.ILog)"/> 清理该实例。</para>
        /// </summary>
        /// <param name="config">串口构造函数参数，参见 <see cref="SerialPort"/> 构造函数。
        ///     <para>参数顺序："portName, baudRate, Parity, dataBits, StopBits"，按顺序不得少于 1 个参数，如果只有一个参数时，第二个参数 baudRate 将默认为 9600。示例："COM3", "COM3,115200", "COM3,9600,0"</para>
        ///     <para>注意：如果构造函数参数不正确，或不规范，将抛出 <see cref="ArgumentNullException"/> 或 <see cref="ArgumentException"/> 异常信息。</para>
        /// </param>
        /// <param name="dataReceivedCallback">数据接收回调，为 null 时，不监听 <see cref="SerialPort.DataReceived"/> 事件。</param>
        /// <param name="receivedBytesThreshold">接收字节阀值，对象在收到这样长度的数据之后会触发事件 (<see cref="SerialPort.DataReceived"/>) 处理函数
        ///     <para>获取或设置 <see cref="SerialPort.DataReceived"/> 事件发生前内部输入缓冲区中的字节数。</para>
        ///     <para><see cref="SerialPort.DataReceived"/> 事件触发前内部输入缓冲区中的字节数；默认值为 1，可跟据数据设计大小定义。</para>
        /// </param>
        /// <param name="ignoreOpenError">是否忽略由 <see cref="SerialPort.Open()"/> 产生的异常，为监听设备热插拔自动重新连接做准备，参见：<see cref="AutoReconnection(SerialPort, log4net.ILog)"/>。
        ///     <para>如果为 true 则产生的异常信息由 Log(如果有) 记录，否则会抛出异常信息。</para>
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns> 返回串口对象 </returns>
        public static SerialPort CreateInstance(string config, Action<SerialPort, byte[]> dataReceivedCallback, int receivedBytesThreshold = 1, bool ignoreOpenError = true)
        {
            SpaceCGUtils.Log.InfoFormat("Create SerialPort Instance : {0}", config);
            if (String.IsNullOrWhiteSpace(config)) throw new ArgumentNullException("串口构造函数参数不能为空");

            SerialPort serialPort;
            string[] cfg = config.Replace(" ", "").Split(',');

            //Create Instance
            if (cfg.Length == 1)
                serialPort = new SerialPort(cfg[0].ToUpper(), 9600);
            else if (cfg.Length == 2)
                serialPort = new SerialPort(cfg[0].ToUpper(), int.Parse(cfg[1]));
            else if (cfg.Length == 3)
                serialPort = new SerialPort(cfg[0].ToUpper(), int.Parse(cfg[1]), (Parity)Enum.Parse(typeof(Parity), cfg[2]));
            else if (cfg.Length == 4)
                serialPort = new SerialPort(cfg[0].ToUpper(), int.Parse(cfg[1]), (Parity)Enum.Parse(typeof(Parity), cfg[2]), int.Parse(cfg[3]));
            else if (cfg.Length == 5)
                serialPort = new SerialPort(cfg[0].ToUpper(), int.Parse(cfg[1]), (Parity)Enum.Parse(typeof(Parity), cfg[2]), int.Parse(cfg[3]), (StopBits)Enum.Parse(typeof(StopBits), cfg[4]));
            else
                throw new ArgumentException(nameof(config), $"串口参考配置错误:[{config}]");

            //默认配置参数
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;
            serialPort.Handshake = Handshake.None;
            serialPort.ReceivedBytesThreshold = receivedBytesThreshold;

            //事件监听处理
            if (dataReceivedCallback != null)
            {
                serialPort.DataReceived += (s, e) =>
                {
                    if (SpaceCGUtils.Log.IsDebugEnabled)
                        SpaceCGUtils.Log.DebugFormat("SerialPort_DataReceived  EventType:{0}  BytesToRead:{1}", e.EventType, serialPort.BytesToRead);

                    int length = -1;
                    byte[] buffer = new byte[serialPort.BytesToRead];

                    try
                    {
                        length = serialPort.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        SpaceCGUtils.Log.Error("SerialPort_DataReceived", ex);
                    }

                    if (buffer.Length != length)
                        SpaceCGUtils.Log.WarnFormat("串口数据接收不完整，有丢失数据 {0} byte", buffer.Length - length);

                    if (length > 0) dataReceivedCallback?.Invoke(serialPort, buffer);
                };
            }

            serialPort.PinChanged += (s, e) => SpaceCGUtils.Log.InfoFormat("PinChanged::{0}", e.EventType);
            serialPort.ErrorReceived += (s, e) => SpaceCGUtils.Log.ErrorFormat($"串口上发生错误：[{e.EventType}:{(int)e.EventType}], Description：{SerialErrorDescriptions[e.EventType]}");

            //SerialPort Open
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                if (ignoreOpenError)
                    SpaceCGUtils.Log.ErrorFormat("CreateInstance Error:{0}", ex);
                else
                    throw ex;
            }

            return serialPort;
        }
        

        /// <summary>
        /// 串口设备热插拔自动重新连接
        /// <para>使用 ManagementEventWatcher WQL 事件监听模式</para>
        /// </summary>
        /// <param name="serialPort"></param>
        public static void AutoReconnection(this SerialPort serialPort)
        {
            if (serialPort == null) throw new ArgumentException("参数不能为空");
            SpaceCGUtils.Log.InfoFormat("ManagementEventWatcher WQL Event Listen SerialPort Name:{0}", serialPort.PortName);

            TimeSpan withinInterval = TimeSpan.FromSeconds(1);
            string wql_condition = $"TargetInstance isa 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%({serialPort.PortName.ToUpper()})'";

            ManagementScope scope = new ManagementScope(@"\\.\Root\CIMV2")
            {
                Options = new ConnectionOptions() { EnablePrivileges = true },
            };

            ManagementEventWatcher CreationEvent = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceCreationEvent", withinInterval, wql_condition));
            CreationEvent.EventArrived += (s, e) =>
            {
                if (!serialPort.IsOpen) serialPort.Open();
                SpaceCGUtils.Log.InfoFormat("Instance Creation Event SerialPort Name:{0}", serialPort.PortName);
            };
            ManagementEventWatcher DeletionEvent = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceDeletionEvent", withinInterval, wql_condition));
            DeletionEvent.EventArrived += (s, e) =>
            {
                if (serialPort.IsOpen) serialPort.Close();
                SpaceCGUtils.Log.ErrorFormat("Instance Deletion Event SerialPort Name:{0}", serialPort.PortName);
            };

            CreationEvent.Start();
            DeletionEvent.Start();
        }

        /// <summary>
        /// 串口设备热插拔自动重新连接
        /// <para>使用 HwndSource Hook Window Message #WM_DEVICECHANGE 事件监听模式</para>
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="window">IsLoaded 为 True 的窗口对象</param>
        public static void AutoReconnection(this SerialPort serialPort, System.Windows.Window window)
        {
            if (serialPort == null || window == null) throw new ArgumentException("参数不能为空");
            if (!window.IsLoaded) throw new InvalidOperationException("Window 对象 IsLoaded 为 True 时才能获取窗口句柄");
            SpaceCGUtils.Log.InfoFormat("HwndSource Hook Window Message #WM_DEVICECHANGE Event Listen SerialPort Name:{0}", serialPort.PortName);

            HwndSource hwndSource = HwndSource.FromVisual(window) as HwndSource;
            if (hwndSource != null) hwndSource.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                MessageType mt = (MessageType)msg;
                if (mt != MessageType.WM_DEVICECHANGE) return IntPtr.Zero;

                DeviceBroadcastType dbt = (DeviceBroadcastType)wParam.ToInt32();
                if (dbt == DeviceBroadcastType.DBT_DEVICEARRIVAL || dbt == DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE)
                {
                    DEV_BROADCAST_HDR hdr = Marshal.PtrToStructure<DEV_BROADCAST_HDR>(lParam);
                    if (hdr.dbch_devicetype != DeviceType.DBT_DEVTYP_PORT) return IntPtr.Zero;

                    DEV_BROADCAST_PORT port = Marshal.PtrToStructure<DEV_BROADCAST_PORT>(lParam);
                    if (port.dbcp_name.ToUpper() != serialPort.PortName.ToUpper()) return IntPtr.Zero;

                    if (dbt == DeviceBroadcastType.DBT_DEVICEARRIVAL)
                    {
                        if (!serialPort.IsOpen) serialPort.Open();
                        SpaceCGUtils.Log.InfoFormat("Device Arrival SerialPort Name:{0}", port.dbcp_name);
                    }
                    if (dbt == DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE)
                    {
                        if (serialPort.IsOpen) serialPort.Close();
                        SpaceCGUtils.Log.ErrorFormat("Device Remove Complete SerialPort Name:{0}", port.dbcp_name);
                    }

                    handled = true;
                }
                
                return IntPtr.Zero;
            });

            window.Closing += (s, e) =>
            {
                hwndSource?.Dispose();
                hwndSource = null;
            };
        }


        /// <summary>
        /// 关闭并清理 由 <see cref="CreateInstance"/> 创建的串口实例
        /// <para>注意：静态函数，引用参数 serialPort, 该方法会将 实例变量 设为 null </para>
        /// </summary>
        /// <param name="serialPort"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CloseAndDispose(ref SerialPort serialPort)
        {
            SpaceCGUtils.Log.InfoFormat("Close And Dispose Serial Port.");
            if (serialPort == null) throw new ArgumentNullException("参数不能为空");

            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "PinChanged");
            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "DataReceived");
            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "ErrorReceived");

            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                SpaceCGUtils.Log.ErrorFormat("关闭并清理串口时产生异常：{0}", ex);
            }

            serialPort.Dispose();
            serialPort = null;
        }

        /// <summary>
        /// 关闭并清理 由 <see cref="CreateInstance"/> 创建的串口实例
        /// <para>注意：扩展函数，需手动将实例变量设为 null</para>
        /// </summary>
        /// <param name="serialPort"></param>
        public static void CloseAndDispose(this SerialPort serialPort)
        {
            CloseAndDispose(ref serialPort);
        }

        

    }
}
