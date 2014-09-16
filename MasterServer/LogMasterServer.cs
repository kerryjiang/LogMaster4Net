using LogMaster4Net.Base;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogMaster4Net.MasterServer
{
    public class LogMasterServer : AppServer<LoggingSession, LoggingPackageInfo>
    {
        private ILog m_LogMasterLog;

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

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            m_LogMasterLog = LogFactory.GetLog("LogMaster");

            //TODO: setup m_LoggingDeserializer

            return base.Setup(rootConfig, config);
        }

        private Timer m_Timer;

        protected override void OnStarted()
        {
            m_Timer = new Timer(OnTimerCallback, null, 1000 * 60, 1000 * 60);
        }

        private void OnTimerCallback(object state)
        {
            m_LogMasterLog.InfoFormat("Current time: {0}", DateTime.Now);
        }

        protected override void OnStopped()
        {
            m_Timer.Dispose();
            m_Timer = null;
        }

        void LogMasterServer_NewRequestReceived(LoggingSession session, LoggingPackageInfo requestInfo)
        {
            
        }
    }
}
