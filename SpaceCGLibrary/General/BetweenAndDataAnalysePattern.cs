using System;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// 在 包头 和 包尾 之间，数据分析模式适配器基类
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
        /// 在 包头 和 包尾 之间，数据分析适配器
        /// </summary>
        /// <param name="start">包头数据</param>
        /// <param name="end">包尾数据</param>
        protected BetweenAndDataAnalysePattern(IReadOnlyList<byte> start, IReadOnlyList<byte> end)
        {
            if (end == null || end.Count == 0) throw new ArgumentNullException("参数 end 不能为空，长度不能为 0");
            if (start == null || start.Count == 0) throw new ArgumentNullException("参数 start 不能为空，长度不能为 0");

            EndBoyerMoore = new BoyerMoore(end);
            StartBoyerMoore = new BoyerMoore(start);
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
        /// <param name="body">主体源数据</param>
        /// <returns></returns>
        protected virtual TResultType ConvertResultType(List<byte> body)
        {
            return default;
        }
    }

    /// <summary>
    /// 在 包头 和 包尾 之间，数据分析模式适配器
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    public class BetweenAndDataAnalyse<TChannelKey> : BetweenAndDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 在 包头 和 包尾 之间，数据分析适配器
        /// </summary>
        /// <param name="start">包头数据</param>
        /// <param name="end">包尾数据</param>
        public BetweenAndDataAnalyse(IReadOnlyList<byte> start, IReadOnlyList<byte> end) :base(start, end)
        {

        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> body)
        {
            return body.ToArray();
        }
    }
}
