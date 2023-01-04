using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class ModifyEnergy : Power
    {
        public override string Keyword => "modifyenergy";
        public override string Description { get => GetDescription(); }
        public override List<NodeType> ExpectedTypes => new List<NodeType>() { NodeType.Number };
        public ModifyEnergy(List<Node> parameters, CodeLocation location) : base(parameters, location) { }

        public override void Evaluate(IEnumerable<Player> players)
        {
            Expression Amount = (Expression)Parameters[0];

            Amount.Evaluate();
            double damage = (double)Amount.Value;

            foreach (var player in players)
            {
                Amount.Evaluate();
                player.ChangeEnergy((double)Amount.Value);
            }
        }

        private string GetDescription()
        {
            return $"modifies energy by an amount of ({((Expression)Parameters[0]).Description})";
        }

    }
}
