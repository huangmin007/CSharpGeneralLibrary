using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCG.General
{
    /// <summary>
    /// 数据分析通道
    /// </summary>
    /// <typeparam name="TChannelType"></typeparam>
    public class Channel<TChannelType>
    {
        /// <summary>
        /// 数据通道键类型，通道唯一标识
        /// </summary>
        internal TChannelType Key;
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
    /// 抽象多通道数据分析适配器(支持多线程)
    /// </summary>
    /// <typeparam name="TChannelType">通道键类型</typeparam>
    /// <typeparam name="TResultType">返回的数据结果类型</typeparam>
    public abstract class AbstractDataAnalyseAdapter<TChannelType, TResultType> : IDataAnalyseAdapter<TChannelType, TResultType>
    {
        /// <summary>
        /// 数据通道缓存字典
        /// </summary>
        protected readonly ConcurrentDictionary<TChannelType, Channel<TChannelType>> Channels = new ConcurrentDictionary<TChannelType, Channel<TChannelType>>();

        /// <inheritdoc />
        public bool AddChannel(TChannelType key)
        {
            if (Channels.ContainsKey(key))
                throw new ArgumentException($"数据分析通道 key:{key} 已存在");

            try
            {
                return Channels.TryAdd(key, new Channel<TChannelType>()
                {
                    Cache = new List<byte>(512),
                    Capacity = 512,
                    MaxSize = 1024,
                    Key = key,
                }) ;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool AddChannel(TChannelType key, int capacity, int maxSize)
        {
            if (Channels.ContainsKey(key))
                throw new ArgumentException($"数据分析通道 key:{key} 已存在");

            try
            {
                return Channels.TryAdd(key, new Channel<TChannelType>()
                {
                    Cache = new List<byte>(capacity),
                    Capacity = capacity,
                    MaxSize = maxSize,
                    Key = key,
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool RemoveChannel(TChannelType key)
        {
            if (!Channels.ContainsKey(key))
                throw new ArgumentException($"数据分析通道 key:{key} 不存在");

            try
            {
                Channel<TChannelType> channel;
                bool result = Channels.TryRemove(key, out channel);

                if (channel != null)
                {
                    channel.Cache.Clear();
                    channel.Cache = null;
                    channel = null;
                }
                return result;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取数据分析通道对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Channel<TChannelType> GetChannel(TChannelType key)
        {
            Channel<TChannelType> channel = null;

            try
            {
                bool result = Channels.TryGetValue(key, out channel);
                return channel;
            }
            catch (ArgumentNullException)
            {
                return channel;
            }
        }

        /// <inheritdoc/>
        public virtual bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResultHandler)
        {
            // 1. 数据分析过程在这里实现
            // 3. 转换完成回调 analyseResult
            return false;
        }

        /// <summary>
        /// 解析封装数据类型
        /// </summary>
        /// <param name="block">源数据块</param>
        /// <returns> 需子类根据 源数据块 解析对象并返回 结果类型 </returns>
        protected virtual TResultType ParseResultType(byte[] block)
        {
            // 2. 数据解析转换在这里实现
            return default;
        }

        /// <summary>
        /// 解析封装数据类型
        /// </summary>
        /// <param name="block0">源数据块</param>
        /// <param name="block1">源数据块</param>
        /// <returns> 需子类根据 源数据块 解析对象并返回 结果类型 </returns>
        protected virtual TResultType ParseResultType(byte[] block0, byte[] block1)
        {
            // 2. 数据解析转换在这里实现
            return default;
        }

        /// <summary>
        /// 解析封装数据类型
        /// </summary>
        /// <param name="block0">源数据块</param>
        /// <param name="block1">源数据块</param>
        /// <param name="block2">源数据块</param>
        /// <returns> 需子类根据 源数据块 解析对象并返回 结果类型 </returns>
        protected virtual TResultType ParseResultType(byte[] block0, byte[] block1, byte[] block2)
        {
            // 2. 数据解析转换在这里实现
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
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion



    }
}
