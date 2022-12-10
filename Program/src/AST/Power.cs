using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public abstract class Power : Node
    {
        public Power(CodeLocation location) : base(location) { }
        public override bool CheckSemantic(List<Error> errors) => true;
        public abstract void Evaluate(IEnumerable<Player> players);
    }
}