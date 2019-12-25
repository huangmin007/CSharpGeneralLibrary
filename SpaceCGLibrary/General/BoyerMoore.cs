using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.General
{
    public sealed class BoyerMoore<T> where T: IReadOnlyList<T>, new()
    {
        static BoyerMoore()
        {
            //if(typeof(T) != typeof(byte) || typeof(T) != typeof(char))
            {
                //throw new Exception("泛型类型错误");
            }
        }

        public BoyerMoore()
        {
            //T t = new T();
        }

        public int Search<T>(IReadOnlyList<T> data) where T: struct
        {
            return -1;
        }
        
    }

    /// <summary>
    /// Boyer-Moore 算法实现
    /// <para><see cref="BoyerMoore{T}"/> 泛型版本，存在问题，where T: struct, IReadOnlyList&lt;T&gt; 无法将泛型 T 定义在 8 位数据范(byte, char)围内 </para>
    /// <para>所以就只是为了 减少数据的复制转换 (<see cref="List{T}.ToArray"/>, <see cref="Encoding.Default.GetBytes()"/> 等)， string, byte[], IReadOnlyList&lt;byte&gt;类型 分别实现</para>
    /// <para>参考：https://gist.github.com/mjs3339/0772431281093f1bca1fce2f2eca527d </para>
    /// <para>参考：https://www.cnblogs.com/gaochundong/p/boyer_moore_string_matching_algorithm.html </para>
    /// <para>参考：https://baike.baidu.com/item/Boyer-%20Moore%E7%AE%97%E6%B3%95/16548374?fr=aladdin </para>
    /// <para>Boyer-Moore-Horspool 参考：https://blog.csdn.net/lindexi_gd/article/details/100174714 </para>
    /// </summary>
    public sealed class BoyerMoore
    {
        /// <summary>
        /// 字母表大小
        /// </summary>
        private const int ALPHABET_SIZE = 256;

        /// <summary>
        /// 返回的索引集合列表，最初可以存储的元素数量
        /// </summary>
        private const int CAPACITY = 64;

        /// <summary>
        /// 能过 <see cref="GetBadCharacterShift(byte[])"/> 生成的 坏字符表 (Bad Character Heuristic)
        /// </summary>
        private int[] badTable;

        /// <summary>
        /// 通过 <see cref="GetGoodSuffixShift(byte[])"/> 生成的 好后缀表 (Good Suffix Heuristic)
        /// </summary>
        private int[] goodTable;

        /// <summary>
        /// 需要匹配的数据内容
        /// </summary>
        private byte[] pattern;

        /// <summary>
        /// 配置数据长度
        /// </summary>
        public int PatternLength { get; private set; }


        /// <summary>
        /// Boyer-Moore (BM) 匹配查找算法
        /// <para>适合大量数据匹配查找，数据量越大，效率越高</para>
        /// </summary>
        public BoyerMoore()
        {            
        }

        /// <summary>
        /// Boyer-Moore (BM) 匹配查找算法
        /// <para>适合大量数据匹配查找，数据量越大，效率越高</para>
        /// </summary>
        /// <param name="pattern">需要匹配的数据</param>
        public BoyerMoore(byte[] pattern)
        {
            SetPattern(pattern);
        }

        /// <summary>
        /// Boyer-Moore (BM) 匹配查找算法
        /// <para>适合大量数据匹配查找，数据量越大，效率越高</para>
        /// </summary>
        /// <param name="pattern"></param>
        public BoyerMoore(string pattern)
        {
            SetPattern(Encoding.Default.GetBytes(pattern));
        }

        #region Public Functions SetPattern
        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        public void SetPattern(byte[] pattern)
        {
            if (pattern == null || pattern.Length < 1)
                throw new ArgumentNullException("参数 pattern 不能空，长度不能小于 1 ");

            if (badTable != null)
            {
                Array.Clear(badTable, 0, badTable.Length);
                Array.Clear(goodTable, 0, goodTable.Length);

                this.pattern = null;
                this.badTable = null;
                this.goodTable = null;
            }

            this.pattern = pattern;
            this.PatternLength = this.pattern.Length;

            this.badTable = GetBadCharacterShift(pattern);
            this.goodTable = GetGoodSuffixShift(pattern);
        }
        #endregion


        #region Public Functions Search
        /// <summary>
        /// 在 source 中查找匹配 pattern 的 第一个位置，返回 -1 表示没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.Search(byte[], int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <exception cref="ArgumentNullException">参数不能为空，长度不得小于匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">匹配的起始位置不得小于 0，不得大于 源数据长度 - 匹配长度</exception>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(byte[] source, int start = 0)
        {
            if (source == null || source.Length < PatternLength) 
                throw new ArgumentNullException(nameof(source), "参数不能为空，长度不得小于匹配数据长度");
            if (start < 0 || start > source.Length - PatternLength) 
                throw new ArgumentOutOfRangeException(nameof(start), "匹配的起始位置不得小于 0，不得大于 源数据长度 - 匹配长度");

            int i, index = start;
            int maxCompareCount = source.Length - PatternLength;  //最多可比较的次数

            while (index <= maxCompareCount)
            {
                for (i = PatternLength - 1; i >= 0 && pattern[i] == source[i + index]; i--) ;

                if (i < 0)
                    return index;
                else
                    index += Math.Max(badTable[source[i + index]] - PatternLength - 1 + i, goodTable[i]);
            }

            return -1;
        }

        public int Search(List<byte> source, int start = 0)
        {
            if (pattern == null)
                throw new InvalidOperationException("未设置需要匹配的数据");

            if (source == null || source.Count < PatternLength)
                throw new ArgumentNullException("参数 source 不能空，长度不能小于 需要匹配数据的长度 ");

            var index = start;
            var limitLength = source.Count - PatternLength; //限定长度
            var patternLengthMinusOne = PatternLength - 1;

            while (index <= limitLength)
            {
                var j = patternLengthMinusOne;
                while (j >= 0 && pattern[j] == source[index + j]) j--;

                if (j < 0) return index;
                index += Math.Max(badTable[source[index + j]] - PatternLength + 1 + j, 1);
            }

            return -1;
        }


        public unsafe int Serach(ref string source, int start = 0)
        {
            if (pattern == null)
                throw new InvalidOperationException("未设置需要匹配的数据");

            if (source == null || source.Length < PatternLength)
                throw new ArgumentNullException("参数 source 不能空，长度不能小于 需要匹配数据的长度 ");

            var index = 0;
            var limit = source.Length - PatternLength;
            var patternLengthMinusOne = PatternLength - 1;

            fixed (char* pointerToByteArray = source)
            {
                var pointerToByteArrayStartingIndex = pointerToByteArray + start;
                fixed (byte* pointerToPattern = pattern)
                {
                    while (index <= limit)
                    {
                        var j = patternLengthMinusOne;
                        while (j >= 0 && pointerToPattern[j] == pointerToByteArrayStartingIndex[index + j]) j--;

                        if (j < 0) return index;
                        index += Math.Max(badTable[pointerToByteArrayStartingIndex[index + j]] - PatternLength + 1 + j, 1);
                    }
                }
            }

            return -1;
        }
        #endregion


        #region Public Functions SearchAll
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.SearchAll(byte[], int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <exception cref="ArgumentNullException">参数不能为空，长度不得小于匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">匹配的起始位置不得小于 0，不得大于 源数据长度 - 匹配长度</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public List<int> SearchAll(byte[] source, int start = 0)
        {
            if (source == null || source.Length < PatternLength)
                throw new ArgumentNullException(nameof(source), "参数不能为空，长度不得小于匹配数据长度");
            if (start < 0 || start > source.Length - PatternLength)
                throw new ArgumentOutOfRangeException(nameof(start), "匹配的起始位置不得小于 0，不得大于 源数据长度 - 匹配长度");

            int i, index = start;
            List<int> indexs = new List<int>(CAPACITY);
            int maxCompareCount = source.Length - PatternLength;  //最多可比较的次数

            while (index <= maxCompareCount)
            {
                for (i = PatternLength - 1; i >= 0 && pattern[i] == source[i + index]; i--) ;

                if (i < 0)
                {
                    indexs.Add(index);
                    index += goodTable[0];
                }
                else
                {
                    index += Math.Max(badTable[source[i + index]] - PatternLength - 1 + i, goodTable[i]);
                }
            }

            return indexs;
        }
        #endregion


        #region Public Functions SuperSearch
        /// <summary>
        /// 超级匹配，匹配查找第 cIndex 位置的索引
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cIndex"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public int SuperSearch(byte[] source, int cIndex, int start = 0)
        {
            var e = start;
            var c = 0;

            do
            {
                e = Search(source, e);
                if (e == -1) return -1;

                c++;
                e++;
            }
            while (c < cIndex);

            return e - 1;
        }
        #endregion





        #region Private Static Functions GetBadCharacterShift
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
        /// </summary>
        /// <param name="pattern">需要匹配的数据内容</param>
        /// <returns></returns>
        private static int[] GetBadCharacterShift(ref string pattern)
        {
            int i = 0;
            int[] badTable = new int[ALPHABET_SIZE];

            for (i = 0; i < ALPHABET_SIZE; i++)
                badTable[i] = pattern.Length;

            for (i = 0; i < pattern.Length - 1; i++)
                badTable[pattern[i]] = pattern.Length - 1 - i;

            return badTable;
        }
        /// <summary>
         /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
         /// </summary>
         /// <param name="pattern">需要匹配的数据内容</param>
         /// <returns></returns>
        private static int[] GetBadCharacterShift(byte[] pattern)
        {
            int i = 0;
            int[] badTable = new int[ALPHABET_SIZE];

            for (i = 0; i < ALPHABET_SIZE; i++)
                badTable[i] = pattern.Length;

            for (i = 0; i < pattern.Length - 1; i++)
                badTable[pattern[i]] = pattern.Length - 1 - i;

            return badTable;
        }        
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
        /// </summary>
        /// <param name="pattern">需要匹配的数据内容</param>
        /// <returns></returns>
        private static IList<int> GetBadCharacterShift(IReadOnlyList<byte> pattern)
        {
            int i = 0;
            IList<int> badTable = new List<int>();

            for (i = 0; i < ALPHABET_SIZE; i++)
                badTable[i] = pattern.Count;

            for (i = 0; i < pattern.Count - 1; i++)
                badTable[pattern[i]] = pattern.Count - 1 - i;

            return badTable;
        }
        #endregion

        #region Private Static Functions GetGoodSuffixShift
        /// <summary>
        /// 获取有效数据标记表 (好后缀 Good Suffix Heuristic)
        /// </summary>
        /// <returns></returns>
        private static int[] GetGoodSuffixShift(byte[] pattern)
        {
            int i = 0, j = 0;
            int patternLength = pattern.Length;

            int[] goodSuffixShifts = new int[patternLength];
            int[] suffixLengthArray = new int[patternLength];

            #region Get Good Suffix Length Array
            int lastPatternPosition = patternLength - 1;
            suffixLengthArray[patternLength - 1] = patternLength;

            for (i = patternLength - 2; i >= 0; --i)
            {
                if (i > lastPatternPosition && suffixLengthArray[i + patternLength - 1 - j] < i - lastPatternPosition)
                {
                    suffixLengthArray[i] = suffixLengthArray[i + patternLength - 1 - j];
                }
                else
                {
                    if (i < lastPatternPosition) lastPatternPosition = i;

                    j = i;
                    for (; lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - j]; --lastPatternPosition) ;

                    //while (lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - tempIndex])
                    //    --lastPatternPosition;

                    suffixLengthArray[i] = j - lastPatternPosition;
                }
            }
            #endregion

            for (i = 0; i < patternLength; ++ i)
                goodSuffixShifts[i] = patternLength;

            for (i = patternLength - 1; i >= -1; --i)
            {
                if (i == -1 || suffixLengthArray[i] == i + 1)
                {
                    for (; j < patternLength - 1 - i; ++j)
                    {
                        if (goodSuffixShifts[j] == patternLength)
                            goodSuffixShifts[j] = patternLength - 1 - i;
                    }
                }
            }

            for (i = 0; i < patternLength - 1; ++i)
                goodSuffixShifts[patternLength - 1 - suffixLengthArray[i]] = patternLength - 1 - i;

            return goodSuffixShifts;
        }
        /// <summary>
        /// 获取后缀长度数组
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static int[] GetSuffixLengthArray(byte[] pattern)
        {
            int patternLength = pattern.Length;
            int[] suffixLengthArray = new int[patternLength];
            suffixLengthArray[patternLength - 1] = patternLength;

            int i, j = 0;
            int lastPatternPosition = patternLength - 1;

            for (i = patternLength - 2; i >= 0; --i)
            {
                if (i > lastPatternPosition && suffixLengthArray[i + patternLength - 1 - j] < i - lastPatternPosition)
                {
                    suffixLengthArray[i] = suffixLengthArray[i + patternLength - 1 - j];
                }
                else
                {
                    if (i < lastPatternPosition)    lastPatternPosition = i;
                    
                    j = i;
                    for (; lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - j]; -- lastPatternPosition) ;

                    //while (lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - tempIndex])
                    //    --lastPatternPosition;
                    
                    suffixLengthArray[i] = j - lastPatternPosition;
                }
            }

            return suffixLengthArray;
        }
        #endregion



        #region Public Static Functions Search
        /// <summary>
        /// 在 source 中查找匹配 pattern 的 第一个位置，返回 -1 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public static int Search(ref string source, ref string pattern, int start = 0)
        {
            if (pattern == null || pattern.Length < 1) return -1;
            if (source == null || source.Length < pattern.Length) return -1;

            int index = start, i;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;        //最后一个匹配数据的位置
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            int[] offsetTable = GetBadCharacterShift(ref pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的 第一个位置，返回 -1 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public static int Search(byte[] source, byte[] pattern, int start = 0)
        {
            if (pattern == null || pattern.Length < 1) return -1;
            if (source == null || source.Length < pattern.Length) return -1;

            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数

            int index = start, i;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            int[] offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的 第一个位置，返回 -1 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public static int Search(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0)
        {
            if (pattern == null || pattern.Count < 1) return -1;
            if (source == null || source.Count < pattern.Count) return -1;

            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数

            int index = start, i;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            IList<int> offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return -1;
        }
        #endregion

        #region Public Static Functions SearchAll
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static List<int> SearchAll(ref string source, ref string pattern, int start = 0)
        {
            List<int> indexs = new List<int>(CAPACITY);
            if (pattern == null || pattern.Length < 1) return indexs;
            if (source == null || source.Length < pattern.Length) return indexs;

            int index = start, i;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;        //最后一个匹配数据的位置
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            int[] offsetTable = GetBadCharacterShift(ref pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) indexs.Add(index);
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return indexs;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static List<int> SearchAll(byte[] source, byte[] pattern, int start = 0)
        {
            List<int> indexs = new List<int>(CAPACITY);
            if (pattern == null || pattern.Length < 1)  return indexs;
            if (source == null || source.Length < pattern.Length) return indexs;

            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            
            int index = start, i;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            int[] offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) indexs.Add(index);
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return indexs;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static List<int> SearchAll(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0)
        {
            List<int> indexs = new List<int>(CAPACITY);
            if (pattern == null || pattern.Count < 1) return indexs;
            if (source == null || source.Count < pattern.Count) return indexs;

            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数

            int index = start, i;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            IList<int> offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) indexs.Add(index);
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);
            }

            return indexs;
        }

        #endregion

    }
}
