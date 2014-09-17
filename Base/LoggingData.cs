using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace LogMaster4Net.Base
{
    public class LoggingData
    {
        public string ApplicationName { get; set; }

        public string LoggerName { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string ThreadName { get; set; }

        public DateTime TimeStamp { get; set; }

        public LocationInfo LocationInfo { get; set; }

        public string UserName { get; set; }

        public string Identity { get; set; }

        public string ExceptionString { get; set; }

        public string Domain { get; set; }

        public StringDictionary Properties { get; set; }
    }
}
