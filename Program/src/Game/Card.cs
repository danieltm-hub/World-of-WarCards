using AST;

namespace GameProgram
{
    public class Card
    {
        string Name;
        Effector Effect;

        public Card(string name, Effector effect)
        {
            Name = name;
            Effect = effect;
        }
    }
}