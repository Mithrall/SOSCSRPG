using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ConsoleApplication1 {
    public class User {
        public string UserName;
        public List<Character> Characters { get; set; }
        public StreamWriter Sw;

        public User(StreamWriter sw) {
            Sw = sw;
            Characters = new List<Character>();
        }
    }
}
