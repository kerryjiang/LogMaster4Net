using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace LogMaster4Net.Base
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LoggingDeserializerMetadataAttribute : ExportAttribute, ILoggingDeserializerMetadata
    {
        public string Name { get; private set; }

        public LoggingDeserializerMetadataAttribute(string name)
        {
            Name = name;
        }
    }
}
