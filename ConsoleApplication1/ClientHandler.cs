﻿using System;
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

        private void OnlinePlayers() {
            string message = "OnlinePlayers¤";
            foreach (var player in Repo.OnlinePlayers) {
                message += player.name + ": Level" + player.level + ", " + player.characterClass + "¤";
            }

            foreach (var client in Repo.Clients) {
                client.Writer().WriteLine(message);
            }
        }

        internal void Handle() {
            Player currentPlayer = new Player();
            while (true) {
                try {
                    string message = sr.ReadLine();

                    if (!client.Connected || message == "EXIT") {
                        Repo.Clients.Remove(this);
                        Repo.OnlinePlayers.Remove(currentPlayer);
                        Console.WriteLine(IPEP + " - Disconnected");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }

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
                            currentPlayer.name = messages[1];
                            currentPlayer.characterClass = messages[2];
                            Console.WriteLine("New Player: " + currentPlayer.name + ", " + currentPlayer.characterClass);
                            Repo.Players.Add(currentPlayer);
                            Repo.OnlinePlayers.Add(currentPlayer);
                            OnlinePlayers();
                            sw.WriteLine(currentPlayer.experiencePoints + "¤" + currentPlayer.gold + "¤" + currentPlayer.hitPoints + "¤" + currentPlayer.level);
                            break;

                        case "LOAD":
                            currentPlayer = Repo.Players.Find(x => x.name == messages[1]);
                            Console.WriteLine("Old Player: " + currentPlayer.name + ", " + currentPlayer.characterClass);
                            Repo.OnlinePlayers.Add(currentPlayer);
                            OnlinePlayers();
                            sw.WriteLine(currentPlayer.experiencePoints + "¤" + currentPlayer.gold + "¤" + currentPlayer.hitPoints + "¤" + currentPlayer.level);
                            break;

                        case "XP":
                            currentPlayer.experiencePoints += int.Parse(messages[1]);
                            Console.WriteLine("Giving " + currentPlayer.name + ": " + int.Parse(messages[1]) + " xp");
                            break;
                    }
                    
                } catch (Exception e) {
                    if (e.GetType() == typeof(IOException)) {
                        client.Close();
                        Repo.Clients.Remove(this);
                        Repo.OnlinePlayers.Remove(currentPlayer);
                        Console.WriteLine(IPEP + " - Terminated");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }
                }
            }
        }

    }
}
