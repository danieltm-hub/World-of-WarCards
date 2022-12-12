using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class NullObjective : Objective
    {
        public NullObjective(CodeLocation location) : base(location) { }

        public override List<Player> Evaluate()
        {
            return new List<Player>();
        }
    }
}