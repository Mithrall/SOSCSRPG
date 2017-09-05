using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine.Models {
    public class User:INotifyPropertyChanged {
        public List<Character> Characters { get; set; }
        public Character Character { get; set; }

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
