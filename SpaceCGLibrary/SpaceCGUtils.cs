using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceCG
{
    /// <summary>
    /// SpaceCG 实用/通用 函数
    /// </summary>
    public static class SpaceCGUtils
    {
        /// <summary>
        /// 移除对象的匿名事件（Lambda 表达式 或 匿名方法来创建 的 匿名函数）
        /// </summary>
        /// <param name="instanceObj">对象实例</param>
        /// <param name="eventName">对象事件名称</param>
        public static void RemoveAnonymousEvents(Object instanceObj, String eventName)
        {
            if (instanceObj == null || string.IsNullOrWhiteSpace(eventName)) throw new ArgumentNullException("参数不能为空");

            try
            {
                FieldInfo fields = instanceObj.GetType().GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                if (fields == null) return;

                object values = fields.GetValue(instanceObj);
                if (values == null) return;

                if (values is Delegate)
                {
                    Delegate anonymity = (Delegate)values;
                    foreach (Delegate handler in anonymity.GetInvocationList())
                    {
                        Console.WriteLine("Remove Anonymous Event: {0}", handler.Method.Name);
                        instanceObj.GetType().GetEvent(eventName).RemoveEventHandler(instanceObj, handler);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Anonymous Events Remove Error:{0}", ex);
            }
        }
    }
}
