using LogMaster4Net.Base;
using LogMaster4Net.Log4NetAdapter;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogMaster4Net.MasterServer
{
    public class LogMasterServer : AppServer<LoggingSession, LoggingPackageInfo>
    {
        private ILoggingDeserializer m_LoggingDeserializer;

        internal ILoggingDeserializer LoggingDeserializer
        {
            get { return m_LoggingDeserializer; }
        }

        //[ImportMany]
        //private IEnumerable<Lazy<ILoggingDeserializer, ILoggingDeserializerMetadata>> m_LoggingDeserializers;

        public LogMasterServer()
            : base(new DefaultReceiveFilterFactory<LogReceiveFilter, LoggingPackageInfo>())
        {
            this.NewRequestReceived += LogMasterServer_NewRequestReceived;
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            m_LoggingDeserializer = new Log4NetLoggingDeserializer();
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            
        }

        protected override void OnStopped()
        {

        }

        void LogMasterServer_NewRequestReceived(LoggingSession session, LoggingPackageInfo requestInfo)
        {
            var loggings = requestInfo.Data;

            for (var i = 0; i < loggings.Count; i++)
            {
                var loggingData = loggings[i];

                ILog log;

                if (string.IsNullOrEmpty(loggingData.ApplicationName))
                    log = LogFactory.GetLog(loggingData.LoggerName);
                else
                    log = LogFactory.GetLog(loggingData.ApplicationName, loggingData.LoggerName);

                if(log == null)
                {
                    Logger.ErrorFormat("Failed to find a logger, LogAppName:[{0}], LoggerName: [{1}].", loggingData.ApplicationName, loggingData.LoggerName);
                    continue;
                }

                log.Log(loggingData);
            }
        }
    }
}
