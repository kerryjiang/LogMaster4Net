using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using AnyLog;

namespace LogMaster4Net.Base
{
    public abstract class XmlLoggingDeserializer : XmlElementParser<LoggingData>, ILoggingDeserializer
    {
        private readonly XmlNamespaceManager m_XmlNamespaceManager;

        protected XmlLoggingDeserializer(Dictionary<string, Action<LoggingData, XmlReader>> assigners)
            : base(assigners)
        {
            m_XmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            RegisterXmlNamespaces(m_XmlNamespaceManager);
        }

        protected abstract void RegisterXmlNamespaces(XmlNamespaceManager namespaceManager);

        protected static void ReadException(LoggingData logging, XmlReader reader)
        {
            reader.Read();
            logging.ExceptionString = reader.Value;
            reader.Read();
            reader.ReadEndElement();
        }

        protected static void ReadProperties(LoggingData logging, XmlReader reader)
        {
            reader.MoveToContent();

            var properties = logging.Properties;

            if(properties == null)
            {
                properties = new StringDictionary();
                logging.Properties = properties;
            }

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
                var logging = Parse(xmlReader);
                Prepare(logging);
                logs.Add(logging);
            }

            return logs;
        }

        protected abstract void Prepare(LoggingData logging);
    }
}
