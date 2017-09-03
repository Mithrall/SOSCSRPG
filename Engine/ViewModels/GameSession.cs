using Engine.Models;

namespace Engine.ViewModels {
    public class GameSession {
        public Player CurrentPlayer { get; set; }

        public GameSession(string name, string type) {

            
            CurrentPlayer = new Player();
            CurrentPlayer.Name = name;
            CurrentPlayer.CharacterClass = type;

            CurrentPlayer.HitPoints = 10;
            CurrentPlayer.Gold = 100;
            CurrentPlayer.ExperiencePoints = 0;
            CurrentPlayer.Level = 1;
            

        }
    }
}
