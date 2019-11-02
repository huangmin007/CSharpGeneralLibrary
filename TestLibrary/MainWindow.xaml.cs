using log4net.Layout;
using SpaceCG.Log4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace TestLibrary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            Log.InfoFormat("MainWindow.");


            TextBoxBaseAppender appender = new TextBoxBaseAppender(TextBox_Logs, 50);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("hello");
            builder.AppendLine("world");

            String[] lines = builder.ToString().Split('\n');
            Console.WriteLine(lines.Length);
        }

        int count = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TextBox_Logs.AppendText("Hello");
            //Log.InfoFormat("Hello {0}", count);
            App.Log.InfoFormat("World {0}", count);
            //log4net.LogManager.GetLogger("ApplicationLogger").InfoFormat("Hello {0}", count);
            count++;

            Log.ErrorFormat("Test Error Message: {0}", count);

            Console.WriteLine("Hello World {0}", count);
        }
    }
}
