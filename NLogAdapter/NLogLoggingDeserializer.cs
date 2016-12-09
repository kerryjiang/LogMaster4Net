using System;
using System.Collections.Generic;
using System.Collections.Specialized;
#if DOTNETCORE
using System.Composition;
#else
using System.ComponentModel.Composition;
#endif
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
        private static readonly DateTime m_Log4jDateBase = new DateTime(1970, 1, 1);

        public NLogLoggingDeserializer()
            : base(new Dictionary<string, Action<LoggingData, XmlReader>>(StringComparer.OrdinalIgnoreCase)
            {
                { "logger", (l, r) => l.LoggerName = r.Value },
                { "domain", (l, r) => l.Domain = r.Value },
                { "username", (l, r) => l.UserName = r.Value },
                { "thread", (l, r) => l.ThreadName = r.Value },
                { "level", (l, r) => l.Level = r.Value },
                { "timestamp", (l, r) => l.TimeStamp = m_Log4jDateBase.AddMilliseconds(long.Parse(r.Value)).ToLocalTime() },
                { "message", (l, r) => l.Message = r.ReadElementContentAsString() },
                { "properties", ReadProperties },
                { "throwable", ReadException },
                { "locationInfo", XmlElementParser.CreateParser<LocationInfo>(new Dictionary<string, Action<LocationInfo, XmlReader>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "class", (l, r) => l.ClassName = r.Value },
                        { "method", (l, r) => l.MethodName = r.Value },
                        { "file", (l, r) => l.FileName = r.Value },
                        { "line", (l, r) => l.LineNumber = r.Value }
                    }, (log, location) =>
                        {
                            if (log.LocationInfo == null)
                                log.LocationInfo = location;
                        })
                }
            })
        {

        }

        protected override void RegisterXmlNamespaces(XmlNamespaceManager namespaceManager)
        {
            namespaceManager.AddNamespace("log4j", "urn:log4j");
            namespaceManager.AddNamespace("nlog", "urn:nlog");
        }

        protected override void Prepare(LoggingData logging)
        {
            if (logging.Properties != null)
                logging.ApplicationName = logging.Properties["log4japp"];
        }
    }
}
