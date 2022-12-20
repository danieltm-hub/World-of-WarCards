using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class NullPower : Power
    {
        public override string Keyword() => "null";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public NullPower(List<Expression> parameters, CodeLocation location) : base(parameters, location) {}

        public override void Evaluate(IEnumerable<Player> players)
        {
            //do nothing
            return;
        }
    }
}