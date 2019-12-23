#pragma warning disable CS1591,CS1572,CS1573
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
                        Console.WriteLine("Remove Anonymous Event: {0}", handler.Method.Name);
                        instanceObj.GetType().GetEvent(eventName).RemoveEventHandler(instanceObj, handler);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Remove Anonymous Events Error:{0}", ex);
            }
        }
        
        /*
                /// <summary>
                /// 修改注册表，设置开机启动项目
                /// </summary>
                /// <param name="fileName"></param>
                /// <param name="isAutoRun"></param>
                public static void SetAutoRun(string fileName, bool isAutoRun)
                {
                    RegistryKey reg = null;
                    try
                    {
                        if (!System.IO.File.Exists(fileName))
                            throw new Exception("该文件不存在!");

                        String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                        reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                        if (reg == null)
                            reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                        if (isAutoRun)
                            reg.SetValue(name, fileName);
                        else
                            reg.SetValue(name, false);
                    }
                    catch
                    {
                        Console.WriteLine("写入注册表失败");
                    }
                    finally
                    {
                        if (reg != null)
                            reg.Close();
                    }
                }
                */
    }
}
