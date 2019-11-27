using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpaceCG.Native.WinAPI
{
    /// <summary>
    /// WinUser.h
    /// <para>LPCTSTR，LPWSTR, PTSTR, LPTSTR，L表示long指针，P表示这是一个指针，T表示_T宏,这个宏用来表示你的字符是否使用UNICODE, 如果你的程序定义了UNICODE或者其他相关的宏，那么这个字符或者字符串将被作为UNICODE字符串，否则就是标准的ANSI字符串。C表示是一个常量,const。STR表示这个变量是一个字符串。</para>
    /// <para>参考： https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ </para>
    /// </summary>
    public static class WinUser
    {
        public const string DLL_Name = "user32.dll";

        #region SetWindowPos 函数的参数值 hWndInsertAfter
        /// <summary>
        /// 将窗口置于Z顺序的顶部。
        /// </summary>
        public const int HWND_TOP = 0;
        /// <summary>
        /// 将窗口置于Z顺序的底部。
        /// 如果hWnd参数标识了最顶部的窗口，则该窗口将失去其最顶部的状态，并放置在所有其他窗口的底部。
        /// </summary>
        public const int HWND_BOTTOM = 1;
        /// <summary>
        /// 将窗口置于所有非最上面的窗口上方；即使禁用窗口，窗口也将保持其最高位置。
        /// </summary>
        public const int HWND_TOPMOST = -1;
        /// <summary>
        /// 将窗口放置在所有非最上面的窗口上方（即，所有最上面的窗口的后面）。如果窗口已经是非最上面的窗口，则此标志无效。
        /// </summary>
        public const int HWND_NOTOPMOST = -2;
        #endregion


        #region SetWindowPos 函数的参数值 uFlags
        /// <summary>
        /// SetWindowPos 函数的标志位参数
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
        /// </summary>
        [Flags]
        public enum SWP_Flags
        {
            /// <summary>
            /// 如果调用线程和拥有窗口的线程连接到不同的输入队列，则系统会将请求发布到拥有窗口的线程
            /// <para>这样可以防止在其他线程处理请求时调用线程阻塞其执行</para>
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            /// 防止生成 WM_SYNCPAINT 消息
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            /// 在窗口周围绘制框架（在窗口的类描述中定义）
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            /// 应用使用 SetWindowLong 函数设置的新框架样式；将 WM_NCCALCSIZE 消息发送到窗口，即使未更改窗口的大小。
            /// <para>如果未指定此标志，则仅在更改窗口大小时才发送 WM_NCCALCSIZE </para>
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            /// 隐藏窗口
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            /// 不激活窗口。
            /// <para>如果未设置此标志，则激活窗口并将其移至最顶层或非顶层组的顶部（取决于 hWndInsertAfter 参数的设置）。</para>
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            /// 丢弃客户区的全部内容。
            /// <para>如果未指定此标志，则在调整窗口大小或位置后，将保存客户区的有效内容并将其复制回客户区。</para>
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            /// 保留当前位置（忽略X和Y参数）。
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            /// 不更改所有者窗口在Z顺序中的位置。
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            /// 不重绘更改。
            /// <para>如果设置了此标志，则不会发生任何重绘。这适用于工作区，非工作区（包括标题栏和滚动条）以及由于移动窗口而导致未显示的父窗口的任何部分。</para>
            /// <para>设置此标志后，应用程序必须显式使窗口和父窗口中需要重绘的任何部分无效或重绘。</para>
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            /// 与 SWP_NOOWNERZORDER 标志相同。
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            /// 阻止窗口接收 WM_WINDOWPOSCHANGING 消息
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            /// 保留当前大小（忽略cx和cy参数）
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            /// 保留当前的Z顺序（忽略 hWndInsertAfter 参数）
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            /// 显示窗口
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,
        }
        #endregion


        /// <summary>
        /// 更改子窗口，弹出窗口或顶级窗口的大小，位置和Z顺序；这些窗口是根据其在屏幕上的外观排序的；最顶部的窗口获得最高排名，并且是Z顺序中的第一个窗口。
        /// <para>如果使用 SetWindowLong 更改了某些窗口数据，则必须调用 SetWindowPos 才能使更改生效。对 uFlags 使用以下组合：SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED。</para>
        /// <para>示例：SetWindowPos(new WindowInteropHelper(this).Handle., (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_Flags.SWP_NOMOVE | SWP_Flags.SWP_NOSIZE); //将窗口 Z 序设置为最顶</para>
        /// <para>示例：SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, 10, 10, 800, 600, SWP_Flags.SWP_NOZORDER ); //设置窗口大小及位置，忽略 Z 序</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">在Z顺序中位于定位的窗口之前的窗口的句柄。</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags">窗口大小和位置标志</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_Name, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);



        /// <summary>
        /// 将创建指定窗口的线程带入前台并激活该窗口。
        /// <para>键盘输入直接指向窗口，并且为用户更改了各种视觉提示。系统向创建前景窗口的线程分配的优先级比向其他线程分配的优先级高。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setforegroundwindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>如果将窗口带到前台，则返回值为非零。如果未将窗口带到前台，则返回值为零</returns>
        [DllImport(DLL_Name, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 检索前景窗口（用户当前正在使用的窗口）的句柄。系统向创建前景窗口的线程分配的优先级比向其他线程分配的优先级高。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getforegroundwindow</para>
        /// </summary>
        /// <returns>返回值是前景窗口的句柄。在某些情况下，例如某个窗口失去激活状态，前景窗口可以为NULL。</returns>
        [DllImport(DLL_Name, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();



        /// <summary>
        /// 更改指定窗口的位置和尺寸。对于顶级窗口，位置和尺寸是相对于屏幕的左上角的。对于子窗口，它们相对于父窗口客户区的左上角。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-movewindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="bRepaint">指示是否要重新绘制窗口。
        /// <para>如果此参数为TRUE，则窗口会收到一条消息。如果参数为FALSE，则不会进行任何重绘。这适用于客户区域，非客户区域（包括标题栏和滚动栏）以及由于移动子窗口而暴露的父窗口的任何部分。</para></param>
        /// <returns>如果函数成功，则返回值为非零。</returns>
        [DllImport(DLL_Name)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        /// <summary>
        /// 检索桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是在其上绘制其他窗口的区域。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getdesktopwindow </para>
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_Name)]
        public extern static IntPtr GetDesktopWindow();


        /// <summary>
        /// 检索指定窗口的边界矩形的尺寸。尺寸以相对于屏幕左上角的屏幕坐标给出。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrect </para>
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpRect">指向一个 RECT 结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零</returns>
        [DllImport(DLL_Name)]
        public static extern bool GetWindowRect(IntPtr hwnd, out WinDef.RECT lpRect);


        /// <summary>
        /// 检索顶级窗口的句柄，该窗口的类名和窗口名与指定的字符串匹配。此功能不搜索子窗口。此功能不执行区分大小写的搜索。
        /// <para>要从指定的子窗口开始搜索子窗口，请使用 FindWindowEx 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-findwindowa </para>
        /// </summary>
        /// 
        /// <param name="lpClassName">[LPCSTR]
        /// 如果 lpClassName 指向一个字符串，则它指定窗口类名称。类名可以是在 RegisterClass 或 RegisterClassEx 中注册的任何名称，也可以是任何预定义的控件类名称。
        /// <para>如果 lpClassName 为NULL，它将找到标题与 lpWindowName 参数匹配的任何窗口</para>
        /// </param>
        /// 
        /// <param name="lpWindowName">[LPCSTR]窗口名称（窗口标题）。如果此参数为 NULL，则所有窗口名称均匹配。</param>
        /// <returns>如果函数成功，返回值为具有指定类名和窗口名的窗口句柄；如果函数失败，返回值为NULL。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_Name, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static IntPtr FindWindowA(string lpClassName, string lpWindowName);


        /// <summary>
        /// 检索指定窗口所属的类的名称。
        /// <para>GetClassNameA(LPSTR), GetClassNameW(LPWSTR)，只是参考 lpClassName 字符类型不同</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getclassname </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类</param>
        /// <param name="lpClassName">[LPTSTR]类名字符串。</param>
        /// <param name="nMaxCount">lpClassName缓冲区的长度，以字符为单位。缓冲区必须足够大以包含终止的空字符。否则，类名字符串将被截断为 nMaxCount-1 字符。</param>
        /// <returns>如果函数成功，则返回值是复制到缓冲区的字符数，不包括终止的空字符。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError函数。</returns>
        [DllImport(DLL_Name, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        /// <summary>
        /// 将指定窗口标题栏的文本（如果有的话）复制到缓冲区中。如果指定的窗口是控件，则复制控件的文本。但是 GetWindowText 无法在另一个应用程序中检索控件的文本。
        /// <para>GetWindowTextA(LPSTR), GetWindowTextW(LPWSTR) </para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowtexta </para>
        /// </summary>
        /// <param name="hWnd">包含文本的窗口或控件的句柄</param>
        /// <param name="lpString">[LPSTR]将接收文本的缓冲区。如果字符串与缓冲区一样长或更长，则字符串将被截断并以空字符终止。</param>
        /// <param name="nMaxCount">要复制到缓冲区的最大字符数，包括空字符。如果文本超过此限制，则会被截断。</param>
        /// <returns>如果函数成功，则返回值是所复制字符串的长度（以字符为单位），不包括终止的空字符。
        /// <para>如果窗口没有标题栏或文本，如果标题栏为空，或者窗口或控件句柄无效，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</para>
        /// </returns>
        [DllImport(DLL_Name, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextA(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    }
}
