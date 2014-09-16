using LogMaster4Net.Base;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMaster4Net.MasterServer
{
    public class LoggingPackageInfo : IPackageInfo<string>
    {
        public LoggingPackageInfo(LoggingData data)
        {
            Data = data;
        }
        public LoggingData Data { get; private set; }

        public string Key
        {
            get { return Data.ApplicationName; }
        }
    }
}
