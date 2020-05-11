using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCG.Generic
{
    /// <summary>
    /// 【抽象类（基类型：<see cref="byte"/>）】根据 数据起始标记 和 数据终止标记 对数据模式分析的适配器 抽象类
    /// <para>数据模式：{ (数据起始标记) + [动态数据主体] + (数据终止标记) }</para>
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class BetweenAndDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
    {
        /// <summary>
        /// Start Boyer Moore
        /// </summary>
        protected readonly BoyerMoore StartBoyerMoore;

        /// <summary>
        /// End Boyer Moore
        /// </summary>
        protected readonly BoyerMoore EndBoyerMoore;

        /// <summary>
        /// 根据 数据起始标记 和 数据终止标记 对数据模式分析的适配器 抽象类
        /// <para>数据模式：{ (数据起始标记) + [动态数据主体] + (数据终止标记) }</para>
        /// </summary>
        /// <param name="startBytes">数据起始字节</param>
        /// <param name="endBytes">数据终止字节</param>
        protected BetweenAndDataAnalysePattern(IReadOnlyList<byte> startBytes, IReadOnlyList<byte> endBytes)
        {
            if (endBytes == null || endBytes.Count == 0) throw new ArgumentNullException(nameof(endBytes), "参数不能为空，长度不能为 0");
            if (startBytes == null || startBytes.Count == 0) throw new ArgumentNullException(nameof(startBytes), "参数不能为空，长度不能为 0");

            EndBoyerMoore = new BoyerMoore(endBytes);
            StartBoyerMoore = new BoyerMoore(startBytes);
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
        {
            if (key == null || data == null || analyseResultHandler == null) return false;
            Channel<TChannelKey, byte> channel = GetChannel(key);
            if (channel == null) return false;

            channel.AddRange(data);

            // start index - end index
            while(true)
            {
                int start = StartBoyerMoore.Search(channel.Cache, channel.Offset);
                if (start < 0) break;

                start += StartBoyerMoore.PatternLength;
                int end = EndBoyerMoore.Search(channel.Cache, start);
                if (end < 0) break;

                int bodySize = end - start;
                var bodyBytes = channel.GetRange(start, bodySize);
                TResultType result = ConvertResultType(bodyBytes);
                
                bool handled = analyseResultHandler.Invoke(key, result);
                if (handled)
                    channel.RemoveRange(channel.Offset, bodySize + EndBoyerMoore.PatternLength);
                else
                    channel.Offset = end + EndBoyerMoore.PatternLength;
            }

            channel.CheckOverflow();

            return true;
        }

        /// <summary>
        /// 通道数据块转换数据类型
        /// </summary>
        /// <param name="bodyBytes">数据主体字节</param>
        /// <returns></returns>
        protected virtual TResultType ConvertResultType(List<byte> bodyBytes)
        {
            return default;
        }
    }

    /// <summary>
    /// 【返回 byte[] 】根据 数据起始标记 和 数据终止标记 对数据模式分析的适配器
    /// <para>数据模式：{ (数据起始标记) + [动态数据主体] + (数据终止标记) }</para>
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    public class BetweenAndDataAnalyse<TChannelKey> : BetweenAndDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 根据 数据起始标记 和 数据终止标记 对数据模式分析的适配器
        /// <para>数据模式：{ (数据起始标记) + [动态数据主体] + (数据终止标记) }</para>
        /// </summary>
        /// <param name="startBytes">数据起始字节</param>
        /// <param name="endBytes">数据终止字节</param>
        public BetweenAndDataAnalyse(IReadOnlyList<byte> startBytes, IReadOnlyList<byte> endBytes) :base(startBytes, endBytes)
        {
        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> bodyBytes)
        {
            return bodyBytes.ToArray();
        }
    }

    /// <summary>
    /// 抽象类的 代码应用示例
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    public class BetweenAndStringAnalyse<TChannelKey> : BetweenAndDataAnalysePattern<TChannelKey, string>
    {
        /// <summary>
        /// 抽象类的 代码应用示例
        /// </summary>
        /// <param name="startString">起始字符</param>
        /// <param name="endString">结束字符</param>
        public BetweenAndStringAnalyse(string startString, string endString):
            base(Encoding.Default.GetBytes(startString), Encoding.Default.GetBytes(endString))
        {
        }

        /// <inheritdoc/>
        protected override string ConvertResultType(List<byte> bodyBytes)
        {
            return Encoding.Default.GetString(bodyBytes.ToArray());
        }
    }
}
