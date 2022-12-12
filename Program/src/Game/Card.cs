using AST;


namespace GameProgram
{
    public class Card
    {
        public string Name { get; private set; }
        public Effector Effect { get; private set; }
        public CodeLocation Location { get; private set; }

        public Card(string name, Effector effect, CodeLocation location)
        {
            Name = name;
            Effect = effect;
            Location = location;
        }
    }
}