using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SpaceCG.WindowsAPI.Kernel32;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace SpaceCG.Extension
{
    /// <summary>
    /// HPSocket 扩展类/功能
    /// </summary>
    public static partial class HPSocketExtension
    {
        /// <summary>
        /// 创建 HPSocket.IClient 客户端连接对象，并创建匿名监听事件。如果只关心数据的接收/处理，适用此方法，其它事件状态会记录在日志中。
        /// <para>建议使用 <see cref="DisposeClient(HPSocket.IClient, log4net.ILog)"/> 断开销毁客户端对象。 </para>
        /// <para> &lt;IClient&gt; 类型约束实现 <see cref="HPSocket.IClient"/> 接口，参考: <see cref="HPSocket.Tcp.TcpClient"/>, <see cref="HPSocket.Udp.UdpClient"/>, <see cref="HPSocket.Udp.UdpCast"/> ... </para>
        /// </summary>
        /// <typeparam name="IClient">&lt;IClient&gt; 类型约束实现 <see cref="HPSocket.IClient"/> 接口，参考: <see cref="HPSocket.Tcp.TcpClient"/>, <see cref="HPSocket.Udp.UdpClient"/>, <see cref="HPSocket.Udp.UdpCast"/> ...</typeparam>
        /// <param name="remoteAddress">远程服务端地址</param>
        /// <param name="remotePort">远程服务端端口</param>
        /// <param name="receivedCallback">数据接收回调</param>
        /// <param name="autoConnect">断开后是否自动连接，等待 3000ms 后重新连接</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>返回 <see cref="HPSocket.IClient"/> 实例对象。</returns>
        public static IClient CreateClient<IClient>(string remoteAddress, ushort remotePort, Action<HPSocket.IClient, byte[]> receivedCallback, bool autoConnect = true) 
            where IClient : class, HPSocket.IClient, new()
        {
            if (string.IsNullOrWhiteSpace(remoteAddress) || remotePort <= 0 || receivedCallback == null)
                throw new ArgumentException("参数设置错误");

            IClient client = new IClient()
            {
                Async = true,
                Port = remotePort,
                Address = remoteAddress,
                FreeBufferPoolSize = 64,        //默认：60
                FreeBufferPoolHold = 64 * 3,    //默认：60
            };            

            int Timeout = 1000;         // 等待重新连接挂起的毫秒数
            //client.ExtraData = true;
            bool IsAvailable = true;    // 网络是否可用，指本机网络配置，或是物理网络是否断开
            // 检查远程服务地址，是否为本机地址        
            bool IsLocalAddress = remoteAddress == "0.0.0.0" || remoteAddress == "127.0.0.1";
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in entry.AddressList) IsLocalAddress = IsLocalAddress || remoteAddress == ip.ToString();

            //监听事件
            client.OnConnect += (HPSocket.IClient sender) =>
            {
                Timeout = 1000;
                ushort localPort = 0;
                string localAddr = null;                
                client.GetListenAddress(out localAddr, out localPort);
                SpaceCGUtils.Log.InfoFormat("客户端({0}) {1}:{2} 连接成功", typeof(IClient), localAddr, localPort);

                return HPSocket.HandleResult.Ok;
            };
            client.OnReceive += (HPSocket.IClient sender, byte[] data) =>
            {
                receivedCallback?.Invoke(sender, data);
                return HPSocket.HandleResult.Ok;
            };
            client.OnClose += (HPSocket.IClient sender, HPSocket.SocketOperation enOperation, int errorCode) =>
            {
                string message = Kernel32Extension.GetSysErrroMessage((uint)errorCode);
                SpaceCGUtils.Log.InfoFormat("客户端({0})连接被断开({1})，描述：({2}) {3}", typeof(IClient), enOperation, errorCode, message);

                if (IsAvailable && autoConnect)
                //if (client.ExtraData != null && (bool)client.ExtraData && autoConnect)
                {
                    SpaceCGUtils.Log.InfoFormat("客户端等待 {0}ms 后，重尝试重新连接", Timeout);
                    
                    Task.Run(() =>
                    {
                        Thread.Sleep(Timeout);
                        Console.WriteLine("client....client");
                        client.Connect();
                        Timeout = Timeout >= 10000 ? Timeout : Timeout + 1000;
                    });
                }

                return HPSocket.HandleResult.Ignore;
            };

            //连接服务
            bool result = client.Connect();

            // 非本机地址，监听网络的可用性
            if (!IsLocalAddress)
            {
                NetworkChange.NetworkAvailabilityChanged += (object sender, NetworkAvailabilityEventArgs e) =>
                {
                    if (client == null) return;
                    SpaceCGUtils.Log.InfoFormat("网络的可用性发生变化，Network Change, IsAvailable : {0}", e.IsAvailable);

                    Timeout = 1000;
                    IsAvailable = e.IsAvailable;
                    //client.ExtraData = e.IsAvailable;

                    if (!e.IsAvailable && client.IsConnected) client.Stop();
                    if (e.IsAvailable && !client.IsConnected) client.Connect();
                };
            }
            else
            {
                SpaceCGUtils.Log.InfoFormat("客户端({0})连接的为本地网络服务地址：{1} ，未监听网络的可用性变化。", typeof(IClient), remoteAddress);
            }

            return client;
        }

        /// <summary>
        /// 销毁由 <see cref="CreateClient"/> 创建的客户端对象
        /// <para>注意：静态函数，非引用参数 client, 需实例变量 设为 null </para>
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DisposeClient(this HPSocket.IClient client)
        {
            Console.WriteLine("connectid:{0}", client.ConnectionId);

            if (client == null) throw new ArgumentNullException("参数 client 不能为空");
            //if(client.ExtraData != null && client.ExtraData is bool) client.ExtraData = false;

            ushort localPort = 0;
            string localAddr = "0.0.0.0";
            Type type = client.GetType();

            if (client.IsConnected)
                client.GetListenAddress(out localAddr, out localPort);

            SpaceCGUtils.RemoveAnonymousEvents(client, "OnClose");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnConnect");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnReceive");

            SpaceCGUtils.RemoveAnonymousEvents(client, "OnSend");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnHandShake");
            SpaceCGUtils.RemoveAnonymousEvents(client, "OnPrepareConnect");

            client.Dispose();
            client = null;

            SpaceCGUtils.Log.InfoFormat("客户端({0}) {1}:{2} 断开连接并销毁释放", type, localAddr, localPort);
        }


        /// <summary>
        /// 创建简单 HPSocket.IServer 服务端对象，并创建匿名监听事件。
        /// <para>建议使用 <see cref="DisposeServer(HPSocket.IServer, log4net.ILog)"/> 断开销毁客户端对象。 </para>
        /// <para>&lt;IServer&gt; 类型约束实现 <see cref="HPSocket.IServer"/> 接口，参考: <see cref="HPSocket.Tcp.TcpServer"/>, <see cref="HPSocket.Udp.UdpServer"/> ... </para>
        /// </summary>
        /// <typeparam name="IServer">&lt;IServer&gt; 类型约束实现 <see cref="HPSocket.IServer"/> 接口，参考: <see cref="HPSocket.Tcp.TcpServer"/>, <see cref="HPSocket.Udp.UdpServer"/> ... </typeparam>
        /// <param name="localPort">绑定的本机端口号</param>
        /// <param name="receivedCallback">客户端数据接收回调函数</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>返回 <see cref="HPSocket.IServer"/> 实例对象。</returns>
        public static IServer CreateServer<IServer>(ushort localPort, Action<IntPtr, byte[]> receivedCallback)
            where IServer : class, HPSocket.IServer, new()
        {
            if (localPort <= 0 || receivedCallback == null) throw new ArgumentException("参数不能为空");

            IServer server = new IServer()
            {
                Port = localPort,
                MaxConnectionCount = 255,
            };

            server.OnAccept += (HPSocket.IServer sender, IntPtr connId, IntPtr client) =>
            {
                ushort port = 0;
                string ip = "0.0.0.0";
                server.GetRemoteAddress(connId, out ip, out port);
                SpaceCGUtils.Log.InfoFormat("客户端 {0}:{1} 连接成功", ip, port);

                return HPSocket.HandleResult.Ok;
            };
            server.OnReceive += (HPSocket.IServer sender, IntPtr connId, byte[] data) =>
            {
                receivedCallback?.Invoke(connId, data);
                return HPSocket.HandleResult.Ok;
            };
            server.OnClose += (HPSocket.IServer sender, IntPtr connId, HPSocket.SocketOperation socketOperation, int errorCode) =>
            {
                ushort port = 0;
                string ip = "0.0.0.0";
                server.GetRemoteAddress(connId, out ip, out port);
                string message = Kernel32Extension.GetSysErrroMessage((uint)errorCode);
                SpaceCGUtils.Log.InfoFormat("客户端 {0}:{1} 断开连接({2})，描述：({3}) {4}", ip, port, socketOperation, errorCode, message);

                return HPSocket.HandleResult.Ok;
            };

            bool result = server.Start();
            SpaceCGUtils.Log.InfoFormat("服务端({0}) {1}:{2} {3}", typeof(IServer), server.Address, server.Port, result ? "已启动监听" : "启动失败");

            return server;
        }

        /// <summary>
        /// HPSokcet.IServer 广播数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="connId">不对此 connId 广播，或可为 null </param>
        /// <param name="bytes"></param>
        public static void Broadcast(this HPSocket.IServer server, IntPtr connId, byte[] bytes)
        {
            if (server == null) throw new ArgumentNullException("参数 server 不能为空");

            List<IntPtr> clients = server.GetAllConnectionIds();
            foreach(IntPtr client in clients)
            {
                if (client != connId && server.IsConnected(client))
                    server.Send(client, bytes, bytes.Length);
            }
        }

        /// <summary>
        /// 销毁由 <see cref="CreateServer"/> 创建的服务端对象
        /// <para>注意：静态函数，非引用参数 server, 需实例变量 设为 null </para>
        /// </summary>
        /// <param name="server"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void DisposeServer(this HPSocket.IServer server)
        {
            if (server == null) throw new ArgumentNullException("参数 server 不能为空");

            ushort port = server.Port;
            string address = server.Address;
            Type type = server.GetType();

            SpaceCGUtils.RemoveAnonymousEvents(server, "OnSend");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnClose");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnAccept");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnReceive");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnShutdown");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnHandShake");
            SpaceCGUtils.RemoveAnonymousEvents(server, "OnPrepareListen");

            List<IntPtr> clients = server.GetAllConnectionIds();
            foreach (IntPtr client in clients)
                if(server.IsConnected(client))  server.Disconnect(client, true);

            server.Dispose();
            server = null;

            SpaceCGUtils.Log.InfoFormat("服务端({0}) {1}:{2} 已停止服务并销毁释放", type, address, port);
        }

    }
}
