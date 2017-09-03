using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    public class ClientHandler {
        TcpClient client;
        StreamReader read;
        StreamWriter write;
        IPEndPoint IPEP;

        string UserName;

        public ClientHandler(TcpClient client) {
            this.client = client;
            read = new StreamReader(client.GetStream());
            write = new StreamWriter(client.GetStream());
            write.AutoFlush = true;
            IPEP = (IPEndPoint)client.Client.RemoteEndPoint;
        }

        internal IPEndPoint IP() {
            return IPEP;
        }
        internal StreamWriter Writer() {
            return write;
        }

        internal void Handle() {
            while (true) {
                try {
                    string message = read.ReadLine();
                    string[] messages = message.Split('¤');
                    if (messages[0] == "name") {
                        UserName = messages[1];
                        Console.WriteLine("Chosen Name: " + UserName);
                    }
                    
                } catch (Exception e) {
                    if (e.GetType() == typeof(IOException)) {
                        client.Close();
                        Repo.Clients.Remove(this);
                        Console.WriteLine(IPEP + " - Terminated");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }
                }
            }
        }

    }
}
