using System;
using System.Runtime.InteropServices;

namespace SpaceCG.WindowAPI.WinUser
{
    #region Delegate
    /// <summary>
    /// HOOKPROC 回调函数
    /// <para>与 SetWindowsHookEx 函数一起使用的应用程序定义或库定义的回调函数。调用 SendMessage 函数后，系统将调用此函数。钩子程序可以检查消息；它不能修改它。</para>
    /// <para>所述 HOOKPROC 类型定义一个指向这个回调函数。CallWndRetProc 是应用程序定义或库定义的函数名称的占位符。</para>
    /// <para>应用程序通过在调用 SetWindowsHookEx 函数时指定 WH_CALLWNDPROCRET 挂钩类型和指向该挂钩过程的指针来安装该挂钩过程。</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nc-winuser-hookproc </para>
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam">指定消息是否由当前进程发送。如果消息是由当前进程发送的，则该消息为非零；否则为0。否则为NULL。</param>
    /// <param name="lParam">指向 CWPRETSTRUCT 结构的指针，该结构包含有关消息的详细信息。</param>
    /// <returns>如果 nCode 小于零，则挂钩过程必须返回 CallNextHookEx 返回的值。
    /// <para>如果 nCode 大于或等于零，则强烈建议您调用 CallNextHookEx 并返回它返回的值。否则，其他安装了 WH_CALLWNDPROCRET 挂钩的应用程序将不会收到挂钩通知，因此可能会出现错误的行为。如果挂钩过程未调用 CallNextHookEx，则返回值应为零。</para>
    /// </returns>
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
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
    public enum SwpFlag
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
        /// 同时闪烁窗口标题和任务栏按钮。这等效于设置 FLASHW_CAPTION | FLASHW_TRAY 标志。
        /// </summary>
        ALL = 0x00000003,
        /// <summary>
        /// 刷新窗口标题。
        /// </summary>
        CAPTION = 0x00000001,
        /// <summary>
        /// 停止闪烁。系统将窗口还原到其原始状态。
        /// </summary>
        STOP = 0x00000000,
        /// <summary>
        /// 连续闪烁，直到设置了FLASHW_STOP标志。
        /// </summary>
        TIMER = 0x00000004,
        /// <summary>
        /// 持续闪烁直到窗口到达前台。
        /// </summary>
        TIMERNOFG = 0x0000000C,
        /// <summary>
        /// 刷新任务栏按钮。
        /// </summary>
        TRAY = 0x00000002,
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
    /// The list of VirtualKeyCodes
    /// <para>虚拟键代码的符号/常数/名称，十六进制值以及鼠标或键盘等效项</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/inputdev/virtual-key-codes </para>
    /// </summary>
    public enum VirtualKeyCode
    {
        /// <summary>
        /// Left mouse button
        /// </summary>
        LBUTTON = 0x01,
        /// <summary>
        /// Right mouse button
        /// </summary>
        RBUTTON = 0x02,
        /// <summary>
        /// Control-break processing
        /// </summary>
        CANCEL = 0x03,
        /// <summary>
        /// Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        MBUTTON = 0x04,
        /// <summary>
        /// Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        XBUTTON1 = 0x05,
        /// <summary>
        /// Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        XBUTTON2 = 0x06,

        // 0x07 : Undefined

        /// <summary>
        /// BACKSPACE key
        /// </summary>
        BACK = 0x08,
        /// <summary>
        /// TAB key
        /// </summary>
        TAB = 0x09,

        // 0x0A - 0x0B : Reserved

        /// <summary>
        /// CLEAR key
        /// </summary>
        CLEAR = 0x0C,
        /// <summary>
        /// ENTER key
        /// </summary>
        RETURN = 0x0D,

        // 0x0E - 0x0F : Undefined

        /// <summary>
        /// SHIFT key
        /// </summary>
        SHIFT = 0x10,
        /// <summary>
        /// CTRL key
        /// </summary>
        CONTROL = 0x11,
        /// <summary>
        /// ALT key
        /// </summary>
        MENU = 0x12,
        /// <summary>
        /// PAUSE key
        /// </summary>
        PAUSE = 0x13,
        /// <summary>
        /// CAPS LOCK key
        /// </summary>
        CAPITAL = 0x14,
        /// <summary>
        /// Input Method Editor (IME) Kana mode
        /// </summary>
        KANA = 0x15,
        /// <summary>
        /// IME Hanguel mode (maintained for compatibility; use HANGUL)
        /// </summary>
        HANGEUL = 0x15,
        /// <summary>
        /// IME Hangul mode
        /// </summary>
        HANGUL = 0x15,

        // 0x16 : Undefined

        /// <summary>
        /// IME Junja mode
        /// </summary>
        JUNJA = 0x17,
        /// <summary>
        /// IME final mode
        /// </summary>
        FINAL = 0x18,
        /// <summary>
        /// IME Hanja mode
        /// </summary>
        HANJA = 0x19,
        /// <summary>
        /// IME Kanji mode
        /// </summary>
        KANJI = 0x19,

        // 0x1A : Undefined

        /// <summary>
        /// ESC key
        /// </summary>
        ESCAPE = 0x1B,
        /// <summary>
        /// IME convert
        /// </summary>
        CONVERT = 0x1C,
        /// <summary>
        /// IME nonconvert
        /// </summary>
        NONCONVERT = 0x1D,
        /// <summary>
        /// IME accept
        /// </summary>
        ACCEPT = 0x1E,
        /// <summary>
        /// IME mode change request
        /// </summary>
        MODECHANGE = 0x1F,
        /// <summary>
        /// SPACEBAR
        /// </summary>
        SPACE = 0x20,
        /// <summary>
        /// PAGE UP key
        /// </summary>
        PRIOR = 0x21,
        /// <summary>
        /// PAGE DOWN key
        /// </summary>
        NEXT = 0x22,
        /// <summary>
        /// END key
        /// </summary>
        END = 0x23,
        /// <summary>
        /// HOME key
        /// </summary>
        HOME = 0x24,
        /// <summary>
        /// LEFT ARROW key
        /// </summary>
        LEFT = 0x25,
        /// <summary>
        /// UP ARROW key
        /// </summary>
        UP = 0x26,
        /// <summary>
        /// RIGHT ARROW key
        /// </summary>
        RIGHT = 0x27,
        /// <summary>
        /// DOWN ARROW key
        /// </summary>
        DOWN = 0x28,
        /// <summary>
        /// SELECT key
        /// </summary>
        SELECT = 0x29,
        /// <summary>
        /// PRINT key
        /// </summary>
        PRINT = 0x2A,
        /// <summary>
        /// EXECUTE key
        /// </summary>
        EXECUTE = 0x2B,
        /// <summary>
        /// PRINT SCREEN key
        /// </summary>
        SNAPSHOT = 0x2C,
        /// <summary>
        /// INS key
        /// </summary>
        INSERT = 0x2D,
        /// <summary>
        /// DEL key
        /// </summary>
        DELETE = 0x2E,
        /// <summary>
        /// HELP key
        /// </summary>
        HELP = 0x2F,
        /// <summary>
        /// 0 key
        /// </summary>
        VK_0 = 0x30,
        /// <summary>
        /// 1 key
        /// </summary>
        VK_1 = 0x31,
        /// <summary>
        /// 2 key
        /// </summary>
        VK_2 = 0x32,
        /// <summary>
        /// 3 key
        /// </summary>
        VK_3 = 0x33,
        /// <summary>
        /// 4 key
        /// </summary>
        VK_4 = 0x34,
        /// <summary>
        /// 5 key
        /// </summary>
        VK_5 = 0x35,
        /// <summary>
        /// 6 key
        /// </summary>
        VK_6 = 0x36,
        /// <summary>
        /// 7 key
        /// </summary>
        VK_7 = 0x37,
        /// <summary>
        /// 8 key
        /// </summary>
        VK_8 = 0x38,
        /// <summary>
        /// 9 key
        /// </summary>
        VK_9 = 0x39,

        //
        // 0x3A - 0x40 : Udefined
        //

        /// <summary>
        /// A key
        /// </summary>
        VK_A = 0x41,
        /// <summary>
        /// B key
        /// </summary>
        VK_B = 0x42,
        /// <summary>
        /// C key
        /// </summary>
        VK_C = 0x43,
        /// <summary>
        /// D key
        /// </summary>
        VK_D = 0x44,
        /// <summary>
        /// E key
        /// </summary>
        VK_E = 0x45,
        /// <summary>
        /// F key
        /// </summary>
        VK_F = 0x46,
        /// <summary>
        /// G key
        /// </summary>
        VK_G = 0x47,
        /// <summary>
        /// H key
        /// </summary>
        VK_H = 0x48,
        /// <summary>
        /// I key
        /// </summary>
        VK_I = 0x49,
        /// <summary>
        /// J key
        /// </summary>
        VK_J = 0x4A,
        /// <summary>
        /// K key
        /// </summary>
        VK_K = 0x4B,
        /// <summary>
        /// L key
        /// </summary>
        VK_L = 0x4C,
        /// <summary>
        /// M key
        /// </summary>
        VK_M = 0x4D,
        /// <summary>
        /// N key
        /// </summary>
        VK_N = 0x4E,
        /// <summary>
        /// O key
        /// </summary>
        VK_O = 0x4F,
        /// <summary>
        /// P key
        /// </summary>
        VK_P = 0x50,
        /// <summary>
        /// Q key
        /// </summary>
        VK_Q = 0x51,
        /// <summary>
        /// R key
        /// </summary>
        VK_R = 0x52,
        /// <summary>
        /// S key
        /// </summary>
        VK_S = 0x53,
        /// <summary>
        /// T key
        /// </summary>
        VK_T = 0x54,
        /// <summary>
        /// U key
        /// </summary>
        VK_U = 0x55,
        /// <summary>
        /// V key
        /// </summary>
        VK_V = 0x56,
        /// <summary>
        /// W key
        /// </summary>
        VK_W = 0x57,
        /// <summary>
        /// X key
        /// </summary>
        VK_X = 0x58,

        /// <summary>
        /// Y key
        /// </summary>
        VK_Y = 0x59,

        /// <summary>
        /// Z key
        /// </summary>
        VK_Z = 0x5A,

        /// <summary>
        /// Left Windows key (Microsoft Natural keyboard)
        /// </summary>
        LWIN = 0x5B,

        /// <summary>
        /// Right Windows key (Natural keyboard)
        /// </summary>
        RWIN = 0x5C,

        /// <summary>
        /// Applications key (Natural keyboard)
        /// </summary>
        APPS = 0x5D,

        // 0x5E : reserved

        /// <summary>
        /// Computer Sleep key
        /// </summary>
        SLEEP = 0x5F,
        /// <summary>
        /// Numeric keypad 0 key
        /// </summary>
        NUMPAD0 = 0x60,
        /// <summary>
        /// Numeric keypad 1 key
        /// </summary>
        NUMPAD1 = 0x61,
        /// <summary>
        /// Numeric keypad 2 key
        /// </summary>
        NUMPAD2 = 0x62,
        /// <summary>
        /// Numeric keypad 3 key
        /// </summary>
        NUMPAD3 = 0x63,
        /// <summary>
        /// Numeric keypad 4 key
        /// </summary>
        NUMPAD4 = 0x64,
        /// <summary>
        /// Numeric keypad 5 key
        /// </summary>
        NUMPAD5 = 0x65,
        /// <summary>
        /// Numeric keypad 6 key
        /// </summary>
        NUMPAD6 = 0x66,
        /// <summary>
        /// Numeric keypad 7 key
        /// </summary>
        NUMPAD7 = 0x67,
        /// <summary>
        /// Numeric keypad 8 key
        /// </summary>
        NUMPAD8 = 0x68,
        /// <summary>
        /// Numeric keypad 9 key
        /// </summary>
        NUMPAD9 = 0x69,
        /// <summary>
        /// Multiply key
        /// </summary>
        MULTIPLY = 0x6A,
        /// <summary>
        /// Add key
        /// </summary>
        ADD = 0x6B,
        /// <summary>
        /// Separator key
        /// </summary>
        SEPARATOR = 0x6C,
        /// <summary>
        /// Subtract key
        /// </summary>
        SUBTRACT = 0x6D,
        /// <summary>
        /// Decimal key
        /// </summary>
        DECIMAL = 0x6E,
        /// <summary>
        /// Divide key
        /// </summary>
        DIVIDE = 0x6F,
        /// <summary>
        /// F1 key
        /// </summary>
        F1 = 0x70,
        /// <summary>
        /// F2 key
        /// </summary>
        F2 = 0x71,
        /// <summary>
        /// F3 key
        /// </summary>
        F3 = 0x72,
        /// <summary>
        /// F4 key
        /// </summary>
        F4 = 0x73,
        /// <summary>
        /// F5 key
        /// </summary>
        F5 = 0x74,
        /// <summary>
        /// F6 key
        /// </summary>
        F6 = 0x75,
        /// <summary>
        /// F7 key
        /// </summary>
        F7 = 0x76,
        /// <summary>
        /// F8 key
        /// </summary>
        F8 = 0x77,
        /// <summary>
        /// F9 key
        /// </summary>
        F9 = 0x78,
        /// <summary>
        /// F10 key
        /// </summary>
        F10 = 0x79,
        /// <summary>
        /// F11 key
        /// </summary>
        F11 = 0x7A,
        /// <summary>
        /// F12 key
        /// </summary>
        F12 = 0x7B,
        /// <summary>
        /// F13 key
        /// </summary>
        F13 = 0x7C,
        /// <summary>
        /// F14 key
        /// </summary>
        F14 = 0x7D,
        /// <summary>
        /// F15 key
        /// </summary>
        F15 = 0x7E,
        /// <summary>
        /// F16 key
        /// </summary>
        F16 = 0x7F,
        /// <summary>
        /// F17 key
        /// </summary>
        F17 = 0x80,
        /// <summary>
        /// F18 key
        /// </summary>
        F18 = 0x81,
        /// <summary>
        /// F19 key
        /// </summary>
        F19 = 0x82,
        /// <summary>
        /// F20 key
        /// </summary>
        F20 = 0x83,
        /// <summary>
        /// F21 key
        /// </summary>
        F21 = 0x84,
        /// <summary>
        /// F22 key
        /// </summary>
        F22 = 0x85,
        /// <summary>
        /// F23 key
        /// </summary>
        F23 = 0x86,
        /// <summary>
        /// F24 key
        /// </summary>
        F24 = 0x87,

        //
        // 0x88 - 0x8F : Unassigned
        //

        /// <summary>
        /// NUM LOCK key
        /// </summary>
        NUMLOCK = 0x90,
        /// <summary>
        /// SCROLL LOCK key
        /// </summary>
        SCROLL = 0x91,

        // 0x92 - 0x96 : OEM Specific

        // 0x97 - 0x9F : Unassigned

        //
        // L* & R* - left and right Alt, Ctrl and Shift virtual keys.
        // Used only as parameters to GetAsyncKeyState() and GetKeyState().
        // No other API or message will distinguish left and right keys in this way.
        //

        /// <summary>
        /// Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LSHIFT = 0xA0,
        /// <summary>
        /// Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RSHIFT = 0xA1,
        /// <summary>
        /// Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LCONTROL = 0xA2,
        /// <summary>
        /// Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RCONTROL = 0xA3,
        /// <summary>
        /// Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LMENU = 0xA4,
        /// <summary>
        /// Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RMENU = 0xA5,
        /// <summary>
        /// Windows 2000/XP: Browser Back key
        /// </summary>
        BROWSER_BACK = 0xA6,
        /// <summary>
        /// Windows 2000/XP: Browser Forward key
        /// </summary>
        BROWSER_FORWARD = 0xA7,
        /// <summary>
        /// Windows 2000/XP: Browser Refresh key
        /// </summary>
        BROWSER_REFRESH = 0xA8,
        /// <summary>
        /// Windows 2000/XP: Browser Stop key
        /// </summary>
        BROWSER_STOP = 0xA9,
        /// <summary>
        /// Windows 2000/XP: Browser Search key
        /// </summary>
        BROWSER_SEARCH = 0xAA,
        /// <summary>
        /// Windows 2000/XP: Browser Favorites key
        /// </summary>
        BROWSER_FAVORITES = 0xAB,
        /// <summary>
        /// Windows 2000/XP: Browser Start and Home key
        /// </summary>
        BROWSER_HOME = 0xAC,
        /// <summary>
        /// Windows 2000/XP: Volume Mute key
        /// </summary>
        VOLUME_MUTE = 0xAD,
        /// <summary>
        /// Windows 2000/XP: Volume Down key
        /// </summary>
        VOLUME_DOWN = 0xAE,
        /// <summary>
        /// Windows 2000/XP: Volume Up key
        /// </summary>
        VOLUME_UP = 0xAF,
        /// <summary>
        /// Windows 2000/XP: Next Track key
        /// </summary>
        MEDIA_NEXT_TRACK = 0xB0,
        /// <summary>
        /// Windows 2000/XP: Previous Track key
        /// </summary>
        MEDIA_PREV_TRACK = 0xB1,
        /// <summary>
        /// Windows 2000/XP: Stop Media key
        /// </summary>
        MEDIA_STOP = 0xB2,
        /// <summary>
        /// Windows 2000/XP: Play/Pause Media key
        /// </summary>
        MEDIA_PLAY_PAUSE = 0xB3,
        /// <summary>
        /// Windows 2000/XP: Start Mail key
        /// </summary>
        LAUNCH_MAIL = 0xB4,
        /// <summary>
        /// Windows 2000/XP: Select Media key
        /// </summary>
        LAUNCH_MEDIA_SELECT = 0xB5,
        /// <summary>
        /// Windows 2000/XP: Start Application 1 key
        /// </summary>
        LAUNCH_APP1 = 0xB6,
        /// <summary>
        /// Windows 2000/XP: Start Application 2 key
        /// </summary>
        LAUNCH_APP2 = 0xB7,

        //
        // 0xB8 - 0xB9 : Reserved
        //

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ';:' key 
        /// </summary>
        OEM_1 = 0xBA,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '+' key
        /// </summary>
        OEM_PLUS = 0xBB,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the ',' key
        /// </summary>
        OEM_COMMA = 0xBC,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '-' key
        /// </summary>
        OEM_MINUS = 0xBD,
        /// <summary>
        /// Windows 2000/XP: For any country/region, the '.' key
        /// </summary>
        OEM_PERIOD = 0xBE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '/?' key 
        /// </summary>
        OEM_2 = 0xBF,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '`~' key 
        /// </summary>
        OEM_3 = 0xC0,

        //
        // 0xC1 - 0xD7 : Reserved
        //

        //
        // 0xD8 - 0xDA : Unassigned
        //

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '[{' key
        /// </summary>
        OEM_4 = 0xDB,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '\|' key
        /// </summary>
        OEM_5 = 0xDC,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ']}' key
        /// </summary>
        OEM_6 = 0xDD,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key
        /// </summary>
        OEM_7 = 0xDE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// </summary>
        OEM_8 = 0xDF,

        //
        // 0xE0 : Reserved
        //

        //
        // 0xE1 : OEM Specific
        //

        /// <summary>
        /// Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
        /// </summary>
        OEM_102 = 0xE2,

        //
        // (0xE3-E4) : OEM specific
        //

        /// <summary>
        /// Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
        /// </summary>
        PROCESSKEY = 0xE5,

        //
        // 0xE6 : OEM specific
        //

        /// <summary>
        /// Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        /// </summary>
        PACKET = 0xE7,

        //
        // 0xE8 : Unassigned
        //

        //
        // 0xE9-F5 : OEM specific
        //

        /// <summary>
        /// Attn key
        /// </summary>
        ATTN = 0xF6,
        /// <summary>
        /// CrSel key
        /// </summary>
        CRSEL = 0xF7,
        /// <summary>
        /// ExSel key
        /// </summary>
        EXSEL = 0xF8,
        /// <summary>
        /// Erase EOF key
        /// </summary>
        EREOF = 0xF9,
        /// <summary>
        /// Play key
        /// </summary>
        PLAY = 0xFA,
        /// <summary>
        /// Zoom key
        /// </summary>
        ZOOM = 0xFB,
        /// <summary>
        /// Reserved
        /// </summary>
        NONAME = 0xFC,
        /// <summary>
        /// PA1 key
        /// </summary>
        PA1 = 0xFD,
        /// <summary>
        /// Clear key
        /// </summary>
        OEM_CLEAR = 0xFE,
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
            return $"vkc:{wVk}, wScan:{wScan}, flags:{dwFlags}";
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
            return string.Format("type:{0} input:{1}", type, type == InputType.MOUSE ? Mouse.ToString() : type == InputType.KEYBOARD ? Keyboard.ToString() : Hardware.ToString()) ;
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
    /// ShowWindow 函数参考 nCmdShow 的值之一
    /// </summary>
    public enum SwCmd
    {
        /// <summary>
        /// 最小化一个窗口，即使拥有该窗口的线程没有响应。仅当最小化来自其他线程的窗口时，才应使用此标志。
        /// </summary>
        FORCEMINIMIZE = 11,
        /// <summary>
        /// 隐藏该窗口并激活另一个窗口。
        /// </summary>
        HIDE = 0,
        /// <summary>
        /// 最大化指定的窗口。
        /// </summary>
        MAXIMIZE = 3,
        /// <summary>
        /// 最小化指定的窗口并以Z顺序激活下一个顶级窗口。
        /// </summary>
        MINIMIZE = 6,
        /// <summary>
        /// 激活并显示窗口。如果窗口最小化或最大化，则系统会将其还原到其原始大小和位置。恢复最小化窗口时，应用程序应指定此标志。
        /// </summary>
        RESTORE = 9,
        /// <summary>
        /// 激活窗口并以其当前大小和位置显示它。
        /// </summary>
        SHOW = 5,
        /// <summary>
        /// 根据启动应用程序的程序传递给CreateProcess函数的STARTUPINFO结构中指定的SW_值设置显示状态。
        /// </summary>
        SHOWDEFAULT = 10,
        /// <summary>
        /// 激活窗口并将其显示为最大化窗口。
        /// </summary>
        SHOWMAXIMIZED = 3,
        /// <summary>
        /// 激活窗口并将其显示为最小化窗口。
        /// </summary>
        SHOWMINIMIZED = 2,
        /// <summary>
        /// 将窗口显示为最小化窗口。该值类似于SW_SHOWMINIMIZED，除了未激活窗口。
        /// </summary>
        SHOWMINNOACTIVE = 7,
        /// <summary>
        /// 以当前大小和位置显示窗口。该值与SW_SHOW相似，除了不激活窗口。
        /// </summary>
        SHOWNA = 8,
        /// <summary>
        /// 以最新大小和位置显示窗口。该值类似于SW_SHOWNORMAL，除了未激活窗口。
        /// </summary>
        SHOWNOACTIVATE = 4,
        /// <summary>
        /// 激活并显示一个窗口。如果窗口最小化或最大化，则系统会将其还原到其原始大小和位置。首次显示窗口时，应用程序应指定此标志。
        /// </summary>
        SHOWNORMAL = 1,
    }

    /// <summary>
    /// RegisterHotKey 函数参数 fsModifiers 的值之一或值组合
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
        /// 更改热键行为，以使键盘自动重复操作不会产生多个热键通知。Windows Vista：  不支持此标志。
        /// </summary>
        NOREPEAT = 0x4000,
        /// <summary>
        /// 必须按住SHIFT键。
        /// </summary>
        SHIFT = 0x0004,
        /// <summary>
        /// 按住WINDOWS键。这些键带有Windows徽标。保留与WINDOWS键相关的键盘快捷键，供操作系统使用。
        /// </summary>
        WIN = 0x0008,
    }

    /// <summary>
    /// AnimateWindow 函数参数 dwFlags 的值之一或值组合
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-animatewindow </para>
    /// </summary>
    [Flags]
    public enum AwFlag
    {
        /// <summary>
        /// 激活窗口。不要将此值与 AW_HIDE 一起使用。
        /// </summary>
        ACTIVATE = 0x00020000,
        /// <summary>
        /// 使用淡入淡出效果。仅当 hwnd 是顶级窗口时，才可以使用此标志。
        /// </summary>
        BLEND = 0x00080000,
        /// <summary>
        /// 如果使用 AW_HIDE，则使窗口看起来向内折叠；如果不使用AW_HIDE，则使窗口向外折叠。各种方向标记无效。
        /// </summary>
        CENTER = 0x00000010,
        /// <summary>
        /// 隐藏窗口。默认情况下，显示窗口。
        /// </summary>
        HIDE = 0x00010000,
        /// <summary>
        /// 从左到右对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        HOR_POSITIVE = 0x00000001,
        /// <summary>
        /// 从右到左对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与AW_CENTER或AW_BLEND一起使用时将被忽略。
        /// </summary>
        HOR_NEGATIVE = 0x00000002,
        /// <summary>
        /// 使用幻灯片动画。默认情况下，使用滚动动画。与 AW_CENTER 一起使用时，将忽略此标志。
        /// </summary>
        SLIDE = 0x00040000,
        /// <summary>
        /// 从上到下对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        VER_POSITIVE = 0x00000004,
        /// <summary>
        /// 从底部到顶部对窗口进行动画处理。此标志可与滚动或幻灯片动画一起使用。与 AW_CENTER 或 AW_BLEND 一起使用时将被忽略。
        /// </summary>
        VER_NEGATIVE = 0x00000008,
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
    #endregion


    #region Pointer Touch Info
    /// <summary>
    /// Touch 反馈模式
    /// <para>InitializeTouchInjection 函数参数 dwMode 的值之一或值组合</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/input_touchinjection/constants </para>
    /// </summary>
    [Flags]
    public enum TouchFeedbackMode
    {
        /// <summary>
        /// 指定默认的触摸可视化。最终的用户在 Pen and Touch 控制面板中的设置可能会抑制注入的触摸反馈。
        /// </summary>
        DEFAULT = 0x1,
        /// <summary>
        /// 指定间接触摸可视化。注入的触摸反馈将覆盖“ 笔和触摸”控制面板中的最终用户设置。
        /// </summary>
        INDIRECT = 0x2,
        /// <summary>
        /// 指定没有触摸可视化。TOUCH_FEEDBACK_INDIRECT | TOUCH_FEEDBACK_NONE 应用程序和控件提供的触摸反馈可能不会受到影响。
        /// </summary>
        NONE = 0x3,
    }

    /// <summary>
    /// 指针输入类型。
    /// <para> POINTERINFO 结构体字段 pointerType 的值之一 </para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ne-winuser-tagpointer_input_type </para>
    /// </summary>
    public enum PointerInputType
    {
        /// <summary>
        /// 通用指针类型。此类型永远不会出现在指针消息或指针数据中。一些数据查询功能允许调用者将查询限制为特定的指针类型。所述 PT_POINTER 类型可以在这些功能被用来指定该查询是包括所有类型的指针
        /// </summary>
        POINTER,
        /// <summary>
        /// 触摸指针类型。
        /// </summary>
        TOUCH,
        /// <summary>
        /// 笔指针类型。
        /// </summary>
        PEN,
        /// <summary>
        /// 鼠标指针类型。
        /// </summary>
        MOUSE,
        /// <summary>
        /// 触摸板指针类型（Windows 8.1和更高版本）。
        /// </summary>
        TOUCHPAD
    };

    /// <summary>
    /// POINTERINFO 结构体字段 pointerFlags 的值之一或值组合
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
    /// POINTERINFO 结构体字段 ButtonChangeType 的值之一
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
    /// POINTERTOUCHINFO 结构体字段 pointerInfo 的值
    /// <para>包含所有指针类型共有的基本指针信息。应用程序可以使用 GetPointerInfo，GetPointerFrameInfo，GetPointerInfoHistory 和 GetPointerFrameInfoHistory 函数检索此信息。</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-pointer_info </para>
    /// </summary>
    public struct POINTERINFO
    {
        /// <summary>
        /// POINTER_INPUT_TYPE枚举中的一个值，它指定指针类型。
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
        public long PerformanceCount;
        /// <summary>
        /// POINTER_BUTTON_CHANGE_TYPE 枚举中的一个值，用于指定此输入与先前输入之间的按钮状态更改。
        /// </summary>
        public PointerButtonChangeType ButtonChangeType;
    }

    /// <summary>
    /// POINTERTOUCHINFO 结构体字段 touchFlags 的值之一
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
    /// POINTERTOUCHINFO 结构体字段 touchMask 的值之一或值组合
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
    /// <para>InjectTouchInput 函数参数 contacts 触摸数据集合</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-pointer_touch_info </para>
    /// </summary>
    public struct POINTERTOUCHINFO
    {
        /// <summary>
        /// 嵌入式 POINTERINFO 标头结构。
        /// </summary>
        public POINTERINFO pointerInfo;
        /// <summary>
        /// 目前没有，为 0 。
        /// </summary>
        public TouchFlag touchFlags;
        /// <summary>
        /// 指示哪个可选字段包含有效值。该成员可以是零，也可以是“ 触摸蒙版”常量的值的任意组合。
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


    #region Window Message Struct
    /// <summary>
    /// Window 消息结构体
    /// <para>MSG, *PMSG, *NPMSG, *LPMSG</para>
    /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ns-winuser-msg </para>
    /// </summary>
    public struct MSG
    {
        /// <summary>
        /// 窗口的句柄，其窗口过程接收消息。当消息是线程消息时，此成员为NULL。
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// 消息标识符。应用程序只能使用低位字；高位字由系统保留。
        /// </summary>
        public MsgFlag message;
        /// <summary>
        /// [WPARAM] 有关消息的其他信息。确切含义取决于消息成员的值 。
        /// </summary>
        public int wParam;
        /// <summary>
        /// [LPARAM] 有关消息的其他信息。确切含义取决于消息成员的值 。
        /// </summary>
        public int lParam;
        /// <summary>
        /// 消息发布的时间。
        /// </summary>
        public uint time;
        /// <summary>
        /// 消息发布时的光标位置，以屏幕坐标表示。
        /// </summary>
        public POINT pt;
        /// <summary>
        /// lPrivate
        /// </summary>
        public uint lPrivate;
    }

    /// <summary>
    /// MSG 结构体字段 message 的值之一或值组合
    /// </summary>
    [Flags]
    public enum MsgFlag
    {
        /// <summary>
        ///WM_KEYDOWN 按下一个键
        /// </summary>
        WM_KEYDOWN = 0x0100,
        /// <summary>
        ///释放一个键
        /// </summary>
        WM_KEYUP = 0x0101,
        /// <summary>
        ///按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息
        /// </summary>
        WM_CHAR = 0x102,
        /// <summary>
        ///当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口
        /// </summary>
        WM_DEADCHAR = 0x103,
        /// <summary>
        ///当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSKEYDOWN = 0x104,
        /// <summary>
        ///当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSKEYUP = 0x105,
        /// <summary>
        ///当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSCHAR = 0x106,
        /// <summary>
        ///当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSDEADCHAR = 0x107,
        /// <summary>
        ///在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务
        /// </summary>
        WM_INITDIALOG = 0x110,
        /// <summary>
        ///当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译
        /// </summary>
        WM_COMMAND = 0x111,
        /// <summary>
        ///当用户选择窗口菜单的一条命令或///当用户选择最大化或最小化时那个窗口会收到此消息
        /// </summary>
        WM_SYSCOMMAND = 0x112,
        /// <summary>
        ///发生了定时器事件
        /// </summary>
        WM_TIMER = 0x113,
        /// <summary>
        ///当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件
        /// </summary>
        WM_HSCROLL = 0x114,
        /// <summary>
        ///当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件
        /// </summary>
        WM_VSCROLL = 0x115,
        /// <summary>
        ///当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单
        /// </summary>
        WM_INITMENU = 0x116,
        /// <summary>
        ///当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部
        /// </summary>
        WM_INITMENUPOPUP = 0x117,
        /// <summary>
        ///当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）
        /// </summary>
        WM_MENUSELECT = 0x11F,
        /// <summary>
        ///当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者
        /// </summary>
        WM_MENUCHAR = 0x120,
        /// <summary>
        ///当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待
        /// </summary>
        WM_ENTERIDLE = 0x121,
        /// <summary>
        ///在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色
        /// </summary>
        WM_CTLCOLORMSGBOX = 0x132,
        /// <summary>
        ///当一个编辑型控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色
        /// </summary>
        WM_CTLCOLOREDIT = 0x133,
        /// <summary>
        ///当一个列表框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色
        /// </summary>
        WM_CTLCOLORLISTBOX = 0x134,
        /// <summary>
        ///当一个按钮控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色
        /// </summary>
        WM_CTLCOLORBTN = 0x135,
        /// <summary>
        ///当一个对话框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色
        /// </summary>
        WM_CTLCOLORDLG = 0x136,
        /// <summary>
        ///当一个滚动条控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色
        /// </summary>
        WM_CTLCOLORSCROLLBAR = 0x137,
        /// <summary>
        ///当一个静态控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色
        /// </summary>
        WM_CTLCOLORSTATIC = 0x138,
        /// <summary>
        ///当鼠标轮子转动时发送此消息个当前有焦点的控件
        /// </summary>
        WM_MOUSEWHEEL = 0x20A,
        /// <summary>
        ///双击鼠标中键
        /// </summary>
        WM_MBUTTONDBLCLK = 0x209,
        /// <summary>
        ///释放鼠标中键
        /// </summary>
        WM_MBUTTONUP = 0x208,
        /// <summary>
        ///移动鼠标时发生，同WM_MOUSEFIRST
        /// </summary>
        WM_MOUSEMOVE = 0x200,
        /// <summary>
        ///按下鼠标左键
        /// </summary>
        WM_LBUTTONDOWN = 0x201,
        /// <summary>
        ///释放鼠标左键
        /// </summary>
        WM_LBUTTONUP = 0x202,
        /// <summary>
        ///双击鼠标左键
        /// </summary>
        WM_LBUTTONDBLCLK = 0x203,
        /// <summary>
        ///按下鼠标右键
        /// </summary>
        WM_RBUTTONDOWN = 0x204,
        /// <summary>
        ///释放鼠标右键
        /// </summary>
        WM_RBUTTONUP = 0x205,
        /// <summary>
        ///双击鼠标右键
        /// </summary>
        WM_RBUTTONDBLCLK = 0x206,
        /// <summary>
        ///按下鼠标中键
        /// </summary>
        WM_MBUTTONDOWN = 0x207,

        /// <summary>
        ///
        /// </summary>
        WM_USER = 0x0400,
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
        /// <summary>
        ///创建一个窗口
        /// </summary>
        WM_CREATE = 0x01,
        /// <summary>
        ///当一个窗口被破坏时发送
        /// </summary>
        WM_DESTROY = 0x02,
        /// <summary>
        ///移动一个窗口
        /// </summary>
        WM_MOVE = 0x03,
        /// <summary>
        ///改变一个窗口的大小
        /// </summary>
        WM_SIZE = 0x05,
        /// <summary>
        ///一个窗口被激活或失去激活状态
        /// </summary>
        WM_ACTIVATE = 0x06,
        /// <summary>
        ///一个窗口获得焦点
        /// </summary>
        WM_SETFOCUS = 0x07,
        /// <summary>
        ///一个窗口失去焦点
        /// </summary>
        WM_KILLFOCUS = 0x08,
        /// <summary>
        ///一个窗口改变成Enable状态
        /// </summary>
        WM_ENABLE = 0x0A,
        /// <summary>
        ///设置窗口是否能重画
        /// </summary>
        WM_SETREDRAW = 0x0B,
        /// <summary>
        ///应用程序发送此消息来设置一个窗口的文本
        /// </summary>
        WM_SETTEXT = 0x0C,
        /// <summary>
        ///应用程序发送此消息来复制对应窗口的文本到缓冲区
        /// </summary>
        WM_GETTEXT = 0x0D,
        /// <summary>
        ///得到与一个窗口有关的文本的长度（不包含空字符）
        /// </summary>
        WM_GETTEXTLENGTH = 0x0E,
        /// <summary>
        ///要求一个窗口重画自己
        /// </summary>
        WM_PAINT = 0x0F,
        /// <summary>
        ///当一个窗口或应用程序要关闭时发送一个信号
        /// </summary>
        WM_CLOSE = 0x10,
        /// <summary>
        ///当用户选择结束对话框或程序自己调用ExitWindows函数
        /// </summary>
        WM_QUERYENDSESSION = 0x11,
        /// <summary>
        ///用来结束程序运行
        /// </summary>
        WM_QUIT = 0x12,
        /// <summary>
        ///当用户窗口恢复以前的大小位置时，把此消息发送给某个图标
        /// </summary>
        WM_QUERYOPEN = 0x13,
        /// <summary>
        ///当窗口背景必须被擦除时（例在窗口改变大小时）
        /// </summary>
        WM_ERASEBKGND = 0x14,
        /// <summary>
        ///当系统颜色改变时，发送此消息给所有顶级窗口
        /// </summary>
        WM_SYSCOLORCHANGE = 0x15,
        /// <summary>
        ///当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束
        /// </summary>
        WM_ENDSESSION = 0x16,
        /// <summary>
        ///当隐藏或显示窗口是发送此消息给这个窗口
        /// </summary>
        WM_SHOWWINDOW = 0x18,
        /// <summary>
        ///发此消息给应用程序哪个窗口是激活的，哪个是非激活的
        /// </summary>
        WM_ACTIVATEAPP = 0x1C,
        /// <summary>
        ///当系统的字体资源库变化时发送此消息给所有顶级窗口
        /// </summary>
        WM_FONTCHANGE = 0x1D,
        /// <summary>
        ///当系统的时间变化时发送此消息给所有顶级窗口
        /// </summary>
        WM_TIMECHANGE = 0x1E,
        /// <summary>
        ///发送此消息来取消某种正在进行的摸态（操作）
        /// </summary>
        WM_CANCELMODE = 0x1F,
        /// <summary>
        ///如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口
        /// </summary>
        WM_SETCURSOR = 0x20,
        /// <summary>
        ///当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给///当前窗口
        /// </summary>
        WM_MOUSEACTIVATE = 0x21,
        /// <summary>
        ///发送此消息给MDI子窗口///当用户点击此窗口的标题栏，或///当窗口被激活，移动，改变大小
        /// </summary>
        WM_CHILDACTIVATE = 0x22,
        /// <summary>
        ///此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息
        /// </summary>
        WM_QUEUESYNC = 0x23,
        /// <summary>
        ///此消息发送给窗口当它将要改变大小或位置
        /// </summary>
        WM_GETMINMAXINFO = 0x24,
        /// <summary>
        ///发送给最小化窗口当它图标将要被重画
        /// </summary>
        WM_PAINTICON = 0x26,
        /// <summary>
        ///此消息发送给某个最小化窗口，仅///当它在画图标前它的背景必须被重画
        /// </summary>
        WM_ICONERASEBKGND = 0x27,
        /// <summary>
        ///发送此消息给一个对话框程序去更改焦点位置
        /// </summary>
        WM_NEXTDLGCTL = 0x28,
        /// <summary>
        ///每当打印管理列队增加或减少一条作业时发出此消息
        /// </summary>
        WM_SPOOLERSTATUS = 0x2A,
        /// <summary>
        ///当button，combobox，listbox，menu的可视外观改变时发送
        /// </summary>
        WM_DRAWITEM = 0x2B,
        /// <summary>
        ///当button, combo box, list box, list view control, or menu item 被创建时
        /// </summary>
        WM_MEASUREITEM = 0x2C,
        /// <summary>
        ///此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息
        /// </summary>
        WM_VKEYTOITEM = 0x2E,
        /// <summary>
        ///此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息
        /// </summary>
        WM_CHARTOITEM = 0x2F,
        /// <summary>
        ///当绘制文本时程序发送此消息得到控件要用的颜色
        /// </summary>
        WM_SETFONT = 0x30,
        /// <summary>
        ///应用程序发送此消息得到当前控件绘制文本的字体
        /// </summary>
        WM_GETFONT = 0x31,
        /// <summary>
        ///应用程序发送此消息让一个窗口与一个热键相关连
        /// </summary>
        WM_SETHOTKEY = 0x32,
        /// <summary>
        ///应用程序发送此消息来判断热键与某个窗口是否有关联
        /// </summary>
        WM_GETHOTKEY = 0x33,
        /// <summary>
        ///此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标
        /// </summary>
        WM_QUERYDRAGICON = 0x37,
        /// <summary>
        ///发送此消息来判定combobox或listbox新增加的项的相对位置
        /// </summary>
        WM_COMPAREITEM = 0x39,
        /// <summary>
        ///显示内存已经很少了
        /// </summary>
        WM_COMPACTING = 0x41,
        /// <summary>
        ///发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x46,
        /// <summary>
        ///发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x47,
        /// <summary>
        ///当系统将要进入暂停状态时发送此消息
        /// </summary>
        WM_POWER = 0x48,
        /// <summary>
        ///当一个应用程序传递数据给另一个应用程序时发送此消息
        /// </summary>
        WM_COPYDATA = 0x4A,
        /// <summary>
        ///当某个用户取消程序日志激活状态，提交此消息给程序
        /// </summary>
        WM_CANCELJOURNA = 0x4B,
        /// <summary>
        ///当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口
        /// </summary>
        WM_NOTIFY = 0x4E,
        /// <summary>
        ///当用户选择某种输入语言，或输入语言的热键改变
        /// </summary>
        WM_INPUTLANGCHANGEREQUEST = 0x50,
        /// <summary>
        ///当平台现场已经被改变后发送此消息给受影响的最顶级窗口
        /// </summary>
        WM_INPUTLANGCHANGE = 0x51,
        /// <summary>
        ///当程序已经初始化windows帮助例程时发送此消息给应用程序
        /// </summary>
        WM_TCARD = 0x52,
        /// <summary>
        ///此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就发送给有焦点的窗口，如果///当前都没有焦点，就把此消息发送给///当前激活的窗口
        /// </summary>
        WM_HELP = 0x53,
        /// <summary>
        ///当用户已经登入或退出后发送此消息给所有的窗口，///当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息
        /// </summary>
        WM_USERCHANGED = 0x54,
        /// <summary>
        /// 公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构
        /// </summary>
        WM_NOTIFYFORMAT = 0x55,

        // <summary>
        //当用户某个窗口中点击了一下右键就发送此消息给这个窗口
        // </summary>
        //public static int WM_CONTEXTMENU = ??,

        /// <summary>
        ///当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口
        /// </summary>
        WM_STYLECHANGING = 0x7C,
        /// <summary>
        ///当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口
        /// </summary>
        WM_STYLECHANGED = 0x7D,
        /// <summary>
        ///当显示器的分辨率改变后发送此消息给所有的窗口
        /// </summary>
        WM_DISPLAYCHANGE = 0x7E,
        /// <summary>
        ///此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄
        /// </summary>
        WM_GETICON = 0x7F,
        /// <summary>
        ///程序发送此消息让一个新的大图标或小图标与某个窗口关联
        /// </summary>
        WM_SETICON = 0x80,
        /// <summary>
        ///当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送
        /// </summary>
        WM_NCCREATE = 0x81,
        /// <summary>
        ///此消息通知某个窗口，非客户区正在销毁
        /// </summary>
        WM_NCDESTROY = 0x82,
        /// <summary>
        ///当某个窗口的客户区域必须被核算时发送此消息
        /// </summary>
        WM_NCCALCSIZE = 0x83,
        /// <summary>
        ///移动鼠标，按住或释放鼠标时发生
        /// </summary>
        WM_NCHITTEST = 0x84,
        /// <summary>
        ///程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时
        /// </summary>
        WM_NCPAINT = 0x85,
        /// <summary>
        ///此消息发送给某个窗口仅当它的非客户区需要被改变来显示是激活还是非激活状态
        /// </summary>
        WM_NCACTIVATE = 0x86,
        /// <summary>
        ///发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件通过应
        /// </summary>
        WM_GETDLGCODE = 0x87,
        /// <summary>
        ///当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 非客户区为：窗体的标题栏及窗 的边框体
        /// </summary>
        WM_NCMOUSEMOVE = 0xA0,
        /// <summary>
        ///当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息
        /// </summary>
        WM_NCLBUTTONDOWN = 0xA1,
        /// <summary>
        ///当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        WM_NCLBUTTONUP = 0xA2,
        /// <summary>
        ///当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0xA3,
        /// <summary>
        ///当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCRBUTTONDOWN = 0xA4,
        /// <summary>
        ///当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCRBUTTONUP = 0xA5,
        /// <summary>
        ///当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0xA6,
        /// <summary>
        ///当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONDOWN = 0xA7,
        /// <summary>
        ///当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONUP = 0xA8,
        /// <summary>
        ///当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0xA9,
    }
    #endregion

    /// <summary>
    /// WinUser.h 常用/实用 函数
    /// <para>Marshal.GetLastWin32Error()</para>
    /// <para>LPCTSTR，LPWSTR, PTSTR, LPTSTR，L表示long指针，P表示这是一个指针，T表示_T宏,这个宏用来表示你的字符是否使用UNICODE, 如果你的程序定义了UNICODE或者其他相关的宏，那么这个字符或者字符串将被作为UNICODE字符串，否则就是标准的ANSI字符串。C表示是一个常量,const。STR表示这个变量是一个字符串。</para>
    /// <para>参考： https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/ </para>
    /// </summary>
    public static partial class WinUser
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
