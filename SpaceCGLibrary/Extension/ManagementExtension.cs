#pragma warning disable CS1591,CS1572
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace SpaceCG.Extension
{
    /// <summary>
    /// System.Management 命名空间 扩展/实用/通用 函数
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/cimwin32-wmi-providers </para>
    /// <para>有关 WMI 类，参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-provider </para>
    /// <para>有关 WMI Events，参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/determining-the-type-of-event-to-receive </para>
    /// <para>有关 WQL 语法，参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </para>
    /// </summary>
    public static class ManagementExtension
    {
        /// <summary>
        /// \\.\Root\CIMV2
        /// <para>System.Management.ManagementScope 的服务器和命名空间。</para>
        /// </summary>
        public const string LocalPath = @"\\.\Root\CIMV2";


        #region "__InstanceCreationEvent" AND "__InstanceDeletionEvent"

        /// <summary> ManagementEventWatcher Object </summary>
        static ManagementEventWatcher InstanceCreationEvent;
        /// <summary> ManagementEventWatcher Object </summary>
        static ManagementEventWatcher InstanceDeletionEvent;

        /// <summary>
        /// 监听 "__InstanceCreationEvent" AND "__InstanceDeletionEvent" 事件；请使用 <see cref="RemoveInstanceChange"/> 移除监听
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity'"    //监听即插即用设备状态，有关 Win32_PnPEntity(WMI类) 属性参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-pnpentity </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%({Serial.PortName})'"    //监听即插即用设备状态，且名称为串口名称</para>
        /// <para>示例：$"TargetInstance ISA 'Win32_LogicalDisk' AND TargetInstance.DriveType = 2 OR TargetInstance.DriveType = 4"  //监听移动硬盘状态 </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="withinInterval">指定对于接收此事件而言所能接受的滞后时间。该值用于以下情况：对于所请求的查询没有显式事件提供程序，并且需要 WMI 轮询条件。该间隔是在必须发送事件通知之前可以经过的最长时间。</param>
        /// <param name="changeCallback"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ListenInstanceChange(String wql_condition, TimeSpan withinInterval, Action<ManagementBaseObject> changeCallback)
        {
            if (InstanceCreationEvent != null || InstanceDeletionEvent != null)
                throw new InvalidOperationException("此函数只是单个监听示例，不可重复调用监听");

            if (string.IsNullOrWhiteSpace(wql_condition) || changeCallback == null) throw new ArgumentNullException("参数不能为空");

            ManagementScope scope = new ManagementScope(@"\\.\Root\CIMV2", new ConnectionOptions()
            {
                //Username = "",
                //Password = "",
                EnablePrivileges = true,
            });

            //__InstanceCreationEvent 
            InstanceCreationEvent = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceCreationEvent", withinInterval, wql_condition));
            InstanceCreationEvent.EventArrived += (s, e) =>
            {
                SpaceCGUtils.Log.InfoFormat("Instance Creation Event :: {0}", e.NewEvent.ClassPath);
                changeCallback?.Invoke(e.NewEvent);
            };

            //__InstanceDeletionEvent
            InstanceDeletionEvent = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceDeletionEvent", withinInterval, wql_condition));
            InstanceDeletionEvent.EventArrived += (s, e) =>
            {
                SpaceCGUtils.Log.InfoFormat("Instance Deletion Event :: {0}", e.NewEvent.ClassPath);
                changeCallback?.Invoke(e.NewEvent);
            };

            InstanceCreationEvent.Start();
            InstanceDeletionEvent.Start();
        }
        /// <summary>
        /// 监听 "__InstanceCreationEvent" AND "__InstanceDeletionEvent" 事件；请使用 <see cref="RemoveInstanceChange"/> 移除监听
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity'"    //监听即插即用设备状态，有关 Win32_PnPEntity(WMI类) 属性参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-pnpentity </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%({Serial.PortName})'"    //监听即插即用设备状态，且名称为串口名称</para>
        /// <para>示例：$"TargetInstance ISA 'Win32_LogicalDisk' AND TargetInstance.DriveType = 2 OR TargetInstance.DriveType = 4"  //监听移动硬盘状态 </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="changeCallback"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ListenInstanceChange(String wql_condition, Action<ManagementBaseObject> changeCallback)
        {
            ListenInstanceChange(wql_condition, TimeSpan.FromSeconds(1), changeCallback);
        }
        /// <summary>
        /// 监听 "__InstanceCreationEvent" AND "__InstanceDeletionEvent" 事件；请使用 <see cref="RemoveInstanceChange"/> 移除监听
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity'"    //监听即插即用设备状态，有关 Win32_PnPEntity(WMI类) 属性参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-pnpentity </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Name LIKE '%({Serial.PortName})'"    //监听即插即用设备状态，且名称为串口名称</para>
        /// <para>示例：$"TargetInstance ISA 'Win32_LogicalDisk' AND TargetInstance.DriveType = 2 OR TargetInstance.DriveType = 4"  //监听移动硬盘状态 </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="withinInterval">指定对于接收此事件而言所能接受的滞后时间。该值用于以下情况：对于所请求的查询没有显式事件提供程序，并且需要 WMI 轮询条件。该间隔是在必须发送事件通知之前可以经过的最长时间。</param>
        /// <param name="changeCallback"></param>
        /// <param name="Log"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ListenInstanceChangeAsync(String wql_condition, TimeSpan withinInterval, Action<ManagementBaseObject> changeCallback)
        {
            await Task.Run(() => ListenInstanceChange(wql_condition, withinInterval, changeCallback));
        }
        /// <summary>
        /// 移除并销毁 由 <see cref="ListenInstanceChange(string, TimeSpan, Action{ManagementBaseObject}, log4net.ILog)"/> 创建的监听。
        /// <para>移除监听 "__InstanceCreationEvent" AND "__InstanceDeletionEvent" 事件</para>
        /// </summary>
        public static void RemoveInstanceChange()
        {
            if (InstanceCreationEvent != null)
            {
                SpaceCGUtils.RemoveAnonymousEvents(InstanceCreationEvent, "EventArrived");
                InstanceCreationEvent?.Stop();
                InstanceCreationEvent?.Dispose();
            }

            if (InstanceDeletionEvent != null)
            {
                SpaceCGUtils.RemoveAnonymousEvents(InstanceDeletionEvent, "EventArrived");
                InstanceDeletionEvent?.Stop();
                InstanceDeletionEvent?.Dispose();
            }

            InstanceCreationEvent = null;
            InstanceDeletionEvent = null;
        }
        
        #endregion


        #region "__InstanceModificationEvent"

        /// <summary> ManagementEventWatcher Object </summary>
        static ManagementEventWatcher InstanceModificationEvent;

        /// <summary>
        /// 监听 "__InstanceModificationEvent" 事件 （持续监听事件，按自定义时间间隔查询）
        /// <para>请使用 <see cref="RemoveInstanceModification"/> 移除监听 </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_Battery'"    //持续监听电池状态，EstimatedChargeRemaining 表示电池电量；更多 Win32_Battery 类的属性，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="withinInterval">指定对于接收此事件而言所能接受的滞后时间。该值用于以下情况：对于所请求的查询没有显式事件提供程序，并且需要 WMI 轮询条件。该间隔是在必须发送事件通知之前可以经过的最长时间。</param>
        /// <param name="changeCallback"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ListenInstanceModification(String wql_condition, TimeSpan withinInterval, Action<ManagementBaseObject> changeCallback)
        {
            if (InstanceModificationEvent != null)
                throw new InvalidOperationException("此函数只是单个监听示例，不可重复调用监听"); ;
            if (string.IsNullOrWhiteSpace(wql_condition) || changeCallback == null) throw new ArgumentNullException("参数不能为空");

            ManagementScope scope = new ManagementScope(@"\\.\Root\CIMV2", new ConnectionOptions()
            {
                EnablePrivileges = true,
            });

            //__InstanceModificationEvent 
            InstanceModificationEvent = new ManagementEventWatcher()
            {
                Scope = scope,
                Query = new WqlEventQuery()
                {
                    Condition = wql_condition,
                    WithinInterval = withinInterval,
                    EventClassName = "__InstanceModificationEvent",
                }
            };
            InstanceModificationEvent.EventArrived += (s, e) =>
            {
                SpaceCGUtils.Log.DebugFormat("Instance Modification Event :: {0}", e.NewEvent.ClassPath);
                changeCallback?.Invoke(e.NewEvent);
            };
            InstanceModificationEvent.Start();
        }
        /// <summary>
        /// 监听 "__InstanceModificationEvent"(继承 "__InstanceOperationEvent") 事件 （持续监听事件，按固定 1s 查询一次状态生成事件）；有关事件请参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/--instancemodificationevent 
        /// <para>请使用 RemoveInstanceModification 移除监听 </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_Battery'"    //持续监听电池状态，EstimatedChargeRemaining 表示电池电量表示电池电量；更多 Win32_Battery 类的属性，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="changeCallback"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ListenInstanceModification(String wql_condition, Action<ManagementBaseObject> changeCallback)
        {
            ListenInstanceModification(wql_condition, TimeSpan.FromSeconds(1), changeCallback);
        }
        /// <summary>
        /// 监听 "__InstanceModificationEvent" 事件 （持续监听事件，按自定义时间间隔查询）
        /// <para>请使用 <see cref="RemoveInstanceModification"/> 移除监听 </para>
        /// <para>示例：$"TargetInstance ISA 'Win32_Battery'"    //持续监听电池状态，EstimatedChargeRemaining 表示电池电量；更多 Win32_Battery 类的属性，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery </para>
        /// <para>更多 WMI 类，请参考：https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        /// </summary>
        /// <param name="wql_condition">要应用到指定类的事件的条件。WQL 条件语句，关于 WQL 参考：https://docs.microsoft.com/zh-cn/windows/win32/wmisdk/wql-sql-for-wmi?redirectedfrom=MSDN </param>
        /// <param name="withinInterval">指定对于接收此事件而言所能接受的滞后时间。该值用于以下情况：对于所请求的查询没有显式事件提供程序，并且需要 WMI 轮询条件。该间隔是在必须发送事件通知之前可以经过的最长时间。</param>
        /// <param name="changeCallback"></param>
        /// <param name="Log"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ListenInstanceModificationAsync(String wql_condition, TimeSpan withinInterval, Action<ManagementBaseObject> changeCallback)
        {
            await Task.Run(() => ListenInstanceModification(wql_condition, withinInterval, changeCallback));
        }
        /// <summary>
        /// 移除并销毁 由 <see cref="ListenInstanceChange(string, TimeSpan, Action{ManagementBaseObject}, log4net.ILog)"/> 创建的监听。
        /// <para>移除监听 "__InstanceModificationEvent" 事件</para>
        /// </summary>
        public static void RemoveInstanceModification()
        {
            if (InstanceModificationEvent == null) return;
            SpaceCGUtils.RemoveAnonymousEvents(InstanceModificationEvent, "EventArrived");

            InstanceModificationEvent?.Stop();
            InstanceModificationEvent?.Dispose();
            InstanceModificationEvent = null;
        }
        #endregion

        
        #region ToDebug ToString
        public static void ToDebug(this PropertyData data)
        {
            if (data == null) return;
            string values = "";

            if(data.Value == null)
            {
                values = string.Format("\"{0}\"", "<null>");
            }
            else
            {
                if(data.IsArray)
                {
                    foreach (var item in data.Value as Array)
                        values += string.Format("\"{0}\",", item == null ? "<null>" : item);
                }
                else
                {
                    values = string.Format("\"{0}\"", data.Value.ToString());
                }
            }

            Console.WriteLine("\t{0} {1} {2}", data.Name.PadRight(16), data.Type.ToString().PadRight(12), values.PadRight(64));
        }
        public static void ToDebug(this MethodData data)
        {
            if (data == null) return;

            Console.WriteLine("  Name:{0}", data.Name);

            if (data.InParameters != null)
            {
                Console.WriteLine("  [In]");
                ToDebug(data.InParameters.Properties);
            }
            if (data.OutParameters != null)
            {
                Console.WriteLine("  [Out]");
                ToDebug(data.OutParameters.Properties);
            }
        }
        public static void ToDebug(this QualifierData data)
        {
            if (data == null) return;

            string value = string.Format("\"{0}\"", data.Value != null ? data.Value.ToString() : "<null>");
            Console.WriteLine("\t{0} {1}", data.Name.PadRight(16), value.PadRight(64));
        }

        public static void ToDebug(this PropertyDataCollection collection)
        {
            if (collection == null || collection.Count <= 0) return;

            foreach (PropertyData data in collection) ToDebug(data);
        }
        public static void ToDebug(this MethodDataCollection collection)
        {
            if (collection == null || collection.Count <= 0) return;

            Console.WriteLine("Methods:");
            foreach (MethodData data in collection) ToDebug(data);
        }
        public static void ToDebug(this QualifierDataCollection collection)
        {
            if (collection == null || collection.Count <= 0) return;

            Console.WriteLine("Qualifiers:");
            foreach (QualifierData data in collection) ToDebug(data);
        }

        public static void ToDebug(this ManagementClass cls)
        {
            if (cls == null) return;

            Console.WriteLine("=====================================================Start");
            Console.WriteLine("ManagementClass:\"{0}\"", cls.ClassPath);
            Console.WriteLine("Properties:");

            //SystemProperties
            ToDebug(cls.SystemProperties);
            //Properties
            ToDebug(cls.Properties);
            //Methods
            ToDebug(cls.Methods);
            //Qualifiers
            ToDebug(cls.Qualifiers);

            Console.WriteLine("=====================================================End");
        }
        public static void ToDebug(this ManagementBaseObject obj)
        {
            if (obj == null) return;
            
            Console.WriteLine("=====================================================Start");
            Console.WriteLine("ManagementBaseObject:\"{0}\"", obj.ClassPath);
            Console.WriteLine("Properties:");

            //SystemProperties
            ToDebug(obj.SystemProperties);
            //Properties
            ToDebug(obj.Properties);
            //Qualifiers
            ToDebug(obj.Qualifiers);
            
            Console.WriteLine("=====================================================End");
        }
        #endregion

        /// <summary>
        /// 获取 ManagementClass 的属性
        /// </summary>
        /// <param name="path">WMI 类的路径。该类表示 WMI 中的一个 CIM 管理类。CIM 类表示包括硬件、软件、进程等在内的管理信息。
        ///     <para>系统硬件类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        ///     <para>操作系统类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/operating-system-classes </para>
        ///     <para>性能计数器类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/performance-counter-classes </para>
        ///     <para>WMI服务管理类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/wmi-service-management-classes </para>
        /// </param>
        /// <returns></returns>
        public static string[] GetClassProperties(string path)
        {
            using (ManagementClass mc = new ManagementClass(path))
                return (from PropertyData data in mc.Properties select data.Name).ToArray();
        }

        /// <summary>
        /// 调用指定的 WMI 查询并返回结果
        /// </summary>
        /// <param name="query">对象将调用的 WMI 查询</param>
        /// <returns></returns>
        public static Dictionary<string, string>[] Searcher(string query)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                int index = 0;
                ManagementObjectCollection collection = searcher.Get();
                Dictionary<string, string>[] list = new Dictionary<string, string>[collection.Count];

                foreach (ManagementObject obj in collection)
                {
                    list[index] = new Dictionary<string, string>();
                    foreach (PropertyData pd in obj.Properties)
                    {
                        if (string.IsNullOrWhiteSpace(pd.Name)) continue;
                        list[index].Add(pd.Name, pd.Value == null ? "<null>" : pd.IsArray ? string.Join(",", (Array)pd.Value) : pd.Value.ToString());
                    }

                    index++;
                }

                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">WMI 类的路径。该类表示 WMI 中的一个 CIM 管理类。CIM 类表示包括硬件、软件、进程等在内的管理信息。
        ///     <para>系统硬件类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/computer-system-hardware-classes </para>
        ///     <para>操作系统类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/operating-system-classes </para>
        ///     <para>性能计数器类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/performance-counter-classes </para>
        ///     <para>WMI服务管理类参考：https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/wmi-service-management-classes </para>
        /// </param>
        /// <param name="conditions"></param>
        /// <param name="properties">需要输出的 WMI 类的属性值，为空表示所有属性</param>
        /// <returns></returns>
        public static Dictionary<string, string>[] Searcher(string path, string conditions, params string[] properties)
        {
            string pros = properties.Length > 0 ? string.Join(",", properties) : "*";
            string query = string.IsNullOrWhiteSpace(conditions) ? $"SELECT {pros} FROM {path}" : $"SELECT {pros} FROM {path} WHERE {conditions}";

            return Searcher(query);
        }

        /// <summary>
        /// 获取 CPU处理器 信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetProcessorInfo()
        {
            return Searcher("SELECT * FROM Win32_Processor")[0];
        }

        /// <summary>
        /// 获取主板信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetBaseBoardInfo()
        {
            return Searcher("SELECT * FROM Win32_BaseBoard")[0];
        }

        #region GetPortNames
        /// <summary>
        /// 获取当前计算机的 串行端口 完整名称 的数组
        /// <para>与 <see cref="System.IO.Ports.SerialPort.GetPortNames"/> 不同，SerialPort.GetPortNames() 只输出类似"COM3,COM4,COMn"，该函数输出串口对象的名称或是驱动名，类似："USB Serial Port (COM3)" ... </para>
        /// <para>这只是 WMI 示例应用函数，用于查询 串口名称 信息。更多应用参考 WMI。</para>
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            String query = "SELECT Name FROM Win32_PnPEntity WHERE Name LIKE '%(COM_)' OR Name LIKE '%(COM__)'";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                var names = from ManagementObject obj in searcher.Get()
                            from PropertyData pd in obj.Properties
                            where !string.IsNullOrWhiteSpace(pd.Name) && pd.Value != null
                            select pd.Value.ToString();

                return names.ToArray();
            }
        }

        /// <summary>
        /// 获取当前计算机的 串行端口 完整名称 的数组
        /// <para>与 <see cref="System.IO.Ports.SerialPort.GetPortNames"/> 不同，SerialPort.GetPortNames() 只输出类似"COM3,COM4,COMn"，该函数输出串口对象的名称或是驱动名，类似："USB Serial Port (COM3)" ... </para>
        /// <para>这只是 WMI 示例应用函数，用于查询 串口名称 信息。更多应用参考 WMI。</para>
        /// </summary>
        /// <returns></returns>
        public static async Task<string[]> GetPortNamesAsync()
        {
            return await Task.Run<string[]>(()=> GetPortNamesAsync());
        }
        #endregion

    }
}
