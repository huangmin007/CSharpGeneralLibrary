using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.Extension
{
    /// <summary>
    /// String Extension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 将字符解析为 <see cref="System.Int32"/> 类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则按字符串分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果；</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToInt32Array(this String value, ref int[] array, char separator=',', NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0) 
                array = new int[stringArray.Length];

            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (String.IsNullOrWhiteSpace(stringArray[i])) continue;

                int temp = array[i];
                if (!Int32.TryParse(stringArray[i], style, provider, out array[i]))
                    array[i] = temp;
            }
        }



        /// <summary>
        /// 将字符解析为 <see cref="System.UInt32"/> 类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则字符分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToUInt32Array(this String value, ref uint[] array, char separator = ',', NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0)
                array = new uint[stringArray.Length];

            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (String.IsNullOrWhiteSpace(stringArray[i])) continue;

                uint temp = array[i];
                if (!UInt32.TryParse(stringArray[i], style, provider, out array[i]))
                    array[i] = temp;
            }
        }


        /// <summary>
        /// 将字符解析为 <see cref="System.Byte"/> 类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则字符分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToByteArray(this String value, ref byte[] array, char separator = ',', NumberStyles style = NumberStyles.HexNumber, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0)
                array = new byte[stringArray.Length];

            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (String.IsNullOrWhiteSpace(stringArray[i])) continue;

                byte temp = array[i];
                if (!Byte.TryParse(stringArray[i], style, provider, out array[i]))
                    array[i] = temp;
            }
        }

    }
}
