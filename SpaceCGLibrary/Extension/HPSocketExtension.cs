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
        /// 创建 HPSocket.TcpClient 客户端连接对象。
        /// <para>请使用 <see cref="DisposeConnect(ref TcpClient, log4net.ILog)"/> 断开销毁客户端对象。 </para>
        /// </summary>
        /// <param name="address">服务端地址</param>
        /// <param name="port">服务端端口</param>
        /// <param name="receivedCallback">数据接收回调</param>
        /// <param name="autoConnection">断开连接后是否等待 3000ms 后自动重新连接</param>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static TcpClient CreateConnect(string address, ushort port, Action<byte[]> receivedCallback, bool autoConnection, log4net.ILog Log = null)
        {
            TcpClient client = new TcpClient();

            client.OnReceive += (IClient sender, byte[] bytes) =>
            {
                receivedCallback?.Invoke(bytes);
                return HandleResult.Ok;
            };
            client.OnClose += (IClient sender, SocketOperation enOperation, int errorCode) =>
            {
                string message = Kernel32Utils.GetSysErrroMessage((uint)errorCode);
                Log?.InfoFormat("客户端连接被断开({0})，描述：({1}) {2}", enOperation, errorCode, message);

                if (autoConnection)
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

            client.Connect(address, port, true);

            return client;
        }

        /// <summary>
        /// 销毁由 <see cref="CreateConnect"/> 创建的客户端对象
        /// </summary>
        /// <param name="client"></param>
        /// <param name="Log"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DisposeConnect(ref TcpClient client, log4net.ILog Log = null)
        {
            if (client == null) throw new ArgumentNullException("参数不能为空");

            ushort localPort = 0;
            string localAddr = "C";
            client.GetListenAddress(ref localAddr, ref localPort);

            SpaceCGUtils.RemoveAnonymousEvents(client, "OnClose");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnConnect");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnReceive");

            client.Destroy();
            client = null;

            Log?.InfoFormat("客户端 {0}:{1} 主动断开并销毁释放", localAddr, localPort);
        }
    }
}
