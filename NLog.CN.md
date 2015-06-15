LogMaster4Net 用法之 NLog
=============

1. 所需的程序集

		- NLog.dll
		- AnyLog.NLog.dll
		- LogMaster4Net.NLogAdapter.dll
	



2. 服务器配置

		<server name="LogMasterServer"
	            serverType="LogMaster4Net.MasterServer.LogMasterServer, LogMaster4Net.MasterServer"
	            ip="Any" port="2020" mode="Udp"
	            maxConnectionNumber="100"
				clearIdleSession="true"
				idleSessionTimeOut="3600"
				clearIdleSessionInterval="600"
				maxRequestLength="40960"
                receiveBufferSize="40960"
				loggingDeserializer="nlog">
	    </server>

3. 服务日志配置文件

		\Config\nlog.config: the logging confuguration of the server self;
		\Config\nlog.[LogAppName].Config : the logging configuration for your application whose name is [LogAppName]};

4. 程序设置

	- 日志 Target 配置
	
			<target xsi:type="Network"
          			name="udp"
          			encoding="utf-8"
          			address="udp://127.0.0.1:2020"
			        layout="${log4jxmlevent:includeSourceInfo=true:includeCallSite=true:includeMDC=true:appInfo=[LogAppName]:includeNDC=true:includeNLogData=true}">
    		</target>

	- 设置 LogAppName

	LogAppName is defined in the layout attribute of target configuration as the field "appInfo", please replace it to the value you want:

	layout="${log4jxmlevent:includeSourceInfo=true:includeCallSite=true:includeMDC=true:**appInfo=[LogAppName]**:includeNDC=true:includeNLogData=true}"



