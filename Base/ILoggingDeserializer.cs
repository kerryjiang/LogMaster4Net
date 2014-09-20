using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.Base
{
    public interface ILoggingDeserializer
    {
        IList<LoggingData> Deserialize(string log);
    }
}
