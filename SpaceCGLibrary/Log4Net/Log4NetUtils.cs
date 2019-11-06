using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// Log4Net 实用/通用 函数
    /// </summary>
    public class Log4NetUtils
    {
        /// <summary>
        /// 跟据文件创建日期，保留最近创建的文件数量，其余的删除掉
        /// <para>注意：该函数是比较文件的创建日期，不是修改日期</para>
        /// </summary>
        /// <param name="count">要保留的数量</param>
        /// <param name="path">文件目录，当前目录 "/" 表示，不可为空</param>
        /// <param name="searchPattern">只在目录中(不包括子目录)，查找匹配的文件；例如："*.jpg" 或 "temp_*.png"</param>
        public static void ReserveFileCount(int count, string path, string searchPattern = null)
        {
            if (count < 0 || String.IsNullOrWhiteSpace(path)) throw new ArgumentException("参数错误");
            
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
    }
}
