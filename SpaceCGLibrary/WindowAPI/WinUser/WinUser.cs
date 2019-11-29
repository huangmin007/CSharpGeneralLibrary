using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpaceCG.WindowAPI.WinUser
{
    /// <summary>
    /// WinUser.h 常用/实用 函数
    /// <para>Marshal.GetLastWin32Error()</para>
    /// <para>LPCTSTR，LPWSTR, PTSTR, LPTSTR，L表示long指针，P表示这是一个指针，T表示_T宏,这个宏用来表示你的字符是否使用UNICODE, 如果你的程序定义了UNICODE或者其他相关的宏，那么这个字符或者字符串将被作为UNICODE字符串，否则就是标准的ANSI字符串。C表示是一个常量,const。STR表示这个变量是一个字符串。</para>
    /// <para>参考： https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ </para>
    /// </summary>
    public static partial class WinUser
    {

        /// <summary>
        /// [原子函数] 更改子窗口，弹出窗口或顶级窗口的大小，位置和Z顺序；这些窗口是根据其在屏幕上的外观排序的；最顶部的窗口获得最高排名，并且是Z顺序中的第一个窗口。
        /// <para>如果使用 SetWindowLong 更改了某些窗口数据，则必须调用 SetWindowPos 才能使更改生效。对 uFlags 使用以下组合：SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED。</para>
        /// <para>示例：SetWindowPos(new WindowInteropHelper(this).Handle., (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_Flags.SWP_NOMOVE | SWP_Flags.SWP_NOSIZE); //将窗口 Z 序设置为最顶</para>
        /// <para>示例：SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, 10, 10, 800, 600, SWP_Flags.SWP_NOZORDER ); //设置窗口大小及位置，忽略 Z 序</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">在Z顺序中位于定位的窗口之前的窗口的句柄。见 <see cref="SwpState"/></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags">窗口大小和位置标志 <see cref="SwpFlag"/></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);
        /// <summary>
        /// 更改子窗口，弹出窗口或顶级窗口的大小，位置和Z顺序；这些窗口是根据其在屏幕上的外观排序的；最顶部的窗口获得最高排名，并且是Z顺序中的第一个窗口。
        /// <para>如果使用 SetWindowLong 更改了某些窗口数据，则必须调用 SetWindowPos 才能使更改生效。对 uFlags 使用以下组合：SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED。</para>
        /// <para>示例：SetWindowPos(new WindowInteropHelper(this).Handle., (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_Flags.SWP_NOMOVE | SWP_Flags.SWP_NOSIZE); //将窗口 Z 序设置为最顶</para>
        /// <para>示例：SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, 10, 10, 800, 600, SWP_Flags.SWP_NOZORDER ); //设置窗口大小及位置，忽略 Z 序</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">在Z顺序中位于定位的窗口之前的窗口位置。见 <see cref="SwpState"/></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags">窗口大小和位置标志 <see cref="SwpFlag"/></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, SwpFlag wFlags);
        /// <summary>
        /// 更改子窗口，弹出窗口或顶级窗口的大小，位置和Z顺序；这些窗口是根据其在屏幕上的外观排序的；最顶部的窗口获得最高排名，并且是Z顺序中的第一个窗口。
        /// <para>如果使用 SetWindowLong 更改了某些窗口数据，则必须调用 SetWindowPos 才能使更改生效。对 uFlags 使用以下组合：SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED。</para>
        /// <para>示例：SetWindowPos(new WindowInteropHelper(this).Handle., (IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_Flags.SWP_NOMOVE | SWP_Flags.SWP_NOSIZE); //将窗口 Z 序设置为最顶</para>
        /// <para>示例：SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, 10, 10, 800, 600, SWP_Flags.SWP_NOZORDER ); //设置窗口大小及位置，忽略 Z 序</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">在Z顺序中位于定位的窗口之前的窗口的值。见 <see cref="SwpState"/> </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags">窗口大小和位置标志 <see cref="SwpFlag"/></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, SwpState hWndInsertAfter, int x, int y, int cx, int cy, SwpFlag wFlags);



        /// <summary>
        /// 设置窗口的窗口区域。窗口区域确定系统允许绘图的窗口区域。系统不会显示位于窗口区域之外的窗口的任何部分。
        /// <para>若要获取窗口的窗口区域，请调用 GetWindowRgn 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowrgn </para>
        /// </summary>
        /// <param name="hWnd">要设置其窗口区域的窗口的句柄。</param>
        /// <param name="hRgn">[HRGN] 区域的句柄。该功能将窗口的窗口区域设置为此区域。如果 hRgn 为NULL，则该函数将窗口区域设置为NULL。</param>
        /// <param name="bRedraw">指定在设置窗口区域后系统是否重画窗口。如果bRedraw为TRUE，则系统将这样做；否则，事实并非如此。通常，如果窗口可见，则将bRedraw设置为TRUE。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern int SetWindowRgn(IntPtr hWnd, ref RECT hRgn, bool bRedraw);

        /// <summary>
        /// 获得一个窗口的窗口区域的副本。通过调用SetWindowRgn函数来设置窗口的窗口区域。窗口区域确定系统允许绘图的窗口区域。系统不会显示位于窗口区域之外的窗口的任何部分。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrgn </para>
        /// </summary>
        /// <param name="hWnd">要获取其窗口区域的窗口句柄</param>
        /// <param name="hRgn">[HRGN] 处理将被修改为代表窗口区域的区域</param>
        /// <returns>返回 <see cref="GwrResult"/> 之一的结果。 </returns>
        [DllImport(DLL_NAME)]
        public static extern int GetWindowRgn(IntPtr hWnd, ref RECT hRgn);


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
        [DllImport(DLL_NAME)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);



        /// <summary>
        /// 将创建指定窗口的线程带入前台并激活该窗口。
        /// <para>键盘输入直接指向窗口，并且为用户更改了各种视觉提示。系统向创建前景窗口的线程分配的优先级比向其他线程分配的优先级高。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setforegroundwindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>如果将窗口带到前台，则返回值为非零。如果未将窗口带到前台，则返回值为零</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 检索前景窗口（用户当前正在使用的窗口）的句柄。系统向创建前景窗口的线程分配的优先级比向其他线程分配的优先级高。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getforegroundwindow</para>
        /// </summary>
        /// <returns>返回值是前景窗口的句柄。在某些情况下，例如某个窗口失去激活状态，前景窗口可以为NULL。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 检索桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是在其上绘制其他窗口的区域。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getdesktopwindow </para>
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public extern static IntPtr GetDesktopWindow();


        /// <summary>
        /// 将窗口句柄检索到附加到调用线程的消息队列的活动窗口。
        /// <para>要获取前景窗口的句柄，可以使用 GetForegroundWindow。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getactivewindow </para>
        /// </summary>
        /// <returns>返回值是附加到调用线程的消息队列的活动窗口的句柄。否则，返回值为NULL。</returns>
        [DllImport(DLL_NAME)]
        public extern static IntPtr GetActiveWindow();
        /// <summary>
        /// 激活一个窗口。该窗口必须附加到调用线程的消息队列。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setactivewindow </para>
        /// </summary>
        /// <param name="hWnd">要激活的顶层窗口的句柄。</param>
        /// <returns>如果函数成功，则返回值是先前处于活动状态的窗口的句柄。如果函数失败，则返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public extern static IntPtr SetActiveWindow(IntPtr hWnd);

        /// <summary>
        /// 检索指定窗口的边界矩形的尺寸。尺寸以相对于屏幕左上角的屏幕坐标给出。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrect </para>
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpRect">指向一个 RECT 结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


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
        [DllImport(DLL_NAME, EntryPoint = "FindWindowA")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 检索指定窗口所属的类的名称。
        /// <para>GetClassNameA(LPSTR), GetClassNameW(LPWSTR)，只是参考 lpClassName 字符类型不同</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getclassname </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类</param>
        /// <param name="lpClassName">[LPSTR]类名字符串。
        /// <para>注意：这里 lpClassName 需要设置容量大小，否则会出现意外的错误；例如：StringBuilder sb = new StringBuilder(255); </para></param>
        /// <param name="nMaxCount">lpClassName 缓冲区的长度，以字符为单位。缓冲区必须足够大以包含终止的空字符。否则，类名字符串将被截断为 nMaxCount-1 字符。</param>
        /// <returns>如果函数成功，则返回值是复制到缓冲区的字符数，不包括终止的空字符。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError 函数。</returns>
        [DllImport(DLL_NAME, EntryPoint = "GetClassNameA", SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// 将指定窗口标题栏的文本（如果有的话）复制到缓冲区中。如果指定的窗口是控件，则复制控件的文本。但是 GetWindowText 无法在另一个应用程序中检索控件的文本。
        /// <para>GetWindowTextA(LPSTR), GetWindowTextW(LPWSTR) </para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowtexta </para>
        /// </summary>
        /// <param name="hWnd">包含文本的窗口或控件的句柄</param>
        /// <param name="lpString">[LPSTR]将接收文本的缓冲区。如果字符串与缓冲区一样长或更长，则字符串将被截断并以空字符终止。
        /// <para>注意：这里 lpString 最好是设置容量大小，例如：StringBuilder sb = new StringBuilder(255); </para></param>
        /// <param name="nMaxCount">要复制到缓冲区的最大字符数，包括空字符。如果文本超过此限制，则会被截断。</param>
        /// <returns>如果函数成功，则返回值是所复制字符串的长度（以字符为单位），不包括终止的空字符。
        /// <para>如果窗口没有标题栏或文本，如果标题栏为空，或者窗口或控件句柄无效，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</para>
        /// </returns>
        [DllImport(DLL_NAME, EntryPoint = "GetWindowTextA", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 更改指定窗口标题栏的文本（如果有的话）。如果指定的窗口是控件，则更改控件的文本。但是，SetWindowText 无法在另一个应用程序中更改控件的文本。
        /// <para>要在另一个进程中设置控件的文本，请直接发送 WM_SETTEXT 消息，而不是调用 SetWindowText。 </para>
        /// <para>该函数 SetWindowText 函数不展开制表符（ASCII代码0×09）。制表符显示为竖线（|）字符。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowtexta </para>
        /// </summary>
        /// <param name="hWnd">要更改其文本的窗口或控件的句柄。</param>
        /// <param name="lpString">[LPCSTR] 新标题或控件文本</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "SetWindowTextA", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, String lpString);
        /// <summary>
        /// 更改指定窗口标题栏的文本（如果有的话）。如果指定的窗口是控件，则更改控件的文本。但是，SetWindowText 无法在另一个应用程序中更改控件的文本。
        /// <para>要在另一个进程中设置控件的文本，请直接发送 WM_SETTEXT 消息，而不是调用 SetWindowText。 </para>
        /// <para>该函数 SetWindowText 函数不展开制表符（ASCII代码0×09）。制表符显示为竖线（|）字符。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowtexta </para>
        /// </summary>
        /// <param name="hWnd">要更改其文本的窗口或控件的句柄。</param>
        /// <param name="lpString">[LPCSTR] 新标题或控件文本</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "SetWindowTextA", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, StringBuilder lpString);
        /// <summary>
        /// 启用或禁用向指定窗口或控件的鼠标和键盘输入。禁用输入后，该窗口不会接收到诸如鼠标单击和按键之类的输入。启用输入后，窗口将接收所有输入。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-enablewindow </para>
        /// </summary>
        /// <param name="hWnd">要启用或禁用的窗口句柄。</param>
        /// <param name="bEnable">指示是启用还是禁用窗口。如果此参数为 TRUE，则启用窗口。如果参数为 FALSE，则禁用窗口。</param>
        /// <returns>如果以前禁用了窗口，则返回值为非零。如果该窗口先前未禁用，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        /// <summary>
        /// 确定指定窗口的可见性状态。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iswindowvisible </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果指定的窗口，其父窗口，其父级的父窗口等具有 WS_VISIBLE 样式，则返回值为非零。否则，返回值为零。
        /// <para>因为返回值指定窗口是否具有 WS_VISIBLE 样式，所以即使该窗口被其他窗口完全遮盖了，返回值也可能为非零。</para>
        /// </returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        /// <summary>
        /// 确定窗口是否最大化。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iszoomed </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果缩放窗口，则返回值为非零。如果窗口未缩放，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsZoomed(IntPtr hWnd);
        /// <summary>
        /// 设置窗口的显示状态。
        /// <para>要在显示或隐藏窗口时执行某些特殊效果，请使用 AnimateWindow。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-showwindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow">[int] 控制窗口的显示方式 <see cref="SwCmd"/></param>
        /// <returns>如果该窗口以前是可见的，则返回值为非零。如果该窗口以前是隐藏的，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool ShowWindow(IntPtr hWnd, SwCmd nCmdShow);
        /// <summary>
        /// 设置指定窗口的显示状态，而无需等待操作完成。
        /// <para>要在显示或隐藏窗口时执行某些特殊效果，请使用 AnimateWindow。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-showwindowasync </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow">[int] 控制窗口的显示方式 <see cref="SwCmd"/></param>
        /// <returns>如果该窗口以前是可见的，则返回值为非零。如果该窗口以前是隐藏的，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, SwCmd nCmdShow);
        /// <summary>
        /// 在显示或隐藏窗口时使您产生特殊效果。动画有四种类型：滚动，滑动，折叠或展开以及alpha混合淡入。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-animatewindow </para>
        /// </summary>
        /// <param name="hWnd">窗口动画的句柄。调用线程必须拥有此窗口。</param>
        /// <param name="dwTime">播放动画所需的时间（以毫秒为单位）。通常，动画播放需要200毫秒。</param>
        /// <param name="dwFlags">动画的类型</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。获取扩展的错误信息，请调用GetLastError函数。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool AnimateWindow(IntPtr hWnd, int dwTime, AwFlag dwFlags);
        /// <summary>
        /// 最小化（但不破坏）指定的窗口。
        /// <para>要销毁窗口，应用程序必须使用 DestroyWindow 函数。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-closewindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool CloseWindow(IntPtr hWnd);
        /// <summary>
        /// 销毁指定的窗口。
        /// <para>如果指定的窗口是父窗口或所有者窗口，则DestroyWindow在销毁父窗口或所有者窗口时会自动销毁关联的子窗口或所有者窗口。该函数首先销毁子窗口或所有者窗口，然后销毁父窗口或所有者窗口。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-destroywindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hWnd);
        /// <summary>
        /// 将焦点切换到指定的窗口，并将其置于前景。
        /// <para>通常调用此函数来维护窗口z顺序。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-switchtothiswindow </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄。</param>
        /// <param name="fUnknown">TRUE 此参数指示窗口正在被切换到使用Alt/CTL + Tab键序列。否则，此参数应为 FALSE。</param>
        [DllImport(DLL_NAME)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fUnknown);


        /// <summary>
        /// 闪烁指定的窗口一次。它不会更改窗口的活动状态。
        /// <para>若要将窗口刷新指定的次数，请使用 FlashWindowEx 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-flashwindow </para>
        /// </summary>
        /// <param name="hWnd">要刷新的窗口的句柄。窗口可以打开或最小化。</param>
        /// <param name="bInvert">如果此参数为 TRUE，则窗口从一种状态闪烁到另一种状态。如果为 FALSE，则窗口将返回其原始状态（活动或不活动）。
        /// <para>当最小化应用程序且此参数为 TRUE 时，任务栏窗口按钮将闪烁活动/不活动。如果为 FALSE，则任务栏窗口按钮将不活动地闪烁，这意味着它不会更改颜色。它会闪烁，就像正在重绘一样，但不会向用户提供视觉上的反转提示。</para>
        /// </param>
        /// <returns>返回值指定在调用FlashWindow函数之前窗口的状态 。如果在调用之前将窗口标题绘制为活动窗口，则返回值为非零。否则，返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
        /// <summary>
        /// 闪烁指定的窗口。它不会更改窗口的活动状态。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-flashwindowex </para>
        /// </summary>
        /// <param name="pfwi">[PFLASHWINFO]指向 FLASHWINFO 结构的指针</param>
        /// <returns>返回值指定在调用 FlashWindowEx 函数之前窗口的状态 。如果在调用之前将窗口标题绘制为活动窗口，则返回值为非零。否则，返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool FlashWindowEx(ref FLASHINFO pfwi);
        /// <summary>
        /// 触发视觉信号以指示正在播放声音。
        /// <para>通过使用 SPI_SETSOUNDSENTRY 值调用 SystemParametersInfo 来设置通知行为。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-soundsentry </para>
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern bool SoundSentry();


        /// <summary>
        /// 如果窗口附加到调用线程的消息队列，则检索具有键盘焦点的窗口的句柄。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getfocus </para>
        /// </summary>
        /// <returns>返回值是具有键盘焦点的窗口的句柄。如果调用线程的消息队列没有与键盘焦点相关联的窗口，则返回值为NULL。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetFocus();
        /// <summary>
        /// 检索已捕获鼠标的窗口的句柄（如果有）。一次只能捕获一个窗口。无论光标是否在其边界内，此窗口都会接收鼠标输入。
        /// <para>一个NULL的返回值意味着当前线程未捕获鼠标。但是，很可能另一个线程或进程捕获了鼠标。要获取另一个线程上的捕获窗口的句柄，请使用 GetGUIThreadInfo 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcapture </para>
        /// </summary>
        /// <returns>返回值是与当前线程关联的捕获窗口的句柄。如果线程中没有窗口捕获到鼠标，则返回值为 NULL。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetCapture();

        /// <summary>
        /// 表示无法关闭系统，并设置启动系统关闭后要显示给用户的原因字符串。
        /// <para>只能从创建由hWnd参数指定的窗口的线程中调用此函数。否则，函数将失败，最后的错误代码是 ERROR_ACCESS_DENIED。</para>
        /// <para>应用程序在开始无法中断的操作（例如刻录CD或DVD）时应调用此函数。操作完成后，请调用 ShutdownBlockReasonDestroy 函数以指示可以关闭系统。</para>
        /// <para>由于用户通常在系统关闭时很着急，因此他们可能只花几秒钟的时间查看系统显示的关闭原因。因此，重要的是您的原因字符串必须简短明了。例如，“正在进行CD刻录。” 优于“此应用程序阻止系统关闭，因为正在进行CD刻录。请勿关闭。”</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-shutdownblockreasoncreate </para>
        /// </summary>
        /// <param name="hWnd">应用程序主窗口的句柄。</param>
        /// <param name="pwszReason">[LPCWSTR] 应用程序必须阻止系统关闭的原因。该字符串将在显示 MAX_STR_BLOCKREASON 个字符后被截断。</param>
        /// <returns>如果调用成功，则返回值为非零。如果调用失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, string pwszReason);
        /// <summary>
        /// 表示可以关闭系统并释放原因字符串。
        /// <para>只能从创建由hWnd参数指定的窗口的线程中调用此函数。否则，函数将失败，最后的错误代码是 ERROR_ACCESS_DENIED。</para>
        /// <para>如果先前已通过 ShutdownBlockReasonCreate 函数阻止了系统关闭，则此函数将释放原因字符串。否则，此功能为无操作。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-shutdownblockreasondestroy </para>
        /// </summary>
        /// <param name="hWnd">应用程序主窗口的句柄。</param>
        /// <returns>如果调用成功，则返回值为非零。如果调用失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);


        /// <summary>
        /// 检索当前光标的句柄。
        /// <para>要获取有关全局游标的信息，即使它不是当前线程所有，也可以使用 GetCursorInfo </para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcursor </para>
        /// </summary>
        /// <returns>返回值(HCURSOR)是当前游标的句柄。如果没有游标，则返回值为NULL。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetCursor();

        /// <summary>
        /// 检索有关全局游标的信息。
        /// <para>CURSORINFO pci = new CURSORINFO(){ cbSize = Marshal.SizeOf(typeof(CURSORINFO)) };</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcursorinfo </para>
        /// </summary>
        /// <param name="pci">[PCURSORINFO]指向接收信息的CURSORINFO结构的指针。请注意，在调用此函数之前，必须将cbSize成员设置为sizeof(CURSORINFO)。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);

        /// <summary>
        /// 检索鼠标光标在屏幕坐标中的位置。
        /// <para>光标位置始终在屏幕坐标中指定，并且不受包含光标的窗口的映射模式的影响。调用过程必须对窗口站具有 WINSTA_READATTRIBUTES 访问权限。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcursorpos </para>
        /// </summary>
        /// <param name="lpPoint">[LPPOINT]指向POINT结构的指针，该结构接收光标的屏幕坐标。</param>
        /// <returns>如果成功返回非零，否则返回零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        /// <summary>
        /// 返回系统 DPI。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getdpiforsystem </para>
        /// </summary>
        /// <returns>返回 DPI 值</returns>
        [DllImport(DLL_NAME)]
        public static extern int GetDpiForSystem();

        /// <summary>
        /// 返回关联窗口的每英寸点数（dpi）值。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getdpiforwindow </para>
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>窗口的DPI取决于窗口的 DPI_AWARENESS。有关更多信息，请参见备注。无效的hwnd值将导致返回值为0。</returns>
        [DllImport(DLL_NAME)]
        public static extern int GetDpiForWindow(IntPtr hWnd);


        /// <summary>
        /// 确定在调用线程的消息队列中是否有鼠标按钮或键盘消息。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getinputstate </para>
        /// </summary>
        /// <returns>如果队列包含一个或多个新的鼠标按钮或键盘消息，则返回值为非零。如果队列中没有新的鼠标按钮或键盘消息，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetInputState();

        /// <summary>
        /// 将256个虚拟键的状态复制到指定的缓冲区。
        /// <para>要检索单个键的状态信息，请使用 GetKeyState 函数。若要检索单个键的当前状态，而不管是否已从消息队列中检索到相应的键盘消息，请使用 GetAsyncKeyState 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getkeyboardstate </para>
        /// </summary>
        /// <param name="lpKeyState">[PBYTE]接收每个虚拟密钥的状态数据的256字节数组。
        /// <para>函数返回时，lpKeyState参数指向的数组的每个成员都 包含虚拟键的状态数据。</para>
        /// <para>如果高位为1，则按键按下；否则为0。否则，它会上升。如果键是切换键（例如CAPS LOCK），则切换键时低位为1；如果取消切换，则低位为0。对于非拨动键，低位无意义。</para>
        /// <para>切换键在打开时被称为切换键。切换键时，键盘上的切换键指示灯（如果有）将亮起；如果不切换键，则指示灯将熄灭。</para>
        /// </param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool GetKeyboardState(ref byte[] lpKeyState);

        /// <summary>
        /// 检索指定虚拟键的状态。状态指定按键是向上，向下还是切换（打开，关闭-每次按下按键时交替显示）。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getkeystate </para>
        /// </summary>
        /// <param name="nVirtKey">虚拟键 <see cref="VirtualKeyCode"/>。如果所需的虚拟键是字母或数字（A到Z，a到z或0到9）， 则必须将nVirtKey设置为该字符的ASCII值。对于其他密钥，它必须是虚拟密钥代码。</param>
        /// <returns>返回值指定指定虚拟键的状态，如下所示：
        /// <para>如果高位为1，则按键按下；否则为0。否则，它会上升。</para>
        /// <para>如果低位为1，则切换键。如果打开了一个键（例如CAPS LOCK键），则会对其进行切换。如果低位为0，则此键处于关闭状态且不切换。切换键时，键盘上的切换键指示灯（如果有）将亮起，而当取消切换键时，其指示灯将熄灭。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern short GetKeyState(int nVirtKey);
        /// <summary>
        /// 检索指定虚拟键的状态。状态指定按键是向上，向下还是切换（打开，关闭-每次按下按键时交替显示）。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getkeystate </para>
        /// </summary>
        /// <param name="nVirtKey">虚拟键 <see cref="VirtualKeyCode"/>。如果所需的虚拟键是字母或数字（A到Z，a到z或0到9）， 则必须将nVirtKey设置为该字符的ASCII值。对于其他密钥，它必须是虚拟密钥代码。</param>
        /// <returns>返回值指定指定虚拟键的状态，如下所示：
        /// <para>如果高位为1，则按键按下；否则为0。否则，它会上升。</para>
        /// <para>如果低位为1，则切换键。如果打开了一个键（例如CAPS LOCK键），则会对其进行切换。如果低位为0，则此键处于关闭状态且不切换。切换键时，键盘上的切换键指示灯（如果有）将亮起，而当取消切换键时，其指示灯将熄灭。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern short GetKeyState(VirtualKeyCode nVirtKey);

        /// <summary>
        /// 确定在调用函数时按键是向上还是向下，以及在先前调用GetAsyncKeyState之后是否按下了该键。
        /// <para>该 GetAsyncKeyState 功能可与鼠标按钮。但是，它检查物理鼠标按钮的状态，而不是检查物理按钮映射到的逻辑鼠标按钮的状态。</para>
        /// <para>例如，调用GetAsyncKeyState（VK_LBUTTON）始终返回物理鼠标左键的状态，而不管它是映射到逻辑鼠标左键还是逻辑右键。您可以通过调用确定系统当前的物理鼠标按钮到逻辑鼠标按钮的映射GetSystemMetrics(SM_SWAPBUTTON)。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getasynckeystate </para>
        /// </summary>
        /// <param name="vKey">虚拟密钥代码 <see cref="VirtualKeyCode"/>，您可以使用左和右区分常数来指定某些键</param>
        /// <returns>如果函数成功，则返回值指定自上次调用GetAsyncKeyState以来是否按下了该键，以及该键当前处于向上还是向下。
        /// <para>如果设置了最高有效位，则该键处于按下状态；如果设置了最低有效位，则在上一次调用GetAsyncKeyState之后按下了该键。但是，您不应该依赖于此最后的行为。</para>
        /// <para>在以下情况下，返回值为零：</para>
        /// <para>1.当前桌面不是活动桌面</para>
        /// <para>2.前台线程属于另一个进程，并且桌面不允许挂钩或日志记录。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern short GetAsyncKeyState(int vKey);
        /// <summary>
        /// 确定在调用函数时按键是向上还是向下，以及在先前调用GetAsyncKeyState之后是否按下了该键。
        /// <para>该 GetAsyncKeyState 功能可与鼠标按钮。但是，它检查物理鼠标按钮的状态，而不是检查物理按钮映射到的逻辑鼠标按钮的状态。</para>
        /// <para>例如，调用GetAsyncKeyState（VK_LBUTTON）始终返回物理鼠标左键的状态，而不管它是映射到逻辑鼠标左键还是逻辑右键。您可以通过调用确定系统当前的物理鼠标按钮到逻辑鼠标按钮的映射GetSystemMetrics(SM_SWAPBUTTON)。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getasynckeystate </para>
        /// </summary>
        /// <param name="vKey">虚拟密钥代码 <see cref="VirtualKeyCode"/>，您可以使用左和右区分常数来指定某些键</param>
        /// <returns>如果函数成功，则返回值指定自上次调用GetAsyncKeyState以来是否按下了该键，以及该键当前处于向上还是向下。
        /// <para>如果设置了最高有效位，则该键处于按下状态；如果设置了最低有效位，则在上一次调用GetAsyncKeyState之后按下了该键。但是，您不应该依赖于此最后的行为。</para>
        /// <para>在以下情况下，返回值为零：</para>
        /// <para>1.当前桌面不是活动桌面</para>
        /// <para>2.前台线程属于另一个进程，并且桌面不允许挂钩或日志记录。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern short GetAsyncKeyState(VirtualKeyCode vKey);

        /// <summary>
        /// 将虚拟键代码转换（映射）为扫描代码或字符值，或将扫描代码转换为虚拟键代码。
        /// <para>要指定用于翻译指定代码的键盘布局的句柄，请使用 MapVirtualKeyEx 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-mapvirtualkeya </para>
        /// </summary>
        /// <param name="uCode"><see cref="VirtualKeyCode"/> 或扫描代码。如何解释此值取决于 uMapType 参数的值。</param>
        /// <param name="uMapType">参数的值取决于 uCode 参数的值 <see cref="MapVKType"/></param>
        /// <returns>返回值可以是扫描代码，虚拟键代码或字符值，具体取决于 uCode 和 uMapType 的值。如果没有转换，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern uint MapVirtualKeyA(uint uCode, MapVKType uMapType);

        /// <summary>
        /// 合成击键，鼠标动作和按钮单击。
        /// <para>第三方库：https://github.com/michaelnoonan/inputsimulator </para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-sendinput </para>
        /// </summary>
        /// <param name="cInputs">pInputs数组中的结构数</param>
        /// <param name="pInputs">[LPINPUT] INPUT 结构的数组。每个结构代表一个要插入键盘或鼠标输入流的事件。</param>
        /// <param name="cbSize">INPUT 结构的大小（以字节为单位）。如果 cbSize 不是INPUT结构的大小，则该函数失败。 大小应为 Marshal.SizeOf(typeof(INPUT))</param>
        /// <returns>该函数返回成功插入键盘或鼠标输入流中的事件数。如果函数返回零，则输入已经被另一个线程阻塞。要获取扩展的错误信息，请调用GetLastError。
        /// <para>当UIPI阻止此功能时，该功能将失败。请注意，GetLastError和返回值都不会指示失败是由UIPI阻塞引起的。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint SendInput(uint cInputs, INPUT[] pInputs, int cbSize);

        /// <summary>
        /// 检索当前线程的额外消息（附加对象）信息。额外的消息信息是与当前线程的消息队列关联的应用程序或驱动程序定义的值。
        /// <para>若要设置线程的额外消息信息，请使用SetMessageExtraInfo函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getmessageextrainfo </para>
        /// </summary>
        /// <returns>[LPARAM] 返回值指定额外的信息。额外信息的含义是特定于设备的。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetMessageExtraInfo();

        /// <summary>
        /// 定义系统范围的热键。
        /// <para>此功能无法将热键与另一个线程创建的窗口关联。如果为热键指定的击键已经被另一个热键注册，则 RegisterHotKey 失败。</para>
        /// <para>如果已经存在具有相同 hWnd 和 id 参数的热键，则将其与新的热键一起维护。应用程序必须显式调用 UnregisterHotKey 来注销旧的热键。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-registerhotkey </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄，它将接收由热键生成的 WM_HOTKEY 消息。如果此参数为 NULL，则将 WM_HOTKEY 消息发布到调用线程的消息队列中，并且必须在消息循环中进行处理。</param>
        /// <param name="id">热键的标识符。如果 hWnd 参数为 NULL，则热键与当前线程关联，而不与特定窗口关联。如果已经存在具有相同 hWnd 和 id 参数的热键。</param>
        /// <param name="fsModifiers">必须将这些键与 uVirtKey 参数指定的键组合在一起 才能生成 WM_HOTKEY 消息</param>
        /// <param name="vk">热键的虚拟键代码 <see cref="VirtualKeyCode"/></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, RhkModifier fsModifiers, VirtualKeyCode vk);
        /// <summary>
        /// 释放先前由调用线程注册的热键。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-unregisterhotkey </para>
        /// </summary>
        /// <param name="hWnd">与要释放的热键关联的窗口的句柄。如果热键未与窗口关联，则此参数应为 NULL。</param>
        /// <param name="id">要释放的热键的标识符。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);




        /// <summary>
        /// 将指定的消息发送到一个或多个窗口。该SendMessage函数的函数调用指定的窗口的窗口过程，并不会返回，直到窗口过程已经处理了该消息。
        /// <para>需要使用 HWND_BROADCAST 进行通信的应用程序应使用 RegisterWindowMessage 函数来获取用于应用程序间通信的唯一消息。</para>
        /// <para>要发送消息并立即返回，请使用 SendMessageCallback 或 SendNotifyMessage 函数。要将消息发布到线程的消息队列中并立即返回，请使用 PostMessage 或 PostThreadMessage 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-sendmessage </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄，其窗口过程将接收到该消息。如果此参数为 HWND_BROADCAST((HWND)0xFFFF)，则消息将发送到系统中的所有顶级窗口，包括禁用或不可见的无主窗口，重叠的窗口和弹出窗口；但是消息不会发送到子窗口。
        /// <para>消息发送受 UIPI 约束。进程的线程只能将消息发送到完整性级别较低或相等的进程中的线程的消息队列。</para></param>
        /// <param name="Msg">要发送的消息。</param>
        /// <param name="wParam">其他特定于消息的信息。</param>
        /// <param name="lParam">其他特定于消息的信息。</param>
        /// <returns>返回值指定消息处理的结果；这取决于发送的消息。使用 GetLastError 检索错误。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, MsgFlag Msg, int wParam, int lParam);
        /// <summary>
        /// 将消息放置（张贴）在与创建指定窗口的线程相关联的消息队列中，并在不等待线程处理消息的情况下返回消息。
        /// <para>要将消息发布到与线程关联的消息队列中，请使用 PostThreadMessage 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-postmessagea </para>
        /// </summary>
        /// <param name="hWnd">窗口的句柄，其窗口过程将接收消息。特殊值：
        /// <para>HWND_BROADCAST((HWND)0xFFFF) 该消息将发布到系统中的所有顶级窗口，包括禁用或不可见的无主窗口，重叠的窗口和弹出窗口。该消息未发布到子窗口。</para>
        /// <para>NULL 该函数的行为就像到呼叫 PostThreadMessage 与 dwThreadId 参数集到当前线程的标识符。</para>
        /// </param>
        /// <param name="Msg">要发布的消息类型</param>
        /// <param name="wParam">[WPARAM] 其他特定于消息的信息。</param>
        /// <param name="lParam">[LPARAM] 其他特定于消息的信息。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, MsgFlag Msg, int wParam, int lParam);
        /// <summary>
        /// 从调用线程的消息队列中检索消息。该函数分派传入的已发送消息，直到已发布的消息可供检索为止。与GetMessage不同，PeekMessage函数在返回之前不等待消息发布。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getmessage </para>
        /// </summary>
        /// <param name="lpMsg">[LPMSG] 指向MSG结构的指针，该结构从线程的消息队列接收消息信息。</param>
        /// <param name="hWnd">要获取其消息的窗口的句柄。该窗口必须属于当前线程。
        /// <para>如果 hWnd 为 NULL，则 GetMessage 检索属于当前线程的任何窗口的消息，以及当前线程的消息队列中 hwnd 值为 NULL 的消息（请参阅MSG结构）。因此，如果 hWnd 为 NULL，则将同时处理窗口消息和线程消息。</para>
        /// <para>如果 hWnd 为 -1，则 GetMessage 仅检索当前线程的消息队列中其 hwnd 值为 NULL 的消息，即由 PostMessage（当 hWnd 参数为 NULL）或 PostThreadMessage 发布的线程消息 。</para>
        /// </param>
        /// <param name="wMsgFilterMin">要检索的最低消息值的整数值。使用WM_KEYFIRST（0x0100）指定第一条键盘消息，或使用WM_MOUSEFIRST（0x0200）指定第一条鼠标消息。</param>
        /// <param name="wMsgFilterMax">要检索的最高消息值的整数值。使用WM_KEYLAST指定最后的键盘消息，或使用WM_MOUSELAST指定最后的鼠标消息。</param>
        /// <returns>如果函数检索到 WM_QUIT 以外的消息，则返回值为非零。如果该函数检索 WM_QUIT 消息，则返回值为零。
        /// <para>如果有错误，则返回值为-1。例如，如果 hWnd 是无效的窗口句柄或 lpMsg 是无效的指针，该函数将失败。要获取扩展的错误信息，请调用 GetLastError。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool GetMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        /// <summary>
        /// 调度传入的已发送消息，检查线程消息队列中是否有已发布消息，并检索消息（如果存在）。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-peekmessagea </para>
        /// </summary>
        /// <param name="lpMsg">[LPMSG] 指向接收消息信息的MSG结构的指针。</param>
        /// <param name="hWnd">要获取其消息的窗口的句柄。该窗口必须属于当前线程。
        /// <para>如果 hWnd 为 NULL，则 PeekMessage 检索属于当前线程的任何窗口的消息，以及当前线程的消息队列中 hwnd 值为 NULL 的消息（请参阅MSG结构）。因此，如果 hWnd 为 NULL，则将同时处理窗口消息和线程消息。</para>
        /// <para>如果 hWnd 为 -1，则 PeekMessage 仅检索当前线程的消息队列中其 hwnd 值为 NULL 的消息，即，由 PostMessage（当 hWnd 参数为 NULL）或 PostThreadMessage 发布的线程消息 。</para>
        /// </param>
        /// <param name="wMsgFilterMin">在要检查的消息范围内的第一条消息的值。使用 WM_KEYFIRST（0x0100）指定第一条键盘消息，或使用 WM_MOUSEFIRST（0x0200）指定第一条鼠标消息。
        ///     <para>如果 wMsgFilterMin 和 wMsgFilterMax 都为零，则 PeekMessage 返回所有可用消息（即，不执行范围过滤）。</para>
        /// </param>
        /// <param name="wMsgFilterMax">要检查的消息范围中的最后一条消息的值。使用 WM_KEYLAST 指定最后的键盘消息，或使用 WM_MOUSELAST 指定最后的鼠标消息。
        ///     <para>如果 wMsgFilterMin 和 wMsgFilterMax 都为零，则 PeekMessage 返回所有可用消息（即，不执行范围过滤）。</para>
        /// </param>
        /// <param name="wRemoveMsg">指定如何处理消息 <see cref="PmFlag"/>。</param>
        /// <returns>如果有消息，则返回值为非零。如果没有可用消息，则返回值为零。</returns>
        [DllImport(DLL_NAME, EntryPoint = "PeekMessageA")]
        public static extern bool PeekMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);


        /// <summary>
        /// 为调用的应用程序配置触摸注入上下文，并初始化该应用程序可以注入的最大同时 接触 数量。
        /// <para>注意：InitializeTouchInjection 必须在对 InjectTouchInput 的任何调用之前。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-initializetouchinjection </para>
        /// </summary>
        /// <param name="maxCount">触摸触点的最大数量。
        ///     <para>所述 MAXCOUNT 参数必须大于 0 且小于或等于 MAX_TOUCH_COUNT（256）如在 WINUSER.H 定义。</para>
        /// </param>
        /// <param name="dwMode">接触反馈模式 <see cref="TouchFeedbackMode"/>。该 dwMode 参数必须是 TOUCH_FEEDBACK_DEFAULT，TOUCH_FEEDBACK_INDIRECT，或 TOUCH_FEEDBACK_NONE。</param>
        /// <returns>如果函数成功，则返回值为TRUE。如果函数失败，则返回值为FALSE。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool InitializeTouchInjection(uint maxCount, TouchFeedbackMode dwMode);
        /// <summary>
        /// 模拟触摸输入。
        /// <para>注意：InitializeTouchInjection 必须在对 InjectTouchInput 的任何调用之前。</para>
        /// <para>如果指定 POINTER_INFO.PerformanceCount 字段，则在实际注入时，时间戳将以 0.1 毫秒的分辨率转换为当前时间。如果自定义 PerformanceCount 导致与上一次注入相同的.1毫秒窗口，则API将返回错误（ERROR_NOT_READY），并且不会注入数据。虽然不会立即因错误使注入无效，但下一次成功的注入必须具有 PerformanceCount 值，该值与先前成功的注入之间至少相隔 0.1 毫秒。同样，如果使用该字段，则自定义 dwTime 值必须至少相隔 1 毫秒。</para>
        /// <para>如果在注入参数中同时指定了 dwTime 和 PerformanceCount，则 InjectTouchInput 失败，并显示错误代码（ERROR_INVALID_PARAMETER）。一旦注入应用程序以 dwTime 或 PerformanceCount 参数启动，则时间戳记字段必须正确填写。一旦注入序列开始，注入就无法将自定义时间戳字段从一个切换为另一个。</para>
        /// <para>如果未指定 dwTime 或 PerformanceCount 值，则 InjectTouchInput 会根据API调用的时间分配时间戳。如果调用之间的间隔小于0.1毫秒，则API可能返回错误（ERROR_NOT_READY）。该错误不会立即使输入无效，但是注入应用程序需要再次重试同一帧以确保注入成功。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-injecttouchinput </para>
        /// </summary>
        /// <param name="count">contacts 中数组的大小；计数的最大值由 InitializeTouchInjection 函数的 maxCount 参数指定。</param>
        /// <param name="contacts">代表桌面上所有 contacts 的 POINTER_TOUCH_INFO 结构的数组。每个 contact 的屏幕坐标必须在桌面范围内。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool InjectTouchInput(uint count, POINTERTOUCHINFO[] contacts);
    }
}
