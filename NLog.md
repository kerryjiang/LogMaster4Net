LogMaster4Net Usage for NLog
=============

1. Required Assemblies

		- NLog.dll
		- AnyLog.NLog.dll
		- LogMaster4Net.NLogAdapter.dll
	


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
				loggingDeserializer="nlog">
	    </server>

3. Server Logging Configuration

		\Config\nlog.config: the logging confuguration of the server self;
		\Config\nlog.[LogAppName].Config : the logging configuration for your application whose name is [LogAppName]};

4. Application Setting

	- Logging Target Configuration
	
			<target xsi:type="Network"
          			name="udp"
          			encoding="utf-8"
          			address="udp://127.0.0.1:2020"
			        layout="${log4jxmlevent:includeSourceInfo=true:includeCallSite=true:includeMDC=true:appInfo=[LogAppName]:includeNDC=true:includeNLogData=true}">
    		</target>

	- Set LogAppName

	LogAppName is defined in the layout attribute of target configuration as the field "appInfo", please replace it to the value you want:

	layout="${log4jxmlevent:includeSourceInfo=true:includeCallSite=true:includeMDC=true:**appInfo=[LogAppName]**:includeNDC=true:includeNLogData=true}"



