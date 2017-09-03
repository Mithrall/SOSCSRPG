using Engine.Models;

namespace Engine.ViewModels {
    public class GameSession {
        public Player CurrentPlayer { get; set; }

        public GameSession(string name, string type) {
            
            CurrentPlayer = new Player();
            CurrentPlayer.Name = name;
            CurrentPlayer.CharacterClass = type;            
        }
    }
}
