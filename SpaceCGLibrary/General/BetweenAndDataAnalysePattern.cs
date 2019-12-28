using System;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// 在 包头 和 包尾 之间，数据分析模式适配器基类
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class BetweenAndDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, TResultType>
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
            Channel<TChannelKey> channel = GetChannel(key);
            if (channel == null) return false;

            bool handled = false;
            channel.Cache.AddRange(data);

            int lastPosition = 0;
            int endLength = EndBoyerMoore.PatternLength;
            int startLength = StartBoyerMoore.PatternLength;

            do
            {
                // 长度不够一个包等下次再来
                if (data.Count - lastPosition < startLength + endLength) break;
               
                // 搜索起始标志
                var startPosition = StartBoyerMoore.Search(channel.Cache, lastPosition);
                // 是否找到了
                if (startPosition == -1) break;
                startPosition = lastPosition + startPosition + startLength;

                // 搜索结束标志, 从起始位置+起始标志长度开始找
                var count = EndBoyerMoore.Search(channel.Cache, startPosition);
                if (count == -1) break;

                // 得到一条完整数据包
                var bodyData = channel.Cache.GetRange(startPosition, count);
                lastPosition += count + startLength + endLength;

                TResultType result = ConvertResultType(bodyData);
                bool boo = analyseResultHandler?.Invoke(key, result) ?? false;
                handled = handled || boo;
            }
            while (true);

            // 清除已处理了的数据
            if (handled && lastPosition > 0)
                channel.Cache.RemoveRange(0, lastPosition);

            // 如果缓存大小，大于设置的最大大小，则移除多余的数据
            if (channel.Cache.Count >= channel.MaxSize)
                channel.Cache.RemoveRange(0, channel.Cache.Count - channel.MaxSize);

            return handled;
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
