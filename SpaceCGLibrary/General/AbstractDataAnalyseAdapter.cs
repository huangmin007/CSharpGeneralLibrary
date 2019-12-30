using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCG.General
{

    /// <summary>
    /// 数据通道对象
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TDataType">数据类型</typeparam>
    public sealed class Channel<TChannelKey, TDataType> : IDisposable where TDataType : struct
    {
        private TChannelKey _Key;
        private List<TDataType> _Cache;

        /// <summary>
        /// 通道的唯一标识键
        /// </summary>
        public TChannelKey Key => _Key;
        /// <summary>
        /// 缓存容器实例
        /// <para>不建议直接操作该实例，而是调用 <see cref="Channel{TChannelKey, TDataType}"/> 的方法操作缓存实例</para>
        /// </summary>
        public List<TDataType> Cache => _Cache;        
        /// <summary>
        /// 最大缓存数据大小
        /// </summary>
        public int MaxSize { get; }
        
        private int _offset = 0;
        /// <summary>
        /// 数据处理的偏移量 (内部索引)
        /// <para>在 <see cref="Offset"/> 左侧的数据不会处理，只会溢出移除</para>
        /// <para>函数 <see cref="GetRange(int, int, bool)"/>, <see cref="RemoveRange(int, int)"/> ... 的参数 index 属于外部索引</para>
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value < 0 ? 0 : value >= Cache.Count ? Cache.Count - 1 : value; }
        }

        /// <summary>
        /// 有效元素大小/可读取的数据大小
        /// <para> <see cref="Available"/> = <see cref="Cache.Count"/> - <see cref="Offset"/></para>
        /// </summary>
        public int Available => Cache.Count - Offset;

        /// <summary>
        /// 数据通道对象，默认最大缓存元素大小 1024 
        /// </summary>
        /// <param name="key">通道键类型</param>
        public Channel(TChannelKey key)
        {
            this._Key = key;
            this.MaxSize = 1024;
            this._Cache = new List<TDataType>((int)(MaxSize * 1.25));
        }
        /// <summary>
        /// 数据通道对象
        /// </summary>
        /// <param name="key">通道键类型</param>
        /// <param name="maxSize">通道缓存大小</param>
        public Channel(TChannelKey key, int maxSize)
        {
            this._Key = key;
            this.MaxSize = maxSize;
            this._Cache = new List<TDataType>((int)(this.MaxSize * 1.25));
        }

        /// <summary>
        /// 检查溢出。如果溢出，则清除多余的数据，从最先入缓存处计算
        /// <para> 如果有清除数据，则 <see cref="Offset"/> 也会向左移动，移动范围为清除数据的大小</para>
        /// </summary>
        public void CheckOverflow()
        {
            if (Cache.Count > MaxSize)
            {
                Offset -= Cache.Count - MaxSize;
                Cache.RemoveRange(0, Cache.Count - MaxSize);
            }
        }
        
        /// <summary>
        /// 添加数据到缓存结尾
        /// </summary>
        /// <param name="data"></param>
        public void Add(TDataType data)
        {
            Cache.Add(data);
            CheckOverflow();
        }
        /// <summary>
        /// 添加数据到缓存结尾
        /// </summary>
        /// <param name="data"></param>
        public void AddRange(IEnumerable<TDataType> data)
        {
            Cache.AddRange(data);
            CheckOverflow();
        }

        /// <summary>
        /// 获取部份缓存数据，从 <see cref="Offset"/> 处开始的 size 大小，移除会修改 <see cref="Offset"/> 值。
        /// <para>如果参数错误，则返回 null ；如果可读取的数据小于 size ，则返回可读取的数据部份</para>
        /// </summary>
        /// <param name="size">从 <see cref="Offset"/> 处开始的元素数量</param>
        /// <param name="remove">是否从缓存中移除返回的数据大小，实则调用 <see cref="RemoveRange(int)"/></param>
        /// <returns>返回从 <see cref="Offset"/> 处开始的元素数量</returns>
        public List<TDataType> GetRange(int size, bool remove = false)
        {
            if (size < 0) return null;
            return GetRange(Offset, size, remove);
        }
        /// <summary>
        /// 获取部份缓存数据，从 index 处开始的 size 元素数量，移除不会修改 <see cref="Offset"/> 值。
        /// <para>如果参数错误，则返回 null ；如果可读取的数据小于 size ，则返回可读取的数据部份</para>
        /// </summary>
        /// <param name="index">范围开始处的从零开始的索引</param>
        /// <param name="size">范围中的元素数量</param>
        /// <param name="remove">是否从缓存中移除返回的数据大小，实则调用 <see cref="RemoveRange(int, int)"/></param>
        /// <returns>返回从 index 处开始的 size 元素数量</returns>
        public List<TDataType> GetRange(int index, int size, bool remove = false)
        {
            if (index < 0 || index > Cache.Count || size < 0) return null;

            int avaliable = Cache.Count - index;
            List<TDataType> list = Cache.GetRange(index, avaliable <= size ? avaliable : size);

            if (remove) RemoveRange(index, size);

            return list;
        }

        /// <summary>
        /// 从缓存中移除部份数据，从 <see cref="Offset"/> 处开始移除数据，<see cref="Offset"/> 会向左偏移被移除的数量
        /// </summary>
        /// <param name="size">移除数据的数量</param>
        public void RemoveRange(int size)
        {
            if (size <= 0) return;

            Cache.RemoveRange(Offset, Available <= size ? Available : size);
            Offset -= Available <= size ? Available : size;
        }
        /// <summary>
        /// 从缓存中移除部份数据，从 index 处开始移除数据，<see cref="Offset"/> 不会被修改
        /// </summary>
        /// <param name="index">范围开始处的从零开始的索引</param>
        /// <param name="size">移除数据的数量</param>
        public void RemoveRange(int index, int size)
        {
            if (index < 0 || index > Cache.Count || size <= 0) return;

            int avaliable = Cache.Count - index;
            Cache.RemoveRange(index, avaliable <= size ? avaliable : size);
        }

        /// <summary>
        /// 清除通道所有缓存的数据，设置 <see cref="Offset"/> 为 0
        /// </summary>
        public void Clear()
        {
            Offset = 0;
            _Cache.Clear();
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Clear();

            _Cache = null;
            _Key = default;     //如果 TChannelKey 是引用类型，需要清除
        }
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
    /// <para>该类为抽象类，需继承实现 <see cref="AnalyseChannel(TChannelKey, IReadOnlyList{TDataType}, AnalyseResultHandler{TChannelKey, TResultType})"/>, <see cref="ConvertResultType(IReadOnlyList{TDataType}, object[])"/> 函数</para>
    /// </summary>
    /// <typeparam name="TChannelKey">通道键类型</typeparam>
    /// <typeparam name="TDataType">数据类型</typeparam>
    /// <typeparam name="TResultType">数据结果封装类型</typeparam>
    public abstract class AbstractDataAnalyseAdapter<TChannelKey, TDataType, TResultType> where TDataType: struct 
    {
        /// <summary>
        /// 多通道数据缓存字典
        /// </summary>
        private readonly ConcurrentDictionary<TChannelKey, Channel<TChannelKey, TDataType>> Channels = new ConcurrentDictionary<TChannelKey, Channel<TChannelKey, TDataType>>();

        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns>添加成功返回 true </returns>
        public bool AddChannel(TChannelKey key)
        {
            if (key == null) return false;

            return Channels.TryAdd(key, new Channel<TChannelKey, TDataType>(key));
        }

        /// <summary>
        /// 添加一个数据分析通道
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <param name="maxSize">缓存数据的最大容量</param>
        /// <returns> 添加成功返回 true </returns>
        public bool AddChannel(TChannelKey key, int maxSize)
        {
            if (key == null) return false;
            if (maxSize <= 0)
            {
                Console.WriteLine($"Error:最大缓存大小设置错误");
                return false;
            }

            return Channels.TryAdd(key, new Channel<TChannelKey, TDataType>(key, maxSize));
        }

        /// <summary>
        /// 获取数据通道对象；不会抛出异常信息，如果通道不存在，则返回 null 值
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns> </returns>
        public Channel<TChannelKey, TDataType> GetChannel(TChannelKey key)
        {
            if (key == null) return null;

            Channel<TChannelKey, TDataType> channel = null;
            bool result = Channels.TryGetValue(key, out channel);

            return result ? channel : null;
        }

        /// <summary>
        /// 移除一个数据分析通道，移除会清除缓存数据
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <returns>移除成功返回 true </returns>
        public bool RemoveChannel(TChannelKey key)
        {
            if (key == null) return false;

            Channel<TChannelKey, TDataType> channel = null;
            bool result = Channels.TryRemove(key, out channel);
            if (result) channel.Dispose();

            return result;
        }

        /// <summary>
        /// 清除所有通道，及其通道缓存数据
        /// </summary>
        public void ClearChannels()
        {
            var results = from Channel in Channels
                          select RemoveChannel(Channel.Key);
            Channels.Clear();
        }
        
        /// <summary>
        /// 分析指定通道的数据
        /// <para>考虑到如果在线程中运行，不建议抛出异常信息，直接返回 bool 值，参数错误 或 数据错误 返回 false</para>
        /// </summary>
        /// <param name="key">通道的唯一标识键，不能为 null 值，或无效引用</param>
        /// <param name="data">需要分析或缓存的数据</param>
        /// <param name="analyseResultHandler">分析产生结果时的代理调用</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <returns> 返回数据分析状态 </returns>
        public virtual bool AnalyseChannel(TChannelKey key, IReadOnlyList<TDataType> data, AnalyseResultHandler<TChannelKey, TResultType> analyseResultHandler)
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
        protected virtual TResultType ConvertResultType(IReadOnlyList<TDataType> packet, params object[] args)
        {
            //2. 数据解析转换在这里实现
            return default;
        }

    }
}
