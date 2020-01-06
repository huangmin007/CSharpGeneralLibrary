#pragma warning disable CS1591,CS1572
using SpaceCG.WindowsAPI.WinUser;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SpaceCG.Examples
{
    [Example]
    public class MouseHook:IDisposable
    {
        HookProc HookProc;
        IntPtr HookIntPtr;

        public MouseHook()
        {
            Initizlize();
        }

        protected void Initizlize()
        {
            HookProc = HookProcHandler;
            Process process = Process.GetCurrentProcess();
            ProcessModule processModule = process.MainModule;
            //IntPtr hInstance = Kernel32.GetModuleHandle(processModule.ModuleName);

            HookIntPtr = WinUser.SetWindowsHookEx(HookType.WH_MOUSE_LL, HookProc, processModule.BaseAddress, 0);
        }

        protected IntPtr HookProcHandler(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0)
            {
                MessageType flag = (MessageType)wParam;
                MouseHookStruct mouseData = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                Console.WriteLine("Flag:{0} mouseData:{1}", flag, mouseData);
            }
            return WinUser.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (HookIntPtr != null && HookIntPtr != IntPtr.Zero)
                        WinUser.UnhookWindowsHookEx(HookIntPtr);
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                HookIntPtr = IntPtr.Zero;
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MouseHook() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}