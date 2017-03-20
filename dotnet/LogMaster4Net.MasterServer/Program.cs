using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogMaster4Net.MasterServer
{
    public class Program
    {
        private static ILogger _logger;

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("app.json", true)
                .AddCommandLine(args)
                .Build();

            var services = new ServiceCollection();
            services.AddOptions().Configure<ServerConfig>(config);
            var serviceProvider = services.BuildServiceProvider();
            var serverConfigOptions = serviceProvider.GetService<IOptions<ServerConfig>>();

            var serverConfig = serverConfigOptions.Value;

            if (serverConfig == null)
                return;

            var factory = new LoggerFactory();
            factory.AddConsole();
            _logger = factory.CreateLogger("MasterServer");
            _logger.LogInformation("Hello World!");

            foreach (var listner in serverConfig.Listners)
            {
                var ipAddress = IPAddress.Parse(listner.Ip);
                var listenerEndPoint = new IPEndPoint(ipAddress, listner.Port);
                var socket = new Socket(ipAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(listenerEndPoint);
                socket.Listen(100);

                var sae = new SocketAsyncEventArgs();
                var buffer = new byte[1024 * 1024 * 2];                
                sae.SetBuffer(buffer, 0, buffer.Length);
                sae.Completed += OnPackageReceived;
                sae.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                if(!socket.ReceiveFromAsync(sae))
                {
                    OnPackageReceived(socket, sae);
                }
            }
        }

        private static void OnPackageReceived(object sender, SocketAsyncEventArgs e)
        {
            var socket = sender as Socket;

            var log = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);

            Task.Run(() => ProcessLog(log));

            if(!socket.ReceiveFromAsync(e))
            {
                OnPackageReceived(socket, e);
            }
        }
        
        private static void ProcessLog(string remoteLog)
        {

        }
    }
}
