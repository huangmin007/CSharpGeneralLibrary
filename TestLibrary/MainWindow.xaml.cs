using SpaceCG.Log4Net.Controls;
using System.Windows;
using System;
using System.ComponentModel;
using System.Windows.Interop;
using System.Text;
using System.Windows.Media;
using System.IO.Ports;
using SpaceCG.Extension;

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
            //Console.WriteLine("{0} {2}", "test log", "error");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Handle = new WindowInteropHelper(this).Handle;

            SerialPort port = SerialPortExtension.CreateInstance("COM3,9600", (s, d) =>
            {

            });

        }

        private void SerialPortReceivedHandler(byte[] data)
        {
            string str = Encoding.Default.GetString(data);
            Console.WriteLine("{0}, {1}", data.Length, str);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Log.Error("aaa", new Exception("Exception,Exception,Exception"));
        }



    }
}



