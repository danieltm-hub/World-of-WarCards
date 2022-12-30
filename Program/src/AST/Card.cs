using AST;


namespace GameProgram
{
    public class Card : Node, IClonable<Card>
    {
        public string Name { get; private set; }
        public List<Effect> Effects { get; private set; }
        public override string Description => GetDescription();
        public Expression Coldown { get; private set; }
        public Expression EnergyCost { get; private set; }
        public Card(string name, List<Effect> effects, Expression cooldown, Expression energyCost, CodeLocation location) : base(location)
        {
            Type = NodeType.Card;
            Name = name;
            Effects = effects;
            Location = location;
            Coldown = cooldown;
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

            if (!Coldown.CheckSemantic(errors))
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

        public void Play()
        {
            foreach (var effect in Effects)
            {
                effect.Evaluate();
            }
        }
        private string GetDescription()
        {
            string description = $"Card: {Name} \n";

            description += $"Cooldown: {Coldown.Description} \n";
            description += $"EnergyCost: {EnergyCost.Description} \n";

            int count = 1;
            foreach (Effect effect in Effects)
            {
                description += $"Effect{count}:\n{effect.Description} \n";
                count += 1;
            }

            return description;
        }

        public Card Clone()
        {
            return new Card(Name, Effects, Coldown, EnergyCost, Location);
        }
    }
}