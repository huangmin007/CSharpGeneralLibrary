using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;

namespace SpaceCG.Extension
{
    /// <summary>
    /// Application Extension
    /// </summary>
    public static partial class ApplicationExtension
    {
        /// <summary>
        /// 运行设置是否只运行一个实例，以及启用 Application 的事件记录，和未处理的异常信息记录
        /// </summary>
        /// <param name="app"></param>
        /// <param name="shutdownmMode">Shutdown Mode </param>
        /// <param name="runOnlyOne">只运行一个应用实例</param>
        /// <param name="name">如果 runOnlyOne 为 true, 该参数有效，表示系统范围内同步事件的名称</param>
        public static void RunDefaultSetting(this Application app, ShutdownMode shutdownmMode = ShutdownMode.OnMainWindowClose, bool runOnlyOne = false, string name = "MyApplicationName")
        {
            EventWaitHandle ProgramStarted;
            app.ShutdownMode = shutdownmMode;

            app.Startup += (s, e) =>
            {
                if (runOnlyOne)
                {
                    bool createNew;
                    ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, name, out createNew);
                    if (!createNew)
                    {
                        MessageBox.Show("程序正在运行中......", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                        Environment.Exit(0);
                    }
                }
                SpaceCGUtils.Log.InfoFormat("Application Startup.");
            };

            app.Exit += (s, e) => SpaceCGUtils.Log.InfoFormat("Application Exit.");
            //在异常由应用程序引发但未进行处理时发生
            app.DispatcherUnhandledException += (s, e) => SpaceCGUtils.Log?.ErrorFormat("Application Unhandled Exception: Handled:{0}  Exception:{1}", e.Handled, e.Exception);
        }

        /// <summary>
        /// 指示应用程序是否正在运行实例
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningInstance()
        {
            bool createNew;
            string name = "MutexNameOrAppNameOrOther";

#if WinFrom
            System.Threading.Mutex mutex = new Mutex(true, name, out createNew);
            if(createNew)
            {
                mutex.ReleaseMutex();
                return false;
            }
            else
            {
                return true;
            }
#else
            //应该变量不能放在函数体内
            EventWaitHandle eventWait = new EventWaitHandle(false, EventResetMode.AutoReset, name, out createNew);
            if (createNew)
            {
                return false;
            }
            else
            {
                return true;
            }
#endif
        }

        /// <summary>
        /// 读取应用程序范围属性的集合，一般发生在 <see cref="Application.OnStartup(StartupEventArgs)"/> 处理
        /// </summary>
        /// <param name="app"></param>
        /// <param name="filename"></param>
        public static void ReadApplicationProperties(this Application app, string filename)
        {
            // 从独立存储还原应用程序范围属性
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();

            try
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string[] keyValue = reader.ReadLine().Split(new char[] { ',' });
                            app.Properties[keyValue[0]] = keyValue[1];
                        }
                        reader.Close();
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
                SpaceCGUtils.Log.ErrorFormat("尝试访问的文件不存在", ex);
            }
        }

        /// <summary>
        /// 保存应用程序范围属性的集合，一般发生在 <see cref="Application.OnExit(ExitEventArgs)"/> 处理
        /// </summary>
        /// <param name="app"></param>
        /// <param name="filename"></param>
        public static void WriteApplicationProperties(this Application app, string filename)
        {
            // 将应用程序范围属性持久化到独立存储
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filename, FileMode.Create, storage))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (string key in app.Properties.Keys)
                    {
                        writer.WriteLine("{0},{1}", key, app.Properties[key]);
                    }
                    writer.Close();
                }
            }
        }
    }
}
