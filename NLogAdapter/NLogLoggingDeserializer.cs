using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using LogMaster4Net.Base;
using SuperSocket.SocketBase.Logging;

namespace LogMaster4Net.NLogAdapter
{
    [Export(typeof(ILoggingDeserializer))]
    [ExportMetadata("Name", "NLog")]
    public class NLogLoggingDeserializer : ILoggingDeserializer
    {
        public IList<LoggingData> Deserialize(string log)
        {
            throw new NotImplementedException();
        }
    }
}
