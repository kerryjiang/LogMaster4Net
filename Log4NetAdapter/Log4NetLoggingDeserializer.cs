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
            m_AttrAssignersDict = new Dictionary<string, Action<LoggingData, XmlReader>>(StringComparer.OrdinalIgnoreCase)
            {
                {"logger", (l, r) => l.LoggerName = r.Value},
                {"domain", (l, r) => l.Domain = r.Value},
                {"username", (l, r) => l.UserName = r.Value},
                {"thread", (l, r) => l.ThreadName = r.Value},
                {"level", (l, r) => l.Level = r.Value},
                {"timestamp", (l, r) => l.TimeStamp = DateTime.Parse(r.Value)},
                {"message", (l, r) => l.Message = r.ReadElementContentAsString()},
                {"properties", ReadProperties},
                {"exception", ReadException}
            };

            NameTable nt = new NameTable();
            m_XmlNamespaceManager = new XmlNamespaceManager(nt);
            m_XmlNamespaceManager.AddNamespace("log4net", "urn:log4net");
        }

        private static void ReadException(LoggingData logging, XmlReader reader)
        {
            reader.Read();
            logging.ExceptionString = reader.Value;
            reader.Read();
            reader.ReadEndElement();
        }

        private static void ReadProperties(LoggingData logging, XmlReader reader)
        {
            reader.MoveToContent();

            var properties = new StringDictionary();

            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                if (!reader.MoveToFirstAttribute())
                {
                    reader.MoveToElement();
                    continue;
                }                   

                var key = reader.Value;

                if (!reader.MoveToNextAttribute())
                {
                    reader.MoveToElement();
                    continue;
                }

                var value = reader.Value;

                properties.Add(key, value);
                reader.MoveToElement();
            }

            logging.Properties = properties;

            reader.ReadEndElement();
        }

        public IList<LoggingData> Deserialize(string log)
        {
            var logs = new List<LoggingData>();

            var context = new XmlParserContext(null, m_XmlNamespaceManager, null, XmlSpace.None);
            var xmlReader = new XmlTextReader(log, XmlNodeType.Element, context)
            {
                WhitespaceHandling = WhitespaceHandling.None
            };

            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.None)
            {
                logs.Add(ReadLoggingData(xmlReader));
            }

            return logs;
        }


        LoggingData ReadLoggingData(XmlReader xmlReader)
        {

            var logging = new LoggingData();

            xmlReader.MoveToContent();

            Action<LoggingData, XmlReader> attrAssigner;

            if (xmlReader.MoveToFirstAttribute())
            {
                if (m_AttrAssignersDict.TryGetValue(xmlReader.Name, out attrAssigner))
                    attrAssigner(logging, xmlReader);

                while (xmlReader.MoveToNextAttribute())
                {
                    if (m_AttrAssignersDict.TryGetValue(xmlReader.Name, out attrAssigner))
                        attrAssigner(logging, xmlReader);
                }
            }

            xmlReader.MoveToElement();

            if (xmlReader.Read() && xmlReader.NodeType != XmlNodeType.EndElement)
            {
                while (xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    if (string.IsNullOrEmpty(xmlReader.LocalName))
                        break;

                    if (m_AttrAssignersDict.TryGetValue(xmlReader.LocalName, out attrAssigner))
                        attrAssigner(logging, xmlReader);
                    else
                        xmlReader.Read();
                }
            }

            if (logging.Properties != null)
                logging.ApplicationName = logging.Properties["LogAppName"];

            xmlReader.ReadEndElement();

            return logging;
        }

    }
}
