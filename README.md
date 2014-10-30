LogMaster4Net
=============

**LogMaster4Net** is a central log server which can receive log messages of your other applications and organize them as your demand. It can help you to manage your all applications log messages in a central place. So if you have lots of different applications running and most of them have their own logging, this project will be pretty useful for you.

**LogMaster4Net** actually is a UDP server which is base on **SuperSocket**


1. Server Configuration

		<server name="LogMasterServer"
	            serverType="LogMaster4Net.MasterServer.LogMasterServer, LogMaster4Net.MasterServer"
	            ip="Any" port="2020" mode="Udp"
	            maxConnectionNumber="100"
				clearIdleSession="true"
				idleSessionTimeOut="3600"
				clearIdleSessionInterval="600"
				maxRequestLength="40960"
                receiveBufferSize="40960">
	    </server>

2. Server Logging Configuration

		\Config\log4net.config: the logging confuguration of the server self;
		\Config\log4net.[LogAppName].Config : the logging configuration for your application whose name is [LogAppName]};

3. Application Setting
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



*The logging framework used by the log server and applications must be log4net. LogMaster4Net will support other logging frameworks later.*
