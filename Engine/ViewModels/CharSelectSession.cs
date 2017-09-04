using System.ComponentModel;
using System.Runtime.CompilerServices;
using Engine.Models;

namespace Engine.ViewModels {
    public class CharSelectsSession : INotifyPropertyChanged {
        public User CurrentUser { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CharSelectsSession() {
            CurrentUser = new User();
        }
    }
}