using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Engine.Models;

namespace WPFUI {
    public partial class Game:Window {

        Player CurrentPlayer;

        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;

        private bool running = true;

        private string _onlinePlayers;
        public string OnlinePlayers {
            get { return _onlinePlayers; }
            set {
                if (_onlinePlayers != value) {
                    _onlinePlayers = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Game() {
            InitializeComponent();

            CurrentPlayer = new Player();
            DataContext = CurrentPlayer;
        }

        //temp for testing
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            CurrentPlayer.ExperiencePoints += +10;
            sw.WriteLine("XP¤" + 10);
        }

        public bool Connect(string ip) {
            try {
                client = new TcpClient(ip, 12345);
                stream = client.GetStream();
                sr = new StreamReader(stream);
                sw = new StreamWriter(stream);
                sw.AutoFlush = true;
                return true;
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        public bool isNew(string name) {
            sw.WriteLine("ISNEW¤" + name);

            if (sr.ReadLine() != "EXISTS") {
                return true;
            } else {
                return false;
            }

        }

        public void StartNew(string name, string type) {
            CurrentPlayer.Name = name;
            CurrentPlayer.CharacterClass = type;

            sw.WriteLine("NEW¤" + name + "¤" + type);
            string[] stats = sr.ReadLine().Split('¤');
            CurrentPlayer.ExperiencePoints = int.Parse(stats[0]);
            CurrentPlayer.Gold = int.Parse(stats[1]);
            CurrentPlayer.HitPoints = int.Parse(stats[2]);
            CurrentPlayer.Level = int.Parse(stats[3]);

            Thread thread = new Thread(Loop);
            thread.Start();
        }

        public void StartExisting(string name, string type) {
            CurrentPlayer.Name = name;
            CurrentPlayer.CharacterClass = type;

            sw.WriteLine("LOAD¤" + name);
            string[] stats = sr.ReadLine().Split('¤');
            CurrentPlayer.ExperiencePoints = int.Parse(stats[0]);
            CurrentPlayer.Gold = int.Parse(stats[1]);
            CurrentPlayer.HitPoints = int.Parse(stats[2]);
            CurrentPlayer.Level = int.Parse(stats[3]);

            Thread thread = new Thread(Loop);
            thread.Start();
        }

        public void Loop() {
            while (running) {
                string message = sr.ReadLine();
                if (message != "") {
                    Dispatcher.Invoke(new UpdateText(UpdateTextBoxes), message);
                }
            }
        }

        public delegate void UpdateText(string message);
        public void UpdateTextBoxes(string message) {
            string[] messages = message.Split('¤');
            string code = messages[0];

            switch (code) {
                case "OnlinePlayers":
                    OnlinePlayers = "";
                    foreach (var player in messages) {
                        OnlinePlayers += player + "\n";
                    }
                    break;
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e) {
            sw.WriteLine("EXIT");
            running = false;
            Environment.Exit(1);
        }
    }
}
