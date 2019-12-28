using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCG.General
{
    /// <summary>
    /// 数据通道对象
    /// </summary>
    /// <typeparam name="TChannelKey"></typeparam>
    public class Channel<TChannelKey>
    {
        /// <summary>
        /// 通道的唯一标识键
        /// </summary>
        internal TChannelKey Key;
        /// <summary>
        /// 通道缓存数据
        /// </summary>
        internal List<byte> Cache;
        /// <summary>
        /// 缓存数据最初可以存储的元素数
        /// </summary>
        internal int Capacity;
        /// <summary>
        /// 缓存数据最大字节大小
        /// </summary>
        internal int MaxSize;
    }

    /// <summary>
    /// 数据分析产生结果时调用的代理函数
    /// <para>返回结果如果 true 表示清除这部份的缓存数据，否则会存储在缓存中，直到缓存大小超出 最大缓存 才会被移除</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    /// <param name="key">通道的唯一标识键</param>
    /// <param name="result">分析产生的数据结果</param>
    /// <returns> 返回结果如果 true 表示清除这部份的缓存数据，否则会存储在缓存中，直到缓存大小超出 最大缓存 才会被移除 </returns>
    public delegate bool AnalyseResultHandler<in TChannelKey, in TResultType>(TChannelKey key, TResultType result);

    /// <summary>
    /// 抽象类，多通道数据分析适配器(支持多线程)
    /// <para>该类为抽象类，需继承实现 <see cref="AnalyseChannel(TChannelKey, IReadOnlyList{byte}, AnalyseResultHandler{TChannelKey, TResultType})"/>, <see cref="ParseResultType(byte[][])"/> 函数</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class AbstractDataAnalyseAdapter<TChannelKey, TResultType> : IDisposable
    {
        /// <summary>
        /// 多通道数据缓存字典
        /// </summary>
        private readonly ConcurrentDictionary<TChannelKey, Channel<TChannelKey>> Channels = new ConcurrentDictionary<TChannelKey, Channel<TChannelKey>>();

        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns>添加成功返回 true </returns>
        public bool AddChannel(TChannelKey key)
        {
            if (key == null) return false;

            bool result = Channels.TryAdd(key, new Channel<TChannelKey>()
            {
                Cache = new List<byte>(512),
                Capacity = 512,
                MaxSize = 1024,
                Key = key,
            });

            return result;
        }

        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <param name="capacity">缓存数据块最初可以存储的元素数量；缓存数据块可自动扩容(会产生数据拷贝)，但不会超出 maxSize; 实际数据缓存量建议不要超过该值</param>
        /// <param name="maxSize">缓存数据块最大容量，一般是 capacity * 1.5 到 2.0 大小</param>
        /// <returns> 添加成功返回 true </returns>
        public bool AddChannel(TChannelKey key, int capacity, int maxSize)
        {
            if (key == null) return false;
            if (capacity <= 0 || maxSize < capacity)
            {
                Console.WriteLine($"Error:: {nameof(capacity)}, {nameof(maxSize)} 缓存大小设置错误");
                return false;
            }

            bool result = Channels.TryAdd(key, new Channel<TChannelKey>()
            {
                Cache = new List<byte>(capacity),
                Capacity = capacity,
                MaxSize = maxSize,
                Key = key,
            });

            return result;
        }

        /// <summary>
        /// 移除一个数据分析通道，移除会清除缓存数据
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns>移除成功返回 true </returns>
        public bool RemoveChannel(TChannelKey key)
        {
            if (key == null) return false;

            Channel<TChannelKey> channel = null;
            bool result = Channels.TryRemove(key, out channel);
            if (result)
            {
                channel.Cache.Clear();
                channel.Cache = null;
            }

            return result;
        }

        /// <summary>
        /// 获取数据通道对象；不会抛出异常信息，如果通道不存在，则返回 null 值
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns> </returns>
        protected Channel<TChannelKey> GetChannel(TChannelKey key)
        {
            if (key == null) return null;

            Channel<TChannelKey> channel = null;
            bool result = Channels.TryGetValue(key, out channel);

            return result ? channel : null;
        }
        
        /// <summary>
        /// 分析指定通道的数据
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <param name="data">需要分析或缓存的数据</param>
        /// <param name="analyseResultHandler">分析产生结果时的代理调用</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <returns> 返回数据分析状态 </returns>
        public virtual bool AnalyseChannel(TChannelKey key, IReadOnlyList<byte> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
        {
            // 1. 数据分析过程在这里实现
            // 3. 转换完成后回调 analyseResultHandler
            return false;
        }

        /// <summary>
        /// 通道数据块转换数据类型
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="args">更多数据包</param>
        /// <returns> 返回数据结果 </returns>
        protected virtual TResultType ConvertResultType(IReadOnlyList<byte> packet, params object[] args)
        {
            //2. 数据解析转换在这里实现
            return default;
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    var results = from Channel in Channels
                                  select RemoveChannel(Channel.Key);
                    Channels.Clear();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DataAnalyseAdapter()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
