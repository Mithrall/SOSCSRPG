using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Engine.Models;

namespace Engine.ViewModels {
    public class CharSelectsSession:INotifyPropertyChanged {
        public User CurrentUser { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CharSelectsSession() {
            CurrentUser = new User();
        }

        public void CharSetup(List<Character> characters) {
            CurrentUser.Characters = Enumerable.Range(0, characters.Count).Select(x =>
                new Character {
                    Name = characters[x].Name,
                    CharacterClass = characters[x].CharacterClass,
                    Level = characters[x].Level
                }).ToList();
        }
    }
}