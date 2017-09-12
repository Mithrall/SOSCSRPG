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

            List<List<string[]>[]> temp = Repo.Users.Select(x => new List<string[]>[] {
                new List<string[]> { new string[] { "USERNAME¤", x.UserName }}, x.Characters.Select(y => new string[] {
                    y.Name, y.CharacterClass, y.ExperiencePoints.ToString(), y.Gold.ToString(), y.HitPoints.ToString(), y.XpNeeded.ToString()
                }).ToList() 
            }).ToList();


            foreach (var user in temp) {
                foreach (var chars in user) {
                    foreach (var strings in chars) {
                        foreach (var s in strings) {
                            File.AppendAllText(SavePath, s + "\n");
                        }
                    }
                }
            }

            Console.WriteLine("Cleanup complete");

            Thread.Sleep(1000);
            //allow main to run off
            _exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion
    }
}
