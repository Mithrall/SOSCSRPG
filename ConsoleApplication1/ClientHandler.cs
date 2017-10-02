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

        internal IPEndPoint Ip() {
            return IPEP;
        }
        internal StreamWriter Writer() {
            return sw;
        }



        internal void Handle() {
            User currentUser = new User(sw);
            Character currentCharacter = new Character(currentUser);
            int currentEnemyNumber = 0;


            while (true) {
                Enemy currentEnemy = Repo.Enemies[currentEnemyNumber];
                try {
                    string message = sr.ReadLine();

                    //DISCONNECT CLIENT
                    if (!client.Connected || message == "EXIT") {
                        Repo.Clients.Remove(this);
                        Repo.OnlineCharacters.Remove(currentCharacter);
                        Server.OnlineCharacter();
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
                            Console.WriteLine("New Player: " + currentUser.UserName);
                            Repo.Users.Add(currentUser);
                            break;

                        case "LOADUSER":
                            currentUser = Repo.Users.Find(x => x.UserName == messages[1]);
                            Console.WriteLine("Old Player: " + currentUser.UserName);
                            int amount = currentUser.Characters.Count;
                            string reply = amount.ToString();
                            //NAME ¤ CLASS ¤ LEVEL      PER CHAR
                            foreach (var character in currentUser.Characters) {
                                reply += "¤" + character.Name + "¤" + character.CharacterClass + "¤" + character.Level;
                            }
                            sw.WriteLine(reply);
                            break;

                        case "NEWCHAR":
                            currentUser.Characters.Add(new Character(currentUser) {
                                Name = messages[1],
                                CharacterClass = messages[2]
                            });
                            break;

                        case "LOAD":
                            currentCharacter = currentUser.Characters.Find(x => x.Name == messages[1]);
                            Repo.OnlineCharacters.Add(currentCharacter);
                            sw.WriteLine(currentCharacter.CharacterClass + "¤" + currentCharacter.ExperiencePoints + "¤" + currentCharacter.XpNeeded + "¤" + currentCharacter.Gold + "¤" + currentCharacter.HitPoints + "¤" + currentCharacter.Level);
                            Server.OnlineCharacter();
                            
                            sw.WriteLine("NEWENEMY¤" + currentEnemy.Name + "¤" + currentEnemy.HitPoints + "¤" + currentEnemy.MaxHitPoints + "¤" + currentEnemy.Xp + "¤" + currentEnemy.Gold);

                            //List<string[]> test = Repo.Users[0].Characters.Select(x => new[] {
                            //    x.Name, x.CharacterClass, x.ExperiencePoints.ToString(), x.Gold.ToString(),
                            //    x.HitPoints.ToString(), /*x.Owner.ToString(),*/ x.XpNeeded.ToString()
                            //}).ToList();
                            //foreach (var chars in test) {
                            //    foreach (string _char in chars) {
                            //        Console.WriteLine(_char);
                            //    }
                            //}

                            break;

                        case "DELETECHAR":
                            var tempCharacter = currentUser.Characters.Find(x => x.Name == messages[1]);
                            currentUser.Characters.Remove(tempCharacter);
                            break;

                        //temp for testing
                        case "XP":
                            currentCharacter.ExperiencePoints += int.Parse(messages[1]);
                            sw.WriteLine("Xp¤" + currentCharacter.ExperiencePoints);
                            break;

                        case "ATTACK":
                            //OUT OF HP = DEAD
                            if (currentEnemy.HitPoints - int.Parse(messages[1]) <= 0) {
                                currentEnemy.HitPoints = currentEnemy.MaxHitPoints;

                                //new enemy (RANDOM TBD)
                                currentEnemyNumber = 0;


                                foreach (var tempClient in Repo.Clients) {
                                    tempClient.Writer().WriteLine("ENEMYDEAD¤");
                                }

                            } else { //NOT DEAD
                                currentEnemy.HitPoints -= int.Parse(messages[1]);
                                foreach (var tempClient in Repo.Clients) {
                                    tempClient.Writer().WriteLine("ENEMY¤" + currentEnemy.HitPoints);
                                }
                            }
                            break;

                        case "REWARD":
                            currentCharacter.ExperiencePoints += currentEnemy.Xp;
                            currentCharacter.Gold += currentEnemy.Gold;
                            sw.WriteLine("REWARD¤" + currentCharacter.ExperiencePoints + "¤" + currentCharacter.Gold);
                            break;

                            //ENDTEMP
                    }

                } catch (Exception e) {
                    if (e.GetType() == typeof(IOException)) {
                        client.Close();
                        Repo.Clients.Remove(this);
                        Repo.OnlineCharacters.Remove(currentCharacter);
                        Server.OnlineCharacter();
                        Console.WriteLine(IPEP + " - Terminated");
                        Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");
                        break;
                    }
                }
            }
        }
    }
}
