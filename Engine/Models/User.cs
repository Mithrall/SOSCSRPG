using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine.Models {
    public class User:INotifyPropertyChanged {

        private List<Character> _characters;
        public List<Character> Characters {
            get { return _characters; }
            set {
                _characters = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName {
            get { return _userName; }
            set {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
