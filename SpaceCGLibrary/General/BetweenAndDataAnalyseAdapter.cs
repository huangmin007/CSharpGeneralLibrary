using System;

namespace SpaceCG.General
{
    /// <summary>
    /// 在 包头 和 包尾 之间，数据分析适配器基类
    /// </summary>
    /// <typeparam name="TChannelType"></typeparam>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class BetweenAndDataAnalyseAdapter<TChannelType, TResultType> : AbstractDataAnalyseAdapter<TChannelType, TResultType>
    {
        /// <summary>
        /// Start Boyer Moore
        /// </summary>
        private readonly BoyerMoore startBoyerMoore;

        /// <summary>
        /// End Boyer Moore
        /// </summary>
        private readonly BoyerMoore endBoyerMoore;

        /// <summary>
        /// 在 包头 和 包尾 之间，数据分析适配器
        /// </summary>
        /// <param name="start">包头数据</param>
        /// <param name="end">包尾数据</param>
        protected BetweenAndDataAnalyseAdapter(byte[] start, byte[] end)
        {
            if (end == null || end.Length == 0) throw new ArgumentNullException("参数 end 不能为空，长度不能为 0");
            if (start == null || start.Length == 0) throw new ArgumentNullException("参数 start 不能为空，长度不能为 0");

            endBoyerMoore = new BoyerMoore(end);
            startBoyerMoore = new BoyerMoore(start);
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResultHandler)
        {
            Channel<TChannelType> channel = GetChannel(key);
            if (channel == null) return false;

            bool handled = false;
            channel.Cache.AddRange(data);
            //if (channel.Cache.Count > data.Length)
            //    data = channel.Cache.ToArray();

            int lastPosition = 0;
            int endLength = endBoyerMoore.PatternLength;
            int startLength = startBoyerMoore.PatternLength;

            do
            {
                // 长度不够一个包等下次再来
                if (data.Length - lastPosition < startLength + endLength) break;
               
                // 搜索起始标志
                var startPosition = startBoyerMoore.Search(channel.Cache, lastPosition);
                // 是否找到了
                if (startPosition == -1) break;
                startPosition = lastPosition + startPosition + startLength;

                // 搜索结束标志, 从起始位置+起始标志长度开始找
                var count = endBoyerMoore.Search(channel.Cache, startPosition);
                if (count == -1) break;

                // 得到一条完整数据包
                var bodyData = channel.Cache.GetRange(startPosition, count).ToArray();
                lastPosition += count + startLength + endLength;

                TResultType result = ParseResultType(bodyData);
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

        /// <inheritdoc/>
        protected override TResultType ParseResultType(params byte[][] blocks)
        {
            return default;
        }
    }
}
