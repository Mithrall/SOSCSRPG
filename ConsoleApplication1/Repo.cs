using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    public static class Repo {
        public static List<ClientHandler> Clients = new List<ClientHandler>();


        private static void Broadcast() {
            Console.WriteLine("Broadcasting...");
            foreach (var client in Clients) {
               
            }
        }
    }
}
