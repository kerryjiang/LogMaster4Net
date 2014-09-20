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
            //Task.Factory.StartNew(DoWork);
        }

        //private void DoWork()
        //{
        //    var log = LogFactory.GetLog("LogMaster");

        //    while(State == ServerState.Running)
        //    {
        //        Thread.Sleep(5000);
        //        log.InfoFormat("Current Time: {0}", DateTime.Now);
        //    }
        //}

        protected override void OnStopped()
        {

        }

        void LogMasterServer_NewRequestReceived(LoggingSession session, LoggingPackageInfo requestInfo)
        {
            var loggings = requestInfo.Data;

            for (var i = 0; i < loggings.Count; i++)
            {
                var loggingData = loggings[i];

                if (string.IsNullOrEmpty(loggingData.ApplicationName))
                    LogFactory.GetLog(loggingData.LoggerName).Log(loggingData);
                else
                    LogFactory.GetLog(loggingData.ApplicationName, loggingData.LoggerName).Log(loggingData);
            }
        }
    }
}
