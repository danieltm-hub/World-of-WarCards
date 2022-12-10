using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameProgram;

namespace AST
{

    public class Self : Objective
    {
        public Self(CodeLocation location) : base(location) { }

        public override List<Player> Evaluate()
        {
            return new List<Player>() { GameManager.CurrentPlayer };
        }
    }

}