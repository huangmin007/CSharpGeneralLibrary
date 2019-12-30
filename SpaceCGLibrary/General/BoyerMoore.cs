using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// Boyer-Moore 算法实现。该算法查找原始 单字节(Max 0xFF)集合 效率非常非常高；如果匹配的 pattern 集合长度为 1 或很少，也不建议使用该方法
    /// <para>字符匹配查找：类搜索方法 (Search*) 使用的是数组，类静态搜索方法 (BoyerMoore.Search*) 参数 tSize 大于 0xFF 时使用字典，小于 0xFF 时使用数组</para>
    /// <para>类查找函数会检查参数，但不会抛出异常，只会返回有或无，适合在 for, while, 等循环体或重复搜索 对性能要求较高的环境中使用；
    ///     静态查找函数会检查参数类型并抛出异常，不生成使用好后缀表，只使用坏字符表。
    /// </para>
    /// <para>参考：https://baike.baidu.com/item/Boyer-%20Moore%E7%AE%97%E6%B3%95/16548374?fr=aladdin </para>
    /// <para>参考：https://www.cnblogs.com/gaochundong/p/boyer_moore_string_matching_algorithm.html </para>
    /// </summary>
    public sealed class BoyerMoore
    {
        /// <summary>
        /// 返回的索引集合列表，初使最大可以存储的元素数量
        /// </summary>
        internal const int CAPACITY = 256;

        /// <summary>
        /// 匹配数据的长度
        /// </summary>
        public int PatternLength { get; private set; }

        #region Variables
        /// <summary>
        /// 能过 <see cref="GetBadCharacterShift(IReadOnlyList{byte})"/> 生成的 坏字符表 (Bad Character Heuristic)
        /// </summary>
        private int[] badTable;
        /// <summary>
        /// 通过 <see cref="GetGoodSuffixShift{T}(IReadOnlyList{T})"/> 生成的 好后缀表 (Good Suffix Heuristic)
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
            ResetPattern(pattern);
        }
        /// <summary>
        /// Boyer-Moore (BM) 匹配查找算法
        /// <para>适合大量数据匹配查找，数据量越大，效率越高</para>
        /// </summary>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="tSize">坏字符 (Bad Character Heuristic) 表的大小，取决于字符 (Unicode 字符的16位值序列) 的最大值，如果是英文和符号字符的全集，是 0xFF 大小；如果是中文字符的全集就是 0xFFFF 大小；
        ///     <para>建议全英文符号字符集可直接设置为 0xFF 大小；中文字符两种方案：1.取中文字符的最大值 2.使用中文字符字典；如果使用中文字符全集，将会达到 0xFFFF 大小的数组</para>
        /// </param>
        public BoyerMoore(ref string pattern, uint tSize = 0xFF)
        {
            ResetPattern(ref pattern, tSize);
        }
        ~BoyerMoore()
        {
            ClearParams();
        }

        /// <summary>
        /// 清理一些变量和数组
        /// </summary>
        private void ClearParams()
        {
            if (badTable != null)
                Array.Clear(badTable, 0, badTable.Length);
            if (goodTable != null)
                Array.Clear(goodTable, 0, goodTable.Length);

            badTable = null;
            goodTable = null;

            PatternLength = -1;

            patternChars = null;
            patternBytes = null;
        }
        #endregion

        #region Public Functions ResetPattern
        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="tSize">坏字符 (Bad Character Heuristic) 表的大小，取决于字符 (Unicode 字符的16位值序列) 的最大值，如果是英文和符号字符的全集，是 0xFF 大小；如果是中文字符的全集就是 0xFFFF 大小；
        ///     <para>建议全英文符号字符集可直接设置为 0xFF 大小；中文字符两种方案：1.取中文字符的最大值 2.使用中文字符字典；如果使用中文字符全集，将会达到 0xFFFF 大小的数组</para>
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public void ResetPattern(ref string pattern, uint tSize = 0xFF)
        {
            if (pattern == null || pattern.Length < 1)
                throw new ArgumentNullException("参数 pattern 不能空，长度不能小于 1 ");

            ClearParams();

            this.patternBytes = null;
            this.patternChars = pattern;
            this.PatternLength = this.patternChars.Length;

            try
            {
                this.badTable = GetBadCharacterShift(ref pattern, tSize);
                this.goodTable = GetGoodSuffixShift(ref pattern);
            }
            catch (Exception ex)
            {
                ClearParams();
                throw new ArgumentException($" {nameof(tSize)} 参数错误，创建 (坏字符 Bad Character Heuristic) 表失败，查找的数据内容与表大小不匹配", ex);
            }
        }
        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        public void ResetPattern(IReadOnlyList<byte> pattern)
        {
            if (pattern == null || pattern.Count < 1)
                throw new ArgumentNullException("参数 pattern 不能空，长度不能小于 1 ");

            ClearParams();

            this.patternChars = null;
            this.patternBytes = pattern;
            this.PatternLength = this.patternBytes.Count;

            this.badTable = GetBadCharacterShift(pattern);
            this.goodTable = GetGoodSuffixShift(pattern);
        }
        #endregion


        #region Public Functions Search
        /// <summary>
        /// 在 source 中的 start 到 end 的位置中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示跟据参数条件没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.Search(ref string, ref string, uint, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(ref string source, int start = 0, int end = int.MaxValue)
        {
            if (patternChars == null || source == null || source.Length < PatternLength) return -1;
            if (start < 0 || end < 0 || end <= start || end < patternChars.Length || end - start < patternChars.Length) return -1;

            int i, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Length - PatternLength;

            while (index <= maxCompareCount)
            {
                //DebugTrace(ref source, ref patternChars, index);

                i = lastPatternPosition;
                while (i >= 0 && patternChars[i] == source[index + i]) i--;

                if (i < 0) return index;
                index += Math.Max(badTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);

                if (index > end) break;
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中的 start 到 end 的位置中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示跟据参数条件没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.Search(IReadOnlyList{byte}, IReadOnlyList{byte}, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(IReadOnlyList<byte> source, int start = 0, int end = int.MaxValue)
        {
            if (patternBytes == null || source == null || source.Count < PatternLength) return -1;
            if (start < 0 || end < 0 || end <= start || end < patternBytes.Count || end - start < patternBytes.Count) return -1;

            int i, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                //DebugTrace<byte>(source, patternBytes, index);

                i = lastPatternPosition;
                while (i >= 0 && patternBytes[i] == source[index + i]) i--;

                if (i < 0) return index;
                index += Math.Max(badTable[source[index + i]] - PatternLength + 1 + i, goodTable[i]);

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
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.SearchAll(ref string, ref string, uint, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public int[] SearchAll(ref string source, int start = 0, int end = int.MaxValue)
        {
            int index = start;
            List<int> indexs = new List<int>(Math.Min(source.Length / patternChars.Length, BoyerMoore.CAPACITY));

            do
            {
                index = Search(ref source, index, end);
                if (index != -1)
                {
                    indexs.Add(index);
                    index += patternChars.Length;
                }
            }
            while (index > 0);

            return indexs.ToArray();
        }
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// <para>Boyer-Moore 算法实现，与静态方法 <see cref="BoyerMoore.SearchAll(IReadOnlyList{byte}, IReadOnlyList{byte}, int, int)"/> 不一样，该方法同时生成并使用了 坏字符（Bad Character Heuristic） 和 好后缀（Good Suffix Heuristic）表</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public int[] SearchAll(IReadOnlyList<byte> source, int start = 0, int end = int.MaxValue)
        {
            int index = start;
            List<int> indexs = new List<int>(Math.Min(source.Count / patternBytes.Count, BoyerMoore.CAPACITY));

            do
            {
                index = Search(source, index, end);
                if (index != -1)
                {
                    indexs.Add(index);
                    index += patternBytes.Count;
                }
            }
            while (index > 0);

            return indexs.ToArray();
        }
        #endregion


        #region Internal Debug Trace
        /// <summary>
        /// 调试输出匹配过程
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="index"></param>
        internal static void DebugTrace(ref string source, ref string pattern, int index)
        {
            Console.WriteLine("-------------------------> {0}", index);
            Console.WriteLine("{0}", string.Join("", source));
            Console.WriteLine("{0}", string.Join("", pattern).PadLeft(index + pattern.Length, '-'));
        }
        /// <summary>
        /// 调试输出匹配过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="index"></param>
        internal static void DebugTrace<T>(IReadOnlyList<T> source, IReadOnlyList<T> pattern, int index)
        {
            Console.WriteLine("--> index:{0}", index);
            Console.WriteLine("{0}", string.Join(",", source));
            //Console.WriteLine("{0}", string.Join(",", pattern).PadLeft(index + pattern.Count, '-'));
        }
        #endregion

        #region Static Internal Functions GetBadCharacterShift        
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)
        /// </summary>
        /// <param name="pattern">需要匹配的数据内容</param>
        /// <param name="size">坏字符 (Bad Character Heuristic) 表的大小，取决于字符 (Unicode 字符的16位值序列) 的最大值，如果是英文和符号字符的全集，是 0xFF 大小；如果是中文字符的全集就是 0xFFFF 大小；
        ///     <para>建议全英文符号字符集可直接设置为 0xFF 大小；中文字符两种方案：1.取中文字符的最大值 2.使用中文字符字典；如果使用中文字符全集，将会达到 0xFFFF 大小的数组</para>
        /// </param>
        /// <returns></returns>
        internal static int[] GetBadCharacterShift(ref string pattern, uint size = 0xFF)
        {
            int i = 0;
            int[] badTable = new int[size + 1];

            for (i = 0; i < badTable.Length; i++)
                badTable[i] = pattern.Length;

            for (i = 0; i < pattern.Length; i++)
                badTable[pattern[i]] = pattern.Length - i - 1;

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
                badTable[pattern[i]] = pattern.Count - i - 1;

            return badTable;
        }
        /// <summary>
        /// 获取无效数据标记表 (坏字符 Bad Character Heuristic)，返回的是字典类型数据
        /// </summary>
        /// <typeparam name="T">键类型</typeparam>
        /// <param name="pattern"></param>
        /// <param name="useThreadSafe">是否使用 可由多个线程同时访问的 键/值对 的 线程安全集合</param>
        /// <returns></returns>
        internal static IDictionary<T, int> GetBadCharacterShift<T>(IReadOnlyList<T> pattern, bool useThreadSafe = false)
        {
            IDictionary<T, int> badTable;
            if (useThreadSafe)
                badTable = new ConcurrentDictionary<T, int>(2, pattern.Count + 8);
            else
                badTable = new Dictionary<T, int>(pattern.Count + 8);

            for (int i = 0; i < pattern.Count; i++)
                badTable[pattern[i]] = pattern.Count - i - 1;

            return badTable;
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

                    while (lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - j])
                        --lastPatternPosition;

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
        /// <summary>
        /// 获取有效数据标记表 (好后缀 Good Suffix Heuristic)
        /// </summary>
        /// <returns></returns>
        internal static int[] GetGoodSuffixShift<T>(IReadOnlyList<T> pattern)
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

                    //while (lastPatternPosition >= 0 && pattern[lastPatternPosition] == pattern[lastPatternPosition + patternLength - 1 - j])
                    //    --lastPatternPosition;

                    while (lastPatternPosition >= 0 && pattern[lastPatternPosition].Equals(pattern[lastPatternPosition + patternLength - 1 - j]))
                        --lastPatternPosition;

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
        /// 在 source 中的 start 到 end 的位置中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示跟据参数条件没匹配到；tSize 大于 0xFF 时会使用字典做为表
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="tSize">坏字符 (Bad Character Heuristic) 表的大小，取决于字符 (Unicode 字符的16位值序列) 的最大值，如果是英文和符号字符的全集，是 0xFF 大小；如果是中文字符的全集就是 0xFFFF 大小；
        ///     <para>建议全英文符号字符集可直接设置为 0xFF 大小；中文字符两种方案：1.取中文字符的最大值 2.使用中文字符字典；如果使用中文字符全集，将会达到 0xFFFF 大小的数组</para>
        /// </param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="ArgumentNullException">空参数错误</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 -1 表示没匹配到 </returns>        
        public static int Search(ref string source, ref string pattern, uint tSize = 0xFF, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Length <= 0)
                throw new ArgumentNullException(nameof(pattern), "参数不能为空，或长度不能为 0 ");
            if (source == null || source.Length < pattern.Length)
                throw new ArgumentNullException(nameof(source), $"参数不能为空，或长度不能小于 {nameof(pattern)} 的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Length || end - start < pattern.Length)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;        //最后一个匹配数据的位置
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数

            if (tSize <= 0xFF)
            {
                int[] offsetTable = GetBadCharacterShift(ref pattern, tSize);
                while (index <= maxCompareCount)
                {
                    i = lastPatternPosition;
                    while (i >= 0 && pattern[i] == source[index + i]) i--;

                    if (i < 0) return index;
                    index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                    if (index > end) break;
                }
            }
            else
            {
                int value;
                bool hasValue;
                IDictionary<char, int> offsetTable = GetBadCharacterShift(pattern.ToCharArray());

                while (index <= maxCompareCount)
                {
                    i = lastPatternPosition;
                    while (i >= 0 && pattern[i] == source[index + i]) i--;

                    if (i < 0) return index;

                    hasValue = offsetTable.TryGetValue(source[index + i], out value);
                    index += Math.Max(hasValue ? value - patternLength + i + 1 : i + 1, 1);

                    if (index > end) break;
                }
            }

            return -1;
        }
        /// <summary>
        /// 在 source 中的 start 到 end 的位置中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示跟据参数条件没匹配到
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回 -1, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="ArgumentNullException">空参数错误</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public static int Search(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Count <= 0)
                throw new ArgumentNullException(nameof(pattern), "参数不能为空，或长度不能为 0 ");
            if (source == null || source.Count < pattern.Count)
                throw new ArgumentNullException(nameof(source), $"参数不能为空，或长度不能小于 {nameof(pattern)} 的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int maxCompareCount = sourceLength - patternLength;  //最多可比较的次数

            int i, index = start;
            int lastPatternPosition = patternLength - 1;        //最后一个匹配数据的位置
            int[] offsetTable = GetBadCharacterShift(pattern);

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && pattern[i] == source[index + i]) i--;

                if (i < 0) return index;
                index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                if (index > end) break;
            }

            return -1;
        }
        #endregion

        #region Static Public Functions SearchAll
        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到；tSize 大于 0xFF 时会使用字典做为表
        /// <para>Boyer-Moore-Horspool 算法实现，是 Boyer-Moore 算法 的简化版本，只用到了 坏字符（Bad Character Heuristic）表</para>
        /// <para>注意：参数错误 (参数为空，或长度小于 1，或 source 长度小于 pattern 的长度) 也会返回空集合, 而不抛出异常信息。</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pattern">需要匹配的数据</param>
        /// <param name="tSize">坏字符 (Bad Character Heuristic) 表的大小，取决于字符 (Unicode 字符的16位值序列) 的最大值，如果是英文和符号字符的全集，是 0xFF 大小；如果是中文字符的全集就是 0xFFFF 大小；
        ///     <para>建议全英文符号字符集可直接设置为 0xFF 大小；中文字符两种方案：1.取中文字符的最大值 2.使用中文字符字典；如果使用中文字符全集，将会达到 0xFFFF 大小的数组</para>
        /// </param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="ArgumentNullException">空参数错误</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static int[] SearchAll(ref string source, ref string pattern, uint tSize = 0xFF, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Length <= 0)
                throw new ArgumentNullException(nameof(pattern), "参数不能为空，或长度不能为 0 ");
            if (source == null || source.Length < pattern.Length)
                throw new ArgumentNullException(nameof(source), $"参数不能为空，或长度不能小于 {nameof(pattern)} 的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Length || end - start < pattern.Length)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            int sourceLength = source.Length;
            int patternLength = pattern.Length;
            int lastPatternPosition = pattern.Length - 1;
            int maxCompareCount = sourceLength - patternLength;
            List<int> indexs = new List<int>(Math.Min(source.Length / pattern.Length, CAPACITY));

            if (tSize <= 0xFF)
            {
                int[] offsetTable = GetBadCharacterShift(ref pattern, tSize);
                while (index <= maxCompareCount)
                {
                    i = lastPatternPosition;
                    while (i >= 0 && pattern[i] == source[index + i]) i--;

                    if (i <= 0)
                    {
                        indexs.Add(index);
                        index += patternLength;
                    }
                    else
                        index += Math.Max(offsetTable[source[index + i]] - patternLength + 1 + i, 1);

                    if (index > end) break;
                }
            }
            else
            {
                int value;
                bool hasValue;
                IDictionary<char, int> offsetTable = GetBadCharacterShift(pattern.ToCharArray());

                while (index <= maxCompareCount)
                {
                    i = lastPatternPosition;
                    while (i >= 0 && pattern[i] == source[index + i]) i--;

                    if (i <= 0)
                    {
                        indexs.Add(index);
                        index += patternLength;
                    }
                    else
                    {
                        hasValue = offsetTable.TryGetValue(source[index + i], out value);
                        index += Math.Max(hasValue ? value - patternLength + i + 1 : i + 1, 1);
                    }

                    if (index > end) break;
                }
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
        /// <exception cref="ArgumentNullException">空参数错误</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public static int[] SearchAll(IReadOnlyList<byte> source, IReadOnlyList<byte> pattern, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || pattern.Count <= 0)
                throw new ArgumentNullException(nameof(pattern), "参数不能为空，或长度不能为 0 ");
            if (source == null || source.Count < pattern.Count)
                throw new ArgumentNullException(nameof(source), $"参数不能为空，或长度不能小于 {nameof(pattern)} 的长度 ");
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count)
                throw new ArgumentOutOfRangeException($"参数 {nameof(start)}, {nameof(end)} 设置错误，超出查找匹配界限范围");

            int i, index = start;
            int sourceLength = source.Count;
            int patternLength = pattern.Count;
            int lastPatternPosition = patternLength - 1;
            int maxCompareCount = sourceLength - patternLength;

            int[] offsetTable = GetBadCharacterShift(pattern);
            List<int> indexs = new List<int>(Math.Min(source.Count / pattern.Count, CAPACITY));

            while (index <= maxCompareCount)
            {
                i = lastPatternPosition;
                while (i >= 0 && pattern[i] == source[index + i]) i--;

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
    /// Boyer-Moore 算法实现，<see cref="BoyerMoore"/> 的泛型版本，泛型版本使用的是字典作为 好字符 表，而不是使用数组
    /// <para>适合 集合数据 与 集合数据 的查找，在 source 集合中查找 pattern 集合相同位置、相同数据的索引；与 <see cref="BoyerMoore"/> 类一样 Search*() 方法不会抛出异常信息，只会返回结果</para>
    /// <para>集合数据的元素对比 是使用 <see cref="object.Equals(object)"/> 的方法，所以自定义基数据类型(比如自定义结构数据类型)，需要实现应该方法</para>
    /// </summary>
    /// <typeparam name="T">基本数据类型，自定义基数据类型(比如自定义结构数据类型)，需要实现 <see cref="object.Equals(object)"/> 方法</typeparam>
    public sealed class BoyerMoore<T> where T : struct
    {
        /// <summary>
        /// 匹配数据的长度
        /// </summary>
        public int PatternLength { get; private set; }

        /// <summary>
        /// 需要匹配的数据
        /// </summary>
        private IReadOnlyList<T> pattern;

        /// <summary>
        /// 通过 <see cref="BoyerMoore.GetBadCharacterShift{T}(IReadOnlyList{T}, bool)"/> 生成的 坏字符表 (Bad Character Heuristic)
        /// </summary>
        private IDictionary<T, int> badTable;

        /// <summary>
        /// 通过 <see cref="BoyerMoore.GetGoodSuffixShift{T}(IReadOnlyList{T})"/> 生成的 好后缀表 (Good Suffix Heuristic)
        /// </summary>
        private int[] goodTable;
        /// <summary>
        /// 是否使用 可由多个线程同时访问的 键/值对 的 线程安全集合
        /// </summary>
        private bool useThreadSafe = false;

        /// <summary>
        /// BoyerMoore 泛型版本
        /// </summary>
        public BoyerMoore()
        { 
        }
        ~BoyerMoore()
        {
            ClearParams();
        }
        /// <summary>
        /// 清理参数
        /// </summary>
        private void ClearParams()
        {
            if (badTable != null) badTable.Clear();
            badTable = null;

            if (goodTable != null) Array.Clear(goodTable, 0, goodTable.Length);
            goodTable = null;

            pattern = null;
            PatternLength = -1;
        }

        /// <summary>
        /// BoyerMoore 泛型版本
        /// </summary>
        public BoyerMoore(IReadOnlyList<T> pattern)
        {
            ResetPattern(pattern);
        }

        /// <summary>
        /// 设置需要匹配数据
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="useThreadSafe">是否使用 可由多个线程同时访问的 键/值对 的 线程安全集合</param>
        public void ResetPattern(IReadOnlyList<T> pattern, bool useThreadSafe = false)
        {
            if(pattern == null || pattern.Count <= 0)
                throw new ArgumentNullException(nameof(pattern), "参数不能空，或需要匹配的数据不能太小");

            ClearParams();

            this.pattern = pattern;
            this.PatternLength = pattern.Count;
            this.useThreadSafe = useThreadSafe;

            goodTable = BoyerMoore.GetGoodSuffixShift(pattern);
            badTable = BoyerMoore.GetBadCharacterShift(pattern, useThreadSafe);
        }

        /// <summary>
        /// 在 source 中的 start 到 end 的位置中查找匹配 pattern 第一次出现的位置并返回，如果返回 -1 表示跟据参数条件没匹配到
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns> 返回 -1 表示没匹配到 </returns>
        public int Search(IReadOnlyList<T> source, int start = 0, int end = int.MaxValue)
        {
            if (pattern == null || source == null || source.Count < PatternLength) return -1;
            if (start < 0 || end < 0 || end <= start || end < pattern.Count || end - start < pattern.Count) return -1;

            bool hasValue;
            int i, value, index = start;
            int lastPatternPosition = PatternLength - 1;
            int maxCompareCount = source.Count - PatternLength;

            while (index <= maxCompareCount)
            {
                //BoyerMoore.DebugTrace<T>(source, pattern, index);

                i = lastPatternPosition;
                while (i >= 0 && pattern[i].Equals(source[index + i])) i--;

                if (i < 0) return index;

                hasValue = badTable.TryGetValue(source[index + i], out value);
                index += Math.Max(hasValue ? value - PatternLength + i + 1 : i + 1, goodTable[i]); // : 1);

                if (index > end) break;
            }

            return -1;
        }

        /// <summary>
        /// 查找第 rCount 次匹配到的位置索引，如果第 rCount 次的匹配不存在，则返回 -1 
        /// <para> 相当于 <see cref="SearchAll(IReadOnlyList{T}, int, int)"/>[rCount - 1]，但不做全面查找；如果 rCount = 1 则与 <see cref="Search(IReadOnlyList{T}, int, int)"/> 相同 </para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="rCount">匹配到第 rCount 次后结束</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <returns></returns>
        public int SearchAt(IReadOnlyList<T> source, int rCount, int start = 0, int end = int.MaxValue)
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

        /// <summary>
        /// 在 source 中查找匹配 pattern 的所有位置的集合，返回 空集合 表示没匹配到
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="start">从 source 指定的起始位置开始匹配查找</param>
        /// <param name="end">到 source 指定的结束位置停止匹配查找</param>
        /// <exception cref="InvalidOperationException">未设置需要匹配的数据</exception>
        /// <exception cref="ArgumentNullException">数据源不能为空，长度不得小于需要匹配数据长度</exception>
        /// <exception cref="ArgumentOutOfRangeException">起始结束匹配位置设置错误，超出查找匹配界限范围</exception>
        /// <returns> 返回 空集合 表示没匹配到 </returns>
        public int[] SearchAll(IReadOnlyList<T> source, int start = 0, int end = int.MaxValue)
        {
            int index = start;
            List<int> indexs = new List<int>(Math.Min(source.Count / pattern.Count, BoyerMoore.CAPACITY));

            do
            {
                index = Search(source, index, end);
                if (index != -1)
                {
                    indexs.Add(index);
                    index += pattern.Count;
                }
            }
            while (index > 0);

            return indexs.ToArray();
        }

    }
}
