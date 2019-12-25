using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.General
{
    /// <summary>
    /// 数据包大小 = 固定包头大小 + 动态包体大小，固定包头数据分析适配器基类
    /// <para>跟据读取的包头(固定字节大小)内容，获取包体大小</para>
    /// </summary>
    /// <typeparam name="TChannelType">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    public abstract class FixedHeaderDataAnalyseAdapter<TChannelType, TResultType> : AbstractDataAnalyseAdapter<TChannelType, TResultType>
    {
        /// <summary>
        /// 包头大小, 调用 <see cref="GetBodySize"/> 方法时候会返回动态包体大小；需要继承当前接口的类在构造函数中设置包头长度
        /// </summary>
        private readonly int headerSize;

        /// <summary>
        ///  数据包最大大小，如果 数据包大小(包头长度 + 包体长度) 大于该值，表示数据错误
        /// </summary>
        private readonly int maxPacketSize;

        /// <summary>
        /// 固定包头数据分析适配器
        /// </summary>
        /// <param name="headerSize">包头字节 占 整体数据包的大小</param>
        /// <param name="maxPacketSize">整体数据包 预计 最大字节大小，超出则清除处理，为 0 表示不做包的超出检测比较</param>
        /// <exception cref="ArgumentException">参数错误，参数 headerSize 必须大于 0, 参数 maxPacketSize 必须大于或等于 0 </exception>
        protected FixedHeaderDataAnalyseAdapter(int headerSize, int maxPacketSize)
        {
            if (headerSize < 1 || maxPacketSize < 0)
                throw new ArgumentException("参数 headerSize 必须大于 0, 参数 maxPacketSize 必须大于或等于 0");

            this.headerSize = headerSize;
            this.maxPacketSize = maxPacketSize;
        }

        /// <inheritdoc/>
        public override bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResult)
        {
            Channel<TChannelType> channel = GetChannel(key);
            if (channel == null) return false;
            if (maxPacketSize != 0 && channel.MaxSize < maxPacketSize)
                throw new ArgumentException("参数异常：通道缓存大小 小于 数据包最大大小");


            bool handled = false; 
            channel.Cache.AddRange(data);   // 添加数据到通道缓存
            
            do
            {
                if (channel.Cache.Count < headerSize) return false;

                // 包头字节数据
                byte[] headerBytes = channel.Cache.GetRange(0, headerSize).ToArray();
                // 包体长度从适配器子类中获取
                int bodySize = GetBodySize(headerBytes);
                // 当前完整的 数据包长度(包头大小 + 数据包的大小)
                var currentPacketSize = headerSize + bodySize;

                // 判断最大封包长度，该整包大小超出设定的包大小，数据错误？？是否要清除呢？？
                if (maxPacketSize != 0 && currentPacketSize > maxPacketSize)
                {
                    //  ...
                    // 如果缓存大小，大于设置的最大大小，则移除多余的数据
                    if (channel.Cache.Count >= channel.MaxSize)
                        channel.Cache.RemoveRange(0, channel.Cache.Count - channel.MaxSize);

                    return false;
                }

                if (channel.MaxSize < currentPacketSize)
                    throw new ArgumentException("参数异常：通道缓存大小 小于 当前数据包大小");

                // 缓存数据大小 还没 达到当前包的大小 ?
                if (channel.Cache.Count >= currentPacketSize)
                {
                    // 包体字节数据
                    byte[] bodyBytes = channel.Cache.GetRange(headerSize, bodySize).ToArray();
                    
                    TResultType result = ParseResultType(headerBytes, bodyBytes);   // 包体数据封装，从适配器子类中实现
                    handled = analyseResult?.Invoke(key, result) ?? false;          // 分析结果回调

                    // 如果数据处理了，则移除处理完成后的数据
                    if (handled) channel.Cache.RemoveRange(0, currentPacketSize);
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
        /// 分析头部字节数据
        /// <para>因为字节端不确定，或是头部信息不确定，所以需要在这里实现分析头部数据并返回一个值</para>
        /// </summary>
        /// <param name="header">头部字节数据</param>
        /// <returns></returns>
        protected virtual int GetBodySize(byte[] header)
        {
            return 0;
        }

        /// <summary>
        /// 解析封装数据类型
        /// </summary>
        /// <param name="header">包头</param>
        /// <param name="body">源数据包体</param>
        /// <returns>需子类根据包体 data 自己解析对象并返回</returns>
        protected virtual TResultType ParseResultType(byte[] header, byte[] body)
        {
            return default;
        }
    }
}
