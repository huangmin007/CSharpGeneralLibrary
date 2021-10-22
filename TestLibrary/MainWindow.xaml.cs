using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Interop;
using SpaceCG.WindowsAPI.User32;
using System.Windows.Media;
using SpaceCG.Log4Net.Controls;
using System.Runtime.InteropServices;
using Examples;

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

        RawInputExample rawInput;

        public MainWindow()
        {
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            int tier = RenderCapability.Tier >> 16;
            Console.WriteLine("Tier:{0}", tier);
#endif
            InitializeComponent();
            LoggerWindow LoggerWindow = new LoggerWindow();
            Log.InfoFormat("MainWindow.");
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
            Log.InfoFormat("OnDipChanged...");
        }

        protected override void OnInitialized(EventArgs e)
        {
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
     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Log.InfoFormat("Window_Loaded");

            Console.WriteLine();
            //rawInput = new RawInputExample(this);

            //handle = new WindowInteropHelper(this).Handle;

            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(WindowRawInputHandler);
        }

        protected IntPtr WindowRawInputHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            Log.InfoFormat("Message Type:{0}", msgType);

            if (msgType == MessageType.WM_DPICHANGED || msgType == MessageType.WM_DPICHANGED_BEFOREPARENT)
            {
                Console.WriteLine("testteetata");
                Log.InfoFormat("Msg WM_DPICHANDED Event");
            }

            if(msgType == MessageType.WM_KEYDOWN || msgType == MessageType.WM_KEYUP || msgType == MessageType.WM_APPCOMMAND)
            {
                Console.WriteLine("Key Message");
            //Console.WriteLine("Message Type:{0}  {1}  {2}", msgType, lParam, GET_APPCOMMAND_LPARAM(lParam.ToInt32()));
            }

            return IntPtr.Zero;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Error("aaa", new Exception("Exception,Exception,Exception"));
        }

    }
}



