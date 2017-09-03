namespace ConsoleApplication1 {
    public class Player {

        public string name;
        public string characterClass;


        public int hitPoints;
        public int experiencePoints;
        public int level;
        public int gold;

        public Player(string name, string characterClass) {

            this.name = name;
            this.characterClass = characterClass;

            //start stats
            hitPoints = 50;
            experiencePoints = 0;
            level = 1;
            gold = 0;
        }
    }
}