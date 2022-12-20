using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class ModifyHealth : Power
    {
        public override List<NodeType> ExpectedTypes => new List<NodeType>() {NodeType.Number};
        public ModifyHealth(List<Expression> parameters, CodeLocation location) : base(parameters,location) {}

        public override void Evaluate(IEnumerable<Player> players)
        {
            Expression Amount = Parameters[0];

            Amount.Evaluate();
            double damage = (double)Amount.Value;

            foreach (var player in players)
            {
                Amount.Evaluate(); //lo pusimos aqui dntro por si luego usa variables del enemigo
                player.ChangeHealth((double)Amount.Value);
            }
        }
       
    }
}