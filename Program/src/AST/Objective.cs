using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public abstract class Objective : Node
    {
        public override NodeType Type => NodeType.Objective;
        public Objective(CodeLocation location) : base(location) { }
        public override bool CheckSemantic(List<Error> errors) => true;
        public abstract List<Player> Evaluate();
    }

}