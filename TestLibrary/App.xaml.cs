using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SpaceCG.Template;

namespace TestLibrary
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        /// <summary>
        /// 应用程序日志对象
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(App));
        //public static readonly log4net.ILog Log = log4net.LogManager.GetLogger("ApplicationLogger");

        protected override void OnStartup(StartupEventArgs e)
        {
            //log4net.Config.BasicConfigurator.Configure();

            //SplashScreen splashScreen = new SplashScreen("InitScreen.png");
            //splashScreen.Show(true, true);

            base.OnStartup(e);            
            Log.InfoFormat("Application OnStartup");

            //当主窗口关闭或在调用 Shutdown() 时，应用程序将关闭。
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //在异常由应用程序引发但未进行处理时发生
            this.DispatcherUnhandledException += DispatcherUnhandledExceptionHandler;
        }

        private void DispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.ErrorFormat("Application Unhandled Exception: {0}", e.Exception);
            e.Handled = true; //阻止默认的未处理异常处理
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.InfoFormat("Application OnExit");
        }
        
    }
}
