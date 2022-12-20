using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class NullObjective : Objective
    {
        public override string Keyword() => "null";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public NullObjective(List<Expression> parameters, CodeLocation location) : base(parameters, location) { }

        public override List<Player> Evaluate()
        {
            return new List<Player>();
        }
    }
}