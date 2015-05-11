using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml;
using AnyLog;
using LogMaster4Net.Base;

namespace LogMaster4Net.NLogAdapter
{
    [Export(typeof(ILoggingDeserializer))]
    [LoggingDeserializerMetadata("NLog")]
    public class NLogLoggingDeserializer : XmlLoggingDeserializer
    {
        public NLogLoggingDeserializer()
            : base()
        {

        }

        protected override Dictionary<string, Action<LoggingData, XmlReader>> GetAttrAssigners()
        {
            return new Dictionary<string, Action<LoggingData, XmlReader>>(StringComparer.OrdinalIgnoreCase)
            {
                {"logger", (l, r) => l.LoggerName = r.Value},
                {"domain", (l, r) => l.Domain = r.Value},
                {"username", (l, r) => l.UserName = r.Value},
                {"thread", (l, r) => l.ThreadName = r.Value},
                {"level", (l, r) => l.Level = r.Value},
                {"timestamp", (l, r) => l.TimeStamp = DateTime.FromBinary(long.Parse(r.Value))},
                {"message", (l, r) => l.Message = r.ReadElementContentAsString()},
                {"properties", ReadProperties},
                {"exception", ReadException}
            };
        }

        protected override void RegisterXmlNamespaces(XmlNamespaceManager namespaceManager)
        {
            namespaceManager.AddNamespace("log4j", "urn:log4j");
            namespaceManager.AddNamespace("nlog", "urn:nlog");
        }

        protected override void AssignApplicationName(LoggingData logging)
        {
            if (logging.Properties != null)
                logging.ApplicationName = logging.Properties["log4japp"];
        }
    }
}
