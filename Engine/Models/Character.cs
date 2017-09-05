using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine.Models {
    public class Character : INotifyPropertyChanged {
        private string _name;
        private string _characterClass;
        private int _hitPoints;
        private int _experiencePoints;
        private int _level;
        private int _gold;

        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string CharacterClass {
            get { return _characterClass; }
            set {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        
        public int HitPoints {
            get { return _hitPoints; }
            set {
                _hitPoints = value;
                OnPropertyChanged();
            }
        }

        public int ExperiencePoints {
            get {
                return _experiencePoints;
            }
            set {
                _experiencePoints = value;
                OnPropertyChanged();
            }
        }

        public int Level {
            get { return _level; }
            set {
                _level = value;
                OnPropertyChanged();
            }
        }

        public int Gold {
            get { return _gold; }
            set {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
