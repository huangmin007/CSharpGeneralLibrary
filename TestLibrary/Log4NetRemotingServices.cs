using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using log4net.Layout;

namespace TestLibrary
{
    public class Log4NetRemotingServices
    {
        //TcpChannel channel;

        public Log4NetRemotingServices()
        {
            TcpChannel serverChannel = new TcpChannel(9090);
            //TcpChannel serverChannel = new TcpChannel(9090);
            Console.WriteLine(serverChannel.ChannelName);
            ChannelServices.RegisterChannel(serverChannel, false);
            //RemotingConfiguration.ApplicationName = "RemotingAppender";

            Console.WriteLine("The name of the channel is {0}.", serverChannel.ChannelName);
            Console.WriteLine("The priority of the channel is {0}.", serverChannel.ChannelPriority);

            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteObject), "RemoteObject.rem", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterActivatedServiceType(typeof(RemoteObject));
            RemotingServices.Marshal(new RemoteObject());
        }
    }

    public class RemoteObject : MarshalByRefObject, log4net.Appender.RemotingAppender.IRemoteLoggingSink
    {
        public RemoteObject()
        {

        }

        void RemotingAppender.IRemoteLoggingSink.LogEvents(LoggingEvent[] events)
        {
            Console.WriteLine("remote...");
            foreach (LoggingEvent evt in events)
            {
                String text = string.Empty;
                text = evt.LoggerName + "-" + evt.RenderedMessage + Environment.NewLine;
                Console.WriteLine(text);
            }
        }
    }
}
