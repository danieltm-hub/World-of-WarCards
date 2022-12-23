using AST;


namespace GameProgram
{
    public class Card : Node
    {
        public string Name { get; private set; }
        public List<Effect> Effects { get; private set; }


        public Card(string name, List<Effect> effects, CodeLocation location) : base(location)
        {
            Type = NodeType.Card;
            Name = name;
            Effects = effects;
            Location = location;
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            foreach (var effect in Effects)
            {
                if (!effect.CheckSemantic(errors)) return false;
            }
            return true;
        }
    }
}