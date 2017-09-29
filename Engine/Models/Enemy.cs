using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine.Models {
    public class Enemy : INotifyPropertyChanged {
        private string _name;
        private int _hitPoints;
        private int _maxHitPoints;
        private int _xp;
        private int _gold;

        public string Name {
            get { return _name; }
            set {
                _name = value;
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
        public int MaxHitPoints {
            get { return _maxHitPoints; }
            set {
                _maxHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Xp {
            get {
                return _xp;
            }
            set {
                _xp = value;
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
