using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.General
{
    /// <summary>
    /// 跟据终止字节数据，分割数据包分析适配器基类
    /// </summary>
    /// <typeparam name="TChannelType">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    public abstract class TerminatorDataAnalyseAdapter<TChannelType, TResultType> : AbstractDataAnalyseAdapter<TChannelType, TResultType>
    {
        /// <summary>
        /// Boyer Moore
        /// </summary>
        private readonly BoyerMoore boyerMoore;

        /// <summary>
        /// 跟据终止字节数据，分割数据包分析适配器
        /// </summary>
        /// <param name="terminator">终止字节数据</param>
        /// <exception cref="ArgumentNullException">参数错误，参数 terminator 不能为空</exception>
        protected TerminatorDataAnalyseAdapter(byte[] terminator)
        {
            if (terminator == null || terminator.Length == 0)
                throw new ArgumentNullException("参数 terminator 不能为空，长度不能为 0");

            boyerMoore = new BoyerMoore(terminator);
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResult)
        {
            Channel<TChannelType> channel = GetChannel(key);
            if (channel == null) return false;

            bool handled = false;
            channel.Cache.AddRange(data);
            if (channel.Cache.Count > data.Length)
                data = channel.Cache.ToArray();

            // 在数据中搜索 终止数据，并返回索引集合
            List<int> indexs = boyerMoore.SearchAll(data);
            if (indexs.Count == 0) return false;

            int lastPosition = 0;
            int terminatorLength = boyerMoore.PatternLength;    // 终止符长度

            foreach (int index in indexs)
            {
                // 数据包大小
                int packetSize = index + 1 - lastPosition;
                // 数据包字节
                byte[] packetBytes = channel.Cache.GetRange(lastPosition, packetSize).ToArray();
                // update last position 
                lastPosition += packetSize + terminatorLength;
                
                TResultType result = ParseResultType(packetBytes);          // 包体数据封装，从适配器子类中实现
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

        /// <inheritdoc/>
        protected virtual TResultType ParseResultType(byte[] packet)
        {
            return default;
        }
    }
}
