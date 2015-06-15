LogMaster4Net Usage for Log4Net
=============

1. Required Assemblies

		- log4net.dll
		- AnyLog.Log4Net.dll
		- LogMaster4Net.Log4NetAdapter.dll
	


2. Server Configuration

		<server name="LogMasterServer"
	            serverType="LogMaster4Net.MasterServer.LogMasterServer, LogMaster4Net.MasterServer"
	            ip="Any" port="2020" mode="Udp"
	            maxConnectionNumber="100"
				clearIdleSession="true"
				idleSessionTimeOut="3600"
				clearIdleSessionInterval="600"
				maxRequestLength="40960"
                receiveBufferSize="40960"
				loggingDeserializer="log4net">
	    </server>

3. Server Logging Configuration

		\Config\log4net.config: the logging confuguration of the server self;
		\Config\log4net.[LogAppName].Config : the logging configuration for your application whose name is [LogAppName]};

4. Application Setting
	- Set LogAppName

			log4net.GlobalContext.Properties["LogAppName"] = "MyTool1";

	- Logging Configuration
	
			<appender name="udpAppender" type="log4net.Appender.UdpAppender">
		      <remoteAddress value="[ServerAddress]" />
		      <remotePort value="2020" />
		      <layout type="log4net.Layout.XmlLayout">
		          <locationInfo value="true" />
		      </layout>
		    </appender>



