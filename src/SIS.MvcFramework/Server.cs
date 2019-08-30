using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.Common;
using SIS.HTTP.Common;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.Sessions;

namespace SIS.MvcFramework
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly IServerRoutingTable serverRoutingTable;

        private bool isRunning;
        private readonly IHttpSessionStorage sessionStorage;

        public Server(int port, IServerRoutingTable serverRoutingTable, IHttpSessionStorage sessionStorage)
        {
            serverRoutingTable.ThrowIfNull(nameof(serverRoutingTable));

            this.port = port;
            this.serverRoutingTable = serverRoutingTable;
            
            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), port);
            this.sessionStorage = sessionStorage;
        }

        private async Task ListenAsync(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable,this.sessionStorage);
            await connectionHandler.ProcessRequestAsync();
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                //Console.WriteLine("Waiting for client...");

                var client = this.tcpListener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => this.ListenAsync(client));
            }
        }
    }
}
