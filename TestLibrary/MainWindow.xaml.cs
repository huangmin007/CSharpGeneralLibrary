using SpaceCG.Log4Net.Controls;
using System.Windows;
using System;
using System.ComponentModel;
using System.Windows.Interop;
using HidSharp;
using System.Linq;
using HidSharp.Reports.Encodings;
using HidSharp.Reports;
using SpaceCG.WindowsAPI.User32;
using System.Collections.Generic;
using HidSharp.Reports.Input;
using SpaceCG.WindowsAPI.GDI;
using System.Threading;

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

        static readonly IReadOnlyDictionary<IntPtr, String> Devices;
        static MainWindow()
        {
            Devices = User32Extension.GetRawInputDevicesName();
        }

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

            if (thread != null) thread.Abort();
        }

        DeviceItemInputParser InputParser;
        private void PrintfHidDeviceInfo()
        {
            HidDevice[] devices = DeviceList.Local.GetHidDevices(1111, 4755).ToArray();

            foreach (HidDevice dev in devices)
            {
                Console.WriteLine("-----------------------------");
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
                    foreach (EncodedItem element in EncodedItem.DecodeItems(rawReportDescriptor, 0, rawReportDescriptor.Length))
                    {
                        if (element.ItemType == ItemType.Main && element.TagForMain == MainItemTag.EndCollection) { indent -= 4; }
                        Console.WriteLine("  {0}{1}", new string(' ', indent), element);
                        if (element.ItemType == ItemType.Main && element.TagForMain == MainItemTag.Collection) { indent += 4; }
                    }

                    var reportDescriptor = dev.GetReportDescriptor();
                    foreach (DeviceItem deviceItem in reportDescriptor.DeviceItems)
                    {
                        if(InputParser == null)
                            InputParser = deviceItem.CreateDeviceItemInputParser();

                        foreach (var usage in deviceItem.Usages.GetAllValues())
                            Console.WriteLine(string.Format("Usage: {0:X4} {1}", usage, (Usage)usage));

                        foreach (var report in deviceItem.Reports)
                        {
                            Console.WriteLine(string.Format("{0}: ReportID={1}, Length={2}, Items={3}",
                                                report.ReportType, report.ReportID, report.Length, report.DataItems.Count));
                            foreach (DataItem dataItem in report.DataItems)
                            {
                                Console.WriteLine(string.Format("  {0} Elements x {1} Bits, Units: {2}, Expected Usage Type: {3}, Flags: {4}, Usages: {5}  TotalBits: {6}",
                                    dataItem.ElementCount, dataItem.ElementBits, dataItem.Unit.System, dataItem.ExpectedUsageType, dataItem.Flags,
                                    string.Join(", ", dataItem.Usages.GetAllValues().Select(usage => usage.ToString("X4") + " " + ((Usage)usage).ToString())), dataItem.TotalBits));
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
        
        private Thread thread;
        private void ThreadFunc()
        {
            Console.WriteLine("sta");
            SpaceCG.WindowsAPI.User32.MSG msg = new SpaceCG.WindowsAPI.User32.MSG();
            while(User32.GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                Console.WriteLine("Msg:{0}", msg.message);
            }
            Console.WriteLine("thr");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread = new Thread(ThreadFunc);
            thread.Start();

            var next = true;
            uint index = 0;
            Console.WriteLine(DISPLAY_DEVICE.Size);
            do
            {
                DISPLAY_DEVICE lpDisplay = new DISPLAY_DEVICE();
                lpDisplay.cb = DISPLAY_DEVICE.Size;
                lpDisplay.StateFlags = DisplayStateFlags.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP;

                var result = WinGDI.EnumDisplayDevices(null, index, ref lpDisplay, 0x00000001);
                
                if(result)
                {
                    var boo = WinGDI.EnumDisplayDevices(lpDisplay.DeviceName, 0, ref lpDisplay, 0x00000001);
                    if(boo)
                        Console.WriteLine(">>{0} {1}", index, lpDisplay);
                }

                if (result)
                {
                    index++;
                    next = true;
                }
                else
                {
                    next = false;
                }
            }
            while (next);

            handle = new WindowInteropHelper(this).Handle;
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(WindowRawInputHandler);

            /*
            //PrintfHidDeviceInfo();

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
            RAWINPUTDEVICE touchDevice = new RAWINPUTDEVICE()
            {
                usUsagePage = 13,
                usUsage = 4,
                dwFlags = RawInputFlags.RIDEV_INPUTSINK,
                hwndTarget = handle,
            };
            RAWINPUTDEVICE[] rawInputDevices = new RAWINPUTDEVICE[] { keyboardDevice, touchDevice };//mouseDevice
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
            */
        }

        //readonly uint cbSize = (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER));
        //ConcurrentDictionary<IntPtr, string> devicesName = new ConcurrentDictionary<IntPtr, string>();

        public int GET_APPCOMMAND_LPARAM(int lParam)
        {
            return (short)((lParam >> 16) & 0xFFFF) & ~0xF0000;
        }

        public int GET_DEVICE_LPARAM(uint lParam)
        {
            return 0;
        }

        protected IntPtr WindowRawInputHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            if(msgType == MessageType.WM_KEYDOWN || msgType == MessageType.WM_KEYUP || msgType == MessageType.WM_APPCOMMAND)
            {
                Console.WriteLine("Message Type:{0}  {1}  {2}", msgType, lParam, GET_APPCOMMAND_LPARAM(lParam.ToInt32()));

            }
            return IntPtr.Zero;

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
                RAWHID hid = data.data.hid;
                byte[] rawData = hid.GetRawData();
                for(int i = 0; i < hid.dwCount * hid.dwSizeHid; i ++)
                    Console.Write("{0:X2} ", rawData[i]);
                Console.WriteLine(" Length:{0}", rawData.Length);

                int dx = ((byte)rawData[4] << 8) | (byte)rawData[3]; 
                int dy = ((byte)rawData[6] << 8) | (byte)rawData[5];
                Console.WriteLine("x:{0} y:{1}", dx, dy);
            }

            return IntPtr.Zero;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Log.Error("aaa", new Exception("Exception,Exception,Exception"));
        }


    }
}



