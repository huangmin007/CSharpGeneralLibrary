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
        public static log4net.ILog Log = log4net.LogManager.GetLogger("ApplicationLogger");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.InfoFormat("Application OnStartup");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.InfoFormat("Application OnExit");
        }
    }
}
