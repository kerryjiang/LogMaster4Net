using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.MasterServer
{
    public class LogReceiveFilter : IReceiveFilter<LoggingPackageInfo>
    {
        public LoggingPackageInfo Filter(BufferList data, out int rest)
        {
            var session = AppContext.CurrentSession as LoggingSession;
            var loggingDeserialzier = session.AppServer.LoggingDeserializer;

            rest = 0;

            using (var reader = this.GetBufferReader(data))
            {
                var logging = reader.ReadString(data.Total, Encoding.UTF8);

                try
                {
                    return new LoggingPackageInfo(loggingDeserialzier.Deserialize(logging));
                }
                catch(Exception e)
                {
                    session.Logger.ErrorFormat("Failed to deserialize the logging:\r\n{0}", logging);
                    throw e;
                }                
            }
        }

        public IReceiveFilter<LoggingPackageInfo> NextReceiveFilter { get; private set; }

        public void Reset()
        {

        }

        public FilterState State { get; private set; }
    }
}
