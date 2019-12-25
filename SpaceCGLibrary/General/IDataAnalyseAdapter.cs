using System;
using System.Collections.Generic;

namespace SpaceCG.General
{
    /// <summary>
    /// 分析结果处理，代理函数
    /// </summary>
    /// <typeparam name="TChannelType">通道键类型</typeparam>
    /// <typeparam name="TResultType">数据结果类型</typeparam>
    /// <param name="key">通道键类型，或通道唯一标识 key </param>
    /// <param name="result">分析完成后的数据类型</param>
    /// <returns></returns>
    public delegate bool AnalyseResultHandler<in TChannelType, in TResultType>(TChannelType key, TResultType result);

    /// <summary>
    /// 数据分析适配器
    /// </summary>
    /// <typeparam name="TChannelType">通道 key 类型</typeparam>
    /// <typeparam name="TResultType">分析完成后的结果类型</typeparam>
    public interface IDataAnalyseAdapter<TChannelType, TResultType> : IDisposable
    {
        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道键类型，或通道唯一标识 key</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns>添加成功返回 true </returns>
        bool AddChannel(TChannelType key);

        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道键类型，或通道唯一标识 key</param>
        /// <param name="capacity">缓存数据最初可以存储的元素数</param>
        /// <param name="maxSize">缓存数据最大字节大小</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns>添加成功返回 true </returns>
        bool AddChannel(TChannelType key, int capacity, int maxSize);

        /// <summary>
        /// 分析指定通道的数据
        /// </summary>
        /// <param name="key">通道键类型，或通道唯一标识 key</param>
        /// <param name="data">需要分析，或需要添加到缓存的数据</param>
        /// <param name="analyseResultHandler">分析产生结果时的代理调用</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns>数据分析成功，或结果数据处理完成后 返回 true </returns>
        bool AnalyseChannel(TChannelType key, byte[] data, AnalyseResultHandler<TChannelType, TResultType> analyseResultHandler);

        /// <summary>
        /// 移除并清除数据分析通道
        /// </summary>
        /// <param name="key">通道键类型，或通道唯一标识 key</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns>移除成功返回 true </returns>
        bool RemoveChannel(TChannelType key);
    }

    
}
