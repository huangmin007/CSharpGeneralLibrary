using System;
using System.Reflection;

namespace SpaceCG
{
    /// <summary>
    /// SpaceCG 实用/通用 函数
    /// </summary>
    public static class SpaceCGUtils
    {
        /// <summary>
        /// SpaceCG Library Logger
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger("Library.Logger");

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SpaceCGUtils()
        {

        }

        /// <summary>
        /// 移除对象的匿名委托事件（Lambda 表达式 或 匿名方法来创建 的 匿名函数）
        /// </summary>
        /// <param name="instanceObj">对象实例</param>
        /// <param name="eventName">对象事件名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveAnonymousEvents(Object instanceObj, String eventName)
        {
            if (instanceObj == null || string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException("参数不能为空");

            BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

            try
            {
                FieldInfo fields = instanceObj.GetType().GetField(eventName, bindingAttr);      //当前类类型中查找
                if (fields == null)
                {
                    fields = instanceObj.GetType().BaseType.GetField(eventName, bindingAttr);   //基类类型中查找
                    if (fields == null)  return;
                }

                object values = fields.GetValue(instanceObj);
                if (values == null) return;

                if (values is Delegate)
                {
                    Delegate anonymity = (Delegate)values;
                    foreach (Delegate handler in anonymity.GetInvocationList())
                    {
                        Log.InfoFormat("Object {0} Remove Anonymous Event: {1}", nameof(instanceObj), handler.Method.Name);
                        instanceObj.GetType().GetEvent(eventName).RemoveEventHandler(instanceObj, handler);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.InfoFormat("Remove Anonymous Events Error: ObjectInstance:{0}({1}) EventName:{2}", instanceObj, nameof(instanceObj), eventName);
                Log.ErrorFormat("Remove Anonymous Events Error:{0}", ex);
            }
        }
        
    }
}
