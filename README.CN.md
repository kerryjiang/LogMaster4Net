LogMaster4Net
=============

**LogMaster4Net** 是一个能够接收其他程序发来的日志信息并将它们按照你的要求的来处理的日志服务器软件。 它能帮助你在一个中心位置管理多个程序的日志。因此如果在你的系统中有很多程序在运行而且他们都有自己的日志功能的话，本软件将会对你非常有用。

**LogMaster4Net** 实际上是一个基于 **SuperSocket** 的 UDP 服务器。


1. 服务器配置

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

2. 服务日志配置文件

		\Config\log4net.config: 服务器自己的日志配置文件;
		\Config\log4net.[LogAppName].Config : 名为[LogAppName]的日志配置文件;

3. 程序设置
	- 设置 LogAppName

			log4net.GlobalContext.Properties["LogAppName"] = "MyTool1";

	- 日志配置
	
			<appender name="udpAppender" type="log4net.Appender.UdpAppender">
		      <remoteAddress value="[ServerAddress]" />
		      <remotePort value="2020" />
		      <layout type="log4net.Layout.XmlLayout">
		          <locationInfo value="true" />
		      </layout>
		    </appender>



*使用此服务器暂时必须使用log4net作为你的日志组件。 LogMaster4Net 以后将支持其它的日志组件*
