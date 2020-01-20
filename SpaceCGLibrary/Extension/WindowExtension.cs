using System;
using System.Windows;
using System.Windows.Interop;
using SpaceCG.WindowsAPI.User32;

namespace SpaceCG.Extension
{
    /// <summary>
    /// 窗体 扩展/实用/通用 函数
    /// </summary>
    public static class WindowExtension
    {
        /// <summary>
        /// 设置窗体显示状态，主要用于 <code>ConfigurationManager.AppSettings["WindowState"]</code> 配置使用；例 SettingWindowState("1,0,0,2");
        /// <para>读取配置文件 key:WindowState，值是数值数组 [Topmost, WindowStyle, ResizeMode, WindowState]，对应转换为枚举值和Boolean值</para>
        /// <para>【Topmost】窗口是否显示在最顶层 z 顺序；  
        ///     0：窗口不置顶；
        ///     1：窗口置顶
        /// </para>  
        /// <para>【WindowStyle】窗口的边框样式；
        ///     0：全屏，仅工作区可见(该标题栏和边框不会显示)；
        ///     1：有一个边框的窗口；
        ///     2：具有 三维 边框的窗口；
        ///     3：内置的工具窗口；
        /// </para>
        /// <para>【ResizeMode】窗口大小调整模式；
        ///     0：窗口不能调整大小；
        ///     1：只能将和还原窗口；
        ///     2：窗口的大小进行调整；
        ///     3：窗口的大小进行调整；
        /// </para>
        /// <para>【WindowState】是否还原窗口中，最小化、最大化；
        ///     0：还原窗口
        ///     1：窗口最小化
        ///     2：窗口最大化
        /// </para>
        /// </summary>
        /// <param name="window"></param>
        /// <param name="config">配置参数值</param>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SettingWindowState(this Window window, string config)
        {
            if (string.IsNullOrWhiteSpace(config))
                throw new ArgumentNullException("参数 config 不能为空值");

            string[] cfg = config.Replace(" ", "").Split(',');
            if (cfg.Length != 4)
                throw new ArgumentOutOfRangeException($"参数 config=[{config}] 值与设计不符合");
            
            window.Topmost = cfg[0].ToLower() == "1";
            window.WindowStyle = (System.Windows.WindowStyle)Enum.Parse(typeof(System.Windows.WindowStyle), cfg[1]);
            window.ResizeMode = (ResizeMode)Enum.Parse(typeof(ResizeMode), cfg[2]);
            window.WindowState = (WindowState)Enum.Parse(typeof(WindowState), cfg[3]);
        }

        /// <summary>
        /// 设置窗体的基本显示参数
        /// </summary>
        /// <param name="window"></param>
        /// <param name="topmost">窗口是否出现在 Z 顺序的最顶层 </param>
        /// <param name="style">窗口的边框样式</param>
        /// <param name="mode">窗口大小调整模式</param>
        /// <param name="state">窗口是处于还原、最小化还是最大化状态</param>
        public static void SettingWindowState(this Window window, bool topmost, System.Windows.WindowStyle style, ResizeMode mode, WindowState state)
        {
            window.Topmost = topmost;
            window.WindowStyle = style;
            window.ResizeMode = mode;
            window.WindowState = state;
        }

        

    }
}
