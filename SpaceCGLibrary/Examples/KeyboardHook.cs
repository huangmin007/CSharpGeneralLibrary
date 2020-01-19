﻿#pragma warning disable CS1591,CS1572
using SpaceCG.WindowsAPI.User32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SpaceCG.Examples
{
    /// <summary>
    /// Keyboard Hook
    /// </summary>
    [Example]
    public class KeyboardHook:IDisposable
    {
        HookProc HookProc;
        IntPtr HookIntPtr;

        public event KeyEventHandler KeyUp;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyPress;

        private List<KeyboardLLHookStruct> KeysList;

        public KeyboardHook()
        {
            Initialize();
        }
        
        /// <summary>
        /// initizlize
        /// </summary>
        protected void Initialize()
        {
            HookProc = HookProcHandler;
            Process process = Process.GetCurrentProcess();
            ProcessModule processModule = process.MainModule;
            //IntPtr hInstance = Kernel32.GetModuleHandle(processModule.ModuleName);

            KeysList = new List<KeyboardLLHookStruct>();
            HookIntPtr = User32.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, HookProc, processModule.BaseAddress, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        protected IntPtr HookProcHandler(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0)
            {
                MessageType flag = (MessageType)wParam;
                KeyboardLLHookStruct keyData = (KeyboardLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardLLHookStruct));

                Console.WriteLine("Flag:{0} KeyData:{1}", flag, keyData);


                //按下控制键
                if ((KeyDown != null || KeyPress != null) && (flag == MessageType.WM_KEYDOWN || flag == MessageType.WM_SYSKEYDOWN))
                {
                    if (IsCtrlAltShiftKeys(keyData.vkCode) && KeysList.IndexOf(keyData) == -1)
                    {
                        KeysList.Add(keyData);
                    }
                }
                //松开控制键
                if ((KeyDown != null || KeyPress != null) && (flag == MessageType.WM_KEYUP || flag == MessageType.WM_SYSKEYUP))
                {
                    if (IsCtrlAltShiftKeys(keyData.vkCode))
                    {
                        for (int i = KeysList.Count - 1; i >= 0; i--)
                        {
                            if (KeysList[i].vkCode == keyData.vkCode)     KeysList.RemoveAt(i); 
                        }
                    }
                }

                if (KeyDown != null && (flag == MessageType.WM_KEYDOWN || flag == MessageType.WM_SYSKEYDOWN))
                {
                    KeyEventArgs Args = new KeyEventArgs(null, null, keyData.time, KeyInterop.KeyFromVirtualKey((int)keyData.vkCode));
                    KeyDown(this, Args);
                }

                if (KeyUp != null && (flag == MessageType.WM_KEYUP || flag == MessageType.WM_SYSKEYUP))
                {
                    KeyEventArgs Args = new KeyEventArgs(null, null, keyData.time, KeyInterop.KeyFromVirtualKey((int)keyData.vkCode));
                    KeyUp(this, Args);
                }

                if(KeyPress != null && flag == MessageType.WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    User32.GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];

                    if (User32.ToAscii(keyData.vkCode, (uint)keyData.scanCode, keyState, inBuffer, (uint)keyData.flags) == 1)
                    {
                        Console.WriteLine("KeyChar:{0}", (char)inBuffer[0]);
                        Key k = KeyInterop.KeyFromVirtualKey((int)keyData.vkCode);
                    }
                }
            }

            return User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        
        /// <summary>
        /// 根据已经按下的控制键生成key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Key GetDownKeys()
        {
            byte[] lpkeyState = new byte[256];

            User32.GetKeyboardState(lpkeyState);

            // ba la ba la

            return Key.A;
        }
        

        /// <summary>
        /// 是否按下Ctrl,Alt,Shift等功能键
        /// </summary>
        /// <param name="vkCey"></param>
        /// <returns></returns>
        protected Boolean IsCtrlAltShiftKeys(VirtualKeyCode vkCey)
        {
            if (vkCey == VirtualKeyCode.LCONTROL || vkCey == VirtualKeyCode.RCONTROL || 
                vkCey == VirtualKeyCode.LMENU || vkCey == VirtualKeyCode.RMENU || 
                vkCey == VirtualKeyCode.LSHIFT || vkCey == VirtualKeyCode.RSHIFT)
                return true; 
            return false;
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
                        User32.UnhookWindowsHookEx(HookIntPtr);
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                HookIntPtr = IntPtr.Zero;

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~KeyboardHook() {
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
