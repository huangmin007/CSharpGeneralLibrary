using System;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// 跟据终止字节数据分割数据包的分析模式适配器基类
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    public abstract class TerminatorDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, TResultType>
    {
        /// <summary>
        /// Terminator Boyer - Moore
        /// </summary>
        protected readonly BoyerMoore BoyerMoore;

        /// <summary>
        /// 跟据终止字节数据，分割数据包分析适配器
        /// </summary>
        /// <param name="terminator">终止字节数据</param>
        /// <exception cref="ArgumentNullException">参数错误，参数 terminator 不能为空</exception>
        protected TerminatorDataAnalysePattern(IReadOnlyList<byte> terminator)
        {
            if (terminator == null || terminator.Count == 0)
                throw new ArgumentNullException(nameof(terminator), "参数不能为空，长度不能为 0");

            this.BoyerMoore = new BoyerMoore(terminator);
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResult)
        {
            Channel<TChannelKey> channel = GetChannel(key);
            if (channel == null) return false;

            bool handled = false;
            channel.Cache.AddRange(data);

            // 在数据中搜索 终止数据，并返回索引集合
            int[] indexs = this.BoyerMoore.SearchAll(channel.Cache);
            if (indexs.Length == 0) return false;

            int lastPosition = 0;
            int terminatorLength = this.BoyerMoore.PatternLength;    // 终止符长度

            foreach (int index in indexs)
            {
                // 数据包大小
                var packetSize = index + 1 - lastPosition;
                // 数据包字节
                var packetBytes = channel.Cache.GetRange(lastPosition, packetSize);
                // update last position 
                lastPosition += packetSize + terminatorLength;
                
                TResultType result = ConvertResultType(packetBytes);          // 包体数据封装，从适配器子类中实现
                bool boo = analyseResult?.Invoke(key, result) ?? false;     // 分析结果回调
                handled = handled || boo;
            }

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
        /// <param name="packet">源数据包内容</param>
        /// <returns> 返回数据结果 </returns>
        protected virtual TResultType ConvertResultType(List<byte> packet)
        {
            return default;
        }
    }

    /// <summary>
    /// 跟据终止字节数据分割数据包的分析模式适配器
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    public class TerminatorDataAnalyse<TChannelKey> : TerminatorDataAnalysePattern<TChannelKey, byte[]>
    {
        /// <summary>
        /// 跟据终止字节数据，分割数据包分析适配器
        /// </summary>
        /// <param name="terminator">终止字节数据</param>
        /// <exception cref="ArgumentNullException">参数错误，参数 terminator 不能为空</exception>
        public TerminatorDataAnalyse(IReadOnlyList<byte> terminator) :base(terminator)
        {

        }
        /// <inheritdoc/>
        protected override byte[] ConvertResultType(List<byte> packet)
        {
            return packet.ToArray();
        }
    }
}
