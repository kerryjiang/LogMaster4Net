using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
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
        public LogMasterServer()
            : base(new DefaultReceiveFilterFactory<LogReceiveFilter, StringPackageInfo>())
        {
            this.NewRequestReceived += LogMasterServer_NewRequestReceived;
        }

        private Timer m_Timer;

        protected override void OnStarted()
        {
            m_Timer = new Timer(OnTimerCallback, null, 5000, 5000);
        }

        private void OnTimerCallback(object state)
        {
            Logger.InfoFormat("Current time: {0}", DateTime.Now);
        }

        void LogMasterServer_NewRequestReceived(AppSession session, StringPackageInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
        }
    }
}
