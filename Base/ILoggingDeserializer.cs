using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnyLog;

namespace LogMaster4Net.Base
{
    public interface ILoggingDeserializer
    {
        IList<LoggingData> Deserialize(string log);
    }
}
