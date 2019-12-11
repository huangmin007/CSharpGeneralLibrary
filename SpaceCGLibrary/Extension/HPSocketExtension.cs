using System;
using System.Threading;
using System.Threading.Tasks;
using SpaceCG.HPSocket;
using SpaceCG.WindowsAPI.Kernel32;

namespace SpaceCG.Extension
{
    /// <summary>
    /// HPSocket 扩展类
    /// </summary>
    public static partial class HPSocketExtension
    {
        /// <summary>
        /// 创建 HPSocket.TcpClient 客户端连接对象，并监听事件。
        /// <para>建议使用 <see cref="DisposeTcpClient(ref TcpClient, log4net.ILog)"/> 断开销毁客户端对象。 </para>
        /// </summary>
        /// <param name="address">服务端地址</param>
        /// <param name="port">服务端端口</param>
        /// <param name="receivedCallback">数据接收回调</param>
        /// <param name="autoConnect">断开后是否自动连接，等待 3000ms 后重新连接</param>
        /// <param name="Log"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>返回 TcpClient 对象</returns>
        public static TcpClient CreateTcpClient(string address, ushort port, Action<byte[]> receivedCallback, bool autoConnect = true, log4net.ILog Log = null)
        {
            if (string.IsNullOrWhiteSpace(address) || port <= 0 || receivedCallback == null)
                throw new ArgumentException("参数设置错误");

            TcpClient client = new TcpClient();

            //设置参数
            client.SocketBufferSize = 1024 * 4;     //默认：4096
            client.FreeBufferPoolSize = 64;         //默认：60
            client.FreeBufferPoolHold = 64 * 3;     //默认：60

            //监听事件
            client.OnReceive += (IClient sender, byte[] bytes) =>
            {
                receivedCallback?.Invoke(bytes);
                return HandleResult.Ok;
            };
            client.OnClose += (IClient sender, SocketOperation enOperation, int errorCode) =>
            {
                string message = Kernel32Utils.GetSysErrroMessage((uint)errorCode);
                Log?.InfoFormat("客户端连接被断开({0})，描述：({1}) {2}", enOperation, errorCode, message);

                if (autoConnect)
                {
                    Log?.InfoFormat("客户端等待 3000ms 后重尝试重新连接");

                    Task.Run(() =>
                    {
                        Thread.Sleep(3000);
                        bool result = client.Connect(address, port, true);
                    });
                }

                return HandleResult.Ignore;
            };
            client.OnConnect += (IClient sender) =>
            {
                ushort localPort = 0;
                string localAddr = null;                
                client.GetListenAddress(ref localAddr, ref localPort);
                Log?.InfoFormat("客户端 {0}:{1} 连接成功", localAddr, localPort);

                return HandleResult.Ok;
            };

            //连接服务
            client.Connect(address, port, true);

            if (Log != null)
            {
                ushort localPort = 0;
                string localAddr = "0.0.0.0";
                client.GetListenAddress(ref localAddr, ref localPort);
                Log?.InfoFormat("Create TcpClient {0}:{1}", localAddr, localPort);
            }

            return client;
        }


        /// <summary>
        /// 销毁由 <see cref="CreateTcpClient"/> 创建的客户端对象
        /// <para>注意：静态函数，引用参数 client, 该方法会将 实例变量 设为 null </para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="Log"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DisposeTcpClient(ref TcpClient client, log4net.ILog Log = null)
        {
            if (client == null) throw new ArgumentNullException("参数不能为空");

            ushort localPort = 0;
            string localAddr = "0.0.0.0";
            client.GetListenAddress(ref localAddr, ref localPort);

            SpaceCGUtils.RemoveAnonymousEvents(client, "OnClose");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnConnect");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnReceive");

            client.Destroy();
            client = null;

            Log?.InfoFormat("客户端 {0}:{1} 主动断开并销毁释放", localAddr, localPort);
        }

        /// <summary>
        /// 销毁由 <see cref="CreateTcpClient"/> 创建的客户端对象
        /// <para>注意：扩展函数，需手动将实例变量设为 null</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="Log"></param>
        public static void DisposeTcpClient(this TcpClient client, log4net.ILog Log = null)
        {
            DisposeTcpClient(ref client, Log);
        }

        
    }
}
