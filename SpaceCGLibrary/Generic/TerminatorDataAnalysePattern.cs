using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCG.Generic
{
    /// <summary>
    /// 跟据终止字节数据分割数据包的分析模式适配器基类
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    public abstract class TerminatorDataAnalysePattern<TChannelKey, TResultType> : AbstractDataAnalyseAdapter<TChannelKey, byte, TResultType>
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
        public override bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
        {
            if (key == null || data == null || analyseResultHandler == null) return false;
            Channel<TChannelKey, byte> channel = GetChannel(key);
            if (channel == null) return false;
            
            channel.AddRange(data);

            // 在数据中搜索 终止数据，并返回索引集合
            // 0 - search index
            while(true)
            {
                int index = this.BoyerMoore.Search(channel.Cache, channel.Offset);
                if (index < 0) break;

                var bodySize = index - channel.Offset;
                if (bodySize < 0) break;

                var bodyBytes = channel.GetRange(channel.Offset, bodySize);

                TResultType result = ConvertResultType(bodyBytes);
                bool handled = analyseResultHandler.Invoke(key, result);

                // 清除已处理了的数据
                if (handled)
                    channel.RemoveRange(channel.Offset, bodySize + this.BoyerMoore.PatternLength);
                else
                    channel.Offset = index + this.BoyerMoore.PatternLength;
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
    /// 跟据终原始止字节数据分割数据包的分析模式适配器
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

    /// <summary>
    /// 跟据终止字符数据分割数据包的分析模式适配器
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    public class TerminatorStringAnalyse<TChannelKey>: TerminatorDataAnalysePattern<TChannelKey, string>
    {
        /// <summary>
        /// 跟据终止字符数据，分割数据包分析适配器
        /// </summary>
        /// <param name="terminator">终止字符数据</param>
        /// <exception cref="ArgumentNullException">参数错误，参数 terminator 不能为空</exception>
        public TerminatorStringAnalyse(string terminator) : base(Encoding.Default.GetBytes(terminator))
        {
        }

        /// <inheritdoc/>
        protected override string ConvertResultType(List<byte> packet)
        {
            return Encoding.Default.GetString(packet.ToArray());
        }
    }
}
