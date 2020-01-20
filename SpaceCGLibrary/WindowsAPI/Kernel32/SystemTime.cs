using System;
using System.Runtime.InteropServices;

/***
 * 
 * 
 * 
 * 
 * 
**/

namespace SpaceCG.WindowsAPI.Kernel32
{

    #region Enumerations
    #endregion


    #region Structures
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
    #endregion


    #region Deletages
    #endregion


    #region Notifications
    #endregion


    /// <summary>
    /// 
    /// </summary>
    public static partial class Kernel32
    {
        /// <summary>
        /// 获取系统时间
        /// <para>这只是一个示例，传参类型为 Class 引用</para>
        /// <para> <see cref="SystemTimeClass"/> 与 <see cref="SystemTimeStruct"/> 隐式转换 </para>
        /// </summary>
        /// <param name="systemTime"></param>
        [DllImport("kernel32.dll")]
        public static extern void GetSystemTime(SystemTimeClass systemTime);

        /// <summary>
        /// 获取系统时间
        /// <para>这只是一个示例，传参类型为 Struct 按值传，引用传需加 ref </para>
        /// <para> <see cref="SystemTimeClass"/> 与 <see cref="SystemTimeStruct"/> 隐式转换 </para>
        /// </summary>
        /// <param name="systemTime"></param>
        [DllImport("kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTimeStruct systemTime);
    }


    /// <summary>
    /// WindowsAPI Kernel32库，扩展常用/通用，功能/函数，扩展示例，以及使用方式ad
    /// </summary>
    public static partial class Kernel32Extension
    {
    }

}
