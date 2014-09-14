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
    public class LogMasterServer : AppServer<AppSession>
    {
        private ILog m_LogMasterLog;

        public LogMasterServer()
            : base(new DefaultReceiveFilterFactory<LogReceiveFilter, StringPackageInfo>())
        {
            this.NewRequestReceived += LogMasterServer_NewRequestReceived;
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            m_LogMasterLog = LogFactory.GetLog("LogMaster");
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

        void LogMasterServer_NewRequestReceived(AppSession session, StringPackageInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
        }
    }
}
