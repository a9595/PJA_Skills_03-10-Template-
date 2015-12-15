namespace PJA_Skills_032.Model
{
    public class Skill
    {
        public string Name { get; set; }

        public Skill()
        {
            Name = "BSI";
        }

        public Skill(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}