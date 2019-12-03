using SpaceCG.Log4Net;
using System.Diagnostics;
using System.Windows;
using System;
using System.Collections;
using System.Timers;
using System.Windows.Input;
using SpaceCG.WindowsAPI.WinUser;
using SpaceCG.WindowsAPI.Kernel32;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SpaceCG.Examples;
using System.Text;
using SpaceCG.WindowsAPI.DBT;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.Config", Watch = true)]
namespace TestLibrary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MainWindow));

        protected IntPtr Handle;

        public MainWindow()
        {
            InitializeComponent();
            Log.InfoFormat("MainWindow.");
            TextBoxBaseAppender appender = new TextBoxBaseAppender(TextBox_Logs);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Console.WriteLine(e.Key);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (mouseHook != null) mouseHook.Dispose();
            if (keyboardHook != null) keyboardHook.Dispose();

            bool result = WinUser.UnregisterHotKey(Handle, 0);
            Console.WriteLine("result:{0}", result);
        }

        PerformanceCounter PC;
        PerformanceCounter[] PCs;
        PerformanceCounterCategory PCC;

        MouseHook mouseHook;
        KeyboardHook keyboardHook;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //mouseHook = new MouseHook();
            //keyboardHook = new KeyboardHook();

            /*
            PC = new PerformanceCounter("Process", "Working Set", "WnCloud");
            PC.MachineName = ".";
            PC.ReadOnly = true;

            PCC = new PerformanceCounterCategory("Process");
            PCs = PCC.GetCounters("TIM");
            //打印每个字段的数据类型，及说明
            foreach (PerformanceCounter counter in PCs)
            {
                Console.WriteLine("{0}  {1}  {2}", counter.CounterName.PadRight(32), counter.CounterType.ToString().PadRight(32), counter.CounterHelp);
            }

            Timer timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            */
            Handle = new WindowInteropHelper(this).Handle;

            bool result = WinUser.RegisterHotKey(Handle, 0, RhkModifier.CONTROL, VirtualKeyCode.VK_A);
            Console.WriteLine("result:{0}", result);

            //WinUser.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, WindowProcHandler, Process.GetCurrentProcess().MainModule.BaseAddress, 0);
            
            HwndSource hwndSource = HwndSource.FromVisual(this) as HwndSource;
            Console.WriteLine("{0} == {1} = true", Handle, hwndSource.Handle);
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(WindowProcHandler));//挂钩

            //HwndSource.FromHwnd(Handle).AddHook(WindowProcHandler);
            //Marshal.GetLastWin32Error();
        }

        protected IntPtr WindowProcHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType mt = (MessageType)msg;
            Console.WriteLine("MessageType:{0} {1}", mt, wParam.ToInt32());

            if (mt == MessageType.WM_DEVICECHANGE)
            {
                //Console.WriteLine("MessageType:{0} {1}", mt, wParam.ToInt32());
                DeviceBroadcastType dbt = (DeviceBroadcastType)wParam.ToInt32();
                Console.WriteLine("DeviceBroadcastType: {0}", dbt);

                switch (dbt)
                {
                    case DeviceBroadcastType.DBT_DEVICEARRIVAL:
                    case DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE:
                        Console.WriteLine(dbt == DeviceBroadcastType.DBT_DEVICEARRIVAL ? "Device Arrival" : "Device Move Complete");

                        DEV_BROADCAST_HDR hdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure<DEV_BROADCAST_HDR>(lParam);
                        Console.WriteLine("{0}", hdr);

                        if (hdr.dbch_devicetype == DeviceType.DBT_DEVTYP_PORT)
                        {
                            DEV_BROADCAST_PORT port = (DEV_BROADCAST_PORT)Marshal.PtrToStructure<DEV_BROADCAST_PORT>(lParam);
                            Console.WriteLine(port);
                        }
                        if (hdr.dbch_devicetype == DeviceType.DBT_DEVTYP_VOLUME)
                        {
                            DEV_BROADCAST_VOLUME volume = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure<DEV_BROADCAST_VOLUME>(lParam);
                            Console.WriteLine(volume);
                        }
                        break;

                    default:
                        break;
                }

                handled = true;
            }

            return IntPtr.Zero;
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("{0}", PC.NextValue() / 1024.0);

            foreach(PerformanceCounter counter in PCs)
            {
                Console.WriteLine("{0}  {1}  {2}  {3}", counter.InstanceName, counter.CounterName.PadRight(32), counter.CounterType.ToString().PadRight(16), counter.NextValue());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;

            StringBuilder lpString = new StringBuilder(256);
            int length = WinUser.GetWindowText(hWnd, lpString, 256);
            Console.WriteLine("Length:{0}  String:{1}", length, lpString);

            length = WinUser.GetClassName(hWnd, lpString, 256);
            Console.WriteLine("Length:{0}  String:{1}", length, lpString);

        }

        

    }
}

