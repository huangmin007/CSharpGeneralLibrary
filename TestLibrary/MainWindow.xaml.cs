using SpaceCG.Log4Net.Controls;
using System.Windows;
using System;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Media;
using HidSharp;
using System.Linq;
using HidSharp.Reports.Encodings;
using HidSharp.Reports;
using SpaceCG.WindowsAPI.User32;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SpaceCG.WindowsAPI.Kernel32;
using SpaceCG.WindowsAPI;

namespace TestLibrary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MainWindow));

        private IntPtr handle;
        private HwndSource hwndSource;

        public MainWindow()
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            int tier = RenderCapability.Tier >> 16;
            Console.WriteLine("Tier:{0}", tier);
#endif
            InitializeComponent();
            Log.InfoFormat("MainWindow.");
        }

        protected override void OnInitialized(EventArgs e)
        {
            LoggerWindow LoggerWindow = new LoggerWindow();
            LoggerWindow.Show();

            base.OnInitialized(e);
            App.Log.InfoFormat("Initialize.");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(hwndSource != null)
            {
                hwndSource.RemoveHook(WindowRawInputHandler);
                hwndSource.Dispose();
            }
        }

        private void PrintfHidDeviceInfo()
        {
            HidDevice[] devices = DeviceList.Local.GetHidDevices().ToArray();

            foreach (HidDevice dev in devices)
            {
                Console.WriteLine("{0}, {1}", dev, dev.DevicePath);

                try
                {
                    Console.WriteLine(string.Format("Max Lengths: Input {0}, Output {1}, Feature {2}", dev.GetMaxInputReportLength(), dev.GetMaxOutputReportLength(), dev.GetMaxFeatureReportLength()));
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e);
                    continue;
                }

                try
                {
                    var rawReportDescriptor = dev.GetRawReportDescriptor();
                    Console.WriteLine("Report Descriptor:");
                    Console.WriteLine("  {0} ({1} bytes)", string.Join(" ", rawReportDescriptor.Select(d => d.ToString("X2"))), rawReportDescriptor.Length);

                    int indent = 0;
                    foreach (var element in EncodedItem.DecodeItems(rawReportDescriptor, 0, rawReportDescriptor.Length))
                    {
                        if (element.ItemType == ItemType.Main && element.TagForMain == MainItemTag.EndCollection) { indent -= 2; }
                        Console.WriteLine("  {0}{1}", new string(' ', indent), element);
                        if (element.ItemType == ItemType.Main && element.TagForMain == MainItemTag.Collection) { indent += 2; }
                    }

                    var reportDescriptor = dev.GetReportDescriptor();

                    foreach (var deviceItem in reportDescriptor.DeviceItems)
                    {
                        foreach (var usage in deviceItem.Usages.GetAllValues())
                            Console.WriteLine(string.Format("Usage: {0:X4} {1}", usage, (Usage)usage));

                        foreach (var report in deviceItem.Reports)
                        {
                            Console.WriteLine(string.Format("{0}: ReportID={1}, Length={2}, Items={3}",
                                                report.ReportType, report.ReportID, report.Length, report.DataItems.Count));
                            foreach (var dataItem in report.DataItems)
                            {
                                Console.WriteLine(string.Format("  {0} Elements x {1} Bits, Units: {2}, Expected Usage Type: {3}, Flags: {4}, Usages: {5}",
                                    dataItem.ElementCount, dataItem.ElementBits, dataItem.Unit.System, dataItem.ExpectedUsageType, dataItem.Flags,
                                    string.Join(", ", dataItem.Usages.GetAllValues().Select(usage => usage.ToString("X4") + " " + ((Usage)usage).ToString()))));
                            }
                        }

                        //tryOpen
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:::{0}", ex);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handle = new WindowInteropHelper(this).Handle;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(WindowRawInputHandler);

            //PrintfHidDeviceInfo();
            //Task.Run(() =>
            //{
                GetRawInputDeviceList();
            //});
            return;
            //0x01 0x06 键盘
            //0x01 0x02 鼠标
            //13, 4 SiS HID Touch
            //65280, 1 SiS HID Touch
            RAWINPUTDEVICE rawInputDevice = new RAWINPUTDEVICE()
            {
                usUsagePage = 0x01, //13,//0x01,
                usUsage = 0x02, //4, //0x02,
                dwFlags = RawInputFlags.RIDEV_INPUTSINK,
                hwndTarget = handle,
            };
            RAWINPUTDEVICE[] rawInputDevices = new RAWINPUTDEVICE[1] { rawInputDevice };
            if (User32.RegisterRawInputDevices(rawInputDevices, (uint)rawInputDevices.Length, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
                Console.WriteLine("注册原始输入数据的设备成功");
            else
                Console.WriteLine("注册原始输入数据的设备失败");
        }

        readonly uint cbSize = (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));
        ConcurrentDictionary<IntPtr, string> devicesName = new ConcurrentDictionary<IntPtr, string>();

        protected IntPtr WindowRawInputHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            if(msgType == MessageType.WM_INPUT_DEVICE_CHANGE)
            {
                Console.WriteLine("Change........");
                return IntPtr.Zero;
            }
            if (msgType != MessageType.WM_INPUT) return IntPtr.Zero;

            Console.WriteLine(User32.GET_RAWINPUT_CODE_WPARAM(wParam));
            Task.Run(()=>
            {
                uint dwSize = 128;
                //buffer = Marshal.AllocHGlobal((int)dwSize);
                //int result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, buffer, ref dwSize, cbSize);

                RAWINPUT raw = new RAWINPUT();
                //int result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, ref raw, ref dwSize, cbSize);
                int result = User32.GetRawInputData(lParam, RIDFlag.RID_HEADER, ref raw, ref dwSize, cbSize);

                Console.WriteLine("result 1:: {0} {1}", result, dwSize);
                string message = Kernel32Utils.GetSysErrroMessage("GetRawInputData");
                Console.WriteLine(message);

                if (result > 0)
                {
                    //RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                    string name = null;
                    if (!devicesName.TryGetValue(raw.header.hDevice, out name))
                    {
                        name = GetRawInputDeviceName(raw.header.hDevice);
                        if (!string.IsNullOrWhiteSpace(name)) devicesName.TryAdd(raw.header.hDevice, name);
                    }

                    POINT lpPoint = new POINT();
                    User32.GetCursorPos(ref lpPoint);
                    Console.WriteLine("{0} {1}", name, lpPoint);
                }
            });
            handled = true;
            return IntPtr.Zero;

            IntPtr buffer = IntPtr.Zero;
            try
            {
#if true
                uint dwSize = 128;
                //buffer = Marshal.AllocHGlobal((int)dwSize);
                //int result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, buffer, ref dwSize, cbSize);

                RAWINPUT raw = new RAWINPUT();
                int result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, ref raw, ref dwSize, cbSize);

                Console.WriteLine("result 1:: {0} {1}", result, dwSize);
                string message = Kernel32Utils.GetSysErrroMessage("GetRawInputData");
                Console.WriteLine(message);

                if (result > 0)
                {
                    //RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

                    string name = null;
                    if (!devicesName.TryGetValue(raw.header.hDevice, out name))
                    {
                        name = GetRawInputDeviceName(raw.header.hDevice);
                        if (!string.IsNullOrWhiteSpace(name)) devicesName.TryAdd(raw.header.hDevice, name);
                    }

                    POINT lpPoint = new POINT();
                    User32.GetCursorPos(ref lpPoint);
                    Console.WriteLine("{0} {1}", name, lpPoint);
#if false
                    if (raw.header.dwType == RawInputType.RIM_TYPEMOUSE)
                    {
                        Console.WriteLine("{0} {1}", raw.header.hDevice, raw.mouse);
                    }
                    else if(raw.header.dwType == RawInputType.RIM_TYPEHID)
                    {
                        Console.WriteLine("{0} {1}", raw.header.hDevice, raw.hid);
                        byte[] destination = raw.hid.GetRawData();
                        //byte[] destination = new byte[raw.hid.dwSizeHid * raw.hid.dwCount];
                        //Marshal.Copy(raw.hid.bRawData, destination, 0, destination.Length);

                        for(int i = 0; i < destination.Length; i ++)
                            Console.Write("{0:X2} ", destination[i]);
                        Console.WriteLine();
                    }
#endif

                }
#else
                    uint dwSize = 0;
                int result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, IntPtr.Zero, ref dwSize, cbSize);
                
                Console.WriteLine("result 1:: {0} {1}", result, dwSize);
                string message = Kernel32Utils.GetSysErrroMessage("GetRawInputData");
                Console.WriteLine(message);

                if (result == 0)
                {
                    buffer = Marshal.AllocHGlobal((int)dwSize);
                    result = User32.GetRawInputData(lParam, RIDFlag.RID_INPUT, buffer, ref dwSize, cbSize);
                    if (result == dwSize)
                    {
                        RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                        if (raw.header.dwType == RawInputType.RIM_TYPEMOUSE)
                        {
                            Console.WriteLine("{0} {1}", raw.header.hDevice, raw.mouse);
                        }

                        string name = null;
                        if (devicesName.TryGetValue(raw.header.hDevice, out name))
                        {
                            Console.WriteLine(name);
                        }
                        else
                        {
                            name = GetRawInputDeviceName(raw.header.hDevice);
                            if (name != null) devicesName.TryAdd(raw.header.hDevice, name);
                        }
                    }
                }
#endif      
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                handled = true;
                if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer);
            }

            return IntPtr.Zero;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Log.Error("aaa", new Exception("Exception,Exception,Exception"));
        }

        public static string GetRawInputDeviceName(IntPtr hDevice)
        {
            if (hDevice == IntPtr.Zero) return null;
            uint pcbSize = 0;
            int result = User32.GetRawInputDeviceInfo(hDevice, RIDIFlag.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
            Console.WriteLine("result 1: {0} {1}", result, pcbSize);
            if (result != 0) return null;

            IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
            result = User32.GetRawInputDeviceInfo(hDevice, RIDIFlag.RIDI_DEVICENAME, pData, ref pcbSize);
            Console.WriteLine("result 2: {0} {1}", result, pcbSize);
            if (result != pcbSize) return null;

            string name = Marshal.PtrToStringAuto(pData, (int)pcbSize);
            Console.WriteLine(name);
            return name;
        }

        protected static void GetRawInputDeviceList()
        {
            uint deviceCount = 0;
            uint cbSize = (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST));
            int result = User32.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, cbSize);
            Console.WriteLine("result 1: {0} {1} {2}", result, deviceCount, cbSize);

            if (result >= 0)
            {
                IntPtr devicesList = Marshal.AllocHGlobal((int)(deviceCount * cbSize));
                result = User32.GetRawInputDeviceList(devicesList, ref deviceCount, cbSize);
                //Console.WriteLine("result 2:{0}", result);

                if (result == deviceCount)
                {
                    for (int i = 0; i < deviceCount; i++)
                    {
                        RAWINPUTDEVICELIST rid = Marshal.PtrToStructure<RAWINPUTDEVICELIST>(IntPtr.Add(devicesList, (int)(cbSize * i)));
                        //RAWINPUTDEVICELIST rid = Marshal.PtrToStructure<RAWINPUTDEVICELIST>(new IntPtr(devicesList.ToInt32() + (cbSize * i)));

                        IntPtr pData = IntPtr.Zero;
                        try
                        {
                            Console.WriteLine("index:{0}", rid);

                            uint pcbSize = 0;
                            result = User32.GetRawInputDeviceInfo(rid.hDevice, RIDIFlag.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
                            Console.WriteLine("result 3: {0} {1}", result, pcbSize);
                            if (result != 0) return;

                            pData = Marshal.AllocHGlobal((int)pcbSize * 2);
                            result = User32.GetRawInputDeviceInfo(rid.hDevice, RIDIFlag.RIDI_DEVICENAME, pData, ref pcbSize);
                            //Console.WriteLine("result 4: {0} {1}", result, pcbSize);
                            string name = Marshal.PtrToStringAuto(pData, (int)pcbSize);
                            Console.WriteLine(name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            if (pData != IntPtr.Zero)
                                Marshal.FreeHGlobal(pData);
                        }
                    }
                }
                Thread.Sleep(500);
                //Marshal.FreeHGlobal(devicesList);
                Console.WriteLine("Complete...");
            }
        }

    }
}



