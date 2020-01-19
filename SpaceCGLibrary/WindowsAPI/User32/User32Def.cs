#pragma warning disable CS1591,CS1572

using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SpaceCG.WindowsAPI.User32
{
    #region Delegate
    /// <summary>
    /// HOOKPROC 回调函数
    /// <para>与 SetWindowsHookEx 函数一起使用的应用程序定义或库定义的回调函数。调用 SendMessage 函数后，系统将调用此函数。钩子程序可以检查消息；它不能修改它。</para>
    /// <para>所述 HOOKPROC 类型定义一个指向这个回调函数。CallWndRetProc 是应用程序定义或库定义的函数名称的占位符。</para>
    /// <para>应用程序通过在调用 SetWindowsHookEx 函数时指定 WH_CALLWNDPROCRET 挂钩类型和指向该挂钩过程的指针来安装该挂钩过程。</para>
    /// <para>KeyboardProc 参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/keyboardproc </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nc-winuser-hookproc </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-functions </para>
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam">指定消息是否由当前进程发送。如果消息是由当前进程发送的，则该消息为非零；否则为0。否则为NULL。</param>
    /// <param name="lParam">指向 CWPRETSTRUCT 结构的指针，该结构包含有关消息的详细信息。</param>
    /// <returns>如果 nCode 小于零，则挂钩过程必须返回 CallNextHookEx 返回的值。
    /// <para>如果 nCode 大于或等于零，则强烈建议您调用 CallNextHookEx 并返回它返回的值。否则，其他安装了 WH_CALLWNDPROCRET 挂钩的应用程序将不会收到挂钩通知，因此可能会出现错误的行为。如果挂钩过程未调用 CallNextHookEx，则返回值应为零。</para>
    /// </returns>
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// 应用程序定义的功能，用于处理发送到窗口的消息。所述 WNDPROC 类型定义一个指向这个回调函数。WindowProc 是应用程序定义的函数名称的占位符。
    /// <para>参考 WPF <see cref="HwndSourceHook"/> </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/legacy/ms633573(v=vs.85) </para>
    /// </summary>
    /// <param name="hwnd">窗口的句柄。</param>
    /// <param name="uMsg">有关系统提供的消息的列表，请参阅系统定义的消息。</param>
    /// <param name="wParam">附加消息信息。此参数的内容取决于uMsg参数的值。</param>
    /// <param name="lParam">附加消息信息。此参数的内容取决于uMsg参数的值。</param>
    /// <returns>返回值是消息处理的结果，并取决于发送的消息。</returns>
    public delegate IntPtr WindowProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// 与 EnumWindows 或 EnumDesktopWindows 函数一起使用的应用程序定义的回调函数。它接收顶级窗口句柄。所述 WNDENUMPROC 类型定义一个指向这个回调函数。EnumWindowsProc 是应用程序定义的函数名称的占位符。
    /// <para>WNDENUMPROC WNDENUMPROC </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/legacy/ms633498(v=vs.85) </para>
    /// </summary>
    /// <param name="hwnd">顶级窗口的句柄。</param>
    /// <param name="lParam">在 EnumWindows 或 EnumDesktopWindows 中给出的应用程序定义的值。</param>
    /// <returns>要继续枚举，回调函数必须返回 TRUE；要停止枚举，它必须返回 FALSE。</returns>
    public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

    /// <summary>
    /// 与 EnumChildWindows 函数一起使用的应用程序定义的回调函数。它接收子窗口句柄。所述WNDENUMPROC类型定义一个指向这个回调函数。EnumChildProc 是应用程序定义的函数名称的占位符。
    /// <para>WNDENUMPROC</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/legacy/ms633493(v=vs.85) </para>
    /// </summary>
    /// <param name="hwnd">在 EnumChildWindows 中指定的父窗口的子窗口的句柄。</param>
    /// <param name="lParam"> 在EnumChildWindows 中给定的应用程序定义的值。</param>
    /// <returns>要继续枚举，回调函数必须返回 TRUE；要停止枚举，它必须返回 FALSE。</returns>
    public delegate bool EnumChildProc(IntPtr hwnd, IntPtr lParam);

    #endregion

    #region Hook Struct&Enum
    /// <summary>
    /// 包含有关低级键盘输入事件的信息。
    /// <para>HookType 类型为 WH_KEYBOARD_LL 的数据结构体，HookProc 代理函数参数 lParam 数据结构体</para>
    /// <para>KBDLLHOOKSTRUCT, *LPKBDLLHOOKSTRUCT, *PKBDLLHOOKSTRUCT</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-structures </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-functions </para>
    /// </summary>
    public struct KeyboardLLHookStruct
    {
        /// <summary>
        /// 表示一个在1到254间的虚似键盘码
        /// </summary>
        public VirtualKeyCode vkCode;
        /// <summary>
        /// key 表示硬件扫描码 
        /// </summary>
        public int scanCode;
        /// <summary>
        /// 扩展键标志，事件注入标志，上下文代码和过渡状态标志。该成员的指定如下。应用程序可以使用以下值来测试按键标志。测试LLKHF_INJECTED（位4）将告诉您是否已注入事件。如果是这样，那么测试LLKHF_LOWER_IL_INJECTED（位1）将告诉您是否从较低完整性级别运行的进程注入了事件。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-kbdllhookstruct?redirectedfrom=MSDN </para>
        /// </summary>
        public int flags;
        /// <summary>
        /// 此消息的时间戳，等于此消息返回的 GetMessageTime。
        /// </summary>
        public int time;
        /// <summary>
        /// 与消息关联的其他信息。
        /// </summary>
        public IntPtr dwExtraInfo;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"vkCode:{vkCode}, scanCode:{scanCode}, flags:{flags}, time:{time}";
        }
    }

    /// <summary>
    /// 包含有关传递给 WH_MOUSE 挂钩过程 MouseProc 的鼠标事件的信息。
    /// <para>MOUSEHOOKSTRUCT, * LPMOUSEHOOKSTRUCT, * PMOUSEHOOKSTRUCT</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-mousehookstruct?redirectedfrom=MSDN </para>
    /// </summary>
    public struct MouseHookStruct
    {
        /// <summary>
        /// 光标的x和y坐标，以屏幕坐标表示。
        /// </summary>
        public POINT pt;
        /// <summary>
        /// 窗口的句柄，它将接收与mouse事件相对应的鼠标消息。
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// 命中测试值。有关命中测试值的列表，请参见 WM_NCHITTEST 消息的描述。
        /// </summary>
        public uint wHitTestCode;
        /// <summary>
        /// 与消息关联的其他信息。
        /// </summary>
        public IntPtr dwExtraInfo;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"pt:{pt}, {hwnd}, {wHitTestCode}, {dwExtraInfo}";
        }
    }
    /// <summary>
    /// Structure used by WH_MOUSE_LL
    /// <para>MSLLHOOKSTRUCT, FAR *LPMSLLHOOKSTRUCT, *PMSLLHOOKSTRUCT;</para>
    /// </summary>
    public struct MouseLLHookStruct
    {
        public POINT pt;
        public int mouseData;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    /// <summary>
    /// 挂钩过程的类型
    /// <para> SetWindowsHookEx 函数数参考 idHook 的值之一 </para>
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa </para>
    /// <para>参考对应的结构数据：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/hook-structures </para>
    /// </summary>
    public enum HookType
    {
        /// <summary>
        /// [线程或全局] 安装挂钩过程，以监视由于对话框，消息框，菜单或滚动条中的输入事件而生成的消息。有关更多信息，请参见 MessageProc 挂钩过程。
        /// </summary>
        WH_MSGFILTER = -1,
        /// <summary>
        /// [仅全局] 安装一个挂接过程，该过程记录发布到系统消息队列中的输入消息。该挂钩对于记录宏很有用。有关更多信息，请参见 JournalRecordProc 挂钩过程。
        /// </summary>
        WH_JOURNALRECORD = 0,
        /// <summary>
        /// [仅全局] 安装该消息发布之前由一个记录一个钩子程序WH_JOURNALRECORD钩子程序。欲了解更多信息，请参阅 JournalPlaybackProc 钩子程序。
        /// </summary>
        WH_JOURNALPLAYBACK = 1,
        /// <summary>
        /// [线程或全局] 安装挂钩过程，以监视击键消息。有关更多信息，请参见 KeyboardProc 挂接过程。
        /// </summary>
        WH_KEYBOARD = 2,
        /// <summary>
        /// [线程或全局] 安装挂钩过程，以监视发布到消息队列的消息。有关更多信息，请参见 GetMsgProc 挂钩过程。
        /// </summary>
        WH_GETMESSAGE = 3,
        /// <summary>
        /// [线程或全局] 安装挂钩程序，该程序在系统将消息发送到目标窗口过程之前监视消息。有关更多信息，请参见 CallWndProc 挂接过程。
        /// </summary>
        WH_CALLWNDPROC = 4,
        /// <summary>
        /// [线程或全局] 安装一个挂钩程序，该程序接收对 CBT 应用程序有用的通知。有关更多信息，请参见 CBTProc 挂钩过程。
        /// </summary>
        WH_CBT = 5,
        /// <summary>
        /// [仅全局] 安装挂钩过程，以监视由于对话框，消息框，菜单或滚动条中的输入事件而生成的消息。挂钩过程会在与调用线程相同的桌面中监视所有应用程序的这些消息。有关更多信息，请参见 SysMsgProc 挂接过程。
        /// </summary>
        WH_SYSMSGFILTER = 6,
        /// <summary>
        /// [线程或全局] 安装监视鼠标消息的挂钩过程。有关更多信息，请参见 MouseProc 挂钩过程。
        /// </summary>
        WH_MOUSE = 7,
        /// <summary>
        /// #if defined(_WIN32_WINDOWS) hardware
        /// </summary>
        WH_HARDWARE = 8,
        /// <summary>
        /// [线程或全局] 安装对调试其他挂钩过程有用的挂钩过程。有关更多信息，请参见 DebugProc 挂钩过程。
        /// </summary>
        WH_DEBUG = 9,
        /// <summary>
        /// [线程或全局] 安装一个挂钩程序，该程序接收对外壳程序有用的通知。有关更多信息，请参见 ShellProc 挂钩过程。
        /// </summary>
        WH_SHELL = 10,
        /// <summary>
        /// [线程或全局] 安装一个挂钩程序，当应用程序的前台线程即将变为空闲时将调用该挂钩程序。该挂钩对于在空闲时间执行低优先级任务很有用。有关更多信息，请参见 ForegroundIdleProc 挂钩过程。
        /// </summary>
        WH_FOREGROUNDIDLE = 11,
        /// <summary>
        /// [线程或全局] 安装挂钩过程，以监视目标窗口过程处理完的消息。有关更多信息，请参见 CallWndRetProc 挂接过程。
        /// </summary>
        WH_CALLWNDPROCRET = 12,
        /// <summary>
        /// [仅全局] 安装钩子程序，以监视低级键盘输入事件。有关更多信息，请参见 LowLevelKeyboardProc 挂钩过程。
        /// </summary>
        WH_KEYBOARD_LL = 13,
        /// <summary>
        /// [仅全局] 安装钩子过程，以监视低级鼠标输入事件。有关更多信息，请参见 LowLevelMouseProc 挂钩过程。
        /// </summary>
        WH_MOUSE_LL = 14,
    }
    #endregion


    #region SetWindowPos 函数的参数值 hWndInsertAfter uFlags
    /// <summary>
    /// SetWindowPos 函数参数 hWndInsertAfter 的值之一
    /// </summary>
    public enum SwpState
    {
        /// <summary>
        /// 将窗口放置在所有非最上面的窗口上方（即，所有最上面的窗口的后面）。如果窗口已经是非最上面的窗口，则此标志无效。
        /// </summary>
        HWND_NOTOPMOST = -2,
        /// <summary>
        /// 将窗口置于所有非最上面的窗口上方；即使禁用窗口，窗口也将保持其最高位置。
        /// </summary>
        HWND_TOPMOST = -1,
        /// <summary>
        /// 将窗口置于Z顺序的顶部。
        /// </summary>
        HWND_TOP = 0,
        /// <summary>
        /// 将窗口置于Z顺序的底部。
        /// <para>如果hWnd参数标识了最顶部的窗口，则该窗口将失去其最顶部的状态，并放置在所有其他窗口的底部。</para>
        /// </summary>
        HWND_BOTTOM = 1,
    }

    /// <summary>
    /// SetWindowPos 函数参数 wFlags 的值之一或组合值
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos </para>
    /// </summary>
    [Flags]
    public enum SwpFlag : uint
    {
        /// <summary>
        /// 保留当前大小（忽略 cx 和 cy 参数）
        /// </summary>
        NOSIZE = 0x0001,
        /// <summary>
        /// 保留当前位置（忽略 X 和 Y 参数）。
        /// </summary>
        NOMOVE = 0x0002,
        /// <summary>
        /// 保留当前的Z顺序（忽略 hWndInsertAfter 参数）
        /// </summary>
        NOZORDER = 0x0004,
        /// <summary>
        /// 不重绘更改。
        /// <para>如果设置了此标志，则不会发生任何重绘。这适用于工作区，非工作区（包括标题栏和滚动条）以及由于移动窗口而导致未显示的父窗口的任何部分。</para>
        /// <para>设置此标志后，应用程序必须显式使窗口和父窗口中需要重绘的任何部分无效或重绘。</para>
        /// </summary>
        NOREDRAW = 0x0008,
        /// <summary>
        /// 不激活窗口。
        /// <para>如果未设置此标志，则激活窗口并将其移至最顶层或非顶层组的顶部（取决于 hWndInsertAfter 参数的设置）。</para>
        /// </summary>
        NOACTIVATE = 0x0010,
        /// <summary>
        /// 在窗口周围绘制框架（在窗口的类描述中定义）
        /// </summary>
        DRAWFRAME = 0x0020,
        /// <summary>
        /// 应用使用 SetWindowLong 函数设置的新框架样式；将 WM_NCCALCSIZE 消息发送到窗口，即使未更改窗口的大小。
        /// <para>如果未指定此标志，则仅在更改窗口大小时才发送 WM_NCCALCSIZE </para>
        /// </summary>
        FRAMECHANGED = 0x0020,
        /// <summary>
        /// 显示窗口
        /// </summary>
        SHOWWINDOW = 0x0040,
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        HIDEWINDOW = 0x0080,
        /// <summary>
        /// 丢弃客户区的全部内容。
        /// <para>如果未指定此标志，则在调整窗口大小或位置后，将保存客户区的有效内容并将其复制回客户区。</para>
        /// </summary>
        NOCOPYBITS = 0x0100,
        /// <summary>
        /// 不更改所有者窗口在 Z 顺序中的位置。
        /// </summary>
        NOOWNERZORDER = 0x0200,
        /// <summary>
        /// 与 SWP_NOOWNERZORDER 标志相同。
        /// </summary>
        NOREPOSITION = 0x0200,
        /// <summary>
        /// 阻止窗口接收 WM_WINDOWPOSCHANGING 消息
        /// </summary>
        NOSENDCHANGING = 0x0400,
        /// <summary>
        /// 防止生成 WM_SYNCPAINT 消息
        /// </summary>
        DEFERERASE = 0x2000,
        /// <summary>
        /// 如果调用线程和拥有窗口的线程连接到不同的输入队列，则系统会将请求发布到拥有窗口的线程
        /// <para>这样可以防止在其他线程处理请求时调用线程阻塞其执行</para>
        /// </summary>
        ASYNCWINDOWPOS = 0x4000,
    }
    #endregion


    #region FlashWindowEx PFLASHWINFO
    /// <summary>
    /// FLASHWINFO 结构体。注意一定要设置 cbSize 大小，带参的构造函数 (FlashInfo(IntPtr hwnd)) 中已经设置;
    /// <para>FLASHWINFO, *PFLASHWINFO</para>
    /// <para>包含窗口的闪烁状态以及系统应刷新窗口的次数。</para>
    /// <para>示例：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-flashwinfo </para>
    /// </summary>
    public struct FLASHINFO
    {
        /// <summary>
        /// 结构的大小，以字节为单位。
        /// <para>等于 (uint)Marshal.SizeOf(typeof(FLASHWINFO)); </para>
        /// </summary>
        public uint cbSize;
        /// <summary>
        /// 要刷新的窗口的句柄。该窗口可以打开或最小化。
        /// </summary>
        public IntPtr hWnd;
        /// <summary>
        /// 闪光灯状态
        /// </summary>
        public FlashFlag dwFlags;
        /// <summary>
        /// 刷新窗口的次数。
        /// </summary>
        public uint uCount;
        /// <summary>
        /// 刷新窗口的速率，以毫秒为单位。如果 dwTimeout 为零，则该函数使用默认的光标闪烁速率。
        /// </summary>
        public int dwTimeout;
        /// <summary>
        /// FLASHWINFO 结构体
        /// </summary>
        /// <param name="hwnd">要刷新的窗口的句柄。该窗口可以打开或最小化。</param>
        public FLASHINFO(IntPtr hwnd)
        {
            uCount = 3;
            hWnd = hwnd;
            dwTimeout = 500;
            dwFlags = FlashFlag.ALL;
            cbSize = (uint)Marshal.SizeOf(typeof(FLASHINFO));
        }

        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{cbSize}, {hWnd}, {dwFlags}, {uCount}, {dwTimeout}";
        }
    }

    /// <summary>
    /// FLASHINFO 结构体 dwFlags 字段的值之一或值组合
    /// </summary>
    [Flags]
    public enum FlashFlag
    {
        /// <summary>
        /// 停止闪烁。系统将窗口还原到其原始状态。
        /// </summary>
        STOP = 0x00000000,
        /// <summary>
        /// 刷新窗口标题。
        /// </summary>
        CAPTION = 0x00000001,
        /// <summary>
        /// 刷新任务栏按钮。
        /// </summary>
        TRAY = 0x00000002,
        /// <summary>
        /// 同时闪烁窗口标题和任务栏按钮。这等效于设置 FLASHW_CAPTION | FLASHW_TRAY 标志。
        /// </summary>
        ALL = 0x00000003,
        /// <summary>
        /// 连续闪烁，直到设置了FLASHW_STOP标志。
        /// </summary>
        TIMER = 0x00000004,
        /// <summary>
        /// 持续闪烁直到窗口到达前台。
        /// </summary>
        TIMERNOFG = 0x0000000C,
    }
    #endregion


    /// <summary>
    /// 包含全局光标信息。注意 cbSize 大小需要设置
    /// <para>CURSORINFO, * PCURSORINFO, * LPCURSORINFO</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-cursorinfo </para>
    /// </summary>
    public struct CURSORINFO
    {
        /// <summary>
        /// 结构的大小，以字节为单位。
        /// <para>等于 Marshal.SizeOf(typeof(CURSORINFO)); </para>
        /// </summary>
        public int cbSize;
        /// <summary>
        /// 光标状态。
        /// <para>0  光标被隐藏。</para>
        /// <para>CURSOR_SHOWING    0x00000001  光标正在显示。</para>
        /// <para>CURSOR_SUPPRESSED 0x00000002  Windows 8：光标被抑制。该标志指示系统未在绘制光标，因为用户是通过触摸或笔而不是鼠标来提供输入的。</para>
        /// </summary>
        public int flags;
        /// <summary>
        /// 光标的句柄。
        /// </summary>
        public IntPtr hCursor;
        /// <summary>
        /// 接收光标的屏幕坐标的结构。
        /// </summary>
        public POINT ptScreenPos;
        /// <summary>
        /// CURSORINFO 结构体
        /// </summary>
        /// <param name="cursor"></param>
        public CURSORINFO(IntPtr cursor)
        {
            flags = 0x00000001;
            hCursor = cursor;
            ptScreenPos = new POINT();
            cbSize = Marshal.SizeOf(typeof(CURSORINFO));
        }
        /// <summary>
        /// @ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{flags}, {ptScreenPos}";
        }
    }


    #region INPUT MOUSEINPUT KEYBDINPUT HARDWAREINPUT
    /// <summary>
    /// MOUSEINPUT 结构体字段 dwFlags 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-mouseinput </para>
    /// </summary>
    [Flags]
    public enum MouseFlag
    {
        /// <summary>
        /// 移动发生
        /// </summary>
        MOUSEEVENTF_MOVE = 0x0001,
        /// <summary>
        /// 按下左按钮。
        /// </summary>
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        /// <summary>
        /// 释放左按钮。
        /// </summary>
        MOUSEEVENTF_LEFTUP = 0x0004,
        /// <summary>
        /// 按下了右按钮。
        /// </summary>
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        /// <summary>
        /// 释放了右键。
        /// </summary>
        MOUSEEVENTF_RIGHTUP = 0x0010,
        /// <summary>
        /// 按下中间按钮。
        /// </summary>
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        /// <summary>
        /// 中间按钮被释放。
        /// </summary>
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        /// <summary>
        /// 按下了X按钮。
        /// </summary>
        MOUSEEVENTF_XDOWN = 0x0080,
        /// <summary>
        /// X按钮被释放。
        /// </summary>
        MOUSEEVENTF_XUP = 0x0100,
        /// <summary>
        /// 如果鼠标带有滚轮，则滚轮已移动。移动量在mouseData中指定。
        /// </summary>
        MOUSEEVENTF_WHEEL = 0x0800,
        /// <summary>
        /// 如果鼠标带有滚轮，则将滚轮水平移动。移动量在mouseData中指定。 Windows XP / 2000：  不支持此值。
        /// </summary>
        MOUSEEVENTF_HWHEEL = 0x1000,
        /// <summary>
        /// 该WM_MOUSEMOVE消息将不会被合并。默认行为是合并WM_MOUSEMOVE消息。   Windows XP / 2000：  不支持此值。
        /// </summary>
        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
        /// <summary>
        /// 将坐标映射到整个桌面。必须与MOUSEEVENTF_ABSOLUTE一起使用。
        /// </summary>
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
        /// <summary>
        /// 在DX和DY成员含有规范化的绝对坐标。如果未设置该标志，则dx和dy包含相对数据（自上次报告位置以来的位置变化）。无论将哪种类型的鼠标或其他定点设备连接到系统，都可以设置或不设置此标志。有关鼠标相对运动的更多信息，请参见以下“备注”部分。
        /// </summary>
        MOUSEEVENTF_ABSOLUTE = 0x8000,
    }

    /// <summary>
    /// KEYBDINPUT 结构体字段 dwFlags 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-keybdinput </para>
    /// </summary>
    [Flags]
    public enum KeyboardFlag
    {
        /// <summary>
        /// 如果指定，则在扫描代码之前加上前缀字节，该前缀字节的值为0xE0（224）。
        /// </summary>
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        /// <summary>
        /// 如果指定，则释放密钥。如果未指定，则按下该键。
        /// </summary>
        KEYEVENTF_KEYUP = 0x0002,
        /// <summary>
        /// 如果指定，系统将合成VK_PACKET击键。该WVK参数必须为零。该标志只能与KEYEVENTF_KEYUP标志结合使用。
        /// </summary>
        KEYEVENTF_UNICODE = 0x0004,
        /// <summary>
        /// 如果指定，则wScan会识别密钥，而wVk将被忽略。
        /// </summary>
        KEYEVENTF_SCANCODE = 0x0008,
    }

    
    /// <summary>
    /// 包含有关模拟鼠标事件的信息。
    /// <para>MOUSEINPUT, *PMOUSEINPUT, *LPMOUSEINPUT</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-mouseinput </para>
    /// </summary>
    public struct MOUSEINPUT
    {
        /// <summary>
        /// 鼠标的绝对位置或自上一次鼠标事件发生以来的运动量，取决于dwFlags成员的值。绝对数据指定为鼠标的x坐标；相对数据指定为移动的像素数。
        /// </summary>
        public int dx;
        /// <summary>
        /// 鼠标的绝对位置或自上一次鼠标事件发生以来的运动量，取决于dwFlags成员的值。绝对数据指定为鼠标的y坐标；相对数据指定为移动的像素数。
        /// </summary>
        public int dy;
        /// <summary>
        /// 如果 dwFlags 包含MOUSEEVENTF_WHEEL，则 mouseData 指定滚轮移动量。正值表示轮子向前旋转，远离用户；负值表示方向盘朝着用户向后旋转。一轮点击定义为 WHEEL_DELTA，即120。
        /// <para>如果 dwFlags 不包含 MOUSEEVENTF_WHEEL，MOUSEEVENTF_XDOWN 或 MOUSEEVENTF_XUP，则 mouseData 应该为 0。</para>
        /// <para>如果 dwFlags 包含 MOUSEEVENTF_XDOWN或MOUSEEVENTF_XUP，则 mouseData 指定按下或释放了哪个 X 按钮。该值可以是以下标志的任意组合。</para>
        /// <para>1.XBUTTON1    0x0001  设置是否按下或释放第一个X按钮。</para>
        /// <para>2.XBUTTON2    0x0002  设置是否按下或释放第二个X按钮。</para>
        /// </summary>
        public uint mouseData;
        /// <summary>
        /// 一组位标记，用于指定鼠标移动和按钮单击的各个方面。该成员中的位可以是以下值的任何合理组合。
        /// <para>设置指定鼠标按钮状态的位标志以指示状态的变化，而不是持续的状态。例如，如果按下并按住鼠标左键，则在第一次按下左键时会设置 MOUSEEVENTF_LEFTDOWN，但随后的动作不会设置。同样，仅在首次释放按钮时设置 MOUSEEVENTF_LEFTUP。</para>
        /// <para>您不能在 dwFlags 参数中同时指定 MOUSEEVENTF_WHEEL 标志和 MOUSEEVENTF_XDOWN 或 MOUSEEVENTF_XUP标志，因为它们都需要使用 mouseData 字段。</para>
        /// </summary>
        public MouseFlag dwFlags;
        /// <summary>
        /// 事件的时间戳，以毫秒为单位。如果此参数为0，则系统将提供其自己的时间戳。
        /// </summary>
        public uint time;
        /// <summary>
        /// 与鼠标事件关联的附加值。应用程序调用 GetMessageExtraInfo 以获得此额外信息。
        /// </summary>
        public IntPtr dwExtraInfo;
        /// <summary>
        /// 设置鼠标 x, y 位置
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void SetPosition(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"x:{dx}, y:{dy}, data:{mouseData}, flags:{dwFlags}";
        }
    }

    /// <summary>
    /// 包含有关模拟键盘事件的信息。
    /// <para>KEYBDINPUT, * PKEYBDINPUT, * LPKEYBDINPUT;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-keybdinput </para>
    /// </summary>
    public struct KEYBDINPUT
    {
        /// <summary>
        /// 一个虚拟键码( VirtualKeyCode )。该代码必须是1到254之间的值。如果dwFlags成员指定KEYEVENTF_UNICODE，则wVk必须为0。
        /// </summary>
        public VirtualKeyCode wVk;
        /// <summary>
        /// 密钥的硬件扫描代码。如果 dwFlags 指定 KEYEVENTF_UNICODE，则 wScan 指定要发送到前台应用程序的 Unicode 字符。
        /// </summary>
        public ushort wScan;
        /// <summary>
        /// 指定按键的标志组合
        /// </summary>
        public KeyboardFlag dwFlags;
        /// <summary>
        /// 事件的时间戳，以毫秒为单位。如果此参数为零，则系统将提供其自己的时间戳。
        /// </summary>
        public uint time;
        /// <summary>
        /// 与击键关联的附加值。使用 GetMessageExtraInfo 函数可获得此信息。
        /// </summary>
        public IntPtr dwExtraInfo;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"vkc:{wVk}, wScan:{wScan}, flags:{dwFlags}, time:{time}";
        }
    }

    /// <summary>
    /// 包含有关由除键盘或鼠标之外的输入设备生成的模拟消息的信息。
    /// <para>HARDWAREINPUT, * PHARDWAREINPUT, * LPHARDWAREINPUT</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-hardwareinput </para>
    /// </summary>
    public struct HARDWAREINPUT
    {
        /// <summary>
        /// 输入硬件生成的消息。
        /// </summary>
        public uint uMsg;
        /// <summary>
        /// uMsg的lParam参数的低位字(WORD)。
        /// </summary>
        public ushort wParamL;
        /// <summary>
        /// uMsg的lParam参数的高位字(WORD)。
        /// </summary>
        public ushort wParamH;
    }

    /// <summary>
    /// INPUT 结构体字段 type 的值之一
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// 该事件是鼠标事件。使用联合的mi结构。
        /// </summary>
        MOUSE = 0,
        /// <summary>
        /// 该事件是键盘事件。使用联合的ki结构。
        /// </summary>
        KEYBOARD = 1,
        /// <summary>
        /// 该事件是硬件事件。使用联合的hi结构。
        /// </summary>
        HARDWARE = 2,
    }

    /// <summary>
    /// 通过使用 SendInput 来存储信息合成输入事件，如按键、鼠标移动和鼠标点击。
    /// <para>INPUT_KEYBOARD 支持非键盘输入法，例如手写识别或语音识别，就好像它是使用 KEYEVENTF_UNICODE 标志输入的文本一样。有关更多信息，请参见 KEYBDINPUT 的备注部分。</para>
    /// <para>INPUT, *PINPUT, *LPINPUT</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-input </para>
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        /// <summary>
        /// 输入事件的类型。
        /// </summary>
        [FieldOffset(0)]
        public InputType type;
        /// <summary>
        /// 有关模拟鼠标事件的信息。
        /// </summary>
        [FieldOffset(4)]
        public MOUSEINPUT Mouse;
        /// <summary>
        /// 有关模拟键盘事件的信息。
        /// </summary>
        [FieldOffset(4)]
        public KEYBDINPUT Keyboard;
        /// <summary>
        /// 有关模拟硬件事件的信息。
        /// </summary>
        [FieldOffset(4)]
        public HARDWAREINPUT Hardware;
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("type:{0} input:{1}", type, type == InputType.MOUSE ? Mouse.ToString() : type == InputType.KEYBOARD ? Keyboard.ToString() : Hardware.ToString());
        }
    }
    #endregion


    #region Functions Arguments Enumables Value
    /// <summary>
    /// MapVirtualKey 函数参数 uMapType 的值之一
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-mapvirtualkeya </para>
    /// </summary>
    public enum MapVKType
    {
        /// <summary>
        /// uCode 是虚拟密钥代码，并转换为扫描代码。如果它是不能区分左手键和右手键的虚拟键代码，则返回左手扫描代码。如果没有转换，则该函数返回0。
        /// </summary>
        VK_TO_VSC = 0,
        /// <summary>
        /// uCode 是一种扫描代码，并转换为虚拟键代码，该虚拟键代码无法区分左手键和右手键。如果没有转换，则该函数返回0。
        /// </summary>
        VSC_TO_VK = 1,
        /// <summary>
        /// uCode 是虚拟键码，并在返回值的低位字中转换为未移位的字符值。死键（变音符号）通过设置返回值的最高位来指示。如果没有转换，则该函数返回0。
        /// </summary>
        VK_TO_CHAR = 2,
        /// <summary>
        /// uCode 是一种扫描代码，并被翻译成可区分左手键和右手键的虚拟键码。如果没有转换，则该函数返回0。
        /// </summary>
        VSC_TO_VK_EX = 3,
    }

    /// <summary>
    /// GetWindowRgn 函数返回值之一 （排列值存在问题）
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindowrgn </para>
    /// </summary>
    public enum GwrResult
    {
        /// <summary>
        /// 空区域，该区域为空。
        /// </summary>
        NULLREGION,
        /// <summary>
        /// 简单区域，该区域是单个矩形。
        /// </summary>
        SIMPLEREGION,
        /// <summary>
        /// 复杂区域，该区域不止一个矩形。
        /// </summary>
        COMPLEXREGION,
        /// <summary>
        /// 错误，指定的窗口没有区域，或者尝试返回该区域时发生错误。
        /// </summary>
        ERROR,
    }

    /// <summary>
    /// <see cref="User32.ShowWindow"/> 函数参考 nCmdShow 的值之一
    /// </summary>
    public enum SwCmd
    {
        /// <summary>
        /// 隐藏该窗口并激活另一个窗口。
        /// </summary>
        HIDE = 0,
        /// <summary>
        /// 激活并显示一个窗口。如果窗口最小化或最大化，则系统会将其还原到其原始大小和位置。首次显示窗口时，应用程序应指定此标志。
        /// </summary>
        SHOWNORMAL = 1,
        /// <summary>
        /// 激活窗口并将其显示为最小化窗口。
        /// </summary>
        SHOWMINIMIZED = 2,
        /// <summary>
        /// 激活窗口并将其显示为最大化窗口。
        /// </summary>
        SHOWMAXIMIZED = 3,
        /// <summary>
        /// 最大化指定的窗口。
        /// </summary>
        MAXIMIZE = 3,
        /// <summary>
        /// 以最新大小和位置显示窗口。该值类似于SW_SHOWNORMAL，除了未激活窗口。
        /// </summary>
        SHOWNOACTIVATE = 4,
        /// <summary>
        /// 激活窗口并以其当前大小和位置显示它。
        /// </summary>
        SHOW = 5,
        /// <summary>
        /// 最小化指定的窗口并以Z顺序激活下一个顶级窗口。
        /// </summary>
        MINIMIZE = 6,
        /// <summary>
        /// 将窗口显示为最小化窗口。该值类似于SW_SHOWMINIMIZED，除了未激活窗口。
        /// </summary>
        SHOWMINNOACTIVE = 7,
        /// <summary>
        /// 以当前大小和位置显示窗口。该值与SW_SHOW相似，除了不激活窗口。
        /// </summary>
        SHOWNA = 8,
        /// <summary>
        /// 激活并显示窗口。如果窗口最小化或最大化，则系统会将其还原到其原始大小和位置。恢复最小化窗口时，应用程序应指定此标志。
        /// </summary>
        RESTORE = 9,
        /// <summary>
        /// 根据启动应用程序的程序传递给CreateProcess函数的STARTUPINFO结构中指定的SW_值设置显示状态。
        /// </summary>
        SHOWDEFAULT = 10,
        /// <summary>
        /// 最小化一个窗口，即使拥有该窗口的线程没有响应。仅当最小化来自其他线程的窗口时，才应使用此标志。
        /// </summary>
        FORCEMINIMIZE = 11,

        //MAX = 11,
    }

    /// <summary>
    /// <see cref="User32.GetWindow"/> 函数参考 uCmd 的值之一
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-getwindow </para>
    /// </summary>
    public enum GwCmd
    {
        /// <summary>
        /// 检索到的句柄标识Z顺序中最高的同一类型的窗口。
        /// <para>如果指定的窗口是最上面的窗口，则该句柄标识最上面的窗口。如果指定的窗口是顶级窗口，则该句柄标识顶级窗口。如果指定的窗口是子窗口，则该句柄标识同级窗口。</para>
        /// </summary>
        GW_HWNDFIRST = 0,
        /// <summary>
        /// 检索到的句柄标识Z顺序中最低的同一类型的窗口。
        /// <para>如果指定的窗口是最上面的窗口，则该句柄标识最上面的窗口。如果指定的窗口是顶级窗口，则该句柄标识顶级窗口。如果指定的窗口是子窗口，则该句柄标识同级窗口。</para>
        /// </summary>
        GW_HWNDLAST = 1,
        /// <summary>
        /// 检索到的句柄以Z顺序标识指定窗口下方的窗口。
        /// <para>如果指定的窗口是最上面的窗口，则该句柄标识最上面的窗口。如果指定的窗口是顶级窗口，则该句柄标识顶级窗口。如果指定的窗口是子窗口，则该句柄标识同级窗口。</para>
        /// </summary>
        GW_HWNDNEXT = 2,
        /// <summary>
        /// 检索到的句柄以Z顺序标识指定窗口上方的窗口。
        /// <para>如果指定的窗口是最上面的窗口，则该句柄标识最上面的窗口。如果指定的窗口是顶级窗口，则该句柄标识顶级窗口。如果指定的窗口是子窗口，则该句柄标识同级窗口。</para>
        /// </summary>
        GW_HWNDPREV = 3,
        /// <summary>
        /// 检索到的句柄标识指定窗口的所有者窗口（如果有）。有关更多信息，请参见“ 拥有的Windows”。
        /// </summary>
        GW_OWNER = 4,
        /// <summary>
        /// 如果指定的窗口是父窗口，则检索到的句柄在Z顺序的顶部标识子窗口。否则，检索到的句柄为NULL。该功能仅检查指定窗口的子窗口。它不检查后代窗口。
        /// </summary>
        GW_CHILD = 5,
        /// <summary>
        /// 检索到的句柄标识指定窗口拥有的启用的弹出窗口（搜索使用通过GW_HWNDNEXT找到的第一个此类窗口）；否则，如果没有启用的弹出窗口，则检索到的句柄是指定窗口的句柄。
        /// </summary>
        GW_ENABLEDPOPUP = 6,
    }

    /// <summary>
    /// RegisterHotKey 函数参数 fsModifiers 的值之一或值组合
    /// <para>OR <see cref="MessageType.WM_HOTKEY"/> lParam</para>
    /// <para>参考 https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-registerhotkey </para>
    /// </summary>
    [Flags]
    public enum RhkModifier
    {
        /// <summary>
        /// 必须按住ALT键。
        /// </summary>
        ALT = 0x0001,
        /// <summary>
        /// 必须按住CTRL键。
        /// </summary>
        CONTROL = 0x0002,
        /// <summary>
        /// 必须按住SHIFT键。
        /// </summary>
        SHIFT = 0x0004,
        /// <summary>
        /// 按住WINDOWS键。这些键带有Windows徽标。保留与WINDOWS键相关的键盘快捷键，供操作系统使用。
        /// </summary>
        WIN = 0x0008,
        /// <summary>
        /// 更改热键行为，以使键盘自动重复操作不会产生多个热键通知。Windows Vista：  不支持此标志。
        /// </summary>
        NOREPEAT = 0x4000,
    }

    /// <summary>
    /// AnimateWindow 函数参数 dwFlags 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-animatewindow </para>
    /// </summary>
    [Flags]
    public enum AwFlag
    {
        /// <summary>
        /// 从左到右对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        HOR_POSITIVE = 0x00000001,
        /// <summary>
        /// 从右到左对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与AW_CENTER或AW_BLEND一起使用时将被忽略。
        /// </summary>
        HOR_NEGATIVE = 0x00000002,
        /// <summary>
        /// 从上到下对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        VER_POSITIVE = 0x00000004,
        /// <summary>
        /// 从底部到顶部对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        VER_NEGATIVE = 0x00000008,
        /// <summary>
        /// 如果使用 AW_HIDE，则使窗口看起来向内折叠；如果不使用AW_HIDE，则使窗口向外折叠。各种方向标记无效。
        /// </summary>
        CENTER = 0x00000010,
        /// <summary>
        /// 隐藏窗口。默认情况下，显示窗口。
        /// </summary>
        HIDE = 0x00010000,
        /// <summary>
        /// 激活窗口。不要将此值与 AW_HIDE 一起使用。
        /// </summary>
        ACTIVATE = 0x00020000,
        /// <summary>
        /// 使用幻灯片动画。默认情况下，使用滚动动画。与 AW_CENTER 一起使用时，将忽略此标志。
        /// </summary>
        SLIDE = 0x00040000,
        /// <summary>
        /// 使用淡入淡出效果。仅当 hwnd 是顶级窗口时，才可以使用此标志。
        /// </summary>
        BLEND = 0x00080000,
    }


    /// <summary>
    /// PeekMessage 函数参数 wRemoveMsg 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-peekmessagea </para>
    /// </summary>
    [Flags]
    public enum PmFlag
    {
        /// <summary>
        /// 通过 PeekMessage 处理后，消息不会从队列中删除。
        /// </summary>
        NOREMOVE = 0x0000,
        /// <summary>
        /// 经过 PeekMessage 处理后，消息将从队列中删除。
        /// </summary>
        REMOVE = 0x0001,
        /// <summary>
        /// 防止系统释放任何等待调用方进入空闲状态的线程（请参见WaitForInputIdle）。
        /// 将此值与 PM_NOREMOVE 或 PM_REMOVE 结合使用。
        /// </summary>
        NOYIELD = 0x0002,
    }

    /// <summary>
    /// Key State Masks for Mouse Messages (wParam)
    /// <para>MessageType WM_MouseXXX wParam value type</para>
    /// </summary>
    [Flags]
    public enum MouseKey
    {
        /// <summary>
        /// </summary>
        MK_LBUTTON = 0x0001,
        /// <summary>
        /// </summary>
        MK_RBUTTON = 0x0002,
        /// <summary>
        /// </summary>
        MK_SHIFT = 0x0004,
        /// <summary>
        /// </summary>
        MK_CONTROL = 0x0008,
        /// <summary>
        /// </summary>
        MK_MBUTTON = 0x0010,
        /// <summary>
        /// </summary>
        MK_XBUTTON1 = 0x0020,
        /// <summary>
        /// </summary>
        MK_XBUTTON2 = 0x0040,
    }

    /// <summary>
    /// Syatem Window Style
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/window-styles </para>
    /// </summary>
    [Flags]
    public enum WindowStyle : uint
    {
        /// <summary>
        /// 该窗口是一个重叠的窗口。重叠的窗口具有标题栏和边框。与 WS_TILED 样式相同。
        /// </summary>
        WS_OVERLAPPED = 0x00000000,
        /// <summary>
        /// 窗口是一个弹出窗口。此样式不能与 WS_CHILD 样式一起使用。
        /// </summary>
        WS_POPUP = 0x80000000,
        /// <summary>
        /// 该窗口是子窗口。具有这种样式的窗口不能具有菜单栏。此样式不能与 WS_POPUP 样式一起使用。
        /// </summary>
        WS_CHILD = 0x40000000,
        /// <summary>
        /// 最初将窗口最小化。与 S_ICONIC 样式相同。
        /// </summary>
        WS_MINIMIZE = 0x20000000,
        /// <summary>
        /// 该窗口最初是可见的。可以使用 ShowWindow 或 SetWindowPos 函数打开和关闭此样式。
        /// </summary>
        WS_VISIBLE = 0x10000000,
        /// <summary>
        /// 该窗口最初被禁用。禁用的窗口无法接收来自用户的输入。要在创建窗口后更改此设置，请使用 EnableWindow 函数。
        /// </summary>
        WS_DISABLED = 0x08000000,
        /// <summary>
        /// 相对于彼此剪辑子窗口；也就是说当特定的子窗口接收到 WM_PAINT 消息时，WS_CLIPSIBLINGS 样式会将所有其他重叠的子窗口剪切到要更新的子窗口区域之外。如果未指定 WS_CLIPSIBLINGS 并且子窗口重叠，则在子窗口的客户区域内进行绘制时，可以在相邻子窗口的客户区域内进行绘制。
        /// </summary>
        WS_CLIPSIBLINGS = 0x04000000,
        /// <summary>
        /// 在父窗口内进行绘制时，不包括子窗口所占的区域。创建父窗口时使用此样式。
        /// </summary>
        WS_CLIPCHILDREN = 0x02000000,
        /// <summary>
        /// 该窗口最初被最大化。
        /// </summary>
        WS_MAXIMIZE = 0x01000000,
        /// <summary>
        /// 窗口具有标题栏（包括 WS_BORDER 样式）。WS_BORDER | WS_DLGFRAME  
        /// </summary>
        WS_CAPTION = 0x00C00000,
        /// <summary>
        /// 窗口具有细线边框。
        /// </summary>
        WS_BORDER = 0x00800000,
        /// <summary>
        /// 窗口具有通常用于对话框的样式的边框。具有这种样式的窗口不能具有标题栏。
        /// </summary>
        WS_DLGFRAME = 0x00400000,
        /// <summary>
        /// 该窗口具有垂直滚动条。
        /// </summary>
        WS_VSCROLL = 0x00200000,
        /// <summary>
        /// 该窗口具有水平滚动条。
        /// </summary>
        WS_HSCROLL = 0x00100000,
        /// <summary>
        /// 该窗口的标题栏上有一个窗口菜单。该 WS_CAPTION 风格也必须指定。
        /// </summary>
        WS_SYSMENU = 0x00080000,
        /// <summary>
        /// 窗口具有大小调整边框。与 WS_SIZEBOX 样式相同。
        /// </summary>
        WS_THICKFRAME = 0x00040000,
        /// <summary>
        /// 该窗口是一组控件中的第一个控件。该组由该第一个控件和在其后定义的所有控件组成，直到下一个具有 WS_GROUP 样式的下一个控件。每个组中的第一个控件通常具有 WS_TABSTOP 样式，以便用户可以在组之间移动。用户随后可以使用方向键将键盘焦点从组中的一个控件更改为组中的下一个控件。
        /// <para>您可以打开和关闭此样式以更改对话框导航。若要在创建窗口后更改此样式，请使用 SetWindowLong 函数。</para>
        /// </summary>
        WS_GROUP = 0x00020000,
        /// <summary>
        /// 该窗口是一个控件，当用户按下 TAB 键时可以接收键盘焦点。按下 TAB 键可将键盘焦点更改为 WS_TABSTOP 样式的下一个控件。
        /// <para>您可以打开和关闭此样式以更改对话框导航。若要在创建窗口后更改此样式，请使用 SetWindowLong 函数。为了使用户创建的窗口和无模式对话框可与制表符一起使用，请更改消息循环以调用 IsDialogMessage 函数。</para>
        /// </summary>
        WS_TABSTOP = 0x00010000,
        /// <summary>
        /// 该窗口有一个最小化按钮。不能与 WS_EX_CONTEXTHELP 样式结合使用。该 WS_SYSMENU 风格也必须指定。
        /// </summary>
        WS_MINIMIZEBOX = 0x00020000,
        /// <summary>
        /// 该窗口具有最大化按钮。不能与 WS_EX_CONTEXTHELP 样式结合使用。该 WS_SYSMENU 风格也必须指定。
        /// </summary>
        WS_MAXIMIZEBOX = 0x00010000,
        /// <summary>
        /// 该窗口是一个重叠的窗口。重叠的窗口具有标题栏和边框。与 WS_OVERLAPPED 样式相同。
        /// </summary>
        WS_TILED = WS_OVERLAPPED,
        /// <summary>
        /// 最初将窗口最小化。与 WS_MINIMIZE 样式相同。
        /// </summary>
        WS_ICONIC = WS_MINIMIZE,
        /// <summary>
        /// 窗口具有大小调整边框。与 WS_THICKFRAME 样式相同。
        /// </summary>
        WS_SIZEBOX = WS_THICKFRAME,
        /// <summary>
        /// 该窗口是一个重叠的窗口。与 WS_OVERLAPPEDWINDOW 样式相同。
        /// </summary>
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
        /// <summary>
        /// 该窗口是一个重叠的窗口。与 WS_TILEDWINDOW 样式相同。
        /// </summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        /// <summary>
        /// 该窗口是一个弹出窗口。该 WS_CAPTION 和 WS_POPUPWINDOW 风格一定要结合使窗口菜单可见。
        /// </summary>
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        /// <summary>
        /// 与 WS_CHILD 样式相同。
        /// </summary>
        WS_CHILDWINDOW = WS_CHILD,
    }

    /// <summary>
    /// Extended Window Styles
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/winmsg/extended-window-styles </para>
    /// </summary>
    [Flags]
    public enum WindowSytleEx : uint
    {
        /// <summary>
        /// 窗口有一个双边框。该窗口可以任选地用一个标题栏，通过指定所创建的 WS_CAPTION 在样式 dwStyle 参数。
        /// </summary>
        WS_EX_DLGMODALFRAME = 0x00000001,
        /// <summary>
        /// 使用此样式创建的子窗口在创建或销毁时不会将 WM_PARENTNOTIFY 消息发送到其父窗口。
        /// </summary>
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        /// <summary>
        /// 该窗口应放置在所有非最上面的窗口上方，并且即使在停用该窗口的情况下也应保持在它们之上。若要添加或删除此样式，请使用 SetWindowPos 函数。
        /// </summary>
        WS_EX_TOPMOST = 0x00000008,
        /// <summary>
        /// 该窗口接受拖放文件。
        /// </summary>
        WS_EX_ACCEPTFILES = 0x00000010,
        /// <summary>
        /// 在绘制窗口下方的兄弟姐妹（由同一线程创建）之前，不应绘制窗口。该窗口显示为透明，因为基础同级窗口的位已被绘制。
        /// <para>要获得透明性而没有这些限制，请使用 SetWindowRgn 函数。</para>
        /// </summary>
        WS_EX_TRANSPARENT = 0x00000020,
        /// <summary>
        /// 该窗口是MDI子窗口。
        /// </summary>
        WS_EX_MDICHILD = 0x00000040,
        /// <summary>
        /// 该窗口旨在用作浮动工具栏。工具窗口的标题栏比普通标题栏短，并且窗口标题使用较小的字体绘制。
        /// <para>当用户按下 ALT + TAB 时，工具窗口不会出现在任务栏或对话框中。如果工具窗口具有系统菜单，则其图标不会显示在标题栏上。但是您可以通过右键单击或键入 ALT + SPACE 来显示系统菜单。</para>
        /// </summary>
        WS_EX_TOOLWINDOW = 0x00000080,
        /// <summary>
        /// 窗口的边框带有凸起的边缘。
        /// </summary>
        WS_EX_WINDOWEDGE = 0x00000100,
        /// <summary>
        /// 窗口的边框带有凹陷的边缘。
        /// </summary>
        WS_EX_CLIENTEDGE = 0x00000200,
        /// <summary>
        /// 窗口的标题栏包含一个问号。当用户单击问号时，光标将变为带有指针的问号。如果用户然后单击子窗口，则该子窗口会收到 WM_HELP 消息。
        /// <para>子窗口应将消息传递给父窗口过程，该过程应使用HELP_WM_HELP命令调用 WinHelp 函数。帮助应用程序显示一个弹出窗口，通常包含子窗口的帮助。WS_EX_CONTEXTHELP 不能与 WS_MAXIMIZEBOX 或WS_MINIMIZEBOX 样式一起使用。</para>
        /// </summary>
        WS_EX_CONTEXTHELP = 0x00000400,
        /// <summary>
        /// 该窗口具有通用的“右对齐”属性。这取决于窗口类。仅当外壳语言是希伯来语，阿拉伯语或其他支持阅读顺序对齐的语言时，此样式才有效。否则，样式将被忽略。
        /// <para>将 WS_EX_RIGHT 样式用于静态或编辑控件分别具有与使用 SS_RIGHT 或 ES_RIGHT 样式相同的效果。通过按钮控件使用此样式与使用 BS_RIGHT 和 BS_RIGHTBUTTON 样式具有相同的效果。</para>
        /// </summary>
        WS_EX_RIGHT = 0x00001000,
        /// <summary>
        /// 该窗口具有通用的左对齐属性。这是默认值。
        /// </summary>
        WS_EX_LEFT = 0x00000000,
        /// <summary>
        /// 如果外壳语言是希伯来语，阿拉伯语或其他支持阅读顺序对齐的语言，则使用从右到左的阅读顺序属性显示窗口文本。对于其他语言，样式将被忽略。
        /// </summary>
        WS_EX_RTLREADING = 0x00002000,
        /// <summary>
        /// 使用从左到右的阅读顺序属性显示窗口文本。这是默认值。
        /// </summary>
        WS_EX_LTRREADING = 0x00000000,
        /// <summary>
        /// 如果外壳语言是希伯来语，阿拉伯语或其他支持阅读顺序对齐的语言，则垂直滚动条（如果有）位于客户区域的左侧。对于其他语言，样式将被忽略。
        /// </summary>
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        /// <summary>
        /// 垂直滚动条（如果有）在客户区的右侧。这是默认值。
        /// </summary>
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        /// <summary>
        /// 窗口本身包含子窗口，应参与对话框导航。如果指定了此样式，则对话框管理器在执行导航操作（例如处理 TAB 键，箭头键或键盘助记符）时会循环到此窗口的子级中。
        /// </summary>
        WS_EX_CONTROLPARENT = 0x00010000,
        /// <summary>
        /// 该窗口具有三维边框样式，旨在用于不接受用户输入的项目。
        /// </summary>
        WS_EX_STATICEDGE = 0x00020000,
        /// <summary>
        /// 可见时将顶级窗口强制到任务栏上。
        /// </summary>
        WS_EX_APPWINDOW = 0x00040000,
        /// <summary>
        /// 该窗口是一个重叠的窗口。
        /// </summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        /// <summary>
        /// 该窗口是调色板窗口，这是一个无模式对话框，显示了一系列命令。
        /// </summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        /// <summary>
        /// 窗户是一个分层的窗户。如果窗口中有一个不能用这种风格类样式之一 CS_OWNDC 或 CS_CLASSDC。
        /// <para>Windows 8：该 WS_EX_LAYERED 样式支持顶级窗口和子窗口。以前的 Windows 版本仅对顶级窗口支持 WS_EX_LAYERED。</para>
        /// </summary>
        WS_EX_LAYERED = 0x00080000,
        /// <summary>
        /// 该窗口不会将其窗口布局传递给其子窗口。
        /// </summary>
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        /// <summary>
        /// 窗口不渲染到重定向表面。这适用于不具有可见内容或使用除表面以外的机制来提供其视觉效果的窗口。
        /// </summary>
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        /// <summary>
        /// 如果外壳语言是希伯来语，阿拉伯语或其他支持阅读顺序对齐的语言，则窗口的水平原点在右边缘。水平值增加到左侧。
        /// </summary>
        WS_EX_LAYOUTRTL = 0x00400000,
        /// <summary>
        /// 使用双缓冲以从下到上的绘制顺序绘制窗口的所有后代。从下到上的绘画顺序允许后代窗口具有半透明（alpha）和透明（color-key）效果，但前提是后代窗口也设置了 WS_EX_TRANSPARENT 位。双缓冲允许绘制窗口及其后代，而不会闪烁。如果窗口有此不能使用类样式之一 CS_OWNDC 或 CS_CLASSDC。Windows 2000：不支持此样式。
        /// </summary>
        WS_EX_COMPOSITED = 0x02000000,
        /// <summary>
        /// 当用户单击它时，以这种样式创建的顶级窗口不会成为前台窗口。当用户最小化或关闭前景窗口时，系统不会将此窗口置于前景。不应通过程序访问或使用讲述人等可访问技术通过键盘导航来激活该窗口。
        /// <para>要激活该窗口，请使用 SetActiveWindow 或 SetForegroundWindow 函数。默认情况下，该窗口不显示在任务栏上。要强制窗口显示在任务栏上，请使用 WS_EX_APPWINDOW 样式。</para>
        /// </summary>
        WS_EX_NOACTIVATE = 0x08000000,
    }

    /// <summary>
    /// 包含窗口信息。
    /// <para>WINDOWINFO, * PWINDOWINFO, * LPWINDOWINFO;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-windowinfo </para>
    /// </summary>
    public struct WINDOWINFO
    {
        /// <summary>
        /// 结构的大小，以字节为单位。呼叫者必须将此成员设置为 sizeof(WINDOWINFO)。
        /// </summary>
        public uint cbSize;
        /// <summary>
        /// 窗口的坐标。
        /// </summary>
        public RECT rcWindow;
        /// <summary>
        /// 客户区域的坐标。
        /// </summary>
        public RECT rcClient;
        /// <summary>
        /// 窗口样式。有关窗口样式的表，请参见 <see cref="WindowStyle"/>。
        /// </summary>
        public WindowStyle dwStyle;
        /// <summary>
        /// 扩展的窗口样式。有关扩展窗口样式的表，请参见 。
        /// </summary>
        public WindowSytleEx dwExStyle;
        /// <summary>
        /// 窗口状态。如果此成员是 WS_ACTIVECAPTION（0x0001），则该窗口处于活动状态。否则，该成员为零。
        /// </summary>
        public uint dwWindowStatus;
        /// <summary>
        /// 窗口边框的宽度，以像素为单位。
        /// </summary>
        public uint cxWindowBorders;
        /// <summary>
        /// 窗口边框的高度，以像素为单位。
        /// </summary>
        public uint cyWindowBorders;
        /// <summary>
        /// 窗口类原子（请参见RegisterClass）。
        /// </summary>
        public ushort atomWindowType;
        /// <summary>
        /// 创建窗口的应用程序的 Windows 版本。
        /// </summary>
        public ushort wCreatorVersion;
        /// <summary>
        /// 创建一个已经设置 cbSize 大小的 WINDOWINFO 对象。
        /// </summary>
        /// <returns></returns>
        public static WINDOWINFO Create()
        {
            return new WINDOWINFO() { cbSize = (uint)Marshal.SizeOf(typeof(WINDOWINFO)) };
        }
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{rcWindow}, {rcClient}, {dwStyle}, {dwExStyle}, {dwWindowStatus}, {cxWindowBorders} {cyWindowBorders}, {atomWindowType}, {wCreatorVersion}";
        }
    }

    /// <summary>
    /// 包含有关窗口的大小和位置的信息。
    /// <para>WINDOWPOS, * LPWINDOWPOS, * PWINDOWPOS;</para>
    /// <para>BeginDeferWindowPos, DeferWindowPos, EndDeferWindowPos</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-windowpos?redirectedfrom=MSDN </para>
    /// </summary>
    public struct WINDOWPOS
    {
        /// <summary>
        /// 窗口在Z顺序中的位置（前后位置）。该成员可以是放置该窗口的窗口的句柄，也可以是 <see cref="User32.SetWindowPos(IntPtr, int, int, int, int, int, SwpFlag)"/> 函数列出的特殊值之一。
        /// </summary>
        public IntPtr hwndInsertAfter;
        /// <summary>
        /// 窗口的句柄。
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// 窗口左边缘的位置。
        /// </summary>
        public int x;
        /// <summary>
        /// 窗口顶部边缘的位置。
        /// </summary>
        public int y;
        /// <summary>
        /// 窗口宽度，以像素为单位。
        /// </summary>
        public int cx;
        /// <summary>
        /// 窗口高度，以像素为单位。
        /// </summary>
        public int cy;
        /// <summary>
        /// 窗口位置。该成员可以是 <see cref="SwpFlag"/> 一个或多个值。
        /// </summary>
        public SwpFlag flags;
    }

    #endregion

    #region Touch Info
    /// <summary>
    /// 封装用于触摸输入的数据。
    /// <para>TOUCHINPUT, * PTOUCHINPUT</para>
    /// <para>typedef TOUCHINPUT const * PCTOUCHINPUT;</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-touchinput </para>
    /// </summary>
    public struct TOUCHINPUT
    {
        /// <summary>
        /// 触摸输入的x坐标（水平点）。该成员以物理屏幕坐标的百分之一像素表示。
        /// </summary>
        public int x;
        /// <summary>
        /// 触摸输入的y坐标（垂直点）。该成员以物理屏幕坐标的百分之一像素表示。
        /// </summary>
        public int y;
        /// <summary>
        /// 源输入设备的设备句柄。触摸输入提供程序会在运行时为每个设备提供唯一的提供程序。
        /// </summary>
        public IntPtr hSource;
        /// <summary>
        /// 区分特定触摸输入的触摸点标识符。从接触点下降到恢复接触点，此值在触摸接触序列中保持一致。以后可以将 ID 重新用于后续联系人。
        /// </summary>
        public uint dwID;
        /// <summary>
        /// 一组位标志，用于指定触摸点按下，释放和运动的各个方面。
        /// </summary>
        public TouchEventFlag dwFlags;
        /// <summary>
        /// 一组位标志，用于指定结构中的哪些可选字段包含有效值。可选字段中有效信息的可用性是特定于设备的。仅当在 dwMask 中设置了相应的位时，应用程序才应使用可选的字段值。
        /// </summary>
        public TouchMaskFlag dwMask;
        /// <summary>
        /// 事件的时间戳，以毫秒为单位。消费应用程序应注意，系统不对此字段执行任何验证；当未设置 TOUCHINPUTMASKF_TIMEFROMSYSTEM 标志时，此字段中值的准确性和顺序完全取决于触摸输入提供程序。
        /// </summary>
        public uint dwTime;
        /// <summary>
        /// 与触摸事件关联的附加值。
        /// </summary>
        public IntPtr dwExtraInfo;
        /// <summary>
        /// 在物理屏幕坐标中，触摸接触区域的宽度以百分之一像素为单位。仅当 dwMask 成员设置了 TOUCHEVENTFMASK_CONTACTAREA 标志时，此值才有效。
        /// </summary>
        public uint cxContact;
        /// <summary>
        /// 在物理屏幕坐标中，触摸接触区域的高度以百分之一像素为单位。仅当 dwMask 成员设置了 TOUCHEVENTFMASK_CONTACTAREA 标志时，此值才有效。
        /// </summary>
        public uint cyContact;
    }

    /// <summary>
    /// 结构体 TOUCHINPUT 属性 dwFlags 的值之一或值组合
    /// <para> 如果计算机上的目标硬件不支持悬停，则当设置 TOUCHEVENTF_UP 标志时，将清除 TOUCHEVENTF_INRANGE 标志。如果计算机上的目标硬件支持悬停，则将分别设置 TOUCHEVENTF_UP 和 TOUCHEVENTF_INRANGE 标志</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-touchinput </para>
    /// </summary>
    [Flags]
    public enum TouchEventFlag
    {
        /// <summary>
        /// 发生了移动。不能与 TOUCHEVENTF_DOWN 结合使用。
        /// </summary>
        TOUCHEVENTF_MOVE = 0x0001,
        /// <summary>
        /// 通过新的联系人建立了相应的接触点。不能与 TOUCHEVENTF_MOVE 或 TOUCHEVENTF_UP 结合使用。
        /// </summary>
        TOUCHEVENTF_DOWN = 0x0002,
        /// <summary>
        /// 触摸点已删除。
        /// </summary>
        TOUCHEVENTF_UP = 0x0004,
        /// <summary>
        /// 接触点在范围内。此标志用于在兼容硬件上启用触摸悬浮支持。不需要支持悬停的应用程序可以忽略此标志。
        /// </summary>
        TOUCHEVENTF_INRANGE = 0x0008,
        /// <summary>
        /// 指示此 TOUCHINPUT 结构对应于主要接触点。有关主要接触点的更多信息，请参见以下文本。
        /// </summary>
        TOUCHEVENTF_PRIMARY = 0x0010,
        /// <summary>
        /// 使用GetTouchInputInfo接收时，此输入未合并。
        /// </summary>
        TOUCHEVENTF_NOCOALESCE = 0x0020,
        /// <summary>
        /// 触摸事件来自用户的手掌。
        /// </summary>
        TOUCHEVENTF_PALM = 0x0080,
    }
    /// <summary>
    /// 结构体 TOUCHINPUT 属性 dwMask 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-touchinput </para>
    /// </summary>
    [Flags]
    public enum TouchMaskFlag
    {
        /// <summary>
        /// cxContact 和 cyContact 有效。有关主要接触点的更多信息，请参见以下文本。
        /// </summary>
        TOUCHINPUTMASKF_CONTACTAREA = 0x0004,
        /// <summary>
        /// dwExtraInfo 有效。
        /// </summary>
        TOUCHINPUTMASKF_EXTRAINFO = 0x0002,
        /// <summary>
        /// 系统时间在 TOUCHINPUT 结构中设置。
        /// </summary>
        TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001,
    }

    #endregion

    #region Pointer Touch Info
    /// <summary>
    /// Touch 反馈模式
    /// <para> <see cref="User32.InitializeTouchInjection"/>  函数参数 dwMode 的值之一</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/input_touchinjection/constants </para>
    /// </summary>
    public enum TouchFeedbackMode
    {
        /// <summary>
        /// 指定默认的触摸可视化。最终的用户在 Pen and Touch 控制面板中的设置可能会抑制注入的触摸反馈。
        /// </summary>
        DEFAULT = 0x1,
        /// <summary>
        /// 指定间接触摸可视化。注入的触摸反馈将覆盖“笔和触摸”控制面板中的最终用户设置。
        /// </summary>
        INDIRECT = 0x2,
        /// <summary>
        /// 指定没有触摸可视化。TOUCH_FEEDBACK_INDIRECT | TOUCH_FEEDBACK_NONE 应用程序和控件提供的触摸反馈可能不会受到影响。
        /// </summary>
        NONE = 0x3,
    }

    /// <summary>
    /// 指针输入类型。
    /// <para> <see cref="POINTERINFO"/> 结构体字段 <see cref="POINTERINFO.pointerType"/> 的值之一 </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ne-winuser-tagpointer_input_type </para>
    /// </summary>
    public enum PointerInputType
    {
        /// <summary>
        /// 通用指针类型。此类型永远不会出现在指针消息或指针数据中。一些数据查询功能允许调用者将查询限制为特定的指针类型。所述 PT_POINTER 类型可以在这些功能被用来指定该查询是包括所有类型的指针
        /// </summary>
        POINTER = 1,
        /// <summary>
        /// 触摸指针类型。
        /// </summary>
        TOUCH = 2,
        /// <summary>
        /// 笔指针类型。
        /// </summary>
        PEN = 3,
        /// <summary>
        /// 鼠标指针类型。
        /// </summary>
        MOUSE = 4,
        /// <summary>
        /// 触摸板指针类型（Windows 8.1和更高版本）。
        /// </summary>
        TOUCHPAD = 5
    };

    /// <summary>
    /// <see cref="POINTERINFO"/> 结构体字段 <see cref="POINTERINFO.pointerFlags"/> 的值之一或值组合
    /// <para>XBUTTON1 和 XBUTTON2 是许多鼠标设备上使用的其他按钮。它们返回与标准鼠标按钮相同的数据。</para>
    /// <para>注入的输入将发送到运行注入过程的会话的桌面。有用于由以下组合所指示触摸输入注射（交互式和悬停）两个输入状态 pointerFlags ：</para>
    /// <para>INRANGE | UPDATE  Touch 触摸悬停开始或移动</para>
    /// <para>INRANGE | INCONTACT | DOWN    触摸向下</para>
    /// <para>INRANGE | INCONTACT | UPDATE  触摸接触动作</para>
    /// <para>INRANGE | UP  触摸向上并过渡到悬停</para>
    /// <para>UPDATE    触摸悬停结束</para>
    /// <para>UP    触摸结束</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/inputmsg/pointer-flags-contants </para>
    /// </summary>
    [Flags]
    public enum PointerFlag
    {
        /// <summary>
        /// 默认
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// 指示新指针的到达。
        /// </summary>
        NEW = 0x00000001,
        /// <summary>
        /// 指示此指针继续存在。如果未设置此标志，则表明指针已离开检测范围。
        /// <para>此标志通常不设置仅当指针悬停叶检测范围（UPDATE是组），或当在与窗口面叶片的检测范围相接触的指针（UP是集）。</para>
        /// </summary>
        INRANGE = 0x00000002,
        /// <summary>
        /// 指示该指针与数字转换器表面接触。未设置此标志时，表示悬停指针。
        /// </summary>
        INCONTACT = 0x00000004,
        /// <summary>
        /// 指示主要操作，类似于鼠标左键按下。触摸指针与数字化仪表面接触时会设置此标志。
        /// <para>笔指针在未按下任何按钮的情况下与数字化仪表面接触时，会设置此标志。当鼠标左键按下时，鼠标指针将设置此标志。</para>
        /// </summary>
        FIRSTBUTTON = 0x00000010,
        /// <summary>
        /// 指示辅助操作，类似于鼠标右键按下。触摸指针不使用此标志。
        /// <para>当笔筒按钮按下时，笔指针与数字转换器表面接触时会设置此标志。当鼠标右键按下时，鼠标指针会设置此标志。</para>
        /// </summary>
        SECONDBUTTON = 0x00000020,
        /// <summary>
        /// 类似于按下鼠标滚轮的按钮。触摸指针不使用此标志。
        /// <para>笔指针不使用此标志。按下鼠标滚轮按钮时，鼠标指针会设置此标志。</para>
        /// </summary>
        THIRDBUTTON = 0x00000040,
        /// <summary>
        /// 类似于第一个扩展鼠标（XButton1）按下按钮。触摸指针不使用此标志。
        /// <para>笔指针不使用此标志。当第一个扩展鼠标（XBUTTON1）按钮按下时，鼠标指针将设置此标志。</para>
        /// </summary>
        FOURTHBUTTON = 0x00000080,
        /// <summary>
        /// 类似于按下第二个扩展鼠标（XButton2）的按钮。触摸指针不使用此标志。
        /// <para>笔指针不使用此标志。当第二个扩展鼠标（XBUTTON2）按钮按下时，鼠标指针将设置此标志。</para>
        /// </summary>
        FIFTHBUTTON = 0x00000100,
        /// <summary>
        /// 指示该指针已被指定为主指针。主指针是一个单一的指针，它可以执行超出非主指针可用的动作的动作。例如，当主指针与窗口的表面接触时，它可以通过向其发送WM_POINTERACTIVATE消息来为窗口提供激活机会。
        /// <para>根据系统上所有当前用户的交互（鼠标，触摸，笔等）来标识主指针。因此，主指针可能未与您的应用程序关联。多点触摸交互中的第一个联系人被设置为主指针。一旦标识了主要指针，则必须先提起所有联系人，然后才能将新的联系人标识为主要指针。对于不处理指针输入的应用程序，只有主指针的事件被提升为鼠标事件。</para>
        /// </summary>
        PRIMARY = 0x00002000,
        /// <summary>
        /// 置信度是来自源设备的关于指针是表示预期交互还是意外交互的建议，这尤其与PT_TOUCH指针有关，在PT_TOUCH指针中，意外交互（例如用手掌）可以触发输入。此标志的存在指示源设备对该输入是预期交互的一部分具有高置信度。
        /// </summary>
        CONFIDENCE = 0x000004000,
        /// <summary>
        /// 指示指针以异常方式离开，例如，当系统收到该指针的无效输入或具有活动指针的设备突然离开时。如果接收输入的应用程序可以这样做，则应将交互视为未完成，并撤销相关指针的任何影响。
        /// </summary>
        CANCELED = 0x000008000,
        /// <summary>
        /// 指示该指针已转换为向下状态；也就是说，它与数字转换器表面接触。
        /// </summary>
        DOWN = 0x00010000,
        /// <summary>
        /// 表示这是一个简单的更新，不包括指针状态更改。
        /// </summary>
        UPDATE = 0x00020000,
        /// <summary>
        /// 指示该指针已转换为向上状态；也就是说，与数字转换器表面的接触结束了。
        /// </summary>
        UP = 0x00040000,
        /// <summary>
        /// 指示与指针轮相关的输入。对于鼠标指针，这等效于鼠标滚轮（WM_MOUSEHWHEEL）的操作。
        /// </summary>
        WHEEL = 0x00080000,
        /// <summary>
        /// 指示与指针h轮相关联的输入。对于鼠标指针，这等效于鼠标水平滚动轮（WM_MOUSEHWHEEL）的操作。
        /// </summary>
        HWHEEL = 0x00100000,
        /// <summary>
        /// 指示此指针已被另一个元素捕获（关联），并且原始元素丢失了捕获（请参见WM_POINTERCAPTURECHANGED）。
        /// </summary>
        CAPTURECHANGED = 0x00200000,
        /// <summary>
        /// 指示此指针具有关联的转换。
        /// </summary>
        HASTRANSFORM = 0x00400000,
    }

    /// <summary>
    /// <see cref="POINTERINFO"/> 结构体字段 <see cref="POINTERINFO.buttonChangeType"/> 的值之一
    /// <para>标识与指针关联的按钮状态的变化 <see cref="PointerFlag"/></para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ne-winuser-pointer_button_change_type </para>
    /// </summary>
    public enum PointerButtonChangeType
    {
        /// <summary>
        /// 按钮状态无变化。
        /// </summary>
        NONE,
        /// <summary>
        /// 第一个按钮转换为按下状态。
        /// </summary>
        FIRSTBUTTON_DOWN,
        /// <summary>
        /// 第一个按钮转换为释放状态。
        /// </summary>
        FIRSTBUTTON_UP,
        /// <summary>
        /// 第二个按钮转换为按下状态。
        /// </summary>
        SECONDBUTTON_DOWN,
        /// <summary>
        /// 第二个按钮转换为释放状态。
        /// </summary>
        SECONDBUTTON_UP,
        /// <summary>
        /// 第三个按钮转换为按下状态。
        /// </summary>
        THIRDBUTTON_DOWN,
        /// <summary>
        /// 第三个按钮转换为释放状态。
        /// </summary>
        THIRDBUTTON_UP,
        /// <summary>
        /// 第四个按钮转换为按下状态。
        /// </summary>
        FOURTHBUTTON_DOWN,
        /// <summary>
        /// 第四个按钮转换为释放状态。
        /// </summary>
        FOURTHBUTTON_UP,
        /// <summary>
        /// 第五个按钮转换为按下状态。
        /// </summary>
        FIFTHBUTTON_DOWN,
        /// <summary>
        /// 第五个按钮转换为释放状态。
        /// </summary>
        FIFTHBUTTON_UP
    }

    /// <summary>
    /// <see cref="POINTERTOUCHINFO"/> 结构体字段 <see cref="POINTERTOUCHINFO.pointerInfo"/> 的值
    /// <para>包含所有指针类型共有的基本指针信息。应用程序可以使用 GetPointerInfo，GetPointerFrameInfo，GetPointerInfoHistory 和 GetPointerFrameInfoHistory 函数检索此信息。</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-pointer_info </para>
    /// </summary>
    public struct POINTERINFO
    {
        /// <summary>
        /// POINTER_INPUT_TYPE 枚举中的一个值，它指定指针类型。
        /// </summary>
        public PointerInputType pointerType;
        /// <summary>
        /// 一个在其生存期内唯一标识指针的标识符。指针在首次检测到时就存在，而在超出检测范围时结束其存在。请注意，如果某个物理实体（手指或笔）超出了检测范围，然后又再次被检测到，则将其视为新的指针，并可以为其分配新的指针标识符。
        /// </summary>
        public uint pointerId;
        /// <summary>
        /// 源设备在单个输入帧中报告更新的多个指针共有的标识符。例如，并行模式多点触摸数字转换器可以在单次更新中向系统报告多个触摸触点的位置。
        /// </summary>
        public uint frameId;
        /// <summary>
        /// 可以是来自 Pointer Flags 常量的标志的任何合理组合。
        /// </summary>
        public PointerFlag pointerFlags;
        /// <summary>
        /// 处理可用于原始输入设备 API 和数字转换器设备 API 调用的源设备。
        /// </summary>
        public IntPtr sourceDevice;
        /// <summary>
        /// 此消息所针对的窗口。如果通过与该窗口建立联系来隐式捕获指针，或者使用指针捕获API显式地捕获指针，则这就是捕获窗口。如果未捕获指针，则这是生成此消息时指针所在的窗口。
        /// </summary>
        public IntPtr hwndTarget;
        /// <summary>
        /// 指针的预测屏幕坐标，以像素为单位。
        /// </summary>
        public POINT ptPixelLocation;
        /// <summary>
        /// 针的预测屏幕坐标，以 HIMETRIC 单位。
        /// </summary>
        public POINT ptHimetricLocation;
        /// <summary>
        /// 指针的屏幕坐标，以像素为单位。有关调整的屏幕坐标，请参见 ptPixelLocation
        /// </summary>
        public POINT ptPixelLocationRaw;
        /// <summary>
        /// 指针的屏幕坐标，以 HIMETRIC 单位。有关调整的屏幕坐标，请参见 ptHimetricLocation。
        /// </summary>
        public POINT ptHimetricLocationRaw;
        /// <summary>
        /// 0或消息的时间戳，基于收到消息时的系统滴答计数。
        /// </summary>
        public int dwTime;
        /// <summary>
        /// 合并到此消息中的输入计数。此计数与调用GetPointerInfoHistory可以返回的条目总数相匹配。如果未发生合并，则对于消息表示的单个输入，此计数为1。
        /// </summary>
        public uint historyCount;
        /// <summary>
        /// InputData
        /// </summary>
        public int InputData;
        /// <summary>
        /// 指示在生成输入时按下了哪些键盘修饰键。可以为零或以下值的组合。
        /// POINTER_MOD_SHIFT –按下了SHIFT键。
        /// POINTER_MOD_CTRL –按下CTRL键。
        /// </summary>
        public int dwKeyStates;
        /// <summary>
        /// 收到指针消息时的高分辨率性能计数器的值（高精度，64位替代dwTime）。当触摸数字化仪硬件在其输入报告中支持扫描时间戳信息时，可以校准该值。
        /// </summary>
        public long performanceCount;
        /// <summary>
        /// POINTER_BUTTON_CHANGE_TYPE 枚举中的一个值，用于指定此输入与先前输入之间的按钮状态更改。
        /// </summary>
        public PointerButtonChangeType buttonChangeType;
    }

    /// <summary>
    /// <see cref="POINTERTOUCHINFO"/> 结构体字段 <see cref="POINTERTOUCHINFO.touchFlags"/> 的值之一
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/inputmsg/touch-flags-constants </para>
    /// </summary>
    public enum TouchFlag
    {
        /// <summary>
        /// The default value.
        /// </summary>
        NONE = 0x00000000,
    }

    /// <summary>
    /// <see cref="POINTERTOUCHINFO"/> 结构体字段 <see cref="POINTERTOUCHINFO.touchMask"/> 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/inputmsg/touch-mask-constants </para>
    /// </summary>
    [Flags]
    public enum TouchMask
    {
        /// <summary>
        /// 默认。所有可选字段均无效。
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// 关系
        /// </summary>
        CONTACTAREA = 0x00000001,
        /// <summary>
        /// 方向
        /// </summary>
        ORIENTATION = 0x00000002,
        /// <summary>
        /// 压力
        /// </summary>
        PRESSURE = 0x00000004,
    }

    /// <summary>
    /// 指针类型共有的基本触摸信息。
    /// <para><see cref="User32.InjectTouchInput"/> 函数参数 contacts 触摸数据集合</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-pointer_touch_info </para>
    /// </summary>
    public struct POINTERTOUCHINFO
    {
        /// <summary>
        /// 嵌入式 <see cref="POINTERINFO"/> 标头结构。
        /// </summary>
        public POINTERINFO pointerInfo;
        /// <summary>
        /// 目前没有，为 0 。
        /// </summary>
        public TouchFlag touchFlags;
        /// <summary>
        /// 指示哪个可选字段包含有效值。该成员可以是零，也可以是“触摸蒙版”常量的值的任意组合。
        /// </summary>
        public TouchMask touchMask;
        /// <summary>
        /// 接触区域的预测屏幕坐标，以像素为单位。默认情况下，如果设备不报告接触区域，则此字段默认为以指针位置为中心的 0×0 矩形。
        /// <para>预测值基于数字化仪报告的指针位置和指针的运动。该校正可以补偿由于感测和处理数字化仪上的指针位置时固有的延迟而导致的视觉滞后。这适用于 PT_TOUCH 类型的指针。</para>
        /// </summary>
        public RECT rcContact;
        /// <summary>
        /// 接触区域的原始屏幕坐标，以像素为单位。有关调整的屏幕坐标，请参见 rcContact。
        /// </summary>
        public RECT rcContactRaw;
        /// <summary>
        /// 指针方向，其值介于0到359之间，其中 0 表示与 x 轴对齐并从左到右指向的触摸指针；增大的值表示沿顺时针方向的旋转度。
        /// <para>如果设备未报告方向，则此字段默认为 0。</para>
        /// </summary>
        public uint orientation;
        /// <summary>
        /// 笔压标准化为 0 到 1024 之间的范围。如果设备未报告压力，则默认值为 0。
        /// </summary>
        public uint pressure;
    }
    #endregion
    

    /// <summary>
    /// WinUser.h 常用/实用 函数
    /// <para> <see cref="Marshal.GetLastWin32Error()"/> </para>
    /// <para>LPCTSTR，LPWSTR, PTSTR, LPTSTR，L表示long指针，P表示这是一个指针，T表示_T宏,这个宏用来表示你的字符是否使用UNICODE, 如果你的程序定义了UNICODE或者其他相关的宏，那么这个字符或者字符串将被作为UNICODE字符串，否则就是标准的ANSI字符串。C表示是一个常量,const。STR表示这个变量是一个字符串。</para>
    /// <para>参考： https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ </para>
    /// </summary>
    public static partial class User32
    {
        /// <summary>
        /// dll name
        /// </summary>
        public const string DLL_NAME = "user32";

        /// <summary>
        /// 最大同时触摸常量数,Specifies the maximum number of simultaneous contacts.
        /// </summary>
        public const uint MAX_TOUCH_COUNT = 256;

    }
}
