using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using log4net.Core;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// Log4Net 实用/通用 函数
    /// </summary>
    public class Log4NetUtils
    {
        /// <summary>
        /// 保留目录中的文件数量
        /// <para>跟据文件创建日期排序，保留 count 个最新文件，超出 count 数量的文件删除</para>
        /// <para>注意：该函数是比较文件的创建日期</para>
        /// </summary>
        /// <param name="count">要保留的数量</param>
        /// <param name="path">文件目录，当前目录 "/" 表示，不可为空</param>
        /// <param name="searchPattern">只在目录中(不包括子目录)，查找匹配的文件；例如："*.jpg" 或 "temp_*.png"</param>
        public static void ReserveFileCount(int count, string path, string searchPattern = null)
        {
            if (count < 0 || String.IsNullOrWhiteSpace(path))
            {
                Debug.Fail("ReserveFileCount 参数错误");
                return;
            }
            
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = searchPattern == null ? dir.GetFiles() : dir.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

            if (files.Length <= count) return;

            //按文件的创建时间，升序排序(最新创建的排在最前面)
            Array.Sort(files, (f1, f2) =>
            {
                return f2.CreationTime.CompareTo(f1.CreationTime);
            });

            for (int i = count; i < files.Length; i++)
            {
                files[i].Delete();
                Trace.TraceWarning("Delete File ... CreationTime:{0}\t Name:{1}", files[i].CreationTime, files[i].Name);
            }
        }

        /// <summary>
        /// 保留目录中的文件天数
        /// <para>跟据文件上次修时间起计算，保留 days 天的文件，超出 days 天的文件删除</para>
        /// <para>注意：该函数是比较文件的上次修改日期</para>
        /// </summary>
        /// <param name="days">保留天数</param>
        /// <param name="path">文件夹目录</param>
        /// <param name="searchPattern">文件匹配类型</param>
        public static void ReserveFileDays(int days, string path, string searchPattern = null)
        {
            if (days < 0 || String.IsNullOrWhiteSpace(path))
            {
                Debug.Fail("ReserveFileDays 参数错误");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = searchPattern == null ? dir.GetFiles() : dir.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
            if (files.Length == 0) return;

            IEnumerable<FileInfo> removes =
                from file in files
                where file.LastWriteTime < DateTime.Today.AddDays(-days)
                select file;

            foreach(var file in removes)
            {
                file.Delete();
                Trace.TraceWarning("Delete File ... LastWriteTime:{0}\t Name:{1}", file.LastWriteTime, file.Name);
            }
        }

        /// <summary>
        /// 序列化 <see cref="log4net.Core.LoggingEvent"/> 对象
        /// </summary>
        /// <param name="logger">需要序列化的 <see cref="log4net.Core.LoggingEvent"/> 对象</param>
        /// <returns> 返回序列化后的字节数据 </returns>
        public static byte[] SerializeLoggingEvent(LoggingEvent logger)
        {
            byte[] buffer = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, logger);
                    buffer = stream.GetBuffer();
                    stream.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return buffer;
        }

        /// <summary>
        /// 反序列化 <see cref="log4net.Core.LoggingEvent"/> 对象
        /// </summary>
        /// <param name="buffer">要反序列化的数据的流</param>
        /// <returns>返回 <see cref="log4net.Core.LoggingEvent"/> 对象</returns>
        public static LoggingEvent DeserializeLoggingEvent(byte[] buffer)
        {
            LoggingEvent logger = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    IFormatter formatter = new BinaryFormatter();
                    logger = (LoggingEvent)formatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return logger;
        }
    }
}
