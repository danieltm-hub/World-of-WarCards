using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class ModifyHealth : Power
    {
        private Expression Amount;

        public ModifyHealth(Expression amount, CodeLocation location) : base(location)
        {
            Amount = amount;
        }

        public override void Evaluate(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Amount.Evaluate(); //lo pusimos aqui dntro por si luego usa variables del enemigo
                player.ChangeHealth((double)Amount.Value);
                System.Console.WriteLine(Amount.Value);
            }
        }
    }
}