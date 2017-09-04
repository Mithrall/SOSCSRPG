using System.ComponentModel;
using System.Runtime.CompilerServices;
using Engine.Models;

namespace Engine.ViewModels {
    public class GameSession : INotifyPropertyChanged {
        public Characters CurrentCharacter { get; set; }

        private string _onlineCharacters;
        public string OnlineCharacters {
            get { return _onlineCharacters; }
            set {
                if (_onlineCharacters != value) {
                    _onlineCharacters = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameSession() {
            CurrentCharacter = new Characters();
        }
    }
}