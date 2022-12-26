using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{

    public class Self : Objective
    {
        public override string Keyword => "self";
        public override string Description => "current player";
        public override List<NodeType> ExpectedTypes => new List<NodeType>();
        public Self(List<Node> parameters, CodeLocation location) : base(parameters, location) { }
        public override List<Player> Evaluate()
        {
            return new List<Player>() { GameManager.CurrentGame.CurrentPlayer };
        }
    }

}