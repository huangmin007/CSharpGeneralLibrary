using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using SpaceCG.Log4Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.Config", Watch = true)]
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

            //.Properties["Operator"] = 12;
            TextBoxBaseAppender appender = new TextBoxBaseAppender(TextBox_Logs);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //FileInfo file = new FileInfo("Log/logs.log");
            //System.IO.File.ReadAllLines(file.Name);
            int[] data = { 2, 5, 3, 6, 9, 1, 8, 7, 5, 6, 4 };

            //列出奇偶分组
            var result0 = from num in data group num by num % 2 == 0;
            var result00 = data.GroupBy(num => num % 2 == 0);

            foreach (var group in result0)
            {
                Console.WriteLine("gggg");
                foreach (var num in group)
                    Console.WriteLine($"num::{num}");
            }

            //计算出偶数个数
            var count = data.Aggregate(0, (total, next) => next % 2 == 0 ? total++ : total);

            //列出前三个最小的值
            var result1 = (from num in data orderby num select num).Take(3);
            var result11 = data.OrderBy(num => num).Take(3);

            result1.ToList();

            foreach (var num in result11)
                Console.WriteLine($"num:{num}");

            //Log4NetUtils.ReserveFileCount(30, fi.Directory.Name, "*." + fi.Extension);
            //Log4NetUtils.ReserveFileDays(6, fi.Directory.Name);

            Console.WriteLine("async start..");
            int t = await TestAsync();
            Console.WriteLine($"num:{t}");
            Console.WriteLine("async over..");
        }
        
        static void TestValueType(System.ValueType val)
        {
            
        }

        private async Task<int> TestAsync()
        {
            await Task.Delay(1000);
            return 1;
        }

        int count = 0;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //TextBox_Logs.AppendText("Hello");
            //Log.InfoFormat("Hello {0}", count);

            App.Log.InfoFormat("中文 {0}", count);
            //log4net.LogManager.GetLogger("ApplicationLogger").InfoFormat("Hello {0}", count);
            count++;

            Log.WarnFormat("test Warn Message.. 中文测试");
            Log.ErrorFormat("Test Error Message: {0}", count);
            Log.FatalFormat("test Fatil message..");

            await SumPageSizesAsync();

            Console.WriteLine("Hello World {0}", count);

            ListBox listBox = new ListBox();
            //LoggingEvent evt = new LoggingEvent();

            //RemoteSyslogAppender;
        }

        private async Task<int> ProcessURLAsync(string url)
        {
            var byteArray = await GetURLContentsAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }



        private async Task SumPageSizesAsync()
        {

            // Make a list of web addresses.
            List<string> urlList = SetUpURLList();

            //IEnumerable<Task<int>> downloadTasksQuery = from url in urlList select ProcessURLAsync(url);

            // Use ToArray to execute the query and start the download tasks.
            //Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            

            Task<int>[] downloadTasks = new Task<int>[urlList.Count];
            for(int i =0; i < urlList.Count; i ++)
            {
                downloadTasks[i] = ProcessURLAsync(urlList[i]);
            }


            // Await the completion of all the running tasks.
            int[] lengths = await Task.WhenAll(downloadTasks);
            int total = lengths.Sum();

            
            //var total = 0;
            /*foreach (var url in urlList)
            {
                byte[] urlContents = await GetURLContentsAsync(url);

                DisplayResults(url, urlContents);

                // Update the total.
                total += urlContents.Length;

            }
            */

            // Display the total count for all of the websites.
            App.Log.Info($"\r\n\r\nTotal bytes returned:  {total}\r\n");
        }

        private List<string> SetUpURLList()
        {
            List<string> urls = new List<string>
            {
                "https://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/hh290136.aspx",
                "https://msdn.microsoft.com/library/ee256749.aspx",
                "https://msdn.microsoft.com/library/hh290138.aspx",
                "https://msdn.microsoft.com/library/hh290140.aspx",
                "https://msdn.microsoft.com/library/dd470362.aspx",
                "https://msdn.microsoft.com/library/aa578028.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com/library/ff730837.aspx"
            };
            return urls;
        }

        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            // The downloaded resource ends up in the variable named content.
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL.
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for
            // the response.
            using (WebResponse response = await webReq.GetResponseAsync())

            // The previous statement abbreviates the following two statements.

            //Task<WebResponse> responseTask = webReq.GetResponseAsync();
            //using (WebResponse response = await responseTask)
            {
                // Get the data stream that is associated with the specified url.
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.
                    await responseStream.CopyToAsync(content);

                    // The previous statement abbreviates the following two statements.

                    // CopyToAsync returns a Task, not a Task<T>.
                    //Task copyTask = responseStream.CopyToAsync(content);

                    // When copyTask is completed, content contains a copy of
                    // responseStream.
                    //await copyTask;
                }
            }
            // Return the result as a byte array.
            return content.ToArray();
        }

        private void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format
            // is designed to be used with a monospaced font, such as
            // Lucida Console or Global Monospace.
            var bytes = content.Length;
            // Strip off the "https://".
            var displayURL = url.Replace("https://", "");
            App.Log.Info($"\n{displayURL,-58} {bytes,8}");
        }
    }
}
