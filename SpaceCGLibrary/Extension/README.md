## Extension 扩展方法
    只是为了能少写些代码，或不重复写些代码。

* ### HPSocketExtension
    [HP-Socket 库](https://github.com/ldcsaa/HP-Socket)  
    [HP-Socket.Net 库](https://gitee.com/int2e/HPSocket.Net/tree/master)  
    应该类中的扩展函数，适合于只关心数据的接收/处理，其它过程只记录在日志中，或不需要处理。
```C#
// udp 是无状态连接，不需要监听连接状态
HPSocket.IClient client = HPSocketExtension.CreateClient<UdpClient>("127.0.0.1", 9999, (client, data) =>
{
    Console.WriteLine("{0} {1}", data.Length, Encoding.Default.GetString(data););
}, false, App.Log);

// tcp 中断后自动重新连接
TcpClient client = HPSocketExtension.CreateClient<TcpClient>("127.0.0.1", 9999, (client, data) =>
{
    Console.WriteLine("{0} {1}", data.Length, Encoding.Default.GetString(data););
}, true, App.Log);
```

* ### ManagementExtension
  等等

* ### SerialPortExtension
   等等