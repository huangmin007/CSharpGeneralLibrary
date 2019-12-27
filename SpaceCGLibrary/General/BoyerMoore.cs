using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.General
{
    /// <summary>
    /// Boyer-Moore 算法实现。问题：多字节查找是否可以不生成坏字符表，直接 while 循环中查找 pattern 的位置，两种方式哪种效率更高呢？？
    /// <para>该算法查找原始 单字节集合 效率非常高(如果匹配的 pattern集合 长度为 1 或很少，也不建议使用该方法)，但 按该算法查找 双字节字符(比如中文字符) 效率就怎么样了</para>
    /// <para>当 isSingleByteOnly 为 false 时使用双字节字符查找，双字节字符查找生成的坏字符表占 0xFFFF + 1 长度(哎，太大了，占用字节大小 = sizeof(int) * (0xFFFF + 1))，
    ///     所以啊，双字节查找适合大量数据的查找才能发挥该算法的优势，否则不建议使用应该算法查找双字节字符 </para>
    /// <para>为了提高性能，字符参数使用 ref 引用类型减少数据拷贝，函数参数 isSingleByteOnly, start, end 都是为了提升查找性能，设计的限制参数</para>
    /// <para>静态查找函数，会检查参数，但不会抛出异常，只会返回有或无，也是为了提升性能</para>
    /// <para>参考：https://baike.baidu.com/item/Boyer-%20Moore%E7%AE%97%E6%B3%95/16548374?fr=aladdin </para>
    /// <para>参考：https://www.cnblogs.com/gaochundong/p/boyer_moore_string_matching_algorithm.html </para>
    /// </summary>
    public sealed class BoyerMoore
    {
        #region Private Variables
        /// <summary>
        /// 返回的索引集合列表，最初可以存储的元素数量
        /// </summary>
        internal const int CAPACITY = 256;

        /// <summary>
        /// 能过 <see cref="GetBadCharacterShift(IReadOnlyList{byte})"/> 生成的 坏字符表 (Bad Character Heuristic)
        /// </summary>
        private int[] offsetTable;

        /// <summary>
        /// 通过 <see cref="GetGoodSuffixShift(IReadOnlyList{byte})"/> 生成的 好后缀表 (Good Suffix Heuristic)
        /// </summary>
        private int[] goodTable;

        /// <summary>
        /// 需要匹配的字符数据
        /// </summary>
        private string patternChars;

        /// <summary>
        /// 需要匹配的字节数据
        /// </summary>
        private IReadOnlyList<byte> patternBytes;
        #endregion

        /// <summary>
        /// 匹配数据的长度
        /// </summary>
        public int PatternLength { get; private set; }


        #region Constructors
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
        public BoyerMoore(IReadOnlyList<byte> pattern)
        {
            SetPattern(pattern);
        }
        /// <summary>
        /// Boyer-Moore (BM) 匹配查找算法
        /// <para>适合大量数据匹配查找，数据量越大，效率越高</para>
        /// </summary>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="isSingleByteOnly">需要匹配查找的数据，是否只是单字节字符</param>
        public BoyerMoore(ref string pattern, bool isSingleByteOnly = true)
        {
            SetPattern(ref pattern, isSingleByteOnly);
        }
        #endregion


        #region Public Functions SetPattern
        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="isSingleByteOnly">需要匹配查找的数据，是否只是单字节字符</param>
        public void SetPattern(ref string pattern, bool isSingleByteOnly = true)
        {
            if (pattern == null || pattern.Length < 1)
                throw new ArgumentNullException("参数 pattern 不能空，长度不能小于 1 ");

            if (offsetTable != null)
                Array.Clear(offsetTable, 0, offsetTable.Length);
            if (goodTable != null)
                Array.Clear(goodTable, 0, goodTable.Length);

            this.patternBytes = null;
            this.patternChars = pattern;
            this.PatternLength = this.patternChars.Length;

            this.offsetTable = GetBadCharacterShift(ref pattern, isSingleByteOnly);
            this.goodTable = GetGoodSuffixShift(ref pattern);
        }
        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        public void SetPattern(IReadOnlyList<byte> pattern)
        {
            if (pattern == null || pattern.Count < 1)
                throw new ArgumentNullException("参数 pattern 不能空，长度不能小于 1 ");

            if (offsetTable != null)
                Array.Clear(offsetTable, 0, offsetTable.Length);
            if(goodTable != null)
                Array.Clear(goodTable, 0, goodTable.Length);

            this.patternChars = null;
            this.patternBytes = pattern;
            this.PatternLength = this.patternBytes.Count;

            this.offsetTable = GetBadCharacterShift(pattern);
            this.goodTable = GetGoodSuffixShift(pattern);
        }
        #endregion


        #region Public Functions Search
        /// <summary>
        /// 在 source 中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示在 source 中没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.Search(ref string, ref string, bool, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="InvalidOperationException">未设置需要匹配的数据</exception>
        /// <exception cref="ArgumentNullException">数据源不能为空，长度不得小于需要匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(ref string source, int start = 0, int end = int.MaxValue)
        {
            if (patternChars == null)
                throw new InvalidOperationException($"未设置需要匹配的数据: {nameof(patternChars)}");
            if (source == null || source.Length < PatternLength)
                throw new ArgumentNullException(nameof(source), "参数不能空，长度不能小于 需要匹配数据的长度 ");
            if (start < 0 || end < 0 || end <= start || end < patternChars.Length || end - start < patternChars.Length)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Length - PatternLength;

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && patternChars[i] == source[index + i]) i--;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);

                if (index > end) break;
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示在 source 中没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.Search(IReadOnlyList{byte}, IReadOnlyList{byte}, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="InvalidOperationException">未设置需要匹配的数据</exception>
        /// <exception cref="ArgumentNullException">数据源不能为空，长度不得小于需要匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(IReadOnlyList<byte> source, int start = 0, int end = int.MaxValue)
        {
            if (patternBytes == null)
                throw new InvalidOperationException($"未设置需要匹配的数据:{nameof(patternBytes)}");
            if (source == null || source.Count < PatternLength)
                throw new ArgumentNullException("参数 source 不能空，长度不能小于 需要匹配数据的长度 ");
            if (start < 0 || end < 0 || end <= start || end < patternBytes.Count || end - start < patternBytes.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && patternBytes[i] == source[index + i]) i--;

                if (i <= 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);

                if (index > end) break;
            }

            return -1;
        }
        #endregion


        #region Public Functions SearchAt
        /// <summary>
        /// 查找第 rCount 次匹配到的位置索引，如果第 rCount 次的匹配不存在，则返回 -1
        /// <para> 相当于 <see cref="SearchAll(ref string, int, int)"/>[rCount - 1]，但不做全面查找；如果 rCount = 1 则与 <see cref="Search(ref string, int, int)"/> 相同 </para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="rCount">匹配到第 rCount 次后结束</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns></returns>
        public int SearchAt(ref string source, int rCount, int start = 0, int end = int.MaxValue)
        {
            int count = 0, index = start;

            do
            {
                index = Search(ref source, index, end);
                if (index == -1) return -1;

                count++;
                index++;
            }
            while (count < rCount);

            return index - 1;
        }
        /// <summary>
        /// 查找第 rCount 次匹配到的位置索引，如果第 rCount 次的匹配不存在，则返回 -1 
        /// <para> 相当于 <see cref="SearchAll(IReadOnlyList{byte}, int, int)"/>[rCount - 1]，但不做全面查找；如果 rCount = 1 则与 <see cref="Search(IReadOnlyList{byte}, int, int)"/> 相同 </para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="rCount">匹配到第 rCount 次后结束</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns></returns>
        public int SearchAt(IReadOnlyList<byte> source, int rCount, int start = 0, int end = int.MaxValue)
        {
            int count = 0, index = start;

            do
            {
                index = Search(source, index, end);
                if (index == -1) return -1;

                count++;
                index++;
            }
            while (count < rCount);

            return index - 1;
        }
        #endregion


        #region Public Functions SearchAll
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.SearchAll(ref string, ref string, bool, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="InvalidOperationException">未设置需要匹配的数据</exception>
        /// <exception cref="ArgumentNullException">数据源不能为空，长度不得小于需要匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public int[] SearchAll(ref string source, int start = 0, int end = int.MaxValue)
        {
            if (patternChars == null)
                throw new InvalidOperationException($"未设置需要匹配的数据: {nameof(patternChars)}");
            if (source == null || source.Length < PatternLength)
                throw new ArgumentNullException(nameof(source), "参数不能为空，且长度不能小于需要匹配数据的长度");
            if (start < 0 || end < 0 || end <= start || end < patternChars.Length || end - start < patternChars.Length)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            List<int> indexs = new List<int>(CAPACITY);
            int maxCompareCount = source.Length - PatternLength;  //最多可比较的次数

            while (index <= maxCompareCount)
            {
                //for (i = PatternLength - 1; i >= 0 && patternChars[i] == source[i + index]; i--) ;
                i = PatternLength - 1;
                while (i >= 0 && patternChars[i] == source[index + i]) i--;

                if (i < 0)
                {
                    indexs.Add(index);
                    index += PatternLength; //goodTable[0];
                }
                else
                {
                    index += Math.Max(offsetTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);
                }

                if (index > end) break;
            }

            return indexs.ToArray();
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.SearchAll(IReadOnlyList{byte}, IReadOnlyList{byte}, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="InvalidOperationException">未设置需要匹配的数据</exception>
        /// <exception cref="ArgumentNullException">数据源不能为空，长度不得小于需要匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public int[] SearchAll(IReadOnlyList<byte> source, int start = 0, int end = int.MaxValue)
        {
            if (patternBytes == null)
                throw new InvalidOperationException($"未设置需要匹配的数据: {nameof(patternBytes)}");
            if (source == null || source.Count < PatternLength)
                throw new ArgumentNullException(nameof(source), "参数不能为空，且长度不能小于需要匹配数据的长度");
            if (start < 0 || end < 0 || end <= start || end < patternBytes.Count || end - start < patternBytes.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            List<int> indexs = new List<int>(CAPACITY);
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                i = PatternLength - 1;
                while (i >= 0 && patternBytes[i] == source[index + i]) i--;
                //for (i = PatternLength - 1; i >= 0 && patternBytes[i] == source[i + index]; i--) ;

                if (i < 0)
                {
                    indexs.Add(index);
                    index += PatternLength; //goodTable[0];
                }
                else
                {
                    index += Math.Max(offsetTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);
                }

                if (index > end) break;
            }

            return indexs.ToArray();
        }
        #endregion



        #region Internal Debug
        /// <summary>
        /// 调试输出匹配过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="index"></param>
        internal static void DebugTrace<T>(IReadOnlyList<T> source, IReadOnlyList<T> pattern, int index)
        {
            Console.WriteLine("-------------------------> {0}", index);
            Console.WriteLine("{0}", string.Join("", source));
            Console.WriteLine("{0}", string.Join("", pattern).PadLeft(index + pattern.Count, '-'));
        }
        #endregion

        #region Static Internal Functions GetBadCharacterShift
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
        /// </summary>
        /// <param name="pattern">需要匹配的数据内容</param>
        /// <param name="isSingleByteOnly">需要匹配查找的数据，是否只是单字节字符</param>
        /// <returns></returns>
        internal static int[] GetBadCharacterShift(ref string pattern, bool isSingleByteOnly = true)
        {
            int i = 0;
            int length = (isSingleByteOnly ? 0xFF : 0xFFFF) + 1;
            int[] badTable = new int[length];

            for (i = 0; i < length; i++)
                badTable[i] = pattern.Length;

            for (i = 0; i < pattern.Length; i++)
                badTable[pattern[i]] = pattern.Length - 1 - i;

            return badTable;
        }
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
        /// </summary>
        /// <param name="pattern">需要匹配的数据内容</param>
        /// <returns></returns>
        internal static int[] GetBadCharacterShift(IReadOnlyList<byte> pattern)
        {
            int i = 0;
            int[] badTable = new int[0xFF + 1];

            for (i = 0; i < badTable.Length; i++)
                badTable[i] = pattern.Count;

            for (i = 0; i < pattern.Count; i++)
                badTable[pattern[i]] = pattern.Count - 1 - i;

            return badTable;
        }

        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)，返回的是字典类型数据
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        internal static Dictionary<TKey, int> GetBadCharacterShift<TKey>(IReadOnlyList<TKey> pattern)
        {
            Dictionary<TKey, int> offsetTable = new Dictionary<TKey, int>(pattern.Count + 8);
            for (int i = 0; i < pattern.Count; i++)
                offsetTable[pattern[i]] = pattern.Count - 1 - i;

            return offsetTable;
        }
        #endregion

        #region Static Internal Functions GetGoodSuffixShift
        /// <summary>
        /// 获取有效数据标记表 (好后缀 Good Suffix Heuristic)
        /// </summary>
        /// <returns></returns>
        internal static int[] GetGoodSuffixShift(ref string pattern)
        {
            int i, j = 0;
            int patternLength = pattern.Length;

            int[] goodSuffixShifts = new int[patternLength];
            int[] suffixLengthArray = new int[patternLength];

            #region Get Good Suffix Length Array 后缀长度数组
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
        /// 获取有效数据标记表 (好后缀 Good Suffix Heuristic)
        /// </summary>
        /// <returns></returns>
        internal static int[] GetGoodSuffixShift(IReadOnlyList<byte> pattern)
        {
            int i, j = 0;
            int patternLength = pattern.Count;

            int[] goodSuffixShifts = new int[patternLength];
            int[] suffixLengthArray = new int[patternLength];

            #region Get Good Suffix Length Array 后缀长度数组
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

            for (i = 0; i < patternLength; ++i)
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

        #endregion



        #region Static Public Functions Search
        /// <summary>
        /// 在 source 中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示在 source 中没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="isSingleByteOnly">需要匹配查找的数据，是否只是单字节字符</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>        
        public static int Search(ref string source, ref string pattern, bool isSingleByteOnly = true, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Length < 1) return -1;
            if (source == null || source.Length < pattern.Length) return -1;
            if (start < 0 || end < 0 || end <= start || end < pattern.Length || end - start < pattern.Length) return -1;

            int i, index = start;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;        //最后一个匹配数据的位置
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            int[] offsetTable = GetBadCharacterShift(ref pattern, isSingleByteOnly);

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while ( i >= 0 && pattern[i] == source[index + i]) i--;
                //for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                if (index > end) break;
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示在 source 中没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public static int Search(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Count < 1) return -1;
            if (source == null || source.Count < pattern.Count) return -1;
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count) return -1;

            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            
            int i, index = start;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            int[] offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                //Console.WriteLine("------------------------->{0}", index);
                //Console.WriteLine("{0}", string.Join("", source));
                //Console.WriteLine("{0}", string.Join("", pattern).PadLeft(index + pattern.Count, '-'));
                //Console.WriteLine("-------------------------E.{0}", index);

                i = lastPatternPosition;
                while (i >= 0 && pattern[i] == source[index + i]) i--;
                //for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                if (index > end) break;
            }

            return -1;
        }
        #endregion

        #region Static Public Functions SearchAll
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="isSingleByteOnly">需要匹配查找的数据，是否只是单字节字符</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static int[] SearchAll(ref string source, ref string pattern, bool isSingleByteOnly = true, int start = 0, int end = int.MaxValue)
        {
            List<int> indexs = new List<int>(CAPACITY);
            if (pattern == null || pattern.Length < 1) return indexs.ToArray();
            if (source == null || source.Length < pattern.Length) return indexs.ToArray();
            if (start < 0 || end < 0 || end <= start || end < pattern.Length || end - start < pattern.Length) return indexs.ToArray();

            int i, index = start;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;        //最后一个匹配数据的位置
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数
            int[] offsetTable = GetBadCharacterShift(ref pattern, isSingleByteOnly);

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && pattern[i] == source[index + i]) i--;
                //for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i <= 0)
                {
                    indexs.Add(index);
                    index += patternLength;
                }
                else
                    index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                if (index > end) break;
            }

            return indexs.ToArray();
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static int[] SearchAll(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0, int end = int.MaxValue)
        {
            List<int> indexs = new List<int>(Math.Min(source.Count / pattern.Count, CAPACITY));
            if (pattern == null || pattern.Count < 1) return indexs.ToArray();
            if (source == null || source.Count < pattern.Count) return indexs.ToArray();
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count) return indexs.ToArray();

            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int maxCompareCount = sourceLength - patternLength;

            int i, index = start;
            int lastPatternPosition = patternLength - 1;
            int[] offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && pattern[i] == source[index + i]) i--;
                //for (i = lastPatternPosition; i >= 0 && pattern[i] == source[index + i]; i--) ;

                if (i <= 0)
                {
                    indexs.Add(index);
                    index += patternLength;
                }
                else
                    index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                if (index > end) break;
            }

            return indexs.ToArray();
        }

        #endregion

    }

    /// <summary>
    /// <see cref="BoyerMoore"/> 的泛型版本，性能？？？不好说，内部做了太多数据转换，好像有问题，在设计阶段就发现了
    /// </summary>
    /// <typeparam name="T">基本数据类型，基类型占用字节大小最好不要超过 2 个字节</typeparam>
    public sealed class BoyerMoore<T> where T : struct
    {
        /// <summary>
        /// 基类型占用字节大小
        /// </summary>
        public static readonly int TypeSize = 1;

        /// <summary>
        /// 匹配数据的长度
        /// </summary>
        public int PatternLength { get; private set; }

        /// <summary>
        /// 需要匹配的数据
        /// </summary>
        private IReadOnlyList<T> pattern;

        /// <summary>
        /// badTable
        /// </summary>
        public Dictionary<T, int> badTable;

        static BoyerMoore()
        {
            TypeSize = Marshal.SizeOf(typeof(T));
            Console.WriteLine("Size::{0}", TypeSize);

            if (TypeSize > 4)
                throw new Exception($"暂时只支持 32 位基类型数据处理");
        }

        public BoyerMoore()
        {            
        }

        public BoyerMoore(IReadOnlyList<T> pattern)
        {
            SetPattern(pattern);
        }

        public void SetPattern(IReadOnlyList<T> pattern)
        {
            this.pattern = pattern;
            PatternLength = pattern.Count;

            badTable = BoyerMoore.GetBadCharacterShift<T>(pattern);

            //badTable = new Dictionary<T, int>(pattern.Count + 8);
            //for (int i = 0; i < pattern.Count; i++)
            //{
                //if (badTable.ContainsKey(pattern[i]))
                    //badTable[pattern[i]] = pattern.Count - 1 - i;
                //else
               //     badTable.Add(pattern[i], pattern.Count - 1 - i);
            //}
        }

        public int Search(IReadOnlyList<T> source, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null)
                throw new InvalidOperationException($"未设置需要匹配的数据:{nameof(pattern)}");
            if (source == null || source.Count < PatternLength)
                throw new ArgumentNullException("参数 source 不能空，长度不能小于 需要匹配数据的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, value, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                BoyerMoore.DebugTrace<T>(source, pattern, index);

                i = lastPatternPosition;
                while (i >= 0 && pattern[i].Equals(source[index + i])) i--;
                //for(i = lastPatternPosition; i >= 0 && pattern[i].Equals(source[index + i]); i -- )

                if (i < 0) return index;
                if (badTable.TryGetValue(source[index + i], out value))
                    index += Math.Max(value - PatternLength + 1 + i, 1);
                else
                    index += Math.Max(1 + i, 1);

                //index += Math.Max((badTable.ContainsKey(source[index + i]) ? badTable[source[index + i]] : PatternLength) - PatternLength + 1 + i, 1);

                if (index > end) break;
            }

            return -1;
        }

        public int SearchAt(IReadOnlyList<T> source, int rCount, int start = 0, int end = int.MaxValue)
        {
            return -1;
        }

        public int[] SearchAll(IReadOnlyList<T> source, int start = 0, int end = int.MaxValue)
        {
            List<int> indexs = new List<int>(Math.Min(source.Count / pattern.Count, BoyerMoore.CAPACITY));
            if (pattern == null)
                throw new InvalidOperationException($"未设置需要匹配的数据:{nameof(pattern)}");
            if (source == null || source.Count < PatternLength)
                throw new ArgumentNullException("参数 source 不能空，长度不能小于 需要匹配数据的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, value, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                BoyerMoore.DebugTrace<T>(source, pattern, index);

                i = lastPatternPosition;
                while (i >= 0 && pattern[i].Equals(source[index + i])) i--;
                //for(i = lastPatternPosition; i >= 0 && pattern[i].Equals(source[index + i]); i -- )

                if (i < 0)
                {
                    indexs.Add(index);
                    index += PatternLength;
                    Console.WriteLine("Find");
                }
                else
                {
                    if(badTable.TryGetValue(source[index + i], out value))
                        index += Math.Max(value - PatternLength + 1 + i, 1);
                    else
                        index += Math.Max(1 + i, 1);

                    //index += Math.Max((badTable.ContainsKey(source[index + i]) ? badTable[source[index + i]] : PatternLength) - PatternLength + 1 + i, 1);
                }

                if (index > end) break;
            }

            return indexs.ToArray();
        }


        /// <summary>
        /// 将struct类型转换为byte[]
        /// </summary>
        public static byte[] StructToBytes(T structure, int size)
        {
            IntPtr buffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);

                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in StructToBytes ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


    }

}
