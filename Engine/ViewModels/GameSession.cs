using System.ComponentModel;
using System.Runtime.CompilerServices;
using Engine.Models;

namespace Engine.ViewModels {
    public class GameSession : INotifyPropertyChanged {
        public Player CurrentPlayer { get; set; }

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

        public GameSession() {
            CurrentPlayer = new Player();
        }
    }
}