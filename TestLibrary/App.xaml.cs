using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestLibrary
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用程序日志对象
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(App));
        //public static readonly log4net.ILog Log = log4net.LogManager.GetLogger("ApplicationLogger");

        protected override void OnStartup(StartupEventArgs e)
        {
            //log4net.Config.BasicConfigurator.Configure();

            base.OnStartup(e);
            Log.InfoFormat("Application OnStartup");
            SpaceCG.Log4Net.Log4NetUtils.ReserveFileCount(5, "Logs", "*.log");

            Console.WriteLine("IsDebugEnabled::{0}", Log.IsDebugEnabled);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.InfoFormat("Application OnExit");
        }
    }
}
