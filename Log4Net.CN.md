LogMaster4Net 用法之 Log4Net
=============

1. 所需的程序集

		- log4net.dll
		- AnyLog.Log4Net.dll
		- LogMaster4Net.Log4NetAdapter.dll
	



2. 服务器配置

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

3. 服务日志配置文件

		\Config\log4net.config: 服务器自己的日志配置文件;
		\Config\log4net.[LogAppName].Config : 名为[LogAppName]的日志配置文件;

4. 程序设置
	- 设置 LogAppName

			log4net.GlobalContext.Properties["LogAppName"] = "MyTool1";

	- 日志配置
	
			<appender name="udpAppender" type="log4net.Appender.UdpAppender">
		      <remoteAddress value="[ServerAddress]" />
		      <remotePort value="2020" />
			  <encoding value="utf-8"/>
		      <layout type="log4net.Layout.XmlLayout">
		          <locationInfo value="true" />
		      </layout>
		    </appender>
