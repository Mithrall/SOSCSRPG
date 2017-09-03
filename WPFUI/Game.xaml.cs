using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using Engine.ViewModels;

namespace WPFUI {
    public partial class Game:Window {
        private GameSession _gameSession;

        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;

        private bool running = true;

        public Game() {
            InitializeComponent();
            DataContext = _gameSession;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            _gameSession.CurrentPlayer.ExperiencePoints += +10;
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
            _gameSession = new GameSession(name, type);
            sw.WriteLine("NEW¤" + name + "¤" + type);
            string[] stats = sr.ReadLine().Split('¤');
            _gameSession.CurrentPlayer.ExperiencePoints = int.Parse(stats[0]);
            _gameSession.CurrentPlayer.Gold = int.Parse(stats[1]);
            _gameSession.CurrentPlayer.HitPoints = int.Parse(stats[2]);
            _gameSession.CurrentPlayer.Level = int.Parse(stats[3]);

            Thread thread = new Thread(Loop);
            thread.Start();
        }

        public void StartExisting(string name, string type) {
            _gameSession = new GameSession(name, type);
            sw.WriteLine("LOAD¤" + name);
            string[] stats = sr.ReadLine().Split('¤');
            _gameSession.CurrentPlayer.ExperiencePoints = int.Parse(stats[0]);
            _gameSession.CurrentPlayer.Gold = int.Parse(stats[1]);
            _gameSession.CurrentPlayer.HitPoints = int.Parse(stats[2]);
            _gameSession.CurrentPlayer.Level = int.Parse(stats[3]);

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
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e) {
            sw.WriteLine("EXIT");
            running = false;
            Environment.Exit(1);
        }
    }
}
