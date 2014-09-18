using LogMaster4Net.Base;
using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LogMaster4Net.Log4NetAdapter
{
    [Export(typeof(ILoggingDeserializer))]
    [ExportMetadata("Name", "log4net")]
    public class Log4NetLoggingDeserializer : ILoggingDeserializer
    {
        private static readonly Dictionary<string, Action<LoggingData, XmlReader>> m_AttrAssignersDict;

        private static readonly XmlNamespaceManager m_XmlNamespaceManager;

        static Log4NetLoggingDeserializer()
        {
            m_AttrAssignersDict = new Dictionary<string, Action<LoggingData, XmlReader>>(StringComparer.OrdinalIgnoreCase);
            
            m_AttrAssignersDict.Add("logger", (l, r) => l.LoggerName = r.Value);
            m_AttrAssignersDict.Add("domain", (l, r) => l.Domain = r.Value);
            m_AttrAssignersDict.Add("username", (l, r) => l.UserName = r.Value);
            m_AttrAssignersDict.Add("thread", (l, r) => l.ThreadName = r.Value);
            m_AttrAssignersDict.Add("level", (l, r) => l.Level = r.Value);
            m_AttrAssignersDict.Add("timestamp", (l, r) => l.TimeStamp = DateTime.Parse(r.Value));
            m_AttrAssignersDict.Add("message", (l, r) => l.Message = r.ReadElementContentAsString());
            m_AttrAssignersDict.Add("properties", (l, r) => AssignProperties(l, r));

            NameTable nt = new NameTable();
            m_XmlNamespaceManager = new XmlNamespaceManager(nt);
            m_XmlNamespaceManager.AddNamespace("log4net", "urn:log4net");
        }

        private static void AssignProperties(LoggingData logging, XmlReader reader)
        {
            reader.MoveToContent();

            var properties = new StringDictionary();

            while (reader.Read())
            {
                if (!reader.MoveToFirstAttribute())
                    continue;

                var key = reader.Value;

                if (!reader.MoveToNextAttribute())
                    continue;

                var value = reader.Value;

                properties.Add(key, value);
            }
        }

        public LoggingData Deserialize(string log)
        {
            var logging = new LoggingData();

            var context = new XmlParserContext(null, m_XmlNamespaceManager, null, XmlSpace.None);
            var xmlReader = new XmlTextReader(log, XmlNodeType.Element, context);
            xmlReader.MoveToContent();

            Action<LoggingData, XmlReader> attrAssigner;

            if(xmlReader.MoveToFirstAttribute())
            {
                if (m_AttrAssignersDict.TryGetValue(xmlReader.Name, out attrAssigner))
                    attrAssigner(logging, xmlReader);

                while(xmlReader.MoveToNextAttribute())
                {
                    if (m_AttrAssignersDict.TryGetValue(xmlReader.Name, out attrAssigner))
                        attrAssigner(logging, xmlReader);
                }
            }

            xmlReader.MoveToElement();

            while(xmlReader.Read())
            {
                if (m_AttrAssignersDict.TryGetValue(xmlReader.LocalName, out attrAssigner))
                    attrAssigner(logging, xmlReader);
            }

            return logging;
        }
    }
}
