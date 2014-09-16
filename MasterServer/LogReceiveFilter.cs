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
                return new LoggingPackageInfo(loggingDeserialzier.Deserialize(reader.ReadString(data.Total, Encoding.UTF8)));
            }
        }

        public IReceiveFilter<LoggingPackageInfo> NextReceiveFilter { get; private set; }

        public void Reset()
        {

        }

        public FilterState State { get; private set; }
    }
}
