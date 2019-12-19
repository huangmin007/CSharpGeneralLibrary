using System;
using System.Windows;
using System.Windows.Threading;

namespace SpaceCG.Template
{
    /// <summary>
    /// Application Template
    /// </summary>
    public partial class AppTemplate: Application
    {
        /// <summary>
        /// 应用程序日志对象
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger("ApplicationLogger");

        protected override void OnStartup(StartupEventArgs e)
        {
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
