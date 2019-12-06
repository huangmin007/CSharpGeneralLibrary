using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace SpaceCG.WindowsAPI.Kernel32
{
    /// <summary>
    /// Kernel32.h 常用/实用 函数
    /// <para>Marshal.GetLastWin32Error(), new WindowInteropHelper(Window).Handle</para>
    /// </summary>
    public static partial class Kernel32
    {
        /// <summary>
        /// 获取系统时间
        /// <para>这只是一个示例，传参类型为 Class 引用</para>
        /// <para> SystemTimeClass 与 SystemTimeStruct 隐式转换 </para>
        /// </summary>
        /// <param name="systemTime"></param>
        [DllImport(DLL_NAME)]
        public static extern void GetSystemTime(SystemTimeClass systemTime);
        /// <summary>
        /// 获取系统时间
        /// <para>这只是一个示例，传参类型为 Struct 按值传，引用传需加 ref </para>
        /// <para> SystemTimeClass 与 SystemTimeStruct 隐式转换 </para>
        /// </summary>
        /// <param name="systemTime"></param>
        [DllImport(DLL_NAME)]
        public static extern void GetSystemTime(ref SystemTimeStruct systemTime);

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
        [DllImport(DLL_NAME, EntryPoint = "LoadLibrary", SetLastError = true)]
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

        /// <summary>
        /// 检索指定模块的模块句柄。该模块必须已由调用进程加载。
        /// <para>返回的句柄不是全局的或可继承的。它不能被其他进程复制或使用。</para>
        /// <para>如果 lpModuleName 不包含路径，并且有多个加载的模块具有相同的基本名称和扩展名，则您无法预测将返回哪个模块句柄。要变通解决此问题，您可以指定路径，使用并行程序集或使用 GetModuleHandleEx 来指定内存位置而不是 DLL 名称。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea </para>
        /// </summary>
        /// <param name="lpModuleName">[LPCSTR] 加载的模块的名称（.dll或.exe文件）。如果省略文件扩展名，则会附加默认库扩展名.dll。</param>
        /// <returns>如果函数成功，则返回值是指定模块的句柄。如果函数失败，则返回值为NULL。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, EntryPoint = "GetModuleHandle", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);


        /// <summary>
        /// 创建或打开文件或 I/O 设备。最常用的 I/O 设备如下：文件，文件流，目录，物理磁盘，卷，控制台缓冲区，磁带机，通信资源，邮筒和管道。该函数返回一个句柄，根据文件或设备以及指定的标志和属性，该句柄可用于访问各种类型的 I/O 的文件或设备。
        /// <para>要将此操作作为事务处理操作执行，从而产生可用于事务处理 I/O 的句柄，请使用 CreateFileTransacted 函数。</para>
        /// <para>CreateFile 最初是专门为文件交互而开发的，但此后已进行了扩展和增强，以包括 Windows 开发人员可用的大多数其他类型的 I/O 设备和机制。</para>
        /// <para>使用 CreateFile 返回的对象句柄完成应用程序后 ，请使用 CloseHandle 函数关闭该句柄。这不仅释放了系统资源，而且还对共享文件或设备以及将数据提交到磁盘等事物产生更大的影响。</para>
        /// <para>参考：https://docs.microsoft.com/zh-cn/windows/win32/api/fileapi/nf-fileapi-createfilea </para>
        /// </summary>
        /// <param name="lpFileName">[LPCSTR] 要创建或打开的文件或设备的名称。您可以在此名称中使用正斜杠（/）或反斜杠（）。</param>
        /// <param name="dwDesiredAccess">所请求的对文件或设备的访问，可以概括为读，写，两者均为零或都不为零
        ///     <para>如果此参数为零，即使将拒绝GENERIC_READ访问，应用程序也可以查询某些元数据，例如文件，目录或设备属性，而无需访问该文件或设备。</para>
        /// </param>
        /// <param name="dwShareMode">文件或设备的请求共享模式，可以读取，写入，两者，删除，全部或全部不共享。对属性或扩展属性的访问请求不受此标志的影响。
        ///     <para>如果此参数为零，并且 CreateFile 成功，则在关闭文件或设备的句柄之前，无法共享文件或设备，也无法再次打开该文件或设备。</para>
        /// </param>
        /// <param name="lpSecurityAttributes">指向SECURITY_ATTRIBUTES 结构的指针，该结构包含两个单独但相关的数据成员：一个可选的安全描述符，以及一个布尔值，该值确定子进程是否可以继承返回的句柄。此参数可以为NULL。
        ///     <para>如果此参数为 NULL，则 CreateFile 返回的句柄 不能被应用程序可能创建的任何子进程继承，并且与返回的句柄关联的文件或设备将获得默认的安全描述符。</para>
        /// </param>
        /// <param name="dwCreationDisposition">对存在或不存在的文件或设备执行的操作。对于文件以外的设备，此参数通常设置为OPEN_EXISTING。</param>
        /// <param name="dwFlagsAndAttributes">文件或设备属性和标志，FILE_ATTRIBUTE_NORMAL是文件的最常见默认值。此参数可以包括可用文件属性（FILE_ATTRIBUTE_*）的任意组合。所有其他文件属性将覆盖 FILE_ATTRIBUTE_NORMAL。
        ///     <para>此参数还可以包含标志的组合（FILE_FLAG_），用于控制文件或设备的缓存行为，访问模式和其他特殊用途的标志。这些与任何 FILE_ATTRIBUTE_值组合。</para>
        ///     <para>通过指定SECURITY_SQOS_PRESENT标志，此参数还可以包含安全服务质量（SQOS）信息 。在属性和标志表之后的表中提供了其他与SQOS相关的标志信息。</para>
        /// </param>
        /// <param name="hTemplateFile">具有 GENERIC_READ 访问权限的模板文件的有效句柄。模板文件为正在创建的文件提供文件属性和扩展属性。
        ///     <para>此参数可以为 NULL。打开现有文件时，CreateFile 会忽略此参数。当打开一个新的加密文件时，该文件会从其父目录继承任意访问控制列表。</para>
        /// </param>
        /// <returns>如果函数成功，则返回值是指定文件，设备，命名管道或邮件插槽的打开句柄。
        ///     <para>如果函数失败，则返回值为 INVALID_HANDLE_VALUE。要获取扩展的错误信息，请调用 GetLastError。</para>
        /// </returns>
        [DllImport(DLL_NAME, EntryPoint = "CreateFile", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, SECURITY_ATTRIBUTES lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="dwShareMode"></param>
        /// <param name="lpSecurityAttributes"></param>
        /// <param name="dwCreationDisposition"></param>
        /// <param name="dwFlagsAndAttributes"></param>
        /// <param name="hTemplateFile"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME, EntryPoint = "CreateFile", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,  uint dwFlagsAndAttributes, IntPtr hTemplateFile);
#if Test

        public static extern void ReadFile();

        public static extern void WriteFile();

        /// <summary>
        /// 关闭打开的对象句柄。
        /// <para>如果应用程序在调试器下运行，则该函数将收到无效的句柄值或伪句柄值，否则将引发异常。如果您两次关闭一个句柄，或者在 FindFirstFile 函数返回的句柄上 调用 CloseHandle 而不是调用 FindClose 函数，则会发生这种情况。</para>
        /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle </para>
        /// </summary>
        /// <param name="hObject">打开对象的有效句柄。</param>
        /// <returns>如果函数成功，则返回值为非零。如果函数失败，则返回值为零。要获取扩展的错误信息，请调用 GetLastError。</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport(DLL_NAME, EntryPoint = "DeleteFile", SetLastError = true)]
        public static extern bool DeleteFile(string lpFileName);
#endif
    }
}
