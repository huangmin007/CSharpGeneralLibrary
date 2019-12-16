using System;
using System.Threading;
using System.Threading.Tasks;
using SpaceCG.HPSocket;
using SpaceCG.WindowsAPI.Kernel32;
using System.Net.NetworkInformation;
using System.Net;

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
            
            int Timeout = 1000;         // 等待重新连接挂起的毫秒数
            bool IsAvailable = true;    // 网络是否可用            
            bool IsLocalAddress = address == "0.0.0.0" || address == "127.0.0.1";   // 是否连接的为本地地址

            //监听事件
            client.OnConnect += (IClient sender) =>
            {
                Timeout = 1000;
                ushort localPort = 0;
                string localAddr = null;                
                client.GetListenAddress(ref localAddr, ref localPort);
                Log?.InfoFormat("客户端 {0}:{1} 连接成功", localAddr, localPort);

                return HandleResult.Ok;
            };
            client.OnReceive += (IClient sender, byte[] bytes) =>
            {
                receivedCallback?.Invoke(bytes);
                return HandleResult.Ok;
            };
            client.OnClose += (IClient sender, SocketOperation enOperation, int errorCode) =>
            {
                string message = Kernel32Utils.GetSysErrroMessage((uint)errorCode);
                Log?.InfoFormat("客户端连接被断开({0})，描述：({1}) {2}", enOperation, errorCode, message);

                if (IsAvailable && autoConnect)
                {
                    Log?.InfoFormat("客户端等待 {0}ms 后重尝试重新连接", Timeout);

                    Task.Run(() =>
                    {
                        Thread.Sleep(Timeout);
                        client.Connect(address, port, true);
                        Timeout += 1000;
                    });
                }

                return HandleResult.Ignore;
            };

            //连接服务
            bool result = client.Connect(address, port, true);

            //非本机地址，网络的可用性监听
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in entry.AddressList)  IsLocalAddress = IsLocalAddress || address == ip.ToString();
            if (!IsLocalAddress)
            {
                NetworkChange.NetworkAvailabilityChanged += (object sender, NetworkAvailabilityEventArgs e) =>
                {
                    Log?.InfoFormat("网络的可用性发生变化，Network Change, IsAvailable : {0}", e.IsAvailable);

                    Timeout = 1000;
                    IsAvailable = e.IsAvailable;

                    if (!e.IsAvailable && client.IsConnected) client.Stop();
                    if (e.IsAvailable && !client.IsConnected) client.Connect(address, port, true);
                };
            }
            else
            {
                Log?.InfoFormat("客户端连接的为本地网络服务地址：{0} ，未监听网络的可用性变化。", address);
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
