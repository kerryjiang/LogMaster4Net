using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using AnyLog;
using LogMaster4Net.Base;

namespace LogMaster4Net.NLogAdapter
{
    [Export(typeof(ILoggingDeserializer))]
    [LoggingDeserializerMetadata("NLog")]
    public class NLogLoggingDeserializer : ILoggingDeserializer
    {
        public IList<LoggingData> Deserialize(string log)
        {
            throw new NotImplementedException();
        }
    }
}
