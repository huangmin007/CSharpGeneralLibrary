using System;
using System.Collections.Generic;

namespace SpaceCG.Generic
{
    /// <summary>
    /// 【抽象类（基类型：<see cref="byte"/>）】根据 数据头固定长度(数据主体动态长度) 对数据模式分析的适配器 抽象类
    /// <para>数据模式：{ (数据头=数据主体大小) + (数据主体=主体字节数据) }、{ (数据头=[数据起始标记 +]数据主体大小) + (数据主体=主体数据[+ 数据终止标记]) }、{ (数据头=[数据起始标记 + [其它信息 +]]数据主体大小) + (数据主体=主体数据[[+ 校验信息] + 数据终止标记]) }、...</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class FixedHeadDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
    {
        /// <summary>
        /// 数据头占用字节大小(长度), 调用 <see cref="GetBodySize"/> 方法时候会返回 数据主体动态大小；需要继承当前接口的类在构造函数中设置 数据头占用字节长度
        /// </summary>
        protected readonly int HeadSize;

        /// <summary>
        ///  数据包最大字节大小，如果 数据包字节大小(数据头长度 + 数据主体长度) 大于该值，表示数据错误
        /// </summary>
        protected readonly int MaxPacketSize;

        /// <summary>
        /// 根据 数据头固定长度(数据主体动态长度) 对数据模式分析的适配器 抽象类
        /// <para>数据模式：{ (数据头=数据主体大小) + (数据主体=主体字节数据) }、{ (数据头=[数据起始标记 +]数据主体大小) + (数据主体=主体数据[+ 数据终止标记]) }、{ (数据头=[数据起始标记 + [其它信息 +]]数据主体大小) + (数据主体=主体数据[[+ 校验信息] + 数据终止标记]) }、...</para>
        /// <para>注意：参数 headSize 必须大于 0, 参数 maxPacketSize(预计最大包所占用字节大小) 必须小于 最大缓存大小，且大于 包体动态大小，否则计算数据异常，但不抛出异常，只返回 false</para>
        /// </summary>
        /// <param name="headSize">数据头占用字节大小，获取该数据用于计算 数据主体占用字节大小</param>
        /// <param name="maxPacketSize">预计数据包(数据头 + 数据主体)最大字节大小，超出则清除处理；注意：最大包大小不得超出最大缓存大小</param>
        /// <exception cref="ArgumentException">参数错误，参数 headSize 必须大于 0, 参数 maxPacketSize 必须小于缓存大小，且大于包体字节动态大小 </exception>
        protected FixedHeadDataAnalysePattern(int headSize, int maxPacketSize = 1024)
        {
            if (headSize < 1 || maxPacketSize < 0)
                throw new ArgumentException($"参数 {nameof(headSize)} 必须大于 0, 参数 {nameof(maxPacketSize)} 必须小于 最大缓存大小，且大于 动态包体大小");

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
                if (HeadSize + bodySize > MaxPacketSize || bodySize <= 0)
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
        /// 数据头字节数据
        /// <para>因为字节端不确定，或是数据头部信息不确定(可能包含其它数据)，所以需要在这里实现分析头部数据并返回一个值</para>
        /// </summary>
        /// <param name="headBytes">头部源数据</param>
        /// <returns>返回数据主体字节大小</returns>
        protected virtual int GetBodySize(List<byte> headBytes)
        {
            return 0;
        }

        /// <summary>
        /// 通道数据块转换数据类型
        /// </summary>
        /// <param name="headBytes">头部源数据</param>
        /// <param name="bodyBytes">主体源数据</param>
        /// <returns></returns>
        protected virtual TResultType ConvertResultType(List<byte> headBytes, List<byte> bodyBytes)
        {
            return default;
        }
    }

    /// <summary>
    /// 【基类，返回 byte[] 】根据 数据头固定长度(数据主体动态长度) 对数据模式分析的适配器 基类
    /// <para>数据模式：{ (数据头=数据主体大小) + (数据主体=主体字节数据) }、{ (数据头=[数据起始标记 +]数据主体大小) + (数据主体=主体数据[+ 数据终止标记]) }、{ (数据头=[数据起始标记 + [其它信息 +]]数据主体大小) + (数据主体=主体数据[[+ 校验信息] + 数据终止标记]) }、...</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class FixedHeadDataAnalyse<TChannelKey> : FixedHeadDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 【基类，返回 byte[] 】根据 数据头固定长度(数据主体动态长度) 对数据模式分析的适配器 基类
        /// <para>数据模式：{ (数据头=数据主体大小) + (数据主体=主体字节数据) }、{ (数据头=[数据起始标记 +]数据主体大小) + (数据主体=主体数据[+ 数据终止标记]) }、{ (数据头=[数据起始标记 + [其它信息 +]]数据主体大小) + (数据主体=主体数据[[+ 校验信息] + 数据终止标记]) }、...</para>
        /// </summary>
        /// <param name="headSize">数据头占用字节大小，获取该数据用于计算 数据主体占用字节大小</param>
        /// <param name="maxPacketSize">预计数据包(数据头 + 数据主体)最大字节大小，超出则清除处理；注意：最大包大小不得超出最大缓存大小</param>
        public FixedHeadDataAnalyse(int headSize, int maxPacketSize) :base(headSize, maxPacketSize)
        {
        }

        /// <inheritdoc/>
        protected override int GetBodySize(List<byte> headBytes)
        {
            byte[] data = headBytes.ToArray();

            // 如果当前环境不是小端字节序，转换为小端字节序
            if (!BitConverter.IsLittleEndian) Array.Reverse(data);
            
            return BitConverter.ToInt32(data, 0);
        }

        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> headBytes, List<byte> bodyBytes)
        {
            return bodyBytes.ToArray();
        }

    }
}
