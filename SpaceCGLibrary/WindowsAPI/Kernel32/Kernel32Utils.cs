using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowsAPI.Kernel32
{
    /// <summary>
    /// Kernel32 通用/实用 函数集
    /// </summary>
    public static partial class Kernel32Utils
    {
        /// <summary>
        /// 获取系统错误信息的描述
        /// <para>封装 <see cref="Kernel32.FormatMessage(FmFlag, IntPtr, uint, uint, ref string, uint, IntPtr)"/></para>
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static string GetSysErrroMessage(uint errorCode)
        {
#if Two
            string message = null;
            FmFlag flags = FmFlag.FORMAT_MESSAGE_ALLOCATE_BUFFER | FmFlag.FORMAT_MESSAGE_IGNORE_INSERTS | FmFlag.FORMAT_MESSAGE_FROM_SYSTEM;
            Kernel32.FormatMessage(flags, IntPtr.Zero, errorCode, 0, ref message, 255, IntPtr.Zero);
#else
            StringBuilder message = new StringBuilder(255);
            FmFlag flags = FmFlag.FORMAT_MESSAGE_IGNORE_INSERTS | FmFlag.FORMAT_MESSAGE_FROM_SYSTEM;
            Kernel32.FormatMessage(flags, IntPtr.Zero, errorCode, 0, message, 255, IntPtr.Zero);
#endif

            return message.ToString().Trim();
        }

    }
}
