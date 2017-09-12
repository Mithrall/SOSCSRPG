using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Xml.XPath;
using Engine.ViewModels;

namespace WPFUI {
    public partial class Game {

        StreamReader sr;
        StreamWriter sw;
        GameSession _gameSession;

        private bool _running = true;


        public Game(StreamReader sr, StreamWriter sw) {
            InitializeComponent();

            _gameSession = new GameSession();
            DataContext = _gameSession;

            this.sr = sr;
            this.sw = sw;
        }

        //temp for testing
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            sw.WriteLine("XP¤" + 10);
        }

        public void Start(string name) {
            _gameSession.CurrentCharacter.Name = name;

            sw.WriteLine("LOAD¤" + name);
            string[] stats = sr.ReadLine().Split('¤');
            _gameSession.CurrentCharacter.CharacterClass = stats[0];
            _gameSession.CurrentCharacter.ExperiencePoints = int.Parse(stats[1]);
            _gameSession.CurrentCharacter.XpNeeded = int.Parse(stats[2]);
            _gameSession.CurrentCharacter.Gold = int.Parse(stats[3]);
            _gameSession.CurrentCharacter.HitPoints = int.Parse(stats[4]);
            _gameSession.CurrentCharacter.Level = int.Parse(stats[5]);

            Thread thread = new Thread(Loop);
            thread.Start();
        }

        public void Loop() {
            while (_running) {
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
                case "OnlineCharacter":
                    _gameSession.OnlineCharacters = "";
                    for (int i = 1; i < messages.Length; i++) {
                        _gameSession.OnlineCharacters += messages[i] + "\n";
                    }
                    break;
                case "LevelUp":
                    // LEVEL xp xpneeded hp
                    _gameSession.CurrentCharacter.Level = int.Parse(messages[1]);
                    _gameSession.CurrentCharacter.ExperiencePoints = int.Parse(messages[2]);
                    _gameSession.CurrentCharacter.XpNeeded = int.Parse(messages[3]);
                    _gameSession.CurrentCharacter.HitPoints = int.Parse(messages[4]);
                    break;
                case "Xp":
                    _gameSession.CurrentCharacter.ExperiencePoints = int.Parse(messages[1]);
                    break;
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e) {
            sw.WriteLine("EXIT");
            _running = false;
            Environment.Exit(1);
        }
    }
}
