using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogMaster4Net.Base;
using SuperSocket.Common;
using SuperSocket.SocketBase.CompositeTargets;
using SuperSocket.SocketBase.Config;

namespace LogMaster4Net.MasterServer
{
    class LoggingDeserializerCompositeTarget : SingleResultCompositeTargetCore<ILoggingDeserializer, ILoggingDeserializerMetadata>
    {
        public LoggingDeserializerCompositeTarget(Action<ILoggingDeserializer> callback)
            : base((config) => config.Options.GetValue("loggingDeserializer"), callback, true)
        {

        }

        protected override bool MetadataNameEqual(ILoggingDeserializerMetadata metadata, string name)
        {
            return metadata.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
