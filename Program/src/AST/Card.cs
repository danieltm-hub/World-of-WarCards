using AST;


namespace GameProgram
{
    public class Card : Node, IClonable<Card>
    {
        public string Name { get; private set; }
        public List<Effect> Effects { get; private set; }
        public override string Description => GetDescription();
        private Expression EnergyCost;
        public double EnergyCostValue
        {
            get
            {
                EnergyCost.Evaluate();
                return Math.Max(0, (double)EnergyCost.Value);
            }
        }
        private Expression Coldown;
        public double ColdownValue
        {
            get
            {
                Coldown.Evaluate();
                return Math.Max(0, (double)Coldown.Value);
            }
        }

        public double CurrentColdown = 0;

        public Card(string name, List<Effect> effects, Expression coldown, Expression energyCost, CodeLocation location) : base(location)
        {
            Type = NodeType.Card;
            Name = name;
            Effects = effects;
            Location = location;
            Coldown = coldown;
            EnergyCost = energyCost;
        }

        public Card Clone()
        {
            return new Card(Name, Effects, Coldown, EnergyCost, Location);
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
                errors.Add(new Error(ErrorCode.Invalid, Location, $"Coldown"));
            }

            if (!EnergyCost.CheckSemantic(errors))
            {
                Type = NodeType.Error;
                errors.Add(new Error(ErrorCode.Invalid, Location, $"EnergyCost"));
            }

            return Type == NodeType.Card;
        }

        private string GetDescription()
        {
            string description = $"Card: {Name} \n";

            description += $"Coldown: {Coldown.Description} \n";
            description += $"EnergyCost: {EnergyCost.Description} \n";

            int count = 1;
            foreach (Effect effect in Effects)
            {
                description += $"Effect {count}:\n{effect.Description} \n";
                count += 1;
            }

            return description;
        }

        public void Play()
        {
            Effects.ForEach(effect => effect.Evaluate());
            CurrentColdown = ColdownValue;
        }

        public void ReduceColdown() => CurrentColdown = Math.Max(0, CurrentColdown - 1);

        public bool IsSameCard(Card card)
        {
            if (Name != card.Name) return false;
            if (EnergyCostValue != card.EnergyCostValue) return false;
            if (ColdownValue != card.ColdownValue) return false;

            return Effects.Count == card.Effects.Count;
        }
    }
}