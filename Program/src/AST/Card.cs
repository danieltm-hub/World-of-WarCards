using AST;


namespace GameProgram
{
    public class Card : Node
    {
        public string Name { get; private set; }
        public List<Effect> Effects { get; private set; }
        public Expression Cooldown { get; private set; }
        public Expression EnergyCost { get; private set; }

        public Card(string name, List<Effect> effects, Expression cooldown, Expression energyCost, CodeLocation location) : base(location)
        {
            Type = NodeType.Card;
            Name = name;
            Effects = effects;
            Location = location;
            Cooldown = cooldown;
            EnergyCost = energyCost;
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            foreach (var effect in Effects)
            {
                if (!effect.CheckSemantic(errors))        
                {
                    Type = NodeType.Error;
                    errors.Add(new Error(ErrorCode.Invalid, Location, $"Effect"));
                }
            }

            if (!Cooldown.CheckSemantic(errors))
            {
                Type = NodeType.Error;
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Cooldown"));
            }

            if (!EnergyCost.CheckSemantic(errors))
            {
                Type = NodeType.Error;
                errors.Add(new Error(ErrorCode.Invalid, Location, $"EnergyCost"));
            }


            return Type == NodeType.Card;
        }
    }
}