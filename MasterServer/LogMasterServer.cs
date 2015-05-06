using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyLog;
using LogMaster4Net.Base;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace LogMaster4Net.MasterServer
{
    public class LogMasterServer : AppServer<LoggingSession, LoggingPackageInfo>
    {
        private ILoggingDeserializer m_LoggingDeserializer;

        internal ILoggingDeserializer LoggingDeserializer
        {
            get { return m_LoggingDeserializer; }
        }

        public LogMasterServer()
            : base(new DefaultReceiveFilterFactory<LogReceiveFilter, LoggingPackageInfo>())
        {
            this.NewRequestReceived += LogMasterServer_NewRequestReceived;
        }

        protected override void RegisterCompositeTarget(IList<ICompositeTarget> targets)
        {
            base.RegisterCompositeTarget(targets);
            targets.Add(new LoggingDeserializerCompositeTarget((value) => m_LoggingDeserializer = value));
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
