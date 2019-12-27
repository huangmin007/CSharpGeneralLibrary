using System;

namespace SpaceCG.General
{
    /// <summary>
    /// 固定包大小的数据分析适配器基类
    /// <para>按固定大小的字节数据做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelType">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedSizeDataAnalyseAdapter<TChannelType, TResultType>:AbstractDataAnalyseAdapter<TChannelType, TResultType>
    {
        /// <summary>
        /// 整体数据包固定大小
        /// </summary>
        private readonly int packetSize;

        /// <summary>
        /// 固定包大小数据分析适配器
        /// </summary>
        /// <param name="packetSize">整体数据包固定大小</param>
        /// <exception cref="ArgumentException"> 参数错误，数据包不得小于 1 </exception>
        protected FixedSizeDataAnalyseAdapter(int packetSize)
        {
            if (packetSize < 1) throw new ArgumentException($"参数 {nameof(packetSize)} 不得小于 1");
            this.packetSize = packetSize;
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResult)
        {
            Channel<TChannelType> channel = GetChannel(key);
            if (channel == null) return false;

            if (channel.MaxSize < packetSize)
                throw new ArgumentException("参数异常：通道缓存大小 小于 数据包大小");

            bool handled = false;
            channel.Cache.AddRange(data);   // 添加数据到通道缓存

            do
            {
                // 缓存数据大小 还没 达到包的大小 ?
                if (channel.Cache.Count >= packetSize)
                {
                    //当前整包数据字节
                    byte[] packetBytes = channel.Cache.GetRange(0, packetSize).ToArray();

                    TResultType result = ParseResultType(packetBytes);      //包体数据封装，从适配器子类中实现
                    handled = analyseResult?.Invoke(key, result) ?? false;  //分析结果回调

                    // 如果数据处理了，则移除处理完成后的数据
                    if (handled)    channel.Cache.RemoveRange(0, packetSize);
                    // 如果缓存大小，大于设置的最大大小，则移除多余的数据
                    if (channel.Cache.Count >= channel.MaxSize)  
                        channel.Cache.RemoveRange(0, channel.Cache.Count - channel.MaxSize);

                    // 没有数据了就返回了
                    if (channel.Cache.Count == 0) break;
                }
                else
                {
                    return false;
                }
            }
            while (true);

            return handled;
        }

        /// <inheritdoc/>
        protected override TResultType ParseResultType(params byte[][] blocks)
        {
            return default;
        }
    }
}
