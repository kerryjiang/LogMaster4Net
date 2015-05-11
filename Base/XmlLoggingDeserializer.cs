using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using AnyLog;

namespace LogMaster4Net.Base
{
    public abstract class XmlLoggingDeserializer : ILoggingDeserializer
    {
        private readonly Dictionary<string, Action<LoggingData, XmlReader>> m_AttrAssignersDict;

        private readonly XmlNamespaceManager m_XmlNamespaceManager;

        protected XmlLoggingDeserializer()
        {
            m_AttrAssignersDict = GetAttrAssigners();

            m_XmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            RegisterXmlNamespaces(m_XmlNamespaceManager);
        }

        protected abstract Dictionary<string, Action<LoggingData, XmlReader>> GetAttrAssigners();

        protected abstract void RegisterXmlNamespaces(XmlNamespaceManager namespaceManager);

        protected void ReadException(LoggingData logging, XmlReader reader)
        {
            reader.Read();
            logging.ExceptionString = reader.Value;
            reader.Read();
            reader.ReadEndElement();
        }

        protected void ReadProperties(LoggingData logging, XmlReader reader)
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
                var logging = ReadLoggingData(xmlReader);
                logs.Add(logging);
            }

            return logs;
        }

        protected abstract void AssignApplicationName(LoggingData logging);

        protected virtual LoggingData ReadLoggingData(XmlReader xmlReader)
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
                        xmlReader.Skip();
                }
            }

            xmlReader.ReadEndElement();

            return logging;
        }
    }
}
