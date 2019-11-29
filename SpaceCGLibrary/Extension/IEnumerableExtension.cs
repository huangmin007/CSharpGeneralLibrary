using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCG.Extension
{
    /// <summary>
    /// LINQ 扩展 扩展/实用/通用
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 获取指定对象在此集合中的位置索引
        /// <para>注意：该函数是直接使用泛型类的 Equals 方法比较对象</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            if (value == null) throw new ArgumentNullException("参数 value 不能为空");

            //source.Contains(value);
            
            int index = -1;
            int count = source.Count();

            for (int i = 0; i < count; i++)
            {
                if(value.Equals(source.ElementAt(i)))
                {
                    index = i;
                    return index;
                }
             }

            int index2 = source
                .Select((n, i) => new { Value = n, Index = i })
                .Aggregate((v1, v2) => value.Equals(v1) ? v1 : v2)
                .Index;

            return index;
        }

        /// <summary>
        /// 迭代中间值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double Median(this IEnumerable<double> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot compute median for an empty set.");
            }

            var sortedList = from number in source
                             orderby number
                             select number;

            int itemIndex = (int)sortedList.Count() / 2;

            if (sortedList.Count() % 2 == 0)
            {
                // Even number of items.
                return (sortedList.ElementAt(itemIndex) + sortedList.ElementAt(itemIndex - 1)) / 2;
            }
            else
            {
                // Odd number of items.
                return sortedList.ElementAt(itemIndex);
            }
        }

        public static double Median(this IEnumerable<int> source)
        {
            return (from num in source select (double)num).Median();
        }

        public static double Median<T>(this IEnumerable<T> numbers, Func<T, double> selector)
        {
            return (from num in numbers select selector(num)).Median();
        }

        // Extension method for the IEnumerable<T> interface.
        // The method returns every other element of a sequence.
        public static IEnumerable<T> AlternateElements<T>(this IEnumerable<T> source)
        {
            List<T> list = new List<T>();

            int i = 0;
            /*
            foreach (var element in source)
            {
                if (i % 2 == 0)
                {
                    list.Add(element);
                }

                i++;
            }
            */

            return list;
        }
    }
}
