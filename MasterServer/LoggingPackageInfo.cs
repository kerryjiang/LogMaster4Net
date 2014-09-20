using LogMaster4Net.Base;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.MasterServer
{
    public class LoggingPackageInfo : IPackageInfo<string>
    {
        private const string m_PackageKey = "Logging";

        public LoggingPackageInfo(IList<LoggingData> data)
        {
            Data = data;
        }
        public IList<LoggingData> Data { get; private set; }

        public string Key
        {
            get { return m_PackageKey; }
        }
    }
}
