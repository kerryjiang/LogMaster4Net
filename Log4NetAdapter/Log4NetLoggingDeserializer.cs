using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using AnyLog;
using LogMaster4Net.Base;

namespace LogMaster4Net.Log4NetAdapter
{
    [Export(typeof(ILoggingDeserializer))]
    [LoggingDeserializerMetadata("log4net")]
    public class Log4NetLoggingDeserializer : XmlLoggingDeserializer
    {
        public Log4NetLoggingDeserializer()
            : base()
        {

        }

        protected override Dictionary<string, Action<LoggingData, XmlReader>> GetAttrAssigners()
        {
            return new Dictionary<string, Action<LoggingData, XmlReader>>(StringComparer.OrdinalIgnoreCase)
            {
                { "logger", (l, r) => l.LoggerName = r.Value },
                { "domain", (l, r) => l.Domain = r.Value },
                { "username", (l, r) => l.UserName = r.Value },
                { "thread", (l, r) => l.ThreadName = r.Value },
                { "level", (l, r) => l.Level = r.Value },
                { "timestamp", (l, r) => l.TimeStamp = DateTime.Parse(r.Value) },
                { "message", (l, r) => l.Message = r.ReadElementContentAsString()} ,
                { "properties", ReadProperties },
                { "exception", ReadException },
                { "locationInfo", XmlElementParser.CreateParser<LocationInfo>(new Dictionary<string, Action<LocationInfo, XmlReader>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "class", (l, r) => l.ClassName = r.Value },
                        { "method", (l, r) => l.MethodName = r.Value },
                        { "file", (l, r) => l.FileName = r.Value },
                        { "line", (l, r) => l.LineNumber = r.Value }
                    }, (log, location) => log.LocationInfo = location)
                }
            };
        }

        protected override void RegisterXmlNamespaces(XmlNamespaceManager namespaceManager)
        {
            namespaceManager.AddNamespace("log4net", "urn:log4net");
        }

        protected override void AssignApplicationName(LoggingData logging)
        {
            if (logging.Properties != null)
                logging.ApplicationName = logging.Properties["LogAppName"];
        }
    }
}
