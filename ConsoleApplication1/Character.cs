namespace ConsoleApplication1 {
    public class Character {

        public string Name;
        public string CharacterClass;


        public int HitPoints;
        public int ExperiencePoints;
        public int Level;
        public int Gold;

        public Character() {

            //start stats
            HitPoints = 50;
            ExperiencePoints = 0;
            Level = 1;
            Gold = 0;
        }
    }
}