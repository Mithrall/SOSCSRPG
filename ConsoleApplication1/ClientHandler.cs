using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication1 {
    public class ClientHandler {
        TcpClient client;
        StreamReader sr;
        StreamWriter sw;
        IPEndPoint IPEP;

        public ClientHandler(TcpClient client) {
            this.client = client;
            sr = new StreamReader(client.GetStream());
            sw = new StreamWriter(client.GetStream()) { AutoFlush = true };
            IPEP = (IPEndPoint)client.Client.RemoteEndPoint;
        }

        internal IPEndPoint IP() {
            return IPEP;
        }
        internal StreamWriter Writer() {
            return sw;
        }

        private void OnlineCharacter() {
            string message = "OnlineCharacter¤";
            foreach (var character in Repo.OnlineCharacters) {
                message += character.name + ": Level " + character.level + " " + character.characterClass + "¤";
            }

            foreach (var client in Repo.Clients) {
                client.Writer().WriteLine(message);
            }
        }

        internal void Handle() {
            User currentUser = new User();
            while (true) {
                try {
                    string message = sr.ReadLine();

                    //DISCONNECT CLIENT
                    if (!client.Connected || message == "EXIT") {
                        Repo.Clients.Remove(this);
                        //Repo.OnlineCharacters.Remove(currentUser);
                        Console.WriteLine(IPEP + " - Disconnected");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }

                    //READ MESSEAGE FROM CLIENT
                    string[] messages = message.Split('¤');

                    switch (messages[0]) {
                        case "ISNEW":
                            if (Repo.Users.Find(x => x.UserName == messages[1]) != null) {
                                sw.WriteLine("EXISTS");
                            } else {
                                sw.WriteLine("DoesntExists");
                            }
                            break;

                        case "NEWUSER":
                            currentUser.UserName = messages[1];
                            //TBD: LOAD ALLE CHARS TIL CLIENT CHAR SELECT SCREEN
                            Console.WriteLine("New Player: " + currentUser.UserName);
                            Repo.Users.Add(currentUser);

                            //MOVE TO NEW CHARACTER(S)
                            //Repo.OnlineCharacters.Add(currentUser);
                            //sw.WriteLine(currentUser.experiencePoints + "¤" + currentUser.gold + "¤" + currentUser.hitPoints + "¤" + currentUser.level);
                            //OnlineCharacter();
                            break;

                        case "LOAD":
                            currentUser = Repo.Users.Find(x => x.UserName == messages[1]);
                            Console.WriteLine("Old Player: " + currentUser.UserName);

                            sw.WriteLine(currentUser.Characters.ToString()); //FORMAT ?

                            //MOVE TO LOAD CHARACTER into game
                            //Repo.OnlineCharacters.Add(currentUser);
                            //sw.WriteLine(currentUser.experiencePoints + "¤" + currentUser.gold + "¤" + currentUser.hitPoints + "¤" + currentUser.level);
                            //OnlineCharacter();
                            break;
                    }

                } catch (Exception e) {
                    if (e.GetType() == typeof(IOException)) {
                        client.Close();
                        Repo.Clients.Remove(this);
                        //Repo.OnlineCharacters.Remove(currentUser);
                        Console.WriteLine(IPEP + " - Terminated");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }
                }
            }
        }

    }
}
