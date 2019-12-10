using SpaceCG.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.Extension
{
    /// <summary>
    /// <see cref="System.Diagnostics.PerformanceCounter"/>, <see cref="System.Diagnostics.PerformanceCounterCategory"/>
    /// </summary>
    public partial class PerformanceExtension
    {
        /// <summary>
        /// 检索输出本地计算机上注册的 性能计数器 类别的列表 PerformanceCounterCategory.GetCategories()。
        /// </summary>
        public static void ToDebugPCCs()
        {
            PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();
            foreach (PerformanceCounterCategory category in categories)
            {
                if (category == null || !PerformanceCounterCategory.Exists(category.CategoryName)) continue;

                string name = "<null>";
                string help = "<null>";
                string machineName = "<null>";

                try { name = category.CategoryName; } catch { }
                try { help = category.CategoryHelp; } catch { }
                try { machineName = category.MachineName != null ? category.MachineName : "<null>"; } catch { }

                Console.WriteLine("CategoryName: {0} CategoryType: {1} MachineName: {2} Help: {3}",
                    name.PadRight(48), category.CategoryType.ToString().PadRight(16), machineName.PadRight(8), help);
            }
        }

        /// <summary>
        /// 输出 性能计数器组件 信息
        /// </summary>
        /// <param name="pc"></param>
        public static void ToDebug(PerformanceCounter pc)
        {
            if (pc == null) throw new ArgumentNullException("参数 pc 不能为空");

            string name = "<null>";
            string help = "<null>";
            string type = "<null>";
            string instance = "<null>";
            string value = "0.0";

            try { name = pc.CounterName; } catch { }
            try { help = pc.CounterHelp; } catch { }
            try { instance = pc.InstanceName; } catch { }
            try { type = pc.CounterType.ToString(); } catch { }
            try { value = pc.NextValue().ToString(); } catch { }

            Console.WriteLine("Instance:{0} Name:{1} Type:{2} Value:{3} Help:{4}",
                instance.PadRight(16), name.PadRight(32), type.PadRight(32), value.PadRight(16), help);
        }

        /// <summary>
        /// 输出 性能计数器组件 信息
        /// </summary>
        /// <param name="pcs"></param>
        public static void ToDebug(PerformanceCounter[] pcs)
        {
            if (pcs == null) throw new ArgumentNullException("参数 pcs 不能为空");
            
            foreach (PerformanceCounter pc in pcs) ToDebug(pc);
        }

        /// <summary>
        /// 输出 CategoryName(类别) 不能为空的 性能计数器组件 信息 
        /// </summary>
        /// <param name="pcc"></param>
        public static void ToDebug(PerformanceCounterCategory pcc)
        {
            if (pcc == null) throw new ArgumentNullException("参数 pcc 不能为空");
            if (pcc.CategoryName == null) throw new ArgumentNullException("尚未设置类别名称属性 CategoryName ");

            string[] instances = pcc.GetInstanceNames();

            if (instances.Length == 0)
            {
                ToDebug(pcc.GetCounters());
            }
            else
            {
                for (int i = 0; i < instances.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(instances[i])) continue;
                    Console.WriteLine("\t\tInstanceName:{0}", instances[i]);
                    PerformanceCounter[] counters = null;

                    try
                    {
                        counters = pcc.GetCounters(instances[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\t\t\t\t{0}", ex.Message);
                        continue;
                    }

                    if (counters == null || counters.Length == 0) continue;
                    ToDebug(counters);                    
                }
            }//end if
        }

    }
}
