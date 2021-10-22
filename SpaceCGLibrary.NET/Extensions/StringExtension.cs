using System;
using System.Globalization;

namespace SpaceCG.Extension
{
    /// <summary>
    /// String Extension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 跟据分割符，将字符解析为 <see cref="System.Int32"/> 类型数组
        /// <para>示例：ref null => "10,10,,1080" => {10,10,0,1080}</para>
        /// <para>示例：ref {0,0,1920,1080} => "10,10,,720" => {10,10,1920,720}</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则按字符串分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果；</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToInt32Array(this string value, ref int[] array, char separator = ',', NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0)
            {
                array = new int[stringArray.Length];
            }
            
            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrWhiteSpace(stringArray[i]))
                {
                    continue;
                }

                int temp = array[i];
                if (!int.TryParse(stringArray[i], style, provider, out array[i]))
                {
                    array[i] = temp;
                }
            }
        }


        /// <summary>
        /// 跟据分割符，将字符解析为 <see cref="System.UInt32"/> 类型数组
        /// <para>示例：ref null => "10,10,,1080" => {10,10,0,1080}</para>
        /// <para>示例：ref {0,0,1920,1080} => "10,10,,720" => {10,10,1920,720}</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则字符分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToUInt32Array(this string value, ref uint[] array, char separator = ',', NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0)
            {
                array = new uint[stringArray.Length];
            }

            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrWhiteSpace(stringArray[i]))
                {
                    continue;
                }

                uint temp = array[i];
                if (!uint.TryParse(stringArray[i], style, provider, out array[i]))
                {
                    array[i] = temp;
                }
            }
        }


        /// <summary>
        /// 跟据分割符，将字符串解析为 <see cref="System.Byte"/> 类型数组
        /// <para>示例："FA,FA,01,02,03,04,0D,0A" => {0xFA, 0xFA, 0x01, 0x02, 0x03, 0x04, 0x0D, 0x0A}  //NumberStyles.HexNumber</para>
        /// <para>示例："FA FA 01 02 03 04 0D 0A" => {0xFA, 0xFA, 0x01, 0x02, 0x03, 0x04, 0x0D, 0x0A}  //separator=' ', NumberStyles.HexNumber</para>
        /// <para>示例："250,250,1,2,3,4,13,10" => {0xFA, 0xFA, 0x01, 0x02, 0x03, 0x04, 0x0D, 0x0A}  //NumberStyles.None </para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array">如果为空，则字符分割的数组长度返回转换结果，如果不为空，则按 array 的长度返回转换结果</param>
        /// <param name="separator">分隔字符串中子字符串的字符数组、不包含分隔符的空数组或 null。</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 string 中的样式元素。要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 string 的区域性特定格式设置信息。</param>
        /// <returns></returns>
        public static void ToByteArray(this string value, ref byte[] array, char separator = ',', NumberStyles style = NumberStyles.HexNumber, IFormatProvider provider = null)
        {
            string[] stringArray = value.Trim().Split(separator);

            if (array == null || array.Length <= 0)
            {
                array = new byte[stringArray.Length];
            }

            int length = Math.Min(stringArray.Length, array.Length);

            for (int i = 0; i < length; i++)
            {
                if (string.IsNullOrWhiteSpace(stringArray[i]))
                {
                    continue;
                }

                byte temp = array[i];
                if (!byte.TryParse(stringArray[i], style, provider, out array[i]))
                {
                    array[i] = temp;
                }
            }
        }


        /// <summary>
        /// 将 十六进制字符串(hex string) 解析为 <see cref="System.Byte"/> 类型数组，无分割符，与 <see cref="ToByteArray"/> 功能类似
        /// <para>注意：字符串长度不能为奇数位，可使用空格断开以提高可读性</para>
        /// <para>示例1："FAFA010203040D0A" => {0xFA, 0xFA, 0x01, 0x02, 0x03, 0x04, 0x0D, 0x0A}</para>
        /// <para>示例2："FA FA 01 02 03 04 0D 0A" => {0xFA, 0xFA, 0x01, 0x02, 0x03, 0x04, 0x0D, 0x0A}</para>
        /// </summary>
        /// <exception cref="ArgumentException">字符串长度不能为奇数位</exception>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static void ToHexBytes(this string value, ref byte[] array)
        {
            string hexString = value.Replace(" ", "");
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException($"字符串长度不能为奇数位: {value}");
            }

            if (array == null || array.Length <= 0)
            {
                array = new byte[hexString.Length / 2];
            }

            int length = Math.Min(hexString.Length / 2, array.Length);

            for (int i = 0; i < length; i++)
            {
                string byteValue = hexString.Substring(i * 2, 2);
                _ = byte.TryParse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out array[i]);
            }

        }
    }
}
