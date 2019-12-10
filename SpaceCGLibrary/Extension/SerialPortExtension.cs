using SpaceCG.WindowsAPI.DBT;
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
        public static readonly Dictionary<SerialError, string> SerialErrorDescriptions = new Dictionary<SerialError, string>()
        {
            { SerialError.RXOver,   "发生输入缓冲区溢出，输入缓冲区空间不足，或在文件尾 (EOF) 字符之后接收到字符"},
            { SerialError.Overrun,  "发生字符缓冲区溢出，下一个字符将丢失" },
            { SerialError.RXParity, "硬件检测到奇偶校验错误"},
            { SerialError.Frame,    "硬件检测到一个组帧错误"},
            { SerialError.TXFull,   "应用程序尝试传输一个字符，但是输出缓冲区已满"},
        };

        /// <summary>
        /// 创建串口对象(根据配置)
        /// </summary>
        /// <param name="config">串口配置；示例："COM3,9600", "COM3,9600,0," 参数顺序："portName, baudRate, Parity, dataBits, StopBits"</param>
        /// <param name="receivedBytesThreshold">接收字节阀值，对象在收到这样长度的数据之后会触发事件处理函数
        /// <para>获取或设置 System.IO.Ports.SerialPort.DataReceived 事件发生前内部输入缓冲区中的字节数。</para>
        /// <para>System.IO.Ports.SerialPort.DataReceived 事件触发前内部输入缓冲区中的字节数，默认值为 1。</para>
        /// </param>
        /// <param name="Log">日志记录对象</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns> 返回一个根据配置设置好的串口对象 </returns>
        public static SerialPort Create(string config, int receivedBytesThreshold = 1, log4net.ILog Log = null)
        {
            Log?.InfoFormat("Create Serial Port : {0}", config);
            if (String.IsNullOrWhiteSpace(config)) throw new ArgumentNullException("串口配置参数不能为空");

            SerialPort serialPort;
            string[] cfg = config.Replace(" ", "").Split(',');

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
                throw new ArgumentException($"串口配置错误:[{config}]");

            //一般默认的配置参数
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;
            serialPort.Handshake = Handshake.None;
            serialPort.ReceivedBytesThreshold = receivedBytesThreshold;

            return serialPort;
        }

        /// <summary>
        /// 打开串口连接/数据接收，事件采用匿名函数。
        /// <para>请使用 CloseAndDispose 移除匿名事件、关闭/销毁串口</para>
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="receivedCallback"></param>
        /// <param name="ignoreError">忽略 SerialPort.Open() 异常，为监听设备热插拔自动重新连接做准备 </param>
        /// <param name="Log"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>SerialPort.Open() 成功，返回 true , 反之返回 false .</returns>
        public static bool OpenAndListen(this SerialPort serialPort, Action<byte[]> receivedCallback, bool ignoreError = true, log4net.ILog Log = null)
        {
            Log?.InfoFormat("Open And Listen Serial Port.");
            if (serialPort == null) throw new ArgumentNullException("串口对象 serialPort 不能为空");
            if (serialPort.IsOpen) throw new InvalidOperationException("串口对象 serialPort 已经打开，操作失败，数据接收回调设置失败");

            serialPort.DataReceived += (s, e) =>
            {
                if (Log != null && Log.IsDebugEnabled)
                    Log.DebugFormat("SerialPort_DataReceived  EventType:{0}  BytesToRead:{1}", e.EventType, serialPort.BytesToRead);

                int length = -1;
                byte[] buffer = new byte[serialPort.BytesToRead];

                try
                {
                    length = serialPort.Read(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Log?.Error("SerialPort_DataReceived", ex);
                }

                if (buffer.Length != length)
                    Log?.WarnFormat("串口数据接收不完整，有丢失数据 {0} byte", buffer.Length - length);

                if (length > 0) receivedCallback?.Invoke(buffer);
            };
            serialPort.ErrorReceived += (s, e) =>
            {
                Log?.ErrorFormat($"串口上发生错误：[{e.EventType}:{(int)e.EventType}], Description：{SerialErrorDescriptions[e.EventType]}");
            };
            serialPort.PinChanged += (s, e) => Log?.InfoFormat("PinChanged::{0}", e.EventType);
            try
            {
                serialPort.Open();
                return true;
            }
            catch(Exception ex)
            {
                if (!ignoreError) throw ex;
                return false;
            }
        }

        /// <summary>
        /// 设备热插拔自动重新连接
        /// <para>使用 ManagementEventWatcher WQL 事件监听模式</para>
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="Log"></param>
        public static void AutoReconnection(this SerialPort serialPort, log4net.ILog Log = null)
        {
            if (serialPort == null) throw new ArgumentException("参数不能为空");
            Log?.InfoFormat("ManagementEventWatcher WQL Event Listen SerialPort Name:{0}", serialPort.PortName);

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
                Log?.InfoFormat("Instance Creation Event SerialPort Name:{0}", serialPort.PortName);
            };
            ManagementEventWatcher DeletionEvent = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceDeletionEvent", withinInterval, wql_condition));
            DeletionEvent.EventArrived += (s, e) =>
            {
                if (serialPort.IsOpen) serialPort.Close();
                Log?.ErrorFormat("Instance Deletion Event SerialPort Name:{0}", serialPort.PortName);
            };

            CreationEvent.Start();
            DeletionEvent.Start();
        }

        /// <summary>
        /// 设备热插拔自动重新连接
        /// <para>使用 HwndSource Hook Window Message #WM_DEVICECHANGE 事件监听模式</para>
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="window">IsLoaded 为 True 的窗口对象</param>
        /// <param name="Log"></param>
        public static void AutoReconnection(this SerialPort serialPort, System.Windows.Window window, log4net.ILog Log = null)
        {
            if (serialPort == null || window == null) throw new ArgumentException("参数不能为空");
            if (!window.IsLoaded) throw new InvalidOperationException("Window 对象 IsLoaded 为 True 时才能获取窗口句柄");
            Log?.InfoFormat("HwndSource Hook Window Message #WM_DEVICECHANGE Event Listen SerialPort Name:{0}", serialPort.PortName);

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
                        Log?.InfoFormat("Device Arrival SerialPort Name:{0}", serialPort.PortName);
                    }
                    if (dbt == DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE)
                    {
                        if (serialPort.IsOpen) serialPort.Close();
                        Log?.ErrorFormat("Device Remove Complete SerialPort Name:{0}", serialPort.PortName);
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
        /// 关闭清理串口
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="Log">日志记录对象</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void CloseAndDispose(this SerialPort serialPort, log4net.ILog Log = null)
        {
            Log?.InfoFormat("Close And Dispose Serial Port.");
            if (serialPort == null) throw new InvalidOperationException("serialPort 对象未初使化，CloseAndDispose 失败");

            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "PinChanged");
            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "DataReceived");
            SpaceCGUtils.RemoveAnonymousEvents(serialPort, "ErrorReceived");

            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();

                serialPort.Close();
                serialPort.Dispose();
            }

            serialPort = null;
        }

    }
}
