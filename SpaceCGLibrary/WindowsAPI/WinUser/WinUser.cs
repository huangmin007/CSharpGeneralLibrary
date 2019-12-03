using System;
using System.Text;
using System.Runtime.InteropServices;

namespace SpaceCG.WindowsAPI.WinUser
{
    /// <summary>
    /// WinUser.h 常用/实用 函数
    /// <para>Marshal.GetLastWin32Error();  new WindowInteropHelper(Window).Handle; KeyInterop.KeyFromVirtualKey((int)vkCode); </para>
    /// <para>ComponentDispatcher (OR) HwndSource.FromHwnd(hwnd).AddHook(WindowProcHandler); (OR) HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;hwndSource.AddHook(new HwndSourceHook(WindowProcHandler));</para>
    /// <para>#ifdef UNICODE #define Function FunctionA #else #define Function FunctionW #endif</para>
    /// <para>如果窗口类是使用 ANSI 版本的 RegisterClass（RegisterClassA）注册的，则窗口的字符集是 ANSI。如果窗口类是使用 Unicode 版本的 RegisterClass（RegisterClassW）注册的，则窗口的字符集为 Unicode。</para>
    /// <para>LPCTSTR，LPWSTR, PTSTR, LPTSTR，L表示long指针，P表示这是一个指针，T表示 _T宏 这个宏用来表示你的字符是否使用 UNICODE, 如果你的程序定义了 UNICODE 或者其他相关的宏，那么这个字符或者字符串将被作为 UNICODE 字符串，否则就是标准的 ANSI 字符串。C表示是一个常量const。STR表示这个变量是一个字符串。</para>
    /// <para>参考： https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ </para>
    /// </summary>
    public static partial class WinUser
    {

        #region Set Window x,y,z,width,height
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
        [return: MarshalAs(UnmanagedType.Bool)]
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
        public static extern bool SetWindowPos(IntPtr hWnd, [MarshalAs(UnmanagedType.SysInt)]SwpState hWndInsertAfter, int x, int y, int cx, int cy, SwpFlag wFlags);
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
        #endregion


        #region Window Rect
        /// <summary>
        /// 设置窗口的窗口区域。窗口区域确定系统允许绘图的窗口区域。系统不会显示位于窗口区域之外的窗口的任何部分。
        /// <para>若要获取窗口的窗口区域，请调用 GetWindowRgn 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-setwindowrgn </para>
        /// </summary>
        /// <param name="hWnd">要设置其窗口区域的窗口的句柄。</param>
        /// <param name="hRgn">[HRGN] 区域的句柄。该功能将窗口的窗口区域设置为此区域。如果 hRgn 为 NULL，则该函数将窗口区域设置为 NULL。</param>
        /// <param name="bRedraw">指定在设置窗口区域后系统是否重画窗口。如果bRedraw为TRUE，则系统将这样做；否则，事实并非如此。通常，如果窗口可见，则将bRedraw设置为TRUE。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern int SetWindowRgn(IntPtr hWnd, ref RECT hRgn, bool bRedraw);
        /// <summary>
        /// 获得一个窗口的窗口区域的副本。通过调用 SetWindowRgn 函数来设置窗口的窗口区域。窗口区域确定系统允许绘图的窗口区域。系统不会显示位于窗口区域之外的窗口的任何部分。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrgn </para>
        /// </summary>
        /// <param name="hWnd">要获取其窗口区域的窗口句柄</param>
        /// <param name="hRgn">[HRGN] 处理将被修改为代表窗口区域的区域</param>
        /// <returns>返回 <see cref="GwrResult"/> 之一的结果。 </returns>
        [DllImport(DLL_NAME)]
        public static extern int GetWindowRgn(IntPtr hWnd, out RECT hRgn);
        /// <summary>
        /// 检索指定窗口的边界矩形的尺寸。尺寸以相对于屏幕左上角的屏幕坐标给出。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrect </para>
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpRect">指向一个 <see cref="RECT"/> 结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        #endregion


        #region RECT Functions
        /// <summary>
        /// CopyRect 功能复制一个矩形的坐标到另一个。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-copyrect </para>
        /// </summary>
        /// <param name="lprc">指向 RECT 结构的指针，该结构接收源矩形的逻辑坐标。</param>
        /// <param name="lprcSrc">指向要以逻辑单位复制其坐标的RECT结构的指针。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool CopyRect(out RECT lprc, ref RECT lprcSrc);
        /// <summary>
        /// SetRect 对功能设置指定矩形的坐标。这等效于将 left，top，right 和 bottom 参数分配给RECT结构的适当成员。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setrect </para>
        /// </summary>
        /// <param name="lprc">指向包含要设置的矩形的 RECT 结构的指针。</param>
        /// <param name="xLeft">指定矩形左上角的 x 坐标。</param>
        /// <param name="yTop">指定矩形左上角的 y 坐标。</param>
        /// <param name="xRight">指定矩形右下角的 x 坐标。</param>
        /// <param name="yBottom">指定矩形右下角的 y 坐标。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool SetRect(out RECT lprc, int xLeft, int yTop, int xRight, int yBottom);
        /// <summary>
        /// SetRectEmpty 函数创建一个空的矩形：其中所有的坐标都设置为零。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setrectempty </para>
        /// </summary>
        /// <param name="lprc">指向包含矩形坐标的 RECT 结构的指针。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool SetRectEmpty(out RECT lprc);
        /// <summary>
        /// IsRectEmpty 函数确定指定的矩形是否为空。空矩形是没有面积的矩形。也就是说，右侧的坐标小于或等于左侧的坐标，或者底侧的坐标小于或等于顶侧的坐标。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-isrectempty </para>
        /// </summary>
        /// <param name="lprc">指向包含矩形逻辑坐标的 <see cref="RECT"/> 结构的指针。</param>
        /// <returns>如果矩形为空，则返回值为非零。如果矩形不为空，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsRectEmpty(ref RECT lprc);
        /// <summary>
        /// EqualRect 函数确定两个指定的矩形是否通过比较它们的左上角和右下角的坐标相等。
        /// <para>该 EqualRect 功能没有把空矩形作为平等的，如果它们的坐标是不同的。</para>
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-equalrect </para>
        /// </summary>
        /// <param name="lprc1">指向 RECT 结构的指针，该结构包含第一个矩形的逻辑坐标。</param>
        /// <param name="lprc2">指向 RECT 结构的指针，该结构包含第二个矩形的逻辑坐标。</param>
        /// <returns>如果两个矩形相同，则返回值为非零。如果两个矩形不相同，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool EqualRect(ref RECT lprc1, ref RECT lprc2);
        /// <summary>
        /// 确定是否指定的矩形内的指定点所在。如果一个点位于矩形的左侧或顶部，或者位于所有四个侧面，则该点位于矩形内。右侧或底部的一个点被认为是在矩形的外部。
        /// <para>必须在调用 PtInRect 之前将矩形标准化。也就是说，lprc.right 必须大于 lprc.left，而 lprc.bottom 必须大于 lprc.top。如果矩形未标准化，则永远不会在矩形内部考虑点。</para>
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-ptinrect </para>
        /// </summary>
        /// <param name="lprc">指向包含指定矩形的 <see cref="RECT"/> 结构的指针。</param>
        /// <param name="pt">一个 <see cref="POINT"/> 结构，包含指定点。</param>
        /// <returns>如果指定点位于矩形内，则返回值为非零。如果指定的点不在矩形内，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool PtInRect(ref RECT lprc, POINT pt);
        /// <summary>
        /// InflateRect 函数增加或减少指定的矩形的宽度和高度。所述 InflateRect 函数添加 DX 单位矩形的和左和右端 DY 单元的顶部和底部。在 DX 和 DY 参数符号值; 正值增加宽度和高度，负值减小宽度和高度。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-inflaterect </para>
        /// </summary>
        /// <param name="lprc">[LPRECT] 指向大小增加或减小的RECT结构的指针。</param>
        /// <param name="dx">增大或减小矩形宽度的量。此参数必须为负数以减小宽度。</param>
        /// <param name="dy">增加或减少矩形高度的数量。此参数必须为负数以减小高度。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool InflateRect(ref RECT lprc, int dx, int dy);
        /// <summary>
        /// OffsetRect 功能由指定的偏移量移动指定的矩形。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-offsetrect </para>
        /// </summary>
        /// <param name="lprc">指向RECT结构的指针，该结构包含要移动的矩形的逻辑坐标。</param>
        /// <param name="dx">指定向左或向右移动矩形的量。此参数必须为负值，以将矩形向左移动。</param>
        /// <param name="dy">指定向上或向下移动矩形的量。此参数必须为负值才能向上移动矩形。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool OffsetRect(ref RECT lprc, int dx, int dy);
        /// <summary>
        /// IntersectRect 该函数计算两个源矩形的交集与交点矩形的坐标放入目标矩形。如果源矩形不相交，则将一个空矩形（所有坐标均设置为零）放置到目标矩形中。
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-intersectrect </para>
        /// </summary>
        /// <param name="lprcDst">指向 RECT 结构的指针，该结构将接收 lprcSrc1 和 lprcSrc2 参数指向的矩形的交集。此参数不能为 NULL。</param>
        /// <param name="lprc1">指向包含第一个源矩形的 RECT 结构的指针。</param>
        /// <param name="lprc2">指向包含第二个源矩形的 RECT 结构的指针。</param>
        /// <returns>如果矩形相交，则返回值为非零。如果矩形不相交，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IntersectRect(out RECT lprcDst, ref RECT lprc1, ref RECT lprc2);
        /// <summary>
        /// SubtractRect 函数确定一个矩形的由从另一个中减去一个矩形形成的坐标。
        /// <para>该函数仅减去由指定的矩形 lprcSrc2 从由指定的矩形 lprcSrc1 当矩形无论是在X或Y方向上完全相交。
        /// 例如，如果 lprcSrc1 具有坐标（10,10,100,100），而 lprcSrc2 具有坐标（50,50,150,150），则该函数会将 lprcDst 指向的矩形的坐标设置为（10,10,100,100）。
        /// 如果 lprcSrc1 具有坐标（10,10,100,100）并且 lprcSrc2 具有坐标（50,10,150,150），但是，该函数设置 lprcDst 指向的矩形的坐标到（10,10,50,100）。换句话说，所得的矩形是几何差异的边界框。</para>
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-subtractrect </para>
        /// </summary>
        /// <param name="lprcDst">指向一个 RECT 接收矩形的坐标结构减去由指向的矩形确定 lprcSrc2 从矩形指向 lprcSrc1。</param>
        /// <param name="lprc1">指向 RECT 结构的指针，该函数从中减去 lprcSrc2 指向的矩形。</param>
        /// <param name="lprc2">该函数从 lprcSrc1 指向的矩形中减去的 RECT 结构的指针。</param>
        /// <returns>如果结果矩形为空，则返回值为零。如果结果矩形不为空，则返回值为非零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool SubtractRect(out RECT lprcDst, ref RECT lprc1, ref RECT lprc2);
        /// <summary>
        /// UnionRect 函数创建两个矩形的联合。联合是包含两个源矩形的最小矩形。
        /// <para>系统会忽略空矩形的尺寸，即所有坐标均设置为零的矩形，因此它没有高度或宽度。</para>
        /// <para>因为应用程序可以将矩形用于不同的目的，所以矩形函数不使用显式的度量单位。相反，所有矩形坐标和尺寸均以带符号的逻辑值给出。映射模式和使用矩形的功能确定度量单位。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unionrect </para>
        /// </summary>
        /// <param name="lprcDst">指向RECT结构的指针，该结构将接收一个包含 lprcSrc1 和 lprcSrc2 参数指向的矩形的矩形。</param>
        /// <param name="lprc1">指向包含第一个源矩形的 RECT 结构的指针。</param>
        /// <param name="lprc2">指向包含第二个源矩形的 RECT 结构的指针。</param>
        /// <returns>如果指定的结构包含非空矩形，则返回值为非零。如果指定的结构不包含非空矩形，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool UnionRect(out RECT lprcDst, ref RECT lprc1, ref RECT lprc2);
        #endregion


        #region Window Activity
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
        /// 如果窗口附加到调用线程的消息队列，则检索具有键盘焦点的窗口的句柄。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getfocus </para>
        /// </summary>
        /// <returns>返回值是具有键盘焦点的窗口的句柄。如果调用线程的消息队列没有与键盘焦点相关联的窗口，则返回值为NULL。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetFocus();
        /// <summary>
        /// 检索已捕获鼠标的窗口的句柄（如果有）。一次只能捕获一个窗口。无论光标是否在其边界内，此窗口都会接收鼠标输入。
        /// <para>一个 NULL 的返回值意味着当前线程未捕获鼠标。但是，很可能另一个线程或进程捕获了鼠标。要获取另一个线程上的捕获窗口的句柄，请使用 GetGUIThreadInfo 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcapture </para>
        /// </summary>
        /// <returns>返回值是与当前线程关联的捕获窗口的句柄。如果线程中没有窗口捕获到鼠标，则返回值为 NULL。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetCapture();
        #endregion


        #region Window Find OR Enum
        /// <summary>
        /// 通过将句柄传递给每个窗口，依次传递到应用程序定义的回调函数，可以枚举屏幕上所有的顶级窗口。EnumWindows 继续，直到枚举最后一个顶级窗口或回调函数返回 FALSE 为止。
        /// <para>该 EnumWindows 的功能不枚举子窗口，与由拥有该系统拥有一些顶层窗口除外 WS_CHILD 风格。</para>
        /// <para>该函数比循环调用 GetWindow 函数更可靠。调用 GetWindow 来执行此任务的应用程序可能会陷入无限循环或引用已被破坏的窗口的句柄。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-enumwindows </para>
        /// </summary>
        /// <param name="lpEnumFunc">指向应用程序定义的回调函数的指针。有关更多信息，请参见 <see cref="EnumWindowsProc"/>。</param>
        /// <param name="lParam">应用程序定义的值，将传递给回调函数。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。
        /// <para>如果 EnumWindowsProc 返回零，则返回值也为零。在这种情况下，回调函数应调用 SetLastError 以获得有意义的错误代码，以将其返回给 EnumWindows 的调用者。</para></returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        /// <summary>
        /// 通过将句柄传递给每个子窗口并依次传递给应用程序定义的回调函数，可以枚举属于指定父窗口的子窗口。EnumChildWindows 继续，直到枚举最后一个子窗口或回调函数返回 FALSE 为止。
        /// <para>如果子窗口创建了自己的子窗口，则EnumChildWindows也会枚举这些窗口。</para>
        /// <para>将正确枚举在枚举过程中以Z顺序移动或重新定位的子窗口。该函数不会枚举在枚举之前销毁的子窗口或在枚举过程中创建的子窗口。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-enumchildwindows </para>
        /// </summary>
        /// <param name="hWndParent">父窗口的句柄，其子窗口将被枚举。如果此参数为 NULL，则此函数等效于 EnumWindows。</param>
        /// <param name="lpEnumFunc">指向应用程序定义的回调函数的指针。有关更多信息，请参见 <see cref="EnumChildProc"/>。</param>
        /// <param name="lParam">应用程序定义的值，将传递给回调函数。</param>
        /// <returns>不使用返回值。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// 检索顶级窗口的句柄，该窗口的类名和窗口名与指定的字符串匹配。此功能不搜索子窗口。此功能不执行区分大小写的搜索。
        /// <para>要从指定的子窗口开始搜索子窗口，请使用 FindWindowEx 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-findwindowa </para>
        /// </summary>
        /// <param name="lpClassName">[LPCSTR]
        /// 如果 lpClassName 指向一个字符串，则它指定窗口类名称。类名可以是在 RegisterClass 或 RegisterClassEx 中注册的任何名称，也可以是任何预定义的控件类名称。
        /// <para>如果 lpClassName 为NULL，它将找到标题与 lpWindowName 参数匹配的任何窗口</para>
        /// </param>
        /// <param name="lpWindowName">[LPCSTR]窗口名称（窗口标题）。如果此参数为 NULL，则所有窗口名称均匹配。</param>
        /// <returns>如果函数成功，返回值为具有指定类名和窗口名的窗口句柄；如果函数失败，返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "FindWindow", SetLastError = true)]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// [建议不使用该函数] 检索顶级窗口的句柄，该窗口的类名和窗口名与指定的字符串匹配。此功能不搜索子窗口。此功能不执行区分大小写的搜索。
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
        /// <returns>如果函数成功，返回值为具有指定类名和窗口名的窗口句柄；如果函数失败，返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "FindWindow", SetLastError = true)]
        public extern static IntPtr FindWindow(byte[] lpClassName, string lpWindowName);
        /// <summary>
        /// 检索其类名和窗口名与指定的字符串匹配的窗口的句柄。该功能搜索子窗口，从指定子窗口之后的子窗口开始。此功能不执行区分大小写的搜索。
        /// <para>如果 lpszWindow 参数不为 NULL，则 FindWindowEx 调用 GetWindowText 函数以检索窗口名称以进行比较。有关可能出现的潜在问题的描述，请参见 GetWindowText 。</para>
        /// <para>应用程序可以通过以下方式调用此函数：FindWindowEx(NULL, NULL, MAKEINTATOM(0x8000), NULL ); 请注意，0x8000是菜单类的原子。当应用程序调用此函数时，该函数检查是否正在显示该应用程序创建的上下文菜单。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-findwindowexa </para>
        /// </summary>
        /// <param name="hWndParent">父窗口要搜索其子窗口的句柄。
        /// <para>如果 hwndParent 为 NULL，则该函数使用桌面窗口作为父窗口。该功能在作为桌面子窗口的窗口之间搜索。如果 hwndParent 为 HWND_MESSAGE，则该函数搜索所有仅消息窗口。</para></param>
        /// <param name="hWndChildAfter">子窗口的句柄。搜索从Z顺序的下一个子窗口开始。子窗口必须是hwndParent的直接子窗口，而不仅仅是后代窗口。
        /// <para>如果 hwndChildAfter 为 NULL，则搜索从 hwndParent 的第一个子窗口开始。请注意，如果 hwndParent 和 hwndChildAfter 均为 NULL，则该函数将搜索所有顶级窗口和仅消息窗口。</para></param>
        /// <param name="lpszClass">由先前调用RegisterClass或RegisterClassEx函数创建的类名称或类原子。原子必须放在lpszClass的低位字中；高阶字必须为零。
        ///     <para>如果lpszClass是一个字符串，则它指定窗口类名称。类名可以是在RegisterClass或RegisterClassEx中注册的任何名称，也可以是任何预定义的控件类名称，也可以是MAKEINTATOM(0x8000)。在后一种情况下，0x8000是菜单类的原子。</para></param>
        /// <param name="lpszWindow">窗口名称（窗口标题）。如果此参数为NULL，则所有窗口名称均匹配。</param>
        /// <returns>如果函数成功，则返回值是具有指定类和窗口名称的窗口的句柄。如果函数失败，则返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "FindWindowEx", SetLastError = true)]
        public extern static IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
        /// <summary>
        /// [建议不使用该函数] 检索其类名和窗口名与指定的字符串匹配的窗口的句柄。该功能搜索子窗口，从指定子窗口之后的子窗口开始。此功能不执行区分大小写的搜索。
        /// <para>如果 lpszWindow 参数不为 NULL，则 FindWindowEx 调用 GetWindowText 函数以检索窗口名称以进行比较。有关可能出现的潜在问题的描述，请参见 GetWindowText 。</para>
        /// <para>应用程序可以通过以下方式调用此函数：FindWindowEx(NULL, NULL, MAKEINTATOM(0x8000), NULL ); 请注意，0x8000是菜单类的原子。当应用程序调用此函数时，该函数检查是否正在显示该应用程序创建的上下文菜单。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-findwindowexa </para>
        /// </summary>
        /// <param name="hWndParent">父窗口要搜索其子窗口的句柄。
        /// <para>如果 hwndParent 为 NULL，则该函数使用桌面窗口作为父窗口。该功能在作为桌面子窗口的窗口之间搜索。如果 hwndParent 为 HWND_MESSAGE，则该函数搜索所有仅消息窗口。</para></param>
        /// <param name="hWndChildAfter">子窗口的句柄。搜索从Z顺序的下一个子窗口开始。子窗口必须是hwndParent的直接子窗口，而不仅仅是后代窗口。
        /// <para>如果 hwndChildAfter 为 NULL，则搜索从 hwndParent 的第一个子窗口开始。请注意，如果 hwndParent 和 hwndChildAfter 均为 NULL，则该函数将搜索所有顶级窗口和仅消息窗口。</para></param>
        /// <param name="lpszClass">由先前调用RegisterClass或RegisterClassEx函数创建的类名称或类原子。原子必须放在lpszClass的低位字中；高阶字必须为零。
        ///     <para>如果lpszClass是一个字符串，则它指定窗口类名称。类名可以是在RegisterClass或RegisterClassEx中注册的任何名称，也可以是任何预定义的控件类名称，也可以是MAKEINTATOM(0x8000)。在后一种情况下，0x8000是菜单类的原子。</para></param>
        /// <param name="lpszWindow">窗口名称（窗口标题）。如果此参数为NULL，则所有窗口名称均匹配。</param>
        /// <returns>如果函数成功，则返回值是具有指定类和窗口名称的窗口的句柄。如果函数失败，则返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "FindWindowEx", SetLastError = true)]
        public extern static IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, byte[] lpszClass, string lpszWindow);
        #endregion


        #region Window Name Or Class Name
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
        [DllImport(DLL_NAME, EntryPoint = "GetClassName", SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        /// <summary>
        /// 检索指定窗口的标题栏文本的长度（以字符为单位）（如果窗口具有标题栏）。如果指定的窗口是控件，则该函数将检索控件内文本的长度。但是 GetWindowTextLength 无法在另一个应用程序中检索编辑控件的文本长度。
        /// <para>如果目标窗口由当前进程拥有，则 GetWindowTextLength 导致将 WM_GETTEXTLENGTH 消息发送到指定的窗口或控件。</para>
        /// <para>要获取文本的确切长度，请使用 WM_GETTEXT，LB_GETTEXT 或 CB_GETLBTEXT 消息或 GetWindowText 函数。</para>
        /// <para>在某些情况下，GetWindowTextLength 函数可能返回的值大于文本的实际长度。这是由于 ANSI 和 Unicode 的某些混合而发生的，并且是由于系统允许文本中可能存在双字节字符集（DBCS）字符。
        /// 但是返回值将始终至少与文本的实际长度一样大。因此，您始终可以使用它来指导缓冲区分配。当应用程序同时使用 ANSI 函数和使用 Unicode 的通用对话框时，就可能出现此现象。当应用程序的窗口过程为 Unicode 的窗口使用 ANSI 版本的 GetWindowTextLength 或 GetWindowTextLength 的 Unicode 版本时，也会发生这种情况。窗口过程为ANSI的窗口。有关 ANSI 和 ANSI 函数的更多信息，请参见函数原型约定。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowtextlengtha </para>
        /// </summary>
        /// <param name="hWnd">窗口或控件的句柄。</param>
        /// <returns>如果函数成功，则返回值是文本的长度（以字符为单位）。在某些情况下，该值实际上可能大于文本的长度。
        /// <para>如果窗口没有文本，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</para>
        /// </returns>
        [DllImport(DLL_NAME, EntryPoint = "GetWindowTextLength", SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
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
        [DllImport(DLL_NAME, EntryPoint = "GetWindowText", SetLastError = true)]
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
        [DllImport(DLL_NAME, EntryPoint = "SetWindowText", SetLastError = true)]
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
        [DllImport(DLL_NAME, EntryPoint = "SetWindowText", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, StringBuilder lpString);
        #endregion


        #region Window Operation Return Boolean Value
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
        /// 确定是否为鼠标和键盘输入启用了指定的窗口。
        /// <para>子窗口仅在启用且可见的情况下才接收输入。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iswindowenabled </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果启用了窗口，则返回值为非零。如果未启用该窗口，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);
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
        /// 确定指定的窗口句柄是否标识现有窗口。
        /// <para>线程不应对未创建的窗口使用 IsWindow，因为调用此函数后该窗口可能会被破坏。此外，由于窗口句柄被回收，因此该句柄甚至可以指向其他窗口。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iswindow </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果窗口句柄标识现有窗口，则返回值为非零。如果窗口句柄无法识别现有窗口，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsWindow(IntPtr hWnd);
        /// <summary>
        /// 确定指定的窗口是否为本地Unicode窗口。
        /// <para>如果窗口类是使用 ANSI 版本的 RegisterClass（RegisterClassA）注册的，则窗口的字符集是 ANSI。如果窗口类是使用 Unicode 版本的 RegisterClass（RegisterClassW）注册的，则窗口的字符集为 Unicode。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iswindowunicode </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果该窗口是本机Unicode窗口，则返回值为非零。如果该窗口不是本机Unicode窗口，则返回值为零。该窗口是本机ANSI窗口。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsWindowUnicode(IntPtr hWnd);
        /// <summary>
        /// 确定窗口是指定父窗口的子窗口还是子窗口。子窗口是指定父窗口的直接后代（如果该父窗口在父窗口的链中）；父窗口链从原始重叠窗口或弹出窗口通向子窗口。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-ischild </para>
        /// </summary>
        /// <param name="hWndParent">父窗口的句柄。</param>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果该窗口是指定父窗口的子窗口或子窗口，则返回值为非零。如果该窗口不是指定父窗口的子窗口或子窗口，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);
        /// <summary>
        /// 确定窗口是否最大化。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-iszoomed </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果缩放窗口，则返回值为非零。如果窗口未缩放，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsZoomed(IntPtr hWnd);
        /// <summary>
        /// 确定指定的窗口是否最小化（图标）。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-isiconic </para>
        /// </summary>
        /// <param name="hWnd">要测试的窗口的句柄。</param>
        /// <returns>如果窗口是标志性的，则返回值为非零。如果窗口不是标志性的，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool IsIconic(IntPtr hWnd);
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
        /// 创建一个重叠窗口，弹出窗口或子窗口。它指定窗口类，窗口标题，窗口样式，以及（可选）窗口的初始位置和大小。该函数还指定窗口的父级或所有者（如果有）以及窗口的菜单。
        /// <para>除了 CreateWindow 支持的样式之外，要使用扩展的窗口样式，请使用 CreateWindowEx 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-createwindowexa </para>
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <param name="dwStyle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hwndParent"></param>
        /// <param name="hMenu"></param>
        /// <param name="hInstance"></param>
        /// <param name="lpParam">[LPVOID]</param>
        [DllImport(DLL_NAME, EntryPoint = "CreateWindow", SetLastError = true)]
        public static extern void CreateWindow(string lpClassName, string lpWindowName, int dwStyle, int x, int y, int width, int height,
                                              IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, [MarshalAs(UnmanagedType.AsAny)] object lpParam);
        /// <summary>
        /// 创建具有扩展窗口样式的重叠窗口，弹出窗口或子窗口；否则，此函数与 CreateWindow 函数相同。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-createwindowexa </para>
        /// </summary>
        /// <param name="dwExStyle">正在创建的窗口的扩展窗口样式。有关可能值的列表，请参见 扩展窗口样式。</param>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <param name="dwStyle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hwndParent"></param>
        /// <param name="hMenu"></param>
        /// <param name="hInstance"></param>
        /// <param name="lpParam"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME, EntryPoint = "CreateWindowEx", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int width, int height,
                                              IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, [MarshalAs(UnmanagedType.AsAny)] object lpParam);
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
        #endregion


        #region Window Flash/Animation
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
        /// <param name="pfwi">[PFLASHWINFO] 指向 <see cref="FLASHINFO"/> 结构的指针</param>
        /// <returns>返回值指定在调用 FlashWindowEx 函数之前窗口的状态 。如果在调用之前将窗口标题绘制为活动窗口，则返回值为非零。否则，返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool FlashWindowEx(ref FLASHINFO pfwi);
        /// <summary>
        /// 在显示或隐藏窗口时使您产生特殊效果。动画有四种类型：滚动，滑动，折叠或展开以及alpha混合淡入。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-animatewindow </para>
        /// </summary>
        /// <param name="hWnd">窗口动画的句柄。调用线程必须拥有此窗口。</param>
        /// <param name="dwTime">播放动画所需的时间（以毫秒为单位）。通常，动画播放需要200毫秒。</param>
        /// <param name="dwFlags">动画的类型 <see cref="AwFlag"/></param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。获取扩展的错误信息，请调用GetLastError函数。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool AnimateWindow(IntPtr hWnd, int dwTime, AwFlag dwFlags);
        #endregion

        
        /// <summary>
        /// 触发视觉信号以指示正在播放声音。
        /// <para>通过使用 SPI_SETSOUNDSENTRY 值调用 SystemParametersInfo 来设置通知行为。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-soundsentry </para>
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME)]
        public static extern bool SoundSentry();


        /// <summary>
        /// 表示无法关闭系统，并设置启动系统关闭后要显示给用户的原因字符串。
        /// <para>只能从创建由 hWnd 参数指定的窗口的线程中调用此函数。否则，函数将失败，最后的错误代码是 ERROR_ACCESS_DENIED。</para>
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



        #region Window DPI
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
        #endregion


        #region Window Mouse/Cursor State
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
        /// <param name="pci">[PCURSORINFO] 指向接收信息的 <see cref="CURSORINFO"/> 结构的指针。请注意，在调用此函数之前，必须将 cbSize 成员设置为 sizeof(CURSORINFO)。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);
        /// <summary>
        /// 检索鼠标光标在屏幕坐标中的位置。
        /// <para>光标位置始终在屏幕坐标中指定，并且不受包含光标的窗口的映射模式的影响。调用过程必须对窗口站具有 WINSTA_READATTRIBUTES 访问权限。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getcursorpos </para>
        /// </summary>
        /// <param name="lpPoint">[LPPOINT]指向 <see cref="POINT"/> 结构的指针，该结构接收光标的屏幕坐标。</param>
        /// <returns>如果成功返回非零，否则返回零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool GetCursorPos(ref POINT lpPoint);
        #endregion


        #region Window Keyboard State
        /// <summary>
        /// 将 256 个虚拟键的状态复制到指定的缓冲区。
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
        public static extern bool GetKeyboardState(byte[] lpKeyState);
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
        /// 将指定的虚拟键代码和键盘状态转换为相应的一个或多个字符。该功能使用输入语言和键盘布局手柄识别的物理键盘布局来翻译代码。
        /// <para>提供给 ToAscii 函数的参数可能不足以转换虚拟键代码，因为先前的死键存储在键盘布局中。</para>
        /// <para>通常，ToAscii 基于虚拟键代码执行转换。但是，在某些情况下，uScanCode 参数的第 15 位 可用于区分按键和释放按键。扫描代码用于翻译 ALT + 数字键组合。</para>
        /// <para>尽管 NUM LOCK 是会影响键盘行为的切换键，但是 ToAscii 会忽略 lpKeyState（VK_NUMLOCK）的切换设置（低位）， 因为 仅 uVirtKey 参数足以将光标移动键（VK_HOME，VK_INSERT等）与数字键（VK_DECIMAL，VK_NUMPAD0 - VK_NUMPAD9）。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-toascii </para>
        /// </summary>
        /// <param name="uVirtKey">要转换的虚拟密钥代码 <see cref="VirtualKeyCode"/></param>
        /// <param name="uScanCode">要转换的密钥的硬件扫描代码。如果按下键（未按下），则此值的高位被设置。</param>
        /// <param name="lpbKeyState">[const BYTE *] 指向包含当前键盘状态的 256 字节数组的指针。数组中的每个元素（字节）都包含一个键的状态。如果设置了字节的高位，则按键被按下（按下）。低位（如果已设置）指示按键已打开。在此功能中，仅 CAPS LOCK 键的切换位相关。NUM LOCK 和 SCROLL LOCK 键的切换状态将被忽略。</param>
        /// <param name="lpChar">接收翻译后的一个或多个字符的缓冲区。</param>
        /// <param name="uFlags">如果菜单处于活动状态，则此参数必须为1，否则为0。</param>
        /// <returns>如果指定的键是死键，则返回值为负。否则，它是以下值之一。
        /// <para>0.指定的虚拟键没有针对键盘当前状态的转换。</para>
        /// <para>1.一个字符被复制到缓冲区。</para>
        /// <para>2.两个字符被复制到缓冲区。当无法将存储在键盘布局中的死键字符（重音符或变音符）与指定的虚拟键组成单个字符时，通常会发生这种情况。</para>
        /// </returns>
        [DllImport(DLL_NAME)]
        public static extern int ToAscii(VirtualKeyCode uVirtKey, uint uScanCode, byte[] lpbKeyState, byte[] lpChar, uint uFlags);
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
        #endregion


        #region Window Mouse/Keyboard/Hardware Input
        /// <summary>
        /// 确定在调用线程的消息队列中是否有鼠标按钮或键盘消息。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getinputstate </para>
        /// </summary>
        /// <returns>如果队列包含一个或多个新的鼠标按钮或键盘消息，则返回值为非零。如果队列中没有新的鼠标按钮或键盘消息，则返回值为零。</returns>
        [DllImport(DLL_NAME)]
        public static extern bool GetInputState();

        /// <summary>
        /// 合成击键，鼠标动作和按钮单击。已替代 keyboard_event()
        /// <para>第三方库：https://github.com/michaelnoonan/inputsimulator </para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-sendinput </para>
        /// </summary>
        /// <param name="cInputs">pInputs 数组中的结构数，应为 pInputs 数组长度。</param>
        /// <param name="pInputs">[LPINPUT] <seealso cref="INPUT"/> 结构的数组。每个结构代表一个要插入键盘或鼠标输入流的事件。</param>
        /// <param name="cbSize">INPUT 结构的大小（以字节为单位）。如果 cbSize 不是 INPUT 结构的大小，则该函数失败。 大小应为 Marshal.SizeOf(typeof(INPUT))</param>
        /// <returns>该函数返回成功插入键盘或鼠标输入流中的事件数。如果函数返回零，则输入已经被另一个线程阻塞。要获取扩展的错误信息，请调用GetLastError。
        /// <para>当UIPI阻止此功能时，该功能将失败。请注意，GetLastError和返回值都不会指示失败是由UIPI阻塞引起的。</para>
        /// </returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint SendInput(uint cInputs, INPUT[] pInputs, int cbSize);
        #endregion


        #region Windwo Hot Key
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
        #endregion


        #region Window HookEx
        /// <summary>
        /// 将应用程序定义的挂钩过程安装到挂钩链中。您将安装一个挂钩过程来监视系统中的某些类型的事件。这些事件与特定线程或与调用线程在同一桌面上的所有线程相关联。
        /// <para>示例：当前APP:SetWindowsHookEx(idHook, HookProc, IntPtr.Zero, Kernel32.GetCurrentThreadId()); 全局:SetWindowsHookEx(idHook, HookProc, Process.GetCurrentProcess().MainModule.BaseAddress, 0);</para>
        /// <para>在终止之前，应用程序必须调用 UnhookWindowsHookEx 函数以释放与该挂钩关联的系统资源。</para>
        /// <para>调用 CallNextHookEx 函数链接到下一个挂钩过程是可选的，但强烈建议这样做；否则，其他已安装钩子的应用程序将不会收到钩子通知，因此可能会出现不正确的行为。除非绝对需要防止其他应用程序看到该通知，否则应调用 CallNextHookEx。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa </para>
        /// <para>示例：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/using-hooks </para>
        /// <para>更多钩子函数：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-functions </para>
        /// </summary>
        /// <param name="idHook">要安装的挂钩过程的类型 <see cref="HookType"/></param>
        /// <param name="lpfn">指向钩子过程的指针。如果 dwThreadId 参数为零或指定由其他进程创建的线程的标识符，则 lpfn 参数必须指向 DLL 中的挂钩过程。否则，lpfn 可以指向与当前进程关联的代码中的挂钩过程。</param>
        /// <param name="hInstance">DLL 的句柄，其中包含由 lpfn 参数指向的挂钩过程。所述 HMOD 参数必须设置为 NULL，如果 dwThreadId 参数指定由当前进程，并且如果钩子程序是与当前过程相关联的所述代码中创建的线程。</param>
        /// <param name="threadId">挂钩过程将与之关联的线程的标识符。对于桌面应用程序，如果此参数为零，则挂钩过程与与调用线程在同一桌面上运行的所有现有线程相关联。</param>
        /// <returns>如果函数成功，则返回值是挂钩过程的句柄。如果函数失败，则返回值为NULL。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        /// <summary>
        /// 删除通过 <see cref="SetWindowsHookEx"/> 函数安装在挂钩链中的挂钩过程。
        /// <para>即使在 UnhookWindowsHookEx 返回之后，该挂钩过程也可以处于被另一个线程调用的状态。如果没有同时调用该挂钩过程，则在 UnhookWindowsHookEx 返回之前，立即删除该挂钩过程。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-unhookwindowshookex </para>
        /// <para>更多钩子函数：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-functions </para>
        /// </summary>
        /// <param name="hhk">钩子的手柄将被卸下。此参数是通过先前调用 <see cref="SetWindowsHookEx"/> 获得的挂钩句柄。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        /// <summary>
        /// 挂钩信息传递到当前挂钩链中的下一个挂钩过程。挂钩过程可以在处理挂钩信息之前或之后调用此函数。
        /// <para>对于特定的挂钩类型，挂钩程序是成链安装的。CallNextHookEx 调用链中的下一个钩子。</para>
        /// <para>调用 CallNextHookEx 是可选的，但强烈建议您使用；否则，其他已安装钩子的应用程序将不会收到钩子通知，因此可能会出现不正确的行为。除非绝对需要防止其他应用程序看到该通知，否则应调用 CallNextHookEx。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-callnexthookex </para>
        /// <para>更多钩子函数：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-functions </para>
        /// </summary>
        /// <param name="hhk">该参数被忽略。</param>
        /// <param name="nCode">挂钩代码传递给当前的挂钩过程。下一个挂钩过程将使用此代码来确定如何处理挂钩信息。</param>
        /// <param name="wParam">所述的 wParam 值传递到当前挂钩过程。此参数的含义取决于与当前挂钩链关联的挂钩的类型。</param>
        /// <param name="lParam">传递给当前挂钩过程的 lParam 值。此参数的含义取决于与当前挂钩链关联的挂钩的类型。</param>
        /// <returns>该值由链中的下一个挂钩过程返回。当前的挂钩过程还必须返回该值。返回值的含义取决于挂钩类型。有关更多信息，请参见各个挂钩过程的描述。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion


        #region Window Message
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
        public static extern int SendMessage(IntPtr hWnd, MessageType Msg, IntPtr wParam, IntPtr lParam);
        [DllImport(DLL_NAME, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport(DLL_NAME, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hwnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);
        [DllImport(DLL_NAME, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wParam, String lParam);

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
        [DllImport(DLL_NAME, EntryPoint = "PostMessage", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, MessageType Msg, IntPtr wParam, IntPtr lParam);
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
        [DllImport(DLL_NAME, EntryPoint = "PeekMessage")]
        public static extern bool PeekMessage(ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        /// <summary>
        /// 将消息信息传递到指定的窗口过程。
        /// <para>使用 CallWindowProc 函数进行窗口子类化。通常，具有相同类的所有窗口共享一个窗口过程。子类是具有相同类的一个窗口或一组窗口，其消息在传递给该类的窗口过程之前，已被另一个（或多个）过程拦截和处理。</para>
        /// <para>该 SetWindowLong 函数功能改变与特定窗口相关的窗口过程，导致系统调用新的窗口过程而不是以前一个创建子类。应用程序必须通过调用 CallWindowProc 将新窗口过程未处理的任何消息传递给前一个窗口过程。这允许应用程序创建一系列窗口过程。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-callwindowproca </para>
        /// </summary>
        /// <param name="lpPrevWndFunc">上一个窗口过程。如果通过在 nIndex 参数设置为 GWL_WNDPROC 或 DWL_DLGPROC 的情况下调用 GetWindowLong 函数获得此值，则它实际上是窗口或对话框过程的地址，或者是仅对 CallWindowProc 有意义的特殊内部值。</param>
        /// <param name="hWnd">接收消息的窗口过程的句柄。</param>
        /// <param name="Msg">[UINT]消息类型</param>
        /// <param name="wParam">其他特定于消息的信息。此参数的内容取决于Msg参数的值。</param>
        /// <param name="lParam">其他特定于消息的信息。此参数的内容取决于Msg参数的值。</param>
        /// <returns>返回值指定消息处理的结果，并取决于发送的消息。</returns>
        [DllImport(DLL_NAME, EntryPoint = "CallWindowProc")]
        public static extern IntPtr CallWindowProc(WindowProc lpPrevWndFunc, IntPtr hWnd, MessageType Msg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 调用默认窗口过程以为应用程序未处理的任何窗口消息提供默认处理。此功能确保处理所有消息。DefWindowProc用窗口过程接收到的相同参数调用。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-defwindowproca </para>
        /// </summary>
        /// <param name="hWnd">接收到消息的窗口过程的句柄。</param>
        /// <param name="Msg">[UINT] 消息类型</param>
        /// <param name="wParam">附加消息信息。此参数的内容取决于Msg参数的值。</param>
        /// <param name="lParam">附加消息信息。此参数的内容取决于Msg参数的值。</param>
        /// <returns>返回值是消息处理的结果，并取决于消息。</returns>
        [DllImport(DLL_NAME, EntryPoint = "DefWindowProc")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, MessageType Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 检索当前线程的额外消息（附加对象）信息。额外的消息信息是与当前线程的消息队列关联的应用程序或驱动程序定义的值。
        /// <para>若要设置线程的额外消息信息，请使用 SetMessageExtraInfo 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getmessageextrainfo </para>
        /// </summary>
        /// <returns>[LPARAM] 返回值指定额外的信息。额外信息的含义是特定于设备的。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr GetMessageExtraInfo();
        /// <summary>
        /// 设置当前线程的额外消息信息。额外的消息信息是与当前线程的消息队列关联的应用程序或驱动程序定义的值。应用程序可以使用 GetMessageExtraInfo 函数来检索线程的额外消息信息。
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setmessageextrainfo </para>
        /// </summary>
        /// <param name="lParam">与当前线程关联的值。</param>
        /// <returns>返回值是与当前线程关联的先前值。</returns>
        [DllImport(DLL_NAME)]
        public static extern IntPtr SetMessageExtraInfo(IntPtr lParam);
        #endregion


        #region Window Touch Input
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
        #endregion
    }
}
