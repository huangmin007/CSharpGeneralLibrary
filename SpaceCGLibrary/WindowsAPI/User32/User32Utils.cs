using System;
using System.Text;
using System.Collections.Generic;

namespace SpaceCG.WindowsAPI.User32
{
    /// <summary>
    /// WinUser 实用/通用 函数
    /// </summary>
    public static partial class User32Utils
    {
        /// <summary>
        /// StringBuffer 实例所分配的内存中的最大字符数，Default 0xFF.
        /// </summary>
        private const int CAPACITY_SIZE = 0xFF;

        /// <summary>
        /// 遍历屏幕上所有的顶层窗口，然后给回调函数传入每个遍历窗口的句柄。
        /// 不过并不是所有遍历的窗口都是顶层窗口，有一些非顶级系统窗口也会遍历到，详见：EnumWindows 
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<IntPtr> EnumWindows()
        {
            List<IntPtr> windows = new List<IntPtr>();
            User32.EnumWindows((hwnd, lParam) =>
            {
                //lParam: process id
                windows.Add(hwnd);
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary>
        /// 获取 窗口句柄/窗口标题 字典
        /// </summary>
        /// <returns>返回窗口句柄及对应的标题</returns>
        public static IReadOnlyDictionary<IntPtr, string> FindWindowByTitleName()
        {
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();
            User32.EnumWindows((hwnd, IParam) =>
            {
                StringBuilder lpString = new StringBuilder(CAPACITY_SIZE);
                User32.GetWindowText(hwnd, lpString, lpString.Capacity);
                windows.Add(hwnd, lpString.ToString());
                
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary>
        /// 获取 窗口句柄/窗口标题 字典
        /// </summary>
        /// <param name="titleName">关键名称搜索</param>
        /// <returns>返回窗口句柄及对应的标题</returns>
        public static IReadOnlyDictionary<IntPtr, string> FindWindowByTitleName(string titleName)
        {
            if (string.IsNullOrWhiteSpace(titleName)) return FindWindowByTitleName();

            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>(16);
            User32.EnumWindows((hwnd, IParam) =>
            {
                StringBuilder lpString = new StringBuilder(CAPACITY_SIZE);
                User32.GetWindowText(hwnd, lpString, lpString.Capacity); 
                               
                if(lpString.ToString().IndexOf(titleName, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    windows.Add(hwnd, lpString.ToString());
                }
                return true;
            }, IntPtr.Zero);

            return windows;            
        }

        /// <summary>
        /// 获取 窗口句柄/窗口类名 字典
        /// </summary>
        /// <returns>返回窗口句柄及对应的类名</returns>
        public static IReadOnlyDictionary<IntPtr, string> FindWindowByClassName()
        {
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();
            User32.EnumWindows((hwnd, IParam) =>
            {
                StringBuilder lpString = new StringBuilder(CAPACITY_SIZE);
                User32.GetClassName(hwnd, lpString, lpString.Capacity);
                windows.Add(hwnd, lpString.ToString());

                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary>
        /// 获取 窗口句柄/窗口类名 字典
        /// </summary>
        /// <param name="className"></param>
        /// <returns>返回窗口句柄及对应的类名</returns>
        public static IReadOnlyDictionary<IntPtr, string> FindWindowByClassName(string className)
        {
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>(16);
            User32.EnumWindows((hwnd, IParam) =>
            {
                StringBuilder lpString = new StringBuilder(CAPACITY_SIZE);
                User32.GetClassName(hwnd, lpString, lpString.Capacity);
                if (lpString.ToString().IndexOf(className, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    windows.Add(hwnd, lpString.ToString());
                }

                return true;
            }, IntPtr.Zero);

            return windows;
        }


        /// <summary>
        /// <see cref="WindowProc"/> Arguments lParam
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static POINT LParamToPoint(int value)
        {
            return new POINT(value & 0xFFFF, value >> 16);
        }
        /// <summary>
        /// <see cref="WindowProc"/> Arguments lParam
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static POINT LParamToPoint(IntPtr value)
        {
            return LParamToPoint(value.ToInt32());
        }
    }
}
