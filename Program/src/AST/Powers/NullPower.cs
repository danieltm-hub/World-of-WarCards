using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class NullPower : Power
    {
        public NullPower(CodeLocation location) : base(location) {}

        public override void Evaluate(IEnumerable<Player> players)
        {
            //do nothing
            return;
        }
    }
}