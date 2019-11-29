using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowAPI.Kernel32
{
    /// <summary>
    /// Kernel32.h 常用/实用 函数
    /// </summary>
    public static partial class Kernel32
    {
        /// <summary>
        /// dll name
        /// </summary>
        public const string DLL_NAME = "kernel32";

        /// <summary>
        /// 将指定的模块加载到调用进程的地址空间中。指定的模块可能会导致其他模块被加载。
        /// <para>有关其他加载选项，请使用 LoadLibraryEx 函数。</para>
        /// <para>若要启用或禁用 DLL 加载期间加载程序显示的错误消息，请使用 SetErrorMode 函数。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibrarya </para>
        /// </summary>
        /// <param name="lpLibFileName">[LPCSTR] 模块的名称。这可以是库模块（.dll文件）或可执行模块（.exe文件）。指定的名称是模块的文件名，并且与模块模块定义（.def）文件中的 LIBRARY 关键字指定的库模块本身中存储的名称无关。
        ///     <para>如果字符串指定完整路径，则该函数仅在该路径中搜索模块。如果字符串指定相对路径或不带路径的模块名称，则该函数使用标准搜索策略查找模块；有关更多信息，请参见备注。</para>
        ///     <para>如果该功能找不到模块，则该功能将失败。指定路径时，请确保使用反斜杠（\），而不使用正斜杠（/）。有关路径的更多信息，请参见 命名文件或目录。</para>
        ///     <para>如果字符串指定没有路径的模块名称，并且省略了文件扩展名，则该函数会将默认库扩展名.dll附加到模块名。为防止函数将.dll附加到模块名称，请在模块名称字符串中包含尾随字符（。）。</para>
        /// </param>
        /// <returns>如果函数成功，则返回值是模块的句柄。如果函数失败，则返回值为 NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "LoadLibraryA", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        /// <summary>
        /// 释放已加载的动态链接库（DLL）模块，并在必要时减少其引用计数。当引用计数达到零时，将从调用进程的地址空间中卸载模块，并且该句柄不再有效。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/libloaderapi/nf-libloaderapi-freelibrary </para>
        /// </summary>
        /// <param name="hLibModule">加载的库模块的句柄。在 调用 LoadLibrary，LoadLibraryEx，GetModuleHandle 或 GetModuleHandleEx 函数返回该句柄。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。若要获取扩展的错误信息，请调用 GetLastError 函数。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hLibModule);

        
        /// <summary>
        /// 从指定的动态链接库（DLL）检索导出的函数或变量的地址。
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress </para>
        /// </summary>
        /// <param name="hModule">包含函数或变量的 DLL 模块的句柄。在调用LoadLibrary，LoadLibraryEx，LoadPackagedLibrary，或 GetModuleHandle 函数返回该句柄。</param>
        /// <param name="lpProcName">[LPCSTR] 函数或变量名称，或函数的序数值。如果此参数是序数值，则必须使用低位字；高阶字必须为零。</param>
        /// <returns>如果函数成功，则返回值是导出的函数或变量的地址。如果函数失败，则返回值为NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);


        /// <summary>
        /// 检索调用线程的线程标识符。
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthreadid </para>
        /// </summary>
        /// <returns>返回值是调用线程的线程标识符。</returns>
        [DllImport(DLL_NAME)]
        public static extern int GetCurrentThreadId();
    }
}
