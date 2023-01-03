using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{
    public class Current : Entity
    {
        public override object Value { get; set; }
        public override string Keyword => "current";
        public Current(List<Node> parameters, CodeLocation location) : base(parameters, location)
        {
            Value = 0;
        }

        public override void Evaluate()
        {
            Value = GameManager.CurrentGame.CurrentPlayer;
        }

        public override bool CheckSemantic(List<Error> errors)
        {
            return true;
        }

        public override string Description => "Current";
    }
}