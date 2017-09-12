using System;

namespace ConsoleApplication1 {
    public class Character {

        private int _experiencePoints;
        private int _level;

        public User Owner;
        public string Name;
        public string CharacterClass;
        public int HitPoints;
        public int XpNeeded = 100;
        public int Gold;


        public int ExperiencePoints {
            get {
                return _experiencePoints;
            }
            set {
                _experiencePoints = value;
                if (_experiencePoints >= XpNeeded) {
                    ExperiencePoints = _experiencePoints - XpNeeded;
                    Level++;
                    //Chrasher  ? evt. lav timed istedet.
                    Server.OnlineCharacter();
                    // LEVEL xp xpneeded hp
                    Owner.Sw.WriteLine("LevelUp¤" + Level + "¤" + ExperiencePoints + "¤" + XpNeeded + "¤" + HitPoints);
                    Console.WriteLine(Name + " leveled up to: " + Level + ". Xp needed for next level: " + XpNeeded);
                }
            }
        }

        public int Level {
            get {
                return _level;
            }
            set {
                _level = value;
                HitPoints += Level * 30;
                XpNeeded = (int)(XpNeeded + _level * 0.1);
            }
        }

        public Character(User owner) {
            Owner = owner;

            //start stats
            HitPoints = 50;
            ExperiencePoints = 0;
            _level = 1;
            Gold = 0;
        }

        public Character() {
        }
    }
}