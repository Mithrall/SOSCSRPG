using System.Collections.Generic;
using System.ComponentModel;
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
            CurrentUser = new User {
                //TEMP -  until char creation page is done
                Characters = new List<Character> {new Character() {Name = "BOB", CharacterClass = "Mage", Level = 200}}
            };

            CurrentUser.Characters.Add(new Character() { Name = "TankBob", CharacterClass = "Warrior", Level = 30 });
            CurrentUser.Characters.Add(new Character() { Name = "FireBob", CharacterClass = "Mage", Level = 100 });
            CurrentUser.Characters.Add(new Character() { Name = "StealthBob", CharacterClass = "Rogue", Level = 50 });
            CurrentUser.Characters.Add(new Character() { Name = "NinjaBob", CharacterClass = "Rogue", Level = 80 });
        }
    }
}