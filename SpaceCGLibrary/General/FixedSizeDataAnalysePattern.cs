using System;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// 固定大小的数据分析模式适配器基类
    /// <para>按固定大小的字节数据做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedSizeDataAnalysePattern<TChannelKey, TResultType>:AbstractDataAnalyseAdapter<TChannelKey, TResultType>
    {
        /// <summary>
        /// 数据包固定大小
        /// </summary>
        protected readonly int PacketSize;

        /// <summary>
        /// 固定包大小数据分析适配器
        /// </summary>
        /// <param name="packetSize">整体数据包固定大小</param>
        /// <exception cref="ArgumentException"> 参数错误，数据包不得小于 1 </exception>
        protected FixedSizeDataAnalysePattern(int packetSize)
        {
            if (packetSize < 1) throw new ArgumentException($"参数 {nameof(packetSize)} 不得小于 1");
            this.PacketSize = packetSize;
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResult)
        {
            Channel<TChannelKey> channel = GetChannel(key);
            if (channel == null) return false;

            //if (channel.MaxSize < packetSize)
            //    throw new ArgumentException("参数异常：通道缓存大小 小于 数据包大小");

            bool handled = false;
            channel.Cache.AddRange(data);   // 添加数据到通道缓存

            do
            {
                // 缓存数据大小 还没 达到包的大小 ?
                if (channel.Cache.Count >= PacketSize)
                {
                    //当前整包数据字节
                    var packetBytes = channel.Cache.GetRange(0, PacketSize);

                    TResultType result = ConvertResultType(packetBytes);      //包体数据封装，从适配器子类中实现
                    handled = analyseResult?.Invoke(key, result) ?? false;  //分析结果回调

                    // 如果数据处理了，则移除处理完成后的数据
                    if (handled)    channel.Cache.RemoveRange(0, PacketSize);
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

        /// <summary>
        /// 通道数据块转换数据类型
        /// </summary>
        /// <param name="packet">源数据包内容</param>
        /// <returns> 返回数据结果 </returns>
        protected virtual TResultType ConvertResultType(List<byte> packet)
        {
            return default;
        }
    }

    /// <summary>
    /// 固定大小的数据分析
    /// <para>按固定大小的字节数据做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedSizeDataAnalyse<TChannelKey> : FixedSizeDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 固定大小的数据分析
        /// </summary>
        /// <param name="packetSize">整体数据包固定大小</param>
        public FixedSizeDataAnalyse(int packetSize) :base(packetSize)
        {
        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> packet)
        {
            return packet.ToArray();
        }
    }
}
