using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Server {
        static void Main(string[] args) {
            Server Run = new Server();
            Run.Run();
        }

        private void Run() {
            Console.WriteLine("Online");

            TcpListener Server = new TcpListener(IPAddress.Any, 12345);
            TcpClient Client;
            Server.Start();

            while (true) {
                Client = Server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ClientConnection, Client);
            }
        }

        private void ClientConnection(object obj) { // Refactor into a ClientHandler Class
            var client = (TcpClient)obj;

            ClientHandler handler = new ClientHandler(client);

            Repo.Clients.Add(handler);

            Console.WriteLine("New Connection: " + handler.IP());
            Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");

            handler.Handle();
        }

    }
}
