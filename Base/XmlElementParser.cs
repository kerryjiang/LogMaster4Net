using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AnyLog;

namespace LogMaster4Net.Base
{
    public static class XmlElementParser
    {
        public static Action<LoggingData, XmlReader> CreateParser<T>(Dictionary<string, Action<T, XmlReader>> assigners, Action<LoggingData, T> loggingAction)
            where T : class, new()
        {
            var parser = new XmlElementParser<T>(assigners);
            return new Action<LoggingData, XmlReader>((logging, reader) => loggingAction(logging, parser.Parse(reader)));
        }
    }

    public class XmlElementParser<T>
        where T : class, new()
    {
        private Dictionary<string, Action<T, XmlReader>> m_Assigners;
        public XmlElementParser(Dictionary<string, Action<T, XmlReader>> assigners)
        {
            m_Assigners = assigners;
        }

        public T Parse(XmlReader xmlReader)
        {
            T entity = new T();

            xmlReader.MoveToContent();

            Action<T, XmlReader> attrAssigner;

            if (xmlReader.MoveToFirstAttribute())
            {
                if (m_Assigners.TryGetValue(xmlReader.Name, out attrAssigner))
                    attrAssigner(entity, xmlReader);

                while (xmlReader.MoveToNextAttribute())
                {
                    if (m_Assigners.TryGetValue(xmlReader.Name, out attrAssigner))
                        attrAssigner(entity, xmlReader);
                }
            }

            xmlReader.MoveToElement();

            if (xmlReader.IsEmptyElement)
            {
                xmlReader.Read();
                return entity;
            }

            if (xmlReader.Read() && xmlReader.NodeType != XmlNodeType.EndElement)
            {
                while (xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    if (string.IsNullOrEmpty(xmlReader.LocalName))
                        break;

                    if (m_Assigners.TryGetValue(xmlReader.LocalName, out attrAssigner))
                        attrAssigner(entity, xmlReader);
                    else
                        xmlReader.Skip();
                }
            }

            xmlReader.ReadEndElement();

            return entity;
        }
    }
}
