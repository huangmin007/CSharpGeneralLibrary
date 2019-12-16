#pragma warning disable CS1591,CS1572
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceCG.Examples
{
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
