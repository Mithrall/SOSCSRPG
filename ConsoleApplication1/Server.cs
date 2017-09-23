using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleApplication1 {
    class Server {
        public static string SavePath = "C:\\Users\\Kenneth\\Desktop\\Test.txt";
        static bool _exitSystem = false;

        static void Main(string[] args) {
            Server Run = new Server();
            Run.Run();
        }

        private void Run() {
            Console.WriteLine("Online");

            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            //LOAD USERS
            //FORMAT: ¤USERNAME username
            //name class xp gold hp xpneeded
            var text = File.ReadAllLines(SavePath);
            Console.WriteLine(text.Length);

            User currentUser = new User();

            foreach (var lines in text) {

                var line = lines.Split('¤');
                if (line[0] == "USERNAME") {
                    currentUser = new User {
                        UserName = line[1],
                        Characters = new List<Character>()
                    };
                    Repo.Users.Add(currentUser);
                } else {
                    currentUser.Characters.Add(new Character {
                        Name = line[0],
                        CharacterClass = line[1],
                        ExperiencePoints = int.Parse(line[2]),
                        Gold = int.Parse(line[3]),
                        HitPoints = int.Parse(line[4]),
                        XpNeeded = int.Parse(line[5]),
                        Level = int.Parse(line[6])
                    });
                }
            }

            TcpListener server = new TcpListener(IPAddress.Any, 12345);
            server.Start();

            while (!_exitSystem) {
                var client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ClientConnection, client);
            }
        }

        private void ClientConnection(object obj) { // Refactor into a ClientHandler Class
            var client = (TcpClient)obj;

            ClientHandler handler = new ClientHandler(client);

            Repo.Clients.Add(handler);

            Console.WriteLine("New Connection: " + handler.Ip());
            Console.WriteLine(Repo.Clients.Count + " Client(s) Connected");

            handler.Handle();
        }

        public static void OnlineCharacter() {
            string message = "OnlineCharacter";
            foreach (var character in Repo.OnlineCharacters) {
                message += "¤" + character.Name + ": Level " + character.Level + " " + character.CharacterClass;
            }

            foreach (var client in Repo.Clients) {
                client.Writer().WriteLine(message);
            }
        }

        //ON WINDOW CLOSE
        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        private static EventHandler _handler;


        private enum CtrlType {
            CtrlCEvent = 0,
            CtrlBreakEvent = 1,
            CtrlCloseEvent = 2,
            CtrlLogoffEvent = 5,
            CtrlShutdownEvent = 6
        }

        private static bool Handler(CtrlType sig) {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //SAVE ALL USERS + CHARS TO FILE
            //FORMAT: ¤USERNAME username
            //name class xp gold hp xpneeded level
            File.WriteAllText(SavePath, String.Empty);
            foreach (var user in Repo.Users) {
                var n = user.UserName;
                File.AppendAllText(SavePath, "USERNAME¤" + n + Environment.NewLine);
                foreach (var y in user.Characters) {
                    var s = y.Name + "¤" + y.CharacterClass + "¤" + y.ExperiencePoints + "¤" +
                            y.Gold + "¤" + y.HitPoints + "¤" + y.XpNeeded + "¤" + y.Level;
                    File.AppendAllText(SavePath, s + Environment.NewLine);
                }
            }

            Console.WriteLine("Cleanup complete");

            //allow main to run off
            _exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion
    }
}
