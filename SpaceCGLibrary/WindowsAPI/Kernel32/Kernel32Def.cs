#pragma warning disable CS1591,CS1572
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowsAPI.Kernel32
{
    /// <summary>
    /// 系统时间类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class SystemTimeClass
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milsecond;

        /// <summary>
        /// 隐式转换类型 publi static implicit operator Construct()
        /// </summary>
        /// <param name="year"></param>
        public static implicit operator SystemTimeClass(ushort year)
        {
            return new SystemTimeClass() { Year = year };
        }

        /// <summary>
        /// 隐式转换类型
        /// </summary>
        /// <param name="st"></param>
        public static implicit operator SystemTimeClass(SystemTimeStruct st)
        {
            return new SystemTimeClass()
            {
                Year = st.Year,
                Month = st.Month,
                DayOfWeek = st.DayOfWeek,
                Day = st.Day,
                Hour = st.Hour,
                Minute = st.Minute,
                Second = st.Second,
                Milsecond = st.Milsecond,
            };
        }


        public override string ToString()
        {
            return $"SystemTimeClass {Year} {Month} {Day} {DayOfWeek} {Hour} {Minute} {Second} {Milsecond}";
        }
    }

    /// <summary>
    /// 系统时间结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTimeStruct
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milsecond;

        /// <summary>
        /// 隐式转换类型
        /// </summary>
        /// <param name="st"></param>
        public static implicit operator SystemTimeStruct(SystemTimeClass st)
        {
            return new SystemTimeStruct()
            {
                Year = st.Year,
                Month = st.Month,
                DayOfWeek = st.DayOfWeek,
                Day = st.Day,
                Hour = st.Hour,
                Minute = st.Minute,
                Second = st.Second,
                Milsecond = st.Milsecond,
            };
        }

        public override string ToString()
        {
            return $"SystemTimeStruct {Year} {Month} {Day} {DayOfWeek} {Hour} {Minute} {Second} {Milsecond}";
        }
    }

    /// <summary>
    /// <see cref="Kernel32.FormatMessage(FmFlag, IntPtr, uint, uint, ref string, uint, IntPtr)"/> 函数参数 dwFlags 一个或多个值。
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-formatmessage </para>
    /// </summary>
    [Flags]
    public enum FmFlag
    {
        /// <summary>
        /// 没有输出线宽限制。该函数将消息定义文本中的换行符存储到输出缓冲区中。
        /// </summary>
        FORMAT_MESSAGE_NONE = 0x00000000,
        /// <summary>
        /// 该函数分配一个足以容纳格式化消息的缓冲区，并在 lpBuffer 指定的地址处放置一个指向已分配缓冲区的指针 的 lpBuffer 参数是一个指向 LPTSTR ; 您必须将指针强制转换为 LPTSTR。所述 n 大小 参数指定的最小数目 TCHARS 分配用于输出消息缓冲器。 
        /// <para>当不再需要缓冲区时，调用者应使用 LocalFree 函数释放缓冲区。如果格式化消息的长度超过 128K 字节，则 FormatMessage 将失败，随后对 GetLastError 的调用 将返回 ERROR_MORE_DATA。</para>
        /// </summary>
        FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
        /// <summary>
        /// 消息定义中的插入序列将被忽略，并原样传递到输出缓冲区。该标志对于获取消息以供以后格式化很有用。如果设置了此标志，则 Arguments 参数将被忽略。
        /// </summary>
        FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
        /// <summary>
        /// 所述 lpSource 参数是一个指向包含一个消息定义一个空终止字符串。消息定义可以包含插入序列，就像消息表资源中的消息文本可能一样。该标志不能与 FORMAT_MESSAGE_FROM_HMODULE 或 FORMAT_MESSAGE_FROM_SYSTEM 一起使用 。
        /// </summary>
        FORMAT_MESSAGE_FROM_STRING = 0x00000400,
        /// <summary>
        /// 所述 lpSource 参数是包含该消息表资源（多个），以搜寻模块句柄。如果此 lpSource 句柄为 NULL，则将搜索当前进程的应用程序映像文件。该标志不能与 FORMAT_MESSAGE_FROM_STRING 一起使用 。
        /// <para>如果模块没有消息表资源，则该函数将失败，并显示 ERROR_RESOURCE_TYPE_NOT_FOUND。</para>
        /// </summary>
        FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
        /// <summary>
        /// 该功能应在系统消息表资源中搜索所请求的消息。如果使用 FORMAT_MESSAGE_FROM_HMODULE 指定了此标志，则如果在 lpSource 指定的模块中找不到消息，该函数将搜索系统消息表。该标志不能与 FORMAT_MESSAGE_FROM_STRING 一起使用。
        /// <para>如果指定了此标志，则应用程序可以传递 GetLastError 函数的结果 以检索系统定义的错误的消息文本。</para>
        /// </summary>
        FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
        /// <summary>
        /// 的参数参数不是 va_list 的 结构，但它是一个指针，它指向表示参数值的数组。
        /// <para>该标志不能与 64 位整数值一起使用。如果使用的是64位整数，则必须使用 va_list 结构。</para>
        /// </summary>
        FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
        /// <summary>
        /// 该函数将忽略消息定义文本中的常规换行符。该函数将消息定义文本中的硬编码换行符存储到输出缓冲区中。该函数不会产生新的换行符。
        /// </summary>
        FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF,
    }

    /// <summary>
    /// SECURITY_ATTRIBUTES 结构包含安全描述符的对象，并指定通过指定该结构中检索所述手柄是否是继承。此结构为由各种功能（例如 CreateFile，CreatePipe，CreateProcess，RegCreateKeyEx 或 RegSaveKeyEx）创建的对象提供安全设置。
    /// SECURITY_ATTRIBUTES, *PSECURITY_ATTRIBUTES, *LPSECURITY_ATTRIBUTES;
    /// <para>参考：https://docs.microsoft.com/zh-cn/previous-versions/windows/desktop/legacy/aa379560(v=vs.85) </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        /// <summary>
        /// 此结构的大小（以字节为单位）。将此值设置为 SECURITY_ATTRIBUTES 结构的大小。
        /// </summary>
        public int nLength;
        /// <summary>
        /// 指向 SECURITY_DESCRIPTOR 结构的指针，该结构控制对对象的访问。如果此成员的值为 NULL，则为对象分配与调用过程的访问令牌关联的默认安全描述符。
        /// <para>这与通过分配 NULL 自由访问控制列表（DACL）向所有人授予访问权限不同。默认情况下，进程的访问令牌中的默认 DACL 只允许访问该访问令牌所代表的用户。</para>
        /// </summary>
        [MarshalAs(UnmanagedType.AsAny)]
        public IntPtr lpSecurityDescriptor;
        /// <summary>
        /// 一个布尔值，它指定在创建新进程时是否继承返回的句柄。如果此成员为 TRUE，则新进程将继承该句柄。
        /// </summary>
        public bool bInheritHandle;
        /// <summary>
        /// Create new SECURITY_ATTRIBUTES
        /// </summary>
        /// <returns></returns>
        public static SECURITY_ATTRIBUTES Create()
        {
            //return new SECURITY_ATTRIBUTES() { nLength = Marshal.SizeOf<SECURITY_ATTRIBUTES>() };
            return new SECURITY_ATTRIBUTES() { nLength = Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES)) };
        }

        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nLength}, {lpSecurityDescriptor}, {bInheritHandle}";
        }
    }

    /// <summary>
    /// 访问权限，安全对象使用访问掩码格式，其中四个高位指定通用访问权限。每种可保护对象的类型都将这些位映射到一组其标准和特定于对象的访问权限。
    /// </summary>
    [Flags]
    public enum AccessRights : long
    {
        /// <summary>
        /// [文件访问权限常量] 对于文件对象，有权读取相应的文件数据。对于目录对象，有权读取相应的目录数据。
        /// </summary>
        FILE_READ_DATA = 0x0001,
        /// <summary>
        /// [文件访问权限常量] 对于目录，有权列出目录的内容。
        /// </summary>
        FILE_LIST_DIRECTORY = 0x0001,
        /// <summary>
        /// [文件访问权限常量] 对于文件对象，具有将数据写入文件的权利。对于目录对象，有权在目录（FILE_ADD_FILE）中创建文件。
        /// </summary>
        FILE_WRITE_DATA = 0x0002,
        /// <summary>
        /// [文件访问权限常量] 对于目录，有权在目录中创建文件。
        /// </summary>
        FILE_ADD_FILE = 0x0002,
        /// <summary>
        /// [文件访问权限常量] 对于文件对象，有权将数据附加到文件。（对于本地文件，如果在没有FILE_WRITE_DATA的情况下指定了此标志，则写操作将不会覆盖现有数据。）对于目录对象，有权创建子目录（FILE_ADD_SUBDIRECTORY）。
        /// </summary>
        FILE_APPEND_DATA = 0x0004,
        /// <summary>
        /// [文件访问权限常量] 对于目录，有权创建子目录。
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0x0004,
        /// <summary>
        /// [文件访问权限常量] 对于命名管道，有权创建管道。
        /// </summary>
        FILE_CREATE_PIPE_INSTANCE = 0x0004,
        /// <summary>
        /// [文件访问权限常量] 读取扩展文件属性的权利。
        /// </summary>
        FILE_READ_EA = 0x0008,
        /// <summary>
        /// [文件访问权限常量] 写入扩展文件属性的权利。
        /// </summary>
        FILE_WRITE_EA = 0x0010,
        /// <summary>
        /// [文件访问权限常量] 对于本机代码文件，有权执行该文件。授予脚本的访问权限可能导致脚本可执行，具体取决于脚本解释器。
        /// </summary>
        FILE_EXECUTE = 0x0020,
        /// <summary>
        /// [文件访问权限常量] 对于目录，具有遍历目录的权利。默认情况下，为用户分配BYPASS_TRAVERSE_CHECKING 特权，该特权将忽略FILE_TRAVERSE 访问权限。
        /// </summary>
        FILE_TRAVERSE = 0x0020,
        /// <summary>
        /// [文件访问权限常量] 对于目录，有权删除目录及其包含的所有文件，包括只读文件。
        /// </summary>
        FILE_DELETE_CHILD = 0x0040,
        /// <summary>
        /// [文件访问权限常量] 读取文件属性的权利。
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x0080,
        /// <summary>
        /// [文件访问权限常量] 写入文件属性的权利。
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x0100,
        /// <summary>
        /// [文件访问权限常量] 文件的所有可能的访问权限。
        /// </summary>
        FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF,

        /// <summary>
        /// [一般访问权限] 
        /// </summary>
        FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE,
        /// <summary>
        /// [一般访问权限]
        /// </summary>
        FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE,
        /// <summary>
        /// [一般访问权限]
        /// </summary>
        FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE,



        /// <summary>
        /// [标准访问权限] 删除对象的权利。
        /// </summary>
        DELETE = 0x00010000L,
        /// <summary>
        /// [标准访问权限] 有权读取对象的安全描述符中的信息，但不包括系统访问控制列表（SACL）中的信息。
        /// </summary>
        READ_CONTROL = 0x00020000L,
        /// <summary>
        /// [标准访问权限] 修改对象的安全描述符中的任意访问控制列表（DACL）的权限。
        /// </summary>
        WRITE_DAC = 0x00040000L,
        /// <summary>
        /// [标准访问权限] 在对象的安全描述符中更改所有者的权利。
        /// </summary>
        WRITE_OWNER = 0x00080000L,
        /// <summary>
        /// [标准访问权限] 使用对象进行同步的权利。这使线程可以等待，直到对象处于信号状态。某些对象类型不支持此访问权限。
        /// </summary>
        SYNCHRONIZE = 0x00100000L,
        /// <summary>
        /// [标准访问权限] 合并 DELETE，READ_CONTROL，WRITE_DAC 和 WRITE_OWNER 访问。
        /// </summary>
        STANDARD_RIGHTS_REQUIRED = 0x000F0000L,
        /// <summary>
        /// [标准访问权限] 当前定义为等于 READ_CONTROL。它是读取文件或目录对象的安全描述符中的信息的权利。这不包括 SACL 中的信息。
        /// </summary>
        STANDARD_RIGHTS_READ = READ_CONTROL,
        /// <summary>
        /// [标准访问权限] 当前定义为等于 READ_CONTROL。与 STANDARD_RIGHTS_READ 相同。
        /// </summary>
        STANDARD_RIGHTS_WRITE = READ_CONTROL,
        /// <summary>
        /// [标准访问权限] 当前定义为等于 READ_CONTROL。
        /// </summary>
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,
        /// <summary>
        /// [标准访问权限] 合并 DELETE，READ_CONTROL，WRITE_DAC，WRITE_OWNER 和 SYNCHRONIZE访问。
        /// </summary>
        STANDARD_RIGHTS_ALL = 0x001F0000L,

        /// <summary>
        /// 
        /// </summary>
        SPECIFIC_RIGHTS_ALL = 0x0000FFFFL,


        /// <summary>
        /// AccessSystemAcl access type
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000L,
        /// <summary>
        /// MaximumAllowed access type
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000L,





        /// <summary>
        /// [通用访问权限] 所有可能的访问权限
        /// </summary>
        GENERIC_ALL = 0x10000000L,
        /// <summary>
        /// [通用访问权限] 执行访问 
        /// </summary>
        GENERIC_EXECUTE = 0x20000000L,
        /// <summary>
        /// [通用访问权限] 写访问
        /// </summary>
        GENERIC_WRITE = 0x40000000L,
        /// <summary>
        /// [通用访问权限] 读取权限 
        /// </summary>
        GENERIC_READ = 0x80000000L,

    }

    /// <summary>
    /// CreateFile 函数参数 dwCreationDisposition 的值之一
    /// </summary>
    public enum CreationDisposition
    {
        /// <summary>
        /// 仅在尚不存在时创建一个新文件。如果指定的文件存在，则函数失败，并且上一个错误代码设置为 ERROR_FILE_EXISTS（80）。
        /// <para>如果指定的文件不存在，并且是可写位置的有效路径，则会创建一个新文件。</para>
        /// </summary>
        CREATE_NEW = 1,
        /// <summary>
        /// 始终创建一个新文件。如果指定的文件存在并且可写，则该函数将覆盖该文件，该函数将成功，并且上一个错误代码设置为 ERROR_ALREADY_EXISTS（183）。
        /// <para>如果指定的文件不存在并且是有效路径，则创建一个新文件，该函数成功，并且最后一个错误代码设置为零。</para>
        /// </summary>
        CREATE_ALWAYS = 2,
        /// <summary>
        /// 仅打开文件或设备（如果存在）。如果指定的文件或设备不存在，该功能将失败，并且上一个错误代码将设置为 ERROR_FILE_NOT_FOUND（2）。
        /// </summary>
        OPEN_EXISTING = 3,
        /// <summary>
        /// 始终打开文件。如果指定的文件存在，则函数成功，并且最后一个错误代码设置为 ERROR_ALREADY_EXISTS（183）。
        /// <para>如果指定的文件不存在，并且是可写位置的有效路径，则该函数将创建一个文件，并且上一个错误代码将设置为零。</para>
        /// </summary>
        OPEN_ALWAYS = 4,
        /// <summary>
        /// 打开文件并将其截断，以使其大小为零字节（仅存在时）。如果指定的文件不存在，该函数将失败，并且上一个错误代码将设置为 ERROR_FILE_NOT_FOUND（2）。
        /// <para>调用进程必须使用 dwDesiredAccess 参数的一部分设置 GENERIC_WRITE 位来打开文件。</para>
        /// </summary>
        TRUNCATE_EXISTING = 5,
    }

    public static partial class Kernel32
    {
        /// <summary>
        /// dll name
        /// </summary>
        public const string DLL_NAME = "kernel32";
    }
}
