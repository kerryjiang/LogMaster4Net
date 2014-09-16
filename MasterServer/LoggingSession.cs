using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.MasterServer
{
    public class LoggingSession : AppSession<LoggingSession, LoggingPackageInfo>
    {
        internal new LogMasterServer AppServer
        {
            get { return base.AppServer as LogMasterServer; }
        }
    }
}
