using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using Engine.ViewModels;

namespace WPFUI {
    public partial class Game : Window {
        private GameSession _gameSession;

        TcpClient client;
        NetworkStream stream;
        StreamReader sr;
        StreamWriter sw;

        private bool running = true;
        private string _name;

        public Game(string name, string type) {
            InitializeComponent();
            this._name = name;

            _gameSession = new GameSession(name, type);

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
            } catch (Exception) {
                return false;
            }
        }

        public void Start() {
            Thread thread = new Thread(Loop);
            thread.Start();
            sw.WriteLine("name¤" + _name);
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
    }
}
