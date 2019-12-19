#pragma warning disable CS1591,CS1572
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceCG.Examples
{
    /// <summary>
    /// 跨应用程序会话保留和还原应用程序范围属性 示例
    /// </summary>
    public class MyApp:Application
    {
        private string filename = "";

        public MyApp()
        {
            // 初始化应用程序范围属性
            this.Properties["NumberOfAppSessions"] = 0;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 从独立存储还原应用程序范围属性
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            try
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // 分别还原每个应用程序范围属性
                        while (!reader.EndOfStream)
                        {
                            string[] keyValue = reader.ReadLine().Split(new char[] { ',' });
                            this.Properties[keyValue[0]] = keyValue[1];
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                // 在独立存储中找不到文件时的句柄：
                // * 当第一个应用程序会话
                // * 文件被删除时
                Console.WriteLine(ex);
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 将应用程序范围属性持久化到独立存储
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, storage))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // 分别持久化每个应用程序范围属性
                foreach (string key in this.Properties.Keys)
                {
                    writer.WriteLine("{0},{1}", key, this.Properties[key]);
                }
            }
            base.OnExit(e);
        }
    }

    /// <summary>
    /// Application Run One Example
    /// </summary>
    public class ApplicationRunOne
    {
        /// <summary>
        /// 只运行一个实体对象
        /// <para>Main 函数</para>
        /// </summary>
        [Example]
        public static void MainX()
        {
            bool createNew;
            System.Threading.Mutex run = new Mutex(true, "MutexNameOrAppNameOrOther", out createNew);

            if (createNew)
            {
                run.ReleaseMutex();

                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
            }
            else
            {
                //MessageBox.Show(null, "程序正在运行中......", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }
        
        [Example]
        private static EventWaitHandle ProgramStarted { get; set; }
        /// <summary>
        /// WPF override OnStartup
        /// </summary>
        /// <param name="e"></param>
        [Example]
        public static void OnStartup(StartupEventArgs e)
        {
            bool createNew;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "MutexNameOrAppNameOrOther", out createNew);

            if (!createNew)
            {
                MessageBox.Show("程序正在运行中......", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);

                //App.Current.Shutdown();
                Environment.Exit(0);
            }

            //base.OnStartup(e);
        }
    }
}
