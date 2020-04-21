using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCG.General
{
    /// <summary>
    /// 固定大小的数据分析模式适配器基类
    /// <para>按固定大小的字节数据做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedSizeDataAnalysePattern<TChannelKey, TResultType>:AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
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
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
        {
            if (key == null || data == null || analyseResultHandler == null) return false;
            Channel<TChannelKey, byte> channel = GetChannel(key);
            if (channel == null) return false;

            if(channel.MaxSize < PacketSize)
            {
                Console.WriteLine("Error:设定的最大缓存大小 {0}:{1} 小于 设定的包 {2}:{3} 大小", "Cache.MaxSize", channel.MaxSize, nameof(PacketSize), PacketSize);
                return false;
            }

            // 添加数据到通道缓存
            channel.AddRange(data);   

            while(true)
            {
                if (channel.Available < PacketSize) break;

                var packetBytes = channel.GetRange(channel.Offset, PacketSize);
                TResultType result = ConvertResultType(packetBytes);
                bool handled = analyseResultHandler.Invoke(key, result);

                if (handled)
                    channel.RemoveRange(channel.Offset, PacketSize);
                else
                    channel.Offset += PacketSize;
            }

            channel.CheckOverflow();

            return true;
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
    /// 固定大小的原始字节数据分析
    /// <para>按固定大小的字节数据做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedSizeDataAnalyse<TChannelKey> : FixedSizeDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 固定大小的原始字节数据分析
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

    /// <summary>
    /// 固定长度的字符数据分析
    /// <para>按固定长度的字符做为一个数据包分析，无头无尾，按字节大小封包转换</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedSizeStringAnalyse<TChannelKey>: FixedSizeDataAnalysePattern<TChannelKey, string>
    {
        /// <summary>
        /// 固定长度的字符数据分析
        /// </summary>
        /// <param name="length">字符长度</param>
        public FixedSizeStringAnalyse(int length) : base(length)
        {
        }

        /// <inheritdoc/>
        protected override string ConvertResultType(List<byte> packet)
        {
            return Encoding.Default.GetString(packet.ToArray());
        }
    }
}
