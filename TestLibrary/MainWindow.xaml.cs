//#pragma warning disable CS0649
using SpaceCG.Log4Net;
using System.Diagnostics;
using System.Windows;
using System;
using System.Timers;
using System.Windows.Input;
using SpaceCG.WindowsAPI.WinUser;
using SpaceCG.WindowsAPI.Kernel32;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SpaceCG.Examples;
using System.Text;
using SpaceCG.WindowsAPI;
using System.Management;
using SpaceCG.Extension;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Media;
using SpaceCG.Template;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using log4net.Core;
using System.IO;
using HPSocket.Tcp;
using HPSocket;
using HPSocket.Udp;
using System.Reflection;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.Config", Watch = true)]
namespace TestLibrary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MainWindow));

        protected IntPtr Handle;

        private SerialPort serialPort;
        private IClient client;
        private IServer server;

        public MainWindow()
        {
            InitializeComponent();
            Log.InfoFormat("MainWindow.");

            Handle = new WindowInteropHelper(this).Handle;
            Console.WriteLine("{0} {1} {2}", this.IsInitialized, this.IsLoaded, Handle);

            //Client = HPSocketExtension.CreateClient<HPSocket.Tcp.TcpClient>("127.0.0.1", 9999, SocketReceivedHandler, true, App.Log);
            client = HPSocketExtension.CreateClient<TcpClient>("127.0.0.1", 9999, SocketReceivedHandler, true, App.Log);
            //HPSocketExtension.CreateServer<TcpServer<Byte>>(444, (aa, f) => {    });
            server = HPSocketExtension.CreateServer<UdpServer>(4444, (connid, bytes) =>
            {
                Console.WriteLine("dataa......");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    IFormatter format = new BinaryFormatter();
                    LoggingEvent ev = (LoggingEvent)format.Deserialize(ms);
                    Console.WriteLine(ev);
                    Console.WriteLine("{0}, {1}", ev.LoggerName, ev.RenderedMessage);
                }
            }, App.Log);
#if NET46
            Console.WriteLine("win32");
#else
            Console.WriteLine("any cpu");
#endif
            LoggerWindow LoggerWindow = new LoggerWindow();
            LoggerWindow.Show();
            //DeserializeLoggingEvent();


            //IEnumerable<char> t = "aaaa";
            List<int> list = new List<int>() { 1, 2, 3 };
            TestList(list);
            foreach(int i in list)
                Console.Write("{0}, ", i);
            Console.WriteLine();

            int index;
            int[] indexs;

            string str = "广，东省深圳深圳深深圳圳temp市福田,区深圳街，hello";
            string t = "深圳圳";

            SpaceCG.General.BoyerMoore boyer = new SpaceCG.General.BoyerMoore(ref t, false);
            index = boyer.Search(ref str);
            //int c = boyer.SearchAt(ref str, 1);
            indexs = boyer.SearchAll(ref str);
            Console.WriteLine(">>>> {0} {1} {2}", index, "-", string.Join(",,", indexs));

            byte[] data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x04, 0x05, 0x04, 0x02, 0x01, 0x04, 0x05, 0x04, 0x03};
            byte[] s = new byte[] { 0x04, 0x05, 0x04};

            //index = SpaceCG.General.BoyerMoore.Search(data, s);
            //indexs = (int[])SpaceCG.General.BoyerMoore.SearchAll(ref str, ref t, false);
            //Console.WriteLine(">>>>> {0} {1}", index, string.Join(",,", indexs));


            SpaceCG.General.BoyerMoore<byte> tBoyer = new SpaceCG.General.BoyerMoore<byte>();
            //SpaceCG.General.BoyerMoore<TF> tff2 = new SpaceCG.General.BoyerMoore<TF>();
            tBoyer.SetPattern(s);
            index = tBoyer.Search(data);
            indexs = tBoyer.SearchAll(data);
            Console.WriteLine(">>>>>T::{0} {1}", index, string.Join(",,", indexs));
        }

        struct TF
        {
            public int a;
            public int b;
        }

        void TestList(List<int> list)
        {
            list[2] = 10;
        }
        void TestString(ref string str)
        {
            str = "ABCD";
        }

        public void DeserializeLoggingEvent()
        {
            
            LoggingEventData data = new LoggingEventData()
            {
                Domain = "domain",
                ExceptionString = "exception string",
                Level = Level.Info,
                LoggerName = "test",
                Message = "Hello world",
            };
            LoggingEvent le = new LoggingEvent(data);
            

            //string le = @"[2019-12-20 10:14:34] [Huangmin] [ 9] [ INFO] [TestLibrary.App] [CreateTcpClient(103)] - 客户端连接的为本地网络服务地址：127.0.0.1 ，未监听网络的可用性变化。";

            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, le);
            stream.Position = 0;
            for(int i = 0; i < stream.Length; i ++)
                Console.Write("{0} ", stream.ReadByte());
            Console.WriteLine(stream.Length);
            stream.Position = 0;
            
            //byte[] buffer = new byte[stream.Length];
            byte[] buffer = stream.GetBuffer();
            //int length = stream.Read(buffer, 0, buffer.Length);
            //Console.WriteLine("r len:{0}", length);
            //stream.Write(buffer, 0, buffer.Length);
            //for (int i = 0; i < buffer.Length; i++)
            //    buffer[i] = (byte)stream.ReadByte();
            
            stream.Close();
            stream.Dispose();

            client.Send(buffer, buffer.Length);

            //MemoryStream ms = new MemoryStream(buffer);
            //IFormatter format = new BinaryFormatter();
            //LoggingEvent ev = (LoggingEvent)format.Deserialize(ms);
            //Console.WriteLine(ev);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            TextBoxAppender appender = new TextBoxAppender(TextBox_Logs);

            App.Log.InfoFormat("Initialize.");
        }

        private void SocketReceivedHandler(byte[] data)
        {
            Console.WriteLine("Socket {0}", data.Length);
        }

#region override
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //Console.WriteLine(e.Key);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (mouseHook != null) mouseHook.Dispose();
            if (keyboardHook != null) keyboardHook.Dispose();

            bool result = WinUser.UnregisterHotKey(Handle, 0);
            Console.WriteLine("result:{0}", result);

            if (server != null)
            {
                HPSocketExtension.DisposeServer(server, App.Log);
                server = null;
            }
            if (client != null)
            {
                HPSocketExtension.DisposeClient(client, App.Log);
                client = null;
            }
        }
#endregion

        PerformanceCounter PC;
        PerformanceCounter[] PCs;
        PerformanceCounterCategory PCC;

        MouseHook mouseHook;
        KeyboardHook keyboardHook;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxAppender appender2 = new ListBoxAppender(listBox, 10);

            int tier = RenderCapability.Tier >> 16;

            Log.InfoFormat("Window Loaded.");
            Log.DebugFormat("Debug Foramt");
            Log.WarnFormat("Warn Format");
            Log.ErrorFormat("Error Format");
            Log.FatalFormat("Fatal Format");
            Log.InfoFormat("Window Loaded.");
            Log.DebugFormat("Debug Foramt");
            Log.WarnFormat("Warn Format");
            Log.ErrorFormat("Error Format");
            Log.FatalFormat("Fatal Format");


            /*
            IntPtr ptr = new WindowInteropHelper(this).Handle;
            Console.WriteLine("{0} {1} {2} {3}", this.IsInitialized, this.IsLoaded, ptr, Handle);

            await Task.Run(() =>
             {
                 PC = new PerformanceCounter("Process", "Working Set", "WnCloud");
                 PC.MachineName = ".";
                 PC.ReadOnly = true;

                 PCC = new PerformanceCounterCategory("Process");
                 PCs = PCC.GetCounters("TIM");
                 //PerformanceExtension.ToDebug(PCs);
             });
             */
            //System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();


            Handle = new WindowInteropHelper(this).Handle;

            StringBuilder clsName = new StringBuilder(256);
            WinUser.GetClassName(Handle, clsName, 256);
            Console.WriteLine(clsName);

            RECT rect = new RECT();
            WinUser.GetWindowRect(Handle, ref rect);
            Console.WriteLine(rect);
            WinUser.GetClientRect(Handle, ref rect);
            Console.WriteLine(rect);
            //WinUser.GetWindowRgn(Handle, ref rect);
            //Console.WriteLine(rect);

            //bool result = WinUser.RegisterHotKey(Handle, 0, RhkModifier.CONTROL, VirtualKeyCode.VK_A);
            //Console.WriteLine("result:{0}", result);

            //WinUser.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, WindowProcHandler, Process.GetCurrentProcess().MainModule.BaseAddress, 0);

            //OR
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(WindowProcHandler);
            //Marshal.GetLastWin32Error();

            
            string wql = $"TargetInstance isa 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%COM_%'";
            ManagementExtension.ListenInstanceChange(wql, InstanceChangedHandler, App.Log);
            //await Task.Run(() => ManagementExtension.ListenInstanceChange(wql, InstanceChangedHandler, App.Log));
            //await ManagementExtension.ListenInstanceChangeAsync(wql, TimeSpan.FromSeconds(1), InstanceChangedHandler, App.Log);
            /*
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

            /*
            try
            {
                serialPort = SerialPortExtension.Create("COM59,115200", 88, App.Log);
                serialPort.OpenAndListen(SerialPortReceivedHandler, true, App.Log);
                serialPort.AutoReconnection(this, App.Log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EX:::{0}", ex);
            }
            */
        }

        private void SerialPortReceivedHandler(byte[] data) 
        {
            string str = Encoding.Default.GetString(data);
            Console.WriteLine("{0}, {1}", data.Length, str);
            //App.Log.InfoFormat("{0}  {1}", data.Length, str);
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
            Log.InfoFormat("Length:{0}  String:{1}", length, lpString);
           
            ManagementExtension.RemoveInstanceChange();

            if (client != null)
            {
                HPSocketExtension.DisposeClient(client, App.Log);
                //client = null;
            }

            Log.Error("aaa", new Exception("Exception,Exception,Exception"));
        }



    }
}

