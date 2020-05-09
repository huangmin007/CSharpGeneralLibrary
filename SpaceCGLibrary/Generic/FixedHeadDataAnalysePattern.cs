using System;
using System.Collections.Generic;

namespace SpaceCG.Generic
{
    /// <summary>
    /// 数据包大小 = 固定包头大小 + 动态包体大小，固定包头数据分析模式适配器基类
    /// <para>跟据读取的包头(固定字节大小)内容，获取包体大小</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedHeadDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
    {
        /// <summary>
        /// 包头大小, 调用 <see cref="GetBodySize"/> 方法时候会返回动态包体大小；需要继承当前接口的类在构造函数中设置包头长度
        /// </summary>
        protected readonly int HeadSize;

        /// <summary>
        ///  数据包最大大小，如果 数据包大小(包头长度 + 包体长度) 大于该值，表示数据错误
        /// </summary>
        protected readonly int MaxPacketSize;

        /// <summary>
        /// 固定包头数据分析适配器
        /// <para>参数 headSize 必须大于 0, 参数 maxPacketSize 必须小于 最大缓存大小，且大于 动态包体大小，否则算数据异常，但不抛出异常，返回 false</para>
        /// </summary>
        /// <param name="headSize">包头字节 占 整体数据包的大小</param>
        /// <param name="maxPacketSize">整体数据包 预计 最大字节大小，超出则清除处理；注意：最大包大小不得超出最大缓存大小</param>
        /// <exception cref="ArgumentException">参数错误，参数 headSize 必须大于 0, 参数 maxPacketSize 必须小于缓存大小，且大于动态包体大小 + HeadSize </exception>
        protected FixedHeadDataAnalysePattern(int headSize, int maxPacketSize = 1024)
        {
            if (headSize < 1 || maxPacketSize < 0)
                throw new ArgumentException("参数 headSize 必须大于 0, 参数 maxPacketSize 必须小于 最大缓存大小，且大于 动态包体大小");

            this.HeadSize = headSize;
            this.MaxPacketSize = maxPacketSize;
        }
        
        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
        {
            if (key == null || data == null || analyseResultHandler == null) return false;
            Channel<TChannelKey, byte> channel = GetChannel(key);
            if (channel == null) return false;

            if (MaxPacketSize > channel.MaxSize)
            {
                Console.WriteLine("Error:设定的最大包大小 {0}:{1} 超出，设定的最大通道缓存 Cache.MaxSize:{2}  大小", nameof(MaxPacketSize), MaxPacketSize, channel.MaxSize);
                return false;  
            }

            bool isDataError = false;
            channel.AddRange(data);

            while (true)
            {
                if (channel.Available < HeadSize) break;
                var headBytes = channel.GetRange(channel.Offset, HeadSize);

                int bodySize = GetBodySize(headBytes);
                if (HeadSize + bodySize > MaxPacketSize || bodySize < 0)
                {
                    isDataError = true;
                    Console.WriteLine("Error:当前包大小 {0}， 超出设定的最大包大小 {1}", HeadSize + bodySize, MaxPacketSize);
                    break;
                }

                if (channel.Available - HeadSize < bodySize) break;                
                var bodyBytes = channel.GetRange(channel.Offset + HeadSize, bodySize);

                TResultType result = ConvertResultType(headBytes, bodyBytes);
                bool handled = analyseResultHandler.Invoke(key, result);

                if (handled)
                    channel.RemoveRange(channel.Offset, HeadSize + bodySize);
                else
                    channel.Offset += HeadSize + bodySize;
            }

            channel.CheckOverflow();

            return !isDataError;
        }

        /// <summary>
        /// 分析头部字节数据
        /// <para>因为字节端不确定，或是头部信息不确定，所以需要在这里实现分析头部数据并返回一个值</para>
        /// </summary>
        /// <param name="head">头部源数据</param>
        /// <returns>返回包主体字节大小</returns>
        protected virtual int GetBodySize(List<byte> head)
        {
            return 0;
        }

        /// <summary>
        /// 通道数据块转换数据类型
        /// </summary>
        /// <param name="head">头部源数据</param>
        /// <param name="body">主体源数据</param>
        /// <returns></returns>
        protected virtual TResultType ConvertResultType(List<byte> head, List<byte> body)
        {
            return default;
        }
    }

    /// <summary>
    /// 数据包大小 = 固定包头大小 + 动态包体大小，固定包头数据分析
    /// <para>跟据读取的包头(固定字节大小)内容，获取包体大小</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedHeadDataAnalyse<TChannelKey> : FixedHeadDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 固定包头数据分析
        /// </summary>
        /// <param name="headSize">包头字节 占 整体数据包的大小</param>
        /// <param name="maxPacketSize">整体数据包 预计 最大字节大小，超出则清除处理，为 0 表示不做包的超出检测比较</param>
        public FixedHeadDataAnalyse(int headSize, int maxPacketSize) :base(headSize, maxPacketSize)
        {
        }

        /// <inheritdoc/>
        protected override int GetBodySize(List<byte> head)
        {
            byte[] data = head.ToArray();

            // 如果当前环境不是小端字节序，转换为小端字节序
            if (!BitConverter.IsLittleEndian) Array.Reverse(data);
            
            return BitConverter.ToInt32(data, 0);
        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> head, List<byte> body)
        {
            return body.ToArray();
        }

    }
}
