using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCG.Generic
{
    /// <summary>
    /// 【抽象类（基类型：<see cref="byte"/>）】根据 数据固定大小(长度) 对数据模式分析的适配器 抽象类
    /// <para>数据模式：{ (数据=固定长度的字节) }</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedSizeDataAnalysePattern<TChannelKey, TResultType>:AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
    {
        /// <summary>
        /// 数据固定大小
        /// </summary>
        protected readonly int PacketSize;

        /// <summary>
        /// 根据 数据固定大小(长度) 对数据模式分析的适配器 抽象类
        /// <para>数据模式：{ (数据=固定长度的字节数据) }</para>
        /// </summary>
        /// <param name="packetSize">整体数据固定大小</param>
        /// <exception cref="ArgumentException"> 参数错误，数据不得小于 1 </exception>
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
        /// <param name="packetBytes">包字节数据</param>
        /// <returns> 返回数据结果 </returns>
        protected virtual TResultType ConvertResultType(List<byte> packetBytes)
        {
            return default;
        }
    }

    /// <summary>
    /// 【返回 byte[] 】根据 数据固定大小(长度) 对数据模式分析的适配器
    /// <para>数据模式：{ (数据=固定长度的字节数据) }</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedSizeDataAnalyse<TChannelKey> : FixedSizeDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 根据 数据固定大小(长度) 对数据模式分析的适配器
        /// <para>数据模式：{ (数据=固定长度的字节数据) }</para>
        /// </summary>
        /// <param name="packetSize">数据固定大小</param>
        public FixedSizeDataAnalyse(int packetSize) :base(packetSize)
        {
        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> packetBytes)
        {
            return packetBytes.ToArray();
        }
    }

    /// <summary>
    /// 抽象类的 代码应用示例
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedSizeStringAnalyse<TChannelKey>: FixedSizeDataAnalysePattern<TChannelKey, string>
    {
        /// <summary>
        /// 抽象类的 代码应用示例
        /// </summary>
        /// <param name="length">字符长度</param>
        public FixedSizeStringAnalyse(int length) : base(length)
        {
        }

        /// <inheritdoc/>
        protected override string ConvertResultType(List<byte> packetBytes)
        {
            return Encoding.Default.GetString(packetBytes.ToArray());
        }
    }
}
