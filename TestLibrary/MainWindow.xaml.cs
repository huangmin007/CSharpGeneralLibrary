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
using System.Text;

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

            //0x01 0x06 键盘
            //0x01 0x02 鼠标
            //13, 4 SiS HID Touch
            //65280, 1 SiS HID Touch
            RAWINPUTDEVICE mouseDevice = new RAWINPUTDEVICE()
            {
                usUsagePage = 0x01,
                usUsage = 0x02,
                dwFlags = RawInputFlags.RIDEV_INPUTSINK,
                hwndTarget = handle,
            };
            RAWINPUTDEVICE keyboardDevice = new RAWINPUTDEVICE()
            {
                usUsagePage = 0x01,
                usUsage = 0x06,
                dwFlags = RawInputFlags.RIDEV_INPUTSINK,
                hwndTarget = handle,
            };
            RAWINPUTDEVICE[] rawInputDevices = new RAWINPUTDEVICE[] { mouseDevice, keyboardDevice };
            if (User32.RegisterRawInputDevices(rawInputDevices, (uint)rawInputDevices.Length, RAWINPUTDEVICE.Size))
            {
                Console.WriteLine("注册原始输入数据的设备成功");
            }
            else
            {
                Console.WriteLine("注册原始输入数据的设备失败");
                Exception ex = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                Console.WriteLine("LastWin32Error Code:{0},{1}  Message:{2}", Marshal.GetLastWin32Error(), Marshal.GetHRForLastWin32Error(), ex.Message);
            }
        }

        //readonly uint cbSize = (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));
        //ConcurrentDictionary<IntPtr, string> devicesName = new ConcurrentDictionary<IntPtr, string>();

        static readonly IReadOnlyDictionary<IntPtr, String> Devices;
        static MainWindow()
        {
            Devices = User32Extension.GetRawInputDevicesName();
        }

        protected IntPtr WindowRawInputHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            if (msgType != MessageType.WM_INPUT) return IntPtr.Zero;

            RAWINPUTHEADER header = new RAWINPUTHEADER();
            if(!User32Extension.TryGetRawInputHeader(lParam, ref header))
            {
                Console.WriteLine("Try Get Raw Input Header Failed..");
                return IntPtr.Zero;
            }

            RAWINPUT data = new RAWINPUT();
            //if(User32Extension.TryGetRawInputData(lParam, ref data))
            {

            }
            if (User32Extension.TryGetRawInputData(lParam, ref data, ref header.dwSize))
            {
                Console.WriteLine(data);
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



