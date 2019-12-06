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
using SpaceCG.WindowsAPI;
using System.Management;
using SpaceCG.Extension;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

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


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
             {
                 PC = new PerformanceCounter("Process", "Working Set", "WnCloud");
                 PC.MachineName = ".";
                 PC.ReadOnly = true;

                 PCC = new PerformanceCounterCategory("Process");
                 PCs = PCC.GetCounters("TIM");
                //PerformanceExtension.ToDebug(PCs);
            });

            GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            GCHandle.ToIntPtr(handle);
            IntPtr ptr = handle.AddrOfPinnedObject();
            handle.Free();

            //System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();


            Handle = new WindowInteropHelper(this).Handle;

            StringBuilder clsName = new StringBuilder(256);
            WinUser.GetClassName(Handle, clsName, 256);
            Console.WriteLine(clsName);
            
            bool result = WinUser.RegisterHotKey(Handle, 0, RhkModifier.CONTROL, VirtualKeyCode.VK_A);
            Console.WriteLine("result:{0}", result);

            //WinUser.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, WindowProcHandler, Process.GetCurrentProcess().MainModule.BaseAddress, 0);

            //OR
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(WindowProcHandler);
            //Marshal.GetLastWin32Error();

            /*
            string wql = $"TargetInstance isa 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%COM_%'";
            //ManagementExtension.ListenInstanceChange(wql, InstanceChangedHandler, App.Log);
            //await Task.Run(() => ManagementExtension.ListenInstanceChange(wql, InstanceChangedHandler, App.Log));
            await ManagementExtension.ListenInstanceChangeAsync(wql, TimeSpan.FromSeconds(1), InstanceChangedHandler, App.Log);

            //string wql2 = $"TargetInstance ISA 'Win32_Battery'";
            //ManagementExtension.ListenInstanceModification(wql2, TimeSpan.FromSeconds(10), InstanceModification, App.Log);
            Console.WriteLine("hello.");
            foreach (string p in SerialPort.GetPortNames()) Console.WriteLine(p);
            foreach (string p in ManagementExtension.GetPortNames()) Console.WriteLine(p);

            HwndSource hwndSource = HwndSource.FromVisual(this) as HwndSource;
            Console.WriteLine("{0} == {1} = true", Handle, hwndSource.Handle);
            //if (hwndSource != null)
            //    hwndSource.AddHook(new HwndSourceHook(WindowProcHandler));

            Task t = new Task(() => { Console.WriteLine("wait...");
                Thread.Sleep(5000); Console.WriteLine("Thid::{0} {1}", 
                    Thread.CurrentThread.ManagedThreadId, Task.CurrentId); });
            t.Start();

            Console.WriteLine("complete....");
            */
        }

        protected IntPtr WindowProcHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType mt = (MessageType)msg;
            //Console.WriteLine("MessageType:{0} {1}", mt, wParam.ToInt32());
            /*
            if (mt == MessageType.WM_KEYDOWN)
            {
                VirtualKeyCode vk = (VirtualKeyCode)wParam.ToInt32();
                //Console.WriteLine("wParam:{0}", vk);
            }
            if (mt == MessageType.WM_HOTKEY)
            {
                Console.WriteLine("wParam1:{0}", wParam.ToInt32());
            }
            if (mt == MessageType.WM_SETHOTKEY)
            {
                Console.WriteLine("wParam2:{0}", wParam.ToInt32());
            }
            if (mt == MessageType.WM_MOVE)
            {
                Console.WriteLine("wParam:{0} ", WinUser.LParamToPoint(lParam));
            }
            if (mt == MessageType.WM_MOUSEMOVE)
            {
                MouseKey mk = (MouseKey)wParam.ToInt32();
                Console.WriteLine("wParam:{0}  {1}", mk, WinUser.LParamToPoint(lParam));
            }
            */
            if (mt == MessageType.WM_DEVICECHANGE)
            {
                //Console.WriteLine("MessageType:{0} {1}", mt, wParam.ToInt32());
                DeviceBroadcastType dbt = (DeviceBroadcastType)wParam.ToInt32();
                Console.WriteLine("DeviceBroadcastType: {0}", dbt);

                if(dbt == DeviceBroadcastType.DBT_DEVICEARRIVAL || dbt == DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE)
                {
                    DEV_BROADCAST_HDR hdr = Marshal.PtrToStructure<DEV_BROADCAST_HDR>(lParam);
                    Console.WriteLine(hdr);
                    if (hdr.dbch_devicetype != DeviceType.DBT_DEVTYP_PORT)
                    {
                        handled = true;
                        return IntPtr.Zero;
                    }

                    //Get Port Name && Check Port Name
                    DEV_BROADCAST_PORT port = Marshal.PtrToStructure<DEV_BROADCAST_PORT>(lParam);
                    Console.WriteLine(port);
                }
                handled = true;
            }
            
            
            return IntPtr.Zero;
        }

        /// <summary>
        /// PnPEntity Changed Handler
        /// </summary>
        /// <param name="obj"></param>
        protected void InstanceChangedHandler(ManagementBaseObject obj)
        {
            //ManagementExtension.ToDebug(obj);
            //Console.WriteLine("1..{0} {1}", obj.ClassPath.IsClass, obj.ClassPath.IsInstance);
            ManagementBaseObject instance = (ManagementBaseObject)obj.GetPropertyValue("TargetInstance");
            //ManagementExtension.ToDebug(instance);
            //Console.WriteLine("2..{0} {1}", instance.ClassPath.IsClass, instance.ClassPath.IsInstance);
            instance.ToDebug();

            Console.WriteLine(instance.GetPropertyValue("Name"));
        }
        protected void InstanceModification(ManagementBaseObject obj)
        {
            Console.WriteLine("InstanceModification ...");
            obj.ToDebug();
            ManagementBaseObject instance = (ManagementBaseObject)obj.GetPropertyValue("TargetInstance");
            Console.WriteLine("TargetInstance....");
            instance.ToDebug();
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

