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
        StreamReader sr;
        StreamWriter sw;
        IPEndPoint IPEP;

        public ClientHandler(TcpClient client) {
            this.client = client;
            sr = new StreamReader(client.GetStream());
            sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
            IPEP = (IPEndPoint)client.Client.RemoteEndPoint;
        }

        internal IPEndPoint IP() {
            return IPEP;
        }
        internal StreamWriter Writer() {
            return sw;
        }

        internal void Handle() {
            while (true) {
                try {
                    string message = sr.ReadLine();
                    string[] messages = message.Split('¤');

                    switch (messages[0]) {
                        case "ISNEW":
                            if (Repo.Players.Find(x => x.name == messages[1]) != null) {
                                sw.WriteLine("EXISTS");
                            } else {
                                sw.WriteLine("DoesntExists");
                            }
                            break;

                        case "NEW":
                            Player tempP = new Player(messages[1], messages[2]);
                            Console.WriteLine("New Player: " + tempP.name + ", " + tempP.characterClass);
                            Repo.Players.Add(tempP);
                            sw.WriteLine(tempP.experiencePoints + "¤" + tempP.gold + "¤" + tempP.hitPoints + "¤" + tempP.level);
                            break;

                        case "LOAD":
                            Player tempPLoad = Repo.Players.Find(x => x.name == messages[1]);
                            Console.WriteLine("Old Player: " + tempPLoad.name + ", " + tempPLoad.characterClass);
                            sw.WriteLine(tempPLoad.experiencePoints + "¤" + tempPLoad.gold + "¤" + tempPLoad.hitPoints + "¤" + tempPLoad.level);
                            break;
                    }

                    if (!client.Connected || message == "EXIT") {
                        Repo.Clients.Remove(this);
                        Console.WriteLine(IPEP + " - Disconnected");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
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
